Today as we continue our journey through the wide world of __Ruby Exception Handling__, we're going to examine the `Interrupt` exception class. `Interrupt` is a subclass descendant of the `Exception` superclass, and occurs when a specific interrupt signal is received by the active Ruby process, such as when the user manually halts via `Ctrl-C`.

In this article we'll examine the `Interrupt` class in a bit more detail, looking at where it sits within Ruby's `Exception` class hierarchy, how to handle `Interrupt` errors, and a few tips for avoid `Interrupts` exceptions entirely.  Let's get to work!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `SignalException` is a direct descendant of the [`Exception`] class.
- `Interrupt` is a direct descendant of the `SignalException` class.

To get the most out of your own applications and to fully manage any and all [`Ruby Exceptions`], check out the blazing fast [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

# When Should You Use It?

Since `Interrupt` is a descendant of the `SignalException` class, it should come as no surprise that `Interrupt` is just a response to a specific [`signal`] being sent to the active process, and caught by Ruby.  Let's briefly discuss signals to get a refresher on what they are and how they work.

A [`signal`] is a command that can be sent directly to a running process within many operating systems.  Typically, a specific signal will be a short string identifier, each with a unique purpose or intended behavior.  For example, the `TERM` signal is short for `termination` and is intended to terminate the running process it is sent to: `Process.kill('TERM', Process.pid)`.

If you're unsure of what signals are available to your system, simply call the `Signal.list` method in Ruby for a complete list.

Now in most cases, the `Interrupt` exception will be raised via a manual interruption command, when the user presses `Ctrl-C` to halt the active process.  This can be seen in practice with the simple example below:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
    end
rescue Interrupt => e
    print_exception(e, true)
rescue SignalException => e
    print_exception(e, false)
rescue Exception => e
    print_exception(e, false)
end
```

We've created an indefinite loop which pauses every second and outputs the current iteration of our `count` value.  Obviously this would eventually cause a crash if execution was never halted, but for our purposes it works well enough to give us an active process we can manually kill.

After executing and seeing the output number climb for a few seconds, press `Ctrl-C` and we see our expected `Interrupt` exception was raised and rescued:

```
1
2
3
[EXPLICIT] Interrupt:
code.rb:13:in `sleep'
code.rb:13:in `<main>'
```

Additionally, since `Interrupt` descends from the `SignalException` class, we can remove our explicit `rescue` clause for `Interrupt` and just rescue any `SignalException`, which will also catch our manual interrupt in the same way:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
    end
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

The output:

```
1
2
3
4
[EXPLICIT] Interrupt:
code.rb:35:in `sleep'
code.rb:35:in `<main>'
```

While manual interruption is useful, it's important to recognize that the common `Ctrl-C` command to manually interrupt the active process is really just a shortcut way of sending the `INT` signal to that process, telling it to interrupt.

Therefore, just as with the `TERM` signal or many others, we can programmatically send the `INT` signal to our active process through Ruby code itself and raise an `Interrupt` exception in much the same way as before.  Here we're using the same loop, but once the `count` reaches `5`, we send our `INT` signal:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
        if count >= 5 then
            Process.kill('INT', Process.pid)
        end
    end
rescue Interrupt => e
    print_exception(e, true)
rescue SignalException => e
    print_exception(e, false)
rescue Exception => e
    print_exception(e, false)
end
```

This produces similar output to the other manual interrupt methods, since from the perspective of both Ruby and the operating system, these `INT` signal methods are identical:

```
1
2
3
4
5
[EXPLICIT] Interrupt:
code.rb:57:in `kill'
code.rb:57:in `<main>'
```

[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html
[`Ruby Exceptions`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`signal`]: https://ruby-doc.org/core-2.3.3/Signal.html

--------------------------------------------------------------------------------

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
