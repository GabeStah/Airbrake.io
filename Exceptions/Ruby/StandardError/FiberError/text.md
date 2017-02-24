# Ruby Exception Handling: FiberError

Our journey through our __Ruby Exception Handling__ series continues, as today we're taking a look at the `FiberError`.  A `FiberError` occurs when attempting to make a call to [`Fiber`](https://ruby-doc.org/core-2.4.0/Fiber.html) class methods, after the fiber has been `terminated`.

In this article, we'll examine the `FiberError` class in more detail, explore where it sits within Ruby's `Exception` class hierarchy, and also investigate how to handle `FiberErrors` in your own code.  Let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `FiberError` is a direct descendant of the [`EncodingError`](https://ruby-doc.org/core-2.3.3/EncodingError.html) class.

## When Should You Use It?

As is often the case, to understand what a `FiberError` means and how it functions, we must first understand another aspect of Ruby coding; in this case, the [`Fiber`](https://ruby-doc.org/core-2.4.0/Fiber.html) class.

Fundamentally, `fibers` (known as `continuations` in older Ruby versions) are light-weight threads, used for implementing concurrency logic that is _manually managed_ by the code (rather than the OS or virtual machine).  While not commonly used, the basic _purpose_ of `fibers` in Ruby is that a `Fiber` code block "remembers" its code state, and allows you to resume execution within that code block at any time. 

Most commonly, this functionality is best used for creating [`Enumerators`](http://ruby-doc.org/core-2.4.0/Enumerator.html), which are just classes that allow for iteration over a series of objects.  For example, imagine we want to create a `Counter` class, that allows us to enumerate a series of increasing integers.  We can create it using the [`enum_for`](https://ruby-doc.org/core-2.4.0/Object.html#method-i-enum_for) method within our own `Counter` class `iterator` method, by then specifying our series of iterations using the `yield` call:

```ruby
class Counter
    def iterator
        return enum_for(:iterator) if not block_given?
        yield 0
        yield 1
        yield 2
        yield 3
        yield 4
    end
end

counter = Counter.new.iterator
# Output object
puts counter
# Iterate a few times
puts counter.next
puts counter.next
puts counter.next
```

Calling `Counter.new.iterator` generates our new `Enumerator` object (which we can see by outputting the `counter` variable).  Then, as expected, we can use our `Enumerator` behavior by calling the automatic `.next` method on it.  For all `Enumerators` in Ruby, this calls the next `yield` to be iterated, so we output the next number in our list each time.  Here's the output we generated:

```
#<Enumerator:0x00000002d8bfb0>
0
1
2
```

The obvious problem here is that we hard-coded each of our `yield` statements in our original `Counter` class `iterator`.  This is a very poor practice, so we can clean it up by using a `Fiber` instead:

```ruby
class Counter
    def initialize
        @fiber = Fiber.new do
            count = 0
            # Infinite loop
            loop do
                # Yield the current count
                Fiber.yield count
                # Iterate
                count += 1
            end
        end
    end

    # Calls the next Fiber.yield enumeration
    def next
        @fiber.resume
    end
end

counter = Counter.new
# Output object
puts counter
# Iterate a few times
puts counter.next
puts counter.next
puts counter.next
```

While this is a few more lines of code, this is far cleaner and, most importantly, we are no longer forced to manually indicate every single `yield` iteration that we expect.  Instead, we've setup a `Fiber.new` block.  Within that block, we've created an infinite loop that uses `Fiber.yield` to yield the next iteration of our `count` variable, before incrementing the value by `+1`.

The magic of using `fibers` is that we can then call the `.resume` method of our `fiber` from _anywhere in code_, and Ruby will find the `Fiber.yield` statement then resume processing from there, before continuing execution where that `.resume` was called.  In other words, `fiber` allows us to execute code blocks "out of order", as Ruby will remember where the `fiber` yield statement is waiting for our next `.resume` method call.

The result is that we can call the `.next` method for our `counter` as many times as we want, from anywhere, and it will execute `@fiber.resume`, which then yields the current `count` value within our class, iterates the `count` by one, then exits that loop block again until the next time we call `.next`.  The output is as expected:

```
#<Counter:0x00000002b7f8c0>
0
1
2
```

Whew!  Now that we understand the basic purpose of `Fiber`, we can see how a `FiberError` might occur.  Simply put, `FiberErrors` occur when using `Fiber` methods as above, but when the `fiber` can no longer call the `.yield` method because it has been `terminated`.  `Termination` occurs when the final `Fiber.yield` call has been made within the `fiber` code block.  This causes any future attempts to call the `fiber's` `.resume` method to produce a `FiberError`, because that `fiber` is now dead or `terminated`.

In this example, we've modified the code above by **removing the infinite loop**.  This forces our `Fiber.yield` calls to be finite (in fact, we only have one now):

```ruby
require 'fiber'

class Counter
    def initialize
        @fiber = Fiber.new do
            count = 0
            # Yield the current count
            Fiber.yield count
            # Indicated termination
            "Fiber Terminated"
        end
    end

    # Calls the next Fiber.yield enumeration
    def next
        # Check if fiber is alive
        puts "Is fiber alive?: #{@fiber.alive? ? 'Yes' : 'No'}"
        @fiber.resume
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    counter = Counter.new
    # Output object
    puts counter
    # Iterate a few times
    puts counter.next
    puts counter.next
    puts counter.next
rescue FiberError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

There's a bit of extra code that we'll cover in a second, but let's execute this without the infinite loop surrounding our `Fiber.yield` call and see what happens:

```
#<Counter:0x00000002b1d580>
Is fiber alive?: Yes
0
Is fiber alive?: Yes
Fiber Terminated
Is fiber alive?: No
[EXPLICIT] FiberError: dead fiber called
```

Sure enough, a `FiberError` is produced, but let's examine the code a bit more to figure out what's going on.  One thing we did is add a call to the `Fiber.alive?` method within our `Counter.next` method.  This simply checks if the specified `fiber` is still active/can be yielded, and returns a boolean indicating its status.  With that in mind, let's see what's happening here.  As before, we call the `.next` method three times in total, so let's iterate over each to see why the `FiberError` is thrown.

**`counter.next` Call #1**

The first time we call `.next`, the output shows that `Yes`, `@fiber` is `alive`.  It then calls `.resume`, which jumps to the `Fiber.yield` statement inside our `fiber` code block, which outputs the initial `count` value of zero:

```
Is fiber alive?: Yes
0
```

**`counter.next` Call #2**

The second time we call `.next`, _prior to calling `@fiber.resume`_, our `fiber` is still `alive`.  However, once `Fiber.resume` is called, the `fiber` cannot find the next `Fiber.yield` statement within its block (because there isn't one), so it terminates itself.  **NOTE:** Upon termination, a `fiber` will return the value of the last executed expression within the `Fiber` code block.  This is why we added the "Fiber Terminated" line, so that the output from our _second_ `counter.next` call indicates that our `fiber` has, in fact, been `terminated`:

```
Is fiber alive?: Yes
Fiber Terminated
```

**`counter.next` Call #3**

For our third and final `counter.next` call, even prior to calling `.resume` on our `fiber`, we have confirmed that it is no longer `alive`.  As expected, the subsequent call to `.resume` is attempted on a `fiber` that has been terminated (is dead), so we get the expected `FiberError` in our output:

```
Is fiber alive?: No
[EXPLICIT] FiberError: dead fiber called
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A detailed look at the FiberError in Ruby, a direct descendant of the EncodingError class, along with a brief overview Ruby's `Fiber` class.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html