using System.Net.Sockets;

namespace MultiThreading.OptionalTask1.Server;

public class ServerWorker
{
    private readonly MessageHistoryProcessor _messageHistoryProcessor;
    private readonly TcpListenerSingleton _tcpListener;
    private readonly ClientsHandler _clientsHandler;

    public ServerWorker()
    {
        _messageHistoryProcessor = new MessageHistoryProcessor();
        _tcpListener = TcpListenerSingleton.Instance;
        _clientsHandler = new ClientsHandler();
    }

    public void Work()
    {
        using var clientSocket = _tcpListener.GetTcpListener().AcceptSocket();
        new Thread(Work).Start();

        if (clientSocket.Connected)
        {
            using var networkStream = new NetworkStream(clientSocket);
            using var streamReader = new StreamReader(networkStream);
            using var streamWriter = new StreamWriter(networkStream);
            
            var clientName = HandleConnect(streamReader, streamWriter);

            while (true)
                try
                {
                    ReceiveAndProcessMessage(clientName, streamReader);
                }
                catch (IOException)
                {
                    HandleDisconnect(clientName, streamWriter);
                    break;
                }
        }
    }

    #region Private Methods

    private string HandleConnect(StreamReader streamReader, StreamWriter streamWriter)
    {
        _clientsHandler.AddNewClient(streamWriter);

        var clientName = AcceptClientName(streamReader);
        Console.WriteLine($"* {clientName} connected to server. *");

        SendMessageHistory(clientName, streamWriter);
        return clientName;
    }

    private void HandleDisconnect(string clientName, StreamWriter streamWriter)
    {
        Console.WriteLine($"* {clientName} disconnected from server. *");
        _clientsHandler.DeleteClient(streamWriter);
    }

    private static string AcceptClientName(StreamReader streamReader)
    {
        var clientName = streamReader.ReadLine();
        Thread.CurrentThread.Name = clientName;
        return clientName;
    }

    private void ReceiveAndProcessMessage(string clientName, StreamReader streamReader)
    {
        var messageContent = streamReader.ReadLine();
        
        if (string.IsNullOrEmpty(messageContent)) 
            return;

        var message = $"{clientName}: {messageContent}";
        _messageHistoryProcessor.AddMessageToHistory(message);
        Console.WriteLine(message);

        var errorCount = _clientsHandler.SendMessageToAllClients(message);
        if (errorCount > 0)
            Console.WriteLine($"* Failed to send message to {errorCount} client(s) *");
    }

    private void SendMessageHistory(string clientName, StreamWriter streamWriter)
    {
        try
        {
            foreach (var message in _messageHistoryProcessor.GetMessageHistory())
            {
                streamWriter.WriteLine(message);
                streamWriter.Flush();
            }
        }
        catch (IOException)
        {
            HandleDisconnect(clientName, streamWriter);
        }
    }

    #endregion
}
