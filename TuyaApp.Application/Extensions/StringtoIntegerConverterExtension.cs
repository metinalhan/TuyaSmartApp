using System;

namespace TuyaApp.Application.Extensions
{

    //This extension method for convertion string to int32
    public static class StringtoIntegerConverterExtension
    {
        public static int ToInt32(this object str)
        {
            return Convert.ToInt32(str);
        }
    }
}
