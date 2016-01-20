using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Recover
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Nabor nabor = new Nabor();
        public MainWindow()
        {
            InitializeComponent();
            textBox.Text = "234";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            Thread nt = new Thread(cycl);
            nt.Start();
        }

        private void cycl()
        {
            string sample = ""; // *23AE809DDACAF96AF0FD78ED04B6A265E05AA257";
            Dispatcher.Invoke(new ThreadStart(delegate
            {
               sample = textBox.Text.ToString();
            }));
            MySqlConnection connection = new MySqlConnection("Database=stan1;Data Source=192.168.190.11;User Id=root;Password=550s550s;CharSet=utf8");
            connection.Open();
            int[] pod = new int[8];
            //string sample = textBox.Text.ToString();
            for (int i = 0; i < pod.Length; i++)
            {
                pod[i] = 0;
            }
            string stroka = "";
            while(true)
            {
                for (int n = (pod.Length-1); n >=0; n--)
                {
                    pod[n]++;
                    if (pod[n] < nabor.dd.Length)
                        break;
                    pod[n] = 1;
                }
                stroka = "";
                for (int i = 0; i < pod.Length; i++)
                {
                    if (pod[i] == 0)
                        continue;
                    stroka = stroka + nabor.dd[pod[i]];
                }
                //
                MySqlCommand myCommand = new MySqlCommand("SELECT PASSWORD(\"" + stroka + "\")", connection);
                //myCommand.Parameters.Clear();
                //myCommand.Parameters.AddWithValue("S", stroka);
                MySqlDataReader read = myCommand.ExecuteReader();
                read.Read();
                string h = read.GetString(0);
                read.Close();
                Dispatcher.Invoke(new ThreadStart(delegate
                {
                    textBox1.Text = stroka;
                }));
                string h_sh = h.Substring(0, sample.Length);
                if (h_sh == sample)
                    break;
            }
        }
    }
}
