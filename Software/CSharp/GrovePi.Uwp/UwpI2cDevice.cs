using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrovePi
{
    internal class UwpI2cDevice : I2cDevice
    {
        private Windows.Devices.I2c.I2cDevice _internalDevice;

        public UwpI2cDevice(Windows.Devices.I2c.I2cDevice internalDevice)
        {
            _internalDevice = internalDevice;
        }

       
        public I2cDeviceResponse ReadBlock(byte[] buffer)
        {
            return new UwpI2cDeviceResponse(_internalDevice.ReadPartial(buffer)); 
        }

        public I2cDeviceResponse WriteByteData(byte command, byte value)
        {
            _internalDevice.Write(new byte[] { command, value });
            return new UwpI2cDeviceResponse();
        }

        public I2cDeviceResponse WriteBlock(byte[] buffer)
        {
            return new UwpI2cDeviceResponse(_internalDevice.WritePartial(buffer));
        }

       
    }
}
