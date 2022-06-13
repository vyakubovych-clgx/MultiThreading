namespace MultiThreading.OptionalTask1.Server;

public class MessageHistoryProcessor
{
    private static readonly LinkedList<string> Messages = new();
    private static readonly object LockObject = new();
    private const int MAX_MESSAGE_COUNT = 10;

    public void AddMessageToHistory(string message)
    {
        lock (LockObject)
        {
            Messages.AddLast(message);
            if (Messages.Count > MAX_MESSAGE_COUNT)
                Messages.RemoveFirst();
        }
    }

    public IEnumerable<string> GetMessageHistory()
    {
        lock(LockObject)
        {
            return Messages.ToArray();
        }
    }
}