# Ruby Exception Handling: Fatal Error

Today we finally come to the end of the journey through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series.  The last, and perhaps most critical, error left to discuss is the `Fatal Error`, which indicates a severe crash in a Ruby application -- something so dramatic that the process couldn't recover from it through normal means.

There's no time to waste, so let's get on with our last article detailing the plethora of possible Ruby exceptions.  In this piece we'll explore the `Fatal Error` in more detail, looking at where it fits within the Ruby `Exception` class hierarchy, as well as showing a few code examples illustrating how these painful errors might occur.  Let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- `Fatal Error` is the direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class.

## When Should You Use It?

`Fatal Errors` are a beast.  Generally speaking, when a `Fatal Error` occurs it means the application has failed so horrendously that Ruby is unable to recover from the problem.  This can occur for a variety of reasons but some typical cases might be process issues, memory problems, IO failures, and so forth.  However, Ruby already has built-in errors specifically designed for such failures, so when one of those existing error types isn't raised it's usually because the application broke unexpectedly and the a `Fatal Error` occurs.

Consequently, it should come as no surprise that `Fatal Errors` effectively _cannot be `rescued` or recovered from_.  When a `Fatal Error` occurs the Ruby application is generally going to be shutting down/killing the process.

That said, we can still _play_ with `Fatal Errors` a bit, since the underlying object that Ruby uses is still just that: an object.

First we'll start with the full code sample below, after which we'll go over it in more detail.

```ruby
require 'require_all'
require 'FileUtils'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

def valid_example
    # Set valid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\accessible\data.csv'    
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end   
end

def invalid_example
    # Set invalid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\inaccessible\data.csv'
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end     
end

def raise_fatal_error
    begin
        # Get the fatal exception object.
        # Loop through all objects in ObjectSpace.
        fatal = ObjectSpace.each_object(Class).find do |klass|
            # Return match of 'fatal' object.
            klass < Exception && klass.inspect == 'fatal'
        end
        # Raise new fatal exception object.
        raise fatal.new('Uh oh, something is seriously broken!')
    rescue fatal => e
        # Try to rescue our fatal exception object.
        Logging.log(e)
    rescue Exception => e
        # Try to rescue all other exceptions.
        Logging.log(e, { explicit: false })        
    end   
end

def execute_examples
    valid_example
    invalid_example
    raise_fatal_error
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

We begin with a couple normal methods with `begin-rescue` blocks.  In these two examples we're trying to raise a `fatal` exception through invalid permission access to a directory:

```ruby
def valid_example
    # Set valid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\accessible\data.csv'    
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end   
end

def invalid_example
    # Set invalid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\inaccessible\data.csv'
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end     
end
```

In this case we have the `accessible` and `inaccessible` sub-directories.  The `accessible` directory has normal read/write permissions for the executing user account, so there's no problem opening the path to `...\accessible\data.csv` and creating a new file with our data.  That `data.csv` file looks as expected:

```
id, first, last
1, Alice, Smith
2, Bob, Turner
```

However, trying to perform the same creation and write operation to `...\inaccessible\data.csv` produces an error because our executing user doesn't have permission:

```
(INEXPLICIT) Errno::EACCES: Permission denied @ rb_sysopen - D:\work\Airbrake.io\Exceptions\Ruby\Fatal\inaccessible\data.csv
```

Unsurprisingly, this isn't a `Fatal Error` at all.  As it happens, it is _very_ difficult to purposefully cause a `Fatal Error` in Ruby code (and for good reason).  However, to see just what the `Fatal Error` exception object is like and to play with it yourself, we can perform a small trick to raise such an error.

```ruby
def raise_fatal_error
    begin
        # Get the fatal exception object.
        # Loop through all objects in ObjectSpace.
        fatal = ObjectSpace.each_object(Class).find do |klass|
            # Return match of 'fatal' object.
            klass < Exception && klass.inspect == 'fatal'
        end
        # Raise new fatal exception object.
        raise fatal.new('Uh oh, something is seriously broken!')
    rescue fatal => e
        # Try to rescue our fatal exception object.
        Logging.log(e)
    rescue Exception => e
        # Try to rescue all other exceptions.
        Logging.log(e, { explicit: false })        
    end   
end
```

Here in the `#raise_fatal_error` method we're looping through all the objects stored within the `ObjectSpace` module.  Specifically, we want to use the [`ObjectSpace::each_object`](https://ruby-doc.org/core-2.4.0/ObjectSpace.html#method-c-each_object) method, which calls a code block that iterates over every living object known to the current Ruby process.  In other words, we can use `ObjectSpace::each_object` to loop through every built-in Ruby object and try to pull out the one we want.  In this case, we want the `fatal` exception object, so we return that object and assign it to our new `fatal` variable.

From there we can issue a standard `raise` call and pass in our own error message.  Just to make sure our bases are covered we try to `rescue` our actual `fatal` object, but also all `Exception` objects as well, as a backup.

As it turns out, since `fatal` is itself an `Exception` type, we're able to `rescue` it directly, as confirmed by the log output:

```
(EXPLICIT) fatal: Uh oh, something is seriously broken!
```


To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A closer look at Fatal Error class in Ruby, including functional code examples and a short exploration of explicitly raising such errors.