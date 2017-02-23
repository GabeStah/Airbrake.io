As we continue down the well-traveled road of __Ruby Exception Handling__ series, today we're taking a look at the `UndefinedConversionError`.  An `UndefinedConversionError` is raised when using `Encoding` or `String` methods, and the transcoding process is unable to convert a character from one encoding to another.

In this article, we'll examine the `UndefinedConversionError` class, explores where it rests within Ruby's `Exception` class hierarchy, and also dive into how to deal with any `UndefinedConversionError` you might come across in your own projects.  Let's get going!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`EncodingError`](https://ruby-doc.org/core-2.3.3/EncodingError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) class, and is also a superclass with a handful of descendants of its own.
- `UndefinedConversionError` is a direct descendant of the [`EncodingError`](https://ruby-doc.org/core-2.3.3/EncodingError.html) class.

# When Should You Use It?

As we saw in our `ConverterNotFoundError` article, Ruby's [`Encoding`](https://ruby-doc.org/core-2.4.0/Encoding.html) namespace defines all the encoding and transcoding functionality within Ruby, which allows simply conversion from one encoding to another.  However, in the case of the `UndefinedConversionError`, an attempt to convert a character from one encoding to the other fails, typically because that character is not compatible with the target encoding in question.  

For example, here we're attempting to take a single character (the trademark symbol, `™`) and to convert it from `UTF-8` encoding to `IBM437`:

```ruby
# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Attempting to convert trademark symbol
    puts "\u2122".encode("IBM437")
rescue Encoding::UndefinedConversionError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

We also have a helper function, but the meat of our code is that single `puts "\u2122"...` line.

As it happens, this encoding process throws an `UndefinedConversionError`, indicating that the issue is we're attempting to convert the `U+2122` character (the unicode of the trademark symbol) into `IBM437` encoding, which doesn't contain that particular character:

```
[EXPLICIT] Encoding::UndefinedConversionError: U+2122 from UTF-8 to IBM437
G:/dev/work/Airbrake.io/Exceptions/Ruby/StandardError/EncodingError/UndefinedConversionError/code.rb:9:in `encode'
G:/dev/work/Airbrake.io/Exceptions/Ruby/StandardError/EncodingError/UndefinedConversionError/code.rb:9:in `<main>'
```

Since our character simply doesn't exist in the target encoding, in this case there's no solution for what we want to accomplish, other than using a different target encoding.  If we change our target encoding to `UTF-16`, which _does_ support the trademark symbol, we should get our expected output:

```ruby
# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Attempting to convert trademark symbol
    puts "\u2122".encode("UTF-16")
rescue Encoding::UndefinedConversionError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Sure enough, the `™` symbol is spit out just fine, without raising any errors.

```
™
```

While we cannot _force_ transcoding of characters using encodings which don't contain appropriate characters, one method for dealing with this error preemptively is to pass extra arguments to the `encode()` method.  Specifically, we can use the `:replace` keyword argument for the `invalid:` and `undef:` options inside our `encode()` method call:

```ruby
# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Attempting to convert trademark symbol
    puts "Trademark Symbol: \u2122".encode("IBM437", invalid: :replace, undef: :replace)
rescue Encoding::UndefinedConversionError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

With those options set, when the [`encode()`](https://ruby-doc.org/core-2.2.0/String.html#method-i-encode) method encounters a character it cannot transcode, instead of throwing an error, those characters are _replaced_ by placeholder characters.  By default, the replacement is a question mark (`?`), or in unicode converters it's `�`.

Therefore, our expectation from the above example, even when trying to convert the trademark symbol into `IBM437` encoding, we get a replacement character as our output, but no errors are thrown:

```
Trademark Symbol: ?
```

As developers and database administrators can attest, it's somewhat common to see random `�` characters popping up in database text fields from time to time.  In most cases, it's due to these sorts of conversion issues, forcing the developer or database engine to gracefully skip over a potential error like `UndefinedConversionError` by converting unknown characters to `�` instead.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A detailed look at the UndefinedConversionError in Ruby, a direct descendant of the EncodingError class.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html