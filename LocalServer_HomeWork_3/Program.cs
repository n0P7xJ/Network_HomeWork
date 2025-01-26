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

        await RunServerAsync(serverIP, serverPort); // запускаєм сервер для прийома повідомлень
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
        IPEndPoint serverEndPoint = new IPEndPoint(serverIP, serverPort);
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);
            DisplayLog("Server is running and waiting for connections...");

            while (true)
            {
                Socket clientSocket = await serverSocket.AcceptAsync();

                Task.Run(() => HandleClientAsync(clientSocket));
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Server error: {ex.Message}");
        }
    }

    private static async Task HandleClientAsync(Socket client)
    {
        try
        {
            // виділяєм пам'ять для повідомлення
            byte[] buffer = new byte[1024]; 
            int bytesRead;

            while ((bytesRead = await client.ReceiveAsync(buffer, SocketFlags.None)) > 0) // очікуєм повідомлення
            {
                //Відображаєм його
                string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                WriteLine($"USER:{message}");

                //Робим відповідь
                byte[] responseData = Encoding.Unicode.GetBytes(message);
                await client.SendAsync(responseData, SocketFlags.None);
            }
        }
        catch (Exception ex)
        {
            DisplayError($"Error with client {client.RemoteEndPoint}: {ex.Message}");
        }
    }

    private static void DisplayLog(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[INFO] {message}");
        Console.ResetColor();
    }
    private static void DisplayError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {error}");
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
