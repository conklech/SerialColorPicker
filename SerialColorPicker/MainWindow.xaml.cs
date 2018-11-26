using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


using System.IO.Ports;
using System.Threading;
using System.Reactive.Linq;

namespace SerialColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SerialPort _serialPort;

        public MainWindow()
        {
            InitializeComponent();
            _serialPort = new SerialPort("COM3", 115200);

            _serialPort.Open();

            Observable
                .FromEventPattern<RoutedPropertyChangedEventArgs<Color?>>(ClrPcker_Background, "SelectedColorChanged")
                .Sample(TimeSpan.FromSeconds(0.1))
                .ObserveOnDispatcher()
                .Subscribe(x => HandleColor(x.EventArgs.NewValue.Value));
            

        }

        void HandleColor(Color e)
        {
            String s = String.Format("{0},{1},{2},{3}",
                ClrPcker_Background.SelectedColor.Value.R.ToString(),
                ClrPcker_Background.SelectedColor.Value.G.ToString(),
                ClrPcker_Background.SelectedColor.Value.B.ToString(),
                ClrPcker_Background.SelectedColor.Value.A.ToString());
            _serialPort.WriteLine(s);
            Console.WriteLine(s);
            //MessageBox.Show("#" + ClrPcker_Background.SelectedColor.Value.R.ToString() + ClrPcker_Background.SelectedColor.Value.G.ToString() + ClrPcker_Background.SelectedColor.Value.B.ToString());
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _serialPort.Close();
        }
    }
}
