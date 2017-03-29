# Ruby Exception Handling: StopIteration

Moving right along through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll be examining the `StopIteration` error.  `StopIteration` is unique in the realm of Ruby exceptions, since it isn't generated at run-time by something going haywire, but is instead manually `raised` by the developer when there's a need to halt an active iteration.

In this article we'll take a closer look at what a `StopIteration` "error" is, seeing where it fits in the Ruby `Exception` class hierarchy, and diving into how to properly raise `StopIteration` errors yourself.  Let's get this party started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`IndexError`](https://ruby-doc.org/core-2.4.0/IndexError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class, and is a superclass to a few descendants of its own.
- `StopIteration` is a direct descendant of the [`IndexError`](https://ruby-doc.org/core-2.4.0/IndexError.html).

## When Should You Use It?

What is most interesting about `StopIteration` is that it isn't a traditional error, in that it won't be raised automatically by the Ruby runtime running when something goes wrong.  In fact, the name hints at this difference to other errors.  As you may recall, every other error we've covered in [this series](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) has contained the `Error` suffix as part of the name (with the exception of `SignalException`, pun intended).  Yet, even though `StopIteration` _is_ a descendant of the `Exception` class in Ruby, it is _only raised_ when you explicitly raise it in your code.

To see __why__ we'd might want to explicitly raise a `StopIteration` error intentionally, let's consider an example scenario.  Imagine we need to perform a `loop` that enumerates through a series of values.  This is a very common scenario in all languages, but here's what it might look like in Ruby:

```ruby
for index in 0..9 do
    puts index
end
```

As expected, this prints out all values from zero through nine:

```
0
1
2
3
4
5
6
7
8
9
```

Now, what if we want to halt our `loop` execution before reaching the end of our enumeration, based on a particular value?  We have a few options, but the most common technique is to use the `break` keyword, which terminates the current `loop`.  Here we're halting our `loop` when `index` reaches a value of `5`:

```ruby
for index in 0..9 do
    puts index
    # Break from enumeration if index is 5 or greater.
    break if index >= 5
end
```

Now we only print out values from zero to five:

```
0
1
2
3
4
5
```

However, we can alternatively `raise` a `StopIteration` error, instead of using `break`.  Since we're `raising` an error, we've also surrounded our code with some helpers, and the standard `begin-rescue` block:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    for index in 0..9 do
        puts index
        # Raise StopIteration if index is 5 or greater.
        raise StopIteration if index >= 5
    end
rescue StopIteration => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

This performs the same as the `break` example, but since we also `raise` an error, we spit that out as well:

```
0
1
2
3
4
5
[EXPLICIT] StopIteration: StopIteration
```

That's all well and good, but the question remains: _Why would you ever want to use `StopIteration` instead of `break`?_  Answer: When you also want to control the flow of execution for code _outside_ of your loop.

For example, let's take the same simple loop as above, but immediately after our loop we're going to output a simple message of `"Loop complete."`.  First let's try it with `break`:

```ruby
for index in 0..9 do
    puts index
    # Break from enumeration if index is 5 or greater.
    break if index >= 5
end
puts "Loop complete."
```

As expected, we output zero through five, then our completion message:

```
0
1
2
3
4
5
Loop complete.
```

But notice what happens when we add the same post-loop `puts` statement in our `StopIteration` example:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    for index in 0..9 do
        puts index
        # Raise StopIteration if index is 5 or greater.
        raise StopIteration if index >= 5
    end
    puts "Loop complete."
rescue StopIteration => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Outputs:

```
0
1
2
3
4
5
[EXPLICIT] StopIteration: StopIteration
```

We entirely skip over the `puts "Loop complete."` line.  This is the fundamental benefit (or detriment, depending on the situation) of raising a `StopIteration` error.  It allows your code to completely halt all further execution from within an internal loop, until _after_ the `StopIteration` error has been `rescued` in some way.

This practice is largely contested in the Ruby development community, as some developers are strongly against handling `control flow` (i.e. how execution traverses through your code) using exceptions.  Yet, all projects and needs are different, so experiment with the different styles of exiting out of loops to see which best fits with your own requirements.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close look at the StopIteration "error" in Ruby, including a brief exploration of different practices for prematurely exiting loops.