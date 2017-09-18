# .NET Exceptions - System.Security.Authentication.InvalidCredentialException

Making our way through the detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series we've been creating, today we'll be looking into the **System.Security.Authentication.InvalidCredentialException**.  The `InvalidCredentialException` occurs when authentication fails for one of a number of authentication stream classes in the .NET Framework.

In this article we'll explore the `InvalidCredentialException` by looking at where it resides in the overall .NET exception hierarchy.  Then, we'll also show a fully-functional code sample that illustrates how an authentication stream might be created between a client and a listener (server), both successfully and failingly, so let's get to it!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.Security.Authentication.AuthenticationException`](https://docs.microsoft.com/en-us/dotnet/api/system.security.authentication.authenticationexception?view=netframework-4.7)
                - `InvalidCredentialException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using Utility;

namespace Airbrake.Security.Authentication.InvalidCredentialException
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logging.LineSeparator("CLIENT CONNECTION TEST", 60);
                var client = new Client();

                Logging.LineSeparator("CLIENT CONNECTION TEST (w/ CREDENTIALS)", 60);
                var invalidClient = new Client("username", "password");
            }
            catch (System.Security.Authentication.InvalidCredentialException exception)
            {
                Logging.Log(exception);
            }
        }
    }

    internal class Client
    {
        private static TcpClient _client;

        public Client()
        {
            Connect();
        }

        public Client(string username, string password)
        {
            Connect(username, password);
        }

        private static void Connect(string username = null, string password = null, string host = "localhost", int addressIndex = 1, int port = 11000)
        {
            // Get IP address.
            var ipAddress = Dns.GetHostEntry(host).AddressList[addressIndex];
            // Get remote end point.
            var endPoint = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket.
            _client = new TcpClient();
            // Connect the socket to the remote endpoint.
            _client.Connect(endPoint);
            Logging.Log($"Client successfully connected to {endPoint}.");
            // Keep connection alive if transfer isn't complete.
            _client.LingerState = new LingerOption(true, 0);
            // Get negotiation stream from client.
            var negotiateStream = new NegotiateStream(_client.GetStream(), false);
            // Pass the NegotiateStream as the AsyncState object 
            // so that it is available to the callback delegate.


            IAsyncResult asyncResult;

            // If username/password provided, use as credentials.
            if (username != null && password != null)
            {
                asyncResult = negotiateStream.BeginAuthenticateAsClient(
                    new NetworkCredential("username", "password"),
                    host,
                    EndAuthenticateCallback,
                    negotiateStream);
            }
            else
            {
                // Use Identification ImpersonationLevel
                asyncResult = negotiateStream.BeginAuthenticateAsClient(
                    EndAuthenticateCallback,
                    negotiateStream
                );
            }
            
            Logging.Log("Client attempting to authenticate.");
            // Await result.
            asyncResult.AsyncWaitHandle.WaitOne();

            // Send encoded test message to server.
            var message = Encoding.UTF8.GetBytes("Hello, it's me, the client!");
            asyncResult = negotiateStream.BeginWrite(
                message, 
                0, 
                message.Length,
                EndWriteCallback,
                negotiateStream);

            // Await result.
            asyncResult.AsyncWaitHandle.WaitOne();
            Logging.Log($"Successfully sent message containing {message.Length} bytes.");

            // Close the client connection.
            negotiateStream.Close();
            Logging.Log("Client closed.");
        }

        /// <summary>
        /// Invoked when authentication completes.
        /// </summary>
        /// <param name="asyncResult">Authentication result.</param>
        public static void EndAuthenticateCallback(IAsyncResult asyncResult)
        {
            try
            {
                var authStream = (NegotiateStream) asyncResult.AsyncState;
                if (authStream.IsAuthenticated)
                {
                    Logging.Log($"Ending authentication with ImpersonationLevel: {authStream.ImpersonationLevel}");
                }

                // End operation.
                authStream.EndAuthenticateAsClient(asyncResult);
            }
            catch (System.Security.Authentication.InvalidCredentialException exception)
            {
                // Output expected InvalidCredentialExceptions.
                Logging.Log(exception);
            }
        }

        /// <summary>
        /// Invoked when writing completes.
        /// </summary>
        /// <param name="asyncResult">Write result.</param>
        public static void EndWriteCallback(IAsyncResult asyncResult)
        {
            var authStream = (NegotiateStream) asyncResult.AsyncState;
            if (authStream.IsAuthenticated)
            {
                Logging.Log($"Ending write operation with ImpersonationLevel: {authStream.ImpersonationLevel}");
            }

            // End operation.
            authStream.EndWrite(asyncResult);
        }
    }
}
```

