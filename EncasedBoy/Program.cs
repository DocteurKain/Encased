namespace EncasedBoy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using EncasedLib.Services;

    internal class Program
    {
        private static void Main()
        {
            // Doit être changer pour être utilisé !
            // To be changed to be used!

            /*EtfService.GenerateLocaleFile(
                @"E:\Dev\DotNet\Encased\_files\encased.etf",
                @"D:\Fr.locale"
            );*/

            var enLocale = FileService.FileToLocale(@"C:\Games\Encased_Localization\En_1.2.locale");
            var frLocale = FileService.FileToLocale(@"C:\Games\Encased_Localization\Fr_1.2.locale");

            EtfService.GenerateGlobalEtf(enLocale, frLocale, @"D:\Encased_1.2.etf");
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