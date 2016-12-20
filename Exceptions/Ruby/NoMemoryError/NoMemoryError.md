Next in our __Ruby Exception Handling__ series we'll be examining the dreaded `NoMemoryError` exception.  As the name implies, a `NoMemoryError` can occur within Ruby anytime the system attempts to allocate more memory than Ruby can provide or is allowed.

In this post we'll see examples of what can cause `NoMemoryErrors`, what it indicates in your code, and how to use or avoid these exceptions in the future!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `NoMemoryError` is a direct descendant of the [`Exception`] class.

## When Should You Use It?

To catch a potential `NoMemoryError`, it's best to consider the most likely points of execution where the application may utilize too much memory.  Typically this occurs when loading a file into memory (such as when downloading or using [`File.open`]), or when attempting to allocate more than the allowed memory for a particular Ruby object.

That said, while `NoMemoryErrors` _can_ be caught using Ruby's `begin-rescue` syntax, doing so should typically be a last resort, as Ruby only raises a `NoMemoryError` when memory _allocation_ fails.  It __does not__ typically fire when the application itself exceeds the actual physical memory, and as you might expect, the application will simply crash, the machine will grind to a halt, or any number of other awful possibilities.

As an example of where you might make use of `NoMemoryError`, it will most commonly occur when dealing with particularly large Ruby objects.

For example, let's examine some simple code where we're creating a [`String`] object by repeating a single character a specific number of times:

```ruby
puts "a" * 10
```

As expected, this would produce a `String` output that is 10 of the letter "a" in a row:

```
aaaaaaaaaa
```

As you may be aware, most programming languages (and Ruby is no exception) define an `unsigned long` integer as a number with a maximum of 4 bytes, which means the largest number possible for that `long` is _half_ of `2 ^ 32 - 1`, or `2,147,483,647`.  This number is halved, because while a `signed` number indicates both positive and negative values can be represented, an `unsigned long` can only be positive.  Since we have to account for the possibility of zero, the maximum positive value of an `unsigned long` is actually one less than `2 ^ 32 / 2`, or `2,147,483,647`.

In Ruby, depending on which version and bit-level you're using (32-bit vs 64-bit), Ruby does not allow `Strings` to be allocated in memory that are larger in size than the maximum positive `unsigned long` size seen above.

Therefore, we can raise a `NoMemoryError` by using this code:

```ruby
begin
    limit = 2**31 - 1
    puts "Limit: #{limit}"
    puts "a" * limit
rescue NoMemoryError => e
    puts "#{e.class}: #{e.message}"
    puts e.backtrace.join("\n")
end
```

This produces the following output:

```
Limit: 2147483647
NoMemoryError: negative allocation size (or too big)
g:/dev/ruby/nomemoryerror.rb:4:in `*'
g:/dev/ruby/nomemoryerror.rb:4:in `<main>'
```

The key line of code here is `puts "a" * limit`, which tells Ruby that we're want to output a new `String` that we're creating by concatenating the letter `a` `2,147,483,647` times  (which is defined by `2**31 - 1`).  Ruby responds by producing a `NoMemoryError` because the allocation size of our new string is too large for what Ruby allows, which we capture using the `rescue` syntax, in which we output some additional information to make it look like a normal error.

To resolve this `NoMemoryError` issue, we can make one simple fix: We can shrink the size of our string by one more character, by changing `limit` to `2**31 - 2`:

```ruby
begin
    limit = 2**31 - 2
    puts "Limit: #{limit}"
    puts "a" * limit
rescue NoMemoryError => e
    puts "#{e.class}: #{e.message}"
    puts e.backtrace.join("\n")
end
```

Depending on where you are executing this code, you may or may not see the interpretor attempt to create and actually output a string that is some two billion characters in length.

Whether or not the actual output works on your system, the key takeaway here is that `NoMemoryErrors` can be captured within Ruby code when memory allocation gets too overzealous, but the best practice always remains to try to eliminate potential memory leaks before they ever occur.

[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`File.open`]: https://ruby-doc.org/core-2.3.3/File.html#method-c-open
[`String`]: http://ruby-doc.org/core-2.3.3/String.html

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/File.html#method-c-open
- http://ruby-doc.org/core-2.3.3/String.html
