using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
        public static readonly int maxLenTube = 13000 * 6 / 1000; // 15000 - 15 metrov
        public static readonly int minLenTube =  4000 * 6 / 1000; // 15000 - 15 metrov
        private static readonly Settings ps = Settings.Default;
        private readonly Byte[] buffForRead = new Byte[11];
        private List<byte> bufferRecive = new List<byte>(maxLenTube);
        private readonly Crc crc = new Crc();
        private readonly SerialPort serialPort = null;
        public int sampleDataCount = 0;
        private readonly Byte[] sampleDataBytes = new Byte[40];
        public MainWindow mainWindow;

        public Writer()
        {
            serialPort = new SerialPort(ps.Com);
            serialPort.DataReceived += SerialPortDataRecived;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            bufferRecive.Clear();
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
            }
            else
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("Recived_NewTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Ошибка количества параметров");
            }
            bufferRecive.Clear();
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
            if (bufferRecive.Count < maxLenTube)
            {
                mainWindow.Move_Tube();
                // запись дефектов
                try { bufferRecive.Add(buffForRead[6]); }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write.cs");
                    Console.WriteLine("Recived_SegmentTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Error index : " + buffForRead[5].ToString());
                    Console.WriteLine(ex.ToString());
                }
            }
            // отображение дефектов
            if (buffForRead[6]!=0)
            {
                mainWindow.Error_Segment();
            }
            // авто рез
            if (bufferRecive.Count >= maxLenTube)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("Recived_SegmentTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Error very big tube : ");
                Recived_NewTube();
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
            if ((mainWindow.Parameters.Count == 12) && (mainWindow.Parameters2.Count == 1))
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
    }
}
