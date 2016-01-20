using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace test2
{
    /// <summary>
    /// Логика взаимодействия для BDEditorWindow.xaml
    /// </summary>
    public partial class BDEditorWindow : Window
    {
        public BDEditorWindow()
        {
            InitializeComponent();
        }
        //=========================================
        // specialists
        private void Spec_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбраный элемент ?", "Внимание",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(@"
UPDATE
operators
SET active = 0
WHERE Id_Operator = @id
", connection.mySqlConnection);
                    var dataRowView = (DataRowView)dg_Specs.CurrentItem;
                    mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Specs();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void Button_AddUser_Click(object sender, RoutedEventArgs e)
        {
            Connection connection;
            MySqlCommand mySqlCommand;
            try
            {
                var txt_FIO = TextBox_FIO.Text;
                var txt_L_MD = TextBox_LevelMD.Text;
                var txt_L_USD = TextBox_LevelUSD.Text;
                //
                if (txt_FIO.Trim() != "" &&
                    txt_L_MD.Trim() != "" &&
                    txt_L_USD.Trim() != "")
                {
                    connection = new Connection();
                    connection.Open();
                    mySqlCommand = new MySqlCommand(@"
INSERT INTO operators
(Surname, LevelMD, LevelUSD)
VALUES
(@Surname, @LevelMD, @LevelUSD)", connection.mySqlConnection);
                    mySqlCommand.Parameters.AddWithValue("Surname", txt_FIO);
                    mySqlCommand.Parameters.AddWithValue("LevelMD", txt_L_MD);
                    mySqlCommand.Parameters.AddWithValue("LevelUSD", txt_L_USD);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Specs();
                }
                else
                {
                    System.Windows.MessageBox.Show("Не все поля заполнены");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //
        public void fill_dg_Specs()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(@"
SELECT
 Id_Operator AS 'ID'
,Surname AS 'ФИО'
,LevelMD AS 'Уровень MD'
,LevelUSD AS 'Уровень USD'
FROM operators
WHERE active = 1", connection.mySqlConnection);
                var dataAdapter = new MySqlDataAdapter(mySqlCommand.CommandText, connection.mySqlConnection);
                var dSet = new DataSet();
                dataAdapter.Fill(dSet);
                //
                dg_Specs.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        // ==========================================================================
        // smens
        private void Smens_MenuIteamDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбраный элемент ?", "Внимание",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(@"
UPDATE worksmens
SET active = 0
WHERE Id_WorkSmens = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_Smens.CurrentItem;
                    mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Smens();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void Button_AddNameSmen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = TextBox_NameSmen.Text;
                if (txt.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(@"
INSERT INTO
worksmens (NameSmen)
VALUES (@NameSmen)
", connection.mySqlConnection);
                    mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.AddWithValue("NameSmen", txt);
                    mySqlCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Smens();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения и не могут быть пустыми");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_Smens()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(@"
SELECT
  Id_WorkSmen AS 'ID'
, NameSmen AS 'Название смены'
FROM worksmens
WHERE active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_Smens.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        //=============================================
        // type size tube
        private void SizeTube_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                System.Windows.Forms.MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE sizetubes
SET active = 0
WHERE Id_SizeTube = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_SizeTube.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_SizeTube();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_AddSizeTube_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = TextBox_SizeTube.Text;
                if (txt.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO
sizetubes (SizeTube)
VALUES (@SizeTube)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("SizeTube", txt);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_SizeTube();
                }
                else
                {
                    System.Windows.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_SizeTube()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(@"
SELECT
  Id_SizeTube AS 'ID'
, SizeTube AS 'Диаметр трубы'
FROM sizetubes
WHERE active =1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_SizeTube.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //========================================================
        // K.O.
        private void Sample_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?","Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)==System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE controlsamples
SET active = 0
WHERE Id_ControlSample = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_Sample.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Sample();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_AddSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtName = Textbox_Sample_Name.Text;
                var txtMin = Textbox_Sample_DepthMin.Text;
                var txtMax = Textbox_Sample_DepthMax.Text;
                if (txtName.Trim() != "" &&
                    txtMin.Trim() != "" &&
                    txtMax.Trim() != "" &&
                    Combobox_Sample_d.SelectedIndex != -1)
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO
controlsamples (NameControlSample, Id_SizeTube, DepthMin, DepthMax)
VALUES
(@NameControlSample, @Id_SizeTube, @DepthMin, @DepthMax)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("NameControlSample", txtName);
                    myCommand.Parameters.AddWithValue("Id_SizeTube", ((KeyValuePair<int, string>)Combobox_Sample_d.SelectedItem).Key);
                    myCommand.Parameters.AddWithValue("DepthMin", txtMin);
                    myCommand.Parameters.AddWithValue("DepthMax", txtMax);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Sample();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_Sample()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  controlsamples.Id_ControlSample AS 'ID'
, controlsamples.NameControlSample AS 'Название контрольного образца'
, sizetubes.SizeTube AS 'Диаметр трубы'
, controlsamples.DepthMin AS 'Минимальный диаметр'
, controlsamples.DepthMax AS 'Максимальный диаметр'
FROM controlsamples
Inner Join sizetubes ON sizetubes.Id_SizeTube = controlsamples.Id_SizeTube
WHERE controlsamples.active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_Sample.ItemsSource = dSet.Tables[0].DefaultView;
                Combobox_Sample_d.ItemsSource = Get_Db_SizeTubes();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public Dictionary<int, string> Get_Db_SizeTubes()
        {
            var sizeTubes = new Dictionary<int, string>();
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT Id_SizeTube, SizeTube FROM sizetubes WHERE active = 1", connection.mySqlConnection);
                MySqlDataReader dataReader = myCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    sizeTubes.Add(dataReader.GetInt32(0), dataReader.GetString(1));
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return sizeTubes;
        }
        //=============================================
        // gost
        private void NormDoc_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE gosts
SET active = 0
WHERE Id_Gost = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_NormDoc.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_NormDoc();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void Button_AddNormDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtName = Textbox_NormDocName.Text;
                if (txtName.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO
gosts (NameGost)
VALUES (@NameGost)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("NameGost", txtName);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_NormDoc();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_NormDoc()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  Id_Gost AS 'ID'
, NameGost AS 'Название ГОСТа'
FROM gosts
WHERE active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_NormDoc.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //=============================================
        // defect device
        private void Device_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE device
SET active = 0
WHERE Id_Device = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_Device.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Device();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void Button_AddDevice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtName = Textbox_DeviceName.Text;
                if (txtName.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO device (NameDevice)
VALUES (@NameDevice)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("NameDevice", txtName);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Device();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_Device()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  Id_Device AS 'ID'
, NameDevice AS 'Название дефектоскопа'
FROM device
WHERE active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_Device.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //================================================
        // sensors
        private void Sensors_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE
sensors
SET active = 0
WHERE Id_Sensor = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_Sensors.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Sensors();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_AddSensors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtName = Textbox_Sensors.Text;
                if (txtName.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT
INTO sensors (NameSensor)
VALUES (@NameSensor)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("NameSensor", txtName);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Sensors();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_Sensors()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  Id_Sensor AS 'ID'
, NameSensor AS 'Название датчика'
FROM sensors
WHERE active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_Sensors.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //================================================
        // соответствия
        private void Conformity_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE bufferdata
SET active = 0
WHERE Id = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_Conformity.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_Conformity();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_Conformity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
INSERT
INTO bufferdata (Id_Gost, Id_SizeTube)
VALUES (@Id_Gost, @Id_SizeTube)
", connection.mySqlConnection);
                myCommand.Parameters.Clear();
                myCommand.Parameters.AddWithValue("Id_Gost", ((KeyValuePair<int, string>)Combobox_Conformity_Name.SelectedItem).Key);
                myCommand.Parameters.AddWithValue("Id_SizeTube", ((KeyValuePair<int, string>)Combobox_Conformity_d.SelectedItem).Key);
                myCommand.ExecuteNonQuery();
                connection.Close();
                fill_dg_Conformity();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_Conformity()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  bufferdata.Id AS 'ID'
, sizetubes.SizeTube AS 'Диаметр трубы'
, gosts.NameGost AS 'Название ГОСТа'
FROM bufferdata
Inner Join gosts ON gosts.Id_Gost = bufferdata.Id_Gost
Inner Join sizetubes ON sizetubes.Id_SizeTube = bufferdata.Id_SizeTube
WHERE bufferdata.active = 1
", connection.mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                DataSet dSet = new DataSet();
                dataAdapter.Fill(dSet);
                dg_Conformity.ItemsSource = dSet.Tables[0].DefaultView;
                Combobox_Conformity_Name.ItemsSource = Get_db_Gosts();
                Combobox_Conformity_d.ItemsSource = Get_Db_SizeTubes();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public Dictionary<int,string> Get_db_Gosts()
        {
            var gosts = new Dictionary<int, string>();
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT Id_Gost, NameGost FROM gosts WHERE active = 1", connection.mySqlConnection);
                MySqlDataReader dataReader = myCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    gosts.Add(dataReader.GetInt32(0), dataReader.GetString(1)); ;
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return gosts;
        }
        //================================================
        // Временные интервалы смен
        private void VermSmens_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    DataRowView dataRowView = (DataRowView)dg_VremSmens.CurrentItem;
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE timeintervalsmens
SET active = 0
WHERE Id_TimeIntervalSmen = @id
", connection.mySqlConnection);
                    DataSet dSet = new DataSet();
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_VermSmens();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_AddVremSmens_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtBegin = UpDn_VremSmens_Begin.Text;
                var txtEnd = UpDn_VremSmens_End.Text;
                if (txtBegin.Trim() != "" &&
                    txtEnd.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO
timeintervalsmens (TimeIntervalSmen)
VALUES (@TimeIntervalSmen)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("TimeIntervalSmen", txtBegin + "-" + txtEnd);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_VermSmens();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Все поля обязательны для заполнения");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_VermSmens()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  Id_TimeIntervalSmen AS 'ID'
, TimeIntervalSmen AS 'Время'
FROM timeintervalsmens
WHERE active = 1
", connection.mySqlConnection);
                DataSet dSet = new DataSet();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                dataAdapter.Fill(dSet);
                dg_VremSmens.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //================================================
        // виды искуственных дефектов
        private void ArtificialDefect_MenuItemDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Вы действительно хотите удалить выбранный элемент ?", "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
UPDATE listdefects
SET active = 0
WHERE Id_NameDefect = @id
", connection.mySqlConnection);
                    DataRowView dataRowView = (DataRowView)dg_ArtificialDefect.CurrentItem;
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("id", dataRowView.Row.ItemArray[0]);
                    myCommand.ExecuteNonQuery();
                    fill_dg_ArtificialDefect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void Button_AddArtificialDefect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txtName = Textbox_ArtificialDefect.Text;
                if (txtName.Trim() != "")
                {
                    Connection connection = new Connection();
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO
listdefects (NameDefect)
VALUES (@NameDefect)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("NameDefect", txtName);
                    myCommand.ExecuteNonQuery();
                    connection.Close();
                    fill_dg_ArtificialDefect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void fill_dg_ArtificialDefect()
        {
            try
            {
                Connection connection = new Connection();
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
SELECT
  Id_NameDefect AS 'ID'
, NameDefect AS 'Искуственный дефект'
FROM listdefects
WHERE active = 1
", connection.mySqlConnection);
                DataSet dSet = new DataSet();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(myCommand.CommandText, connection.mySqlConnection);
                dataAdapter.Fill(dSet);
                dg_ArtificialDefect.ItemsSource = dSet.Tables[0].DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //====================
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is System.Windows.Controls.TabControl)
            {
                switch (TabControl.SelectedIndex)
                {
                    case 0:
                        fill_dg_Specs();
                        break;
                    case 1:
                        fill_dg_Smens();
                        break;
                    case 2:
                        fill_dg_SizeTube();
                        break;
                    case 3:
                        fill_dg_Sample();
                        break;
                    case 4:
                        fill_dg_NormDoc();
                        break;
                    case 5:
                        fill_dg_Device();
                        break;
                    case 6:
                        fill_dg_Sensors();
                        break;
                    case 7:
                        fill_dg_Conformity();
                        break;
                    case 8:
                        fill_dg_VermSmens();
                        break;
                    case 9:
                        fill_dg_ArtificialDefect();
                        break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        //==============
    }
}
