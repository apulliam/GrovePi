using System;
using System.Collections.Generic;
using System.Text;

namespace GrovePi
{
    public interface I2cDeviceResponse
    {
        bool Succeeded();
    }

    public interface I2cDevice
    {
        I2cDeviceResponse WriteBlock(byte[] buffer);

         I2cDeviceResponse WriteByteData(byte command, byte value);

        //void Read(byte[] buffer);

        I2cDeviceResponse ReadBlock(byte[] buffer);
        
    }
}
