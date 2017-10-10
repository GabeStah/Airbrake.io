# What is a Runtime Error?

A **runtime error** is an application error that occurs during program execution.  `Runtime errors` are usually a _category_ of exception that encompasses a variety of more specific error types such as `logic errors`, `IO errors`, `encoding errors`, `undefined object errors`, `division by zero errors`, and many more.  In this article we'll examine exactly what a `runtime error` is and how it compares to other exception categories.  We'll also look at some basic code samples that will illustrate how to fix `runtime errors`, so let's get started!

## Interpretation vs Compilation

Most programming languages fall into one of two categories, both of which describe how source code is executed by the underlying machine:

- [`Interpretation`](https://en.wikipedia.org/wiki/Interpreted_language) - Interpreted programming languages are directly executed by an [`interpreter`](https://en.wikipedia.org/wiki/Interpreter_(computing)), without altering or transforming the source code prior to execution.  This process reads each statement one at a time, translating each into a sequence of instructions that have already been converted into machine code for rapid execution.  A few of the well-known interpreted languages are `PHP`, `JavaScript`, and `Perl`.
- [`Compilation`](https://en.wikipedia.org/wiki/Compiled_language) - Upon executing the application, compiled programming languages first pass application source code through a [`compiler`](https://en.wikipedia.org/wiki/Compiler), which transforms the source code into a more efficient form of machine code.  This compiled machine code is often referred to as [`bytecode`](https://en.wikipedia.org/wiki/Bytecode).  The machine code is then passed along to an `interpreter`, which executes each instruction one by one, just as with an explicitly `interpreted language`.  Some popular compiled languages are `C` and its many derivatives, `BASIC`, `Haskell`, and `Ruby`.

That said, it's important not to get too hung up on the distinction between interpretation and compilation.  Technically, a programming _language_ itself is neither compiled nor interpreted.  Instead, the difference is simply based on how the language is _implemented_.  Many programming languages, including many of the most popular used today, have the means to be implemented by using both interpreters and compilers, and some of the most reliable languages heavily rely on a combination of the two techniques.

## Runtime Errors vs Compile Time Errors

Since `runtime errors` occur during execution of the application, we can deduce that such errors occur during the `interpretation` phase.  On the other hand, `compile time errors` occur during the `compilation` phase.  `Compilation errors` include issues that can be picked up by the compiler, such as improper syntax, invalid references, and undeclared variables.

To break it down a bit further, let's consider a specific type of `compilation error`, the `syntax error`.  A `syntax error` occurs when the compiler or interpreter can't determine the intention of your code.  For example, let's consider the following simple `C#` code:

```cs
int Sum(int a, int b)
{
    return a + b; 
}
```

Here we have a `method` (also known as a `function` or `subroutine` in other languages) called `Sum` that expects two numeric arguments.  It adds `a` and `b` together and returns the result.  Passing in `5` and `3` to the `Sum(int a, int b)` method results in a returned value of `8`, as expected:

```cs
Console.WriteLine(Helper.Sum(5, 3)); // 8
```

However, watch what happens when we remove the single semicolon (`;`) from the code, like so:

```cs
int Sum(int a, int b)
{
    return a + b
}
```

Trying to execute this code first runs it through the `C#` `compiler`, which attempts to transform it into `bytecode`.  However, the compiler immediately spits out a `compiler error` with the error code of [`CS1002`](https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs1002):

```
; expected in Helper.cs:line 34

The compiler detected a missing semicolon. A semicolon in required at the end of every statement in C#. A statement may span more than one line.
```

The compiler cannot determine the _intention_ of our code because its attempt to parse the code results in an abnormal pattern.  This parsing process is known as [`lexical analysis`](https://en.wikipedia.org/wiki/Lexical_analysis) (or `tokenization`), which is the act of converting characters within a line of source code into a sequence of `tokens`, which are simply pre-defined instructions known to the interpreter.  By breaking all code into such tokens, a compiler is able to determine what your original source code is intended to do, and generate the machine code capable of achieving that goal.

However, if a compiler has trouble with this tokenization process a `compilation error` occurs.  In the `C#` example above, the compiler expects only a handful of potential tokens (and, thus, character series) to follow the `return a + b` statement.  One such token is the semicolon that indicates the end of a statement, but it _does not_ expect the closing brace (`}`) that it actually encountered above, so the compilation error `CS1002` was thrown.

On the other hand, a `runtime error` occurs when the runtime understands the _intention_ of the code, but fails to execute all coded instructions as currently written.  Since this type of error comes up during execution -- after compilation has taken place (i.e. during interpretation) -- `runtime errors` must typically be caught and handled directly within the source code.  Such errors are commonly referred to as `bugs` during application development, since these sorts of exceptions will force the majority of the debugging that takes place.

As a simple example, here we have the `Divide()` method that, like `Sum()`, does just what the name implies:

```cs
double Divide(int a, int b)
{
    return a / b;
}
```

There are no syntax errors, so compilation occurs without a hitch.  However, let's see what happens if we pass in the values `5` and `0` to the `Divide()` method:

```cs
Console.WriteLine(Helper.Divide(5, 0));
// System.DivideByZeroException: 'Attempted to divide by zero.'
```

During execution a [`System.DivideByZeroException`](https://airbrake.io/blog/dotnet-exception-handling/dividebyzeroexception) is thrown, indicating that we've attempted to divide by zero.  This is an example of a common `runtime error` that must be planned for, since it's difficult to know whether some calculation within a larger application might attempt to divide a value by zero at some point.

Every language is difference, but in the case of `C#` we can surround the problematic statement with a [`try-catch`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch) block, which allows us to gracefully handle a `DivideByZeroException`:

```cs
double? Divide(int a, int b)
{
    try
    {
        return a / b;
    }
    catch (DivideByZeroException e)
    {
        // Output any DivideByZeroException, then continue.
        Console.WriteLine(e);
    }
    return null;
}
```

Now, any attempt to divide by zero will throw a `DivideByZeroException`, which is caught and output the console before returning a `null` (empty) value.  In short, such exception handling is the primary way to handle `runtime errors`.

That said, even the best laid plans of mice and men often go awry.  While production defects are far from inevitable, it's critical to have a safety net in the unlikely occurrence that something unexpected happens in your application after it's already out there.  This is where the power of error monitoring software comes into play.  Even during development, but particularly after production release, error monitoring software provides that life line your organization needs to ensure your software remains fully functional.  Any unforeseen defects are immediately identified and reported to your team, without the need for user-generated feedback or awkward error reports.  Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-what-is">Airbrake's</a> exception handling tools today to see how you or your team can keep on top of any defects that slipped through the cracks during production.

---

__META DESCRIPTION__

A brief overview of what runtime errors are, with a comparison of compile time to runtime errors, including brief C# code samples.