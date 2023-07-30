using System;
using System.IO;
using System.Net;
using System.Text;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Define the URL and port on which you want to listen
            string url = "http://localhost:9000/";

            // Create a new HttpListener instance
            HttpListener listener = new HttpListener();

            // Add the URL to the listener's prefix collection
            listener.Prefixes.Add(url);

            try
            {
                // Start the listener
                listener.Start();
                Console.WriteLine($"Server listening on {url}...");

                while (true)
                {
                    // Wait for an incoming request
                    HttpListenerContext context = listener.GetContext();

                    // Get the request and response objects
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    Console.WriteLine($"Received {request.HttpMethod} request from {request.RemoteEndPoint}");

                    // Check if the request URL starts with "/api"
                    if (request.Url.AbsolutePath.StartsWith("/api"))
                    {
                        // Set the response content type
                        response.ContentType = "text/plain";

                        // Create a response message
                        string responseMessage = "Hello, this is the API endpoint!";

                        // Convert the message to a byte array and send it as the response
                        byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
                        response.ContentLength64 = buffer.Length;
                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        // If the URL does not match the "/api" route, send a 404 Not Found response
                        response.StatusCode = 404;
                        response.StatusDescription = "Not Found";
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                // Stop the listener when done
                listener.Stop();
            }
        }
    }
}