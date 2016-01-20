using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using test2.Properties;

namespace test2
{
    class Writer
    {
        private static readonly Settings ps = Settings.Default;
        private readonly Byte[] buffForRead = new Byte[11];
        private readonly Byte[] bufferRecive = new Byte[90];
        private readonly Crc crc = new Crc();
        private readonly SerialPort serialPort = new SerialPort(ps.Com);
        public int sampleDataCount = 0;
        private readonly Byte[] sampleDataBytes = new Byte[40];
        public MainWindow mainWindow;

        public Writer()
        {
            serialPort.DataReceived += SerialPortDataRecived;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
        }

        private void SerialPortDataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.BytesToRead > 0)
            {
                // shift
                for (int j = 0; j < (buffForRead.Length-1); j++)
                {
                    buffForRead[j] = buffForRead[j + 1];
                }
                buffForRead[buffForRead.Length - 1] = (Byte)serialPort.ReadByte();

                //select
                if (buffForRead[0] != 0xe6) continue;
                if (buffForRead[1] != 0x19) continue;
                if (buffForRead[2] != 0xff) continue;
                if (buffForRead[3] != 0x08) continue;

                if (buffForRead[8] != 0x00) continue;
                if (buffForRead[9] != 0x00) continue;
                if (buffForRead[10] != 0x00) continue;

                if (buffForRead[7] != crc.Crc8(buffForRead, 7)) continue;

                // swich do
                if (buffForRead[4] == 0x03) Recived_NewTube(); // новая труба
                if (buffForRead[4] == 0x02) Recived_SegmentTube(); // сегмент трубы
                if (buffForRead[4] == 0x01) Recived_Sample(); // образец ?
                if (buffForRead[4] != 0x01) Recived_NoSample(); // образец ?
            }
        }
        private void Recived_NewTube()
        {
            Connection connection = new Connection();
            mainWindow.New_Tube();
            if ((mainWindow.Parameters.Count == 12) && (mainWindow.Parameters2.Count == 1))
            {
                Write_NewTube write_newTube = new Write_NewTube();
                try
                {
                    write_newTube.DoIt(buffForRead, bufferRecive);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Error write bd:");
                    Console.WriteLine(ex.ToString());
                }
                write_newTube = null;
                /*
                int hasDeffect = 0;
                mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
                {
                    try { connection.Open(); }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("Open() for \"INSERT INTO defectsdata\"");
                    }
                    //              defectsdata
                    MySqlCommand myCommand = new MySqlCommand("INSERT INTO defectsdata(defectsdata.NumberPart, defectsdata.NumberTube, defectsdata.NumberSegments, defectsdata.DataSensors, defectsdata.DatePr, defectsdata.TimePr, defectsdata.Porog, defectsdata.Current, defectsdata.FlDefectTube) values(@A, @B, @C, @D, @E, @F, @G, @H, @I)", connection.mySqlConnection);
                    // номер партии
                    myCommand.Parameters.AddWithValue("A", mainWindow.Parameters["part"]);
                    // порог
                    myCommand.Parameters.AddWithValue("G", mainWindow.Parameters["porog"]);
                    // ток
                    myCommand.Parameters.AddWithValue("H", mainWindow.Parameters["current"]);
                    // номер трубы
                    myCommand.Parameters.AddWithValue("B", LastNumberTube(mainWindow.Parameters["part"]) + 1);
                    // размер трубы
                    myCommand.Parameters.AddWithValue("C", buffForRead[5]);
                    // дефекты
                    Byte[] deffectsArray = new Byte[buffForRead[5]];
                    for (int k = 0; k < (int)buffForRead[5]; k++)
                    {
                        if (bufferRecive[k] != 0) hasDeffect = 1;
                        deffectsArray[k] = bufferRecive[k];
                    }
                    myCommand.Parameters.AddWithValue("D", deffectsArray);
                    // текущая дата
                    DateTime theDateTime = DateTime.Now;
                    myCommand.Parameters.AddWithValue("E", theDateTime.ToString("yyyy-MM-dd"));
                    myCommand.Parameters.AddWithValue("F", theDateTime.ToString("H:mm:ss"));
                    // наличие дефектов
                    myCommand.Parameters.AddWithValue("I", hasDeffect);
                    try { myCommand.ExecuteNonQuery(); } catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("\"INSERT INTO defectsdata\"");
                    }
                    //==============
                    try { connection.Close(); } catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("\"INSERT INTO defectsdata\" - close()");
                    }
                    try { connection.Open(); } catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("Open() for Read last IndexData ");
                    }
                    // чтение индекса
                    myCommand = new MySqlCommand(@"
SELECT
IndexData
FROM defectsdata
ORDER BY IndexData DESC
LIMIT 1
", connection.mySqlConnection);
                    Int64 lastIndex = 0;
                    MySqlDataReader dataRead = null;
                    try
                    {
                        dataRead = myCommand.ExecuteReader();
                        dataRead.Read();
                        lastIndex = dataRead.GetInt64(0);
                    }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("Read last IndexData");
                    }
                    try
                    {
                        dataRead.Close();
                        connection.Close();
                    }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("Close() for Read last IndexData ");
                    }
                    var ko = mainWindow.Parameters["control_sample"];
                    //              indexes
                    try { connection.Open(); }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("Open() for \"INSERT INTO indexes\"");
                    }
                    myCommand = new MySqlCommand(@"
INSERT INTO 
indexes (
Version
,IndexData
,Id_SizeTube
,Id_Gost
,Id_ControlSample
,Id_WorkSmen
,Id_TimeIntervalSmen
,Id_Operator1
,Id_Operator2
,Id_Device
,Id_Sensor
,Id_NameDefect
) VALUES (
@Vers
,@Id_Index
,@Id_SizeTube
,@Id_Gost
,@Id_ControlSamle
,@Id_WorkSmen
,@Id_TimeIntervalSmen
,@Id_Operator1
,@Id_Operator2
,@Id_Device
,@Id_Sensor
,@Id_NameDefect
)", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("Vers", 2);
                    myCommand.Parameters.AddWithValue("Id_Index", lastIndex);
                    myCommand.Parameters.AddWithValue("Id_SizeTube", mainWindow.Parameters["diameter"]);
                    myCommand.Parameters.AddWithValue("Id_Gost", mainWindow.Parameters["gost"]);
                    myCommand.Parameters.AddWithValue("Id_ControlSamle", (int)mainWindow.Parameters["control_sample"]);
                    myCommand.Parameters.AddWithValue("Id_WorkSmen", mainWindow.Parameters["smena"]);
                    myCommand.Parameters.AddWithValue("Id_TimeIntervalSmen", mainWindow.Parameters["smena_time"]);
                    myCommand.Parameters.AddWithValue("Id_Operator1", mainWindow.Parameters["operator1"]);
                    myCommand.Parameters.AddWithValue("Id_Operator2", mainWindow.Parameters["operator2"]);
                    myCommand.Parameters.AddWithValue("Id_Device", mainWindow.Parameters["device"]);
                    myCommand.Parameters.AddWithValue("Id_Sensor", mainWindow.Parameters2["ho"]);
                    myCommand.Parameters.AddWithValue("Id_NameDefect", mainWindow.Parameters["name_defect"]);
                    try
                    { myCommand.ExecuteNonQuery(); }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("\"INSERT INTO indexes\"");
                    }
                    try { connection.Close(); } catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write.cs");
                        Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("indexes close()");
                    }
                }));
                */
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Ошибка количества параметров");
            }
            //out
            {
                int last = mainWindow.parAdvn.GetDb_Last_NumberTube();
                mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
                {
                    mainWindow.lblinfo6.Content = "Пройдено труб:\t\t " + last;
                }));
            }
            connection = null;
        }
        private void Recived_SegmentTube()
        {
            mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
            {
                if ( (mainWindow.TabControl1.SelectedIndex == 2) && mainWindow.button_CONTROL.IsEnabled )
                {
                    mainWindow.button_CONTROL_Click(null, null);
                }
            }));
            mainWindow.Move_Tube();
            // запись дефектов
            bufferRecive[buffForRead[5]] = buffForRead[6];
            // отображение дефектов
            if (buffForRead[6]!=0)
            {
                mainWindow.Error_Segment();
//                bufferRecive[]
            }
        }
        private void Recived_Sample()
        {
            mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
            {
                mainWindow.button_KO.IsEnabled = true;
                if (sampleDataCount < 40)
                {
                    sampleDataBytes[sampleDataCount] = buffForRead[5];
                    mainWindow.Move_Sample_Tube();
                    mainWindow.ButtonCancel.IsEnabled = false;
                    mainWindow.ButtonSave.IsEnabled = false;
                    //
                    if (buffForRead[5] != 0)
                    {
                        mainWindow.Error_Sample_Segment(sampleDataCount);
                    }
                    sampleDataCount++;
                }
                //if (sampleDataCount >= 39)
                else
                {
                    mainWindow.ButtonCancel.IsEnabled = true;
                    mainWindow.ButtonSave.IsEnabled = true;
                    mainWindow._countSample = mainWindow._count;
                }
            }));
        }
        private void Recived_NoSample()
        {
            mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
            {
                mainWindow.button_KO.IsEnabled = true;
            }));
        }
        //==
        public void SaveSample()
        {
            Connection connection = new Connection();
            if (mainWindow.Parameters.Count == 13)
            {
                int hasDeffect = 0;
                //                defectsdata
                try
                {
                    connection.Open();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Open for \"INSERT INTO defectsdata\"");
                }
                MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO defectsdata
(NumberPart, NumberTube, NumberSegments, DataSensors, DatePr, TimePr, Porog, Current, FlDefectTube)
VALUES
(@A, @B, @C, @D, @E, @F, @G, @H, @I)",
connection.mySqlConnection);
                myCommand.Parameters.Clear();
                // номер партии
                myCommand.Parameters.AddWithValue("A", mainWindow.Parameters["part"]);
                // порог
                myCommand.Parameters.AddWithValue("G", mainWindow.Parameters["porog"]);
                // ток
                myCommand.Parameters.AddWithValue("H", mainWindow.Parameters["current"]);
                // маркировка - образец
                myCommand.Parameters.AddWithValue("B", 0);
                // размер образца
                myCommand.Parameters.AddWithValue("C", 40);
                // дефекты
                Byte[] deffectsArray = new Byte[40];
                for (int k = 0; k < deffectsArray.Length; k++)
                {
                    if (sampleDataBytes[k] != 0) hasDeffect = 1;
                    deffectsArray[k] = sampleDataBytes[k];
                }
                myCommand.Parameters.AddWithValue("D", deffectsArray);
                // дата время
                DateTime theDateTime = DateTime.Now;
                myCommand.Parameters.AddWithValue("E", theDateTime.ToString("yyyy-MM-dd"));
                myCommand.Parameters.AddWithValue("F", theDateTime.ToString("H:mm:ss"));
                // наличие дефектов
                myCommand.Parameters.AddWithValue("I", hasDeffect);
                try
                {
                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("\"INSERT INTO defectsdata\"");
                }
                try
                {
                    connection.Close();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Close for \"INSERT INTO defectsdata\"");
                }
                try
                {
                    connection.Open();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Open for \"INSERT INTO indexes\"");
                }
                //        indexes
                myCommand = new MySqlCommand(@"
INSERT INTO indexes
(Version
, IndexData
, Id_SizeTube
, Id_Gost
, Id_ControlSample
, Id_WorkSmen
, Id_TimeIntervalSmen
, Id_Operator1
, Id_Operator2
, Id_Device
, Id_Sensor
, Id_NameDefect
)
VALUES
(2
, (SELECT IndexData FROM defectsdata ORDER BY IndexData DESC LIMIT 1)
, @Id_SizeTube
, @Id_Gost
, @Id_ControlSample
, @Id_WorkSmens
, @Id_TimeIntervalSmen
, @Id_Operator1
, @Id_Operator2
, @Id_Device
, @Id_Sensor
, @Id_NameDefect
)", connection.mySqlConnection);
                myCommand.Parameters.AddWithValue("Id_SizeTube", mainWindow.Parameters["diameter"]);
                myCommand.Parameters.AddWithValue("Id_Gost", mainWindow.Parameters["gost"]);
                myCommand.Parameters.AddWithValue("Id_ControlSample", (int)mainWindow.Parameters["control_sample"]);
                myCommand.Parameters.AddWithValue("Id_WorkSmens", mainWindow.Parameters["smena"]);
                myCommand.Parameters.AddWithValue("Id_TimeIntervalSmen", mainWindow.Parameters["smena_time"]);
                myCommand.Parameters.AddWithValue("Id_Operator1", mainWindow.Parameters["operator1"]);
                myCommand.Parameters.AddWithValue("Id_Operator2", mainWindow.Parameters["operator2"]);
                myCommand.Parameters.AddWithValue("Id_Device", mainWindow.Parameters["device"]);
                myCommand.Parameters.AddWithValue("Id_Sensor", mainWindow.Parameters2["ho"]);
                myCommand.Parameters.AddWithValue("Id_NameDefect", mainWindow.Parameters["name_defect"]);
                try
                {
                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()");
                    Console.WriteLine("\"INSERT INTO indexes\"");
                }
                try
                {
                    connection.Close();
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("SaveSample()");
                    Console.WriteLine("Close for \"INSERT INTO indexes\"");
                }
            }
        }
        public void PortOpen()
        {
            if (!serialPort.IsOpen)
                serialPort.Open();
        }
        public void PortClose()
        {
            serialPort.Close();
        }
        //==
        private int LastNumberTube(int part)
        {
            Connection connection = new Connection();
            int last = 0;
            try
            {
                connection.Open();
            }
            catch
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("LastNumberTube()");
                Console.WriteLine("Open for ExecuteReader");
            }
            MySqlCommand myCommand = new MySqlCommand(@"
SELECT
NumberTube, NumberPart
FROM defectsdata
WHERE NumberTube<>0
ORDER BY IndexData DESC
LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null; ;
            try
            {
                mySqlReader = myCommand.ExecuteReader();
            }
            catch
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("LastNumberTube()");
                Console.WriteLine("ExecuteReader");
            }
            while (mySqlReader.Read())
            {
                try
                {
                    if ((mySqlReader.GetValue(mySqlReader.GetOrdinal("NumberTube")) == null) || (mySqlReader.GetInt32(mySqlReader.GetOrdinal("NumberPart")) != part))
                        last = 0;
                    else
                        last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("NumberTube"));
                }
                catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("LastNumberTube()");
                    Console.WriteLine("ExecuteReader - READ NumberTube, NumberPart, NumberTube");
                }
            }
            mySqlReader.Close();
            /*
            //out
            mainWindow.Dispatcher.Invoke(new ThreadStart(delegate
            {
                mainWindow.lblinfo6.Content = "Пройдено труб:\t\t " + last;
            }));
            */
            try { connection.Close(); } catch
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("LastNumberTube()");
                Console.WriteLine("Close for ExecuteReader");
            }
            return last;
        }
    }
}
