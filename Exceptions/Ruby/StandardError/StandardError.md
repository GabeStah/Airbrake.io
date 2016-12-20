This is an exciting day in the life of our __Ruby Exception Handling__ series, as we tackle the big daddy of all Ruby exceptions, the `StandardError`.  `StandardError` is a superclass with many exception subclasses of its own, but like all errors, it descends from the `Exception` superclass.  `StandardErrors` occur anytime a `rescue` clause catches an exception __without__ an explicit `Exception` class specified.

In this post we'll take a closer look at the `StandardError` class, examining where it lands within Ruby's `Exception` class hierarchy and how to handle `StandardErrors`.  Let's get this party started!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `StandardError` is a direct descendant of the [`Exception`] class, and is also a superclass with many descendants of its own.

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the blazing fast [`Airbrake Ruby`] exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

# When Should You Use It?

Since `StandardError` is the parent to just about every fundamental, typical exception that is likely to be raised in normal Ruby execution, it's critical to understand that the `StandardError` class itself is never actually passed along within a `rescue` code block.  Instead, it simply behaves as the default exception class when no explicit class is provided for any given `rescue` clause.  This behavior is particularly beneficial when your code attempts to account for (and `rescue`) a wide range of expected, explicit exception classes, but you must also include a `rescue` clause for anything unexpected.

For example, here we have a simple example snippet that attempts to `rescue` a number of explicit exception classes, such as `IndexError` and `NameError`, that we might expect to pop up.  However, as a final resort, we're also specifying a `rescue` clause at the end with no explicit exception class specified.  This final `rescue` clause defaults to using `StandardError`, which means it will catch any and all exception classes that are descendants of `StandardError` that _were not_ already `rescued` in a previous clause.

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    raise "Uh oh!"
rescue IndexError => e
    print_exception(e, true)
rescue NameError => e
    print_exception(e, true)
rescue RegexpError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

In this case, because we're simply raising a made up exception (`"Uh oh!"`), we expect none of our explicit exception classes to match, and instead for our backup `StandardError` exception to catch it, as the default.  Once it does, it will then identify which of its own descendants apply to this particular exception, which in the case of an explicit `raise` call, defaults to `RuntimeError`.  Sure enough, this is the resulting output we get:

```
[INEXPLICIT] RuntimeError: Uh oh!
code.rb:8:in `<main>'
```

While this baseline behavior of using `StandardError` as a catchall for unknown exceptions is a very powerful, it's also handy for being the baseline class to extend when creating your own exception classes within your application.

In this example, we've defined a custom `MissingName` exception class, which descends from `StandardError`.  It also initializes with its own `msg` output in case one isn't provided when the exception is raised.

Now within our `begin-rescue` block we once again have a `rescue` clause without an explicit exception class specified, so it will default to `StandardError` and any subsequent descendant class that matches:

```ruby
class MissingName < StandardError
    def initialize(msg="Name is missing!")
        super
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    raise MissingName
rescue => e
    print_exception(e, true)
end
```

In our case, this means our `MissingName` exception class is raised and `rescued` as expected:

```
[EXPLICIT] MissingName: Name is missing!
code.rb:33:in `<main>'
```

The fundamental takeaway when examining `StandardError` is that, unlike `Exception` from which it inherits, `StandardError` simply encompasses all normal, expected exceptions that a typical Ruby application may encounter during execution.  These can all be properly `rescued` and dealt with from within the Ruby application, without any need for a user interference or without any detrimental effects to the underlying system.

Conversely, exceptions outside of the scope of `StandardErrors`, which simply fall elsewhere under the superclass of `Exceptions`, are inherently _system-level_ errors, and thus will typically indicate a non-functional application.

[`Exception`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`Ruby Exceptions`]: https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes
[`Airbrake Ruby`]: https://airbrake.io/languages/ruby_exception_handling
[`StandardError`]: https://ruby-doc.org/core-2.3.3/StandardError.html


--------------------------------------------------------------------------------

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
