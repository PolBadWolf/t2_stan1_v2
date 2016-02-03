using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public class CollectClass
    {
        // за год всего труб (без образцов)
        public void Cyears(Dictionary<string, string> DictDYears)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                #region Open
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }
                #endregion
                #region Sql Command
                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
YEAR(DatePr)
FROM defectsdata
WHERE
IndexData <= @I
AND
NumberTube <> 0
GROUP BY YEAR(DatePr)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }
                #endregion
                #region Execute Read
                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }
                #endregion
                #region Read
                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDYears, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }
                #endregion
                #region Close
                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cyears()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        // за год дефектных (без образцов)
        public void Cdyears(Dictionary<string, string> DictDYears)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
YEAR(DatePr)
FROM defectsdata
WHERE
IndexData <= @I
AND
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY YEAR(DatePr)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDYears, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cdyears()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        //========================================================
        // за месяц труб
        public void Cmonths(Dictionary<string, string> DictMonths)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m')
FROM defectsdata
WHERE
IndexData <= @I
AND
NumberTube <> 0
GROUP BY
DATE_FORMAT(DatePr, '%Y-%m')
ORDER BY
YEAR(DatePr),
MONTH(DatePr)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictMonths, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cmonths()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        // за месяц дефектных труб
        public void Cdmonths(Dictionary<string, string> DictDMonths)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m')
FROM defectsdata
WHERE
IndexData <= @I
AND
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY
DATE_FORMAT(DatePr, '%Y-%m')
ORDER BY YEAR(DatePr), MONTH(DatePr)
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDMonths, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cdmonths()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
        //=====================================================
        // за сутки труб

        public void Cdays(Dictionary<string, string> DictDays)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m-%d')
FROM defectsdata
WHERE
IndexData <= @I
AND
NumberTube <> 0
GROUP BY DatePr
ORDER BY DatePr
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDays, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cdays()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        // ==========
        // за сутки дефектных труб
        public void Cddays(Dictionary<string, string> DictDays)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m-%d')
FROM defectsdata
WHERE
IndexData <= @I
AND
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY DatePr
ORDER BY DatePr
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDays, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cddays()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        //====================
        // за смену труб
        public void Csmens(Dictionary<string, string> DictSmens)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;
            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(defectsdata.IndexData),
CONCAT(DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d'), '|', indexes.Id_WorkSmen)
FROM defectsdata
Inner Join indexes ON defectsdata.IndexData = indexes.IndexData
WHERE
defectsdata.IndexData <= @I
AND
defectsdata.NumberTube <> 0
GROUP BY defectsdata.DatePr, indexes.Id_WorkSmen", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictSmens, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Csmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
                throw (ex);
            }
        }

        //===========================================
        // за смену дефектных труб
        public void Cdsmens(Dictionary<string, string> DictDSmens)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(defectsdata.IndexData),
CONCAT(DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d'), '|', indexes.Id_WorkSmen)
FROM defectsdata
Inner Join indexes ON defectsdata.IndexData = indexes.IndexData
WHERE
defectsdata.IndexData <= @I
AND
defectsdata.FlDefectTube = 1
AND
defectsdata.NumberTube <> 0
GROUP BY
defectsdata.DatePr,
indexes.Id_WorkSmen
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDSmens, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cdsmens()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        //=============================
        // за плавку труб
        public void Cparts(Dictionary<string, string> DictParts)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
NumberPart
FROM defectsdata
WHERE
IndexData <= @I
AND
NumberTube <> 0
GROUP BY
NumberPart
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictParts, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("Cparts()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        //=========================================
        // дефектных труб за плавку
        public void cdparts(Dictionary<string, string> DictDParts)
        {
            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                }
                catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
NumberPart
FROM defectsdata
WHERE
IndexData <= @I
AND
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY
NumberPart
", connection.mySqlConnection);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("I", MainWindow.ac.lastIndex);
                }
                catch
                { throw (new Exception("MySqlCommand Error")); }

                try
                {
                    dataReader = (MySqlDataReader)myCommand.ExecuteReader();
                }
                catch
                { throw (new Exception("ExecuteRead Error")); }

                try
                {
                    while (dataReader.Read())
                    {
                        ArchiveControl.addStat(DictDParts, dataReader.GetString(1), dataReader.GetInt32(0));
                    }
                }
                catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                }
                catch
                { throw (new Exception("Close Error")); }

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("CollectClass.cs");
                Console.WriteLine("cdparts()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
