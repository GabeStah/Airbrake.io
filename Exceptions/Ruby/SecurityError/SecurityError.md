As we plow ahead through our adventure with the __Ruby Exception Handling__ series, today we're examining the `SecurityError` exception class, which is raised anytime Ruby attempts a potentially unsafe operation.

Throughout this post we'll examine what can cause a `SecurityError` and how to configure your application to avoid them wherever possible, so let's get started!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `SecurityError` is a direct descendant of the [`Exception`] class.
- Unlike most other direct descendants of [`Exception`], `SecurityError` is not a superclass, and thus has no descendants of its own.

## When Should You Use It?

To understand what can cause a `SecurityError`, we must first examine code execution security and the concept of [`taint`](https://en.wikipedia.org/wiki/Taint_checking) within Ruby.

The idea behind `taint` is simple: When executing Ruby code, it is not always possible to determine where every object comes from.  Anytime a third-party library or resource is involved, or even any form of user input, Ruby could become insecure.  Objects generated or altered from these outside sources present potential security risks, since Ruby has no way of knowing what _exactly_ occurred during those outside processes.

Thus, a `tainted` object is any object in Ruby that has been altered by one of these outside sources.  This causes that object to be flagged as `tainted`, a boolean value that can easily be checked at any time using the [`tainted?`](http://ruby-doc.org/core-2.3.3/Object.html#method-i-tainted-3F) method.

For example, here we create a simple string object, `name`, and assign it to the value `Bob`.  We then output whether or not it is `tainted`, directly `taint` it ourselves, then evaluate it a second time:

```ruby
name = "Bob"
puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
puts "Tainting poor, old #{name}."
name.taint
puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
```

The output shows us that, initially, `name` is not `tainted`, but once we explicitly `taint` it ourselves, it _is_ `tainted` thereafter:

```
Is Bob tainted? No
Tainting poor, old Bob.
Is Bob tainted? Yes
```

So, why is `taint` important to understand `SecurityErrors`?  The answer is because of Ruby's global `$SAFE` variable, which is used during execution to determine how Ruby should behave when dealing with potentially unsafe code.  There are effectively `5` possible `$SAFE` levels:

| `$SAFE` Level | Effect |
| --- | --- |
| 0 | __[DEFAULT]__ Ruby implements no security checks for `tainted` objects. |
| 1 | Ruby prohibits `tainted` object use in potentially dangerous operations. |
| 2 | Ruby prohibits loading files from globally writable locations. |
| 3 | All created objects are automatically `tainted`. |
| 4 | All `non-tainted` objects are "locked" and may not be modified. |

Check out [this table](http://phrogz.net/programmingruby/taint.html#table_20.1) for a more in-depth specifics of `$SAFE` levels.

Since `$SAFE` defaults to the `0` level, Ruby typically does not care if your code attempts to utilize a `tainted` object, as we saw in our example above with `Bob`.

However, we can _force_ Ruby to consider security levels and potentially `tainted` objects by increasing our `$SAFE` value above the default value of `0`.  However, as outlined above, Ruby won't consider our `tainted` value improper unless we try to utilize a `"potentially dangerous operation"`.  

One obvious example of a dangerous operation is the `eval()` method.  In the following code we've now raised the `$SAFE` level to `1` and taken our original `puts()` calls and placed them inside an `eval()` call, thereby causing Ruby to recognize the potential security risk of this code:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    $SAFE = 1
    name = "Bob"
    puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    puts "Tainting poor, old #{name}."
    name.taint
    eval %{
        puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    }
rescue SecurityError => e
    print_exception(e, true)
end
```

```
Is Bob tainted? No
Tainting poor, old Bob.
[EXPLICIT] SecurityError: Insecure operation - eval
```

If we raise the `$SAFE` level even further, such as level `3` or higher, now all newly created objects are inherently `tainted`.  This means that simply by creating our new `name` object and attempting to access it through `eval()`, we produce a `SecurityError`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    $SAFE = 3
    name = "Bob"
    eval %{
        puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    }
rescue SecurityError => e
    print_exception(e, true)
end
```

Sure enough, this generates our expected output:

```
[EXPLICIT] SecurityError: Insecure operation - eval
```

[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`gets`]: https://ruby-doc.org/core-2.3.3/Kernel.html#method-i-gets

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/SecurityError.html
- http://phrogz.net/programmingruby/taint.html
- http://phrogz.net/programmingruby/taint.html#table_20.1
