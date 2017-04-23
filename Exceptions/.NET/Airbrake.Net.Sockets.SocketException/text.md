# .NET Exceptions - System.Net.Sockets.SocketException

Making our way through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we're going to take a look at the `System.Net.Sockets.SocketException`.  Typically, a `System.Net.Sockets.SocketException` is thrown when an error occurs within a `socket`, such as a failure to connect to a remote network.

In this article we'll examine the `System.Net.Sockets.SocketException` in more detail, including where it resides within the .NET exception hierarchy, along with some working code examples to illustrate how `System.Net.Sockets.SocketExceptions` might be thrown in real-world code.  Let's get this party started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception) is inherited from the [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) class.
- [`System.ComponentModel.Win32Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.win32exception) is inherited from the [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception) class.
- Finally, `System.Net.Sockets.SocketException` inherits from [`System.ComponentModel.Win32Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.win32exception).

## When Should You Use It?

As with most of the .NET exceptions, it's easiest to understand how `System.Net.Sockets.SocketException` works by just starting right out with a code example.  The full code is directly below, after which we'll take a bit of time to examine it:

```cs
using System;
using System.Net;
using System.Net.Sockets;
using Utility;

namespace Airbrake.Net.Sockets.SocketException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Working example, localhost with default port 80.
            ConnectToSocket("127.0.0.1");

            // Invalid example, localhost with inactive port 4444.
            ConnectToSocket("127.0.0.1:4444");

            // Invalid example, Google DNS with default port 80.
            ConnectToSocket("8.8.8.8");
        }

        private static IPEndPoint ParseIPEndPoint(string server)
        {
            Uri uri;
            // Try to generate a valid URI from server.
            if (Uri.TryCreate(server, UriKind.Absolute, out uri) || 
                Uri.TryCreate(String.Concat("http://", server), UriKind.Absolute, out uri))
            {
                // Create new IPEndPoint with parsed Uri arguments.
                return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
            }
            // If Uri creation fails, invalid server parameter was passed.
            throw new FormatException($"Invalid server provided to ParseIPEndPoint of: {server}");
        }

        private static void ConnectToSocket(string uri)
        {
            try
            {
                // Create IPEndPoint.
                IPEndPoint endPoint = ParseIPEndPoint(uri);
                // Create socket from EndPoint arguments.
                var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Attempt connection.
                socket.Connect(endPoint);
                // Confirm connection with output.
                Logging.Log($"Connection established to: {endPoint.Address}:{endPoint.Port}");
                // Close connection.
                socket.Close();
            }
            catch (System.Net.Sockets.SocketException exception)
            {
                Logging.Log(exception);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        public static void Log(Exception exception, bool expected = true)
        {
            string value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}";
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }
    }
}
```

We won't get into the `Utility` namespace or `Logging` class because we've used that many times before and it's just a means of simplifying our output needs during development.  Instead, let's look at the meat of our example, which resides in the `Program` class where we defined two simple methods to help us attempt to establish a socket connection.  As you may recall, a `socket` (or [`network socket`](https://en.wikipedia.org/wiki/Network_socket)) is an endpoint on another system for sending/receiving network data.  Most commonly, a socket is used to generate a `socket address`, which consists of an `IP address` and `port` number that is used to connect with another machine.

With that in mind we have our `ParseIPEndPoint()` method, which takes the provided `server` string value in the typical form of `address:port`, and attempts to parse a valid `uri` from that provided server IP and port.  We aren't covering anywhere near the full spectrum of possible Uris or connection types here, just the basics with and without `http`, but this should illustrate the point.

If a valid `uri` is created, we return a new [`IPEndPoint`](https://docs.microsoft.com/en-us/dotnet/api/system.net.ipendpoint?view=netframework-4.7) instance, which is a .NET class that allows us to easily store network endpoint information (ip, port, etc).  If the `uri` parse fails, we throw an exception.

```cs
private static IPEndPoint ParseIPEndPoint(string server)
{
    Uri uri;
    // Try to generate a valid URI from server.
    if (Uri.TryCreate(server, UriKind.Absolute, out uri) || 
        Uri.TryCreate(String.Concat("http://", server), UriKind.Absolute, out uri))
    {
        // Create new IPEndPoint with parsed Uri arguments.
        return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);
    }
    // If Uri creation fails, invalid server parameter was passed.
    throw new FormatException($"Invalid server provided to ParseIPEndPoint of: {server}");
}
```

Now, we make use of the `ParseIPEndPoint()` method in the `ConnectToSocket()` method, which also expects a single string parameter representing our `ip:port`.  We parse that and generate an `IPEndPoint`, then create a new `Socket` using the `IPEndPoint` that was generated.  We're connecting via `TCP` protocol (just like most web browsers), so that's all fine.  Finally, we attempt to establish a connection to our `socket` instance via the `socket.Connect()` method.  If all goes well, we output a message indicating the connection was made, then `Close()` the connection.

```cs
private static void ConnectToSocket(string uri)
{
    try
    {
        // Create IPEndPoint.
        IPEndPoint endPoint = ParseIPEndPoint(uri);
        // Create socket from EndPoint arguments.
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        // Attempt connection.
        socket.Connect(endPoint);
        // Confirm connection with output.
        Logging.Log($"Connection established to: {endPoint.Address}:{endPoint.Port}");
        // Close connection.
        socket.Close();
    }
    catch (System.Net.Sockets.SocketException exception)
    {
        Logging.Log(exception);
    }
}
```

To test this out and see how a `System.Net.Sockets.SocketException` might be thrown, we have three basic calls to our `ConnectToSocket()` method, one that works and two that will fail:

```cs
static void Main(string[] args)
{
    // Working example, localhost with default port 80.
    ConnectToSocket("127.0.0.1");

    // Invalid example, localhost with inactive port 4444.
    ConnectToSocket("127.0.0.1:4444");

    // Invalid example, Google DNS with default port 80.
    ConnectToSocket("8.8.8.8");
}
```

The first call works fine because my localhost machine (`127.0.0.1`) accepts a connection to the default `http` port (`80`):

```
Connection established to: 127.0.0.1:80
```

However, the second attempt fails because my machine _does not_ accept connections on port `4444` (I tried to pick a random port number here, though some machines may run services on this port).  As a result, my computer refuses the connection, which causes our little application to throw a `System.Net.Sockets.SocketException`, as we expected:

```
[EXPECTED] System.Net.Sockets.SocketException (0x80004005): No connection could be made because the target machine actively refused it 127.0.0.1:4444
```

Lastly, we try establishing a socket connection with a remote server, in this case one of Google's DNS servers (`8.8.8.8`).  Once again, our connection attempt fails, though for a slightly different reason this time:

```
[EXPECTED] System.Net.Sockets.SocketException (0x80004005): A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 8.8.8.8:80
```

In this case, while it is valid to issue a `ping 8.8.8.8` request to Google's DNS, it doesn't allow a socket connection to be established, so the attempt fails and an exception is thrown.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A closer look at the System.Net.Sockets.SocketException in .NET, including some simple code examples for establishing socket connections using C#.