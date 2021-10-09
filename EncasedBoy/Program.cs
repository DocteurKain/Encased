namespace EncasedBoy
{
    using System;
    using EncasedLib;
    using EncasedLib.Services;

    internal class Program
    {
        private static void Main(String[] args)
        {
            var file = @"C:\Users\michael\Desktop\a.fr.locale";

            LocaleService.LocaleToElements(file);



            Console.WriteLine("done!");
            Console.ReadKey(true);
        }
    }
}