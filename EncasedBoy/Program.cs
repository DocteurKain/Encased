namespace EncasedBoy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using EncasedLib.Services;
    using EncasedLib.Tools;
    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main()
        {
            // Doit être changer pour être utilisé !
            // To be changed to be used!

            /*
            
            var frFile = @"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\Fr.locale";

            //var ruLocale = FileService.FileToLocale(@"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\Ru.locale");
            //var enLocale = FileService.FileToLocale(@"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\En.locale");
            var frLocale = FileService.FileToLocale(@"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\Fr.locale_original");

            var list = EtfService.LoadEtf(etfFile);

            foreach(var line in frLocale.Lines)
            {
                var l = list.FirstOrDefault(a => a.Address == line.Address);
                if (l == null) continue;
                line.Text = l.Target;
            }

            FileService.LocaleToFile(frLocale, frFile);
            */

            var etfFile = @"E:\Dev\DotNet\Encased\_files\encased.etf";

            EtfService.GenerateLocaleFile(etfFile, @"d:\En.locale", true);
            EtfService.GenerateLocaleFile(etfFile, @"d:\Fr.locale");
        }

        private static void FoundMissingN()
        {
            var list = EtfService.LoadEtf(@"E:\Dev\DotNet\Encased\_files\encased.etf");
            var sb = new StringBuilder();
            foreach (var l in list)
            {
                var sourceN = Regex.Matches(l.Source, "\\n");
                var targetN = Regex.Matches(l.Target, "\\n");

                if (sourceN.Count > targetN.Count)
                    sb.AppendLine($"{l.Address}  {sourceN.Count}  {targetN.Count}");
            }
            File.WriteAllText(@"d:\encased_n.txt", sb.ToString());
        }

        private static void FoundMissingNr()
        {
            var list = EtfService.LoadEtf(@"E:\Dev\DotNet\Encased\_files\encased.etf");
            var sb = new StringBuilder();
            foreach (var l in list)
            {
                var sourceNr = Regex.Matches(l.Source, "</nr>");
                var targetNr = Regex.Matches(l.Target, "</nr>");

                if (sourceNr.Count != targetNr.Count)
                    sb.AppendLine($"{l.Address}  {sourceNr.Count}  {targetNr.Count}");
            }
            File.WriteAllText(@"d:\encased_nr.txt", sb.ToString());
        }

        private static void FoundMissingNrProblem()
        {
            var list = EtfService.LoadEtf(@"E:\Dev\DotNet\Encased\_files\encased.etf");
            var sb = new StringBuilder();
            foreach (var l in list)
            {
                var targetNrStart = Regex.Matches(l.Source, "<nr>");
                var targetNrStop = Regex.Matches(l.Target, "</nr>");

                if (targetNrStart.Count > targetNrStop.Count)
                    sb.AppendLine($"{l.Address}  {targetNrStart.Count}  {targetNrStop.Count}");
            }
            File.WriteAllText(@"d:\encased_nr_ss.txt", sb.ToString());
        }

        private static void DetectWasteTranslation()
        {
            var list = EtfService.LoadEtf(@"E:\Dev\DotNet\Encased\_files\encased.etf");
            var sb = new StringBuilder();
            foreach (var l in list)
            {
                var oLength = Convert.ToDouble(l.Source.Length);
                var tLength = Convert.ToDouble(l.Target.Length);

                var r = oLength / tLength;

                if (oLength > 15 & r > 2)
                    sb.AppendLine($"{l.Address}  {oLength}  {tLength}");

            }
            File.WriteAllText(@"d:\encased_short.txt", sb.ToString());
        }
    }
}