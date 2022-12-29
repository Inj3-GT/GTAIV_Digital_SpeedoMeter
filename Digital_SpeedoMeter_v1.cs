using System.Drawing;
using System.Windows.Forms;
using GTA;

namespace Digital_Speedometer_ByInj3
{
    public class IprSpeedoMeter : Script
    {

        bool IprSpeedoMeterDraw;
        bool IprSpeedConv;
        bool IprPassenger;

        private void IprDisable()
        {       
            IprSpeedoMeterDraw = !IprSpeedoMeterDraw;                
        }

        string IprSpeedUnit = "mph";
        GTA.Font IprFont;
        
        public IprSpeedoMeter()
        {
        
            BindKey(Keys.F7, new KeyPressDelegate(IprDisable));
            IprFont = new GTA.Font(0.027f, FontScaling.ScreenUnits, true, false);
            PerFrameDrawing += new GTA.GraphicsEventHandler(IprDrawSpeed);
            IprSpeedConv = Settings.GetValueBool("SpeedUnit", "Configuration", true);
            IprSpeedoMeterDraw = Settings.GetValueBool("AutoLoad", "Configuration", true);
            IprPassenger = Settings.GetValueBool("PassengerSpeed", "Configuration", false);
            if (!IprSpeedConv) { IprSpeedUnit = (string)"km/h"; }
            
        }

        Color IprColorWhite = Color.FromArgb(250, Color.White);
        
        private void IprDrawSpeed(object sender, GraphicsEventArgs e)
        {
        
            if (!IprCheckBool())
            {
                return;
            }

            Vehicle IprCurVeh = Player.Character.CurrentVehicle;

            if (IprCurVeh != null)
            {
                if (!IprPassenger)
                {
                    Ped IprPedSeat = IprCurVeh.GetPedOnSeat(VehicleSeat.Driver);

                    if (IprPedSeat != Player)
                    {
                        return;
                    }
                }

                float IprW = Screen.PrimaryScreen.Bounds.Width;
                IprW = (IprW / 2.0f) - 45.0f;

                float IprH = Screen.PrimaryScreen.Bounds.Height;
                IprH -= 35.0f;

                float IprSpeed = IprCurVeh.Speed;              
                IprSpeed = IprSpeedConv ? (IprSpeed * 1.609344f) * 2.2f : (IprSpeed * 3.6f) * 1.5f;

                e.Graphics.DrawText(string.Format("{0} " + (string)IprSpeedUnit, (int)IprSpeed), (float)IprW, (float)IprH, IprColorWhite, IprFont);   
                
            }
        }
        
        private bool IprCheckBool()
        {
            return IprSpeedoMeterDraw && Player.CanControlCharacter && Player.Character.isInVehicle() && Player.Character.isSittingInVehicle();
        }
    }
}