```cs
// Server.cs
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using Utility;

namespace Airbrake.Security.Authentication.InvalidCredentialException
{
    internal class Listener
    {
        public static void Main()
        {
            CreateListener(IPAddress.Any);
        }

        private static void CreateListener(IPAddress ipAddress, int port = 11000)
        {
            // Create listener.
            var listener = new TcpListener(ipAddress, 11000);
            // Listen for client connections.
            listener.Start();

            while (true)
            {
                // Awaits incoming connection.
                var tcpClient = listener.AcceptTcpClient();
                try
                {
                    Logging.LineSeparator("CLIENT CONNECTED");
                    // Authenticate the client.
                    AuthenticateClient(tcpClient);
                }
                catch (System.Security.Authentication.InvalidCredentialException exception)
                {
                    // Output expected InvalidCredentialExceptions.
                    Logging.Log(exception);
                }
                catch (Exception exception)
                {
                    // Output unexpected Exceptions.
                    Logging.Log(exception, false);
                }
            }
        }

        public static void AuthenticateClient(TcpClient client)
        {
            var networkStream = client.GetStream();
            // Create the NegotiateStream.
            var negotiateStream = new NegotiateStream(networkStream, false);
            // Combine client and NegotiateStream instance into ClientState.
            var clientState = new ClientState(negotiateStream, client);

            // Listen for the client authentication request.
            negotiateStream.BeginAuthenticateAsServer(
                EndAuthenticateCallback,
                clientState
            );

            // Wait until the authentication completes.
            clientState.Waiter.WaitOne();
            clientState.Waiter.Reset();

            // Receive encoded message sent by client.
            negotiateStream.BeginRead(
                clientState.Buffer, 
                0, 
                clientState.Buffer.Length,
                EndReadCallback,
                clientState);
            clientState.Waiter.WaitOne();

            // Close stream and client.
            negotiateStream.Close();
            client.Close();
        }

        public static void EndAuthenticateCallback(IAsyncResult asyncResult)
        {
            // Get the saved data.
            var clientState = (ClientState) asyncResult.AsyncState;
            var negotiateStream = (NegotiateStream) clientState.AuthenticatedStream;
            Logging.Log("Ending authentication.");

            try
            {
                // This call blocks until the authentication is complete.
                negotiateStream.EndAuthenticateAsServer(asyncResult);
                // Display properties of the authenticated client.
                var identity = negotiateStream.RemoteIdentity;
                Logging.Log($"{identity.Name} was authenticated using {identity.AuthenticationType}.");
                clientState.Waiter.Set();
            }
            catch (System.Security.Authentication.InvalidCredentialException exception)
            {
                Logging.Log(exception);
                clientState.Waiter.Set();
            }
            catch (AuthenticationException exception)
            {
                Logging.Log(exception, false);
                clientState.Waiter.Set();
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
                clientState.Waiter.Set();
            }
        }
        public static void EndReadCallback(IAsyncResult asyncResult)
        {
            // Get the saved data.
            var clientState = (ClientState)asyncResult.AsyncState;
            var negotiateStream = (NegotiateStream)clientState.AuthenticatedStream;

            try
            {
                var dataBytes = negotiateStream.EndRead(asyncResult);
                clientState.Message.Append(Encoding.UTF8.GetChars(clientState.Buffer, 0, dataBytes));
                if (dataBytes != 0)
                {
                    negotiateStream.BeginRead(clientState.Buffer, 0, clientState.Buffer.Length,
                        EndReadCallback,
                        clientState);
                    return;
                }
                var identity = negotiateStream.RemoteIdentity;
                Logging.Log($"{identity.Name} says {clientState.Message}");
                clientState.Waiter.Set();
            }
            catch (System.Security.Authentication.InvalidCredentialException exception)
            {
                Logging.Log(exception);
                clientState.Waiter.Set();
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
                clientState.Waiter.Set();
            }
        }
    }

    /// <summary>
    /// Basic asynchronous object.
    /// </summary>
    internal class ClientState
    {
        private StringBuilder _message;

        internal ClientState(AuthenticatedStream authenticatedStream, TcpClient client)
        {
            AuthenticatedStream = authenticatedStream;
            Client = client;
        }
        internal TcpClient Client { get; }

        internal AuthenticatedStream AuthenticatedStream { get; }

        internal byte[] Buffer { get; } = new byte[2048];

        internal StringBuilder Message => _message ?? (_message = new StringBuilder());

        internal ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
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

The whole of the connectivity support provided in the .NET Framework is far too intricate and advanced to even begin to scratch the surface in this tiny tutorial.  Therefore, we'll simply rely on the code sample to illustrate the basics of (one small) example for how a client/server connection might be established to send some data, and how failure to authenticate can potentially lead to a `InvalidCredentialException`.

In the realm of DevOps, there's little doubt when it comes to the "chicken-or-the-egg" concerns of the server and the client: The server always comes first.  Thus, we'll continue this trend by going over our server project code first.  We accomplish this with a `TcpListener` instance, which blocks the thread while listening for incoming connections from a TCP client.  This occurs within the `Listener` class:

```cs
internal class Listener
{
    public static void Main()
    {
        CreateListener(IPAddress.Any);
    }

