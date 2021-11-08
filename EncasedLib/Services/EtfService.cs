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

            var date = DateTime.Now.ToString("g");

            sb.AppendLine("##### Encased ETF File #####");
            sb.AppendLine($"##### {date} #####");

            foreach (var element in sourceLocale.Lines)
            {
                var t = targetLocale.Lines.FirstOrDefault(a => a.Address == element.Address);
                
                var targetText = String.Empty;

                if (t != null)
                    targetText = t.Text;

                sb.AppendLine($"## {element.Address} ## {element.Category} ##");
                sb.AppendLine(element.Text.ToOneLine());
                sb.AppendLine(targetText.ToOneLine());
            }

            File.WriteAllText(etfFile, sb.ToString(), Encoding.UTF8);
        }

        public static Boolean GenerateLocaleFile(String etfFile, String localeFile)
        {
            if (!File.Exists(etfFile))
                return false;

            var locale = new Locale();

            locale.Filename = Path.GetFileName(localeFile);
            locale.Header = HeaderTool.Get(locale.Filename);
            locale.Lines = LoadEtfToElements(etfFile);

            FileService.LocaleToFile(locale, localeFile);

            return true;
        }

        public static Boolean AddMissing(String etfFile, String inputLocale, String outputLocale)
        {
            if (!File.Exists(etfFile))
                return false;

            if (!File.Exists(inputLocale))
                return false;

            var locale = FileService.FileToLocale(inputLocale);

            var elements = LoadEtfToElements(etfFile);

            foreach(var element in elements)
            {
                var e = locale.Lines.FirstOrDefault(a => a.Address == element.Address);

                if(e == null)
                {
                    locale.Lines.Add(element);
                }
            }

            var o_elements = locale.Lines.OrderBy(a => a.Address);

            locale.Lines.Clear();
            locale.Lines.AddRange(o_elements);

            FileService.LocaleToFile(locale, outputLocale);

            return true;
        }

        /*
        public static void DetectMissingN(String etfFile, String etfOuputFile)
        {
            if (!File.Exists(etfFile))
                return;

            var list = LoadEtf(etfFile, false);

            var count = 0;

            foreach (var l in list)
            {
                var matchSource = Regex.Matches(l.Source, @"(?<crnn>\[n\]\[n\]\s{0,1}<nr>)");
                var targetSource = Regex.Matches(l.Target, @"(?<crnn>\[n\]\[n\]\s{0,1}<nr>)");
                var target2Source = Regex.Matches(l.Target, @"(?<cr><nr>)");

                if (targetSource.Count == matchSource.Count)
                    continue;

                if (matchSource.Count > 0)
                {
                    if(target2Source.Count == matchSource.Count)
                    {
                        count += matchSource.Count;

                        l.Target = l.Target.Replace("<nr>", "[n][n]<nr>");
                    }               
                
                }
            }

            Console.WriteLine($"count: {count}");

            SaveEtf(list, etfOuputFile, false);
        }
        
        public static void DetectMissingN2(String etfFile, String etfOuputFile)
        {
            var lines = EtfService.LoadEtf(etfFile, false);

            foreach (var line in lines)
            {
                var sourceSentences = line.Source.SplitInSentences();
                var targetSentences = line.Target.SplitInSentences();

                var sourceNcountMatch = Regex.Matches(line.Source, @"\[n\]");
                var targetNcountMatch = Regex.Matches(line.Target, @"\[n\]");

                if (sourceNcountMatch.Count == 0)
                    continue;

                if (sourceNcountMatch.Count == targetNcountMatch.Count)
                    continue;

                if (sourceSentences.Length != targetSentences.Length)
                    continue;

                var sb = new StringBuilder();

                var i = 0;

                foreach (var sourceSentence in sourceSentences)
                {
                    var newTargetSentence = targetSentences[i];

                    var sourceSentenceNcountMatch = Regex.Matches(sourceSentence, @"\[n\]");
                    var targetSentenceNcountMatch = Regex.Matches(newTargetSentence, @"\[n\]");

                    if (sourceSentenceNcountMatch.Count > 0)
                    {
                        if (sourceSentenceNcountMatch.Count > targetSentenceNcountMatch.Count)
                        {
                            // Starts with [n][n]
                            if (sourceSentence.StartsWith("[n][n]") && !newTargetSentence.StartsWith("[n][n]"))
                                newTargetSentence = "[n][n]" + newTargetSentence.TrimStart();

                            if (sourceSentence.StartsWith("[n]") && !newTargetSentence.StartsWith("[n]"))
                                newTargetSentence = "[n]" + newTargetSentence.TrimStart();

                            // Ends with [n][n]

                            // </nr>[n][n]
                            if (sourceSentence.Contains("</nr>[n][n]") && !newTargetSentence.Contains("</nr>[n][n]"))
                                newTargetSentence = newTargetSentence.Replace("</nr>", "</nr>[n][n]");

                            // [n][n]<nr>
                            if (sourceSentence.Contains("[n][n]<nr>") && !newTargetSentence.Contains("[n][n]<nr>"))
                                newTargetSentence = newTargetSentence.Replace("<nr>", "[n][n]<nr>");
                        }
                    }

                    sb.Append(newTargetSentence);
                    i++;
                }

                //
                line.Target = sb.ToString();
            }

            EtfService.SaveEtf(lines, etfOuputFile, false);
        }

        public static void DetectMissingN3(String etfFile, String debugFile)
        {
            if (!File.Exists(etfFile))
                return;

            var list = LoadEtf(etfFile, false);

            var count = 0;
            var enCount = 0;
            var frCount = 0;

            var sb = new StringBuilder();

            foreach (var l in list)
            {
                var enMatch = Regex.Matches(l.Source, @"\[n\]");
                var frMatch = Regex.Matches(l.Target, @"\[n\]");

                enCount += enMatch.Count;
                frCount += frMatch.Count;

                if (enMatch.Count != frMatch.Count)
                {
                    if (enMatch.Count - frMatch.Count > 0)
                    {
                        sb.AppendLine($"{l.Address} en:{enMatch.Count} fr:{frMatch.Count}");
                        count++;
                    }
                }
            }

            Console.WriteLine($"total : {count}");

            File.WriteAllText(debugFile, sb.ToString(), Encoding.UTF8);
        }

        public static void FinalDot(String etfFile, String etfOuputFile)
        {
            if (!File.Exists(etfFile))
                return;

            var lines = LoadEtf(etfFile, false);

            foreach (var line in lines)
            {
                if(line.Source.EndsWith(".") && !line.Target.EndsWith("."))
                {
                    if (line.Target.EndsWith("!"))
                        continue;
                    if (line.Target.EndsWith("?"))
                        continue;
                    if (line.Target.EndsWith("</nr>"))
                        continue;

                    line.Target = line.Target + ".";
                }
            }

            EtfService.SaveEtf(lines, etfOuputFile, false);
        }

        public static String CompareTwoEtfSource(String oldFile, String newFile)
        {
            if (!File.Exists(oldFile))
                return String.Empty;

            if (!File.Exists(newFile))
                return String.Empty;

            var sb = new StringBuilder();

            var oldList = LoadEtf(oldFile, false);
            var newList = LoadEtf(newFile, false);

            foreach(var nL in newList)
            {
                var oL = oldList.FirstOrDefault(a => a.Address == nL.Address);

                if(oL == null)
                {
                    sb.AppendLine($"# {nL.Address} # missing");
                    sb.AppendLine($"{nL.Source}");
                    continue;
                }

                var olWoSpace = oL.Source.Replace(" ", "");
                var nLWoSpace = nL.Source.Replace(" ", "");

                if (olWoSpace != nLWoSpace)
                {
                    sb.AppendLine($"# {nL.Address} # update");
                    sb.AppendLine($"{oL.Source}");
                    sb.AppendLine($"{nL.Source}");
                    continue;
                }
            }

            return sb.ToString();
        }

        public static String CountEmpty(String etfFile)
        {
            if (!File.Exists(etfFile))
                return String.Empty;

            var list = LoadEtf(etfFile);

            var sb = new StringBuilder();

            foreach (var l in list)
            {
                if (l.Source.Trim().Length == 0)
                    continue;

                if (l.Target.Trim().Length == 0)
                    sb.AppendLine($"# {l.Address}");
            }

            return sb.ToString();
        }
        */
        #endregion
        #region Private Methods
        public static List<EtfLine> LoadEtf(String etfFile, Boolean convertToMultiLine = true)
        {
            var list = new List<EtfLine>();

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

                        if (convertToMultiLine)
                        {
                            source = source.ToMultiLine();
                            target = target.ToMultiLine();
                        }

                        list.Add(new EtfLine(address, category, source, target));
                    }
                }
            }

            return list;
        }

        private static List<LocaleLine> LoadEtfToElements(String etfFile, Boolean addSourceIfMissing = true)
        {
            var list = new List<LocaleLine>();

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

                        list.Add(new LocaleLine(address, category, target));
                    }
                }
            }

            return list;
        }

        public static void SaveEtf(List<EtfLine> list, String etfFile, Boolean convertToOneLine = true)
        {
            var sb = new StringBuilder();

            sb.AppendLine("##### Encased ETF File #####");
            sb.AppendLine($"##### En / Fr #####"); // TODO

            foreach(var element in list)
            {
                sb.AppendLine($"## {element.Address} ## {element.Category} ##");

                if (convertToOneLine)
                {
                    sb.AppendLine(element.Source.ToOneLine());
                    sb.AppendLine(element.Target.ToOneLine());
                }
                else
                {
                    sb.AppendLine(element.Source);
                    sb.AppendLine(element.Target);
                }
            }

            File.WriteAllText(etfFile, sb.ToString(), Encoding.UTF8);
        }
        #endregion
    }
}