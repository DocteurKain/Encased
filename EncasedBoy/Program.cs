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
        private static void Main()
        {
            EtfService.ImportAll(
                @"E:\Dev\DotNet\Encased\_files\encased.etf",
                @"d:\source_fr.locale",
                @"d:\Fr.locale");

            /*var (enLoc, enMessage) = LocaleService.FileToLocale(Language.En, @"D:\En.Locale");
            var (frLoc, frMessage) = LocaleService.FileToLocale(Language.Fr, @"D:\Fr.Locale");

            EtfService.GenerateGlobalEtf(enLoc, frLoc, @"d:\_t.etf");^*/

            Console.WriteLine("done!");
            Console.ReadKey(true);
        }
    }
}