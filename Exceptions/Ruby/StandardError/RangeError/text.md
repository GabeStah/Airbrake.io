# Ruby Exception Handling: RangeError

Making our way along through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll explore the `RangeError` within Ruby.  Simply put, the `RangeError` is raised when a numerical value is provided to a Ruby method that falls outside the allowed range of values.

We'll take some time in this article to fully explore the `RangeError` including where it resides within the Ruby `Exception` class hierarchy, along with a few code examples to illustrate some possible ways to raise `RangeErrors` yourself, so let's get going!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `RangeError` is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class.

## When Should You Use It?

To further explore the `RangeError` let's start right out with some example code:

```ruby
# Output the first (2) items in the array.
['Alice', 'Bob', 'Cindy', 'Dave'].first(2)
```

This produces the expected result of the first two values:

```
["Alice", "Bob"]
```

However, like many built-in Ruby methods, the `first` method we're using on our `Array` expects the passed value to fall within a certain range.  Like most programming languages, Ruby's `Integer` value is four bytes in length, allowing the maximum value of an `Integer` to be `2,147,483,647`.  Many methods in Ruby, including `Array.first` that we used above, expect any passed numeric argument to be a valid `Integer` (or `long`), meaning the value cannot exceed that 2.14 million maximum.

So, what happens if we pass a number that's just a _bit_ larger than the maximum allowed for the `Integer` to the `Array.first` method?

```
# Output the first (2147483648) items in the array.
['Alice', 'Bob', 'Cindy', 'Dave'].first(2147483648)
```

Ruby isn't happy about this and it raises a `RangeError`, indicating the value we've provided is larger than an `Integer` (`long`), and is therefore a `bignum`:

```
RangeError: bignum too big to convert into `long'
```

That's all well and good, and we can see how built-in Ruby methods might raise a `RangeError`, but what about raising a `RangeError` in our own code?  When is it appropriate to use that type of error?  To illustrate, we've created a `Book` class with three `attributes`: `author`, `page_count`, and `title`: 

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

class Book
    attr_accessor :author, :page_count, :title

    def initialize(args = {})
        @author = args[:author]
        @page_count = args[:page_count]
        @title = args[:title]
    end
end
```

Simple enough.  Now we can create an instance of our `Book` class within the `create_book` function:

```ruby
def create_book
    begin
        # Create a new book
        book = Book.new(author: 'Patrick Rothfuss', page_count: 662, title: 'The Name of the Wind')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

This works just as expected, creating our book object and outputting the attribute data for it:

```
#<Book:0x000000028457b8>
Patrick Rothfuss
The Name of the Wind
662
```

Our `Book` class places no restrictions or limitations on the attribute fields we've provided.  Specifically, what if we need to create an _abridged_ book class, which would only be valid for books with a limited number of pages?  For that, we've inherited our `Book` class with the `AbridgedBook` class seen below:

```ruby
class AbridgedBook < Book
    # Add page_count getter.
    attr_reader :page_count

    def initialize(args = {})
        # Executes initialize for the parent superclass.
        super(args)
        # Invoke custom setter for passed page_count value, if applicable.
        self.page_count = args[:page_count]
    end

    def page_count=(value)
        min, max = 1, 1000
        # Check if value outside allowed bounds.
        if value < min || value > max
            raise RangeError, "Value of [#{value}] outside bounds of [#{min}] to [#{max}]."
        else
            @page_count = value
        end
    end
end
```

We've also slightly changed the behavior of the `page_count` attribute from the original `Book` class it inherits from.  We're using `attr_reader` instead of `attr_accessor`, so we only automatically generate the `getter` method.  For the `setter` of `page_count=`, we define the method ourselves so we can specify a valid range (1 to 1000) that the `page_count` attribute can fall within.  If it's outside these bounds, we raise a `RangeError`.  Lastly, we're using the `super` method call, which calls the superclass matching method (in this case `initialize`), so we don't need to redefine the assignment of repeated fields like `author` and `title`.

To make use of `AbridgedBook` we have two functions, `create_abridged_book` and `create_invalid_abridged_book`.  For `create_abridged_book` we are creating a book record for _The Stand_ by Stephen King, which is 823 pages, falling within the valid range of `page_count`:

```ruby
def create_abridged_book
    begin
        # Create a new book
        book = AbridgedBook.new(author: 'Stephen King', page_count: 823, title: 'The Stand')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

As expected, this works just fine and our `AbridgedBook` instance is created and we spit out some info about it:

```
#<AbridgedBook:0x00000002845600>
Stephen King
The Stand
823
```

However, within the `create_invalid_abridged_book` we can confirm that our range limitations for `page_count` are working by creating an book instance for _War and Peace_, which is a whopping 1225 pages:

```ruby
def create_invalid_abridged_book
    begin
        # Create a new book
        book = AbridgedBook.new(author: 'Leo Tolstoy', page_count: 1225, title: 'War and Peace')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

As we hoped, our `AbridgedBook.page_count=` setter method catches this value and raises a `RangeError` as we asked it to, informing us that `1225` is outside the bounds we specified:

```
[EXPLICIT] RangeError: Value of [1225] outside bounds of [1] to [1000].
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close examination of the RangeError in Ruby, including working code examples for both built-in Ruby methods and custom raising of a RangeError.