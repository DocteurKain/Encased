namespace EncasedBoy
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using EncasedLib;
    using EncasedLib.Models;
    using EncasedLib.Services;
    using EncasedLib.Tools;

    internal class Program
    {
        private static void Main(String[] args)
        {
            var path = @"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\";

            var frFile = "Fr.locale";

            var (frLocale, frMessage) = LocaleService.FileToLocale(Language.Fr, Path.Combine(path, frFile));

            frLocale.SaveFromJson(@"D:\_fr_complete.json", Formatting.Indented);

            //var locale2 = LocaleTool.LoadFromJson(@"d:\_test_fr.json");
            //LocaleService.LocaleToFile(locale2, @"d:\_test_fr_new.locale");

            //LocaleService.DiffToFile(enLocale, frLocale, @"d:\diff.txt");


            //var enLoc = LocaleTool.LoadFromJson(@"d:\_en_complete.json");
            //var frLoc = LocaleTool.LoadFromJson(@"d:\_fr_complete.json");

            //LocaleService.DiffToFile(enLoc, frLoc, @"d:\diff.txt");

            LocaleService.LocaleToFile(frLocale, @"d:\Fr.locale");

            Console.WriteLine("done!");
            //Console.ReadKey(true);
        }
    }
}