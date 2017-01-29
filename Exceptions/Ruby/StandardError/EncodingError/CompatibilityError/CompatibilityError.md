As we continue to make our way through the expansive __Ruby Exception Handling__ series, today we'll be taking a closer look at the `CompatibilityError`.  `CompatibilityErrors` appear when dealing with encoding in Ruby, and specifically when attempts are made to compare two encoded values which have incompatible encoding types.

Throughout this article we'll delve into the details of the `CompatibilityError` class, examining where it sits within Ruby's `Exception` class hierarchy, as well as how to handle any `CompatibilityErrors` that you may deal with personally.  Let's get going!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`StandardError`] is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.
- [`EncodingError`] is a direct descendant of the [`StandardError`] class, and is also a superclass with a handful of descendants of its own.
- `CompatibilityError` is a direct descendant of the [`EncodingError`] class.

# When Should You Use It?

Character encoding within Ruby (or even in general development) can be a bit confusing to say the least.  Often it's a bit of a "black box" affair, where methods are called to perform encoding and conversions without any clear understanding of how Ruby is handling things.

Thankfully, as newer Ruby versions have been released and steady improvements have been included with each, the challenges of working with encoding have certainly lessened, but headaches still abound when a small snippet of code refuses to function because one tiny little character isn't compatible with some encoding check deep down in your codebase.

For this very reason, Ruby includes a handful of built-in encoding exception classes, the first of which is the `CompatibilityError` we're examining today.

As mentioned in the introduction, the `CompatibilityError` will appear anytime two strings with _incompatible encodings_ are compared in some way.  To understand what this means, let's first look at a working example:

```ruby
# UTF-8 instance
utf8 = "hello"
# Convert to ASCII
forced = "hello".encode('ASCII')
# Compare the two
puts utf8.include? forced
```

Here we're creating a new string with the text `"hello"`.  By default, my character encoding in Ruby is `UTF-8`, so all newly generated strings are automatically assigned `UFT-8` encoding, hence our variable name of `utf8`.

We're also creating a second instance of our `"hello"` string, encoding it to `ASCII` using the `encode()` method, and assigning the value to our `forced` variable.

Finally, we compare the two by checking to see if one value (`utf8`) `includes` the other value (`forced`), and outputting the `true/false` result:

```
true
```

As expected, there are no problems, and even though the encodings of `UTF-8` and `ASCII` for both strings differ, the byte-representations of the characters that make up the simple string of `"hello"` are equivalent in both encodings, thus the comparison works fine.  That is, the `ASCII` decimal value of the lowercase letter `h` is `104`, and the `UTF-8/Unicode` decimal value for `h` is also `104`, so comparing these two simple strings is no problem.

However, what happens if we change our string to use characters which are not represented in the limited `ASCII` character set?  For example, let's try the word `résumé`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # UTF-8 instance
    utf8 = "résumé"
    # Convert to ASCII
    forced = "résumé".force_encoding('ASCII')
    # Output the encoding of forced
    puts forced.encoding
    # Compare the two
    puts utf8.include? forced
rescue Encoding::CompatibilityError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

We've added some helper functions to handle potential exceptions.  We've also changed our call to the `encode` method to `force_encoding`, because a call to `encode` fails since our string contains values not found in the `ASCII` encoding set.  Lastly, we're also outputting the `encoding` value of our `forced` variable, just to double-check that the forced encoding actually took.

What's important to understand here is that calling the `force_encoding()` method doesn't actually change the characters (and thus the underlying string) in anyway; it's simply a hack of sorts to inform Ruby of what we consider to be the "correct" encoding for this string.  Ruby will then use that encoding type value for later execution, as we'll see right now.

The end result of all this is that we're asking Ruby to compare our two strings (which are both identical characters of `"résumé"`) via the `include?()` method, but one of them has been forced to use `ASCII` encoding.  Since `ASCII` has no representation for the handful of special characters we are using, this comparison fails and raises the expected `CompatibilityError`:

```
US-ASCII
[EXPLICIT] Encoding::CompatibilityError: incompatible character encodings: UTF-8 and US-ASCII
code.rb:34:in `include?'
code.rb:34:in `<main>'
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html
[`EncodingError`]: https://ruby-doc.org/core-2.3.3/EncodingError.html

---

__META DESCRIPTION__

A close examination of Ruby's CompatibilityError, a descendant of the EncodingError class.

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
