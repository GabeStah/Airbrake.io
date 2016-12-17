Next on the docket for our __Ruby Exception Handling__ series, we're taking a look at the `SignalException` class. `SignalException` is a subclass descendant of the `Exception` superclass, and occurs when a process monitored by Ruby receives a signal via the operating system.

In this post we'll explore the `SignalException` class, examining where it lands within Ruby's `Exception` class hierarchy, how to handle `SignalException` errors, and a few best practices to avoid this exception from appearing in the first place.  Let's begin our adventure!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `SignalException` is a direct descendant of the [`Exception`] class.

To get the most out of your own applications and to fully manage any and all [`Ruby Exceptions`], check out the blazing fast [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

# When Should You Use It?

As discussed earlier, the `SignalException` class is raised anytime Ruby encounters a received [`signal`] during code execution.  Thus, to properly understand how `SignalExceptions` work, we must first explore a bit about signals, including what they are and what can cause them.

A [`signal`] is simply a command that can be sent directly to a running process within many operating systems.  Typically, a specific signal will be a short string identifier, each with a unique purpose or intended behavior.  For example, the `TERM` signal is short for `termination`, and as you might expect, is intended to terminate the running process it is sent to: `Process.kill('TERM', Process.pid)`.

The specific signals available to you will depend on your particular operating system, but we can easily gather a list of potential candidates using a simple `Signal.list` method within Ruby:

```ruby
Signal.list
```

On Windows 7, this outputs a handful of available signals, including the `TERM` signal mentioned above:

```
{"EXIT"=>0, "INT"=>2, "ILL"=>4, "ABRT"=>22, "FPE"=>8, "KILL"=>9, "SEGV"=>11, "TERM"=>15}
```

Now to send a signal to a particular process, we must identify the correct process using the unique identifier, known as the process ID or `pid`.  Virtually all operating systems use a `pid` system to identify running processes, so even the `Interactive Ruby Shell` (`irb`) that you might be testing simple code with will have its own `pid`.  This can be identified using the `Process.pid` method call:

```ruby
puts "This process has a pid of #{Process.pid}."
```

The output shows what the actual `pid` number is:

```
This process has a pid of 13220.
```

Now with that out of the way, we can tackle the raising of `SignalExceptions` when executing Ruby code.  In this simple example below we're using a standard `begin-rescue` block to catch any potential exceptions.  The crux of the code is the one line where we're calling the `Process.kill` method, passing the `TERM` signal and the currently active process `pid` as the arguments, and attempting to `rescue` the produced `SignalException`, before outputting the resulting exception message using the `print_exception` function:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Sending `TERM` signal to active process
    Process.kill('TERM', Process.pid)
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

Sure enough, this raises the expected `SignalException` and informs us of the type in question (`SIGTERM`):

```
[EXPLICIT] SignalException: SIGTERM
code.rb:8:in `kill'
code.rb:8:in `<main>'
```

It's important to understand that `SignalException` is still a Ruby class, and thus is bound by all the rules that Ruby must abide by during execution, whereas signals and their respective processes, are at a higher level of abstraction since they only abide by the rules of the operating system.  This means that, in some cases, `SignalExceptions` __will not__ be raised because some particular signal types are designed not to be caught or ignored.  For example, the signal `KILL` is similar to `TERM`, except it specifically terminates the process immediately without allowing the process to perform any further actions or cleanup procedures, such as raising errors:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Sending `KILL` signal to active process
    Process.kill('KILL', Process.pid)
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

As expected, executing the code this time with the `KILL` signal sent simply terminates our process immediately, without any further processing or without raising a `SignalException`.

[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html
[`Ruby Exceptions`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`signal`]: https://ruby-doc.org/core-2.3.3/Signal.html

--------------------------------------------------------------------------------

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
