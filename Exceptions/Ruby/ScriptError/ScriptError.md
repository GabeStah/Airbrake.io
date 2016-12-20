Today we continue the __Ruby Exception Handling__ series by taking a look at the `ScriptError` exception class.  `ScriptError` is actually a superclass, which means it is inherited by other exception classes, and thus when a `ScriptError` occurs, Ruby determines which of the subclasses is most relevant and raises that error for further examination.

Below we'll see what can cause various types of `ScriptErrors` and how to configure your application to avoid them wherever possible, so let's get started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `ScriptError` is a direct descendant of the [`Exception`] class.
- `ScriptError` is a superclass to the subclasses `LoadError`, `NotImplementedError`, and `SyntaxError`.

## When Should You Use It?

Unlike its subclass descendants, `ScriptError` itself is a direct descendant of the `Exception` class.  `ScriptError` itself will never be directly returned or `rescued` when executing code, but instead Ruby will generate one of the `ScriptError` subclasses that was raised by the error in question.

This is because `ScriptError` itself doesn't directly represent anything; it's best thought of as a template by which all script-related errors are generated from.

As an example, we can raise a `LoadError`, which is a subclass of `ScriptError`, with the following code:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    require 'invalid/file/path'
rescue ScriptError => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

We are covering our bases here and rescuing the explicit `ScriptError` exception class, but also any generic or unexpected `Exception` that may occur in the second `rescue` clause below that.

The output of the above execution where we try to `require` a file path that doesn't exist is the `raising` of a `LoadError`:

```
[EXPLICIT] LoadError: cannot load such file -- invalid/file/path
G:/dev/programs/Ruby22-x64/lib/ruby/2.2.0/rubygems/core_ext/kernel_require.rb:54:in `require'
G:/dev/programs/Ruby22-x64/lib/ruby/2.2.0/rubygems/core_ext/kernel_require.rb:54:in `require'
scripterror.rb:2:in `<main>'
```

Notice that this exception wasn't `INEXPLICIT`, and instead was rescued by explicitly calling the `ScriptError` superclass.  However, the actual exception class itself that was raised (as represented by the `e` variable) was not `ScriptError`, but was instead a subclass of `ScriptError`, the `LoadError` exception.  As discussed earlier, this is because `ScriptError` is a descendant of `Exception`, while `LoadError` (and others) then descend from `ScriptError`, as can be seen [throughout the Ruby source code](https://github.com/ruby/ruby/blob/7ab8dcebbf93aef02602f70eb835b030702c0f4f/error.c#L2053-L2054).

To illustrate that explicitly `rescuing` `ScriptError` works for other exception types, we can also attempt to raise a `SyntaxError` by evaluating syntactically invalid code:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    eval("1+2=3")
rescue ScriptError => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

Once again, our explicit `rescue` of `ScriptError` produces a subclass, in this case a `SyntaxError`:

```
[EXPLICIT] SyntaxError: (eval):1: syntax error, unexpected '=', expecting end-of-input
1+2=3
    ^
scripterror2.rb:7:in `eval'
scripterror2.rb:7:in `<main>'
```

Since `ScriptError` is a descendant of `Exception`, if we completely remove the explicit call to `ScriptError` in the above example, and only leave the `rescue` for the generic `Exception` class, we still get the (`INEXPLICIT`) result, in this case again the `SyntaxError` exception:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    eval("1+2=3")
rescue Exception => e
    print_exception(e, false)
end
```

Output:

```
[INEXPLICIT] SyntaxError: (eval):1: syntax error, unexpected '=', expecting end-of-input
1+2=3
    ^
scripterror3.rb:7:in `eval'
scripterror3.rb:7:in `<main>'
```

One particular caveat here, however, is that since `ScriptError` and its subclasses are not descendants of `StandardError`, which is the default exception type Ruby creates for `rescue` clauses without explicit class specification, we __cannot__ use a non-explicit `rescue` clause and expect any form of `ScriptError` to trigger that code.

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    eval("1+2=3")
rescue => e
    print_exception(e, false)
end
```

Here we're using the previous `eval()` example again, but we've removed all explicit class calls in the `rescue` to either `ScriptError` or even `Exception`.  Watch what happens with the output:

```
scripterror4.rb:7:in `eval': (eval):1: syntax error, unexpected '=', expecting end-of-input (SyntaxError)
1+2=3
    ^
        from scripterror4.rb:7:in `<main>'
```

Our `print_exception` function is __not even called__, and the reason is because the actual exception class that was raised (`SyntaxError`) is not a descendant of `StandardError`, so the non-explicit `rescue => e` clause fails to trigger.

[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/ScriptError.html
- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/File.html#method-c-open
- http://ruby-doc.org/core-2.3.3/String.html
