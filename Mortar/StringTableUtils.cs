// Decompiled with JetBrains decompiler
// Type: Mortar.StringTableUtils
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class StringTableUtils
    {
      private const int NUM_STRING_TABLES = 2;

      public static StringTableUtils.FontFamily GetFontFamily(StringTableUtils.Language language)
      {
        StringTableUtils.FontFamily fontFamily;
        switch (language)
        {
          case StringTableUtils.Language.LANGUAGE_ENGLISH:
          case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
          case StringTableUtils.Language.LANGUAGE_FRENCH:
          case StringTableUtils.Language.LANGUAGE_SPANISH:
          case StringTableUtils.Language.LANGUAGE_GERMAN:
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
          case StringTableUtils.Language.LANGUAGE_DUTCH:
          case StringTableUtils.Language.LANGUAGE_SWEDISH:
          case StringTableUtils.Language.LANGUAGE_DANISH:
          case StringTableUtils.Language.LANGUAGE_NORWEGIAN:
          case StringTableUtils.Language.LANGUAGE_FINNISH:
            fontFamily = StringTableUtils.FontFamily.WestEuropean;
            break;
          case StringTableUtils.Language.LANGUAGE_KOREAN:
          case StringTableUtils.Language.LANGUAGE_JAPANESE:
          case StringTableUtils.Language.LANGUAGE_CHINESE_TRADITIONAL:
          case StringTableUtils.Language.LANGUAGE_CHINESE_SIMPLIFIED:
            fontFamily = StringTableUtils.FontFamily.EastAsian;
            break;
          default:
            fontFamily = StringTableUtils.FontFamily.WestEuropean;
            break;
        }
        return fontFamily;
      }

      public static void StringTableUtilInit()
      {
      }

      public static void StringTableUtilLoadStrings()
      {
      }

      public static void StringTableUtilLoadStringsTable(int i)
      {
      }

      public static void StringTableUtilUnload()
      {
      }

      public static void StringTableUtilUnloadTable(int i)
      {
      }

      public static bool StringTableUtilLoaded() => false;

      internal static StringTableUtils.Language ParseLanguageFromCulture(string cultureName)
      {
        StringTableUtils.Language languageFromCulture;
        switch (cultureName)
        {
          case "en":
          case "en-AU":
          case "en-BZ":
          case "en-CA":
          case "en-CB":
          case "en-IE":
          case "en-JM":
          case "en-NZ":
          case "en-PH":
          case "en-ZA":
          case "en-TT":
          case "en-GB":
          case "en-ZW":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_ENGLISH_UK;
            break;
          case "en-US":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_ENGLISH;
            break;
          case "fr":
          case "fr-BE":
          case "fr-CA":
          case "fr-FR":
          case "fr-LU":
          case "fr-MC":
          case "fr-CH":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_FRENCH;
            break;
          case "de":
          case "de-AT":
          case "de-DE":
          case "de-LI":
          case "de-LU":
          case "de-CH":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_GERMAN;
            break;
          case "it":
          case "it-IT":
          case "it-CH":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_ITALIAN;
            break;
          case "es":
          case "es-AR":
          case "es-BO":
          case "es-CL":
          case "es-CO":
          case "es-CR":
          case "es-DO":
          case "es-EC":
          case "es-SV":
          case "es-GT":
          case "es-HN":
          case "es-MX":
          case "es-NI":
          case "es-PA":
          case "es-PY":
          case "es-PE":
          case "es-PR":
          case "es-ES":
          case "es-UY":
          case "es-VE":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_SPANISH;
            break;
          case "ko-KR":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_ENGLISH;
            break;
          case "zh-CN":
            languageFromCulture = StringTableUtils.Language.LANGUAGE_CHINESE_SIMPLIFIED;
            break;
          default:
            languageFromCulture = StringTableUtils.Language.LANGUAGE_ENGLISH;
            break;
        }
        return languageFromCulture;
      }

      public enum Table
      {
        TABLE_COMMON,
        TABLE_LEVEL,
        TABLE_MAX,
      }

      public enum Language
      {
        LANGUAGE_ENGLISH,
        LANGUAGE_ENGLISH_UK,
        LANGUAGE_FRENCH,
        LANGUAGE_SPANISH,
        LANGUAGE_GERMAN,
        LANGUAGE_ITALIAN,
        LANGUAGE_DUTCH,
        LANGUAGE_SWEDISH,
        LANGUAGE_DANISH,
        LANGUAGE_NORWEGIAN,
        LANGUAGE_FINNISH,
        LANGUAGE_KOREAN,
        LANGUAGE_JAPANESE,
        LANGUAGE_CHINESE_TRADITIONAL,
        LANGUAGE_CHINESE_SIMPLIFIED,
        LANGUAGE_MAX,
      }

      public enum LM
      {
        LEVEL_MENU,
        CHAPTER_TUTORIAL,
        CHAPTER_1,
        CHAPTER_2,
        CHAPTER_3,
        CHAPTER_4,
        CHAPTER_5,
        CHAPTER_6A,
        CHAPTER_6B,
        LEVEL_CREDITS,
      }

      public enum FontFamily
      {
        WestEuropean,
        EastAsian,
      }
    }
}
