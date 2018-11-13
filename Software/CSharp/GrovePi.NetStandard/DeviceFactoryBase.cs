﻿using GrovePi.I2CDevices;
using GrovePi.Sensors;
using System;
using System.Threading.Tasks;

namespace GrovePi
{
  
    public interface IBuildGroveDevices
    {
        IGrovePi GrovePi();
        IGrovePi GrovePi(int address);
        IRelay Relay(Pin pin);
        ILed Led(Pin pin);
        ITemperatureSensor TemperatureSensor(Pin pin);
        ITemperatureAndHumiditySensor TemperatureAndHumiditySensor(Pin pin, Model model);
        IDHTTemperatureAndHumiditySensor DHTTemperatureAndHumiditySensor(Pin pin, DHTModel model);
        IUltrasonicRangerSensor UltraSonicSensor(Pin pin);

        // AGP - removed untested sensors from this branch
        // IAccelerometerSensor AccelerometerSensor(Pin pin);
        // IAirQualitySensor AirQualitySensor(Pin pin);
        // IRealTimeClock RealTimeClock(Pin pin);
        // ILedBar BuildLedBar(Pin pin);
        // IFourDigitDisplay FourDigitDisplay(Pin pin);
        // IChainableRgbLed ChainableRgbLed(Pin pin);
        IRotaryAngleSensor RotaryAngleSensor(Pin pin);
        IBuzzer Buzzer(Pin pin);
        ISoundSensor SoundSensor(Pin pin);
        ILightSensor LightSensor(Pin pin);
        IButtonSensor ButtonSensor(Pin pin);
        ITouchSensor TouchSensor(Pin pin);
        IVibrationSensor VibrationSensor(Pin pin);
        IRgbLcdDisplay RgbLcdDisplay();
        IRgbLcdDisplay RgbLcdDisplay(int rgbAddress, int textAddress);
        
        // AGP - removed untested sensors from this branch
        // ISixAxisAccelerometerAndCompass SixAxisAccelerometerAndCompass();
        // IPIRMotionSensor PIRMotionSensor(Pin pin);
        // IGasSensorMQ2 GasSensorMQ2(Pin pin);
        // IMiniMotorDriver MiniMotorDriver();
        // IMiniMotorDriver MiniMotorDriver(int ch1Address1, int ch2Address2);
        // IOLEDDisplay9696 OLEDDisplay9696();
        // IOLEDDisplay128X64 OLEDDisplay128X64();
        // IThreeAxisAccelerometerADXL345 ThreeAxisAccelerometerADXL345();
        // IWaterAtomizer WaterAtomizer(Pin pin);
        // ISHTTemperatureAndHumiditySensor SHTTemperatureAndHumiditySensor();
    }

    public abstract class DeviceBuilderBase : IBuildGroveDevices
    {
      
        private const byte GrovePiAddress = 0x04;
        private const byte DisplayRgbI2CAddress = 0xC4;
        private const byte DisplayTextI2CAddress = 0x7C;
        private const byte SixAxisAccelerometerI2CAddress = 0x1e;
        private const byte MiniMotorDriverCH1I2cAddress = 0xC4;
        private const byte MiniMotorDriverCH2I2cAddress = 0xC0;
        private const byte OLED96_96I2cAddress = 0x3C;
        private const byte OLED128_64I2cAddress = 0x3C;
        private const byte ThreeAxisAccelemeterADXL345I2cAddress = 0x53;
        private const byte SHT31TemperatureAndHumidityI2CAddress = 0x44;
        private GrovePi _device;
        private RgbLcdDisplay _rgbLcdDisplay;

        // AGP - removed untested sensors from this branch
        // private SixAxisAccelerometerAndCompass _sixAxisAccelerometerAndCompass;
        // private MiniMotorDriver _miniMotorDriver;
        // private OLEDDisplay9696 _oledDisplay9696;
        // private OLEDDisplay128X64 _oledDisplay128X64;
        // private ThreeAxisAccelerometerADXL345 _ThreeAxisAccelerometerADXL345;
        // private SHTTemperatureAndHumiditySensor _shtTemperatureAndHumiditySensor;

        public IGrovePi GrovePi()
        {
            return BuildGrovePiImpl(GrovePiAddress);
        }

        public IGrovePi GrovePi(int address)
        {
            return BuildGrovePiImpl(address);
        }

        protected abstract I2cDevice GetI2cDevice(int address);

        public IRelay Relay(Pin pin)
        {
            return DoBuild(x => new Relay(x, pin));
        }

        public ILed Led(Pin pin)
        {
            return DoBuild(x => new Led(x, pin));
        }

