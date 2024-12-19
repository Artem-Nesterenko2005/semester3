// MIT License
// Copyright (c) 2024 Artem-Nesterenko2005
// All rights reserved

namespace TestTask3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class for server, listening to port.
/// </summary>
public static class Server
{
    /// <summary>
    /// Launches the client, allowing correspondence.
    /// </summary>
    /// <param name="port">Port for connect.</param>
    /// <returns>Return when client is closed.</returns>
    public static async Task ServerStart(int port)
    {
        TextReader reader = Console.In;
        TextWriter writer = Console.Out;

        TcpListener listener = new (IPAddress.Any, port);
        listener.Start();
        using var socket = await listener.AcceptSocketAsync();

        using var stream = new NetworkStream(socket);
        using var streamReader = new StreamReader(stream);
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };
        while (true)
        {
            var message = await streamReader.ReadLineAsync();
            if (message == "exit")
            {
                break;
            }

            await writer.WriteLineAsync(message);
            var answer = reader.ReadLine();
            await streamWriter.WriteLineAsync(answer);
            await writer.WriteLineAsync(answer);
            if (answer == "exit")
            {
                break;
            }
        }

        socket.Close();
    }
}
