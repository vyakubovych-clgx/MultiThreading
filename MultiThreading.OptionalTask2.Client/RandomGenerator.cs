namespace MultiThreading.OptionalTask2.Client;

public class RandomGenerator
{
    private readonly string[] _messages;
    private readonly int _maxMessagesSentPerConnection;
    private readonly int _maxDelayBetweenMessages;
    private readonly Random _random;

    public RandomGenerator()
    {
        _random = new Random();
        _messages = File.ReadAllLines("Messages.txt");
        _maxMessagesSentPerConnection = 10;
        _maxDelayBetweenMessages = 3000;
    }

    public string GetRandomMessage() => _messages[_random.Next(_messages.Length)];

    public string GetRandomClientName() => $"Client{_random.Next(10000, 100000)}";

    public int GetRandomMessageCount() => _random.Next(1, _maxMessagesSentPerConnection + 1);

    public int GetRandomDelay() => _random.Next(_maxDelayBetweenMessages + 1);
}