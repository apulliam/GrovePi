using System;
using System.Collections.Generic;
using System.Text;

namespace GrovePi
{
    internal class LinuxI2cDevice : I2cDevice
    {
        private int _fileDescriptor = -1;
        internal LinuxI2cDevice(int fileDescriptor)
        {
            this._fileDescriptor = fileDescriptor;
        }

     
        public I2cDeviceResponse ReadBlock(byte[] buffer)
        {
            return new LinuxI2cDeviceResponse(I2cDev.I2cDev.i2c_smbus_read_i2c_block_data(_fileDescriptor, 1, (byte)buffer.Length, buffer));
        }

        public I2cDeviceResponse WriteBlock(byte[] buffer)
        {
            return new LinuxI2cDeviceResponse(I2cDev.I2cDev.i2c_smbus_write_i2c_block_data(_fileDescriptor, 1, (byte)buffer.Length, buffer));
        }

        //public int ReadByte()
        //{
	       // //for i in range(retries):
        //    return I2cDev.I2cDev.i2c_smbus_read_byte(_fileDescriptor);
        //}

        public I2cDeviceResponse WriteByteData(byte command, byte value)
        {
            return new LinuxI2cDeviceResponse(I2cDev.I2cDev.i2c_smbus_write_byte_data(_fileDescriptor, command, value));
        }
    }
}
