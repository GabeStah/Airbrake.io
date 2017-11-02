# The Python Exception Class Hierarchy

The Python exception class hierarchy consists of a few dozen different exceptions spread across a handful of important base class types.  As with most programming languages, errors occur within a Python application when something unexpected goes wrong.  Anything from improper arithmetic and running out of memory to invalid file references and unicode formatting errors may be raised by Python under certain circumstances.

Most of the errors we'll explore in this series are considered `exceptions`, which indicate that these are `non-fatal` errors.  While a `fatal` error will halt execution of the current application, all non-fatal exceptions allow execution to continue.  This allows our code to explicitly catch or `rescue` the exception that has been raised and programmatically react to it in an appropriate manner.

Let's start by looking at the full Python exception class hierarchy, as seen below:

- BaseException
    - Exception
        - ArithmeticError
            - FloatingPointError
            - OverflowError
            - ZeroDivisionError
        - AssertionError
        - AttributeError
        - BufferError
        - EOFError
        - ImportError
            - ModuleNotFoundError
        - LookupError
            - IndexError
            - KeyError
        - MemoryError
        - NameError
            - UnboundLocalError
        - OSError
            - BlockingIOError
            - ChildProcessError
            - ConnectionError
                - BrokenPipeError
                - ConnectionAbortedError
                - ConnectionRefusedError
                - ConnectionResetError
            - FileExistsError
            - FileNotFoundError
            - InterruptedError
            - IsADirectoryError
            - NotADirectoryError
            - PermissionError
            - ProcessLookupError
            - TimeoutError
        - ReferenceError
        - RuntimeError
            - NotImplementedError
            - RecursionError
        - StopIteration
        - StopAsyncIteration            
        - SyntaxError
            - IndentationError
                - TabError
        - SystemError
        - TypeError
        - ValueError
            - UnicodeError
                - UnicodeDecodeError
                - UnicodeEncodeError
                - UnicodeTranslateError
        - Warning
            - BytesWarning
            - DeprecationWarning
            - FutureWarning
            - ImportWarning
            - PendingDeprecationWarning
            - ResourceWarning
            - RuntimeWarning
            - SyntaxWarning
            - UnicodeWarning
            - UserWarning            
    - GeneratorExit
    - KeyboardInterrupt
    - SystemExit

As we publish future, exception-specific articles in this series we'll update the full list above to relevant tutorial and article links for each exception, so this post can act as a go-to resource for Python exception handling tips.

## Major Exception Types Overview

Next, let's briefly discuss each important top-level exception type.  These top-level exceptions will serve as a basis for digging into specific exceptions in future articles.  Before we do that, however, it's worth pointing out what might appear as a slight discrepancy when looking over the list of exception classes provided in Python.  To illustrate, look closely at this small snippet of the Python exception class hierarchy and see if anything slightly strange pops out to you:

- BaseException
    - Exception
        - ArithmeticError
            - FloatingPointError
            - OverflowError
            - ZeroDivisionError
        - AssertionError

For developers that have worked with other programming languages in the past, what you might take note of is the distinction between using the word `exception` in the `BaseException` and `Exception` parent classes, and the use of `error` in most subclasses therein.  Most other languages, such as .NET or Java, explicitly differentiate between `exceptions` and `errors` by separating them into distinct categories.  In such languages, `errors` typically denote `fatal` errors (those that crash the application), whereas `exceptions` are catchable/rescuable errors.

