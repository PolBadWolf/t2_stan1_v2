using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public class Write_NewTube// : IDisposable
    {
        //public void Dispose();

        public Write_NewTube()
        {
        }

        public void DoIt(byte[] buffForRead, List<byte> bufferRecive)
        {
            if (bufferRecive.Count < Writer.minLenTube)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write_NewTube.cs");
                Console.WriteLine("DoIt()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Very small tube : "+((double)bufferRecive.Count/6.0).ToString()+"метров");
                return;
            }
            try
            {
                Connection connection = new Connection();
                try { connection.Open(); } catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write_NewTube.cs");
                    Console.WriteLine("DoIt()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Open()");
                    throw (new Exception("Error Write new tube : open bd"));
                }
                {
                    MySqlCommand myCommand = defectsdata_sql(connection.mySqlConnection);
                    defectsdata_param(myCommand, bufferRecive);
                    try { myCommand.ExecuteNonQuery(); }
                    catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write_NewTube.cs");
                        Console.WriteLine("DoIt()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("defectsdata ExecuteNonQuery()");
                        throw (new Exception("Error Write new tube : write defectsdata"));
                    }
                }
                Int64 lastIndex = lastIndex_defectsdata();
                {
                    MySqlCommand myCommand = indexes_sql(connection.mySqlConnection);
                    indexes_param(myCommand, lastIndex);
                    try { myCommand.ExecuteNonQuery(); } catch
                    {
                        Console.WriteLine("========================================");
                        Console.WriteLine("Write_NewTube.cs");
                        Console.WriteLine("DoIt()  :  " + DateTime.Now.ToString());
                        Console.WriteLine("indexes ExecuteNonQuery()");
                        throw (new Exception("Error Write new tube : write indexes"));
                    }
                }
                try { connection.Close(); } catch
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Write_NewTube.cs");
                    Console.WriteLine("DoIt()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Close()");
                    throw (new Exception("Error Write new tube : close bd"));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //======================================================================================
        private MySqlCommand defectsdata_sql(MySqlConnection conn)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.CommandText= @"
INSERT INTO
defectsdata(NumberPart, NumberTube, NumberSegments, DataSensors, DatePr, TimePr, Porog, Current, FlDefectTube)
values(@A, @B, @C, @D, @E, @F, @G, @H, @I)";
            myCommand.Connection = conn;
            return myCommand;
        }
        private void defectsdata_param(MySqlCommand myCommand, List<byte> bufferRecive)
        {
            int hasDeffect = 0;
            myCommand.Parameters.Clear();
            // номер партии
            myCommand.Parameters.AddWithValue("A", MainWindow.mainWindow.Parameters["part"]);
            // порог
            myCommand.Parameters.AddWithValue("G", MainWindow.mainWindow.Parameters["porog"]);
            // ток
            myCommand.Parameters.AddWithValue("H", MainWindow.mainWindow.Parameters["current"]);
            // номер трубы
            myCommand.Parameters.AddWithValue("B", LastNumberTube(MainWindow.mainWindow.Parameters["part"]) + 1);
            // размер трубы
            myCommand.Parameters.AddWithValue("C", bufferRecive.Count);
            // дефекты
            Byte[] deffectsArray = new Byte[bufferRecive.Count];
            try
            {
                for (int k = 0; k < bufferRecive.Count; k++)
                {
                    if (bufferRecive[k] != 0) hasDeffect = 1;
                    deffectsArray[k] = bufferRecive[k];
                }
            } catch { }
            myCommand.Parameters.AddWithValue("D", deffectsArray);
            // текущая дата
            DateTime theDateTime = DateTime.Now;
            myCommand.Parameters.AddWithValue("E", theDateTime.ToString("yyyy-MM-dd"));
            myCommand.Parameters.AddWithValue("F", theDateTime.ToString("H:mm:ss"));
            // наличие дефектов
            myCommand.Parameters.AddWithValue("I", hasDeffect);
            // add statistic
            MainWindow.ac.addNewTube(theDateTime, hasDeffect);
        }
        //=================================================================================================================
        private int LastNumberTube(int part)
        {
            int last = 0;
            Connection connection = new Connection();
            try { connection.Open(); } catch
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
            try { mySqlReader.Close(); connection.Close(); }
            catch
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Write.cs");
                Console.WriteLine("LastNumberTube()");
                Console.WriteLine("Close for ExecuteReader");
            }
            return last;
        }
        Int64 lastIndex_defectsdata()
        {
            Int64 index = 0;
            Connection connection = new Connection();
            try { connection.Open(); } catch (Exception ex)
            {
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
SELECT IndexData
FROM defectsdata
ORDER BY IndexData DESC
LIMIT 1", connection.mySqlConnection);
            MySqlDataReader myRead = null;
            try
            {
                myRead = myCommand.ExecuteReader();
                myRead.Read();
                index = myRead.GetInt64(0);
                myRead.Close();
                myRead.Dispose();
            } catch (Exception ex)
            {
                //throw (ex);
            }
            return index;
        }
        private MySqlCommand indexes_sql(MySqlConnection conn)
        {
            MySqlCommand myCommand = new MySqlCommand(@"
INSERT INTO 
indexes ( Version, IndexData, Id_SizeTube, Id_Gost, Id_ControlSample, Id_WorkSmen, Id_TimeIntervalSmen, Id_Operator1, Id_Operator2, Id_Device, Id_Sensor, Id_NameDefect )
VALUES ( @Vers, @Id_Index, @Id_SizeTube, @Id_Gost, @Id_ControlSamle, @Id_WorkSmen, @Id_TimeIntervalSmen, @Id_Operator1, @Id_Operator2, @Id_Device, @Id_Sensor, @Id_NameDefect )", conn);
            return myCommand;
        }
        private void indexes_param(MySqlCommand myCommand, Int64 lastIndex)
        {
            myCommand.Parameters.Clear();
            myCommand.Parameters.AddWithValue("Vers", 2);
            myCommand.Parameters.AddWithValue("Id_Index", lastIndex);
            myCommand.Parameters.AddWithValue("Id_SizeTube", MainWindow.mainWindow.Parameters["diameter"]);
            myCommand.Parameters.AddWithValue("Id_Gost", MainWindow.mainWindow.Parameters["gost"]);
            myCommand.Parameters.AddWithValue("Id_ControlSamle", (int)MainWindow.mainWindow.Parameters["control_sample"]);
            myCommand.Parameters.AddWithValue("Id_WorkSmen", MainWindow.mainWindow.Parameters["smena"]);
            myCommand.Parameters.AddWithValue("Id_TimeIntervalSmen", MainWindow.mainWindow.Parameters["smena_time"]);
            myCommand.Parameters.AddWithValue("Id_Operator1", MainWindow.mainWindow.Parameters["operator1"]);
            myCommand.Parameters.AddWithValue("Id_Operator2", MainWindow.mainWindow.Parameters["operator2"]);
            myCommand.Parameters.AddWithValue("Id_Device", MainWindow.mainWindow.Parameters["device"]);
            myCommand.Parameters.AddWithValue("Id_Sensor", MainWindow.mainWindow.Parameters2["ho"]);
            myCommand.Parameters.AddWithValue("Id_NameDefect", MainWindow.mainWindow.Parameters["name_defect"]);
        }
        //
    }
}
