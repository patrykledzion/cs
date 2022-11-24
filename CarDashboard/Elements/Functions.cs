using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Collections;
using System.Net;
using System.Security.Policy;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class Functions
    { 
        private MainWindow window;

        public bool[] blinkers = new bool[2];
        public bool warningLights = false;
        public bool lights = false;
        public bool trafficLights = false;

        public Path[] _lights = new Path[6];
        public Path[] _tLights = new Path[6];

        public Functions(MainWindow window) 
        {
            this.window = window;

            _lights[0] = window.Lights_Main;
            _lights[1] = window.Lights_Beam1;
            _lights[2] = window.Lights_Beam2;
            _lights[3] = window.Lights_Beam3;
            _lights[4] = window.Lights_Beam4;
            _lights[5] = window.Lights_Beam5;

            for (int i = 1; i<_lights.Length; i++)
            {
                _lights[i].Width = 1;
            }

            _tLights[0] = window.TLights_Main;
            _tLights[1] = window.TLights_Beam1;
            _tLights[2] = window.TLights_Beam2;
            _tLights[3] = window.TLights_Beam3;
            _tLights[4] = window.TLights_Beam4;
            _tLights[5] = window.TLights_Beam5;
            _tLights[5] = window.TLights_Beam5;

            for (int i = 1; i<_lights.Length; i++)
            {
                _tLights[i].Width = 1;
            }


            

        }
        public void ToggleVisibility(Panel o)
        {
            if(o.Visibility==Visibility.Visible)
            {
                o.Visibility = Visibility.Hidden; 
            }
            else
            {
                o.Visibility= Visibility.Visible;
            }
        }

        public async Task BlinkerTrigger(Path o, int x)
        {
            if (blinkers[x]) blinkers[x] = false;
            else blinkers[x] = true;

            o.Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0));
            while (blinkers[x])
            {
                long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                milliseconds %= 10000;
                
                if ((milliseconds/100)%10<5) // DARK
                {
                    o.Fill = new SolidColorBrush(Color.FromArgb(255,3,23,0));
                }
                else                        // LIGHT
                {
                    o.Fill = new SolidColorBrush(Color.FromArgb(255,21,147,0));

                }

                await Task.Delay(10);
            }
        }

        public async Task WarningLightsTrigger(Path l, Path r)
        {

            if (warningLights) warningLights = false;
            else warningLights = true;

            l.Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0));
            r.Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0));
            while (warningLights)
            {
                long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                milliseconds %= 10000;

                if ((milliseconds/100)%10<5) // DARK
                {
                    l.Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0));
                    r.Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0));
                }
                else                        // LIGHT
                {
                    l.Fill = new SolidColorBrush(Color.FromArgb(255, 21, 147, 0));
                    r.Fill = new SolidColorBrush(Color.FromArgb(255, 21, 147, 0));

                }

                await Task.Delay(10);
            }
        }

        public async Task LightsTrigger()
        {
             
            if(!lights)
            {
                window.ButtonLights.IsEnabled = false;
                for (int i = 0; i < _lights.Length; i++)
                {
                    _lights[i].Fill = new SolidColorBrush(Color.FromArgb(255, 21, 147, 0)); // LIGHT

                    for (int j = 1; j<6; j++)
                    {
                        _lights[i].Width++;
                        await Task.Delay(1);
                        if (j==5 && i==_lights.Length-1)
                        {
                            lights = true;
                            window.ButtonLights.IsEnabled = true;
                            window.ButtonTrafficLights.IsEnabled = true;
                        }

                    }
                }
                 
            }
            else
            {
                window.ButtonLights.IsEnabled = false;
                for (int i = _lights.Length-1; i >= 0; i--)
                {
                    _lights[i].Fill = new SolidColorBrush(Color.FromArgb(255, 3, 23, 0)); // DARK

                    for (int j = 1; j<6; j++)
                    {
                        _lights[i].Width--;
                        await Task.Delay(1);
                        if (j==5 && i==0)
                        {
                            lights = false;
                            window.ButtonLights.IsEnabled = true;
                            window.ButtonTrafficLights.IsEnabled = false;
                        }
                    }

                    

                }

            }
            
        }

        public async Task TrafficLightsTrigger()
        {
            if (!trafficLights)
            {
                window.ButtonTrafficLights.IsEnabled = false;
                for (int i = 0; i < _lights.Length; i++)
                {
                    _tLights[i].Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 253)); // LIGHT
                    for (int j = 1; j<6; j++)
                    {
                        _tLights[i].Width++;
                        await Task.Delay(1);
                        if (j==5 && i==_tLights.Length-1)
                        {
                            trafficLights = true;
                            window.ButtonTrafficLights.IsEnabled = true;
                            window.ButtonLights.IsEnabled = false;
                        }

                    }      
                }

            }
            else
            {
                window.ButtonTrafficLights.IsEnabled = false;
                for (int i = _tLights.Length-1; i >= 0; i--)
                {
                    _tLights[i].Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 10)); // DARK

                    for (int j = 1; j<6; j++)
                    {
                        _tLights[i].Width--;
                        await Task.Delay(1);
                        if (j==5 && i==0)
                        {
                            trafficLights = false;
                            window.ButtonTrafficLights.IsEnabled = true;
                            window.ButtonLights.IsEnabled = true;
                        }
                    }
                }
            }
        }

        public void ChangeGearLetter(int gear, int number) //P R N D
        {
            switch(gear)
            {
                case 0:
                    window.GearLetter.Text = "P";
                    break;
                case 1:
                    window.GearLetter.Text = "R";
                    break;
                case 2:
                    window.GearLetter.Text = "N";
                    break;
                case 3:
                    window.GearLetter.Text = "D"+number.ToString();
                    break;
            }
        }
    }
}
