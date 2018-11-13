namespace GrovePi.Sensors
{
    public interface ITouchSensor
    {
        SensorStatus CurrentState { get; }
    }

    internal class TouchSensor : Sensor<ITouchSensor>, ITouchSensor
    {
        internal TouchSensor(IGrovePi device, Pin pin) : base(device, pin, PinMode.Input)
        {
        }
    }
}