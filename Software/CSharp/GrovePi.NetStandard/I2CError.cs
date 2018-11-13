using System;

namespace GrovePi
{
    public class I2CError : Exception
    {
        public I2CError(string message) : base(message)
        { }
    }
}
