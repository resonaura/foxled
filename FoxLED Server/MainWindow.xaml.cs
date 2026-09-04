using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using SocketLibrary;
using System.Text.RegularExpressions;

namespace FoxLED_Server
{   
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // State object for reading client data asynchronously  
        public class StateObject
        {
            // Client  socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 1024;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        public LEDConnect LD = new LEDConnect();
        private bool enabled = false;
        public MainWindow()
        {
            InitializeComponent();
            

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if(enabled)
                    {
                        using (var listener = new SocketListener(1337)) // Start listening
                        {
                            using (var remote = listener.Accept()) // Accepts a connection (blocks execution)
                            {
                                var data = remote.Receive(); // Receives data (blocks execution)
                                string[] nums = data.Split('a');


                                byte[] newMAP = new byte[nums.Length];
                                foreach (string c in nums)
                                {
                                    if (c != "")
                                    {
                                        string mynumber = Regex.Replace(c, @"\D", "");
                                        newMAP = addByteToArray(newMAP, (byte)Convert.ToInt32(mynumber));
                                    }

                                }
                                LEDConnect.LED_COUNT = nums.Length / 3;
                                LD.Display(newMAP);
                                //Debug.WriteLine(data);
                                //remote.Send("done"); // Sends the received data back
                            }
                        }
                        
                    }
                    Thread.Sleep(1);

                }

            });
        }
        private byte[] addByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            switch(enabled)
            {
                case true:
                    MainButton.Content = "Запустить";
                    LD.Stop();
                    enabled = false;
                    break;
                default:
                    MainButton.Content = "Остановить";
                    LD.AutoConnect();
                    enabled = true;
                    break;
            }
            
        }
    }
}
