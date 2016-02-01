using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using test2.Properties;

namespace test2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Parameters parametrs = new Parameters();
        private readonly Writer write = new Writer();
        // var
        public int _count = 0;
        public int _countSample = 0;
        public ArrayList indxSample = new ArrayList();
        // dic
        public Dictionary<string, int>    Parameters  = new Dictionary<string, int>();
        public Dictionary<string, double> Parameters2 = new Dictionary<string, double>();
        //
        public BDEditorWindow bdEditorWindow = null;

        public ArchiveWindow archiveWindow = null;
        public Thread myThrArchive = null;
        public static MainWindow mainWindow = null;

        private int flNewTube = 2;

        public System.IO.FileStream log_file = null;
        public System.IO.StreamWriter log_sw = null;
        public System.Windows.Threading.DispatcherTimer _LogTimer = new System.Windows.Threading.DispatcherTimer();

        private ArchiveControl ac_m = new ArchiveControl();

        internal ArchiveControl ac { get { return ac_m; } }

        public test2.Parameters parAdvn
        {
            get { return parametrs; }
        }

        private void flushLog(object sender, EventArgs e)
       {
            if (log_sw != null)
            {
                try { log_sw.Flush(); }
                catch { }
            }
        }
        public MainWindow()
        {
            log_file = new System.IO.FileStream("log3.txt", System.IO.FileMode.Append);
            log_sw = new System.IO.StreamWriter(log_file);
            InitializeComponent();
            Console.SetOut(log_sw);
            _LogTimer.Interval = System.TimeSpan.FromSeconds(5);
            _LogTimer.Tick += flushLog;
            _LogTimer.Start();
            Console.WriteLine("========================================");
            Console.WriteLine("MainWindow()");
            Console.WriteLine(DateTime.Now.ToString());
            log_sw.Flush();
            write.mainWindow = this;
            try
            {
                button_NP_Click(null, null);
                button_NS_Click(null, null);
            } catch { }
            TabItem1.Visibility = Visibility.Hidden;
            TabItem2.Visibility = Visibility.Hidden;
            TabItem3.Visibility = Visibility.Hidden;
            TabItem4.Visibility = Visibility.Hidden;
            //
            mainWindow = this;
        }

        private void FillParameters()
        {
            try
            {
                Parameters.Clear();
                try { Parameters.Add("smena", ((KeyValuePair<int, string>)ComBoxSmena.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBoxSmena");
                    throw (ex);
                }
                try { Parameters.Add("smena_time", ((KeyValuePair<int, string>)ComBoxVremiaSmeny.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBoxVremiaSmeny");
                    throw (ex);
                }
                try { Parameters.Add("operator1", ((KeyValuePair<int, string>)ComBoxSpecialistASK.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBoxSpecialistASK");
                    throw (ex);
                }
                try { Parameters.Add("operator2", ((KeyValuePair<int, string>)ComBoxSpecialistOKKP.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBoxSpecialistOKKP");
                    throw (ex);
                }
                try { Parameters.Add("part", Convert.ToInt32(TextBox_nPlavki.Text)); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("TextBox_nPlavki");
                    throw (ex);
                }
                try { Parameters.Add("gost", ((KeyValuePair<int, string>)ComBox_normDoc.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBox_normDoc");
                    throw (ex);
                }
                try { Parameters.Add("diameter", ((KeyValuePair<int, string>)ComBox_d.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBox_d");
                    throw (ex);
                }
                try { Parameters.Add("control_sample", ((KeyValuePair<int, string>)ComBox_sample.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBox_sample");
                    throw (ex);
                }
                try { Parameters.Add("name_defect", ((KeyValuePair<int, string>)ComBox_defect.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBox_defect");
                    throw (ex);
                }
                try { Parameters.Add("device", ((KeyValuePair<int, string>)ComBox_hard.SelectedItem).Key); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("ComBox_hard");
                    throw (ex);
                }
                try { Parameters.Add("porog", Convert.ToInt32(TextBox_porog.Text)); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("TextBox_porog");
                    throw (ex);
                }
                try { Parameters.Add("current", Convert.ToInt32(TextBox_tok.Text)); } catch (Exception ex)
                {
                    Console.WriteLine("========================================");
                    Console.WriteLine("MainWindow.xaml.cs");
                    Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                    Console.WriteLine("TextBox_tok");
                    throw (ex);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            Parameters2.Clear();
            try
            {
                Parameters2.Add("ho", Convert.ToDouble(TextBox_Ho.Text));
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("MainWindow.xaml.cs");
                Console.WriteLine("FillParameters()  :  " + DateTime.Now.ToString());
                Console.WriteLine("TextBox_Ho");
                throw (ex);
            }
            //
            lblinfo1.Content = ((KeyValuePair<int, string>)ComBoxSmena.SelectedItem).Value;
            lblinfo2.Content = "Специалист по АСК ТЭСЦ 2:\t" + ((KeyValuePair<int, string>)ComBoxSpecialistASK.SelectedItem).Value;
            lblinfo3.Content = "Специалист ОККП:\t" + ((KeyValuePair<int, string>)ComBoxSpecialistOKKP.SelectedItem).Value;
            lblinfo4.Content = "Номер плавки:\t" + TextBox_nPlavki.Text;
            lblinfo5.Content = "Нормативные документы:\t" + ((KeyValuePair<int, string>)ComBox_normDoc.SelectedItem).Value;
            lblinfo6.Content = "Пройдено труб:\t\t" + parametrs.GetDb_Last_NumberTube();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ps = Settings.Default;
            Top = ps.Top;
            Left = ps.Left;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Закрыть ?","Вопрос",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            {
                try
                {
                    bdEditorWindow.Close();
                    bdEditorWindow = null;
                    myThrArchive = null;
                }
                catch
                {
                    
                }
                try
                {
                    archiveWindow.Dispatcher.Invoke(delegate
                    {
                        archiveWindow.Close();
                        archiveWindow = null;
                    });
                }
                catch
                {

                }
                var ps = Settings.Default;
                ps.Top = Top;
                ps.Left = Left;
                ps.Save();
            }
            else
            {
                e.Cancel = true;
            }
        }

        //==
        public void New_Tube()
        {
            if (flNewTube > 1)
            {
                Dispatcher.Invoke(new ThreadStart(delegate
                {
                    Tube.Width = 0;
                    for (int i = 0; i < _count; i++)
                    {
                        Canvas.Children.Remove((UIElement)Canvas.FindName("errorLine" + i));
                        try { Canvas.UnregisterName("errorLine" + i); } catch { }
                    }
                    _count = 0;
                }));
                flNewTube = 0;
            }
            else
                flNewTube++;
        }
        //===========================
        public void Move_Tube()
        {
            if (flNewTube>0)
            {
                flNewTube = 2;
                New_Tube();
            }
            Dispatcher.Invoke(new ThreadStart(delegate
            {
                Tube.Width += 4;
            }));
        }
        public void Move_Sample_Tube()
        {
            TubeSample.Width += 4;
        }
        public void Error_Segment()
        {
            Dispatcher.Invoke(new ThreadStart(delegate
            {
                try
                {
                    var redBrush = new SolidColorBrush { Color = Colors.Red };
                    var errorLine = new Line();

                    Canvas.SetLeft(errorLine, Tube.Width + Canvas.GetLeft(Tube) - 4);
                    errorLine.X1 = 0;
                    errorLine.X2 = 0;
                    errorLine.Y1 = 151;
                    errorLine.Y2 = errorLine.Y1 + 70;
                    errorLine.StrokeThickness = 4;
                    errorLine.Stroke = redBrush;
                    errorLine.Fill = redBrush;
                    Canvas.RegisterName("errorLine" + _count, errorLine);
                    _count++;
                    Canvas.Children.Add(errorLine);
                } catch { }
            }));
        }
        public void Error_Sample_Segment(int segment)
        {
            Dispatcher.Invoke(new ThreadStart(delegate
            {
                try
                {
                    var redBrush = new SolidColorBrush { Color = Colors.Red };
                    var errorLine = new Line();

                    Canvas.SetLeft(errorLine, (Canvas.GetLeft(Tube) + (segment * 4)));
                    errorLine.X1 = 0;
                    errorLine.X2 = 0;
                    errorLine.Y1 = 151;
                    errorLine.Y2 = errorLine.Y1 + 70;
                    errorLine.StrokeThickness = 4;
                    errorLine.Stroke = redBrush;
                    errorLine.Fill = redBrush;
                    string regStr = "errorLine" + _count.ToString();
                    Canvas1.RegisterName(regStr, errorLine);
                    indxSample.Add(Canvas1.Children.Add(errorLine));
                } catch { }
                _count++;
            }));
        }

        private void button_NS_Click(object sender, RoutedEventArgs e)
        {
            TabItem2.IsEnabled = false;
            TabItem3.IsEnabled = false;
            TabControl1.SelectedIndex = 0;
            try
            {
                ComBoxSmena.ItemsSource = parametrs.GetDb_WorkSmens();
                ComBoxVremiaSmeny.ItemsSource = parametrs.GetDb_TimeIntervalSmens();
                ComBoxSpecialistASK.ItemsSource = parametrs.GetDb_SurNames();
                ComBoxSpecialistOKKP.ItemsSource = parametrs.GetDb_SurNames();
                //
                var last = parametrs.GetDb_Last_WorkSmens();
                foreach (var item in ComBoxSmena.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBoxSmena.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_TimeIntervalSmens();
                foreach (var item in ComBoxVremiaSmeny.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBoxVremiaSmeny.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_SurName1();
                foreach (var item in ComBoxSpecialistASK.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBoxSpecialistASK.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_SurName2();
                foreach (var item in ComBoxSpecialistOKKP.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBoxSpecialistOKKP.SelectedItem = item;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_NP_Click(object sender, RoutedEventArgs e)
        {
            TabItem1.IsEnabled = false;
            TabItem3.IsEnabled = false;
            TabControl1.SelectedIndex = 1;
            try
            {
                ComBox_normDoc.ItemsSource = parametrs.GetDb_Gost();
                ComBox_d.ItemsSource = parametrs.GetDb_SizeTube();
                ComBox_sample.ItemsSource = parametrs.GetDb_ControlSample();
                ComBox_defect.ItemsSource = parametrs.GetDb_ListDefects();
                ComBox_hard.ItemsSource = parametrs.GetDb_Device();
                //
                var last = parametrs.GetDb_Last_Gosts();
                foreach (var item in ComBox_normDoc.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBox_normDoc.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_SizeTubes();
                foreach (var item in ComBox_d.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBox_d.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_ControlSamples();
                foreach (var item in ComBox_sample.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBox_sample.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_ListDefects();
                foreach (var item in ComBox_defect.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBox_defect.SelectedItem = item;
                }
                //
                last = parametrs.GetDb_Last_Device();
                foreach (var item in ComBox_hard.Items)
                {
                    if (((KeyValuePair<int, string>)item).Key == last)
                        ComBox_hard.SelectedItem = item;
                }
                //
                TextBox_nPlavki.Text = parametrs.GetDb_Last_Part().ToString();
                TextBox_Ho.Text = parametrs.GetDb_Last_Ho().ToString();
                TextBox_porog.Text = parametrs.GetDb_Last_Porog().ToString();
                TextBox_tok.Text = parametrs.GetDb_Last_Current().ToString();
                if ((Parameters.Count == 12) && (Parameters2.Count == 1))
                {
                    TextBox_nPlavki.Text = Parameters ["part"].ToString();
                    TextBox_Ho.Text      = Parameters2["ho"].ToString();
                    TextBox_porog.Text   = Parameters ["porog"].ToString();
                    TextBox_tok.Text     = Parameters ["current"].ToString();
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw (ex);
            }
        }

        public void button_CONTROL_Click(object sender, RoutedEventArgs e)
        {
            if (ComBoxSmena.SelectedIndex != -1 &&
                ComBoxVremiaSmeny.SelectedIndex != -1 &&
                ComBoxSpecialistASK.SelectedIndex != -1 &&
                ComBoxSpecialistOKKP.SelectedIndex != -1 &&
                TextBox_nPlavki.Text != "" &&
                ComBox_normDoc.SelectedIndex != -1 &&
                ComBox_sample.SelectedIndex != -1 &&
                ComBox_d.SelectedIndex != -1 &&
                TextBox_Ho.Text != "" &&
                ComBox_defect.SelectedIndex != -1 &&
                ComBox_hard.SelectedIndex != -1 &&
                TextBox_porog.Text != "" &&
                TextBox_tok.Text != "")
            {
                FillParameters();
                try
                {
                    write.PortOpen();
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения к COM порту");
                }
                TabControl1.SelectedIndex = 3;
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        public void button_KO_Click(object sender, RoutedEventArgs e)
        {
            if (ComBoxSmena.SelectedIndex != -1 &&
                ComBoxSpecialistASK.SelectedIndex != -1 &&
                ComBoxSpecialistOKKP.SelectedIndex != -1 &&
                ComBoxVremiaSmeny.SelectedIndex != -1 &&
                ComBox_d.SelectedIndex != -1 &&
                ComBox_defect.SelectedIndex != -1 &&
                ComBox_hard.SelectedIndex != -1 &&
                ComBox_normDoc.SelectedIndex != -1 &&
                ComBox_sample.SelectedIndex != -1 &&
                TextBox_Ho.Text != "" &&
                TextBox_nPlavki.Text != "" &&
                TextBox_porog.Text != "" &&
                TextBox_tok.Text != "")
            {
                try
                {
                    write.PortOpen();
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения к COM порту");
                }
                TabControl1.SelectedIndex = 2;
                New_tube_sample();
                write.sampleDataCount = 0;
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bdEditorWindow = new BDEditorWindow();
            bdEditorWindow.Show();
        }

        private void Button_Archive_Click(object sender, RoutedEventArgs e)
        {
            if (myThrArchive == null)
            {
                myThrArchive = new Thread(new ThreadStart(showArchiveWindow));
                myThrArchive.SetApartmentState(ApartmentState.STA);
                myThrArchive.IsBackground = true;
                myThrArchive.Start();
            }
        }
        private void showArchiveWindow()
        {
            try
            {
                archiveWindow = new ArchiveWindow();
                archiveWindow.Show();
                System.Windows.Threading.Dispatcher.Run();
            } catch { }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            New_tube_sample();
            write.sampleDataCount = 0;
        }

        public void New_tube_sample()
        {
            TubeSample.Width = 0;
            /*
            for (int i = 0; i < _count; i++)
            {
                try
                {
                    Canvas1.Children.Remove((UIElement)Canvas1.FindName("errorLine" + i));
                    Canvas1.UnregisterName("errorLine" + i);
                }
                catch
                {

                }
            }
            _count = 0;
            */
            while (_countSample>0)
            {
                //                try { Canvas1.Children.Remove((UIElement)Canvas1.FindName("errorLine" + (_countSample - 1).ToString() ));  } catch { }
                //string regStr = "errorLine" + (_countSample - 1).ToString();
                //UIElement qw = (UIElement)Canvas1.FindName(regStr);
                //qw.Visibility = 0;
                //try { Canvas1.Children.Remove(qw); } catch { }
                try { Canvas1.Children.RemoveAt((int)indxSample[_countSample - 1]); } catch { }
                try { Canvas1.UnregisterName("errorLine" + (_countSample - 1).ToString()); } catch { }
                _countSample--;
            }
            indxSample.Clear();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try { write.SaveSample(); } catch { }
            try { New_tube_sample(); } catch { }
            try { write.sampleDataCount = 0; } catch { }
        }

        private void BdStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BDSettingsWindow win = new BDSettingsWindow();
            var x = win.ShowDialog();
            try
            {
                MainWindow.mainWindow.BdStatus.Text = " Status BD : fail ";
                button_NP_Click(null, null);
            }
            catch { }

        }
    }
}
