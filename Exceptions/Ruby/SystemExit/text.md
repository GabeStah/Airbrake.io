# Ruby Exception Handling: SystemExit

As we approach the end of our all-encompassing [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll be taking a look at the `SystemExit` error, which is raised whenever the `Kernel#exit` method is invoked to terminate the current process.

In this article we'll explore the `SystemExit` exception in more detail by looking at where it resides in the Ruby `Exception` class hierarchy, as well as showing a few code examples to illustrate how `SystemExist` exceptions are typically raised, so let's get started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- `SystemExit` is the direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class.

## When Should You Use It?

Since `SystemExit` exceptions are only raised by calling the [`Kernel#exit`](https://ruby-doc.org/core-2.4.0/Kernel.html#method-i-exit) method it might benefit us to take a closer look at how this method works and how Ruby handles closing the current process in general.  We'll start with the full code example below (well, one of them anyway) and then we'll explore it in more detail afterward:

```ruby
def execute_examples
    exit_process
    Logging.line_separator
    at_exit_example
end

def exit_process
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def at_exit_example
    # Specify +at_exit+ block.  Fires after the exit calls are completed.
    at_exit { Logging.log("This is part of the at_exit block.") }    
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
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

Calling the `Kernel#exit` method in Ruby initiates a termination of the current Ruby script.  It _also_ raises a `SystemExit` exception at the same time, which we can catch and use for our own purposes during the final process termination events.

To that end we start with a simple `#exit_process` method that calls `Kernel#exit`, tries to log a message within the same block as `exit` and immediately afterward, then also `rescues` the `SystemExit` exception and outputs a few more messages for us:

```ruby
def exit_process
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was 
        # skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.") #=> Exiting process.
        # Log exception type.
        Logging.log(e) #=> (EXPLICIT) SystemExit: exit
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

As you can see the output of `"Exit stopped."` never executes because of the invocation of `Kernal#exit`, which immediately begins the termination process and only allows for the raising of our `SystemExit` exception, along with any final garbage collection-style cleanup methods.

One example of handling process exiting in Ruby is to use the [`Kernel#at_exit`](https://ruby-doc.org/core-2.4.0/Kernel.html#method-i-at_exit) method.  `Kernel#at_exit` expects a block to be passed that is converted into a `Proc` and registers it for execution when the current process is exiting.  This allows us to easily perform any sort of cleanup procedures we need to before our application shuts down.

For example, the `#at_exit_example` method starts by specifying a `Kernel#at_exit` block that just outputs a message for testing purposes, then continues on to call `Kernel#exit` just as we did before, along with the associated output messages:

```ruby
def at_exit_example
    # Specify +at_exit+ block.  Fires after 
    # the exit calls are completed.
    at_exit { Logging.log("This is part of the at_exit block.") }    
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was 
        # skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

To see what's going on here we'll show the full output log:

```
Exiting process.
(EXPLICIT) SystemExit: exit
This is part of the at_exit block.
```

As you can see we begin the exit process then `rescue` our `SystemExit` exception, before finalizing the termination procedure by calling the `Proc` we created with `Kernel#at_exit`.  Cool!

Our last example shows how to use the [`ObjectSpace::define_finalizer`](https://ruby-doc.org/core-2.4.0/ObjectSpace.html#method-c-define_finalizer) method.  Unlike `Kernel#at_exit`,  which behaves as a _global_ `Proc` that is called when the current process executes no matter what, the `ObjectSpace::define_finalizer` method accepts an `Object` as the first argument.  When that object is destroyed the `Proc` that is passed as the second argument is executed.  This is particularly handy for invoking special finalization logic when a class instance is destroyed.

For example, here we're using a tried-and-true `Book` class model with a few simple fields.  Within the `#initialize` method we also define a finalizer that applies to `self` (the instance of `Book` that is being destroyed).  We've opted to use our own `#finalize` method to accomplish this and return a `Proc` to be invoked, but we could just as easily created an inline `Proc` instead:

```ruby
class Book
    # Create getter/setter for author and title attribute.
    attr_accessor :author, :title

    def initialize(args = {})
        @author = args[:author]
        @title = args[:title]

        # Define finalizer method for garbage collection cleanup.
        ObjectSpace.define_finalizer(self, finalize)
    end

    def finalize
        # Output message indicating destruction of this instance.
        proc { Logging.log("Destroying '#{@title}' by #{@author}.") }
    end
end

def finalizer_example
    begin
        # Create Book instance.
        book = Book.new( { title: "The Stand", author: "Stephen King"} )
        # Exit the current process.
        exit
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end
```

Within the `#finalizer_example` method we create a new `Book` instance and then exit the process using the `Kernel#exit` method again.  The result is that we still raise a `SystemExit` exception since we called `Kernel#exit`, but afterward the Ruby garbage collector destroys our `book` instance that was created, generating the `Destroying...` text output we were after:

```
Exiting process.
(EXPLICIT) SystemExit: exit
Destroying 'The Stand' by Stephen King.
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A closer look at the SystemExit error in Ruby, including functional code examples and a short exploration of exiting and process termination in Ruby.