    private static void CreateListener(IPAddress ipAddress, int port = 11000)
    {
        // Create listener.
        var listener = new TcpListener(ipAddress, 11000);
        // Listen for client connections.
        listener.Start();

        while (true)
        {
            // Awaits incoming connection.
            var tcpClient = listener.AcceptTcpClient();
            try
            {
                Logging.LineSeparator("CLIENT CONNECTED");
                // Authenticate the client.
                AuthenticateClient(tcpClient);
            }
            catch (System.Security.Authentication.InvalidCredentialException exception)
            {
                // Output expected InvalidCredentialExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

    // ...
}
```

The `CreateListener(IPAddress ipAddress, int port = 11000)` method performs the basic creation and execution of the `TcpListener`, then blocks until a client connects, which we then attempt to authenticate via the `AuthenticateClient(TcpClient client)` method:

```cs
public static void AuthenticateClient(TcpClient client)
{
    var networkStream = client.GetStream();
    // Create the NegotiateStream.
    var negotiateStream = new NegotiateStream(networkStream, false);
    // Combine client and NegotiateStream instance into ClientState.
    var clientState = new ClientState(negotiateStream, client);

    // Listen for the client authentication request.
    negotiateStream.BeginAuthenticateAsServer(
        EndAuthenticateCallback,
        clientState
    );

    // Wait until the authentication completes.
    clientState.Waiter.WaitOne();
    clientState.Waiter.Reset();

    // Receive encoded message sent by client.
    negotiateStream.BeginRead(
        clientState.Buffer, 
        0, 
        clientState.Buffer.Length,
        EndReadCallback,
        clientState);
    clientState.Waiter.WaitOne();

    // Close stream and client.
    negotiateStream.Close();
    client.Close();
}
```

We start by converting the `TcpClient` stream into a `NegotiateStream`, which uses the [`negotiate security policy`](https://msdn.microsoft.com/en-us/library/windows/desktop/aa378748(v=vs.85).aspx) to determine the best way for the client/server to authenticate with one another.  The generated `ClientState` instance can be passed to the `negotiateStream.BeginAuthenticateAsServer(...)` method, along with a reference to our callback method, which awaits the client's authentication request.

Once client authentication is complete, we then use the stream's `BeginRead(...)` method to read the byte-message that was sent by the client, before closing the stream and client entirely.

Both the callback methods are fairly simple:

```cs
public static void EndAuthenticateCallback(IAsyncResult asyncResult)
{
    // Get the saved data.
    var clientState = (ClientState) asyncResult.AsyncState;
    var negotiateStream = (NegotiateStream) clientState.AuthenticatedStream;
    Logging.Log("Ending authentication.");

    try
    {
        // This call blocks until the authentication is complete.
        negotiateStream.EndAuthenticateAsServer(asyncResult);
        // Display properties of the authenticated client.
        var identity = negotiateStream.RemoteIdentity;
        Logging.Log($"{identity.Name} was authenticated using {identity.AuthenticationType}.");
        clientState.Waiter.Set();
    }
    catch (System.Security.Authentication.InvalidCredentialException exception)
    {
        Logging.Log(exception);
        clientState.Waiter.Set();
    }
    catch (AuthenticationException exception)
    {
        Logging.Log(exception, false);
        clientState.Waiter.Set();
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
        clientState.Waiter.Set();
    }
}

public static void EndReadCallback(IAsyncResult asyncResult)
{
    // Get the saved data.
    var clientState = (ClientState)asyncResult.AsyncState;
    var negotiateStream = (NegotiateStream)clientState.AuthenticatedStream;

    try
    {
        var dataBytes = negotiateStream.EndRead(asyncResult);
        clientState.Message.Append(Encoding.UTF8.GetChars(clientState.Buffer, 0, dataBytes));
        if (dataBytes != 0)
        {
            negotiateStream.BeginRead(clientState.Buffer, 0, clientState.Buffer.Length,
                EndReadCallback,
                clientState);
            return;
        }
        var identity = negotiateStream.RemoteIdentity;
        Logging.Log($"{identity.Name} says {clientState.Message}");
        clientState.Waiter.Set();
    }
    catch (System.Security.Authentication.InvalidCredentialException exception)
    {
        Logging.Log(exception);
        clientState.Waiter.Set();
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
        clientState.Waiter.Set();
    }
}
```

`EndAuthenticateCallback(IAsyncResult asyncResult)` basically performs the same process we did before to **begin** authentication, but in reverse.  After converting the `IAsyncResult` parameter to a `NegotiateStream` once again, we attempt to end the authentication process via `EndAuthenticateAsServer(...)`, then outputting the authentication result.  We also make sure to invoke `Waiter.Set()` on the client, which frees up the current thread for other execution.

The `EndReadCallback(IAsyncResult asyncResult)` method does much the same, except it finalizes reading of the byte data sent by the client.

Lastly for the server, the `ClientState` class is just the basic object, with a few important properties, that we're passing along as our asynchronous object when attempting to authenticate the client:

```cs
/// <summary>
/// Basic asynchronous object.
/// </summary>
internal class ClientState
{
    private StringBuilder _message;

    internal ClientState(AuthenticatedStream authenticatedStream, TcpClient client)
    {
        AuthenticatedStream = authenticatedStream;
        Client = client;
    }
    internal TcpClient Client { get; }

    internal AuthenticatedStream AuthenticatedStream { get; }

    internal byte[] Buffer { get; } = new byte[2048];

    internal StringBuilder Message => _message ?? (_message = new StringBuilder());

    internal ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
}
```

Now, let's move on to the client code, which is contained in the `Client` class:

```cs
internal class Client
{
    private static TcpClient _client;

    public Client()
    {
        Connect();
    }

    public Client(string username, string password)
    {
        Connect(username, password);
    }

    private static void Connect(string username = null, string password = null, string host = "localhost", int addressIndex = 1, int port = 11000)
    {
        // Get IP address.
        var ipAddress = Dns.GetHostEntry(host).AddressList[addressIndex];
        // Get remote end point.
        var endPoint = new IPEndPoint(ipAddress, port);
        // Create a TCP/IP socket.
        _client = new TcpClient();
        // Connect the socket to the remote endpoint.
        _client.Connect(endPoint);
        Logging.Log($"Client successfully connected to {endPoint}.");
        // Keep connection alive if transfer isn't complete.
        _client.LingerState = new LingerOption(true, 0);
        // Get negotiation stream from client.
        var negotiateStream = new NegotiateStream(_client.GetStream(), false);
        // Pass the NegotiateStream as the AsyncState object 
        // so that it is available to the callback delegate.


        IAsyncResult asyncResult;

        // If username/password provided, use as credentials.
        if (username != null && password != null)
        {
            asyncResult = negotiateStream.BeginAuthenticateAsClient(
                new NetworkCredential("username", "password"),
                host,
                EndAuthenticateCallback,
                negotiateStream);
        }
        else
        {
            // Use Identification ImpersonationLevel
            asyncResult = negotiateStream.BeginAuthenticateAsClient(
                EndAuthenticateCallback,
                negotiateStream
            );
        }
        
        Logging.Log("Client attempting to authenticate.");
        // Await result.
        asyncResult.AsyncWaitHandle.WaitOne();

        // Send encoded test message to server.
        var message = Encoding.UTF8.GetBytes("Hello, it's me, the client!");
        asyncResult = negotiateStream.BeginWrite(
            message, 
            0, 
            message.Length,
            EndWriteCallback,
            negotiateStream);

        // Await result.
        asyncResult.AsyncWaitHandle.WaitOne();
        Logging.Log($"Successfully sent message containing {message.Length} bytes.");

        // Close the client connection.
        negotiateStream.Close();
        Logging.Log("Client closed.");
    }

    // ...
}
```

The `Connect(...)` method performs most of the logic, which is explained by the comments so we won't go through it step by step.  In essence, the client creates a `TcpClient` instance and attempts to connect to the specified host and port.  Once connected, authentication is attempted by invoking the `NegotiateStream.BeginAuthenticateAsClient(...)` method (the opposite of the `Server` version we used in the server code).  If authentication succeeds, it then attempts to send a simple message to the server, in the form of encoded bytes, before closing the stream.

The `EndAuthenticateCallback(IAsyncResult asyncResult)` and `EndWriteCallback(IAsyncResult asyncResult)` methods perform the opposing actions that we saw on the server side, this time invoking `EndAuthenticateAsClient(...)` and `EndWrite(...)`, respectively:

```cs
/// <summary>
/// Invoked when authentication completes.
/// </summary>
/// <param name="asyncResult">Authentication result.</param>
public static void EndAuthenticateCallback(IAsyncResult asyncResult)
{
    try
    {
        var authStream = (NegotiateStream) asyncResult.AsyncState;
        if (authStream.IsAuthenticated)
        {
            Logging.Log($"Ending authentication with ImpersonationLevel: {authStream.ImpersonationLevel}");
        }

        // End operation.
        authStream.EndAuthenticateAsClient(asyncResult);
    }
    catch (System.Security.Authentication.InvalidCredentialException exception)
    {
        // Output expected InvalidCredentialExceptions.
        Logging.Log(exception);
    }
}

/// <summary>
/// Invoked when writing completes.
/// </summary>
/// <param name="asyncResult">Write result.</param>
public static void EndWriteCallback(IAsyncResult asyncResult)
{
    var authStream = (NegotiateStream) asyncResult.AsyncState;
    if (authStream.IsAuthenticated)
    {
        Logging.Log($"Ending write operation with ImpersonationLevel: {authStream.ImpersonationLevel}");
    }

    // End operation.
    authStream.EndWrite(asyncResult);
}
```

Now, to test everything out we need to execute both our projects simultaneously, starting with the `Listener` class.  Once it is up and running, we can then test out some client connections, as seen in the code below:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Logging.LineSeparator("CLIENT CONNECTION TEST", 60);
            var client = new Client();

            Logging.LineSeparator("CLIENT CONNECTION TEST (w/ CREDENTIALS)", 60);
            var invalidClient = new Client("username", "password");
        }
        catch (System.Security.Authentication.InvalidCredentialException exception)
        {
            Logging.Log(exception);
        }
    }
}
```

Nothing too fancy going on here.  We're simply creating new `Client` instances, the first with default values, and the second by using credential authentication with a username and password.  Both our client and server are running separately, but, if all goes as expected, they'll connect to one another and the various `Logging.Log(...)` outputs in the code will show some results.

Here is the output produced from the first `new Client();` call, on the **client** side:

```
------------------ CLIENT CONNECTION TEST ------------------
Client successfully connected to 127.0.0.1:11000.
Client attempting to authenticate.
Ending authentication with ImpersonationLevel: Identification
Ending write operation with ImpersonationLevel: Identification
Successfully sent message containing 27 bytes.
Client closed.
```

And here's the output from the **server**:

```
----------- CLIENT CONNECTED -----------
Ending authentication.
I7\Gabe was authenticated using NTLM.
I7\Gabe says Hello, it's me, the client!
```

Cool!  Everything seems to be working as expected.  As you can see from the client output, the default `ImpersonationLevel` when we don't provide any credentials is `Identification`.  Since both the client and server are running on the `localhost` (`127.0.0.1:11000`), there's no trouble authenticating, so the client was able to send its message and the server received it!

Now, let's see what the output is from the second `Client("username", "password")` call, starting with the **client**:

```
--------- CLIENT CONNECTION TEST (w/ CREDENTIALS) ----------
Client successfully connected to 127.0.0.1:11000.
Client attempting to authenticate.
[EXPECTED] System.Security.Authentication.InvalidCredentialException: The server has rejected the client credentials. ---> System.ComponentModel.Win32Exception: The logon attempt failed
```

And the **server** output:

```
----------- CLIENT CONNECTED -----------
Ending authentication.
[EXPECTED] System.Security.Authentication.InvalidCredentialException: The server has rejected the client credentials. ---> System.ComponentModel.Win32Exception: The logon attempt failed
[EXPECTED] System.Security.Authentication.InvalidCredentialException: The server has rejected the client credentials. ---> System.ComponentModel.Win32Exception: The logon attempt failed
```

Lo and behold, a `InvalidCredentialException` is thrown our way, indicating on both the client and server side that the logon attempt using `"username"` and `"password"` as our credentials has failed.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A in-depth look at the InvalidCredentialException in .NET, including C# code illustrating how to create and connect clients and servers together.