namespace EncasedLib.Tools
{
    using System;
    using System.Globalization;
    using System.IO;
    using Newtonsoft.Json;
    using Models;

    public static class LocaleTool
    {
        #region Json
        public static Locale LoadFromJson(String jsonFile)
        {
            if (!File.Exists(jsonFile))
                return null;

            var json = File.ReadAllText(jsonFile);

            return JsonConvert.DeserializeObject<Locale>(json);
        }

        public static void SaveFromJson(this Locale locale, String jsonFile, Formatting formatting = Formatting.None)
        {
            var json = JsonConvert.SerializeObject(locale, formatting);

            File.WriteAllText(jsonFile, json);
        }
        #endregion
        #region Calcul Length
        public static Int32 GetLength(String lengthHex)
        {
            if (lengthHex.Length < 2)
                return 0;

            var first = lengthHex.Substring(0, 2);

            if (first == "DA")
                return Int32.Parse(lengthHex.Substring(2, 4), NumberStyles.HexNumber);
            else if (first == "D9")
                return Int32.Parse(lengthHex.Substring(2, 2), NumberStyles.HexNumber);
            else
                return Int32.Parse(lengthHex.Substring(0, 2), NumberStyles.HexNumber) - 160;
        }

        public static Byte[] LengthToBytes(this Int32 value)
        {
            if (value < 32)
                value += 160;
            else if (value < 256)
                value += 55552;
            else
                value += 14286848;

            return value.IntToBytes();
        }
        #endregion
    }
}