# .NET Exceptions - System.Management.ManagementException

Moving along through the detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we will be exploring the **System.Management.ManagementException**.  A `ManagementException` is thrown when something goes wrong while working with the [`Windows Management Instrumentation`](https://docs.microsoft.com/en-us/dotnet/api/system.management?view=netframework-4.7) (`WMI`) components contained within the `System.Management` namespace.  `WMI` essentially allows scripts and applications to perform queries against local or remote systems, devices, and even other apps.  These queries can retrieve a variety of information, including logical disk data, CPU utilization, database connectivity, and so forth.

Throughout this article we'll explore the `ManagementException` by looking at where it sits in the larger .NET exception hierarchy.  We'll also look at some fully-functional C# sample code to illustrate not only how `WMI` works to retrieve system information, but also how invalid queries or issues might result in `ManagementExceptions` in your own code, so let's get going!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - `ManagementException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// Airbrake.Management.ManagementException.Program.cs
using System;
using Utility;

namespace Airbrake.Management.ManagementException
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Logging.LineSeparator("LOCAL DISK INFO");
            GetDiskInfo();
            Logging.LineSeparator("FULL LOCAL DISK INFO");
            GetFullDiskInfo();
            Logging.LineSeparator("LOCAL DISK INFO - INVALID QUERY");
            GetDiskInfo_InvalidQuery();
        }

        private static void GetDiskInfo()
        {
            // Perform query and create Manager.
            var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
            // Loop through query results.
            foreach (var disk in manager.Searcher.Get())
            {
                var free = (ulong) disk.GetPropertyValue("FreeSpace");
                var total = (ulong) disk.GetPropertyValue("Size");
                // Output disk space info.
                Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
            }
        }

        private static void GetFullDiskInfo()
        {
            // Perform query and create Manager.
            var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
            // Output full result data.
            manager.DumpResults();
        }

        private static void GetDiskInfo_InvalidQuery()
        {
            try
            {
                // Perform query and create Manager.
                var manager = new Manager("SELECT * FROM Win32_PhysicalDisk");
                // Loop through query results.
                foreach (var disk in manager.Searcher.Get())
                {
                    var free = (ulong)disk.GetPropertyValue("FreeSpace");
                    var total = (ulong)disk.GetPropertyValue("Size");
                    // Output disk space info.
                    Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
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

```cs
// Extensions.cs
using System;

namespace Airbrake.Management.ManagementException
{
    public static class Extensions
    {
        private static readonly string[] StorageSuffixes =
        {
            "B",
            "KB",
            "MB",
            "GB",
            "TB",
            "PB",
            "EB",
            "ZB",
            "YB"
        };

        /// <summary>
        /// Converts ulong value of total bytes into formatted string, 
        /// representing the number of converted KB, MB, GB, etc.
        /// </summary>
        /// <param name="value">Total number of bytes.</param>
        /// <returns>Formatted string indicating largest order of magnitudinal measurement.</returns>
        public static string ToStorageString(this ulong value)
        {
            var i = 0;
            var decimalValue = (decimal)value;
            while (Math.Round(decimalValue) >= 1000)
            {
                decimalValue /= 1024;
                i++;
            }

            return string.Format($"{decimalValue:n2} {StorageSuffixes[i]}");
        }
    }
}
```

```cs
// Manager.cs
using System;
using System.Management;
using Utility;

namespace Airbrake.Management.ManagementException
{
    /// <summary>
    /// Manages creation and manipulation of WMI objects.
    /// </summary>
    internal class Manager
    {
        private const string ScopePathDefault = "\\\\I7\\root\\cimv2";

        public ObjectQuery Query { get; }
        public string ScopePath { get; } = ScopePathDefault;

        internal ManagementObjectSearcher Searcher { get; }

        private readonly ManagementScope _scope;
        public ManagementScope Scope
        {
            get
            {
                // Update scope path.
                _scope.Path = new ManagementPath(ScopePath);
                return _scope;
            }
        }

        internal Manager(string query, string scope = ScopePathDefault)
        {
            Query = new ObjectQuery(query);
            _scope = new ManagementScope(scope);
            Searcher = new ManagementObjectSearcher(Scope, Query);
        }

        internal Manager(ManagementObjectSearcher searcher)
        {
            Query = searcher.Query;
            _scope = searcher.Scope;
            Searcher = searcher;
        }

        internal Manager(ObjectQuery query, ManagementObjectSearcher searcher, ManagementScope scope)
        {
            Query = query;
            Searcher = searcher;
            _scope = scope;
        }

        /// <summary>
        /// Output Searcher.Get() result property value of passed property.
        /// </summary>
        /// <param name="property">Property value to retrieve.</param>
        public void OutputPropertyValue(string property)
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetPropertyValue(property));
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Dumps all Searcher.Get() results to the log, as formatted text.
        /// </summary>
        public void DumpResults()
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetText(new TextFormat()));
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
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

Windows Management Instrumentation (`WMI`) allows applications and scripts to manage Windows-based systems and devices, both locally and remotely.  Once a connection is established to a system, `WMI` can be used to perform SQL-like queries, which effectively provides an API to the executing application, allowing it to retrieve data or perform changes to the underlying system.  Covering the whole of the `WMI` infrastructure is well beyond the scope of this little tutorial, but feel free to check out the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.management?view=netframework-4.7) for more information.

However, in order to see how a `ManagementException` might be thrown, we need to get into the basics of `WMI` and how it can be manipulated via code.  Before we access system information programmatically, it helps to look at some system data through a visual tool.  Thankfully, most newer versions of Windows already include such a tool, known as the `Windows Management Instrumentation Tester` (or `wbemtest.exe`).  To launch it, open your `Start` menu and search for `wbemtest.exe`, then press `enter` to run the application.

Once open, let's start by connecting to the local computer.  Click `Connect` and verify that `root\cimv2` is entered into the `Namespace` field, then click `Connect` in this second dialog.  Now, click `Open Class` and in the `Target Class Name` field enter `Win32_LogicalDisk` and click `OK`.  This should bring up an `Object Editor` screen for the `Win32_LogicalDisk` class of data found within your local system.

As you can see from the titles above the main boxes, there are some `Qualifiers`, `Properties`, and even some `Methods` that can be performed via `WMI` to query or adjust the local disk.  We won't be doing anything harmful in this tutorial, since we'll just be retrieving data via the `Properties`, so feel free to scroll through those and see what sort of property values exist for your local disks.

Now, let's move onto the sample code to see how we can perform similar queries using C#.  We begin with a custom `Manager` class, which was created to help "manage" the various `WMI`-related objects needed to establish a connection and perform queries:

```cs
// Manager.cs
using System;
using System.Management;
using Utility;

namespace Airbrake.Management.ManagementException
{
    /// <summary>
    /// Manages creation and manipulation of WMI objects.
    /// </summary>
    internal class Manager
    {
        private const string ScopePathDefault = "\\\\I7\\root\\cimv2";

        public ObjectQuery Query { get; }
        public string ScopePath { get; } = ScopePathDefault;

        internal ManagementObjectSearcher Searcher { get; }

        private readonly ManagementScope _scope;
        public ManagementScope Scope
        {
            get
            {
                // Update scope path.
                _scope.Path = new ManagementPath(ScopePath);
                return _scope;
            }
        }

        internal Manager(string query, string scope = ScopePathDefault)
        {
            Query = new ObjectQuery(query);
            _scope = new ManagementScope(scope);
            Searcher = new ManagementObjectSearcher(Scope, Query);
        }

        internal Manager(ManagementObjectSearcher searcher)
        {
            Query = searcher.Query;
            _scope = searcher.Scope;
            Searcher = searcher;
        }

        internal Manager(ObjectQuery query, ManagementObjectSearcher searcher, ManagementScope scope)
        {
            Query = query;
            Searcher = searcher;
            _scope = scope;
        }

        /// <summary>
        /// Output Searcher.Get() result property value of passed property.
        /// </summary>
        /// <param name="property">Property value to retrieve.</param>
        public void OutputPropertyValue(string property)
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetPropertyValue(property));
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Dumps all Searcher.Get() results to the log, as formatted text.
        /// </summary>
        public void DumpResults()
        {
            try
            {
                foreach (var element in Searcher.Get())
                {
                    Logging.Log(element.GetText(new TextFormat()));
                }
            }
            catch (System.Management.ManagementException exception)
            {
                // Output expected ManagementExceptions.
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

There are a handful of properties for the `Manager` class, but the important one is the `ManagementObjectSearcher Searcher` property, which is the fundamental class we'll be using to perform queries.  To create a new `ManagementObjectSearcher` instance we need to specify a `ManagementScope`, which is effectively the `path` to the specific system/computer, along with the `namespace` within said computer, that will be queried.  `root\cimv2` is the primary namespace, and in this case, `I7` is the name of the computer that I'm querying, so that's where we get the `ScopePathDefault` constant value of `"\\\\I7\\root\\cimv2"`.

The `ManagementObjectSearcher` instance also needs a query, which we create by passing a string to a new `ObjectQuery` instance.  Both the `OutputProperty(string property)` and `DumpResults()` methods are fairly simple and self-explanatory, so we won't get into those.

Now, our goal is to retrieve local disk information, specifically regarding storage capacity.  To format the produced output a bit better, we've also created a simple extension method of `ToStorageString(this ulong value)`:

```cs
// Extensions.cs
using System;

namespace Airbrake.Management.ManagementException
{
    public static class Extensions
    {
        private static readonly string[] StorageSuffixes =
        {
            "B",
            "KB",
            "MB",
            "GB",
            "TB",
            "PB",
            "EB",
            "ZB",
            "YB"
        };

        /// <summary>
        /// Converts ulong value of total bytes into formatted string, 
        /// representing the number of converted KB, MB, GB, etc.
        /// </summary>
        /// <param name="value">Total number of bytes.</param>
        /// <returns>Formatted string indicating largest order of magnitudinal measurement.</returns>
        public static string ToStorageString(this ulong value)
        {
            var i = 0;
            var decimalValue = (decimal)value;
            while (Math.Round(decimalValue) >= 1000)
            {
                decimalValue /= 1024;
                i++;
            }

            return string.Format($"{decimalValue:n2} {StorageSuffixes[i]}");
        }
    }
}
```

This method extends `ulong` (i.e. `UInt64`) values, making it easy to convert these large values into recognizable byte-based output formats, such as MB, GB, TB, etc.

Now that everything is setup, let's use our `Manager` class and the `ToStorageString(this ulong value)` extension method to query our disks and get some info.  We start with the `GetDiskInfo()` method:

```cs
private static void GetDiskInfo()
{
    // Perform query and create Manager.
    var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
    // Loop through query results.
    foreach (var disk in manager.Searcher.Get())
    {
        var free = (ulong) disk.GetPropertyValue("FreeSpace");
        var total = (ulong) disk.GetPropertyValue("Size");
        // Output disk space info.
        Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
    }
}
```

Here we perform a simple query of getting all data from the `Win32_LogicalDisk` class (which we explored with the `wbemtest.exe` tool), then loop through the resulting data and get some specific property values to produce a readable log output.  The result of executing this method on my local system shows all three of my primary logical disks and their current free space/capacity:

```
----------- LOCAL DISK INFO ------------
The C: drive has 293.18 GB free, out of 464.41 GB total.
The D: drive has 2.57 TB free, out of 2.73 TB total.
The E: drive has 231.96 GB free, out of 465.63 GB total.
```

Cool!  However, `WMI` is capable of much more, so let's expand a bit and query every field in the `Win32_LogicalDisk` class and output it, just to see the values of everything we could potentially want to look at.  This is performed in the `GetFullDiskInfo()` method, which also uses the `Manager.DumpResults()` method:

```cs
private static void GetFullDiskInfo()
{
    // Perform query and create Manager.
    var manager = new Manager("SELECT * FROM Win32_LogicalDisk");
    // Output full result data.
    manager.DumpResults();
}
```

```cs
internal class Manager
{
    // ...

    /// <summary>
    /// Dumps all Searcher.Get() results to the log, as formatted text.
    /// </summary>
    public void DumpResults()
    {
        try
        {
            foreach (var element in Searcher.Get())
            {
                Logging.Log(element.GetText(new TextFormat()));
            }
        }
        catch (System.Management.ManagementException exception)
        {
            // Output expected ManagementExceptions.
            Logging.Log(exception);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false);
        }
    }
}
```

Executing this code produces the full, formatted list of property key/value pairs for each local disk:

```
--------- FULL LOCAL DISK INFO ---------
instance of Win32_LogicalDisk
{
	Access = 0;
	Caption = "C:";
	Compressed = FALSE;
	CreationClassName = "Win32_LogicalDisk";
	Description = "Local Fixed Disk";
	DeviceID = "C:";
	DriveType = 3;
	FileSystem = "NTFS";
	FreeSpace = "314800488448";
	MaximumComponentLength = 255;
	MediaType = 12;
	Name = "C:";
	Size = "498654154752";
	SupportsDiskQuotas = FALSE;
	SupportsFileBasedCompression = TRUE;
	SystemCreationClassName = "Win32_ComputerSystem";
	SystemName = "I7";
	VolumeName = "";
	VolumeSerialNumber = "866272F2";
};

instance of Win32_LogicalDisk
{
	Access = 0;
	Caption = "D:";
	Compressed = FALSE;
	CreationClassName = "Win32_LogicalDisk";
	Description = "Local Fixed Disk";
	DeviceID = "D:";
	DriveType = 3;
	FileSystem = "NTFS";
	FreeSpace = "2829458685952";
	MaximumComponentLength = 255;
	MediaType = 12;
	Name = "D:";
	Size = "3000457228288";
	SupportsDiskQuotas = FALSE;
	SupportsFileBasedCompression = TRUE;
	SystemCreationClassName = "Win32_ComputerSystem";
	SystemName = "I7";
	VolumeName = "Storage";
	VolumeSerialNumber = "861F4843";
};

instance of Win32_LogicalDisk
{
	Access = 0;
	Caption = "E:";
	Compressed = FALSE;
	CreationClassName = "Win32_LogicalDisk";
	Description = "Local Fixed Disk";
	DeviceID = "E:";
	DriveType = 3;
	FileSystem = "NTFS";
	FreeSpace = "249059831808";
	MaximumComponentLength = 255;
	MediaType = 12;
	Name = "E:";
	Size = "499971518464";
	SupportsDiskQuotas = FALSE;
	SupportsFileBasedCompression = TRUE;
	SystemCreationClassName = "Win32_ComputerSystem";
	SystemName = "I7";
	VolumeName = "Games";
	VolumeSerialNumber = "541D6CE0";
};
```

Finally, everything has worked just as expected up to this point, but let's see what happens if we make a mistake in our query or order of execution when creating a `ManagementObjectSearcher`.  To illustrate this we have the `GetDiskInfo_InvalidQuery()` method:

```cs
private static void GetDiskInfo_InvalidQuery()
{
    try
    {
        // Perform query and create Manager.
        var manager = new Manager("SELECT * FROM Win32_PhysicalDisk");
        // Loop through query results.
        foreach (var disk in manager.Searcher.Get())
        {
            var free = (ulong)disk.GetPropertyValue("FreeSpace");
            var total = (ulong)disk.GetPropertyValue("Size");
            // Output disk space info.
            Logging.Log($"The {disk.GetPropertyValue("Name")} drive has {free.ToStorageString()} free, out of {total.ToStorageString()} total.");
        }
    }
    catch (System.Management.ManagementException exception)
    {
        // Output expected ManagementExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

This may look similar to the `GetDiskInfo()` method (and it is), but the subtle difference is the class we're querying is `Win32_PhysicalDisk` instead of `Win32_LogicalDisk`.  Executing this method results in the following output:

```
--- LOCAL DISK INFO - INVALID QUERY ----
[EXPECTED] System.Management.ManagementException: Invalid class 
```

As it happens, `Win32_PhysicalDisk` is not a valid class in `WMI`, so a `ManagementException` is thrown indicating as much.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the ManagementException in .NET, including C# code illustrating how to create and execute Windows Management Instrumentation queries.