using System.Windows.Input;

namespace TuyaApp.Application.Helpers.DeviceHelpers
{
    //This is static method can help only allow numeric from keyboard press event
    public static class JustNumericInputHelper
    {
        public static bool CanInput(KeyEventArgs e)
        {
            
            if (e.Key >= Key.A && e.Key <= Key.Z || e.Key == Key.OemPeriod || e.Key == Key.OemComma || e.Key == Key.Decimal
               || e.Key == Key.Multiply || e.Key == Key.Divide || e.Key == Key.Add || e.Key == Key.Subtract || e.Key == Key.Space)
            {

                return true;
            }
           

               return false;
           
        }
    }
}
