using GrovePi.I2CDevices;
using GrovePi.Sensors;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using I2cDev;

namespace GrovePi
{
    public static class DeviceFactory
    {
        public static IBuildGroveDevices Build = new DeviceBuilder();
    }
    
    public sealed class DeviceBuilder : DeviceBuilderBase
    {

        private readonly int O_RDWR = 0x0002;  // open for reading and writing
        private readonly int I2C_SLAVE = 0x0703;


        private int max_i2c_retries = 5;

        private string busName;
      

        public DeviceBuilder(int i2cBus = 1) 
        {
            busName = SMBusName();
        }

        private string SMBusName()
        {
            var directory = new DirectoryInfo("/dev/");
            var i2cFiles = directory.GetFiles().Where(m => m.Name.StartsWith("i2c-")).ToArray();

            if (i2cFiles.Count() == 1)
            {
                return i2cFiles[0].FullName;
            }

            if (i2cFiles.Count() > 1)
            {
                throw new ArgumentOutOfRangeException("More than one I2C bus was found. Please specify the bus directly in the I2cDevice constructor.");
            }
            else
            {
                throw new ArgumentOutOfRangeException("No I2C bus was found. Please check that there is an I2C bus for this device - perhaps use 'i2cdetect -l' from a terminal, or the Bifröst Cartographer.");
            }
        }

        private int initDevice(int address)
        {
            int current_retry = 0;
            int i2c_file_device = -1;
            var filename = SMBusName();

            // try to connect for a number of times
            while (current_retry < max_i2c_retries)
            {
                // increase the counter
                current_retry += 1;

                // open port for read/write operation
                if ((i2c_file_device = I2cDev.I2cDev.open(filename, O_RDWR)) < 0)
                {
                    Console.WriteLine("[failed to open i2c port]");
                    // try in the next loop to connect
                    continue;
                }
                // setting up port options and address of the device
                if (I2cDev.I2cDev.ioctl(i2c_file_device, I2C_SLAVE, address) < 0)
                {
                    Console.WriteLine("[unable to get bus access to talk to slave]");
                    // try in the next loop to connect
                    continue;
                }

                // if it got connected, then exit
                break;
            }

            // if connection couldn't be established
            // throw exception
            if (current_retry == max_i2c_retries)
                throw new I2CError("[I2CError on opening port]");

            return i2c_file_device;
        }


        protected override I2cDevice GetI2cDevice(int address)
        {

            return new LinuxI2cDevice(initDevice(address));
        }
    }
}