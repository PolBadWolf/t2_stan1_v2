using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public class Parameters
    {
        public Dictionary<int, string> GetDb_WorkSmens()
        {
            var workSmens = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand("SELECT Id_WorkSmen, NameSmen FROM worksmens WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlDataReader = null;
            try { mySqlDataReader = myCommand.ExecuteReader(); } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlDataReader.Read())
                {
                    workSmens.Add(mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("Id_WorkSmen")),
                                  mySqlDataReader.GetString(mySqlDataReader.GetOrdinal("NameSmen")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlDataReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return workSmens;
        }
        public Dictionary<int, string> GetDb_TimeIntervalSmens()
        {
            var intervalSmens = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_TimeIntervalSmen, TimeIntervalSmen FROM timeintervalsmens WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlDataReader = null;
            try { mySqlDataReader = mySqlCommand.ExecuteReader(); } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlDataReader.Read())
                {
                    intervalSmens.Add(mySqlDataReader.GetInt32(mySqlDataReader.GetOrdinal("Id_TimeIntervalSmen")), mySqlDataReader.GetString(mySqlDataReader.GetOrdinal("TimeIntervalSmen")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlDataReader.Close();
                connection.Close();
                connection = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }

            return intervalSmens;
        }
        // оператор
        public Dictionary<int, string> GetDb_SurNames()
        {
            var surNames = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SurNames()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_Operator, Surname FROM operators WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SurNames()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    surNames.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Operator")), mySqlReader.GetString(mySqlReader.GetOrdinal("Surname")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SurNames()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }

            return surNames;
        }
        // =================
        //                       плавка
        public Dictionary<int, string> GetDb_Gost()
        {
            var gost = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Gost()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_Gost, NameGost FROM gosts WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Gost()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    gost.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Gost")), mySqlReader.GetString(mySqlReader.GetOrdinal("NameGost")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Gost()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Gost()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return gost;
        }

        public Dictionary<int, string> GetDb_SizeTube()
        {
            var sizeTube = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_SizeTube, SizeTube FROM sizetubes WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    sizeTube.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_SizeTube")), mySqlReader.GetString(mySqlReader.GetOrdinal("SizeTube")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTube()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }

            return sizeTube;
        }

        public Dictionary<int, string> GetDb_ControlSample()
        {
            var controlSample = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSample()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_ControlSample, NameControlSample FROM controlsamples WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSample()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    controlSample.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_ControlSample")), mySqlReader.GetString(mySqlReader.GetOrdinal("NameControlSample")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSample()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSample()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return controlSample;
        }

        public Dictionary<int, string> GetDb_ListDefects()
        {
            var listDefects = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_NameDefect, NameDefect FROM listdefects WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    listDefects.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_NameDefect")), mySqlReader.GetString(mySqlReader.GetOrdinal("NameDefect")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return listDefects;
        }

        public Dictionary<int, string> GetDb_Device()
        {
            var device = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand mySqlCommand = new MySqlCommand("SELECT Id_Device, NameDevice FROM device WHERE active = 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = mySqlCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    device.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Device")), mySqlReader.GetString(mySqlReader.GetOrdinal("NameDevice")));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read BD");
                throw (ex);
            }
            try
            {
                mySqlReader.Close();
                connection.Close();
                connection = null;
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return device;
        }

        public Dictionary<int, string> GetDb_SizeTubesCurrent(int id)
        {
            Connection connection = new Connection();
            var sizeTubes = new Dictionary<int, string>();

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                    sizetubes.Id_SizeTube,
                    sizetubes.SizeTube
                FROM
                    bufferdata
                Inner Join sizetubes ON sizetubes.Id_SizeTube = bufferdata.Id_SizeTube
                WHERE bufferdata.Id_Gost = @A", connection.mySqlConnection);
            myCommand.Parameters.AddWithValue("A", id);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                sizeTubes.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("sizetubes.Id_SizeTube")), mySqlReader.GetString(mySqlReader.GetOrdinal("sizetubes.SizeTube")));
            }

            mySqlReader.Close();
            connection.Close();

            return sizeTubes;
        }

        public Dictionary<int, string> GetDb_ControlSamplesCurrent(int id)
        {
            Connection connection = new Connection();
            var controlSamples = new Dictionary<int, string>();

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                controlsamples.Id_ControlSample,
                controlsamples.NameControlSample
                FROM
                controlsamples
                WHERE controlsamples.Id_SizeTube = @A", connection.mySqlConnection);
            myCommand.Parameters.AddWithValue("A", id);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                controlSamples.Add(mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_ControlSample")), mySqlReader.GetString(mySqlReader.GetOrdinal("NameControlSample")));
            }


            mySqlReader.Close();
            connection.Close();

            return controlSamples;
        }

        public int GetDb_Last_WorkSmens()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_WorkSmen
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_WorkSmen"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_TimeIntervalSmens()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_TimeIntervalSmen
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_TimeIntervalSmen"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_SurName1()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Operator1
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Operator1"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_SurName2()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Operator2
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Operator2"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_Gosts()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Gost
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Gost"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_SizeTubes()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_SizeTube
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_SizeTube"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_ControlSamples()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_ControlSample
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_ControlSample"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_ListDefects()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_NameDefect
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_NameDefect"));
            }

            mySqlReader.Close();
            myCommand.Clone();

            return last;
        }

        public int GetDb_Last_Device()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Device
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Id_Device"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_Part()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();

            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                NumberPart
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("NumberPart"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public double GetDb_Last_Ho()
        {
            Connection connection = new Connection();
            double last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Id_Sensor
                FROM
                indexes
                ORDER BY
                Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetDouble(mySqlReader.GetOrdinal("Id_Sensor"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_Porog()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Porog
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Porog"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_Current()
        {
            Connection connection = new Connection();
            int last = 0;

            connection.Open();
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Current
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = myCommand.ExecuteReader();

            while (mySqlReader.Read())
            {
                last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("Current"));
            }

            mySqlReader.Close();
            connection.Close();

            return last;
        }

        public int GetDb_Last_NumberTube()
        {
            int last = 0;
            if (MainWindow.mainWindow.Parameters["part"] == GetDb_Last_Part())
            {
                Connection connection = new Connection();

                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                defectsdata.NumberTube
                FROM
                defectsdata
                ORDER BY
                defectsdata.IndexData DESC
                LIMIT 1", connection.mySqlConnection);
                MySqlDataReader mySqlReader = myCommand.ExecuteReader();

                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(mySqlReader.GetOrdinal("NumberTube"));
                }

                mySqlReader.Close();
                connection.Close();
            }
            return last;
        }
    }
}