Yet, as we see in the hierarchy above, Python merely inherits from `Exception` with a series of `XYZError` classes.  The reason for this naming convention comes from the [`PEP8`](https://www.python.org/dev/peps/pep-0008/#exception-names) Python style guide, which makes an explicit mention that "you should use the suffix 'Error' on your exception names (_if the exception is actually an error_)."  I've added the extra emphasis to that quote, because the latter point is critical here -- most Python exceptions with `Error` in the name are, in fact, _errors_.

## BaseException

The `BaseException` class is, as the name suggests, the base class for all built-in exceptions in Python.  Typically, this exception is never raised on its own, and should instead be inherited by other, lesser exception classes that _can_ be raised.

The `BaseException` class (and, thus, all subclass exceptions as well) allows a `tuple` of arguments to be passed when creating a new instance of the class.  In most cases, a single argument will be passed to an exception, which is a string value indicating the specific error message.

This class also includes a `with_traceback(tb)` method, which explicitly sets the new traceback information to the `tb` argument that was passed to it.

### Exception

`Exception` is the most commonly-inherited exception type (outside of the true base class of `BaseException`).  In addition, all exception classes that are considered errors are subclasses of the `Exception` class.  In general, any custom exception class you create in your own code should inherit from `Exception`.

The `Exception` class contains many direct child subclasses that handle most Python errors, so we'll briefly go over each below:

- `ArithmeticError` - The base class for the variety of arithmetic errors, such as when attempting to divide by zero, or when an arithmetic result would be too large for Python to accurately represent.
- `AssertionError` - This error is raised when a call to the [`assert`] statement fails.
- `AttributeError` - Python's syntax includes something called [`attribute references`](https://docs.python.org/3/reference/expressions.html#attribute-references), which is just the Python way of describing what you might know of as `dot notation`.  In essence, any `primary token` in Python (like an `identifier`, `literal`, and so forth) can be written and followed by a period (`.`), which is then followed by an `identifier`.  That syntax (i.e. `primary.identifier`) is called an `attribute reference`, and anytime the executing script encounters an error in such syntax an `AttributeError` is raised.
- `BufferError` - Python allows applications to access low level memory streams in the form of a [`buffer`](https://docs.python.org/3/c-api/buffer.html#bufferobjects).  For example, the [`bytes`](https://docs.python.org/3/library/stdtypes.html#bytes) class can be used to directly work with bytes of information via a memory buffer.  When something goes wrong within such a buffer operation a `BufferError` is raised.
- `EOFError` - Similar to the [Java EOFException](https://airbrake.io/blog/java-exception-handling/eofexception) article we did a few days ago, Python's `EOFError` is raised when using the [`input()`](https://docs.python.org/3/library/functions.html#input) function and reaching the end of a file without any data.
- `ImportError` - Modules are usually loaded in memory for Python scripts to use via the [`import`](https://docs.python.org/3/reference/simple_stmts.html#import) statement (e.g. `import car from vehicles`).  However, if an `import` attempt fails an `ImportError` will often be raised.
- `LookupError` - Like `ArithmeticError`, the `LookupError` is generally considered a base class from which other subclasses should inherit.  All `LookupError` subclasses deal with improper calls to a collection are made by using invalid `key` or `index` values.
- `MemoryError` - In the event that your Python application is _about_ to run out of memory a `MemoryError` will be raised.  Since Python is smart enough to detect this potential issue _slightly_ before all memory is used up, a `MemoryError` can be `rescued` and allow you to recover from the situation by performing garbage collection of some kind.
- `NameError` - Raised when trying to use an `identifier` with an invalid or unknown name.
- `OSError` - This error is raised when a system-level problem occurs, such as failing to find a local file on disk or running out of disk space entirely.  `OSError` is a parent class to many subclasses explicitly used for certain issues related to operating system failure, so we'll explore those in future publications.
- `ReferenceError` - Python includes the [weakref](https://docs.python.org/3/library/weakref.html#module-weakref) module, which allows Python code to create a specific type of reference known as a `weak reference`.  A `weak reference` is a reference that is not "strong" enough to keep the referenced object alive.  This means that the next cycle of garbage collection will identify the weakly referenced object as no longer strongly referenced by another object, causing the weakly referenced object to be destroyed to free up resources.  If a `weak reference` proxy created via the [`weakref.proxy()`](https://docs.python.org/3/library/weakref.html#weakref.proxy) function is used _after_ the object that is referenced has already been destroyed via garbage collection, a `ReferenceError` will be raised.
- `RuntimeError` - A `RuntimeError` is typically used as a catchall for when an error occurs that doesn't really fit into any other specific error classification.
- `StopIteration` - If no `default` value is passed to the [`next()`](https://docs.python.org/3/library/functions.html#next) function when iterating over a collection, _and_ that collection has no more iterated value to retrieve, a `StopIteration` exception is raised.  Note that this is _not_ classified as an `Error`, since it doesn't mean that an error has occurred.
- `StopAsyncIteration` - As of version 3.5, Python now includes coroutines for asynchronous transactions using the [`async` and `await` syntax](https://www.python.org/dev/peps/pep-0492/).  As part of this feature, collections can be asynchronously iterated using the [`__anext__()`](https://docs.python.org/3/reference/datamodel.html#object.__anext__) method.  The `__anext__()` method requires that a `StopAsyncIteration` instance be raised in order to halt async iteration.
- `SyntaxError` - Just like most programming languages, a `SyntaxError` in Python indicates that there is some improper syntax somewhere in your script file.  A `SyntaxError` can be raised directly from an executing script, or produced via functions like [`eval()`](https://docs.python.org/3/library/functions.html#eval) and [`exec()`](https://docs.python.org/3/library/functions.html#exec).
- `SystemError` - A generic error that is raised when something goes wrong with the Python interpreter (not to be confused with the `OSError`, which handles operating system issues).
- `TypeError` - This error is raised when attempting to perform an operation on an incorrect object type.
- `ValueError` - Should be raised when a function or method receives an argument of the correct type, _but_ with an actual value that is invalid for some reason. 
- `Warning` - Another parent class to many subclasses, the `Warning` class is used to alert the user in non-dire situations.  There are a number of subclass warnings that we'll explore in future articles.

#### GeneratorExit

A `generator` is a specific type of iterator in Python, which simplifies the process of creating iterators with constantly changing values.  By using the `yield` statement within a generator code block, Python will return or "generate" a new value for each call to `next()`.  When the explicit [`generator.close()`](https://docs.python.org/3/reference/expressions.html#generator.close) method is called a `GeneratorExit` instance is raised.

#### KeyboardInterrupt

This simple exception is raised when the user presses a key combination that causes an `interrupt` to the executing script.  For example, many terminals accept `Ctrl+C` as an interrupt keystroke.

#### SystemExit

Finally, the `SystemExit` exception is raised when calling the [`sys.exit()`](https://docs.python.org/3/library/sys.html#sys.exit) method, which explicitly closes down the executing script and exits Python.  Since this is an exception, it can be `rescued` and programmatically responded to immediately before the script actually shuts down.

---

That's just a small taste of the powerful, built-in Python exception class hierarchy as of version 3.6.  Stay tuned for more in-depth articles examining each of these exceptions in greater detail, and be sure to check out Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-hierarchy">error monitoring software</a>, which provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-hierarchy">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A short overview of the Python exception class hierarchy, including a quick look at all the top-level exception classes in the standard library.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html