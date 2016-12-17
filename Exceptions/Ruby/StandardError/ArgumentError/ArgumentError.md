As our journey through the __Ruby Exception Handling__ series continues, today we'll be taking a closer look at the `ArgumentError`.  `ArgumentError` is a descendant class of the `StandardError` superclass, and is typically raised when arguments passed to a method are incorrect, unexpected, or invalid in some way.  

In this article we'll dive deeper into our examination of the `ArgumentError` class, peering at where it sits within Ruby's `Exception` class hierarchy and how to handle any `ArgumentErrors` you may encounter in your own coding adventures.  Let the exploration begin!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`StandardError`] is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.
- `ArgumentError` is a direct descendant of the [`StandardError`] class, and is also a small superclass of its own.

To get the most out of your own applications and to fully manage any and all [`Ruby Exceptions`], check out the blazing fast [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

# When Should You Use It?

While `ArgumentError` is intended to appear in the limited circumstance for when passed arguments to a method are incorrect in some fashion, the sheer number of specific instances in which this can occur is quite massive.  Whether it be passing too many arguments to a method that expects fewer total, passing an incorrect argument data type, or even passing the correct data type but with an invalid value, `ArgumentError` covers the vast majority of these situations.

Therefore, to use it properly, it's simplest to just take a look at a few basic examples so you can extrapolate on this knowledge for your own exception handling needs.

In this example below, we're creating a new array called `data` and giving it the series of values from `1` to `10`.  Next, we want to grab a random sample of these array values and output them, so we're using the [`.sample()`] method on our array, but we've accidentally added two arguments specifying how many elements from the array we want to select:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a random sample from array, with too many arguments
    puts data.sample(3, 5)
rescue ArgumentError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Ruby doesn't care for this, since the [`.sample()`] method only expects one numeric argument, and thus we produce our expected `ArgumentError` informing us of this blunder:

```
[EXPLICIT] ArgumentError: wrong number of arguments (2 for 1)
code.rb:11:in `sample'
code.rb:11:in `<main>'
```

Along the same vein, we can also raise an `ArgumentError` by passing an invalid argument to `.sample()`, such as a negative number in this case.

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a negative number of random elements
    puts data.sample(-5)
rescue ArgumentError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Since collecting a negative number of elements makes no sense, Ruby throws another `ArgumentError` and informs us of our faux pas:

```
[EXPLICIT] ArgumentError: negative sample number
code.rb:28:in `sample'
code.rb:28:in `<main>'
```

As with any exception class that extends from `StandardError`, it's important to note that we can also catch this error even if we neglect to explicitly `rescue` the `ArgumentError` class, and instead fall back to the default `rescuing` of `StandardError`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a negative number of random elements
    puts data.sample(-5)
rescue => e # Rescuing the default `StandardError` exception class
    print_exception(e, false)
end
```

Sure enough, while we didn't explicitly `rescue` our `ArgumentError` class, the default `StandardError` did the rescuing for us, and then drilled down to the matching `ArgumentError` that applied, just as before:

```
[INEXPLICIT] ArgumentError: negative sample number
code.rb:45:in `sample'
code.rb:45:in `<main>'
```

[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html
[`Ruby Exceptions`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html

[`.sample()`]: https://ruby-doc.org/core-2.2.0/Array.html#method-i-sample

--------------------------------------------------------------------------------

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
