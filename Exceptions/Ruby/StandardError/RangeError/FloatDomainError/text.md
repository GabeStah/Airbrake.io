# Ruby Exception Handling: FloatDomainError

Today our journey through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series takes us to our next stop, the `FloatDomainError`.  A `FloatDomainError` is most commonly raised when trying to convert special kinds of `Float` values to other numeric classes which don't support those particular special values.

In this article we'll explore the `FloatDomainError` in more detail, including where it sits inside the Ruby `Exception` class hierarchy, as well as giving a few simple code examples to illustrate how `FloatDomainErrors` might pop up.  Let's get started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`RangeError`](https://ruby-doc.org/core-2.4.0/RangeError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class, and is a superclass with one descendant of its own.
- `FloatDomainError` is the direct descendant of [`RangeError`](https://ruby-doc.org/core-2.4.0/RangeError.html).

## When Should You Use It?

As briefly mentioned in the introduction, a `FloatDomainError` is raised when an attempt is made to convert certain `Float` values to other numeric classes which don't have a way to represent those values.  To understand what this means, let's briefly look at how Ruby handles `Floats` and general numeric conversions.

We'll start with a simple `#to_integer` method that we've created:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def to_integer(value)
    begin
        # Convert value to integer.
        i = value.to_i
        puts "#{value} converted to #{i}"
    rescue FloatDomainError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end
```

This little method accepts a single `value` parameter and then attempts to convert it to an `integer` using the `#to_i` method.  If successful it outputs the conversion that took place, otherwise it raises an error.  It's worth noting that we could obviously use the built-in `#to_i` method for most value types we're working with (`Float`, `Fixnum`, etc), but here we want a bit of extra fluff around the conversion, such as some output and the exception handling block.

To use `#to_integer` we just pass in some arguments, like so:

```ruby
# Convert 3.75 to integer.
to_integer(3.75)
# Convert 12345.67890 to integer.
to_integer(12345.67890)
# Convert Infinity to integer.
to_integer(Float::INFINITY)
```

The output shows that the first two examples work just fine:

```
3.75 converted to 3
12345.6789 converted to 12345
```

These basic instances of the `Float` class can be easily converted to `Integers` by dropping the fractional part of the number. However, our third example, which attempts to convert the special value `Infinity` to an `Integer`, fails and raises a `FloatDomainError`:

```
[EXPLICIT] FloatDomainError: Infinity
```

As it happens, this is because the special value of `Infinity` in Ruby is considered a `Float` class type.  To see this in action, we can launch an `irb` console and test it out:

```
irb(main):001:0> 1 / 0
ZeroDivisionError: divided by 0
        from (irb):6:in `/'
        from (irb):6
        from G:/dev/programs/Ruby23-x64/bin/irb.cmd:19:in `<main>'

irb(main):002:0> 1.0 / 0
=> Infinity

irb(main):003:0> infinity = 1.0 / 0
=> Infinity

irb(main):004:0> infinity.class
=> Float

irb(main):005:0> Float::INFINITY == infinity
=> true

irb(main):006:0> Float::INFINITY.class
=> Float
```

Here we've shown that we can generate an instance of `Infinity` by dividing a `Float` value by zero, so we assign that to the `infinity` variable, then check it's `#class` to show it's still considered a `Float`.  Ruby also provides access to `Infinity` through the `Float::INFINITY` constant, which we also show is equivalent to our instance example.

Since we see that `Infinity` is considered a `Float`, we can then understand why attempting to convert that to an `Integer` in our `#to_integer` method fails.  Ruby tries to execute our request, but since `Infinity` is a special value -- one that is represented by a `Float` type behind the scenes -- it cannot be made into an `Integer` as requested.

We can see something similar with other values and numeric class types also, such as `Rationals`.  Here we have the `#to_rational` method that performs a similar role to the `#to_integer` method, but converting to a `Rational` number instead of an `Integer`:

```ruby
def to_rational(value)
    begin
        # Convert value to rational number.
        r = value.to_r
        puts "#{value} converted to #{r}"
    rescue FloatDomainError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

Let's pass some values to `#to_rational` and see what we get:

```ruby
# Convert 3.75 to rational.
to_rational(3.75)
# Convert 12345.67890 to rational.
to_rational(12345.67890)
# Convert NaN to rational.
to_rational(Float::NAN)
```

As expected, the first two numbers are converted without much trouble.  However, this time we try to convert the special value `NaN` (not a number) to a `Rational`, which also raises a `FloatDomainError`:

```
3.75 converted to 15/4
12345.6789 converted to 6787108751669409/549755813888
[EXPLICIT] FloatDomainError: NaN
```

Our issue stems from the same problem as before.  Just like `Infinity`, the special value of `NaN` is considered a `Float` class type behind the scenes.  Again, we can see this in action with a few statements in an `irb` console:

```
irb(main):001:0> 0 / 0
ZeroDivisionError: divided by 0
        from (irb):1:in `/'
        from (irb):1
        from G:/dev/programs/Ruby23-x64/bin/irb.cmd:19:in `<main>'

irb(main):002:0> 0.0 / 0.0
=> NaN

irb(main):003:0> nan = 0.0 / 0.0
=> NaN

irb(main):004:0> nan.class
=> Float

irb(main):005:0> Float::NAN
=> NaN

irb(main):006:0> Float::NAN.class
=> Float
```

Here we see that we cannot divide by zero using `Integer` representations, but if we use `Floats` we can create an instance of `NaN`, indicating that the resulting value is not actually a number.  Furthermore, we see that `nan#class` is a `Float` type, and that Ruby provides another constant that represents the `NaN` value: `Float::NAN`.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A deep dive into the FloatDomainError in Ruby, including functional code examples and a brief examination of the Infinity and NaN special values in Ruby.