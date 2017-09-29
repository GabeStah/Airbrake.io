# .NET Exceptions - System.ServiceModel.EndpointNotFoundException

Making our way through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll dive into the **System.ServiceModel.EndpointNotFoundException**.  The `EndpointNotFoundException` is thrown when something goes awry while trying to connect to a [Windows Communication Foundation](https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf) (`WCF`) service-oriented application.

In this article we'll examine the `EndpointNotFoundException` in more detail by looking at where it sits in the larger .NET exception hierarchy.  We'll also look at some functional C# code samples that illustrate how a basic service can be setup, and how invalid connections to such a `WCF` service might lead to `EndpointNotFoundExceptions`, so let's get crackin'!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.CommunicationException`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.communicationexception?view=netframework-4.7)
            - `EndpointNotFoundException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// Program.cs
using System;
using AIrbrake.ServiceModel.EndpointNotFoundException.MyServiceReference;
using Utility;

namespace AIrbrake.ServiceModel.EndpointNotFoundException
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Logging.LineSeparator("GetDataTest(0)");
            GetDataTest(0);

            Logging.LineSeparator("GetDataAsyncTest(0)");
            GetDataAsyncTest(0);
        }

        internal static void GetDataTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetData(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        internal static void GetDataAsyncTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetDataAsync(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
```

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- App.config -->
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7" />
    </startup>
    <system.serviceModel>
        <bindings>
            <basicHttpBinding>
                <binding name="BasicHttpBinding_IMyService" />
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://localhost:62792/MyService.svc" binding="basicHttpBinding"
                bindingConfiguration="BasicHttpBinding_IMyService" contract="MyServiceReference.IMyService"
                name="BasicHttpBinding_IMyService" />
        </client>
    </system.serviceModel>
</configuration>
```

```cs
// IMyService.cs
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MyService
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }


    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; } = true;

        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
```

```cs
// MyService.svc
using System;

namespace MyService
{
    public class MyService : IMyService
    {
        public string GetData(int value)
        {
            return $"You entered: {value}";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
```

```cs
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

As mentioned in the introduction, the `EndpointNotFoundException` is tightly correlated with the [Windows Communication Foundation](https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf) (`WCF`), which is a .NET framework for easily creating service-oriented applications.  Essentially, a `WCF` application provides at least one service endpoint (`URI`), which can be connected to and utilized via the service's API.  Plus, such API method calls can even be made asynchronously.

For the code sample today we've created a default `WCF` service application named `MyService`.  It starts with the `IMyService` interface, which defines the basic service and data contracts the service will provide:

```cs
// IMyService.cs
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MyService
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }


    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; } = true;

        [DataMember]
        public string StringValue { get; set; } = "Hello ";
    }
}
```

From there, the `MyService` service implements the `IMyService` interface, providing the `GetData(int value)` method to "retrieve" some data.  Alternatively, a data contract can also be used to retrieve data, but for our simple example we'll stick with directly invoking `GetData(int value)`:

```cs
// MyService.svc
using System;

namespace MyService
{
    public class MyService : IMyService
    {
        public string GetData(int value)
        {
            return $"You entered: {value}";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
```

Now, to access the `MyServiceClient()` class we need to specifically reference the `MyService` service in our Visual Studio project.  This can be done by right-clicking the project name and selecting `Add > Service Reference`.  On this screen, click `Discover` to automatically detect available services from other projects that are active/in the same solution.  In our case, `MyService.svc` is listed, so we select that, then we change the `Namespace` to `MyServiceReference` and click `OK`.

Going through this process will automatically add a reference to the `MyService` service in our `App.config` file, as seen below:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- App.config -->
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7" />
    </startup>
    <system.serviceModel>
        <bindings>
            <basicHttpBinding>
                <binding name="BasicHttpBinding_IMyService" />
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://localhost:62792/MyService.svc" binding="basicHttpBinding"
                bindingConfiguration="BasicHttpBinding_IMyService" contract="MyServiceReference.IMyService"
                name="BasicHttpBinding_IMyService" />
        </client>
    </system.serviceModel>
</configuration>
```

We've defined a few test methods to verify our connection to the `MyService` service, one which is a synchronous call (`GetDataTest(int value)`), and the other that is asynchronous (`GetDataAsyncTest(int value)`):

```cs
// Program.cs
using System;
using AIrbrake.ServiceModel.EndpointNotFoundException.MyServiceReference;
using Utility;

namespace AIrbrake.ServiceModel.EndpointNotFoundException
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Logging.LineSeparator("GetDataTest(0)");
            GetDataTest(0);

            Logging.LineSeparator("GetDataAsyncTest(0)");
            GetDataAsyncTest(0);
        }

