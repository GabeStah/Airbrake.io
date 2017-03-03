# Ruby Exception Handling: EOFError

Next on the docket in our __Ruby Exception Handling__ series, today we're going to examine the `EOFError` in Ruby.  `EOFErrors` are a descendants of the `IOError` class, and consequently, occur only in the specific scenario that an `IO` method is called for a file `stream`, in which that `stream` has already reached the end of the file.

In this article we'll explore just what might cause an `EOFError`, see where it sits within the overall Ruby `Exception` class hierarchy, and take a peak at how you might avoid running into `EOFErrors` of your own, so let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`IOError`](https://ruby-doc.org/core-2.4.0/IOError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) class, and is a superclass to one descendant of its own.
- `EOFError` is a direct descendant of the [`IOError`](https://ruby-doc.org/core-2.4.0/IOError.html) class.

## When Should You Use It?

Most commonly, `EOFErrors` will occur when a further attempt to read from a file `stream` is made, after the end of that file `stream` has already been reached.  Ruby's built-in [`IO`](https://ruby-doc.org/core-2.4.0/IO.html) class features a slew of methods and related classes that allow for significant manipulation of file objects.  Rather than cover them all here, we'll just take a brief look at some example code and how it ends up producing an `EOFError`, along with how to solve it.

In this example, our goal is to create a comma-separated value (`CSV`) file with a list of names and their respective IDs.  Nothing fancy, but this will be helpful when we read the file later.  To create our `csv` file, we've included a simple `append_data` function that loops through the data in our provided array, then writes each element (and associated `index`) to a new line in our truncated `names.csv` file.

Within the `begin-rescue` block, our actual examination code takes place.  Here we're first creating our new `csv` file and populating it, then we reopen the file and call the `.read()` method, which (by default) grabs a string of the full file stream, which we're outputting to the console.

Finally, before we've issued a `.close()` command on our file `stream`, we lastly attempt one more read attempt using the `.readline()` method.

```ruby
FILE_NAME = "names.csv"
NAMES = [
    "Alice Zebra",
    "Bob Yelma",
    "Christine Xylophone",
    "Dan Williams"
]

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def append_data(name, data)
    # Create new file in read-write mode, truncating file
    File.open(name, "w+") { |file|
        # Loop through data elements
        data.each_with_index { |value, index|
            # Add each element as line
            file.puts("#{index},#{value}")
        }
    }
end

begin
    # Create file
    append_data(FILE_NAME, NAMES)
    # Open in read mode (default)
    file = File.open(FILE_NAME)
    # Read file and output data
    puts file.read
    # Read again
    puts file.readline
rescue EOFError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Initially, the first call to `file.read()` works fine, and our output displays the contents of our `names.csv` file.  However, this has forced the file `stream` to read the entire file and thus is sitting at the end of the `stream`.  When our next call to `file.readline()` is made, sure enough, we throw (and must `rescue`) an `EOFError`:

```
0,Alice Zebra
1,Bob Yelma
2,Christine Xylophone
3,Dan Williams
[EXPLICIT] EOFError: end of file reached
code.rb:34:in `readline'
code.rb:34:in `<main>'
```

The challenge is that this isn't really a "fixable" error in the normal sense.  That is to say, Ruby is kind enough to report that we've reached the end of the file, and throws an `EOFError` for our troubles, but the production of such an error typically means there's a fundamental flaw in the structure of the code, since reaching the end of a file that's being read isn't inherently a bad thing.

In most cases (as with our example) the culprit is our non-idiomatic use of `File.open`, in which we assign the resulting file object to a variable, rather than using an inclusive `block` following the `File.open` method call.  By assigning it to a variable, it leaves us open to forgetting to manage our file `stream` status manually.  This typically requires calling the `.close()` method on our file, which closes the file and flushes any pending write operations before doing so.

The advantage to using a `File.open` code block to handle the file object we just created, is that once the block concludes, `.close()` is called for us automatically.  Thus, in our example above, we could change our `begin-rescue-end` block to something like this:

```ruby
begin
    # Create file
    append_data(FILE_NAME, NAMES)
    # Open in read mode (default)
    File.open(FILE_NAME) { |file|
        # Read file and output data
        puts file.read
    }
    # Open in read mode (default)
    File.open(FILE_NAME) { |file|
        # Read again, starting from top
        puts file.readline
    }
rescue EOFError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Admittedly, this isn't pretty code, particularly because we'd almost never want to actually perform the actions we're taking on this file (reading it all, then reading the first line of it immediately afterward).  However, for our example, it serves the purpose: We never manipulate our `file` object outside of a `File.open()` method code block, so our handling of `.close()` is done for us.  Thus, our output no longer shows an `EOFError`, and instead displays the entirety of the file (`file.read()`), then the first line (`file.readline()`):

```
0,Alice Zebra
1,Bob Yelma
2,Christine Xylophone
3,Dan Williams
0,Alice Zebra
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A detailed look at the EOFError in Ruby, a direct descendant of the StandardError class, along with a quick look at Ruby's file stream capabilities.

---

__SOURCES__

- https://ruby-doc.org/core-2.4.0/Exception.html