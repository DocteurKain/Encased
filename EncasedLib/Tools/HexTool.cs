namespace EncasedLib.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class HexTool
    {
        public static Byte[] IntToBytes(this Int32 value)
        {
            var bValue = BitConverter.GetBytes(value);

            if (value < 256)
                return new Byte[] { bValue[0] };

            if (value < 65535)
                return new Byte[] { bValue[1], bValue[0] };

            if (value < 16777215)
                return new Byte[] { bValue[2], bValue[1], bValue[0] };

            return new Byte[] { bValue[3], bValue[2], bValue[1], bValue[0] };
        }

        public static UInt16 ByteToDec(this Byte[] value)
        {
            if (value == null) return 0;
            if (value.Length == 0) return 0;

            var bArray = new Byte[2] { 0x00, 0x00 };

            if (value.Length == 1)
                return BitConverter.ToUInt16(new Byte[2] { value[0], 0x00 }, 0);

            if (value.Length == 2)
                return BitConverter.ToUInt16(value.Reverse().ToArray(), 0);

            return 0;
        }

        public static String ByteToHex(this Byte value)
        {
            return Convert.ToString(value, 16).PadLeft(2, '0').ToUpper();
        }

        public static String BytesToHex(this Byte[] value, Boolean withSpace = false)
        {
            if (value == null) return "n/a";

            var sb = new StringBuilder(value.Length * 3);

            var stringL = 2;
            if (withSpace)
                stringL = 3;

            foreach (var b in value)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(stringL, ' '));

            return sb.ToString().ToUpper().Trim();
        }

        public static String BytesToHex(this List<Byte> value, Boolean withSpace = false)
        {
            return value.ToArray().BytesToHex(withSpace);
        }

        public static Byte[] HexToBytes(this String hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException($"The hexadecimal string have an odd number of digits: {0}", hexString);

            var data = new Byte[hexString.Length / 2];

            for (var index = 0; index < data.Length; index++)
            {
                var byteValue = hexString.Substring(index * 2, 2);

                data[index] = Byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}