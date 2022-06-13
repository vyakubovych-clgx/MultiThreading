using System.Net.Sockets;

namespace MultiThreading.OptionalTask2.Client;

public class ClientWorker
{
    private readonly RandomGenerator _randomGenerator;

    public ClientWorker()
    {
        _randomGenerator = new RandomGenerator();
    }

    public void Work()
    {
        try
        {
            while (true)
            {
                using var client = new TcpClient("localhost", 8006);
                using var networkStream = client.GetStream();
                using var streamWriter = new StreamWriter(networkStream);

                var clientName = _randomGenerator.GetRandomClientName();
                SendMessage(streamWriter, clientName);
                Console.WriteLine($"* Connected to server as {clientName}. *");

                Console.WriteLine("* Sending random messages to server... *");
                SendRandomMessages(streamWriter);
                Console.WriteLine("* Done sending messages. *");

                Console.WriteLine("* Receiving messages from server... *");
                ReadMessages(networkStream);
                Console.WriteLine("* Done receiving messages. *");

                Console.WriteLine("* Disconnected from server. *");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while connecting to server: {ex}");
        }
    }

    #region Private Methods

    private void SendRandomMessages(StreamWriter streamWriter)
    {
        for (var i = 0; i < _randomGenerator.GetRandomMessageCount(); i++)
        {
            SendMessage(streamWriter, _randomGenerator.GetRandomMessage());
            Thread.Sleep(_randomGenerator.GetRandomDelay());
        }
    }

    private static void SendMessage(StreamWriter streamWriter, string message)
    {
        streamWriter.WriteLine(message);
        streamWriter.Flush();
    }

    private static void ReadMessages(NetworkStream networkStream)
    {
        using var streamReader = new StreamReader(networkStream, leaveOpen: true);
        while (streamReader.Peek() != -1)
            Console.WriteLine(streamReader.ReadLine());
    }

    #endregion
}