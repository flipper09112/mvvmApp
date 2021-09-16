using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Helpers
{
    public class ThermalPrinterFormatsHelper
    {
        /** The format that is being build on */
        private byte[] mFormat;

        public ThermalPrinterFormatsHelper()
        {
            // Default:
            mFormat = new byte[] { 27, 33, 0 };
        }

        /**
         * Method to get the Build result
         *
         * @return the format
         */
        public byte[] Get()
        {
            return mFormat;
        }

        public ThermalPrinterFormatsHelper Bold()
        {
            // Apply bold:
            mFormat[2] = ((byte)(0x8 | mFormat[2]));
            return this;
        }

        public ThermalPrinterFormatsHelper Small()
        {
            mFormat[2] = ((byte)(0x1 | mFormat[2]));
            return this;
        }

        public ThermalPrinterFormatsHelper Height()
        {
            mFormat[2] = ((byte)(0x10 | mFormat[2]));
            return this;
        }

        public ThermalPrinterFormatsHelper Width()
        {
            mFormat[2] = ((byte)(0x20 | mFormat[2]));
            return this;
        }

        public ThermalPrinterFormatsHelper Underlined()
        {
            mFormat[2] = ((byte)(0x80 | mFormat[2]));
            return this;
        }

        public static byte[] RightAlign()
        {
            return new byte[] { 0x1B, (byte)'a', 0x02 };
        }

        public static byte[] LeftAlign()
        {
            return new byte[] { 0x1B, (byte)'a', 0x00 };
        }

        public static byte[] CenterAlign()
        {
            return new byte[] { 0x1B, (byte)'a', 0x01 };
        }
    }
}
