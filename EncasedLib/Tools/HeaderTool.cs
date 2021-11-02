namespace EncasedLib.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Models;
    using Services;

    public static class HeaderTool
    {
        public static List<Byte> Get(String filename)
        {
            if (!File.Exists(Constants.HeadersFile))
                return null;

            var json = File.ReadAllText(Constants.HeadersFile);

            var headerList = JsonConvert.DeserializeObject<List<Header>>(json);

            var header = headerList.FirstOrDefault(a => a.Filename == Path.GetFileName(filename));

            if (header == null)
                return null;

            return header.Data;
        }

        private static void GenerateHeader(String encasedPath)
        {
            var locs = new String[] { "De", "En", "Es", "Fr", "Jp", "Ru", "zho_CH", "zho_TW" };
            var locPath = Path.Combine(encasedPath, @"Encased_Data\StreamingAssets\Localization");
            var headerList = new List<Header>();

            foreach (var loc in locs)
            {
                var locFile = Path.Combine(locPath, $"{loc}.locale");

                if (!File.Exists(locFile))
                    continue;

                var locData = FileService.FileToLocale(locFile);

                headerList.Add(new Header(Path.GetFileName(locFile), locData.Header));
            }

            var json = JsonConvert.SerializeObject(headerList, Formatting.None);

            File.WriteAllText(Constants.HeadersFile, json);
        }
    }
}