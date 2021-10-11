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
            EtfService.ImportAll(
                @"E:\Dev\DotNet\Encased\_files\encased_gbl.etf",
                @"C:\Games\Encased\Encased_Data\StreamingAssets\Localization\Fr.locale",
                @"D:\Fr.locale");


            Console.WriteLine("done!");
            //Console.ReadKey(true);
        }
    }
}