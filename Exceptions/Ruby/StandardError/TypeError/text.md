# Ruby Exception Handling: TypeError

Next on the docket of our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series is the lovable `TypeError` class.  `TypeErrors` are fairly simple and are raised when an object that was passed as an argument is not of the expected type.

In this article we'll look at `TypeErrors` a bit closer and see where it resides in the Ruby `Exception` class hierarchy.  We'll also dig into a few simple, functional code examples to see how `TypeErrors` are commonly raised in the hopes of helping you during your own coding endeavors, so let's get crackin'!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `TypeError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).

## When Should You Use It?

Raising a `TypeError` is a rather simple affair -- it commonly occurs when trying to pass an incorrect data type to a method.  Many API functions and methods expect a specific type of object to be passed as arguments, so any attempts to pass invalid types usually results in a failure, and sometimes even the raising of a `TypeError` depending how that module was written.

To see `TypeErrors` in action let's start with some example code.  Below we have the full working code example, after which we'll break it down into easier chunks to see what's going on:

```ruby
def execute_examples
    array_example
    Logging.line_separator
    invalid_array_example
    Logging.line_separator
    string_example
    Logging.line_separator
    invalid_string_example
end

def array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then grab first (4) records and output them to log.
        Logging.log(titles.sort.first(4))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def invalid_array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then try to get first '4' records as string and output.
        Logging.log(titles.sort.first('4'))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at zero index.
        Logging.log(title.insert(0, 'The '))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def invalid_string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at invalid '0' index.
        Logging.log(title.insert('0', 'The '))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

# Execute examples.
execute_examples

module Logging
    extend Utility

    class << self
        # Outputs +value+ to console.
        # +args+ may include:
        #    +:explicit+ (Boolean) - Is +Exception+ class +value+ expected? [default: true]
        #    +:timestamp+ (Boolean) - Should timestamp be included? [default: false]
        #
        # Examples:
        #    
        #    Logging.log('My message') #=> "My message"
        #    Logging.log('My message', { timestamp: true} ) #=> "[12:00:05] My message"
        #
        #    begin
        #       raise Exception.new('An exception!')
        #    rescue Exception => e
        #       Logging.log(e) 
        #    end
        #    #=> (EXPLICIT) Exception: An exception!
        #    #=>    (...backtrace...)
        def log(value, args = {})
            # Check if exception was explicit.
            explicit = args[:explicit].nil? ? true : args[:explicit]
            # Get timestamp if necessary.
            timestamp = args[:timestamp] ? formatted_timestamp : ""

            if value.is_a?(Exception)
                # If +value+ is an +Exception+ type output formatted exception.
                puts timestamp << formatted_exception(value, explicit)
            elsif value.is_a?(String)
                # If +value+ is a +String+ directly output
                puts timestamp << value                
            else
                # If +value+ is anything else output.
                puts timestamp
                puts value
            end 
        end

        # Output the specified +separator+ +count+ times to log.
        # +args may include:
        #    +:count+ (Integer) - Number of characters to output. [default: 20]
        #    +:separator+ (String) - Character or string to duplicate and output. [default: '-']
        def line_separator(args = {})
            count = args[:count].nil? ? 20 : args[:count]
            separator = args[:separator].nil? ? '-' : args[:separator]

            # Concatenate and output.
            puts separator * count
        end

        private

            def formatted_exception(exception, explicit)
                # Set explicit or inexplicit tag.
                output = "(#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}) "
                # Add class and message.
                output << "#{exception.class}: #{exception.message}\n"
                # Append backtrace with leading tabs.
                output << "\t" << exception.backtrace.join("\n\t")
                # Return output string.
                output
            end

            def formatted_timestamp
                "[#{Time.now.strftime("%T")}] "
            end
    end
end
```

To start we have our `array_example` method where we've listed a few book titles in an array and we want to `#sort` that array alphabetically and then grab the first four elements using the `#first` method:

```ruby
def array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then grab first (4) records and output them to log.
        Logging.log(titles.sort.first(4))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

As expected this works just fine and outputs the first four alphabetized titles:

```
Do Androids Dream of Electric Sheep?
Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation
Pride and Prejudice
Something Wicked This Way Comes
```

However, now in our `invalid_array_example` method we're going to try something much the same, but we're accidentally passing the `String` value of `'4'` instead of the `Integer` value of `4` to the `#first` method of our array:

```ruby
def invalid_array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then try to get first '4' records as string and output.
        Logging.log(titles.sort.first('4'))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

As it turns, if we look directly at the source code of Ruby's `#first` method we see that it requires an integer (`int`) parameter for the first value (`argc`):

```c
rb_ary_first(int argc, VALUE *argv, VALUE ary)
{
    if (argc == 0) {
        if (RARRAY_LEN(ary) == 0) return Qnil;
        return RARRAY_AREF(ary, 0);
    }
    else {
        return ary_take_first_or_last(argc, argv, ary, ARY_TAKE_FIRST);
    }
}
```

Therefore, our above Ruby code raises a `TypeError` for our troubles, informing us that we cannot implicitly convert our `String` value to an `Integer` that is required:

```
(EXPLICIT) TypeError: no implicit conversion of String into Integer
```

As it happens there are near-countless methods in Ruby that will raise `TypeErrors` if given improper argument data types.  Rather than an array let's try a `String` using the `#insert` method, which expects an `Integer` as the first argument to indicate the `index` of where the second argument should be inserted into the targetted string.  Here we're adding the missing word `'The'` to our title then outputting the result:

```ruby
def string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at zero index.
        Logging.log(title.insert(0, 'The '))
        #=> The Hitchhiker's Guide to the Galaxy
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

That works just fine, but once again if we pass a non-integer value the `#insert` method doesn't know what to do.  The `invalid_string_example` method throws another `TypeError` just as we saw before:

```ruby
def invalid_string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at invalid '0' index.
        Logging.log(title.insert('0', 'The '))
        #=> (EXPLICIT) TypeError: no implicit conversion of String into Integer
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close look at the the TypeError in Ruby including a short glance at some Ruby method source code along with a few working code examples.