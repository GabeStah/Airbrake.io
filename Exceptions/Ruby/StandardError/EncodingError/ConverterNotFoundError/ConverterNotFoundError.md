As our exploration through the __Ruby Exception Handling__ series continues, today we'll be examining the `ConverterNotFoundError`.  `ConverterNotFoundErrors` appear in Ruby when attempting to use any of the variety of transcoding methods provided by Ruby, but in doing so, passing an invalid converter that Ruby isn't aware of.

In the meat of this article we'll explore the details of the `ConverterNotFoundError` class, see where it sits in Ruby's `Exception` class hierarchy, and also learn how you might deal with any `ConverterNotFoundErrors` that you may encounter in your own coding.  Let's get this party started!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`StandardError`] is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.
- [`EncodingError`] is a direct descendant of the [`StandardError`] class, and is also a superclass with a handful of descendants of its own.
- `ConverterNotFoundError` is a direct descendant of the [`EncodingError`] class.

# When Should You Use It?

To fully understand why a `ConverterNotFoundError` might be raised, we need to explore a bit more about Ruby's handling of encoding practices.  Put simply, Ruby provides the [`Encoding`] namespace, which is parent to all `encoding types` or `converters`, as well as the home of all the `Encoding` errors, including `ConverterNotFoundError`.

This means, we can view all the constants which are part of the `Encoding` namespace by making a simple call to the `.constants` method:

```ruby
puts Encoding.constants
```

The output is a list of all constants that are children within the `Encoding` namespace, including all error classes and all `converters`:

```
CompatibilityError      
UndefinedConversionError
InvalidByteSequenceError
ConverterNotFoundError  
Converter               
ASCII_8BIT              
UTF_8                   
US_ASCII                
Big5                    
...
```

This presents an easy way to see every `converter` that Ruby knows about and can utilize.  These can then be called as part of all the transcoding/encoding methods provided within Ruby, such as the `.encode()` method used on a string.  We can pass either the direct constant (`Encoding::UTF_8`), or use one of the named aliases (such as `'UTF-8'`).  Both of these lines are executed identically:

```ruby
puts "hello".encode(Encoding::UTF_8)
puts "hello".encode('UTF-8')
```

Now that we understand how to find what possible `converters` Ruby is aware of, as well as how to properly pass them to encoding methods, we can produce an `ConverterNotFoundError` by trying to pass an invalid `converter` to the encoding method.  For example, here we're trying to `.encode()` using the `'UTF-89'` `converter`, which doesn't exist (arguably a simple typo to make):

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates ConverterNotFoundError
    puts "hello".encode('UTF-89')
rescue Encoding::ConverterNotFoundError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

As expected, Ruby cannot locate a `converter` with an alias of `'UTF-89'`, so it produces a `ConverterNotFoundError` which we `rescue`:

```
[EXPLICIT] Encoding::ConverterNotFoundError: code converter not found (UTF-8 to UTF-89)
code.rb:9:in `encode'
code.rb:9:in `<main>'
```

To prevent `ConverterNotFoundErrors` from occuring, the best practice is to always reference specific `converters` by the actual constant, not by a string alias.  This is because Ruby will attempt to access a constant which doesn't exist as part of the `Encoding` namespace, and throw a `NameError` in response:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates ConverterNotFoundError
    puts "hello".encode(Encoding::UTF_89)
rescue Encoding::ConverterNotFoundError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Here we're calling the `.encode()` method, but passing a direct reference to a constant of `Encoding::UTF_89`, which again is a probable typo but doesn't exist in the constant set.  As expected, this throws a `NameError` due to the uninitialized constant we specified:

```
[INEXPLICIT] NameError: uninitialized constant Encoding::UTF_89
code.rb:24:in `<main>'
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html
[`EncodingError`]: https://ruby-doc.org/core-2.3.3/EncodingError.html
[`Encoding`]: https://ruby-doc.org/core-2.4.0/Encoding.html

---

__META DESCRIPTION__

A detailed exploration of Ruby's ConverterNotFoundError, a descendant of the EncodingError class.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html
