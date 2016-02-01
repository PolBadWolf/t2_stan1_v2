using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using test2.Properties;
using System.Threading;

namespace test2
{
    class Connection
    {
        private static readonly Settings ps = Settings.Default;
        public static string connect;
        public MySqlConnection mySqlConnection;

        //public BDSettingsWindow bdSettingsWindow { get; set; }
        public BDSettingsWindow bdSettingsWindow = null;// { get; set; }

        public void Open()
        {
            try
            {
                connect = "Database=" + ps.DataBase + ";Data Source=" + ps.DataSource + ";User Id=" + ps.UserId + ";Password=" + ps.Password + ";CharSet=utf8";
                mySqlConnection = new MySqlConnection(connect);
                mySqlConnection.Open();

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => MainWindow.mainWindow.ComStatus.Text = " Port=" + ps.Com));

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                    new Action(() => MainWindow.mainWindow.BdStatus.Text = " Status BD : ok   " ));
            }
            catch
            {
                throw (new Exception("Ошибка открытия БД: host=" + ps.DataSource + "   BD=" + ps.DataBase));
            }
        }

        public void Close()
        {
            try
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
