using System;
using GrovePi.Common;

namespace GrovePi.I2CDevices
{
    public interface IRgbLcdDisplay
    {
        IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue);
        IRgbLcdDisplay SetText(string text);
    }

    internal sealed class RgbLcdDisplay : IRgbLcdDisplay
    {
        private const byte RedCommandAddress = 4;
        private const byte GreenCommandAddress = 3;
        private const byte BlueCommandAddress = 2;
        private const byte TextCommandAddress = 0x80;
        private const byte ClearDisplayCommandAddress = 0x01;
        private const byte DisplayOnCommandAddress = 0x08;
        private const byte NoCursorCommandAddress = 0x04;
        private const byte TwoLinesCommandAddress = 0x28;
        private const byte SetCharacterCommandAddress = 0x40;

        internal RgbLcdDisplay(I2cDevice rgbDevice, I2cDevice textDevice)
        {
            if (rgbDevice == null) throw new ArgumentNullException(nameof(rgbDevice));
            if (textDevice == null) throw new ArgumentNullException(nameof(textDevice));

            RgbDirectAccess = rgbDevice;
            TextDirectAccess = textDevice;
        }

        internal I2cDevice RgbDirectAccess { get; }
        internal I2cDevice TextDirectAccess { get; }

        public IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue)
        {
            //TODO: Find out what these addresses are for , set const.
            RgbDirectAccess.WriteByteData(0, 0);
            RgbDirectAccess.WriteByteData(1, 0);
            RgbDirectAccess.WriteByteData(DisplayOnCommandAddress, 0xaa);
            RgbDirectAccess.WriteByteData(RedCommandAddress, red);
            RgbDirectAccess.WriteByteData(GreenCommandAddress, green);
            RgbDirectAccess.WriteByteData(BlueCommandAddress, blue);
            return this;
        }

        public IRgbLcdDisplay SetText(string text)
        {
            TextDirectAccess.WriteByteData(TextCommandAddress, ClearDisplayCommandAddress);
            Delay.Milliseconds(50);
            TextDirectAccess.WriteByteData(TextCommandAddress, (byte)(DisplayOnCommandAddress | NoCursorCommandAddress));
            TextDirectAccess.WriteByteData(TextCommandAddress, TwoLinesCommandAddress);

            var count = 0;
            var row = 0;

            foreach (var c in text)
            {
                if (c.Equals('\n') || count == Constants.GroveRgpLcdMaxLength)
                {
                    count = 0;
                    row += 1;
                    if (row == Constants.GroveRgpLcdRows)
                        break;
                    TextDirectAccess.WriteByteData(TextCommandAddress, 0xc0); //TODO: find out what this address is
                    if (c.Equals('\n'))
                        continue;
                }
                count += 1;
                TextDirectAccess.WriteByteData(SetCharacterCommandAddress, (byte) c);
            }

            return this;
        }
    }
}