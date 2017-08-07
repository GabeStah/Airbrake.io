# The Java Exception Class Hierarchy

All objects within the Java exception class hierarchy extend from the `Throwable` superclass.  Only instances of `Throwable` (or an inherited subclass) are indirectly thrown by the Java Virtual Machine (JVM), or can be directly thrown via a `throw` statement.  Additionally, only `Throwables` (or an inherited subclass) can be caught via a `catch` statement.

A `Throwable` instance contains the current execution stack, captured when the error exception occurred.  It can also contain a message (obtained via the `getMessage()` method), indicating the relevant error message.  Lastly, for exception chains where one error causes another error to be thrown, a `Throwable` can obtain a potential _cause_ (collected via the `getCause()` method) as well.

In this article we'll dig deeper into the Java exception class hierarchy and see how Java extends beyond the `Throwable` superclass to provide the dozens of built-in error and exception classes that may be thrown by the JVM during execution.  We'll also update this article in the future as new, detailed exception-specific posts are published, so the full hierarchy list will provide easy navigation between the detailed error-focused entries.

## The Hierarchy

As mentioned, every possible built-in exception class extends directly (or subclasses) from the `Throwable` superclass.  The full Java exception class hierarchy can be found below:

- Throwable
    - Error
        - AssertionError
        - LinkageError
            - BootstrapMethodError
            - ClassCircularityError
            - ClassFormatError
                - UnsupportedClassVersionError
            - ExceptionInInitializerError
            - IncompatibleClassChangeError
                - AbstractMethodError
                - IllegalAccessError
                - InstantiationError
                - NoSuchFieldError
                - NoSuchMethodError
            - [NoClassDefFoundError](https://airbrake.io/blog/java-exception-handling/noclassdeffounderror)
            - UnsatisfiedLinkError
            - VerifyError
        - ThreadDeath
        - VirtualMachineError
            - InternalError
            - OutOfMemoryError
            - StackOverflowError
            - UnknownError
    - Exception
        - CloneNotSupportedException
        - InterruptedException
        - ReflectiveOperationException
            - ClassNotFoundException
            - IllegalAccessException
            - InstantiationException
            - NoSuchFieldException
            - NoSuchMethodException
        - RuntimeException
            - ArithmeticException
            - ArrayStoreException
            - ClassCastException
            - EnumConstantNotPresentException
            - [IllegalArgumentException](https://airbrake.io/blog/java-exception-handling/illegalargumentexception)
                - IllegalThreadStateException
                - NumberFormatException
            - IllegalMonitorStateException
            - IllegalStateException
            - IndexOutOfBoundsException
                - [ArrayIndexOutOfBoundsException](https://airbrake.io/blog/java-exception-handling/arrayindexoutofboundsexception)
                - StringIndexOutOfBoundsException
            - NegativeArraySizeException
            - [NullPointerException](https://airbrake.io/blog/java-exception-handling/nullpointerexception)
            - SecurityException
            - TypeNotPresentException
            - UnsupportedOperationException

## Errors vs Exceptions

As with PHP and other prominent languages, the Java exception class hierarchy was built around two distinct categories: `Errors` and `Exceptions`.  

According to the official documentation, an `Error` "indicates serious problems that a reasonable application should not try to catch."  This category includes things like `AssertionErrors` and `ThreadDeath`, which should give an idea of the severity of these types of errors.  It is generally considered good practice not to explicitly catch `Error` classes in code, since they should be dealt with through a change in the application architecture or refactoring, rather than catching (and likely ignoring) them.

On the other hand, `Exceptions` indicate "conditions that a reasonable application might want to catch."  These are more typical errors that occur from time to time in most applications, particularly during development.  Stuff like `ArithmeticExceptions` and `IllegalArgumentException` are found in the `Exceptions` subclass category.

## Overview of Major Subclasses

Below we'll briefly discuss each high-level subclass that extends (or inherits from) `Error` and `Exception`.  The goal here is just to give a quick overview of these categories, as much more detailed looks into specific exceptions will come in future articles.

### Error Descendants

- `AssertionError` - Thrown when an assertion has failed.
- `LinkageError` - Thrown when a class dependency has some form of incompatibility, due to changes made after compilation.
- `ThreadDeath` - Thrown when the (now deprecated) `Thread.stop()` method is invoked.
- `VirtualMachineError` - Thrown when something goes wrong with the Java Virtual Machine, such as running out of resources.

### Exception Descendants

- `CloneNotSupportedException` - Thrown when attempting to `clone` an object of a class that doesn't implement the `Cloneable` interface.
- `InterruptedException` - Thrown when a thread is active, but is somehow interrupted in the process.  
- `ReflectiveOperationException` - Thrown when attempting to perform an invalid reflection operation, such as loading a class that doesn't exist or calling a method that cannot be found.
- `RuntimeException` - Thrown during normal execution of the application and operation of the Java Virtual Machine.  This category includes the most common exceptions such as `ArithmeticException` and `IndexOutOfBoundsException`.

---

That's just a brief overview of the built-in Java exception class hierarchy, but stay tuned for more in-depth articles looking at each of these exceptions in finer detail.  Also, be sure to check out Airbrake's powerful <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Java Error Monitoring</a> software, designed to help you and your team quickly and easily handle all Java exceptions!

---

__META DESCRIPTION__

A quick overview of the Java exception class hierarchy, including the difference between the Error and Exception superclasses.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/package-tree.html