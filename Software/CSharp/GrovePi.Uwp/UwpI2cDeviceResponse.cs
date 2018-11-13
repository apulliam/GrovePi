using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrovePi
{
    internal class UwpI2cDeviceResponse : I2cDeviceResponse
    {
        private Windows.Devices.I2c.I2cTransferResult? _transferResult = null;
        public UwpI2cDeviceResponse()
        {

        }
        public UwpI2cDeviceResponse(Windows.Devices.I2c.I2cTransferResult transferResult)
        {
            _transferResult = transferResult;
        }
        public bool Succeeded()
        {
            if (_transferResult == null)
                return true;
            return _transferResult.Value.Status == Windows.Devices.I2c.I2cTransferStatus.FullTransfer;
        }
    }
}
