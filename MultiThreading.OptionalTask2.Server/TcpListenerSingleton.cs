using System.Net;
using System.Net.Sockets;

namespace MultiThreading.OptionalTask2.Server;

public class TcpListenerSingleton
{
    private static readonly TcpListener TcpListener = new(IPAddress.Parse("127.0.0.1"), 8006);
    private static readonly Lazy<TcpListenerSingleton> Lazy = new (() => new TcpListenerSingleton());

    public static TcpListenerSingleton Instance => Lazy.Value;

    public TcpListener GetTcpListener() => TcpListener;

    private TcpListenerSingleton() { }
}