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
/// Class for client, connects to the specified server.
/// </summary>
public static class Client
{
    /// <summary>
    /// Launches the client, connects to the server, allowing correspondence.
    /// </summary>
    /// <param name="port">Port for connect.</param>
    /// <param name="address">Address for connect.</param>
    /// <returns>Return when client is closed.</returns>
    public static async Task ClientStart(int port, string address)
    {
        TextReader reader = Console.In;
        TextWriter writer = Console.Out;
        using var client = new TcpClient(address, port);

        using var stream = client.GetStream();
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };
        using var streamReader = new StreamReader(stream);

        while (true)
        {
            var message = reader.ReadLine();
            await streamWriter.WriteLineAsync(message);

            if (message == "exit")
            {
                break;
            }

            await writer.WriteLineAsync(message);
            var answer = await streamReader.ReadLineAsync();
            await writer.WriteLineAsync(answer);
            if (answer == "exit")
            {
                break;
            }
        }
    }
}
