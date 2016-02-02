using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;

namespace test2
{
    internal class ArchiveControl
    {
        public ArchiveWindow archiveWindow;
        private int countDeffectsLine = 0;

        private Dictionary<string, string> _countDefectsDays;
        private Dictionary<string, string> _countDefectsMonths;
        private Dictionary<string, string> _countDefectsYears;
        private Dictionary<string, string> _countDefectsParts;
        private Dictionary<string, string> _countDefectsSmens;
        private Dictionary<string, string> _countDays;
        private Dictionary<string, string> _countMonths;
        private Dictionary<string, string> _countYears;
        private Dictionary<string, string> _countParts;
        private Dictionary<string, string> _countSmens;
        private Int64 _countLastIndex = 0;

        public Int64 lastIndex { get { return _countLastIndex; } }

        private bool _countsLoaded = false;

        public void Fist_TreeData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Connection connection = new Connection();
            try
            {
                try { connection.Open(); }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("Fist_TreeData()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("Open()");
                    throw (ex);
                }

                MySqlCommand myCommand = new MySqlCommand("SELECT DISTINCT YEAR (defectsdata.DatePr) FROM defectsdata", connection.mySqlConnection);
                MySqlDataReader dataReader = myCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    var item = new TreeViewItem
                    {
                        Uid = dataReader.GetString(0),
                        Tag = "year",
                        Header = dataReader.GetString(0),
                        ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                    };
                    item.Items.Add("*");
                    archiveWindow.TreeView_arc.Items.Add(item);
                }
                dataReader.Close();
            }
            catch
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                archiveWindow.Close();
                Console.WriteLine("ArchiveControl.cs-Fist_TreeData() DB Error");
            }
            Mouse.OverrideCursor = Cursors.Arrow;
            try { connection.Close(); connection = null; } catch { }
        }
