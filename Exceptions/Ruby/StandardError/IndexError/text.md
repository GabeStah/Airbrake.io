# Ruby Exception Handling: IndexError

Today we'll be continuing our journey through our __Ruby Exception Handling__ series by exploring the wonderful world of the Ruby `IndexError`.  The `IndexError` is raised when there's an attempted call to an element within an array, where the given index is out of bounds or invalid in some way.

Throughout today's article we'll examine exactly what the `IndexError` means, see where it fits within the Ruby `Exception` class hierarchy, and detail how to properly handle any `IndexErrors` you might experience in your own coding endeavors.  Let's get going!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `IndexError` is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) class, and is a superclass to a few descendants of its own.

## When Should You Use It?

The `IndexError` is directly linked to the use of `Arrays` in Ruby, so let's briefly cover arrays and how they work.

We can create an array by calling the `Array.new` method: `Array.new(3)` would create an array with three (`nil`) elements.  We can also create an array inline by specifying the values as part of the array assignment: `names = ["Alice", "Bob", "Christine", "Dan"]`

Once an array is created, there are [numerous](https://docs.ruby-lang.org/en/2.0.0/Array.html) methods to manipulate it.  If we want to fetch a value from the array, we can use the square bracket syntax and include the index we are trying to get:

```ruby
index = 3
names = ["Alice", "Bob", "Christine", "Dan"]
puts names[index] #=> "Dan"
```

We can also use something like the [`Array.fetch`](https://docs.ruby-lang.org/en/2.0.0/Array.html#method-i-fetch) method, which expects the first argument provided to be the `index` of the element to fetch:

```ruby
index = 3
names = ["Alice", "Bob", "Christine", "Dan"]
puts names.fetch(index) #=> "Dan"
```

As with most programming languages, arrays use zero-based numbering, so the first element is at index `0`, the second element at index `1`, and so forth.  Therefore, in both examples above, when we pass the `index` value of `3`, it returns the **fourth** element in our array.

With only four elements total, let's see what happens if we try to retrieve the `fifth` index with the direct square bracket syntax:

```ruby
index = 5
names = ["Alice", "Bob", "Christine", "Dan"]
puts names[index] #=> nil
```

Ruby recognizes that no value exists, and thus returns `nil`.  However, what if we pass the same syntax with the `Array.fetch` method:

```ruby
# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    index = 5
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index)
rescue IndexError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Perhaps unsurprisingly, this raises an explicit `IndexError`, indicating that the `index` of `5` is outside the bounds of our array:

```
[EXPLICIT] IndexError: index 5 outside of array bounds: -4...4
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IndexError/code.rb:10:in `fetch'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IndexError/code.rb:10:in `<main>'
```

There are a couple ways to solve this and prevent `IndexError` from occurring.  The first, and most obvious, is to improve our code logic so that we don't try to retrieve any indices that don't exist with the `Array.fetch` method.

The second option is to pass a **second** argument to the `Array.fetch` method, which acts as a `default` value if the provided `index` is out of bounds:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    index = 5
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index, "No name found.")
rescue IndexError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Now when we execute our code, rather than producing an `IndexError`, the `Array.fetch` method recognizes the second argument of `"No name found."` and returns that value as our output instead:

```
No name found.
```

A third use for `Array.fetch`, which is somewhat interesting if not directly related to the `IndexError`, is that we can attach a code `block` after the `Array.fetch` call, if we want the block to execute _only when_ the `index` value is invalid:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    index = 5
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index) { |i|
        "No name found at index: #{i}."
    }
rescue IndexError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Here's our output:

```
No name found at index: 5.
```

This functionality can be rather useful as a predetermined escape sequence for when we aren't sure of a provided `index` will be valid, so we can perform other actions if necessary.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A closer look at the IndexError in Ruby, a direct descendant of the StandardError class, along with a brief examination of working with Ruby Arrays.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html