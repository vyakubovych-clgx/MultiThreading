using MultiThreading.OptionalTask2.Server;

TcpListenerSingleton.Instance.GetTcpListener().Start();
Console.WriteLine("* Server is ready to accept connections. *");
new ServerWorker().Work();
