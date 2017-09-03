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
