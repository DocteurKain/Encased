namespace EncasedLib.Services
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Models;

    public static class JsonService
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
    }
}