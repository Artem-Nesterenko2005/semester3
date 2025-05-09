using System.Net;
using System.Net.Sockets;
using System.Text;

// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace SimpleFTP
{
    /// <summary>
    /// A class for the server that processes two requests - Get and List.
    /// </summary>
    public class Server
    {
        private readonly TcpListener listener;

        private CancellationTokenSource ?token;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="port">Port for server.</param>
        public Server(int port)
        {
            this.listener = new (IPAddress.Any, port);
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <returns>Task submitted for execution.</returns>
        public async Task Start()
        {
            this.Initialize();
            List<Task> tasks = new ();
            while (!this.token!.IsCancellationRequested)
            {
                var client = await this.listener.AcceptTcpClientAsync();

                tasks.Add(Task.Run(
                    async () =>
                    {
                        using var stream = client.GetStream();
                        StreamReader reader = new (stream);
                        StreamWriter writer = new (stream) { AutoFlush = true };
                        var data = await reader.ReadLineAsync();
                        var elements = data!.Split(" ");

                        switch (elements[0])
                        {
                            case "1":
                                await this.List(elements[1], writer);
                                break;

                            case "2":
                                await this.Get(elements[1], writer);
                                break;

                            default:
                                break;
                        }
                    }));
            }

            await Task.WhenAll(tasks);
        }

        private void Initialize()
        {
            this.token = new CancellationTokenSource();
            this.listener.Start();
        }

        /// <summary>
        /// Stop server with cancellation request.
        /// </summary>
        public void Stop()
        {
            this.listener.Stop();
            this.token!.Cancel();
        }

        private async Task List(string path, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteAsync("-1");
                return;
            }

            var fileSystemEntries = Directory.GetFileSystemEntries(path);
            Array.Sort(fileSystemEntries);
            var size = fileSystemEntries.Length;

            await writer.WriteAsync($"{size}");
            foreach (var entry in fileSystemEntries)
            {
                await writer.WriteAsync($" {entry} {Directory.Exists(entry)}");
            }

            await writer.WriteAsync("\n");
        }

        private async Task Get(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteAsync("-1");
                return;
            }

            var data = await File.ReadAllTextAsync(path);
            await writer.WriteAsync($"{data.Length} {data}\n");
        }
    }
}
