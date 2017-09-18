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