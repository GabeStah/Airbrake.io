# Ruby Exception Handling: NoMethodError

Continuing through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll be taking a look at the `NoMethodError` in Ruby.  As clearly indicated by the name, the `NoMethodError` is raised when a call is made to a receiver (an object) using a method name that doesn't exist.

Throughout this article we'll dig a bit more into the `NoMethodError`, including where it sits within the Ruby `Exception` class hierarchy, as well as reviewing a few simple code examples that illustrate how `NoMethodErrors` can occur.  Let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`NameError`](https://ruby-doc.org/core-2.4.0/NameError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class, and is also a superclass with one descendant.
- `NoMethodError` is a direct descendant of the [`NameError`](https://ruby-doc.org/core-2.4.0/NameError.html) class.

## When Should You Use It?

Let's take a look at a common class configuration in Ruby and how we might accidentally produce a `NoMethodError` with an invalid call.  Below is our full code example, including the `Book` class that has an initializer and holds one attribute (as defined by using `attr_accessor`), which is the `title`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    create_book
    invalid_book_method
end

class Book
    # Create getter/setter for title attribute.
    attr_accessor :title

    def initialize(args = {})
        @title = args[:title]
    end
end

def create_book
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def invalid_book_method
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
        # Output book author (invalid method).
        puts book.author
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

# Execute examples.
execute_examples
```

Our first function that makes use of the `Book` class is `create_book`:

```ruby
def create_book
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

Here we just create a new instance of the `Book` class and assign it to our `book` variable, then we output both the `book` object itself and the `book.title` attribute.  The resulting output is as expected:

```
#<Book:0x0000000282cec0>
The Stand
```

In our second function, `invalid_book_method`, we're also creating a new instance of the `Book` class named `book` and outputting some information, but we also append a call to the `book.author` method:

```ruby
def invalid_book_method
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
        # Output book author (invalid method).
        puts book.author
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

As expected, this raises a `NoMethodError` because `book.author` is an invalid method (we never defined the `author` method within our `Book` class):

```
#<Book:0x0000000282cd58>
The Stand
[EXPLICIT] NoMethodError: undefined method `author' for #<Book:0x0000000282cd58 @title="The Stand">
```

Clearly we should probably have a way to keep track of the author of our books, so we should add the `author` method to our `Book` class.  There are two simple ways to accomplish this.

One option is to continue using the [`attr_accessor`](http://ruby-doc.org/core-2.4.0/Module.html#method-i-attr_accessor) method.  This method provides a bit of fun Ruby magic to our code, by allowing us to tell Ruby that the argument list of `:symbols` we provided should be added to our class as instance variables.  In addition, Ruby will automatically add two new methods to our class, which act as `getter` and `setter` methods with the name of the attribute we provided.

For example, here we're using `attr_accessor` to define the `:author` attribute for our `Book` class (in addition to the previous `:title` attribute we had):

```ruby
class Book
    # Create getter/setter for author and title attribute.
    attr_accessor :author, :title

    # ...
end
```

We removed the unrelated code, but by using `attr_accessor` in this way to define the `:author` attribute, it turns out this is functionally identical to defining the methods ourselves, like so:

```ruby
class Book
    # author getter.
    def author
        @author
    end

    # author setter.
    def author=(value)
        @author = value
    end

    # ...
end
```

Obviously, the `attr_accessor` shortcut is much simpler so that's considered the standard way to approach this problem, but both options are completely viable.  In our case, we'll stick with adding `:title` to our `attr_accessor` argument list, then call the `book_with_author` function:

```ruby
class Book
    # Create getter/setter for author and title attribute.
    attr_accessor :author, :title

    def initialize(args = {})
        @author = args[:author]
        @title = args[:title]
    end
end

def book_with_author
    begin
        # Create a new book
        book = Book.new(author: 'Stephen King', title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
        # Output book author.
        puts book.author
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

Sure enough, this works just fine without raising any errors, and our output includes the previous information, along with the expected `book.author` value:

```
#<Book:0x000000026e78a8>
The Stand
Stephen King
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A deeper look at the NoMethodError in Ruby, including functional code examples and a brief review of the attr_accessor method in Ruby.