using System;
using System.Collections.Generic;
using System.Text;

namespace GrovePi
{
    internal class LinuxI2cDeviceResponse : I2cDeviceResponse
    {
        int _resultCode;
        public LinuxI2cDeviceResponse(int resultCode)
        {
            _resultCode = resultCode;
        }

        public bool Succeeded()
        {
            return _resultCode != -1; 
        }

        public int BytesRead()
        {
            return _resultCode == -1 ? 0 : _resultCode;
        }
    }
}
