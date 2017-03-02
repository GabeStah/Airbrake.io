# Ruby Exception Handling: IOError

Today, on our quest through the misty mountains of __Ruby Exception Handling__, we're going to be spelunking into the chasm of the `IOError` in Ruby.  As the name suggests, an `IOError` occurs whenever there's an issue with `I/O` (or `input/output`) within the application.

While the potential spectrum of `I/O` issues is quite broad, in this article we'll examine a few specific examples of when an `IOError` might appear, see where `IOErrors` fit within the Ruby `Exception` class hierarchy, and hopefully determine how to handle any `IOErrors` you might find on your own adventures.  Let's jump right in!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `IOError` is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) class, and is a superclass to one descendant of its own.

## When Should You Use It?

[`IO`](https://en.wikipedia.org/wiki/Input/output), or `input/output`, is such a common form of communication that some form of API calls for `IO` exist within almost every modern programming language, and Ruby is no exception.  Typically, this is in the form of file manipulation, where files are created or accessed, and data is read from the file or added to the file, as the application requires.

For example, if we want to open (or create) a new file named `names.txt` and append a new line to it with the name `Jane Doe`, we might use a snippet like this: 

```ruby
line = "Jane Doe"
# Open or create new file
File.open("names.txt", "a") { |file|
    # Append line
    file << line
    puts "New line added: #{line}"
}
```

We're using the `File.open()` method, followed by an associated block.  The inclusion of the block forces Ruby to pass a parameter to the block, which is our opened file in this case.  Once opened inside our block, we add our new `line` to the file, then output a little console message to verify that the line was added.  As expected, our output indicates the process was successful:

```
New line added: Jane Doe
```

It's also important to note that our call to `File.open()` includes a second argument, which is the `open mode` that Ruby will attempt to use to access the file.  This basically informs Ruby whether we want to `read`, `write`, `read/write`, `create`, `append`, and so forth with our current code block.  [Here we can see the valid `open modes`](https://ruby-doc.org/core-2.4.0/IO.html#method-c-new) for use within Ruby's `IO` methods.  In our case, we used `a` for `append`, which opens the file in a `write-only` mode, but _also_ creates a new file at that path if one doesn't already exist.

This `open mode` argument is critical, because watch what happens if we remove the `open mode` argument entirely:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    line = "Jane Doe"
    # Open or create new file
    File.open("names.txt") { |file|
        # Append line
        file << line
        puts "New line added: #{line}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

We've changed nothing else about our code, but without specifying an `open mode` argument, the default value of `open mode` is `r`, which indicates `read-only`.  Therefore, even though we were able to open our file, once we attempt to append a new line (`file << line`), Ruby throws an `IOError` at us:

```
[EXPLICIT] IOError: not opened for writing
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:32:in `write'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:32:in `<<'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:32:in `block in <main>'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:30:in `open'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:30:in `<main>'
```

Even though this particular `IOError` message indicates our problem is that the file we've opened doesn't allow writing (i.e. wasn't opened using a `write-enabled` `open mode`), we may also run into `IOErrors` in slightly different scenarios as well, in which case the error message will reflect our particular problem.

For example, here we're trying the inverse of the above example, where we've opened a file explicitly using a `write-only` mode, but we're attempting to `read` from it:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Open or create new file
    File.open("names.txt", "w") { |file|
        # Read line
        puts "File line read: #{file.read}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Sure enough, the output shows we've `rescued` another `IOError`, but this time because we're trying to read from our file when it only allows writing:

```
[EXPLICIT] IOError: not opened for reading
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:52:in `read'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:52:in `block in <main>'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:50:in `open'
D:/work/Airbrake.io/Exceptions/Ruby/StandardError/IOError/code.rb:50:in `<main>'
```

Changing our `open mode` to a form of `read-write` ensures that this error is resolved:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Open or create new file
    File.open("names.txt", "a+") { |file|
        # Read line
        puts "File line read: #{file.read}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

As expected, our output works as expected and we get an output showing what the first line of our file is:

```
File line read: Jane Doe
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A detailed look at the IOError in Ruby, a direct descendant of the StandardError class, along with a brief overview Ruby's `IO` functionality.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html