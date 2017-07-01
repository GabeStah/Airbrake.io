Ruby features a plethora of built-in classes to dramatically simplify exception handling and management.  At the top-most level of the exception class hierarchy resides the `Exception` class, the monolithic parent class of over a half-dozen subclasses which typically regulate the grunt work of Ruby exception handling.

Below is the entire list of built-in subclasses of the `Exception` class:

- [`NoMemoryError`](https://airbrake.io/blog/ruby-exception-handling/nomemoryerror)
- [`ScriptError`](https://airbrake.io/blog/ruby-exception-handling/scripterror)
  - [`LoadError`](https://airbrake.io/blog/ruby-exception-handling/loaderror)
  - [`NotImplementedError`](https://airbrake.io/blog/ruby-exception-handling/notimplementederror)
  - [`SyntaxError`](https://airbrake.io/blog/ruby-exception-handling/syntaxerror)
- [`SecurityError`](https://airbrake.io/blog/ruby-exception-handling/securityerror)
- [`SignalException`](https://airbrake.io/blog/ruby-exception-handling/signalexception)
  - [`Interrupt`](https://airbrake.io/blog/ruby-exception-handling/interrupt)
- [`StandardError`](https://airbrake.io/blog/ruby-exception-handling/standarderror)
  - [`ArgumentError`](https://airbrake.io/blog/ruby-exception-handling/argumenterror)
    - [`UncaughtThrowError`](https://airbrake.io/blog/ruby-exception-handling/uncaughtthrowerror)
  - `EncodingError`
    - [`CompatibilityError`](https://airbrake.io/blog/ruby-exception-handling/compatibilityerror)
    - [`ConverterNotFoundError`](https://airbrake.io/blog/ruby-exception-handling/converternotfounderror)
    - [`InvalidByteSequenceError`](https://airbrake.io/blog/ruby-exception-handling/invalidbytesequenceerror)
    - [`UndefinedConversionError`](https://airbrake.io/blog/ruby-exception-handling/undefinedconversionerror)
  - [`FiberError`](https://airbrake.io/blog/ruby-exception-handling/fibererror)
  - `IOError`
    - [`EOFError`](https://airbrake.io/blog/ruby-exception-handling/eof-error)
  - `IndexError`
    - [`KeyError`](https://airbrake.io/blog/ruby-exception-handling/keyerror)
    - [`StopIteration`](https://airbrake.io/blog/ruby-exception-handling/stopiteration)
  - [`LocalJumpError`](https://airbrake.io/blog/ruby/ruby-exception-handling-localjumperror)
  - [`NameError`](https://airbrake.io/blog/ruby/ruby-exception-handling-nameerror)
    - [`NoMethodError`](https://airbrake.io/blog/ruby-exception-handling/nomethoderror)
  - [`RangeError`](https://airbrake.io/blog/ruby/ruby-exception-handling-rangeerror)
    - [`FloatDomainError`](https://airbrake.io/blog/ruby/ruby-exception-floatdomainerror)
  - [`RegexpError`](https://airbrake.io/blog/ruby/ruby-exception-handling-regexperror)
  - [`RuntimeError`](https://airbrake.io/blog/ruby/ruby-exception-handling-runtimeerror)
  - [`SystemCallError`](https://airbrake.io/blog/ruby/ruby-exception-handling-systemcallerror)
  - [`ThreadError`](https://airbrake.io/blog/ruby/ruby-exception-handling-threaderror)
  - [`TypeError`](https://airbrake.io/blog/ruby/ruby-exception-handling-typeerror)
  - [`ZeroDivisionError`](https://airbrake.io/blog/ruby/ruby-exception-handling-zerodivisionerror)
- [`SystemExit`](https://airbrake.io/blog/ruby/ruby-exception-handling-systemexit)
- [`SystemStackError`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-handling-systemstackerror)
- [`fatal – impossible to rescue`](https://airbrake.io/blog/ruby-exception-handling/fatal-error)

Below we'll briefly examine each of the top-level subclasses of `Exception` to easily define when and why they might be raised during normal execution.

## NoMemoryError

`NoMemoryError` is raised when memory allocation fails and the application must halt.

## ScriptError

`ScriptError` is a superclass which is raised when a runtime error occurs which is caused by failure of the script to execute as expected.  This may be due to invalid `required` file path or simple syntax errors.  Subclasses of `ScriptError` include `LoadError`, `NotImplementedError`, and `SyntaxError`.

## SecurityError

`SecurityError` is raised when execution attempts a potentially unsafe operation.  This most commonly occurs when the `$SAFE` variable is raised above the default level of `0`, which informs Ruby that it can execute potentially insecure code.

For example, the following code raises a `SecurityError` exception, due to calling the insecure `untaint` operation:

```ruby
name = "John Doe"
proc = Proc.new do
  $SAFE = 4
  name.untaint
end
proc.call
```

## SignalException

`SignalException` is raised when a signal is received upon a running process and contains only one subclass, `Interrupt`.  A `SignalException` can be captured with something like the following snippet:

```ruby
pid = fork do
   Signal.trap('HUP') { puts "trapped"; exit }
end

begin
  Process.kill('HUP', pid)
  sleep
rescue SignalException => e
  puts "Received Exception #{e}"
end
```

## StandardError

`StandardError`, as the name implies, is the most common or standard type of exception Ruby will raise.  While most applications will attempt to capture or raise a specific subclass of `StandardError` (such as `ArgumentError`, `EncodingError`, etc), `StandardError` itself will be raised when a rescue clause is executed _without_ any explicit `Exception` class specified:

```ruby
def test
  raise "Error"
end
test rescue "Hello"   #=> "Hello"
```

## SystemExit

`SystemExit` is raised when the [`exit`](http://ruby-doc.org/core-2.3.2/Kernel.html#method-i-exit) method is called.  For example:

```ruby
begin
  exit
  puts "Unreachable"
rescue SystemExit => e
  puts "Received Exception #{e}"
end
puts "ADDITIONAL"
```

__Output__:

```
Received Exception SystemExit
ADDITIONAL
```

## SystemStackError

`SystemStackError` is raised when a stack overflow occurs.  This most commonly occurs when an infinite loop is generated.  For example, running the following:

```ruby
def increment(v)
    increment(v+1)
end
increment(1)
```

__Output__: `SystemStackError: stack level too deep`.

## fatal

`fatal` exceptions are raised when Ruby encounters a fatal error that requires it to exit.  Unlike most other `Exception` subclasses, `fatal` errors cannot be rescued.

---

__SOURCES__

- https://ruby-doc.org/core-2.3.2/Exception.html
- https://ruby-hacking-guide.github.io/security.html
