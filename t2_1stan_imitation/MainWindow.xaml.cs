using System;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using t2_1stan_imitation.Properties;

namespace t2_1stan_imitation
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DoubleAnimation _animation1 = new DoubleAnimation();
        private readonly Crc8 _crc8 = new Crc8();
        private readonly DispatcherTimer _moveTubeTimer = new DispatcherTimer();
        private readonly SerialPort _serialPort = new SerialPort();
        private byte _currentSegmentTube;
        private bool _errorState;
        private bool _randomizeState = false;
        private bool _sampleState;
        //private bool _positionDefectoscope;
        private const double PxMeterFactor = 30;
        private byte _segmentsTube;

        private bool rezOut = false;

        double positionRez;

        public MainWindow()
        {
            InitializeComponent();
            positionRez = Canvas.GetLeft(Rectangle5) + (Rectangle5.Width / 2);
            try
            {
                _serialPort.PortName = "COM5";
                _serialPort.BaudRate = 9600;
                _serialPort.Open();
                _moveTubeTimer.Tick += move_tube;
                _moveTubeTimer.Interval = TimeSpan.FromMilliseconds(Slider1.Value);
                _animation1.Duration = TimeSpan.FromMilliseconds(Slider1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //_positionDefectoscope = false;

            Canvas.SetTop(Rectangle3, 12);
            Canvas.SetTop(Rectangle5, 62);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            //_positionDefectoscope = true;

            Canvas.SetTop(Rectangle3, 25);
            Canvas.SetTop(Rectangle5, 75);
        }

        // button start
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Canvas.GetLeft(RectangleTube) < positionRez)
                {
                    TextBox1.IsEnabled = false;
                    Button2.IsEnabled = false;
                    _moveTubeTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void reset_position_tube(double left)
        {
            left = positionRez - (RectangleTube.Width + 1);
            _currentSegmentTube = 0;
            Label1.Content = _currentSegmentTube.ToString(CultureInfo.InvariantCulture);
            _moveTubeTimer.Stop();
            _animation1.From = Canvas.GetLeft(RectangleTube);
            _animation1.To = left;
            RectangleTube.BeginAnimation(Canvas.LeftProperty, _animation1);
            if (!rezOut)
            {
                PacOut3();
            }
        }

        private void move_tube(object sender, EventArgs e)
        {
            if (_randomizeState)
            {
                var rand = new Random();
                if (rand.Next(0, 100) <= 10)
                {
                    _errorState = true;
                }
            }
            
            try
            {
                // анимация трубы
                _animation1.From = Canvas.GetLeft(RectangleTube);
                _animation1.To = Canvas.GetLeft(RectangleTube) + 5;
                RectangleTube.BeginAnimation(Canvas.LeftProperty, _animation1);
                

                if ((Canvas.GetLeft(RectangleTube) < positionRez) && ((Canvas.GetLeft(RectangleTube) + RectangleTube.Width) >= positionRez))
                {
                    byte flErr = 0;
                    if (_errorState)
                        flErr = 1;
                    else
                        flErr = 0;

                    _errorState = false;

                    if (!_sampleState)
                    {
                        PacOut2(_currentSegmentTube, flErr);
                        if (_currentSegmentTube < 90)
                            _currentSegmentTube++;
                    }
                    else
                    {
                        PacOut1(flErr);
                    }
                }

                if (Canvas.GetLeft(RectangleTube) >= positionRez)
                {
                    if (!_sampleState)
                    {
                        PacOut3();
                    }
                    if (checkBoxAuto.IsChecked == true)
                    {
                        reset_rectangle_tube_width();
                        reset_position_tube(RectangleTube.Width - 112);
                        _moveTubeTimer.Start();
                    }
                    else
                    {
                        _moveTubeTimer.Stop();
                        TextBox1.IsEnabled = true;
                        Button2.IsEnabled = true;
                    }
                }
                /*
                if (Canvas.GetLeft(Rectangle5) <= (Canvas.GetLeft(RectangleTube) + RectangleTube.Width) &&
                                    _currentSegmentTube < _segmentsTube)

                                {
                                    if (_errorState && _positionDefectoscope)
                                    {
                                        if (!_sampleState)
                                        {
                                            PacOut2(_currentSegmentTube, 1); // очередной сегмент трубы
                                            _errorState = false;
                                        }
                                        else
                                        {
                                            PacOut1(1);
                                            _errorState = false;
                                        }
                                    }
                                    else
                                    {
                                        if (!_sampleState)
                                        {
                                            _errorState = false;
                                            PacOut2(_currentSegmentTube, 0); // очередной сегмент трубы
                                        }
                                        else
                                        {
                                            _errorState = false;
                                            PacOut1(0);
                                        }
                                    }

                                    _currentSegmentTube++;
                                }
                                else
                                {
                                    if (Canvas.GetLeft(Rectangle5) <= (Canvas.GetLeft(RectangleTube) + RectangleTube.Width) &&
                                        _currentSegmentTube > _segmentsTube - 1)
                                    {
                                        if (!_sampleState)
                                            PacOut3();
                                        _currentSegmentTube = 0;
                                    }
                                }

                if ((Canvas.GetLeft(Rectangle5) + Rectangle5.Width + 30) <= (Canvas.GetLeft(RectangleTube)))
                {
                    TextBox1.IsEnabled = true;
                    Button2.IsEnabled = true;
                    _moveTubeTimer.Stop();

                }
                */
                Label1.Content = _currentSegmentTube.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextBox1.IsEnabled = true;
            Button2.IsEnabled = true;
            _moveTubeTimer.Stop();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            reset_rectangle_tube_width();
            if ((Canvas.GetLeft(RectangleTube) + RectangleTube.Width) >= positionRez)
            {
                reset_position_tube(RectangleTube.Width - 112);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var ps = Settings.Default;
                Top = ps.Top;
                Left = ps.Left;

                reset_rectangle_tube_width();
                reset_position_tube(RectangleTube.Width - 112);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void reset_rectangle_tube_width()
        {
            try
            {
                // размер трубы
                double lenTube = Convert.ToInt32(TextBox1.Text);
                RectangleTube.Width = (lenTube/100)*PxMeterFactor;
                _segmentsTube = Convert.ToByte((lenTube/100)*30/5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                var ps = Settings.Default;
                ps.Top = Top;
                ps.Left = Left;
                ps.Save();

                _serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // ======================================================
        // 30 импульсов на метр.
        // 5 импульсов = 1 сегмент
        // контрольная сумма = crc8buf
        // ======================================================

        /*
        *    0xE6 0x19 0xFF заголовок
        *    0x08           длина пакета (включая контрольную сумму)
        *    0x01           вид пакета - состояние дефектоскопов при простое стана
        *    0xXX           байт состояния дефектов
        *    0x00           выравнивание длины
        *    0xCRC          контрольная сумма
        *    0x00 0x00 0x00 окончание пакета
        */

        private void PacOut1(byte deffect)
        {
            var packets = new byte[11];

            packets[0] = 0xE6;
            packets[1] = 0x19;
            packets[2] = 0xFF;
            packets[3] = 0x08;

            packets[4] = 0x01;
            packets[5] = deffect;
            packets[6] = 0x00;
            packets[7] = _crc8.ComputeChecksum(packets, 7);

            packets[8] = 0x00;
            packets[9] = 0x00;
            packets[10] = 0x00;

            _serialPort.Write(packets, 0, packets.Length);
        }

        /*
        *    0xE6 0x19 0xFF заголовок
        *    0x08           длина пакета (включая контрольную сумму)
        *    0x02           вид пакета - сегмент трубы
        *    0xNN           номер сегмента по раскладке трубы
        *    0xXX           байт состояния дефектов
        *    0xCRC          контрольная сумма
        *    0x00 0x00 0x00 окончание пакета
        */

// ReSharper disable InconsistentNaming
        private void PacOut2(byte NN, byte deffect)
// ReSharper restore InconsistentNaming
        {
            rezOut = false;
            var Packets = new byte[11];

            Packets[0] = 0xE6;
            Packets[1] = 0x19;
            Packets[2] = 0xFF;
            Packets[3] = 0x08;

            Packets[4] = 0x02;
            Packets[5] = NN;
            Packets[6] = deffect;
            Packets[7] = _crc8.ComputeChecksum(Packets, 7);

            Packets[8] = 0x00;
            Packets[9] = 0x00;
            Packets[10] = 0x00;

            _serialPort.Write(Packets, 0, Packets.Length);
        }

        /*
        *    0xE6 0x19 0xFF заголовок
        *    0x08           длина пакета (включая контрольную сумму)
        *    0x03           вид пакета - новая труба
        *    0xDL           длина трубы в сегментах ( один сегмент = 5 импульсов колеса )
        *    0x00           выравнивание длины
        *    0xCRC          контрольная сумма
        *    0x00 0x00 0x00 окончание пакета
        */

        private void PacOut3()
        {
            rezOut = true;
            var packets = new byte[11];

            packets[0] = 0xE6;
            packets[1] = 0x19;
            packets[2] = 0xFF;
            packets[3] = 0x08;

            packets[4] = 0x03;
            packets[5] = _segmentsTube;
            packets[6] = 0x00;
            packets[7] = _crc8.ComputeChecksum(packets, 7);

            packets[8] = 0x00;
            packets[9] = 0x00;
            packets[10] = 0x00;

            _serialPort.Write(packets, 0, packets.Length);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            _errorState = true;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _moveTubeTimer.Interval = TimeSpan.FromMilliseconds(Slider1.Value);
            _animation1.Duration = TimeSpan.FromMilliseconds(Slider1.Value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _sampleState = (!_sampleState);
            lbl1.Content = "Sample " + _sampleState;
            //PacOut1(0);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            _randomizeState = !_randomizeState;
        }
    }
}