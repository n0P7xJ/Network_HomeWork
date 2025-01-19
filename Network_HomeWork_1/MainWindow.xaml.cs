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
    /// </summary>
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
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portText) || string.IsNullOrEmpty(message))
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
                using (Socket server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp))
                {
                    var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    server.Connect(serverEndPoint);
                    var data = Encoding.Unicode.GetBytes(message); //текст повідомлення кодуємо в байти
                    server.Send(data); //відправляємо дані на сервер
                                       //тепер очікуємо відповіді від сервера
                    data = new byte[1024]; //дані куди буде зберігати відповідь
                    int bytes = 0; //розмір байтів у відповіді сервера
                    do
                    {
                        bytes = server.Receive(data); //отримали відповідь від сервера
                       ResponseTextBox.Text = Encoding.Unicode.GetString(data);

                    } while (server.Available > 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