        public ITemperatureSensor TemperatureSensor(Pin pin)
        {
            return DoBuild(x => new TemperatureSensor(x, pin));
        }

        public ITemperatureAndHumiditySensor TemperatureAndHumiditySensor(Pin pin, Model model)
        {
            return DoBuild(x => new TemperatureAndHumiditySensor(x, pin, model));
        }

        public IDHTTemperatureAndHumiditySensor DHTTemperatureAndHumiditySensor(Pin pin, DHTModel model)
        {
            return DoBuild(x => new DHTTemperatureAndHumiditySensor(x, pin, model));
        }

        // public IAirQualitySensor AirQualitySensor(Pin pin)
        // {
        //     return DoBuild(x => new AirQualitySensor(x, pin));
        // }

        public IUltrasonicRangerSensor UltraSonicSensor(Pin pin)
        {
            return DoBuild(x => new UltrasonicRangerSensor(x, pin));
        }

        // public IAccelerometerSensor AccelerometerSensor(Pin pin)
        // {
        //     return DoBuild(x => new AccelerometerSensor(x, pin));
        // }

        // public IRealTimeClock RealTimeClock(Pin pin)
        // {
        //     return DoBuild(x => new RealTimeClock(x, pin));
        // }

        public IRotaryAngleSensor RotaryAngleSensor(Pin pin)
        {
            return DoBuild(x => new RotaryAngleSensor(x, pin));
        }

        public IBuzzer Buzzer(Pin pin)
        {
            return DoBuild(x => new Buzzer(x, pin));
        }

        public ISoundSensor SoundSensor(Pin pin)
        {
            return DoBuild(x => new SoundSensor(x, pin));
        }

        // public ILedBar BuildLedBar(Pin pin)
        // {
        //     return DoBuild(x => new LedBar(x, pin));
        // }

        // public IFourDigitDisplay FourDigitDisplay(Pin pin)
        // {
        //     return DoBuild(x => new FourDigitDisplay(x, pin));
        // }

        // public IChainableRgbLed ChainableRgbLed(Pin pin)
        // {
        //     return DoBuild(x => new ChainableRgbLed(x, pin));
        // }

        public ILightSensor LightSensor(Pin pin)
        {
            return DoBuild(x => new LightSensor(x, pin));
        }

        public IVibrationSensor VibrationSensor(Pin pin)
        {
            return DoBuild(x => new VibrationSensor(x, pin));
        }

        public IRgbLcdDisplay RgbLcdDisplay(int rgbAddress, int textAddress)
        {
            return BuildRgbLcdDisplayImpl(rgbAddress, textAddress);
        }

        public IRgbLcdDisplay RgbLcdDisplay()
        {
            return BuildRgbLcdDisplayImpl(DisplayRgbI2CAddress, DisplayTextI2CAddress);
        }

        // public ISixAxisAccelerometerAndCompass SixAxisAccelerometerAndCompass()
        // {
        //     return BuildSixAxisAccelerometerAndCompassImpl();
        // }

        // public IMiniMotorDriver MiniMotorDriver()
        // {
        //     return BuildMiniMotorDriverImpl(MiniMotorDriverCH1I2cAddress, MiniMotorDriverCH2I2cAddress);
        // }

        // public IMiniMotorDriver MiniMotorDriver(int ch1Address, int ch2Address)
        // {
        //     return BuildMiniMotorDriverImpl(ch1Address, ch2Address);
        // }

        // public IOLEDDisplay9696 OLEDDisplay9696()
        // {
        //     return BuildOLEDDisplayImpl();
        // }

        // public IOLEDDisplay128X64 OLEDDisplay128X64()
        // {
        //     return BuildOLEDDisplay128X64Impl();
        // }

        // public IThreeAxisAccelerometerADXL345 ThreeAxisAccelerometerADXL345()
        // {
        //     return BuildThreeAxisAccelerometerADXL345Impl();
        // }

        public IButtonSensor ButtonSensor(Pin pin)
        {
            return DoBuild(x => new ButtonSensor(x, pin));
        }

        public ITouchSensor TouchSensor(Pin pin)
        {
            return DoBuild(x => new TouchSensor(x, pin));
        }

        private TSensor DoBuild<TSensor>(Func<GrovePi, TSensor> factory)
        {
            var device = BuildGrovePiImpl(GrovePiAddress);
            return factory(device);
        }

        private GrovePi BuildGrovePiImpl(int address)
        {
            if (_device != null)
            {
                return _device;
            }

            // _device = Task.Run(async () =>
            // {
                return new GrovePi(GetI2cDevice(address));
            // }).Result;
            // return _device;
        }
  
