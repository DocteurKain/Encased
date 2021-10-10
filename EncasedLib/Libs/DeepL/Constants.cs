namespace EncasedLib.Libs.DeepL
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public static class Constants
    {
        public static String ProApiBaseUrl => "https://api.deepl.com/v2";
        public static String FreeApiBaseUrl => "https://api-free.deepl.com/v2";

        public static String UsageStatisticsPath => "usage";
        public static String TranslatePath => "translate";
        public static String TranslateDocumentPath => "document";
        public static String SupportedLanguagesPath => "languages";
        public static String GlossariesPath => "glossaries";

        public static Dictionary<Language, String> SourceLanguageCodeConversionMap => new Dictionary<Language, String>
        {
            [Language.German] = "DE",
            [Language.English] = "EN",
            [Language.BritishEnglish] = "EN", // Region-specific variants are actually not supported, but are here to prevent errors
            [Language.AmericanEnglish] = "EN", // Region-specific variants are actually not supported, but are here to prevent errors
            [Language.French] = "FR",
            [Language.Italian] = "IT",
            [Language.Japanese] = "JA",
            [Language.Spanish] = "ES",
            [Language.Dutch] = "NL",
            [Language.Polish] = "PL",
            [Language.Portuguese] = "PT",
            [Language.BrazilianPortuguese] = "PT", // Region-specific variants are actually not supported, but are here to prevent errors
            [Language.Russian] = "RU",
            [Language.Chinese] = "ZH",
            [Language.Bulgarian] = "BG",
            [Language.Czech] = "CS",
            [Language.Danish] = "DA",
            [Language.Greek] = "EL",
            [Language.Estonian] = "ET",
            [Language.Finnish] = "FI",
            [Language.Hungarian] = "HU",
            [Language.Lithuanian] = "LT",
            [Language.Latvian] = "LV",
            [Language.Romanian] = "RO",
            [Language.Slovak] = "SK",
            [Language.Slovenian] = "SL",
            [Language.Swedish] = "SV"
        };

        public static Dictionary<Language, String> TargetLanguageCodeConversionMap => new Dictionary<Language, string>
        {
            [Language.German] = "DE",
            [Language.BritishEnglish] = "EN-GB",
            [Language.AmericanEnglish] = "EN-US",
            [Language.English] = "EN", // Unspecified variant for backward compatibility; please select EN-GB or EN-US instead
            [Language.French] = "FR",
            [Language.Italian] = "IT",
            [Language.Japanese] = "JA",
            [Language.Spanish] = "ES",
            [Language.Dutch] = "NL",
            [Language.Polish] = "PL",
            [Language.Portuguese] = "PT-PT",
            [Language.BrazilianPortuguese] = "PT-BR",
            [Language.Russian] = "RU",
            [Language.Chinese] = "ZH",
            [Language.Bulgarian] = "BG",
            [Language.Czech] = "CS",
            [Language.Danish] = "DA",
            [Language.Greek] = "EL",
            [Language.Estonian] = "ET",
            [Language.Finnish] = "FI",
            [Language.Hungarian] = "HU",
            [Language.Lithuanian] = "LT",
            [Language.Latvian] = "LV",
            [Language.Romanian] = "RO",
            [Language.Slovak] = "SK",
            [Language.Slovenian] = "SL",
            [Language.Swedish] = "SV"
        };
    }
}