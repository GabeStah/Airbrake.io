Chugging right along through our __Ruby Exception Handling__ series, our next stop takes us to the `LoadError` exception class.  `LoadError` is a subclass descendant of the `ScriptError` superclass, and occurs when Ruby attempts to load a file (via `require` or otherwise) that simply doesn't exist.

In this post we'll examine `LoadError` in a bit more detail, looking at where this exception class sits in Ruby's `Exception` class hierarchy, how to handle `LoadErrors`, and best practices to avoid this error from popping up in the first place!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `ScriptError` is a direct descendant of the [`Exception`] class.
- `LoadError` is a direct descendant of the `ScriptError` class.

## When Should You Use It?

Since `LoadError` is a low-level `Exception` subclass, when Ruby raises an error that matches the requirement of a loading error, a new instance of `LoadError` is created to be examined and manipulated.

To see `LoadError` in action, we'll begin with a simple code snippet where we're attempting to load a file by using an invalid file path that doesn't exist:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    require 'invalid/file/path'
rescue LoadError => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

The `print_exception` function simply takes the passed in `Exception` class argument that was `rescued` by Ruby, and outputs some useful information about it: class name, error message, stack trace, and whether the incoming exception class was explicitly expected or not.

The meat of our example occurs in `require 'invalid/file/path'`, which as the name implies, is a path to an invalid file.  When this executes, we get the following output from the `print_exception` function:

```
[EXPLICIT] LoadError: cannot load such file -- invalid/file/path
G:/dev/programs/Ruby22-x64/lib/ruby/2.2.0/rubygems/core_ext/kernel_require.rb:54:in `require'
G:/dev/programs/Ruby22-x64/lib/ruby/2.2.0/rubygems/core_ext/kernel_require.rb:54:in `require'
loaderror.rb:7:in `<main>'
```

Sure enough, we find that our explicit `rescue` call for the `LoadError` class was executed and passed along, so we see that an `[EXPLICIT] LoadError` has occurred, indicating the file at the path we specified cannot be loaded.

A `LoadError` can also occur as a result of other types of direct file loading besides `require`, such as `load`.  Here we use the same example as above, but try using `load` instead:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    load 'invalid/file/path'
rescue LoadError => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end
```

The resulting output is, as expected, another `LoadError` occurrence due to our invalid file path:

```
[EXPLICIT] LoadError: cannot load such file -- invalid/file/path
loaderror2.rb:7:in `load'
loaderror2.rb:7:in `<main>'
```

If we take out our explicit `rescue LoadError` clause and run the code again, we can capture the `LoadError` using the generic `Exception` class:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    load 'invalid/file/path'
rescue Exception => e
    print_exception(e, false)
end
```

While we didn't explicitly `rescue` a `LoadError` class, our output still grabs the `LoadError` exception that was created:

```
[INEXPLICIT] LoadError: cannot load such file -- invalid/file/path
loaderror3.rb:7:in `load'
loaderror3.rb:7:in `<main>'
```

This behavior is because of the inheritance structure of all the classes we've discussed.  `LoadError` is a subclass of `ScriptError`, which is in turn a subclass of `Exception`.  Since we told Ruby to explicitly `rescue` an `Exception` in this case, we've ensured that Ruby will trigger our `rescue` clause if any `Exception`, or descendant thereof, is raised.

On the other hand, it's also important to note that since `LoadError` is not a descendant of `StandardError`, which is the default exception type Ruby creates for `rescue` clauses without explicit class specification, we __cannot__ expect a non-explicit `rescue` clause to trigger when a `LoadError` is raised.

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    load 'invalid/file/path'
rescue => e
    print_exception(e, false)
end
```

This produces a _generic error_ output which doesn't utilize our non-explicit `rescue` clause at all:

```
loaderror4.rb:7:in `load': cannot load such file -- invalid/file/path (LoadError)
        from loaderror4.rb:7:in `<main>'
```


[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/LoadError.html
- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/Kernel.html
