# The .NET Exception Hierarchy

.NET is among the most robust software frameworks on the market today, yet with that awesome power, comes the responsibility of managing a huge variety of exceptions.  Given that .NET enabled many different languages across all sorts of platforms and devices, .NET must be capable of handling errors that deal with all manner of issues, from web traffic and I/O to operating system problems and database exceptions.  In fact, the latest version of .NET has built-in classes to handle over **`300`** unique exceptions in total!

Managing all these exceptions obviously presents a challenge, so Microsoft implemented a [`namespace`](https://en.wikipedia.org/wiki/Namespace) system within the .NET framework.  A `namespace` is basically just a unique name, which can be assigned to various objects or components, in order to more easily categorize and organize them.

In the case of the .NET framework, `namespaces` are used to provide simple hierarchical structures to the _thousands_ of classes that exist.  For example, the [`System`](https://msdn.microsoft.com/en-us/library/system(v=vs.110).aspx) namespace is the senior-most namespace in the entire framework: every other class and/or namespace in .NET is in some way derived from (a child of) the `System` namespace.

`Namespaces` are then indicated by separating namespaces and classes with a series of chained periods (`.`).  For example, the [`Object`](https://msdn.microsoft.com/en-us/library/system.object(v=vs.110).aspx) class is part of the `System` namespace, so the full inheritance hierarchy is: `System.Object`.

Whew!  With the basics of how .NET uses `namespaces` out of the way, we can dive into the exception class hierarchy.  Since there are hundreds of exceptions in total and an overview of every possible exception namespace is pointless, we'll just examine a few fundamental exception namespaces and their relative hierarchy, as shown in the [`official documentation`](https://msdn.microsoft.com/en-us/library/z4c5tckx(v=vs.110).aspx).

Here's the basic list of .NET exception types, along with their base type that they inherit from:

Exception type | Base type
---------------|----------
Exception | Object
SystemException | Exception
IndexOutOfRangeException | SystemException
NullReferenceException | SystemException
AccessViolationException | SystemException
InvalidOperationException | SystemException
ArgumentException | SystemException
ArgumentNullException | ArgumentException
ArgumentOutOfRangeException | ArgumentException
ExternalException | SystemException
COMException | ExternalException
SEHException | ExternalException

Below we'll examine each of these core exception types provided by .NET, looking at why and when they might be raised during normal execution.

## Exception

The bread and butter of all .NET exceptions, as the name suggests, the `Exception` class is the base class from which all other exceptions inherit.  As with many exception handlers in other programming languages, the `Exception` class provides a number of useful properties that assist with exception handling:

- `StackTrace`: A stack trace to see where exactly the exception occurred.
- `InnerException`: Useful when exceptions chain off one another.  This allows one type of exception to throw another type of exception, ad infinitum.
- `Message`: The detailed message indicating what happened.
- `Data`: A dictionary that can be used to store arbitrary data associated with this particular exception instance.

## SystemException

`SystemException` houses all exceptions related to, well, the system.  These include all runtime-generated errors, such as `System.IO.IOException`, which is thrown when an I/O error occurs.

In short, `SystemException` is the base class that all **non-application** errors inherit from.  If your application code screws up somehow, that's handled by another exception type, but if the system screws up, it's on `SystemException` to sort it out.

## IndexOutOfRangeException

The `IndexOutOfRangeException` is thrown anytime an array or collection is accessed using an index outside its bounds.  

## NullReferenceException

If you attempt to use an object that is considered a `null` object, the `NullReferenceException` will be thrown.

## AccessViolationException

An `AccessViolationException` is thrown when `unmanaged` code attempts to access memory which is unallocated.  For many .NET applications, this will never occur, due to how .NET handles `managed` vs `unmanaged` code.

`Managed` code is code that .NET compiles and executes using the [`common language runtime`](https://msdn.microsoft.com/en-us/library/8bs2ecf4(v=vs.110).aspx).  Conversely, `unmanaged` code compiles into machine code, which **is not** executed within the safety of the `CLR`.

Many languages that rely on .NET, such as `C#` and `Visual Basic`, are **entirely** compiled and executed in the `CLR`.  This means that code written in `C#` is always `managed` code and can, therefore, never throw an `AccessViolationException`.

However, a language like `Visual C++` **does** allow `unmanaged` code to be written.  In such cases, it's entirely possible to access unallocated memory, and throw an `AccessViolationException`.

## InvalidOperationException

`InvalidOperationException` is a pretty cool and somewhat intelligent exception.  It is typically thrown when the state of an `object` cannot support the particular method call being attempted for that object instance.

For example, when the `.MoveNext` method is called on an enumerable object _after_ elements in that collection have been modified.  Since `MoveNext` can no longer determine which object should be next up, it throws an `InvalidOperationException`.

## ArgumentException

As the name implies, `ArgumentException` is thrown when a call is made to a method using an invalid argument.

One interesting note is that since an executing program is not intelligent enough to deduce whether or not a provided argument is valid for the context of the current execution, most of the time an `ArgumentException` is thrown, it's because a developer placed it there intentionally, to be thrown in a particular situation.

For example, if we create a `FullName(string first, string last)` method, we're expecting a first and last name to be passed as arguments, both of which are strings.  However, we may also have other requirements, like neither argument may contain any unexpected punctuation.  We can purposely add logic into our code that validates the arguments that were passed, and if we determine the `first` name provided is invalid, we intentionally throw an `ArgumentException`.  By doing so, this ensures that we (or any other developer that uses our method) know if we're trying to pass invalid arguments to our method.

## ArgumentNullException

`ArgumentNullException` is inherited from `ArgumentException`, but is thrown specifically when a method is called that doesn't allow an argument to be `null`.

## ArgumentOutOfRangeException

Another child of `ArgumentException`, the `ArgumentOutOfRangeException` error is thrown when a method expects argument values within a specified range, yet the provided argument falls outside those bounds. 

## ExternalException

`ExternalException` is the base exception used for any error occurring _externally_, outside the bounds of your own application.  Typically, this means that it's used when there's a problem communicating with a web address, database, and the like.  

## COMException

Examining `ComException` requires a bit of history, so please bear with me.  Prior to the creation of the .NET framework, way back in 1993, Microsoft created and begin using the [`Component Object Model`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms680573(v=vs.85).aspx) (`COM`) binary-interface, which effectively allowed code to be used across languages and environments, in an object-oriented fashion.

`COM` handled exceptions through a property called [`HResult`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms679692(v=vs.85).aspx) (or _result handler_), which is a 32-bit value that combines a few fields together into a simple string.  The encoded `HResult` would tell the application which exception was thrown and the application could respond appropriately.

About a decade after `COM` was introduced, .NET was first released and it quickly began taking over as a much more powerful programming framework, and thus, the usage of `COM` quickly died out.

Unlike `COM`, .NET is far more complex and primarily uses direct object-oriented handling of `Exception` classes for all exception management.  However, in order to provide backward compatibility, .NET provides access to `HResult` through the [`Exception.HResult`](https://msdn.microsoft.com/en-us/library/system.exception.hresult(v=vs.110).aspx) property.  Each exception in .NET is mapped to a distinct `HResult` value.  When `managed` code throws an exception, the `common language runtime` finds the appropriate exception class based on the `HResult` property, allowing `COM` objects to deal with normal, useful .NET exception classes rather than convoluted `HResults`.

In cases where .NET sees an `HResult` that it's unfamiliar with, a `COMException` is thrown.

## SEHException

Also inherited from `ExternalException`, an `SEHException` acts as a sort of "catch-all" for `unmanaged` code exceptions which have not already been mapped to an existing .NET exception class.  `SEH`, in this instance, stands for [`structured exception handling`](https://msdn.microsoft.com/en-us/library/windows/desktop/ms680657(v=vs.85).aspx), which is a mechanism used in .NET for handling both hardware and software exceptions.

Since an `SEHException` requires an exception from `unmanaged` code that has not already been mapped to an existing exception type which might better suit it, throwing of an `SEHException` is rather rare.  

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A broad overview of the .NET Exception Class Hierarchy, including namespaces, exception inheritance, and legacy exception techniques like COM and HResult.

---

__SOURCES__

- https://msdn.microsoft.com/en-us/library/z4c5tckx(v=vs.110).aspx