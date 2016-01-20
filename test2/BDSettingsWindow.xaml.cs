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
using System.Windows.Shapes;
using test2.Properties;

namespace test2
{
    /// <summary>
    /// Логика взаимодействия для BDSettingsWindow.xaml
    /// </summary>
    public partial class BDSettingsWindow : Window
    {
        //private readonly Connection connection = new Connection();
        private Settings ps = Settings.Default;

        public BDSettingsWindow()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ps.DataBase = textBox1.Text;
            ps.DataSource = textBox2.Text;
            ps.UserId = textBox3.Text;
            ps.Password = passwordBox1.Password;
            ps.Save();
            Close();
            //connection.Open();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ps.Reset();
            Close();
        }
    }
}
