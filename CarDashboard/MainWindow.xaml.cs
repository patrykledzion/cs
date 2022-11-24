using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Functions func;
        public Engine engine;
        public MainWindow()
        {
            InitializeComponent();
            func = new Functions(this);
            engine = new Engine(this);
            _=engine.EngineWork();
        }

        private void LeftBlinkerClick(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            if (func.blinkers[1]) _=func.BlinkerTrigger(RightBlinker, 1);
            _=func.BlinkerTrigger(LeftBlinker,0);
        }

        private void RightBlinkerClick(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            if (func.blinkers[0]) _=func.BlinkerTrigger(LeftBlinker, 0);
            _=func.BlinkerTrigger(RightBlinker, 1);
        }

        private void WarningLightsClick(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            _=func.WarningLightsTrigger(LeftBlinker, RightBlinker);
        }

        private void LightsClick(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            _=func.LightsTrigger();
        }

        private void TrafficLightsClick(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            _=func.TrafficLightsTrigger();
        }


        private void AcceleratorPress(object sender, MouseButtonEventArgs e)
        {
            if (!engine.engineOn) return;
            engine.AcceleratorPress();
        }

        private void AcceleratorUp(object sender, MouseButtonEventArgs e)
        {
            if (!engine.engineOn) return;
            engine.AcceleratorUp();
        }

        private void ChangeGearP(object sender, RoutedEventArgs e)
        {
            //0P 1R 2N 3D
            if (!engine.engineOn) return;
            engine.ChangeGear(0);
        }

        private void ChangeGearR(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            engine.ChangeGear(1);
        }

        private void ChangeGearN(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            engine.ChangeGear(2);
        }

        private void ChangeGearD(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) return;
            engine.ChangeGear(3);
        }

        private void StartEngine(object sender, RoutedEventArgs e)
        {
            if (!engine.engineOn) engine.engineOn = true;
            else engine.engineOn = false;
        }
    }
}
