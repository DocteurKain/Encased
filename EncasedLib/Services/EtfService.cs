namespace EncasedLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using EncasedLib.Tools;
    using Models;

    public static class EtfService
    {
        #region Public Methods
        // Etf file
        // Source and Target language
        public static void GenerateGlobalEtf(Locale sourceLocale, Locale targetLocale, String etfFile)
        {
            var sb = new StringBuilder();

            var sourceLng = sourceLocale.Language;
            var targetLng = targetLocale.Language;

            sb.AppendLine("##### Encased ETF File [GBL] #####");
            sb.AppendLine($"##### {sourceLng} / {targetLng} #####");

            foreach (var element in sourceLocale.Elements)
            {
                var t = targetLocale.Elements.FirstOrDefault(a => a.Address == element.Address);
                
                var targetText = String.Empty;

                if (t != null)
                    targetText = t.Text;

                sb.AppendLine($"## {element.Address} ## {element.Category} ##");
                sb.AppendLine(element.Text.ToOneLine());
                sb.AppendLine(targetText.ToOneLine());
            }

            File.WriteAllText(etfFile, sb.ToString(), Encoding.UTF8);
        }

        public static void GenerateMissingEtf(Locale sourceLocale, Locale targetLocale, String etfFile)
        {
            var sb = new StringBuilder();

            var sourceLng = sourceLocale.Language;
            var targetLng = targetLocale.Language;

            sb.AppendLine("##### Encased ETF File [MSS] #####");
            sb.AppendLine($"##### {sourceLng} / {targetLng} #####");

            foreach (var element in sourceLocale.Elements)
            {
                var t = targetLocale.Elements.FirstOrDefault(a => a.Address == element.Address);

                if (t == null)
                {
                    sb.AppendLine($"## {element.Address} ## {element.Category} ##");
                    sb.AppendLine(element.Text.ToOneLine());
                    sb.AppendLine(String.Empty);
                }
            }

            File.WriteAllText(etfFile, sb.ToString(), Encoding.UTF8);
        }

        public static Boolean ImportAll(String etfFile, String inputLocale, String outputLocale)
        {
            if (!File.Exists(etfFile))
                return false;

            if (!File.Exists(inputLocale))
                return false;

            var (locale, message) = LocaleService.FileToLocale(Language.Unknown, inputLocale);

            locale.Elements.Clear();
            locale.Elements = LoadEtf(etfFile);

            LocaleService.LocaleToFile(locale, outputLocale);

            return true;
        }

        public static Boolean AddMissing(String etfFile, String inputLocale, String outputLocale)
        {
            if (!File.Exists(etfFile))
                return false;

            if (!File.Exists(inputLocale))
                return false;

            var (locale, message) = LocaleService.FileToLocale(Language.Unknown, inputLocale);

            var elements = LoadEtf(etfFile);

            foreach(var element in elements)
            {
                var e = locale.Elements.FirstOrDefault(a => a.Address == element.Address);

                if(e == null)
                {
                    locale.Elements.Add(element);
                }
            }

            var o_elements = locale.Elements.OrderBy(a => a.Address);

            locale.Elements.Clear();
            locale.Elements.AddRange(o_elements);

            LocaleService.LocaleToFile(locale, outputLocale);

            return true;
        }
        #endregion
        #region Private Methods
        private static List<Element> LoadEtf(String etfFile, Boolean addSourceIfMissing = true)
        {
            var list = new List<Element>();

            if (!File.Exists(etfFile))
                return list;

            using (var fs = File.OpenRead(etfFile))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                var header_a = sr.ReadLine();
                var header_b = sr.ReadLine();

                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    var match = Regex.Match(line, @"## (?<address>C[DE].{4,8}) ## (?<category>.{2,8}) ##");

                    if (match.Success)
                    {
                        var address = match.Groups["address"].Value.ToString();
                        var category = match.Groups["category"].Value.ToString();
                        var source = sr.ReadLine();
                        var target = sr.ReadLine();

                        if (addSourceIfMissing && target.Length == 0)
                            target = source;

                        target = target.ToMultiLine();

                        list.Add(new Element(address, category, target));
                    }
                }
            }

            return list;
        }
        #endregion
    }
}