        internal static void GetDataTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetData(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        internal static void GetDataAsyncTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetDataAsync(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
```

Running the above test code attempts to connect to `MyService` and passes the `int` value of `0`, before returning the results, which we've output to the console log:

```
------------ GetDataTest(0) ------------
You entered: 0

--------- GetDataAsyncTest(0) ----------
{System.Threading.Tasks.Task`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]](HashCode:64554036)}
  Result: "You entered: 0"
  Id: 1
  Exception: { }
    null
  Status: RanToCompletion
  IsCanceled: False
  IsCompleted: True
  CreationOptions: None
  AsyncState: { }
    null
  IsFaulted: False
```

Cool, everything seems to be working as expected.  The async call returns a `System.Thread.Tasks.Task` object, which is why the result is much more complicated than the direct method call, but we can see that the `Result` property is the same for both method calls.

Now, that's all well and good, but we can also manually add services to our `App.config` file.  Let's see what happens if we change the `endpoint.address` value in `App.config` to the name of an invalid service, such as `InvalidService.svc`:

```xml
<!-- App.config -->
<!-- ... -->
<client>
    <endpoint address="http://localhost:62792/InvalidService.svc" binding="basicHttpBinding"
        bindingConfiguration="BasicHttpBinding_IMyService" contract="MyServiceReference.IMyService"
        name="BasicHttpBinding_IMyService" />
</client>
<!-- ... -->
```

Executing the same two test methods with this change to the `endpoint.address` property produces the following output:

```
------------ GetDataTest(0) ------------
[EXPECTED] System.ServiceModel.EndpointNotFoundException: There was no endpoint listening at http://localhost:62792/InvalidService.svc that could accept the message. This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details. ---> System.Net.WebException: The remote server returned an error: (404) Not Found.

--------- GetDataAsyncTest(0) ----------
Exception thrown: 'System.Reflection.TargetInvocationException' in mscorlib.dll
{System.Threading.Tasks.Task`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]](HashCode:64554036)}
  Result failed with:The remote server returned an error: (404) Not Found.
  Id: 1
  Exception: { }
    {System.AggregateException(HashCode:61494432)}
      InnerExceptions: ...
        {System.ServiceModel.EndpointNotFoundException(HashCode:16578980)}
          Message: "There was no endpoint listening at http://localhost:62792/InvalidService.svc that could accept the message. This is often caused by an incorrect address or SOAP action. See InnerException, if present, for more details."
          Data: ...
          InnerException: { }
          TargetSite: { }
          StackTrace: ...
          HelpLink: null
          Source: "System.ServiceModel.Internals"
          HResult: -2146233087
      Message: "One or more errors occurred."
      Data: ...
      InnerException: { }
        {System.ServiceModel.EndpointNotFoundException(HashCode:16578980)}
        (reference already dumped - line:7)
      TargetSite: { }
        null
      StackTrace: null
      HelpLink: null
      Source: null
      HResult: -2146233088
  Status: Faulted
  IsCanceled: False
  IsCompleted: True
  CreationOptions: None
  AsyncState: { }
    null
  IsFaulted: True
```

Ah hah!  Here we see our good friend the `EndpointNotFoundException` popup.  As the error message indicates, the specific reason for a thrown `EndpointNotFoundException` could be any of a variety of reasons, but typically it's is because the provided `endpoint.address` value in `App.config` is incorrect.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the EndpointNotFoundException in .NET, including C# code illustrating how to create and connect to a simple WCF service.