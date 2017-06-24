# Ruby Exception Handling: SystemStackError

Today our journey through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series continues with one of the last remaining Ruby errors left to discuss: `SystemStackError`.  This little baby pops up when code gets a little recursion-happy and ends up causing a stack overflow.

In this article we'll examine the `SystemStackError` in more detail including where it sits within the Ruby `Exception` class hierarchy and how it might be raised in day-to-day coding.  Let's get crackin'!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- `SystemStackError` is the direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class.

## When Should You Use It?

Understanding what might cause a `SystemStackError` is just a matter of understanding what `stack overflow` means in the realm of development.  Thus, we'd be well served to spend a few minutes refreshing ourselves of what stack overflow is and how it typically occurs.  Most applications are allocated a specific range of `memory addresses` during execution.  These addresses are pointers to `bytes` of data (memory) that the application is allowed to use.  By extension, this memory assignment creates what is known as an `address space`, which is a specific quantity and range of `memory addresses` that can be safely used by the application in order to store and manipulate memory.

Since available memory is finite (for now, anyway) the `address space` assigned to an application is restricted within certain bounds.  The actual _size_ of this `address space` depends on many factors, but the result is that an application is allocated -- and can, therefore, only _use_ -- a certain amount of memory before it runs out.  In many cases the built-in `garbage collector` process within the programming language or framework will frequently gather up previously-used-but-no-longer-required address spaces and empty their contents (taking out the garbage, so to speak), which frees up more memory space to be used by future processing.

However, in some cases, the application's code may attempt to perform tasks that require additional memory _beyond_ what was allocated by the `address space` assigned to the process in the first place.  9 times out of 10, when this happens the application generates a `stack overflow` error which, in the case of Ruby, means raising a `SystemStackError`.

Now that we're refreshed on stack overflows we can dive into a bit of simple code and see how they typically occur in Ruby.  Probably the most common anti-pattern that often leads to stack overflows is the use of `recursion`.  Recursion is the practice of calling a method _within the code of said method_.  This practice is typically used when there exists an `end goal` (something the recursive method is aiming to eventually accomplish), combined with the ability to reduce the workload (and code overhead) of accomplishing that end goal.

For our examples here we have a `MathClass` that includes a couple simple methods called `recursion` and `double`:

```ruby
class MathClass
    class << self
        attr_accessor :count, :data, :value
    end

    def initialize(args = {})
        reset(args)
    end

    # Reset all class attributes, if necessary.
    def reset(args = {})
        self.class.count = args[:count] || 0
        self.class.data = args[:data] || []
        self.class.value = args[:data] || 1
    end

    # Increment the iteration counter.
    def increment_count(args = {})
        output = args[:output].nil? ? false : args[:output]
        Logging.line_separator if output
        self.class.count += 1
        Logging.log(self.class.count) if output
    end

    # Simple recursive method, incrementing the counter and executing itself.
    def recursion
        begin
            # Increment counter.
            increment_count
            # Recursively call.
            recursion
        rescue SystemStackError => e
            # Log stack overflow exception.
            # Exclude backtrace since it may contains tens of thousands of identical lines.
            Logging.log(e, { backtrace: false })
            # Output number of iterations required to hit overflow.
            Logging.log("Recursion iteration count: #{self.class.count}")
        rescue => e
            Logging.log(e, { explicit: false, backtrace: false })
        end    
    end

    # Double the +value+ attribute ad naseum, recursively calling self.
    def double
        begin
            # Double value.
            self.class.value *= 2
            # Add to array to test if memory runs out before overflow.
            self.class.data.push(self.class.value)
            # Increment counter.
            increment_count
            # Recursively double.
            double
        rescue SystemStackError => e
            # Log stack overflow exception.
            # Exclude backtrace since it may contains tens of thousands of identical lines.
            Logging.log(e, { backtrace: false })
            # Output number of iterations required to hit overflow.
            Logging.log("Doubling iteration count: #{self.class.count}")
        rescue => e
            Logging.log(e, { explicit: false })
        end          
    end
end

def execute_examples
    math_class = MathClass.new()
    math_class.double
    math_class.reset
    math_class.recursion
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
            # Include backtrace
            backtrace = args[:backtrace].nil? ? true : args[:backtrace]
            # Get timestamp if necessary.
            timestamp = args[:timestamp] ? formatted_timestamp : ""

            if value.is_a?(Exception)
                # If +value+ is an +Exception+ type output formatted exception.
                puts timestamp << formatted_exception( { exception: value, explicit: explicit, backtrace: backtrace } )
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

            def formatted_exception(args = {})
                backtrace = args[:backtrace].nil? ? true : args[:backtrace]
                # Set explicit or inexplicit tag.
                output = "(#{args[:explicit] ? 'EXPLICIT' : 'INEXPLICIT'}) "
                # Add class and message.
                output << "#{args[:exception].class}: #{args[:exception].message}\n"
                # Append backtrace with leading tabs.
                output << "\t" << args[:exception].backtrace.join("\n\t") if backtrace
                # Return output string.
                output
            end

            def formatted_timestamp
                "[#{Time.now.strftime("%T")}] "
            end
    end
end
```