        private RgbLcdDisplay BuildRgbLcdDisplayImpl(int rgbAddress, int textAddress)
        {
            if (null != _rgbLcdDisplay)
            {
                return _rgbLcdDisplay;
            }
            
            _rgbLcdDisplay = Task.Run(async () =>
            {
                return new RgbLcdDisplay(GetI2cDevice(rgbAddress >> 1), GetI2cDevice(textAddress >> 1));
            }).Result;
            return _rgbLcdDisplay;
        }

        // private SixAxisAccelerometerAndCompass BuildSixAxisAccelerometerAndCompassImpl()
        // {
        //     if (_sixAxisAccelerometerAndCompass != null)
        //     {
        //         return _sixAxisAccelerometerAndCompass;
        //     }

         

        //     _sixAxisAccelerometerAndCompass = Task.Run(async () =>
        //     {
            
        //         return new SixAxisAccelerometerAndCompass(GetI2cDevice(SixAxisAccelerometerI2CAddress));
        //     }).Result;

        //     return _sixAxisAccelerometerAndCompass;
        // }

      

        // private MiniMotorDriver BuildMiniMotorDriverImpl(int ch1Address, int ch2Address)
        // {

        //     if (_miniMotorDriver != null)
        //     {
        //         return _miniMotorDriver;
        //     }

        //     _miniMotorDriver = Task.Run(async () =>
        //     {
        //         return new MiniMotorDriver(GetI2cDevice(ch1Address >> 1), GetI2cDevice(ch1Address >> 1));
        //     }).Result;
        //     return _miniMotorDriver;
        // }


        // private OLEDDisplay9696 BuildOLEDDisplayImpl()
        // {
        //     if(_oledDisplay9696 != null)
        //     {
        //         return _oledDisplay9696;
        //     }
        

        //     _oledDisplay9696 = Task.Run(async () =>
        //     {
            
        //         return new OLEDDisplay9696(GetI2cDevice(OLED96_96I2cAddress));
        //     }).Result;
        //     return _oledDisplay9696;
        // }

        // private OLEDDisplay128X64 BuildOLEDDisplay128X64Impl()
        // {
        //     if (_oledDisplay128X64 != null)
        //     {
        //         return _oledDisplay128X64;
        //     }
         

        //     _oledDisplay128X64 = Task.Run(async () =>
        //     {
              
        //         return new OLEDDisplay128X64(GetI2cDevice(OLED128_64I2cAddress));
        //     }).Result;
        //     return _oledDisplay128X64;
        // }

        // private ThreeAxisAccelerometerADXL345 BuildThreeAxisAccelerometerADXL345Impl()
        // {
        //     if (_ThreeAxisAccelerometerADXL345 != null)
        //     {
        //         return _ThreeAxisAccelerometerADXL345;
        //     }
          

        //     _ThreeAxisAccelerometerADXL345 = Task.Run(async () =>
        //     {
              
        //         return new ThreeAxisAccelerometerADXL345(GetI2cDevice(ThreeAxisAccelemeterADXL345I2cAddress));
        //     }).Result;
        //     return _ThreeAxisAccelerometerADXL345;
        // }

   
        // private SHTTemperatureAndHumiditySensor BuildSHTTemperatureAndHumiditySensorImpl()
        // {
        //     if (_shtTemperatureAndHumiditySensor != null)
        //     {
        //         return _shtTemperatureAndHumiditySensor;
        //     }

          
        //     _shtTemperatureAndHumiditySensor = Task.Run(async () => {
              
        //         return new SHTTemperatureAndHumiditySensor(GetI2cDevice(SHT31TemperatureAndHumidityI2CAddress), SHTModel.Sht31, MeasurementMode.MediumRepeat);
        //     }).Result;
        //     return _shtTemperatureAndHumiditySensor;
        // }

       

        // public IPIRMotionSensor PIRMotionSensor(Pin pin)
        // {
        //     return DoBuild(x => new PIRMotionSensor(x, pin));
        // }

        // public IGasSensorMQ2 GasSensorMQ2(Pin pin)
        // {
        //     return DoBuild(x => new GasSensorMQ2(x, pin));
        // }

        // public IWaterAtomizer WaterAtomizer(Pin pin)
        // {
        //     return DoBuild(x => new WaterAtomizer(x, pin));
        // }

        // public ISHTTemperatureAndHumiditySensor SHTTemperatureAndHumiditySensor()
        // {
        //     return BuildSHTTemperatureAndHumiditySensorImpl();
        // }
    }
}