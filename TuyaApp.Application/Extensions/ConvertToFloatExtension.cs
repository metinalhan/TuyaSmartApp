using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuyaApp.Application.Extensions
{
    public static class ConvertToFloatExtension
    {
        public static float ToFloat(this object value)
        {
            return Convert.ToSingle(value);
        }
    }
}
