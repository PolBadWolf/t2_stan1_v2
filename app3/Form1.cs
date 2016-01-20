using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app3
{
    public partial class Form1 : Form
    {
        MySqlConnection myConnect = null;
        string strConnect = "Database=old_29.12;Data Source=192.168.190.11;User Id=root;Password=550s550s;CharSet=utf8;";
        Thread thr = null;

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            strConnect = "Database=" + textBox_DataBase.Text + ";Data Source=" + textBox_DataSource.Text + ";User Id=" + textBox_UserId.Text + ";Password=" + textBox_Password.Text + ";CharSet=utf8;";
            thr = new Thread(cycle);
            thr.Start();
        }
        //=====================
        void cycle()
        {
            Int64 countMax = 0;
            try
            {
                myConnect = new MySqlConnection(strConnect);
                myConnect.Open();
                MySqlCommand myCommand1;
                MySqlCommand myCommand2;

                {
                    myCommand1 = new MySqlCommand(@"
SELECT COUNT(defectsdata.IndexData) FROM defectsdata WHERE defectsdata.FlDefectTube IS NULL
", myConnect);
                    MySqlDataReader reader = myCommand1.ExecuteReader();
                    reader.Read();
                    countMax = reader.GetInt64(0);
                    reader.Close();
                }
                myCommand1 = new MySqlCommand(@"
SELECT
defectsdata.IndexData,
defectsdata.DataSensors
FROM
defectsdata
WHERE
defectsdata.FlDefectTube IS NULL
", myConnect);
                MySqlDataReader myReader = myCommand1.ExecuteReader();
                Int64 __count = 0;
                bool rz = myReader.Read();
                while (rz)
                {
                    __count++;
                    Int64 indx = myReader.GetInt64(0);
                    byte[] bb;
                    bb = (byte[])myReader.GetValue(1);
                    int fl = 0;
                    for (int i = 0; i < bb.Length; i++)
                    {
                        if (bb[i] > 0)
                            fl = 1;
                    }
                    //================
                    MySqlConnection conn2 = new MySqlConnection(strConnect);
                    conn2.Open();
                    myCommand2 = new MySqlCommand(@"UPDATE defectsdata SET defectsdata.FlDefectTube=@FL WHERE defectsdata.IndexData=@INDX", conn2);
                    myCommand2.Parameters.Clear();
                    myCommand2.Parameters.AddWithValue("FL", fl);
                    myCommand2.Parameters.AddWithValue("INDX", indx);
                    myCommand2.ExecuteNonQuery();
                    conn2.Close();
                    if ((__count % 10) == 0)
                    {
                        this.Invoke(new ThreadStart(delegate
                        {
                            label1.Text = countMax.ToString();
                            label2.Text = __count.ToString();
                        }));
                    }
                    //================
                    try
                    {
                        rz = myReader.Read();
                    }
                    catch
                    {
                        try
                        {
                            myReader.Close();
                            myConnect.Close();
                            myConnect.Open();
                            myReader = myCommand1.ExecuteReader();
                            rz = myReader.Read();
                        }
                        catch
                        {
                            MessageBox.Show("Невыясненная ошибка");
                            this.Invoke(new ThreadStart(delegate
                            {
                                this.Close();
                            }));
                        }
                    }
                }
            }
            finally
            {
                if (thr != null)
                {
                    thr.Abort();
                    thr = null;
                }
            }
        }
        //==========================
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thr != null)
            {
                thr.Abort();
                thr = null;
            }
        }
        //=====================


    }
}