Here the `recursion` method is about as simple as it gets to create a recursive call in Ruby.  There's a lot of extra fluff to help us `rescue` potential errors and the like, but at the most basic level we can create a recursive method by defining the method name and then immediately calling that method inside the method's code block.  That's it: three lines (or fewer) and we're done!

```ruby
# Simple recursive method, incrementing the counter and executing itself.
def recursion
    begin
        # Increment counter.
        increment_count
        # Recursively call.
        recursion
    rescue SystemStackError => e
        # Log stack overflow exception.
        # Exclude backtrace since it may contains tens of thousands of identical lines.
        Logging.log(e, { backtrace: false })
        # Output number of iterations required to hit overflow.
        Logging.log("Recursion iteration count: #{self.class.count}")
    rescue => e
        Logging.log(e, { explicit: false, backtrace: false })
    end    
end
```

The problem with recursion is if you're not careful your code may have formed an `infinite loop`, which is what we have in the example method above.  There's no checks or balances to ensure that the recursive loop breaks at some reasonable point.  Therefore, the Ruby engine has no choice but to eventually halt execution itself by raising a `SystemStackError`, as we see in the output:

```
(EXPLICIT) SystemStackError: stack level too deep
Recursion iteration count: 11910
```

Our other example method is dubbed the `doubler` (sorry for the terrible pun):

```ruby
# Double the +value+ attribute ad naseum, recursively calling self.
def double
    begin
        # Double value.
        self.class.value *= 2
        # Add to array to test if memory runs out before overflow.
        self.class.data.push(self.class.value)
        # Increment counter.
        increment_count
        # Recursively double.
        double
    rescue SystemStackError => e
        # Log stack overflow exception.
        # Exclude backtrace since it may contains tens of thousands of identical lines.
        Logging.log(e, { backtrace: false })
        # Output number of iterations required to hit overflow.
        Logging.log("Doubling iteration count: #{self.class.count}")
    rescue => e
        Logging.log(e, { explicit: false })
    end          
end
```

Here we have another recursive method call in which `doubler` invokes itself, but we've also added some extra functionality to experiment with trying to run out of memory via means other than stack overflow.  In this case we're just doubling the `value` class attribute every iteration and then storing each entry in the `data` attribute array.  Since our `value` attribute will be ever-increasing in size that's one angle of attack to try to see if Ruby runs out of memory.  The other technique is to maintain an ever-growing array in the form of the `data` attribute.

As it turns out, Ruby doesn't much care about our pitiful attempts to use up all the memory via (relatively small) numeric calculations and array storages, and thus execution of the `double` method results in the exact same number of recursive iterations as the `recursion` method before Ruby gives up and raises a `SystemStackError`:

```
(EXPLICIT) SystemStackError: stack level too deep
Doubling iteration count: 11910
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A brief examination of the SystemStackError class in Ruby, including functional code examples and a short exploration of recursion techniques.