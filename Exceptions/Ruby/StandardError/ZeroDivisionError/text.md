# Ruby Exception Handling: ZeroDivisionError

Today in our continued journey through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series we'll be taking a closer look at the `ZeroDivionError`.  As you may suspect the `ZeroDivisionError` occurs when attempting to divide a number by zero.

In this article we'll briefly explore the `ZeroDivisionError` in more detail, including where it sits within the Ruby `Exception` class hierarchy and showing a few simple code examples to illustrate how `ZeroDivisionErrors` occur in the first place.  Let's get started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `ZeroDivisionError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).

## When Should You Use It?

Producing a `ZeroDivisionError` is quite simple, but there are a few small quirks that you'll want to be aware of before assuming your `rescue ZeroDivisionError => e` statement will always catch attempts to divide by zero-ish values.  To illustrate we have a few example methods showing normal division, division by exactly zero, division by floating zero, and division by negative floating zero.  As usual we'll start with the full code snippet and then we'll briefly go through it to see what's going on:

```ruby
def execute_examples
    division_example
    Logging.line_separator
    zero_division_example
    Logging.line_separator
    floating_zero_division_example
    Logging.line_separator
    negative_floating_zero_division_example
end

def division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator.
        denominator = 5
        # Divide and output the result.
        Logging.log(numerator / denominator)
        #=> 3
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero.
        denominator = 0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> (EXPLICIT) ZeroDivisionError: divided by 0
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero as float.
        denominator = 0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> Infinity
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def negative_floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to negative zero as float.
        denominator = -0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> -Infinity
    rescue ZeroDivisionError => e
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
                puts timestamp if !timestamp.empty?
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

We start with a normal and functional `#division_example` method:

```ruby
def division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator.
        denominator = 5
        # Divide and output the result.
        Logging.log(numerator / denominator)
        #=> 3
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

This works as expected and outputs a result of `3`.  However, if we change our `denominator` to `0` we'll produce a `ZeroDivisionError`, as seen in `#zero_division_example`:

```ruby
def zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero.
        denominator = 0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> (EXPLICIT) ZeroDivisionError: divided by 0
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

That should make sense to most of us I think: Just like when you were bored in school and were messing around with your calculator and tried dividing by zero you'd get an error, the same applies in Ruby (and probably all other programming languages for that matter).  However, dividing by zero in all cases may not work exactly as you'd think.  Watch what happens when we change our denominator from `0` to a floating point representation of `0.0`:

```ruby
def floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero as float.
        denominator = 0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> Infinity
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

Suddenly we no longer produce a `ZeroDivisionError` but, instead, we get an output of `Infinity`.  This is because Ruby handles various numeric data object types differently: whole numbers or `Integers` like `0` are calculated differently than `Floats` like `0.0`.

We can see this strangeness continue if we change the `denominator` from `0.0` to the negative version of `-0.0`, which produces an output of _negative_ `Infinity`:

```ruby
def negative_floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to negative zero as float.
        denominator = -0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> -Infinity
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A deeper look at the ZeroDivisionError in Ruby including working code samples and a brief discussion of Ruby's differentiation between numeric types.