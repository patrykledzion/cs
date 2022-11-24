using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Automation.Peers;

namespace WpfApp1
{
    public class Engine
    {
        //Engine
        public bool engineOn = false;
        public int engineSpeed = 0;
        int maxEngineSpeed = 7000;
        int minEngineSpeed = 1000;
        bool restarted = false;

        //Speed
        public int carSpeed = 0;
        int maxSpeed = 210;
        int targetSpeed = 0;
        int wheelsSpeedrpm = 0;
        int wheelsSpeed = 0;

        //Gear box
        public int gear = 0; //0-P, 1-R, 2-N, 3-D
        public int gearNumber = 1; //1-5
        private int maxGear = 5;
        float ratio = 0;

        //Accelerator
        bool acceleratorPressed;
        bool accelerateActive = true;
        
        //Stats
        float startMileage = 15012.7f; // przebieg
        float mileage = 0; 
        float trip = 0;
        public Rectangle[] TemperatureRects = new Rectangle[10];
        public Rectangle[] FuelRects = new Rectangle[10];
        int temperature = 10;
        int timeCounter = 0;
        float maxFuel = 40;
        float fuel = 4.1f;
        float fuelConsuption = 7.2f;

        private MainWindow window;

        public Engine(MainWindow window)
        {
            this.window = window;

            TemperatureRects[0] = window.Temp1;
            TemperatureRects[1] = window.Temp2;
            TemperatureRects[2] = window.Temp3;
            TemperatureRects[3] = window.Temp4;
            TemperatureRects[4] = window.Temp5;
            TemperatureRects[5] = window.Temp6;
            TemperatureRects[6] = window.Temp7;
            TemperatureRects[7] = window.Temp8;
            TemperatureRects[8] = window.Temp9;
            TemperatureRects[9] = window.Temp10;

            FuelRects[0] = window.Fuel1;
            FuelRects[1] = window.Fuel2;
            FuelRects[2] = window.Fuel3;
            FuelRects[3] = window.Fuel4;
            FuelRects[4] = window.Fuel5;
            FuelRects[5] = window.Fuel6;
            FuelRects[6] = window.Fuel7;
            FuelRects[7] = window.Fuel8;
            FuelRects[8] = window.Fuel9;
            FuelRects[9] = window.Fuel10;

 

        }

        public async Task EngineWork()
        {
            float _prevTrip = trip;
            while (true)
            {
                if (!engineOn)
                {
                    engineSpeed = Math.Max(minEngineSpeed, engineSpeed-50);
                    if (!restarted)
                    {
                        minEngineSpeed = 0;
                        window.GearLetter.Text = "";
                        gearNumber = 1;
                        targetSpeed = 0;
                        restarted = true;

                        for (int i = TemperatureRects.Length-1; i>=0; i--)
                        {
                            TemperatureRects[i].Fill = new SolidColorBrush(Color.FromArgb(255, 39, 39, 39));
                            await Task.Delay(50);
                        }
                        
                    }

                    

                }
                if (engineOn)
                {

                    restarted = false;
                    //Accelerate
                    if (!acceleratorPressed && engineSpeed<minEngineSpeed) engineSpeed = Math.Min(minEngineSpeed, engineSpeed+50/gearNumber);
                    else if (acceleratorPressed && carSpeed<maxSpeed) engineSpeed = Math.Min(maxEngineSpeed, engineSpeed+50/gearNumber);
                    else if (!acceleratorPressed) engineSpeed = Math.Max(minEngineSpeed, engineSpeed-50/gearNumber);

                    if (!acceleratorPressed && gear<3) targetSpeed = 0;

                    if (carSpeed < 0) carSpeed = 0;

                    //Engine Speed
                    if (gear==3) minEngineSpeed = 1500; //D
                    else minEngineSpeed = 1000;

                    //Gear Box

                    if (accelerateActive)
                    {

                        if (gearNumber==1) ratio = 3.58f;
                        else if (gearNumber==2) ratio = 1.93f;
                        else if (gearNumber==3) ratio = 1.41f;
                        else if (gearNumber==4) ratio = 1.11f;
                        else if (gearNumber==5) ratio = 0.88f;

                        if (gear<3)
                        {
                            ratio = 0;
                        }

                        wheelsSpeedrpm = (int)(engineSpeed / ratio);
                    }
                    window.func.ChangeGearLetter(gear, gearNumber);

                    if (engineSpeed > 3000 && gearNumber<maxGear && acceleratorPressed && gear==3)
                    {
                        _=GearUp();
                    }
                    else if (engineSpeed <=2500 && gearNumber>1 && !acceleratorPressed && gear==3 && accelerateActive)
                    {
                        _=GearDown();
                    }

                    //Speed
                    wheelsSpeed = 128 * wheelsSpeedrpm; // cm/min 
                    wheelsSpeed = (wheelsSpeedrpm * 3600) / 100000; // km/h

                    targetSpeed = (int)(wheelsSpeed-5);


                    //Stats (Trip, mileage, temperature, fuel)
                    //  Fuel
                    trip+=(float)carSpeed/57600;

                    if (trip-_prevTrip>0.1)
                    {
                        _prevTrip = trip;
                        fuel-=fuelConsuption/1000;
                    }

                    for (int i = 0; i<FuelRects.Length; i++)
                    {
                        if (i<fuel/(maxFuel/10)) FuelRects[i].Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        else FuelRects[i].Fill = new SolidColorBrush(Color.FromArgb(255, 39, 39, 39));
                    }

                    if (fuel<=4) FuelRects[0].Fill = new SolidColorBrush(Color.FromArgb(255, 253, 153, 0));
                    window.text.Text = fuel.ToString();

                    //  Trip & mileage
                    int _tripDiff = (int)(((float)trip-(int)trip)*10);

                    mileage= startMileage+trip;
                    int _mileageDiff = (int)(((float)mileage-(int)mileage)*10);

                    window.Mileage.Text = ((int)mileage).ToString()+"."+((int)_mileageDiff).ToString();
                    window.Trip.Text = ((int)trip).ToString()+"."+((int)_tripDiff).ToString();

                    //  Temperature
                    timeCounter++;
                    if (temperature<90 && timeCounter>10)
                    {
                        timeCounter = 0;
                        temperature++;
                    }

                    for (int i = 0; i<TemperatureRects.Length; i++)
                    {
                        if (i<temperature/18) TemperatureRects[i].Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        else TemperatureRects[i].Fill = new SolidColorBrush(Color.FromArgb(255, 39, 39, 39));
                    }


                }


                //Indicators (Speed, engine speed)
                _=GetTargetSpeed();

                RotateTransform rotateRight = new RotateTransform(-engineSpeed/27);
                rotateRight.CenterX = 0.009;
                rotateRight.CenterY = -0.005;
                window.RightPointer.RenderTransform = rotateRight;

                RotateTransform rotateLeft = new RotateTransform(carSpeed/0.9);
                rotateLeft.CenterX = 0.832;
                rotateLeft.CenterY = 0.01;
                window.LeftPointer.RenderTransform = rotateLeft;

                await Task.Delay(50);

            }      
        }

