namespace EncasedLib.Tools
{
    using System;
    using System.Linq;
    using System.Text;

    public static class HexTool
    {
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
    }
}