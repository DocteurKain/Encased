namespace EncasedLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Tools;

    public static class LocaleService
    {
        public static List<Element> LocaleToElements(String localeFile)
        {
            var list = new List<Element>();

            if (!File.Exists(localeFile))
                return list;

            var bArray = File.ReadAllBytes(localeFile);

            foreach (var b in bArray)
            {

            }

            return list;
        }

        public static Boolean ElementsToLocale(List<Element> elements, String localeFile)
        {
            if (elements == null)
                return false;



            return true;
        }
    }
}