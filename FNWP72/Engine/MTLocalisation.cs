// Decompiled with JetBrains decompiler
// Type: FNWP72.Engine.MTLocalisation
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using FruitNinja;
using Mortar;

namespace FNWP72.Engine
{

    public class MTLocalisation
    {
      private static bool bFontsLoaded;
      private static Font pCachedFont_WestEuropean;
      private static Font pCachedFont_EastAsian;

      public static bool FontsLoaded => MTLocalisation.bFontsLoaded;

      public static StringTableUtils.Language GetCurrentLanguage() => Game.game_work.language;

      public static string GetLocalisedTexturePath()
      {
        return MTLocalisation.GetLocalisedTexturePath(MTLocalisation.GetCurrentLanguage());
      }

      public static string GetLocalisedTexturePath(StringTableUtils.Language language)
      {
        string localisedTexturePath;
        switch (language)
        {
          case StringTableUtils.Language.LANGUAGE_ENGLISH:
          case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
            localisedTexturePath = "localisedwp7/en";
            break;
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            localisedTexturePath = "localisedwp7/fr";
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            localisedTexturePath = "localisedwp7/es";
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            localisedTexturePath = "localisedwp7/de";
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            localisedTexturePath = "localisedwp7/it";
            break;
          case StringTableUtils.Language.LANGUAGE_CHINESE_TRADITIONAL:
          case StringTableUtils.Language.LANGUAGE_CHINESE_SIMPLIFIED:
            localisedTexturePath = "localisedwp7/zh";
            break;
          default:
            localisedTexturePath = "localisedwp7/en";
            break;
        }
        return localisedTexturePath;
      }

      public static string GetLocalisedFontsPath()
      {
        return MTLocalisation.GetLocalisedFontsPath(MTLocalisation.GetCurrentLanguage());
      }

      public static string GetLocalisedFontsPath(StringTableUtils.Language language)
      {
        return MTLocalisation.GetLocalisedFontsPath(StringTableUtils.GetFontFamily(language));
      }

      public static string GetLocalisedFontsPath(StringTableUtils.FontFamily languageFontFamily)
      {
        string localisedFontsPath;
        switch (languageFontFamily)
        {
          case StringTableUtils.FontFamily.WestEuropean:
            localisedFontsPath = "fonts";
            break;
          case StringTableUtils.FontFamily.EastAsian:
            localisedFontsPath = "fontseastasian";
            break;
          default:
            localisedFontsPath = "fonts";
            break;
        }
        return localisedFontsPath;
      }

      public static void LoadFonts()
      {
        MTLocalisation.pCachedFont_EastAsian = MTLocalisation.LoadDefaultFont(StringTableUtils.FontFamily.EastAsian);
        MTLocalisation.pCachedFont_WestEuropean = MTLocalisation.LoadDefaultFont(StringTableUtils.FontFamily.WestEuropean);
        MTLocalisation.bFontsLoaded = true;
      }

      private static Font LoadDefaultFont(StringTableUtils.FontFamily fontFamily)
      {
        return MTLocalisation.LoadFont("font_fruit_ninja.fnt", fontFamily);
      }

      public static Font LoadFont(string fontFileName, StringTableUtils.FontFamily fontFamily)
      {
        Font font = new Font();
        font.Load($"{MTLocalisation.GetLocalisedFontsPath(fontFamily)}/{fontFileName}");
        return font;
      }

      public static Font GetCachedFont(StringTableUtils.Language language)
      {
        return MTLocalisation.GetCachedFont(StringTableUtils.GetFontFamily(language));
      }

      public static Font GetCachedFont(StringTableUtils.FontFamily fontFamily)
      {
        Font cachedFont;
        switch (fontFamily)
        {
          case StringTableUtils.FontFamily.WestEuropean:
            cachedFont = MTLocalisation.pCachedFont_WestEuropean;
            break;
          case StringTableUtils.FontFamily.EastAsian:
            cachedFont = MTLocalisation.pCachedFont_EastAsian;
            break;
          default:
            cachedFont = MTLocalisation.pCachedFont_WestEuropean;
            break;
        }
        return cachedFont;
      }
    }
}
