namespace EncasedLib.Tools
{
    using System;
    using Models;

    public static class LanguageTool
    {
        public static String GetFileName(Language language)
        {
            switch (language)
            {
                case Language.En:
                    return "En.locale";
                case Language.Fr:
                    return "Fr.locale";
                case Language.De:
                    return "De.locale";
                case Language.Es:
                    return "Es.locale";
                default:
                    return String.Empty;
            }
        }

        public static String GetXLiffCode(Language language)
        {
            switch (language)
            {
                case Language.En:
                    return "en-US";
                case Language.Fr:
                    return "fr-FR";
                case Language.De:
                    return "de-DE";
                case Language.Es:
                    return "es-ES";
                default:
                    return String.Empty;
            }
        }
    }
}