        public async Task GetTargetSpeed()
        {
            while(carSpeed>targetSpeed)
            {
                carSpeed--;
                if(gear<3)await Task.Delay(1000);
                else await Task.Delay(20);
                
            }

            while (carSpeed<targetSpeed)
            {
                carSpeed++;
                await Task.Delay(20);
            }
        }

        public async Task GearUp()
        {
            int targetEngineSpeed = engineSpeed;

            

            if (accelerateActive)
            {
                if (gearNumber==1) targetEngineSpeed = (int)(engineSpeed/1.7);
                else if (gearNumber==2) targetEngineSpeed = (int)(engineSpeed/1.32);
                else if (gearNumber==3) targetEngineSpeed = (int)(engineSpeed/1.24);
                else if (gearNumber==4) targetEngineSpeed = (int)(engineSpeed/1.22);
            }

            accelerateActive = false;

            while (engineSpeed > targetEngineSpeed)
            {
                engineSpeed-=150;
                
                if (engineSpeed<=targetEngineSpeed)
                {
                    gearNumber = Math.Min(maxGear, gearNumber+1);
                    window.func.ChangeGearLetter(gear, gearNumber);
                    accelerateActive = true;
                }
                await Task.Delay(10);

            }
        }

        public async Task GearDown()
        {
            int targetEngineSpeed = engineSpeed;

            if (accelerateActive)
            {
                if (gearNumber==2) targetEngineSpeed = (int)(engineSpeed*1.6);
                else if (gearNumber==3) targetEngineSpeed = (int)(engineSpeed*1.15);
                else if (gearNumber==4) targetEngineSpeed = (int)(engineSpeed*1.18);
                else if (gearNumber==5) targetEngineSpeed = (int)(engineSpeed*1.15);
            }

            accelerateActive = false;

            while (engineSpeed < targetEngineSpeed)
            {
                engineSpeed+=150;

                if (engineSpeed>=targetEngineSpeed)
                {
                    gearNumber = Math.Max(1, gearNumber-1);
                    window.func.ChangeGearLetter(gear, gearNumber);
                    accelerateActive = true;
                }
                await Task.Delay(10);

            }
        }

         

        public void AcceleratorPress()
        {
            if(accelerateActive)acceleratorPressed = true;
        }

        public void AcceleratorUp()
        {
            acceleratorPressed = false;
        }

        public void ChangeGear(int gear) // P R N D
        {
            this.gear = gear;
            window.func.ChangeGearLetter(gear, 1);
        }
    }
}
