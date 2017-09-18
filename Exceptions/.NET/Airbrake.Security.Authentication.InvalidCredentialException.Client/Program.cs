using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using Utility;

namespace Airbrake.Security.Authentication.InvalidCredentialException
{
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
