Moving right along through our in-depth __Ruby Exception Handling__ series, today we're going to examine the `InvalidByteSequenceError`.  An `InvalidByteSequenceError` is raised when utilizing Ruby's internal `Encoding` or `String` methods to transcode a string that contains an invalid byte for the particular encoding `converters` being utilized.

Throughout this article we'll examine the `InvalidByteSequenceError` class, look at where it sits within Ruby's `Exception` class hierarchy, and also explore how to deal with any `InvalidByteSequenceErrors` you may come across in your own coding endeavors.  Let's the adventure begin!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`StandardError`] is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.
- [`EncodingError`] is a direct descendant of the [`StandardError`] class, and is also a superclass with a handful of descendants of its own.
- `InvalidByteSequenceError` is a direct descendant of the [`EncodingError`] class.

# When Should You Use It?

As we explored in our `ConverterNotFoundError` article, Ruby's [`Encoding`] namespace houses the majority of all encoding and transcoding functionality within Ruby, allowing us to easily convert from one type of encoding to another.  However, in some instances, there may be a part of a string that cannot be converted for one reason or another.

In the case of the `InvalidByteSequenceError`, the reason for the encoding failure is because the provided string contains at least one `byte` sequence that is unable to be encoded or decoded, by either the `source` encoding (the format the string started in), or the `target` encoding (the format the string is being converted into).

A byte sequence in Ruby is simply a hexadecimal representation of a character.  For example, the byte representation of the letter `a` is simply the `ordinal` `97`, which can be discovered by calling the `.ord()` method on that string:

```ruby
"a".ord
=> 97
```

However, to indicate the hexadecimal value of the byte, we can use the special `\x` escape character prior to the hexadecimal value of our character.  In this case, for the letter `a`, we'd use `\x61`.

```ruby
"\x61".ord
=> 97
"\x61".chr
=> "a"
```

We also called the `.chr()` method to verify we've got the correct character for our byte.

We can also represent multiple characters by chaining together an entire `byte sequence`.  This is done either through a single string (e.g. `"\x01\x02\x03"`) or within an array, which is then `.joined` into a string.  Here we're taking a byte sequence of five characters and joining them together, then outputting the result:

```ruby
["\x68", "\x65", "\x6c", "\x6c", "\x6f"].join
=> "hello"
```

Armed with this basic understanding of how bytes are used in Ruby, we can see what might cause an `InvalidByteSequenceError` to occur.  Specifically, as the name implies, the `InvalidByteSequenceError` pops up when we attempt to use an escaped byte string (e.g. `"\x01"`) that is _invalid_ for the current type of encoding.

There are multiple reasons that a byte could be invalid for an encoding.  Since most text is already encoded using `UTF-8`, we'll take a look at some examples converting from `UTF-8` to `ASCII`.

Here we are simply trying to convert the byte `\xC0` from `UTF-8` to `ASCII`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates InvalidByteSequenceError
    puts "\xC0".encode("ASCII")
rescue Encoding::InvalidByteSequenceError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

This byte value of `\xC0` (and `\xC1`, in fact) are restricted and should never appear in valid `UTF-8` sequences because they could only (theoretically) be used for [`overlong encodings`], meaning an attempt to encode a 7-bit `ASCII` character using two bytes instead of one.  The result is that Ruby's attempt to encode this byte returns a baseline `InvalidByteSequenceError`:

```
[EXPLICIT] Encoding::InvalidByteSequenceError: "\xC0" on UTF-8
code.rb:12:in `encode'
code.rb:12:in `<main>'
```

Once we get into higher byte representations beyond `\xC1`, we start to produce slightly different `InvalidByteSequenceErrors` if our byte sequence isn't correct.  For example, let's try using `\xC2` with nothing else:

```ruby
"\xC2".encode('ASCII')
```

The result tells us that our sequence is incomplete; that something must follow our byte:

```
[EXPLICIT] Encoding::InvalidByteSequenceError: incomplete "\xC2" on UTF-8
code.rb:12:in `encode'
code.rb:12:in `<main>'
```

As it happens, bytes like `\xC2` are `leading bytes`, meaning they prefix other bytes to create a valid byte sequence.  We could resolve this by adding a second byte into the sequence, such as `\xC2\x80`, though we wouldn't produce any valid output since `ASCII` doesn't have that character.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html
[`EncodingError`]: https://ruby-doc.org/core-2.3.3/EncodingError.html
[`Encoding`]: https://ruby-doc.org/core-2.4.0/Encoding.html
[`overlong encodings`]: https://en.wikipedia.org/wiki/UTF-8#Codepage_layout

---

__META DESCRIPTION__

A close look at the InvalidByteSequenceError in Ruby, a direct descendant of the EncodingError class.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html
