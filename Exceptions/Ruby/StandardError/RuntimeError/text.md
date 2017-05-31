# Ruby Exception Handling: RuntimeError

Making our way through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series our next stop brings us to the default raised exception class in Ruby, the `RuntimeError`.  A `RuntimeError` is raised when attempting to perform an invalid operation of some kind, which doesn't meet the criteria of a more specific error classification.

In this article we'll bust through the cracks of the `RuntimeError`, exploring where it sits within the Ruby `Exception` class hierarchy, as well as go over a few simple code examples that help to illustrate how `RuntimeErrors` are raised in the first place.  Let's dig in!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `RuntimeError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).

## When Should You Use It?

Most programming languages loosely define a runtime error as an exception that occurs during the runtime (execution) of the application.  While that's not all that useful of a definition, it can easily be distinguished from other categories of exceptions such as compilation errors, which are caught by the compiler of the language in question before execution even occurs.  For example, an error from incorrect syntax would not be considered a runtime error since it will be caught by the compiler before execution.  We saw this in our article exploring Ruby's [`SyntaxError`](https://airbrake.io/blog/ruby-exception-handling/syntaxerror) using the following example code:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    1=2 #=> syntax error, unexpected '=', expecting keyword_end
rescue SyntaxError => e
    print_exception(e, true)
end
```

That said, even in the realm of runtime errors things tend to be a little confusing since most languages provide dozens if not hundreds, as is the case with .NET, of built-in classes to handle specific runtime exceptions.  Ruby is no different, with specific errors that we've seen before like the [`RegexpError`](https://airbrake.io/blog/ruby/ruby-exception-handling-regexperror) and the [`LocalJumpError`](https://airbrake.io/blog/ruby/ruby-exception-handling-localjumperror).  However, since Ruby cannot inherently provide a specific class for every possible type of runtime error that might occur there must be a fallback and that's where the `RuntimeError` class comes in.

Perhaps the most important property of `RuntimeError` -- something that makes it unique among the dozens of other error classes in Ruby -- is that `RuntimeError` is the **default** error class that is used when calling the standard [`Kernel#raise`](http://ruby-doc.org/core-2.4.0/Kernel.html#method-i-raise) method that we all use to handle exceptions.  While `Kernel#raise` allows us to provide a specific error class which to raise, opting not to specify a class means Ruby will create an instance of `RuntimeError`.

For example, here we have a somewhat real-world example of a `Book` class with a custom `setter` for the `Book#page_count` property within which the value is checked and if it's less than 1000 pages everything is OK.  But, if it's greater than 1000 pages it is outside the allowed bounds and we raise a `RangeError`:

```ruby
class Book
    attr_accessor :author, :page_count, :title

    def initialize(args = {})
        @author = args[:author]
        # Invoke custom setter for passed page_count value, if applicable.
        self.page_count = args[:page_count]
        @title = args[:title]        
    end

    def page_count=(value)
        min, max = 1, 1000
        # Check if value outside allowed bounds.
        if value < min || value > max
            # Raise RangeError if outside bounds
            raise RangeError, "Value of [#{value}] outside bounds of [#{min}] to [#{max}]."
        else
            @page_count = value
        end
    end
end
```

Now when we create a new instance of `Book` we check the `page_count` value provided and either raise a `RangeError` or can continue execution as normal if no error occurs.  This gives us a few illustrating examples:

```ruby
def long_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Leo Tolstoy', page_count: 1225, title: 'War and Peace')
        # If no previous error explicitly generate RuntimeError
        raise RuntimeError, "Oh no, our book is too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def short_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Dr. Seuss', page_count: 72, title: 'Green Eggs and Ham')
        # If no previous error explicitly generate RuntimeError
        raise RuntimeError, "Oh no, our book is too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

def alternate_short_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Dr. Seuss', page_count: 72, title: 'Green Eggs and Ham')
        # If no previous error generate a default error
        raise "Oh no, our book is still too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end
```

In the first example our book is too long so we raise a `RangeError`.  In the second example we don't raise an error during creation of our book, so we get to explicitly raise a `RuntimeError`.  Finally, in the third example our book remains short enough so there's no `RangeError`, however we're not specifying what type of error to raise so it still defaults to `RuntimeError`.  The output for all three of these functions shows the expected error list:

```
[EXPLICIT] RangeError: Value of [1225] outside bounds of [1] to [1000].
[EXPLICIT] RuntimeError: Oh no, our book is too short!
[EXPLICIT] RuntimeError: Oh no, our book is still too short!
```

Outside of raising a `RuntimeError`, either explicitly or indirectly, it's also possible for them to occur during normal execution when an invalid operation occurs.  One such instance where a `RuntimeError` may be raised is when trying to modify an immutable object like a `frozen` string.  For those who may unfamiliar, Ruby provides the [`Object#freeze`](https://ruby-doc.org/core-2.4.1/Object.html#method-i-freeze) method that allows us to explicitly convert a normally mutable object to an immutable.  For example, in this snippet we declare a `name` string variable, add something to it, then output the full string.  We then try that same pattern again, but the second time we explicitly `freeze` our string variable before trying to modify it:

```ruby
def freeze_example
    begin
        # Declare mutable string
        name = 'Jane Doe'
        name << ' is awesome!'
        puts name
        # Declare string and freeze (make it immutable)
        name = 'John Smith'.freeze
        name << ' is awesome!'
        puts name
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

As you might expect the first round works fine but the second round raises a `RuntimeError` because we're trying to modify an immutable string:

```
Jane Doe is awesome!
[EXPLICIT] RuntimeError: can't modify frozen String
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A deeper examination of the RuntimeError in Ruby using working code examples and a brief exploration of default error raising behavior in Ruby.