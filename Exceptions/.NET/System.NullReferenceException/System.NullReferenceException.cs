using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class NullReferenceExceptionExample
    {
        public static void Main(string[] args)
        {
            CreateArray();
            CreateList();            
        }

        private static void CreateArray()
        {            
            try   
            {
                int[] values = null;
                for (int count = 0; count <= 9; count++)
                    values[count] = count;

                foreach (var value in values)
                {
                    Console.WriteLine(value);
                }
            }
            catch(NullReferenceException exception)
            {
                LogException(exception);
            }
        }

        private static void CreateList()
        {            
            try   
            {
                int value = 0;
                List<String> names;
                if (value > 0)
                {
                    names = new List<String>();
                }

                //names.Add("Alice Bob Chris");
            }
            catch(NullReferenceException exception)
            {
                LogException(exception);
            }
        }

        private static void LogException(Exception exception, bool expected = true)
        {
            Console.WriteLine($"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}");
        }
    }
}
