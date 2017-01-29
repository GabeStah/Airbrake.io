The steady journey through our extensive __Ruby Exception Handling__ series continues.  Today we'll be taking a closer look at the `UncaughtThrowError`.  `UncaughtThrowError` is a descendant class of the `ArgumentError`, which is, in turn, a descendant of the `StandardError` superclass we all know and love.  `UncaughtThrowError` is, as the name implies, raised when a `throw` is called but not properly caught by an appropriate `catch` block.

In this article we'll closely examine the `UncaughtThrowError` class, looking at where it sits within Ruby's `Exception` class hierarchy, and also how to deal with any `UncaughtThrowErrors` you may experience during your own coding forays.  Let's get this party started!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`StandardError`] is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.
- [`ArgumentError`] is a direct descendant of the [`StandardError`] class.
- `UncaughtThrowError` is a direct descendant of the [`ArgumentError`] class.

# When Should You Use It?

To better understand what causes an `UncaughtThrowError` to be raised, it's important to understand the basics of the `throw-catch` mechanisms within Ruby.

As we've seen many times throughout our __Ruby Exception Handling__ series, Ruby's most basic error handling capabilities come from the `begin-rescue` block.  Any code executed inside the `begin` block, and which also happens to raise an exception, will instead continue execution within the corresponding `rescue` block matching the particular exception type that was raised.

A similar kind of execution rerouting can also be __explicitly__ performed using the `throw-catch` block.  When execution encounters a `throw` method, it immediately finds the location of a corresponding `catch` block to jump execution to, as indicated by the first passed argument, known as the `tag`.

For example, here we're performing a simple process of iterating our `count` variable every half-second, outputting the `count` along with the `timestamp`.  Once the `count` reaches our limit of `9 or greater`, we execute a `throw` block using the `:alert` tag:

```ruby
result = catch(:alert) do
    count = 0
    while true
        count = count + 1
        puts "Count #{count} at #{Time.now.getutc}"
        sleep 0.5
        throw :alert, count if count >= 9
    end
end
puts "Throw has been caught at count #{result}!"
```

Even though the `while true end` block should cause an infinite loop, we're taking advantage of the `throw` method call using the tag `:alert` and passing the value of `count` along with it.  By then assigning the resulting value of our `catch` block for the corresponding `:alert` tag at the top to the `result` variable, we can execute our infinite loop until `count` meets our limit of `9` iterations.  We then `throw` to `catch(:alert)`, which then passes that value of `count` into the `result` variable after exiting out of the `catch` code block.

The output, as expected, shows nine iterations then the `throw` is `caught`:

```
Count 1 at 2017-01-29 07:01:55 UTC
Count 2 at 2017-01-29 07:01:55 UTC
Count 3 at 2017-01-29 07:01:56 UTC
Count 4 at 2017-01-29 07:01:56 UTC
Count 5 at 2017-01-29 07:01:57 UTC
Count 6 at 2017-01-29 07:01:57 UTC
Count 7 at 2017-01-29 07:01:58 UTC
Count 8 at 2017-01-29 07:01:58 UTC
Count 9 at 2017-01-29 07:01:59 UTC
Throw has been caught at count 9!
```

With a basic understanding of how `throw-catch` blocks work together, it's not much of a leap to extrapolate from there how the `UncaughtThrowError` might come about.  By simply failing to properly `catch` the `:alert` tag we threw, an `UncaughtThrowError` is born.  

In this example, all we've done to our previous code is change the expected tag of our `catch` block from `:alert` to a mismatched tag of `:error`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    count = catch (:error) do
        count = 0
        while true
            count = count + 1
            puts "Count #{count} at #{Time.now.getutc}"
            sleep 0.5
            throw :alert, count if count >= 9
        end
    end
    puts "Throw has been caught at count #{count}!"
rescue UncaughtThrowError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

We've also added some niceties to help us properly `rescue` and display any exceptions we get, but the only fundamental difference is the aforementioned tag change to `:error` in our `catch` block.  As expected, once our `count` reaches our limit of `9` and the `throw` executes, it fails to find a corresponding `catch` block looking for the `:alert` tag, and thus produces an `UncaughtThrowError`:

```
Count 1 at 2017-01-29 07:09:20 UTC
...
Count 9 at 2017-01-29 07:09:24 UTC
[EXPLICIT] UncaughtThrowError: uncaught throw :alert
code.rb:37:in `throw'
code.rb:37:in `block in <main>'
code.rb:31:in `catch'
code.rb:31:in `<main>'
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html
[`ArgumentError`]: https://ruby-doc.org/core-2.3.3/ArgumentError.html

---

__META DESCRIPTION__

A deep-dive analysis of Ruby's UncaughtThrowError, including a brief exploration of throw and catch mechanisms.

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
