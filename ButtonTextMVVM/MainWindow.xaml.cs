using System;
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

namespace ButtonTextMVVM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ButtonTextVM btn;
        private readonly SynchronizationContext _context;
     
        public MainWindow()
        {
            InitializeComponent();
            btn = new ButtonTextVM();
            this.DataContext = btn;
            _context = SynchronizationContext.Current;
           
            DoLoop();
           
           
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if ((CurrentLimit.Text == "") || (Voltage.Text == "") || (OVP.Text == "") || (UVL.Text == "") || (UVL.Text == ""))
                MessageBox.Show("Заполните все поля");
            else
            {
                if ((Convert.ToDouble(Voltage.Text) < 0) && (Convert.ToDouble(Voltage.Text) > 80))
                    MessageBox.Show("Напряжение должно быть не меньше нуля и не больше 80В");
                else
                {
                    if ((Convert.ToDouble(Voltage.Text) < 0) && (Convert.ToDouble(Voltage.Text) > 19))
                        MessageBox.Show("Ограничение силы тока должно быть не меньше нуля и не больше 19А");
                    else
                    {
                        if ((Convert.ToDouble(Voltage.Text) < 0) && (Convert.ToDouble(Voltage.Text) > 19))
                            MessageBox.Show("Ограничение силы тока должно быть не меньше нуля и не больше 19А");
                        else
                        {
                            if ((Convert.ToDouble(OVP.Text) < 0) && (Convert.ToDouble(OVP.Text) > 80) || (Convert.ToDouble(UVL.Text) < 0) && (Convert.ToDouble(UVL.Text) > 80))
                                MessageBox.Show("OVP и UVL быть не меньше нуля и не больше 80В");
                            else
                            {
                                btn.VoltagePart = Voltage.Text;
                                btn.CurrentLimitPart = CurrentLimit.Text;
                                btn.OVPPart = OVP.Text;
                                btn.UVLPart = UVL.Text;
                                btn.OCPPart = OCP.Text;
                            }
                        }
                    }
                }
            }
        }
       
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
           
                btn.Connect(ModelType.Text);
            
        }
        private void Disonnect_Click(object sender, RoutedEventArgs e)
        {
           
                btn.Disconnect();
            
        }
        private void OutputOn_CLick(object sender, RoutedEventArgs e)
        {
            btn.OutputEnable(true);
        }
        private void OutputOff_Click(object sender, RoutedEventArgs e)
        {
            btn.OutputEnable(false);
        }
        private void DoLoop()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    //lblTimer.Text = DateTime.Now.ToLongTimeString(); // no work
                    UpdateConnectionStatus(); // work
                }
            });
        }
      
        private void UpdateConnectionStatus()
        {
            //SynchronizationContext.Current, here, is context of running thread (Task)
            _context.Post(setConnectionStatus, btn.ConnectionStatus());
            _context.Post(setVoltageStatus, btn.VoltageStatus());
            _context.Post(setOutputStatus, btn.OutputStatus());
            _context.Post(setOvpStatus, btn.OVPStatus());
            _context.Post(setUvlStatus, btn.UVLStatus());
            _context.Post(setCurrentStatus, btn.CurrentStatus());
            _context.Post(setCurrentLimitStatus, btn.CurrentLimitStatus());
            _context.Post(setOCPStatus, btn.OCPStatus());
           
            
        }
       
        public void setConnectionStatus(object content)
        {
            StatusBar.Text = (string)content;
        }
      
        public void setVoltageStatus(object content)
        {
            VoltageStatus.Text = (string)content;
        }
        public void setCurrentStatus(object content)
        {
           CurrentStatus.Text = (string)content;
        }
        public void setCurrentLimitStatus(object content)
        {
            CurrentLimitStatus.Text = (string)content;
        }
        public void setOutputStatus(object content)
        {
            OutputStatus.Text = (string)content;
        }
        public void setOvpStatus(object content)
        {
            OVPStatus.Text = (string)content;
        }
        public void setUvlStatus(object content)
        {
            UVLStatus.Text = (string)content;
        }
        public void setOCPStatus(object content)
        {
            OCPStatus.Text = (string)content;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
