namespace GrovePi.Sensors
{
    public interface IVibrationSensor
    {
        int SensorValue();
    }

    internal class VibrationSensor : Sensor<IVibrationSensor>, IVibrationSensor
    {

        public VibrationSensor(IGrovePi device, Pin pin) : base(device, pin, PinMode.Input)
        {
        }

        public int SensorValue()
        {
            return Device.AnalogRead(Pin);
        }
    }
}
