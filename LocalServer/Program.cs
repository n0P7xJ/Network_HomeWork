using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private static async Task Main(string[] args)
    {
        WriteLine("Start...");

        IPAddress serverIP = GetServerIP();
        int serverPort = GetServerPort();

        await RunServerAsync(serverIP, serverPort);
    }
    private static IPAddress GetServerIP()
    {
        string hostName = Dns.GetHostName();
        WriteLine($"Host name: {hostName}\n");

        var localHost = Dns.GetHostEntry(hostName);
        WriteLine("Available IPs:");
        for (int i = 0; i < localHost.AddressList.Length; ++i)
        {
            WriteLine($"{i + 1}. {localHost.AddressList[i]}");
        }

        while (true)
        {
            Write("Enter server IP (number): _>");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= localHost.AddressList.Length)
            {
                return localHost.AddressList[choice - 1];
            }
            DisplayError("Invalid input. Please enter a valid number.");
        }
    }

    private static int GetServerPort()
    {
        while (true)
        {
            Write("Enter server port: _>");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int port) && port > 0 && port <= 65535)
            {
                return port;
            }
            DisplayError("Invalid port. Please enter a number between 1 and 65535.");
        }
    }

    private static async Task RunServerAsync(IPAddress serverIP, int serverPort)
    {
        IPEndPoint iPEndPoint = new IPEndPoint(serverIP, serverPort);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            server.Bind(iPEndPoint);
            server.Listen(10);
            DisplayLog("\nServer is running\n");

            while (true)
            {
                Socket client = await server.AcceptAsync();
                Console.WriteLine("--------------------------------------");
                DisplayLog($"Client {client.RemoteEndPoint} connected to server");

                Task.Run(() => HandleClientAsync(client));
                Console.WriteLine("--------------------------------------\n");
            }
        }
        catch (Exception ex)
        {
            DisplayError(ex.Message);
        }
    }

    private static async Task HandleClientAsync(Socket client)
    {
        try
        {
            byte[] buffer = new byte[1024];
            int bytes;
            do
            {
                bytes = await client.ReceiveAsync(buffer, SocketFlags.None);
                string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                Console.WriteLine("--------------------------------------");
                WriteLine($"{message} from client: {client.RemoteEndPoint}");
                Console.WriteLine("--------------------------------------\n");
            } while (client.Available > 0);

            string response = $"[SERVER] Message received at {DateTime.Now}";
            buffer = Encoding.Unicode.GetBytes(response);
            await client.SendAsync(buffer, SocketFlags.None);
        }
        catch (Exception ex)
        {
            DisplayError($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }

    private static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    private static void DisplayLog(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine(message);
        Console.ResetColor();
    }
    private static void WriteLine(string text)
    {
       Console.WriteLine("[SERVER] " + text);
    }
    private static void Write(string text)
    {
       Console.Write("[SERVER] " + text);
    }
}