//=================================================================
        public void Expander(RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Connection connection = null;
            MySqlCommand myCommand = null;
            try
            {
                connection = new Connection();
                connection.Open();
                var item = (TreeViewItem)e.OriginalSource;
                item.Items.Clear();
                #region Tag year
                if (item.Tag.ToString() == "year")
                {
                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT DISTINCT
DATE_FORMAT(DatePr, '%Y-%m')
FROM defectsdata
WHERE
YEAR(DatePr) = @Yr
", connection.mySqlConnection);
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("Yr", item.Uid);
                        MySqlDataReader dataReader = myCommand.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var itemMonth = new TreeViewItem
                            {
                                Uid = dataReader.GetString(0),
                                Tag = "month",
                                Header = dataReader.GetString(0),
                                ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                            };
                            itemMonth.Items.Add("*");
                            item.Items.Add(itemMonth);
                        }
                        dataReader.Close();
                    } catch { throw (new Exception("year")); }
                }
                #endregion
                #region Tag month
                if (item.Tag.ToString()== "month")
                {
                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT DISTINCT
DATE_FORMAT(DatePr, '%Y-%m-%d')
FROM defectsdata
WHERE
DATE_FORMAT(DatePr, '%Y-%m') = @Yr
", connection.mySqlConnection);
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("Yr", item.Uid);
                        MySqlDataReader dataReader = myCommand.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var itemDay = new TreeViewItem
                            {
                                Uid = dataReader.GetString(0),
                                Tag = "day",
                                Header = dataReader.GetString(0),
                                ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                            };
                            itemDay.Items.Add("*");
                            item.Items.Add(itemDay);
                        }
                        dataReader.Close();
                    }
                    catch { throw (new Exception("month")); }
                }
                #endregion
                #region Tag day
                if (item.Tag.ToString() == "day")
                {
                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT DISTINCT
worksmens.NameSmen,
DATE_FORMAT(defectsdata.DatePr, '%Y-%M-%d'),
DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d'),
indexes.Id_WorkSmen
FROM indexes
INNER JOIN defectsdata ON defectsdata.IndexData = indexes.IndexData
INNER JOIN worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
WHERE
defectsdata.DatePr = @A
", connection.mySqlConnection);
                        //ORDER BY worksmens.NameSmen
                        //";
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("A", item.Uid);
                        MySqlDataReader dataReader = myCommand.ExecuteReader();
                        while (dataReader.Read())
                        {
                            var itemSmens = new TreeViewItem
                            {
                                Uid = dataReader.GetString(2) + "|" + dataReader.GetString(3) + "+" + dataReader.GetString(2) + " / " + dataReader.GetString(0),
                                Tag = "smena",
                                Header = dataReader.GetString(0),
                                ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                            };
                            itemSmens.Items.Add("*");
                            item.Items.Add(itemSmens);
                        }
                        dataReader.Close();
                    }
                    catch { throw (new Exception("day")); }
                }
                #endregion
                #region Tag smena
                if (item.Tag.ToString() == "smena")
                {
                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT DISTINCT
defectsdata.NumberPart,
DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d'),
indexes.Id_WorkSmen,
worksmens.NameSmen
FROM indexes
INNER JOIN defectsdata ON defectsdata.IndexData = indexes.IndexData
INNER JOIN worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
WHERE
defectsdata.DatePr = @A
AND
worksmens.Id_WorkSmen = @B
", connection.mySqlConnection);
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("A", item.Uid.Split('|')[0]);
                        myCommand.Parameters.AddWithValue("B", item.Uid.Split('|')[1]);
                        MySqlDataReader dataRead = myCommand.ExecuteReader();
                        while (dataRead.Read())
                        {
                            var itemPart = new TreeViewItem
                            {
                                Uid = dataRead.GetString(0) + "|" + dataRead.GetString(1) + "|"
                                + dataRead.GetString(2) + "|" + dataRead.GetString(1) + "/"
                                + dataRead.GetString(3) + " / плавка " + dataRead.GetString(0),
                                Tag = "part",
                                Header = "Плавка № " + dataRead.GetString(0),
                                ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                            };
                            itemPart.Items.Add("*");
                            item.Items.Add(itemPart);
                        }
                        dataRead.Close();
                    }
                    catch { throw (new Exception("smena")); }
                }
                #endregion
                #region Tag part
                if (item.Tag.ToString()=="part")
                {
                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
defectsdata.NumberTube,
defectsdata.FlDefectTube,
defectsdata.TimePr,
defectsdata.IndexData
FROM indexes
INNER JOIN defectsdata ON defectsdata.IndexData = indexes.IndexData
INNER JOIN worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
WHERE
defectsdata.DatePr = @A
AND
worksmens.Id_WorkSmen = @B
AND
defectsdata.NumberPart = @C
", connection.mySqlConnection);
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("A", item.Uid.Split('|')[1]);
                        myCommand.Parameters.AddWithValue("B", item.Uid.Split('|')[2]);
                        myCommand.Parameters.AddWithValue("C", item.Uid.Split('|')[0]);
                        MySqlDataReader dataReader = myCommand.ExecuteReader();

                        while (dataReader.Read())
                        {
                            var itemTube = new TreeViewItem
                            {
                                Uid = dataReader.GetString(3),
                                Tag = "tube0",
                                Header = "Труба №" + dataReader.GetString(0),
                                ItemContainerStyle = archiveWindow.TreeView_arc.ItemContainerStyle
                            };
                            if (dataReader.GetInt32(0) == 0)
                            {
                                itemTube.Header = "К.О. (" + dataReader.GetString(2) + ")";
                            }
                            if (dataReader.GetInt32(1) == 1)
                            {
                                var colBrush = new SolidColorBrush
                                {
                                    Color = Colors.Red
                                };
                                itemTube.Tag = "tube1";
                                itemTube.Foreground = colBrush;
                            }
                            item.Items.Add(itemTube);
                        }
                        dataReader.Close();
                    }
                    catch { throw (new Exception("part")); }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("ArchiveControl.cs");
                Console.WriteLine("Expander()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
            try { connection.Close(); connection = null; } catch { }
        }

        //=====================================================================
        public void Tube_Control(TreeViewItem item)
        {
            Connection connection = new Connection();
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                connection.Open();
                if (item.Tag.ToString() == "tube0" || item.Tag.ToString() == "tube1")
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    myCommand.CommandText = @"
SELECT
defectsdata.IndexData,
defectsdata.NumberPart,
defectsdata.NumberTube,
defectsdata.NumberSegments,
defectsdata.DataSensors,
DATE_FORMAT(defectsdata.DatePr, '%Y-%m-%d'),
defectsdata.TimePr,
worksmens.NameSmen,
o1.Surname,
o2.Surname
FROM defectsdata
INNER JOIN indexes ON indexes.IndexData = defectsdata.IndexData
INNER JOIN worksmens ON worksmens.Id_WorkSmen = indexes.Id_WorkSmen
INNER JOIN operators o1 ON o1.Id_Operator = indexes.Id_Operator1
INNER JOIN operators o2 ON o2.Id_Operator = indexes.Id_Operator2
WHERE
indexes.IndexData = @A
LIMIT 1
";
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("A", item.Uid);
                    myCommand.Connection = connection.mySqlConnection;

                    MySqlDataReader dataReader = myCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        archiveWindow.Label1.Content = "Номер плавки\t" + dataReader.GetString(1);
                        archiveWindow.Label2.Content = "Номер трубы\t" + dataReader.GetString(2);
                        archiveWindow.Label3.Content = "Дата проведения Н.К.\t" + dataReader.GetString(5);
                        archiveWindow.Label4.Content = "Время проведения Н.К.\t" + dataReader.GetString(6);
                        archiveWindow.Label5.Content = "Длина трубы (метры)\t\t " + Math.Round((dataReader.GetDouble(3) / 6), 2);
                        archiveWindow.Label7.Content = "Рабочая смена\t" + dataReader.GetString(7);
                        archiveWindow.Label8.Content = "Специалист ОКПП\t" + dataReader.GetString(9);
                        archiveWindow.Label9.Content = "Специалист АСК ТЭСЦ-2\t" + dataReader.GetString(8);

                        for (int i = 0; i < countDeffectsLine; i++)
                        {
                            var line = (Line)archiveWindow.Canvas_Archive.FindName("errorLine" + i);
                            archiveWindow.Canvas_Archive.Children.Remove(line);
                            try
                            {
                                archiveWindow.Canvas_Archive.UnregisterName("errorLine" + i);
                            }
                            catch
                            {

                            }
                        }
                        countDeffectsLine = 0;

                        archiveWindow.Rectangle1.Width = dataReader.GetDouble(3) * 4;

                        var j = 0;

                        foreach (var deffect in (byte[])dataReader.GetValue(4))
                        {
                            if (deffect != 0)
                            {
                                var colorBrush = new SolidColorBrush
                                {
                                    Color = Colors.Red
                                };
                                var errorLine = new Line();

                                Canvas.SetLeft(errorLine, 40 + (j * 4));
                                errorLine.Opacity = 1;
                                errorLine.X1 = 0;
                                errorLine.X2 = 0;
                                errorLine.Y1 = 151;
                                errorLine.Y2 = errorLine.Y1 + 70;
                                errorLine.StrokeThickness = 4;
                                errorLine.Stroke = colorBrush;
                                errorLine.Fill = colorBrush;
                                archiveWindow.Canvas_Archive.RegisterName("errorLine" + countDeffectsLine, errorLine);
                                archiveWindow.Canvas_Archive.Children.Add(errorLine);
                                countDeffectsLine++;
                            }
                            j++;
                        }
                        archiveWindow.Label6.Content = "Кол-во дефектных сегментов\t " + countDeffectsLine;
                    }
                    dataReader.Close();
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            catch
            {
                Console.WriteLine("ArchiveControl.cs-Tube_Control() DB Error");
            }
            try { connection.Close(); connection = null; } catch { }
        }
        //======================================================================
        public void info_router(TreeViewItem item)
        {
            #region statistic tube
            string it = item.Tag.ToString();
            if (it == "tube0")
            {
                archiveWindow.Button_Otchet.IsEnabled = false;
                Tube_Control(item);
                return;
            }
            if (it == "tube1")
            {
                archiveWindow.Button_Otchet.IsEnabled = false;
                Tube_Control(item);
                return;
            }
            #endregion
            if (!_countsLoaded)
            {
                archiveWindow.listBox1.Items.Clear();
                archiveWindow.listBox1.Items.Add("Статистика еще загружается, попробуйте позже.");
                return;
            }
            try
            {
                switch (it)
                {
                    #region case "year" :
                    case "year":
                        {
                            archiveWindow.Button_Otchet.IsEnabled = false;
                            archiveWindow.listBox1.Items.Clear();
                            archiveWindow.listBox1.Items.Add("ВРЕМЯ: \t\t\t" + item.Uid);
                            try { archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t" + _countYears[item.Uid]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t0");
                                _countYears.Add(item.Uid, "0");
                            }
                            try { archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t" + _countDefectsYears[item.Uid]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t0");
                                _countDefectsYears.Add(item.Uid, "0");
                            }
                            double cd = Convert.ToInt32(_countDefectsYears[item.Uid]);
                            double c = Convert.ToInt32(_countYears[item.Uid]);
                            var result = Math.Round(((cd / c) * 100), 2);
                            archiveWindow.listBox1.Items.Add("ПРОЦЕНТ БРАКА: \t" + result);
                        }
                        break;
                    #endregion
                    #region case "month" :
                    case "month":
                        {
                            archiveWindow.Button_Otchet.IsEnabled = false;
                            archiveWindow.listBox1.Items.Clear();
                            archiveWindow.listBox1.Items.Add("ВРЕМЯ: \t\t\t" + item.Uid);
                            try { archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t" + _countMonths[item.Uid]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t0");
                                _countMonths.Add(item.Uid, "0");
                            }
                            try { archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t" + _countDefectsMonths[item.Uid]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t0");
                                _countDefectsMonths.Add(item.Uid, "0");
                            }
                            double cd = Convert.ToInt32(_countDefectsMonths[item.Uid]);
                            double c = Convert.ToInt32(_countMonths[item.Uid]);
                            var result = Math.Round(((cd / c) * 100), 2);
                            archiveWindow.listBox1.Items.Add("ПРОЦЕНТ БРАКА: \t" + result);
                        }
                        break;
                    #endregion
                    #region case "day" :
                    case "day":
                        {
                            archiveWindow.Button_Otchet.IsEnabled = false;
                            archiveWindow.listBox1.Items.Clear();
                            archiveWindow.listBox1.Items.Add("ВРЕМЯ: \t\t\t" + item.Uid);
                            archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t" + _countDays[item.Uid]);
                            try { archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t" + _countDefectsDays[item.Uid]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t0");
                                _countDefectsDays.Add(item.Uid, "0");
                            }
                            double cd = Convert.ToInt32(_countDefectsDays[item.Uid]);
                            double c = Convert.ToInt32(_countDays[item.Uid]);
                            var result = Math.Round(((cd / c) * 100), 2);
                            archiveWindow.listBox1.Items.Add("ПРОЦЕНТ БРАКА: \t" + result);
                        }
                        break;
                    #endregion
                    #region case "smena":
                    case "smena":
                        {
                            archiveWindow.Button_Otchet.IsEnabled = true;
                            archiveWindow.listBox1.Items.Clear();
                            archiveWindow.listBox1.Items.Add("ВРЕМЯ: \t\t\t" + item.Uid.Split('+')[1]);
                            archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t" + _countSmens[item.Uid.Split('+')[0]]);
                            try
                            {
                                string fn = item.Uid.Split('+')[0];
                                string nd = _countDefectsSmens[fn];
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t" + nd);
                            }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t0");
                                _countDefectsSmens.Add(item.Uid.Split('+')[0], "0");
                            }
                            double cd = Convert.ToInt32(_countDefectsSmens[item.Uid.Split('+')[0]]);
                            double c = Convert.ToInt32(_countSmens[item.Uid.Split('+')[0]]);
                            var result = Math.Round(((cd / c) * 100), 2);
                            archiveWindow.listBox1.Items.Add("ПРОЦЕНТ БРАКА: \t" + result);
                        }
                        break;
                    #endregion
                    #region case "part" :
                    case "part":
                        {
                            archiveWindow.Button_Otchet.IsEnabled = false;
                            archiveWindow.listBox1.Items.Clear();
                            archiveWindow.listBox1.Items.Add("ПЛАВКА: \t\t" + item.Uid.Split('|')[0]);
                            archiveWindow.listBox1.Items.Add("ТРУБ: \t\t\t" + _countParts[item.Uid.Split('|')[0]]);
                            try { archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t" + _countDefectsParts[item.Uid.Split('|')[0]]); }
                            catch
                            {
                                archiveWindow.listBox1.Items.Add("ДЕФЕКТНЫХ ТРУБ: \t0");
                                _countDefectsParts.Add(item.Uid.Split('|')[0], "0");
                            }
                            double cd = Convert.ToInt32(_countDefectsParts[item.Uid.Split('|')[0]]);
                            double c = Convert.ToInt32(_countParts[item.Uid.Split('|')[0]]);
                            var result = Math.Round(((cd / c) * 100), 2);
                            archiveWindow.listBox1.Items.Add("ПРОЦЕНТ БРАКА: \t" + result);
                        }
                        break;
                    #endregion
                    default:
                        {
                            archiveWindow.Button_Otchet.IsEnabled = false;
                        }
                        break;

                }
            }
            catch
            {
                Console.WriteLine("ArchiveControl.cs-info_router() DB Error");
            }
        }
        //=======================================================
        public void bgworkercounter()
        {
            var backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync();
        }

        //=====================================
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _countsLoaded = true;
        }

        //=======================================

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //var worker = sender as BackgroundWorker;
            CollectClass collectClass = new CollectClass();

            _countsLoaded = false;
            _countLastIndex = 0;

            Connection connection = null;
            MySqlCommand myCommand = null;
            MySqlDataReader dataReader = null;

            try
            {
                try
                {
                    connection = new Connection();
                    connection.Open();
                } catch
                { throw (new Exception("Open Error")); }

                try
                {
                    myCommand = new MySqlCommand(@"
SELECT Ind
FROM indexes
ORDER BY Ind DESC
LIMIT 1", connection.mySqlConnection);
                } catch
                { throw (new Exception("Open Error")); }

                try
                {
                    dataReader = myCommand.ExecuteReader();
                    dataReader.Read();
                    _countLastIndex = dataReader.GetInt64(0);
                } catch
                { throw (new Exception("Read Error")); }

                try
                {
                    dataReader.Close();
                    connection.Close();
                } catch
                { throw (new Exception("Close Error")); }

                _countYears = collectClass.Cyears();
                _countDefectsYears = collectClass.Cdyears();
                _countMonths = collectClass.Cmonths();
                _countDefectsMonths = collectClass.Cdmonths();
                _countDays = collectClass.Cdays();
                _countDefectsDays = collectClass.Cddays();
                _countSmens = collectClass.Csmens();
                _countDefectsSmens = collectClass.Cdsmens();
                _countParts = collectClass.Cparts();
                _countDefectsParts = collectClass.cdparts();

            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("ArchiveControl.cs");
                Console.WriteLine("backgroundWorker1_DoWork()  :  " + DateTime.Now.ToString());
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Ошибка загрузки статистики");
            }
        }
        //==================================
        public void count()
        {
            bgworkercounter();
        }
        //=====================================
        class CollectClass
        {
            //private readonly Connection connection = new Connection();
            private readonly MySqlCommand myCommand = new MySqlCommand();
            //private MySqlDataReader dataReader;

            // за год всего труб (без образцов)
            public Dictionary<string, string> Cyears()
            {
                var DictDYears = new Dictionary<string, string>();
                DictDYears.Clear();

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
                    } catch
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
                        myCommand.Parameters.AddWithValue("I", MainWindow.mainWindow.ac.lastIndex);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }
                    #endregion
                    #region Execute Read
                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }
                    #endregion
                    #region Read
                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDYears.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }
                    #endregion
                    #region Close
                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cyears()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }
                return DictDYears;
            }
            // за год дефектных (без образцов)
            public Dictionary<string, string> Cdyears()
            {
                var DictDYears = new Dictionary<string, string>();
                DictDYears.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
                    { throw (new Exception("Open Error")); }

                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
YEAR(DatePr)
FROM defectsdata
WHERE
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY YEAR(DatePr)
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    }
                    catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDYears.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }

                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }
                } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cdyears()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDYears;
            }
            //========================================================
            // за месяц труб
            public Dictionary<string,string> Cmonths()
            {
                var DictMonths = new Dictionary<string, string>();
                DictMonths.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
                    { throw (new Exception("Open Error")); }

                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m')
FROM defectsdata
WHERE
NumberTube <> 0
GROUP BY
DATE_FORMAT(DatePr, '%Y-%m')
ORDER BY
YEAR(DatePr),
MONTH(DatePr)
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictMonths.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }

                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cmonths()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictMonths;
            }

            // за месяц дефектных труб
            public Dictionary<string,string> Cdmonths()
            {
                var DictDMonths = new Dictionary<string, string>();
                DictDMonths.Clear();

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
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY
DATE_FORMAT(DatePr, '%Y-%m')
ORDER BY YEAR(DatePr), MONTH(DatePr)
", connection.mySqlConnection);
                    }
                    catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    }
                    catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDMonths.Add(dataReader.GetString(1), dataReader.GetString(0));
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
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cdmonths()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDMonths;
            }
            //=====================================================
            // за сутки труб

            public Dictionary<string, string> Cdays()
            {
                var DictDays = new Dictionary<string, string>();
                DictDays.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
                    { throw (new Exception("Open Error")); }

                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m-%d')
FROM defectsdata
WHERE
NumberTube <> 0
GROUP BY DatePr
ORDER BY DatePr
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    }
                    catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDays.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
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
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cdays()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDays;
            }

            // ==========
            // за сутки дефектных труб
            public Dictionary<string, string> Cddays()
            {
                var DictDays = new Dictionary<string, string>();
                DictDays.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
                    { throw (new Exception("Open Error")); }

                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
DATE_FORMAT(DatePr, '%Y-%m-%d')
FROM defectsdata
WHERE
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY DatePr
ORDER BY DatePr
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDays.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
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
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cddays()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDays;
            }

            //====================
            // за смену труб
            public Dictionary<string, string> Csmens()
            {
                var DictSmens = new Dictionary<string, string>();
                DictSmens.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
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
defectsdata.NumberTube <> 0
GROUP BY
defectsdata.DatePr,
indexes.Id_WorkSmen
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictSmens.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }

                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Csmens()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictSmens;
            }

            //===========================================
            // за смену дефектных труб
            public Dictionary<string, string> Cdsmens()
            {
                var DictDSmens = new Dictionary<string, string>();
                DictDSmens.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
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
defectsdata.FlDefectTube = 1
AND
defectsdata.NumberTube <> 0
GROUP BY
defectsdata.DatePr,
indexes.Id_WorkSmen
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDSmens.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }

                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cdsmens()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDSmens;
            }

            //=============================
            // за плавку труб
            public Dictionary<string, string> Cparts()
            {
                var DictParts = new Dictionary<string, string>();
                DictParts.Clear();

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
NumberTube <> 0
GROUP BY
NumberPart
", connection.mySqlConnection);
                    }
                    catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    }
                    catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictParts.Add(dataReader.GetString(1), dataReader.GetString(0));
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
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("Cparts()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictParts;
            }

            //=========================================
            // дефектных труб за плавку
            public Dictionary<string, string> cdparts()
            {
                var DictDParts = new Dictionary<string, string>();
                DictDParts.Clear();

                Connection connection = null;
                MySqlCommand myCommand = null;
                MySqlDataReader dataReader = null;

                try
                {
                    try
                    {
                        connection = new Connection();
                        connection.Open();
                    } catch
                    { throw (new Exception("Open Error")); }

                    try
                    {
                        myCommand = new MySqlCommand(@"
SELECT
Count(IndexData),
NumberPart
FROM defectsdata
WHERE
FlDefectTube = 1
AND
NumberTube <> 0
GROUP BY
NumberPart
", connection.mySqlConnection);
                    } catch
                    { throw (new Exception("MySqlCommand Error")); }

                    try
                    {
                        dataReader = myCommand.ExecuteReader();
                    } catch
                    { throw (new Exception("ExecuteRead Error")); }

                    try
                    {
                        while (dataReader.Read())
                        {
                            DictDParts.Add(dataReader.GetString(1), dataReader.GetString(0));
                        }
                    } catch
                    { throw (new Exception("Read Error")); }

                    try
                    {
                        dataReader.Close();
                        connection.Close();
                    } catch
                    { throw (new Exception("Close Error")); }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("ArchiveControl.cs");
                    Console.WriteLine("class CollectClass");
                    Console.WriteLine("cdparts()  :  " + DateTime.Now.ToString());
                    Console.WriteLine(ex.ToString());
                }

                return DictDParts;
            }


        }
        // ======================================================
        void addNewTube(string year, string mounth, string day, string smen, string party, int flDef)
        {

        }
        // ======================================================
    }
}
