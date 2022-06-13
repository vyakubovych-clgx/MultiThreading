namespace MultiThreading.OptionalTask1.Server;

public class ClientsHandler
{
    private static readonly List<StreamWriter> Streams = new();
    private static readonly object LockObject = new();

    public void AddNewClient(StreamWriter clientStreamWriter)
    {
        lock (LockObject)
            Streams.Add(clientStreamWriter);
    }

    public void DeleteClient(StreamWriter clientStreamWriter)
    {
        lock(LockObject)
            Streams.Remove(clientStreamWriter);
    }

    public int SendMessageToAllClients(string message)
    {
        var errors = 0;

        lock (LockObject)
            foreach (var stream in Streams)
                try
                {
                    stream.WriteLine(message);
                    stream.Flush();
                }
                catch
                {
                    errors++;
                }

        return errors;
    }
}