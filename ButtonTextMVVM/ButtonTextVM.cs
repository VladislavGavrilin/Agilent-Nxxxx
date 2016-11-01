using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Agilent.AgilentN57xx.Interop;
namespace ButtonTextMVVM
{
    class ButtonTextVM : INotifyPropertyChanged
    {
        private string currentPart = null;
        private string voltagePart = null;
        AgilentN57xx driver = new AgilentN57xx();
        public ButtonTextVM()
          {

          }
        public string ConnectionStatus()
        {
           
                if (driver.Initialized == true)
                {
                    return Convert.ToString(driver.Identity.InstrumentModel)+" подключен";
                  
            }
                else
        {
                   return "Источник питания отключен";
                   
        }

        }
            
        
        public AgilentN57xx Connect(string ModelType)
        {

           string resourceDesc = "GPIB0::5::INSTR";
           if (ModelType == "N5768A")
           {
               resourceDesc = "TCPIP0::169.254.84.86::INSTR";
               string initOptions = "QueryInstrStatus=true, Simulate=false, DriverSetup= Model=N5768A, Trace=false, TraceName=c:\\temp\\traceOut";
               try
               {
                   bool idquery = true;
                   bool reset = true;
                   driver.Initialize(resourceDesc, idquery, reset, initOptions);




               }
               catch (Exception e)
               {

                   MessageBox.Show(e.Message);


               }
           }
           if (ModelType == "N5745A")
           {
               resourceDesc = "TCPIP0::169.254.84.86::INSTR";
               string initOptions = "QueryInstrStatus=true, Simulate=true, DriverSetup= Model=N5745A , Trace=false, TraceName=c:\\temp\\traceOut";
               try
               {
                   bool idquery = true;
                   bool reset = true;
                   driver.Initialize(resourceDesc, idquery, reset, initOptions);




               }
               catch (Exception e)
               {

                   MessageBox.Show(e.Message);


               }
           }

           
           
          
           return driver;      
       }
        public void  Disconnect()
        {
            if (driver.Initialized == true)
                driver.Close();
            else
                MessageBox.Show("Driver is disconnected");
        }
        public string CurrentLimitPart
        {
           set
            {
                if (value == currentPart) return;
                
                OnPropertyChanged();
                
                 try 
                 {
                     var currentLimit = Convert.ToDouble(value);
                     driver.Output.CurrentLimit = currentLimit;
                     OnPropertyChanged();
                     
               
                 }
                catch (Exception e)
                 { 
                    MessageBox.Show(e.Message);
                }
                
                
            }
        }
        public string VoltagePart
        {
            get { return voltagePart; }
            set
            {
                //if (value == voltagePart) return;
                voltagePart = value;
                OnPropertyChanged();
                try
                {
                    var voltage = Convert.ToDouble(value);
                    if ((voltage <= 70))
                    {
                        
                        
                        //driver.Output.Protection.CurrentLimitBehavior = AgilentN57xxCurrentLimitBehaviorEnum.AgilentN57xxCurrentLimitTrip;
                        
                        driver.Output.VoltageLevel = voltage;  // initial voltage setting
                        System.Threading.Thread.Sleep(1000); // delay for output to settle
                        
                        
                    }
                    else
                    {
                        MessageBox.Show("Current must be lower than 2");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

        }
        public string OVPPart
        {
            get { return OVPPart; }
            set
            {
                try
                {
                    var ovp = Convert.ToDouble(value);
                                          //driver.Output.Protection.CurrentLimitBehavior = AgilentN57xxCurrentLimitBehaviorEnum.AgilentN57xxCurrentLimitTrip;
                        driver.Output.Protection.OVPLimit = ovp;
                       
                        System.Threading.Thread.Sleep(1000); // delay for output to settle
                }
                   
                
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            }
        public string UVLPart
        {
            get { return UVLPart; }
            set
            {
                try
                {
                    var uvl = Convert.ToDouble(value);
                    //driver.Output.Protection.CurrentLimitBehavior = AgilentN57xxCurrentLimitBehaviorEnum.AgilentN57xxCurrentLimitTrip;
                    driver.Output.MinVoltageLimit = uvl;

                    System.Threading.Thread.Sleep(1000); // delay for output to settle
                }


                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public string OCPPart
        {
            set
            {
                if (value=="On")
                {
                    driver.Output.Protection.CurrentLimitBehavior = AgilentN57xxCurrentLimitBehaviorEnum.AgilentN57xxCurrentLimitTrip;
                }
                if (value=="Off")
                {
                    driver.Output.Protection.CurrentLimitBehavior = AgilentN57xxCurrentLimitBehaviorEnum.AgilentN57xxCurrentLimitRegulate;
                }
            }    
        }
        
        public string OVPStatus()
        {
            if (driver.Initialized == true)
            {
                double s = driver.Output.Protection.OVPLimit;
                string ovpStatus = Convert.ToString(s);
                return ovpStatus;
                //OnPropertyChanged();
            }
            else return "";
        }
        public string CurrentLimitStatus()
        {
            if (driver.Initialized == true)
            {
                double s = driver.Output.CurrentLimit;
                string ovpStatus = Convert.ToString(s);
                return ovpStatus;
               
            }
            else return "";
        }
        public string CurrentStatus()
        {
            if (driver.Initialized == true)
            {
                double s = driver.Output.Measure(AgilentN57xxMeasureTypeEnum.AgilentN57xxMeasurementCurrent);
                string ovpStatus = Convert.ToString(s);
                return ovpStatus;
                //OnPropertyChanged();
            }
            else return "";
        }
        public string UVLStatus()
        {
            if (driver.Initialized == true)
            {
                double s = driver.Output.MinVoltageLimit;
                string uvlStatus = Convert.ToString(s);
                return uvlStatus;
                //OnPropertyChanged();
            }
            else return "";
        }
        public string OCPStatus()
        {
            if (driver.Initialized == true)
            {
                AgilentN57xxCurrentLimitBehaviorEnum s = driver.Output.Protection.CurrentLimitBehavior;
                string st = Convert.ToString(s);
                if (st=="AgilentN57xxCurrentLimitRegulate")
                    return "Off";
                if (st == "AgilentN57xxCurrentLimitTrip")
                    return "On";
                else return "Null";

                //OnPropertyChanged();
            }
            else return "";
        }

        public string VoltageStatus()
        {
            if (driver.Initialized==true){
                double s = driver.Output.Measure(AgilentN57xxMeasureTypeEnum.AgilentN57xxMeasurementVoltage);
                string voltageStatus = Convert.ToString(s);
                return voltageStatus;
                //OnPropertyChanged();
            }
            else return "";
        }

        public void OutputEnable(bool state)
        {
            driver.Output.Enabled = state;
        }

        public string OutputStatus()
        {
            if (driver.Initialized == true)
            {
                if (driver.Output.Enabled == true)
                    return "Выход включен";
                else
                    return "Выход отключен";
            }
            else return "Выход отключен";
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)

            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
