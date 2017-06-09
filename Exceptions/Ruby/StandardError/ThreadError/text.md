# Ruby Exception Handling: ThreadError

Making our way through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series we have now arrived at the `ThreadError` class.  `ThreadErrors` are raised anytime an invalid operation is attempted on a thread, plain and (somewhat) simple!

Throughout this article we'll examine the `ThreadError` in a bit more detail, looking at where it sits in the Ruby `Exception` class hierarchy and examining some simple example code that illustrates how `ThreadErrors` are raised so you can (hopefully) avoid them in your own coding adventures.  Let's get this party started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `ThreadError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).

## When Should You Use It?

As previously mentioned a `ThreadError` is the result of trying to perform an invalid operation on a thread in Ruby.  Yet working with threads can be rather confusing so let's start with a brief overview of how we can use threading to handle simultaneous processing in our Ruby code.

The [`Thread`](https://ruby-doc.org/core-2.4.0/Thread.html) class provides a number of helper API methods, but since the purpose of using multiple threads is to perform concurrent actions we'll typically just start by instantiating a new thread with `Thread::new`:

```ruby
thread = Thread.new { puts 'My new thread is terminating.' }
```

Our new thread has a code block that tells it to execute a single statement of `puts` for our string message.  However, merely instantiating a new thread doesn't cause it to execute.  As is normal, when executing any Ruby script we always start (and finish) with the `main` thread.  To get our new sub-thread to execute we need to join it to an active thread.  This can be accomplished with the `#join` method of the sub-thread we want to start.  This suspends the current thread execution (our `main` thread in this case) and starts execution of the sub-thread:

```ruby
thread = Thread.new { puts 'My new thread is terminating.' }
thread.join #=> My new thread is terminating.
```

Since our code block for what the new thread should accomplish is so short we immediately get our message output and then the sub-thread is killed and the main thread resumes.

To expand on this a bit and see how `ThreadErrors` are produced we'll take a look at some slightly more complex code that uses threading.  Here we have a few helper functions and modules, but the main functions we care about are `#get_threads` and `#thread_example`:

```ruby
module Logging
    class << self
        # Outputs the +message+ to console with timestamp.
        # If +timestamp+ is +false+, only +message+ is output.
        def log(message, timestamp=true)
            puts "#{timestamp ? "[#{Time.now.strftime("%T")}] " : nil}#{message}"
        end
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def get_threads
    threads = []
    # Create first thread and add to array.
    threads << Thread.new do
        Logging.log 'Sub-thread 1 is sleeping.'
        # Sleep for 2 seconds.
        sleep(2)
        Logging.log 'Sub-thread 1 is terminating.'
    end
    # Create second thread and add to array.
    threads << Thread.new do
        Logging.log 'Sub-thread 2 is terminating.'
    end
    # Return threads array.
    threads
end

def thread_example
    begin
        Logging.log 'Main thread has started.'
        # Loop through all threads.
        get_threads.each do |thread|
            # Join sub-threads to executing (main) thread.
            thread.join
        end
        Logging.log 'Main thread is terminating.'
    rescue ThreadError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
en
```

`#get_threads` creates a couple new sub-threads places them into an array that is returned.  We also output a few log messages so we know when `sub-thread 1` begins and ends its two-second sleep cycle.  `#thread_example` is triggered by the `main` thread and takes our collection of sub-threads and `#joins` them with the `main` thread.  The expected result is that we should see the `main` thread start, then both our sub-threads terminate, and finally our `main` thread should terminate at the end.  Sure enough, running this code shows us that expected output:

```
[17:43:10] Main thread has started.
[17:43:10] Sub-thread 1 is sleeping.
[17:43:10] Sub-thread 2 is terminating.
[17:43:12] Sub-thread 1 is terminating.
[17:43:12] Main thread is terminating.
```

We didn't really try to do anything fancy with our threads in this example, so everything ran as expected, but what happens if we try to mix things up a bit and perform some actions on our threads that may be invalid or out of order?  For example, the overall setup is the same where we can get a few sub-threads via `#get_threads`, but this time let's try calling the `Thread::stop` method while we're looping through our sub-threads to `#join` them up:

```ruby
def stop_thread_example
    begin
        Logging.log 'Main thread has started.'
        # Loop through all threads.
        get_threads.each do |thread|
            # Join sub-threads to executing (main) thread.
            thread.join
            # Stop execution of the current (main) thread.
            Thread.stop
        end
        Logging.log 'Main thread is terminating.'
    rescue ThreadError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end
```

Suddenly we run into trouble and a `ThreadError` is thrown, indicating that we've attempted to stop the only active thread (our `main` thread):

```
[18:05:26] Main thread has started.
[18:05:26] Sub-thread 1 is sleeping.
[18:05:26] Sub-thread 2 is terminating.
[18:05:28] Sub-thread 1 is terminating.
[EXPLICIT] ThreadError: stopping only thread
	note: use sleep to stop forever
```

Notice that we actually get all the output messages indicating our sub-threads have executed as expected _before_ we get the `ThreadError`.  This is because of the ordering that we're executing everything in.  The `#join` call suspends the main thread temporarily and processes the sub-thread that is being called, so only after both of those terminate does the `Thread::stop` call actually run into trouble, which is indicated by the error message -- we cannot stop a thread when it's the only active thread at the time.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A deep dive into the ThreadError in Ruby including a brief look at threading, along with a few functional code snippets.