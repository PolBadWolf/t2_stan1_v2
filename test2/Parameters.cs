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
            try { mySqlDataReader = (MySqlDataReader)myCommand.ExecuteReader(); } catch (Exception ex)
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
                    workSmens.Add(
                        mySqlDataReader.GetInt32(0),
                        mySqlDataReader.GetString(1));
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
            try { mySqlDataReader = (MySqlDataReader)mySqlCommand.ExecuteReader(); } catch (Exception ex)
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
                    intervalSmens.Add(
                        mySqlDataReader.GetInt32(0),
                        mySqlDataReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    surNames.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    gost.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    sizeTube.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    controlSample.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    listDefects.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
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
                mySqlReader = (MySqlDataReader)mySqlCommand.ExecuteReader();
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
                    device.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_Device()  :  " + DateTime.Now.ToString());
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
                Console.WriteLine("Dictionary GetDb_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return device;
        }

        public Dictionary<int, string> GetDb_SizeTubesCurrent(int id)
        {
            var sizeTubes = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTubesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                    sizetubes.Id_SizeTube,
                    sizetubes.SizeTube
                FROM
                    bufferdata
                Inner Join sizetubes ON sizetubes.Id_SizeTube = bufferdata.Id_SizeTube
                WHERE bufferdata.Id_Gost = @A", connection.mySqlConnection);
            try { myCommand.Parameters.AddWithValue("A", id); } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTubesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Add Parameters");
                throw (ex);
            }
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTubesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    sizeTubes.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_SizeTubesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("Dictionary GetDb_SizeTubesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return sizeTubes;
        }

        public Dictionary<int, string> GetDb_ControlSamplesCurrent(int id)
        {
            var controlSamples = new Dictionary<int, string>();
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSamplesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                controlsamples.Id_ControlSample,
                controlsamples.NameControlSample
                FROM
                controlsamples
                WHERE controlsamples.Id_SizeTube = @A", connection.mySqlConnection);
            try
            {
                myCommand.Parameters.AddWithValue("A", id);
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSamplesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Add Parameters");
                throw (ex);
            }
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSamplesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    controlSamples.Add(
                        mySqlReader.GetInt32(0),
                        mySqlReader.GetString(1));
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("Dictionary GetDb_ControlSamplesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("Dictionary GetDb_ControlSamplesCurrent()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return controlSamples;
        }

        public int GetDb_Last_WorkSmens()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_WorkSmen
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_WorkSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_TimeIntervalSmens()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_TimeIntervalSmen
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_TimeIntervalSmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_SurName1()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName1()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Operator1
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName1()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName1()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_SurName1()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_SurName2()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName2()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Operator2
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName2()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SurName2()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_SurName2()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_Gosts()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Gosts()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Gost
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Gosts()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Gosts()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Gosts()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_SizeTubes()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SizeTubes()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_SizeTube
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SizeTubes()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_SizeTubes()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_SizeTubes()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_ControlSamples()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ControlSamples()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_ControlSample
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ControlSamples()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ControlSamples()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_ControlSamples()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_ListDefects()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_NameDefect
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_ListDefects()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_Device()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                indexes.Id_Device
                FROM
                indexes
                ORDER BY
                indexes.Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Device()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_Part()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Part()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                NumberPart
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Part()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Part()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Part()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public double GetDb_Last_Ho()
        {
            double last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Ho()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Id_Sensor
                FROM
                indexes
                ORDER BY
                Ind DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Ho()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetDouble(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Ho()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Ho()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_Porog()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Porog()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Porog
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Porog()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Porog()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Porog()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_Current()
        {
            int last = 0;
            Connection connection = null;
            try
            {
                connection = new Connection();
                connection.Open();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Current()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Open BD");
                throw (ex);
            }
            MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                Current
                FROM
                defectsdata
                ORDER BY
                IndexData DESC
                LIMIT 1", connection.mySqlConnection);
            MySqlDataReader mySqlReader = null;
            try
            {
                mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Current()  :  " + DateTime.Now.ToString());
                Console.WriteLine("ExecuteReader");
                throw (ex);
            }
            try
            {
                while (mySqlReader.Read())
                {
                    last = mySqlReader.GetInt32(0);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Parameters.cs");
                Console.WriteLine("GetDb_Last_Current()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Read");
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
                Console.WriteLine("GetDb_Last_Current()  :  " + DateTime.Now.ToString());
                Console.WriteLine("Close BD");
                throw (ex);
            }
            return last;
        }

        public int GetDb_Last_NumberTube()
        {
            int last = 0;
            if (MainWindow.mainWindow.Parameters["part"] == GetDb_Last_Part())
            {
                Connection connection = new Connection();
                try
                {
                    connection = new Connection();
                    connection.Open();
                } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Parameters.cs");
                    Console.WriteLine("GetDb_Last_NumberTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Open BD");
                    throw (ex);
                }
                MySqlCommand myCommand = new MySqlCommand(@"
                SELECT
                defectsdata.NumberTube
                FROM
                defectsdata
                ORDER BY
                defectsdata.IndexData DESC
                LIMIT 1", connection.mySqlConnection);
                MySqlDataReader mySqlReader = null;
                try
                {
                    mySqlReader = (MySqlDataReader)myCommand.ExecuteReader();
                } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Parameters.cs");
                    Console.WriteLine("GetDb_Last_NumberTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ExecuteReader");
                    throw (ex);
                }
                try
                {
                    while (mySqlReader.Read())
                    {
                        last = mySqlReader.GetInt32(0);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Parameters.cs");
                    Console.WriteLine("GetDb_Last_NumberTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Read");
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
                    Console.WriteLine("GetDb_Last_NumberTube()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Close BD");
                    throw (ex);
                }
            }
            return last;
        }
    }
}
