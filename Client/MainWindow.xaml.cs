using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Network_HomeWork_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> .
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = IpTextBox.Text;
            string portText = PortTextBox.Text;
            string message = MessageTextBox.Text;
            MessageTextBox.Text = "";
            string userName = NameTextBox.Text;

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portText) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(portText, out int port))
            {
                MessageBox.Show("Invalid port number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    await server.ConnectAsync(serverEndPoint);
                    Console.WriteLine("Connected to server.");

                    // Відправляємо повідомлення з ім'ям користувача
                    string fullMessage = $"{userName}=> {message}";
                    byte[] data = Encoding.Unicode.GetBytes(fullMessage);
                    await Task.Run(() => server.Send(data, SocketFlags.None));

                    // Очікуємо відповідь від сервера
                    byte[] buffer = new byte[1024];
                    int bytesRead = await Task.Run(() => server.Receive(buffer, SocketFlags.None));
                    string response = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                    ResponseTextBox.Text += $"{response}\n"; // Вивід відповіді на екран
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static int PortToInt(string text)
        {
            while (true)
            {
                if (int.TryParse(text, out int port) && port > 0 && port <= 65535)
                {
                    return port;
                }
            }
        }

    }
}
