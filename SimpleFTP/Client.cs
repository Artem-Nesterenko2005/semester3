using System.Net;
using System.Net.Sockets;
using System.Text;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace SimpleFTP
{
    /// <summary>
    /// Class for client executing requests.
    /// </summary>
    public class Client
    {
        private readonly IPEndPoint address;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="address">Client IP address.</param>
        public Client(IPEndPoint address)
        {
            this.address = address;
        }

        /// <summary>
        /// Performs input and output operations.
        /// </summary>
        /// <param name="path">Path to the directory or file.</param>
        /// <returns>Results of operation Get or List from server.</returns>
        public async Task<string> Processing(string path)
        {
            TcpClient client = new ();
            try
            {
                await client.ConnectAsync(this.address);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\nServer was stopped. Try start the server.");
                return default!;
            }

            var stream = client.GetStream();
            StreamWriter writer = new (stream) { AutoFlush = true };
            StreamReader reader = new (stream);

            await writer.WriteLineAsync(path);
            var result = await reader.ReadToEndAsync();

            return result;
        }
    }
}
