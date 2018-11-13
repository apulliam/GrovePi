using GrovePi.Common;

namespace GrovePi.Sensors
{
    public interface IUltrasonicRangerSensor
    {
        int MeasureInCentimeters();
    }

    internal class UltrasonicRangerSensor : IUltrasonicRangerSensor
    {
        private const byte CommandAddress = 7;
        private readonly GrovePi _device;
        private readonly Pin _pin;

        internal UltrasonicRangerSensor(GrovePi device, Pin pin)
        {
            _device = device;
            _pin = pin;
        }

        public int MeasureInCentimeters()
        {
            var buffer = new byte[4] {CommandAddress, (byte) _pin, Constants.Unused, Constants.Unused};
            var result = _device.DirectAccess.WriteBlock(buffer);
            if (!result.Succeeded())
            {
                return -1;
            }
            Delay.Milliseconds(50);
            buffer = new byte[3];
            result = _device.DirectAccess.ReadBlock(buffer);
            if (!result.Succeeded())
            {
                return -1;
            }
            return buffer[1]*256 + buffer[2];
        }
    }
}
