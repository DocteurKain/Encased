﻿namespace EncasedLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Models;
    using Tools;

    public static class LocaleService
    {
        public static (Locale locale, String Message) FileToLocale(Language language, String localeFile)
        {
            var locale = new Locale
            {
                Filename = Path.GetFileName(localeFile),
                Language = language
            };

            if (!File.Exists(localeFile))
                return (locale, "File not found!");

            using(var fs = new FileStream(localeFile, FileMode.Open, FileAccess.Read))
            {
                // header
                var header = fs.ReadByte();
                if (header != 0xDE)
                    return (locale, "Bad header!");

                // first unknown bytes
                var headerLoop = true;
                while(headerLoop)
                {
                    var b = fs.ReadByte();
                    if (b == 0xCD)
                        headerLoop = false;
                    else
                        locale.Header.Add(Convert.ToByte(b));
                }

                // data bytes
                var dataLoop = true;
                var tmp = new List<Byte>() { 0xCD };

                while (dataLoop)
                {
                    var b = fs.ReadByte();

                    if(b == -1)
                    {
                        dataLoop = false;
                        continue;
                    }

                    tmp.Add(Convert.ToByte(b));

                    var tmpX = tmp.BytesToHex(false);

                    var match = Regex.Match(tmpX, @"(?<addr>(CD.{4}|CE.{8}))92(?<category>([0-9].{1}|CC.{2}|CD.{4}))(?<length>(DA.{4}|D9.{2}|[AB].{1}))");

                    if(match.Success)
                    {
                        tmp.Clear();

                        var address = match.Groups["addr"].Value.ToString();
                        var category = match.Groups["category"].Value.ToString();
                        var lengthHex = match.Groups["length"].Value.ToString();

                        var length = LocaleTool.GetLength(lengthHex);

                        var data = new List<Byte>();

                        for (var i = 0; i < length; i++)
                        {
                            var c = fs.ReadByte();
                            data.Add(Convert.ToByte(c));
                        }

                        //var dataHex = data.BytesToHex(false);
                        var text = Encoding.UTF8.GetString(data.ToArray());

                        locale.Elements.Add(new ElementStr(address, category, text));
                    }
                }
            }

            return (locale, String.Empty);
        }

        public static String LocaleToFile(Locale locale, String localeFile)
        {
            if (locale == null)
                return "Locale is null!";
            
            var bList = new List<Byte>();

            bList.Add(0xDE);
            bList.AddRange(locale.Header);

            foreach(var element in locale.Elements)
            {
                bList.AddRange(element.Address.HexToBytes());
                bList.Add(0x92);
                bList.AddRange(element.Category.HexToBytes());

                var bText = Encoding.UTF8.GetBytes(element.Text);
                var length = bText.Length;
                var lengthB = length.LengthToBytes();

                bList.AddRange(lengthB);
                bList.AddRange(bText);
            }

            File.WriteAllBytes(localeFile, bList.ToArray());

            return String.Empty;
        }

        public static void AddDiffToFile(Locale source, String diffFile)
        {
            if (!File.Exists(diffFile))
                return;


        }

        public static void DiffToFile(Locale source, Locale target, String diffFile)
        {
            var sb = new StringBuilder();

            foreach(var element in source.Elements)
            {
                var t = target.Elements.FirstOrDefault(a => a.Address == element.Address);

                if (t == null)
                {
                    sb.AppendLine($"##### {element.Address} #####");
                    sb.AppendLine(element.Text.ToOneLine());
                }
            }

            File.WriteAllText(diffFile, sb.ToString());
        }
    }
}