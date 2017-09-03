# .NET Exceptions - System.DivideByZeroException

Moving along through our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we face off against the System.DivideByZeroException.  The `System.DivideByZeroException` is thrown when attempting to divide an `integer` or a `decimal` by zero.

Normally divide by zero errors are pretty boring, but in this article we'll examine the `System.DivideByZeroException` in more detail and see how .NET converts your written code into low-level human-readable instructions, a handful of which can bubble up to a DivideByZeroException.  We'll look at some functional code samples in both C# and the Common Intermediate Language (`CIL`) that is executed behind the scenes, so let's get going!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.ArithmeticException`](https://docs.microsoft.com/en-us/dotnet/api/system.arithmeticexception)
                - `System.DivideByZeroException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using Utility;

namespace Airbrake.DivideByZeroException
{
    internal class Program
    {
        private const int Numerator = 24601;
        private const int Denominator = 0;

        internal static void Main(string[] args)
        {
            Logging.LineSeparator("INT");
            TestToInt();

            Logging.LineSeparator("DOUBLE");
            TestToDouble();

            Logging.LineSeparator("DECIMAL");
            TestToDecimal();
        }

        internal static int DivideInt(int a, int b)
        {
            return a / b;
        }

        internal static double DivideDouble(double a, double b)
        {
            return a / b;
        }

        internal static decimal DivideDecimal(decimal a, decimal b)
        {
            return a / b;
        }

        internal static void TestToInt()
        {
            try
            {
                Logging.Log($"{Numerator} / {Denominator} = {DivideInt(Numerator, Denominator)}");
            }
            catch (System.DivideByZeroException exception)
            {
                // Log unexpected DivideByZeroExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        internal static void TestToDouble()
        {
            try
            {
                Logging.Log($"{Numerator} / {Denominator} = {DivideDouble(Numerator, Denominator)}");
            }
            catch (System.DivideByZeroException exception)
            {
                // Log unexpected DivideByZeroExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        internal static void TestToDecimal()
        {
            try
            {
                Logging.Log($"{Numerator} / {Denominator} = {DivideDecimal(Numerator, Denominator)}");
            }
            catch (System.DivideByZeroException exception)
            {
                // Log unexpected DivideByZeroExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

    }
}

// <Utility/>Logging.cs
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Output(value, outputType);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        public static void Log(string value, object arg0)
        {
            Debug.WriteLine(value, arg0);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(string value, object arg0, object arg1)
        {
            Debug.WriteLine(value, arg0, arg1);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(string value, object arg0, object arg1, object arg2)
        {
            Debug.WriteLine(value, arg0, arg1, arg2);
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Output(value, outputType);
        }

        private static void Output(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            if (value is IXmlSerializable)
            {
                Debug.WriteLine(value);
            }
            else
            {
                Debug.WriteLine(outputType == OutputType.Timestamp
                    ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                    : ObjectDumper.Dump(value));
            }
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= insert.Length + 2;
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

## When Should You Use It?

The `System.DivideByZeroException` is not a particularly exciting or difficult exception to understand, considering the massive quantity of unique exceptions the .NET Framework provides.  However, a discussion of exactly how the `System.DivideByZeroException` works and is thrown, behind the scenes, leads to some interesting territory.  As mentioned in the introduction, the manner in which .NET compiles and processes the high-level code you write (in C# or otherwise) is by converting it into machine-readable instructions known as the [`Command Intermediate Language`](https://en.wikipedia.org/wiki/Common_Intermediate_Language) or `CIL`.  This `CIL` is then converted to `bytecode`, from which a [`Common Language Infrastructure`](https://en.wikipedia.org/wiki/Common_Language_Infrastructure) assembly is created, which can then be directly executed by the computer.

To understand what all this means it's much easier to look at some code examples.  We'll start with our C# code, which defines a couple constants (`Numerator` and `Denominator`), then performs a handful of tests by trying to divide by zero using `int`, `double`, and `decimal` value types.  For example, here we have the series of instructions for performing a division by zero using `doubles`:

```cs
using System;
using Utility;

