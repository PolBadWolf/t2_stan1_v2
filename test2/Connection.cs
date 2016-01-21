using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using test2.Properties;

namespace test2
{
    class Connection
    {
        private static readonly Settings ps = Settings.Default;
        public static string connect;
        public MySqlConnection mySqlConnection;

        //public BDSettingsWindow bdSettingsWindow { get; set; }
        public BDSettingsWindow bdSettingsWindow;// { get; set; }

        public void Open()
        {
            try
            {
                connect = "Database=" + ps.DataBase + ";Data Source=" + ps.DataSource + ";User Id=" + ps.UserId + ";Password=" + ps.Password + ";CharSet=utf8";
                mySqlConnection = new MySqlConnection(connect);
                mySqlConnection.Open();
                MainWindow.mainWindow.BdStatus.Dispatcher.Invoke(delegate
                {
                    MainWindow.mainWindow.BdStatus.Text = "   BD open ok   ";
                });
            }
            catch
            {
                try
                {
                    ps.Reset();
                    connect = "Database=" + ps.DataBase + ";Data Source=" + ps.DataSource + ";User Id=" + ps.UserId + ";Password=" + ps.Password + ";CherSet=utf8";
                    mySqlConnection = new MySqlConnection(connect);
                    mySqlConnection.Open();
                }
                catch (Exception)
                {
                    bdSettingsWindow = new BDSettingsWindow();
                    bdSettingsWindow.label1.Content = "Ошибка подключения к БД";
                    bdSettingsWindow.ShowDialog();
                    MainWindow.mainWindow.BdStatus.Dispatcher.Invoke(delegate
                    {
                        MainWindow.mainWindow.BdStatus.Text = "   BD open error   ";
                    });
                }
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
