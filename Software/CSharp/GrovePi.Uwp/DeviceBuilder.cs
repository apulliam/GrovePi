using GrovePi.I2CDevices;
using GrovePi.Sensors;
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace GrovePi
{
    public static class DeviceFactory
    {
        public static IBuildGroveDevices Build = new DeviceBuilder();
    }



    public sealed class DeviceBuilder : DeviceBuilderBase
    {
        private const string I2CName = "I2C1"; /* For Raspberry Pi 2, use I2C1 */
        private Windows.Devices.Enumeration.DeviceInformationCollection i2cDevices;
        public DeviceBuilder()
        {
            i2cDevices = GetDeviceInfo();
        }

        private DeviceInformationCollection GetDeviceInfo()
        {
            //Find the selector string for the I2C bus controller
            var deviceSelector = Windows.Devices.I2c.I2cDevice.GetDeviceSelector(I2CName);
            //Find the I2C bus controller device with our selector string
            return DeviceInformation.FindAllAsync(deviceSelector).GetResults();
        }

        protected override I2cDevice GetI2cDevice(int address)
        {
            ///* Initialize the I2C bus */
            var connectionSettings = new I2cConnectionSettings(address)
            {
                BusSpeed = I2cBusSpeed.StandardMode
            };
          
            // Create an I2cDevice with our selected bus controller and I2C settings
            return new UwpI2cDevice(Windows.Devices.I2c.I2cDevice.FromIdAsync(i2cDevices[0].Id, connectionSettings).GetResults());
        }
    }

}