namespace Airbrake.DivideByZeroException
{
    internal class Program
    {
        private const int Numerator = 24601;
        private const int Denominator = 0;

        internal static void Main(string[] args)
        {
            // ...

            Logging.LineSeparator("DOUBLE");
            TestToDouble();

            // ...
        }

        internal static double DivideDouble(double a, double b)
        {
            return a / b;
        }

        internal static void TestToDouble()
        {
            try
            {
                Logging.Log($"{Numerator} / {Denominator} = {DivideDouble(Numerator, Denominator)}");
            }
            catch (System.DivideByZeroException exception)
            {
                // Log unexpected DivideByZeroExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
```

The `TestToDouble()` method houses the `try-catch` block, so that each unique data type test can throw an exception separately, without hindering execution of other tests.  Executing the `double` divide by zero test works just fine, without throwing any exceptions.  Instead, as we can see in the output, since we're using floating-point numbers in this case, the result is a value of `positive infinity`:

```
---------------- DOUBLE ----------------
24601 / 0 = ∞
```

However, let's see what happens when we use `int` and `decimal` values instead:

```
----------------- INT ------------------
[EXPECTED] System.DivideByZeroException: Attempted to divide by zero.
--------------- DECIMAL ----------------
[EXPECTED] System.DivideByZeroException: Attempted to divide by zero.
```

In both cases, attempts to divide by zero result in a `System.DivideByZeroException`.  To see exactly _why_ `int` and `decimal` attempts throw an exception, but `double` does not, we need to dig into the `CIL` code that is produced when compiling our C# code above.  Below we see the full `Airbrake.DivideByZeroException` namespace C# code converted into `CIL`:

```nasm
.namespace Airbrake.DivideByZeroException
{
	.class private auto ansi beforefieldinit Airbrake.DivideByZeroException.Program
		extends [mscorlib]System.Object
	{
		// Fields
		.field private static literal int32 Numerator = int32(24601)
		.field private static literal int32 Denominator = int32(0)

		// Methods
		.method assembly hidebysig static 
			void Main (
				string[] args
			) cil managed 
		{
			// Method begins at RVA 0x2050
			// Code size 65 (0x41)
			.maxstack 3
			.entrypoint

			IL_0000: nop
			IL_0001: ldstr "INT"
			IL_0006: ldc.i4.s 40
			IL_0008: ldc.i4.s 45
			IL_000a: call void [Utility]Utility.Logging::LineSeparator(string, int32, char)
			IL_000f: nop
			IL_0010: call void Airbrake.DivideByZeroException.Program::TestToInt()
			IL_0015: nop
			IL_0016: ldstr "DOUBLE"
			IL_001b: ldc.i4.s 40
			IL_001d: ldc.i4.s 45
			IL_001f: call void [Utility]Utility.Logging::LineSeparator(string, int32, char)
			IL_0024: nop
			IL_0025: call void Airbrake.DivideByZeroException.Program::TestToDouble()
			IL_002a: nop
			IL_002b: ldstr "DECIMAL"
			IL_0030: ldc.i4.s 40
			IL_0032: ldc.i4.s 45
			IL_0034: call void [Utility]Utility.Logging::LineSeparator(string, int32, char)
			IL_0039: nop
			IL_003a: call void Airbrake.DivideByZeroException.Program::TestToDecimal()
			IL_003f: nop
			IL_0040: ret
		} // end of method Program::Main

		.method assembly hidebysig static 
			int32 DivideInt (
				int32 a,
				int32 b
			) cil managed 
		{
			// Method begins at RVA 0x20a0
			// Code size 9 (0x9)
			.maxstack 2
			.locals init (
				[0] int32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldarg.1
			IL_0003: div
			IL_0004: stloc.0
			IL_0005: br.s IL_0007

			IL_0007: ldloc.0
			IL_0008: ret
		} // end of method Program::DivideInt

		.method assembly hidebysig static 
			float64 DivideDouble (
				float64 a,
				float64 b
			) cil managed 
		{
			// Method begins at RVA 0x20b8
			// Code size 9 (0x9)
			.maxstack 2
			.locals init (
				[0] float64
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldarg.1
			IL_0003: div
			IL_0004: stloc.0
			IL_0005: br.s IL_0007

			IL_0007: ldloc.0
			IL_0008: ret
		} // end of method Program::DivideDouble

		.method assembly hidebysig static 
			valuetype [mscorlib]System.Decimal DivideDecimal (
				valuetype [mscorlib]System.Decimal a,
				valuetype [mscorlib]System.Decimal b
			) cil managed 
		{
			// Method begins at RVA 0x20d0
			// Code size 13 (0xd)
			.maxstack 2
			.locals init (
				[0] valuetype [mscorlib]System.Decimal
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldarg.1
			IL_0003: call valuetype [mscorlib]System.Decimal [mscorlib]System.Decimal::op_Division(valuetype [mscorlib]System.Decimal, valuetype [mscorlib]System.Decimal)
			IL_0008: stloc.0
			IL_0009: br.s IL_000b

			IL_000b: ldloc.0
			IL_000c: ret
		} // end of method Program::DivideDecimal

		.method assembly hidebysig static 
			void TestToInt () cil managed 
		{
			// Method begins at RVA 0x20ec
			// Code size 83 (0x53)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.DivideByZeroException exception,
				[1] class [mscorlib]System.Exception exception
			)

			IL_0000: nop
			.try
			{
				IL_0001: nop
				IL_0002: ldstr "{0} / {1} = {2}"
				IL_0007: ldc.i4 24601
				IL_000c: box [mscorlib]System.Int32
				IL_0011: ldc.i4.0
				IL_0012: box [mscorlib]System.Int32
				IL_0017: ldc.i4 24601
				IL_001c: ldc.i4.0
				IL_001d: call int32 Airbrake.DivideByZeroException.Program::DivideInt(int32, int32)
				IL_0022: box [mscorlib]System.Int32
				IL_0027: call string [mscorlib]System.String::Format(string, object, object, object)
				IL_002c: ldc.i4.0
				IL_002d: call void [Utility]Utility.Logging::Log(string, valuetype [Utility]Utility.Logging/OutputType)
				IL_0032: nop
				IL_0033: nop
				IL_0034: leave.s IL_0052
			} // end .try
			catch [mscorlib]System.DivideByZeroException
			{
				IL_0036: stloc.0
				IL_0037: nop
				IL_0038: ldloc.0
				IL_0039: ldc.i4.1
				IL_003a: ldc.i4.0
				IL_003b: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_0040: nop
				IL_0041: nop
				IL_0042: leave.s IL_0052
			} // end handler
			catch [mscorlib]System.Exception
			{
				IL_0044: stloc.1
				IL_0045: nop
				IL_0046: ldloc.1
				IL_0047: ldc.i4.0
				IL_0048: ldc.i4.0
				IL_0049: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_004e: nop
				IL_004f: nop
				IL_0050: leave.s IL_0052
			} // end handler

			IL_0052: ret
		} // end of method Program::TestToInt

		.method assembly hidebysig static 
			void TestToDouble () cil managed 
		{
			// Method begins at RVA 0x2168
			// Code size 95 (0x5f)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.DivideByZeroException exception,
				[1] class [mscorlib]System.Exception exception
			)

			IL_0000: nop
			.try
			{
				IL_0001: nop
				IL_0002: ldstr "{0} / {1} = {2}"
				IL_0007: ldc.i4 24601
				IL_000c: box [mscorlib]System.Int32
				IL_0011: ldc.i4.0
				IL_0012: box [mscorlib]System.Int32
				IL_0017: ldc.r8 24601
				IL_0020: ldc.r8 0.0
				IL_0029: call float64 Airbrake.DivideByZeroException.Program::DivideDouble(float64, float64)
				IL_002e: box [mscorlib]System.Double
				IL_0033: call string [mscorlib]System.String::Format(string, object, object, object)
				IL_0038: ldc.i4.0
				IL_0039: call void [Utility]Utility.Logging::Log(string, valuetype [Utility]Utility.Logging/OutputType)
				IL_003e: nop
				IL_003f: nop
				IL_0040: leave.s IL_005e
			} // end .try
			catch [mscorlib]System.DivideByZeroException
			{
				IL_0042: stloc.0
				IL_0043: nop
				IL_0044: ldloc.0
				IL_0045: ldc.i4.1
				IL_0046: ldc.i4.0
				IL_0047: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_004c: nop
				IL_004d: nop
				IL_004e: leave.s IL_005e
			} // end handler
			catch [mscorlib]System.Exception
			{
				IL_0050: stloc.1
				IL_0051: nop
				IL_0052: ldloc.1
				IL_0053: ldc.i4.0
				IL_0054: ldc.i4.0
				IL_0055: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_005a: nop
				IL_005b: nop
				IL_005c: leave.s IL_005e
			} // end handler

			IL_005e: ret
		} // end of method Program::TestToDouble

		.method assembly hidebysig static 
			void TestToDecimal () cil managed 
		{
			// Method begins at RVA 0x21f0
			// Code size 92 (0x5c)
			.maxstack 5
			.locals init (
				[0] class [mscorlib]System.DivideByZeroException exception,
				[1] class [mscorlib]System.Exception exception
			)

			IL_0000: nop
			.try
			{
				IL_0001: nop
				IL_0002: ldstr "{0} / {1} = {2}"
				IL_0007: ldc.i4 24601
				IL_000c: box [mscorlib]System.Int32
				IL_0011: ldc.i4.0
				IL_0012: box [mscorlib]System.Int32
				IL_0017: ldc.i4 24601
				IL_001c: newobj instance void [mscorlib]System.Decimal::.ctor(int32)
				IL_0021: ldsfld valuetype [mscorlib]System.Decimal [mscorlib]System.Decimal::Zero
				IL_0026: call valuetype [mscorlib]System.Decimal Airbrake.DivideByZeroException.Program::DivideDecimal(valuetype [mscorlib]System.Decimal, valuetype [mscorlib]System.Decimal)
				IL_002b: box [mscorlib]System.Decimal
				IL_0030: call string [mscorlib]System.String::Format(string, object, object, object)
				IL_0035: ldc.i4.0
				IL_0036: call void [Utility]Utility.Logging::Log(string, valuetype [Utility]Utility.Logging/OutputType)
				IL_003b: nop
				IL_003c: nop
				IL_003d: leave.s IL_005b
			} // end .try
			catch [mscorlib]System.DivideByZeroException
			{
				IL_003f: stloc.0
				IL_0040: nop
				IL_0041: ldloc.0
				IL_0042: ldc.i4.1
				IL_0043: ldc.i4.0
				IL_0044: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_0049: nop
				IL_004a: nop
				IL_004b: leave.s IL_005b
			} // end handler
			catch [mscorlib]System.Exception
			{
				IL_004d: stloc.1
				IL_004e: nop
				IL_004f: ldloc.1
				IL_0050: ldc.i4.0
				IL_0051: ldc.i4.0
				IL_0052: call void [Utility]Utility.Logging::Log(class [mscorlib]System.Exception, bool, valuetype [Utility]Utility.Logging/OutputType)
				IL_0057: nop
				IL_0058: nop
				IL_0059: leave.s IL_005b
			} // end handler

			IL_005b: ret
		} // end of method Program::TestToDecimal

		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2274
			// Code size 8 (0x8)
			.maxstack 8

			IL_0000: ldarg.0
			IL_0001: call instance void [mscorlib]System.Object::.ctor()
			IL_0006: nop
			IL_0007: ret
		} // end of method Program::.ctor

	} // end of class Airbrake.DivideByZeroException.Program

}
```

_Note: To view the `CIL` of your own .NET code, check out a reflection tool such as [ILSpy](http://ilspy.net/)_.

As you can see, `CIL` is a stack-based assembly language, interpreting instructions in an ordered fashion.  There's far too much to go over everything here, so we'll instead focus on a few snippets of `CIL` that are pertinent to our `System.DivideByZeroException` example.

Here we have the `DivideInt()` method, which accepts two `int` parameters and performs division of `a` over `b`, then returns the result as `int`:

```
.method assembly hidebysig static 
    int32 DivideInt (
        int32 a,
        int32 b
    ) cil managed 
{
    // Method begins at RVA 0x20a0
    // Code size 9 (0x9)
    .maxstack 2
    .locals init (
        [0] int32
    )

    IL_0000: nop
    IL_0001: ldarg.0
    IL_0002: ldarg.1
    IL_0003: div
    IL_0004: stloc.0
    IL_0005: br.s IL_0007

    IL_0007: ldloc.0
    IL_0008: ret
} // end of method Program::DivideInt
```

Let's break down what's happening here.  The method signature with the `int32` parameters `a` and `b` is easy to parse, since it's similar to how normal .NET code looks and behaves.  The [`.maxstack`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbody.maxstacksize?view=netframework-4.7#System_Reflection_MethodBody_MaxStackSize) property indicates how many items are required within the stack to fully execute this particular method.  In this case, we only need to reserve two items in the stack.

[`.locals`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbody.initlocals?view=netframework-4.7#System_Reflection_MethodBody_InitLocals) specifies what value type local variables that aren't declared a specific type should be set to.  In this case, `int32` is the only type we're using, so that's the default.

Now we get into the actual instruction set of the method.  All of these instructions begin with an [`opcode`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes?view=netframework-4.7), which essentially defines what instruction to perform.

- [`nop`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.nop?view=netframework-4.7) is a placeholder instruction.
- [`ldarg.0`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_0?view=netframework-4.7) loads the first argument onto the evaluation stack.
- [`ldarg.1`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_1?view=netframework-4.7) loads the second argument onto the evaluation stack.
- [`div`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div?view=netframework-4.7) is the bread and butter of this method.  It divides the previous two values on the evaluation stack (which we just loaded), and adds the resulting floating-point or integer value onto the stack.
- [`stloc.0`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_0?view=netframework-4.7) extracts the current value from the top of the stack (in this case, the result of our division) and stores it in the first local variable index.
- [`br.s`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.br_s?view=netframework-4.7) (unconditionally) transfers control to the specified instruction (`IL_0007` in this case).  This is similar to `jmp` or `goto` instructions found in other assembly languages.
- [`ldloc.0`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_0?view=netframework-4.7) loads the first local variable, which we stored two instructions ago using `stloc.0`.  This causes the result of our division to be loaded to the top of the stack.
- Finally, [`ret`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ret?view=netframework-4.7) returns from the current method and pushes the value at the top of this method's stack to the calling method's evaluation stack.

So, why is it important to know how the `CIL` works when calling these division methods?  Because, only a _select handful_ of `CIL` `opcodes` are actually capable of producing a `System.DivideByZeroException`.  As you might have guessed, one such `CIL` `opcode` is [`div`](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div?view=netframework-4.7), which was part of the fundamental instruction we saw in the `CIL` of the `DivideInt()` method above.  In fact, the way the `CIL` works is that if the _second_ value in the evaluation stack (the one we loaded using `ldarg.1` above) is equal to `zero`, a `System.DivideByZeroException` is thrown.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.DivideByZeroException in .NET, including C# code converted to CIL assembly format, showing why these errors are thrown.