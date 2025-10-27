// Decompiled with JetBrains decompiler
// Type: Mortar.StringTable
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;

namespace Mortar
{

    public class StringTable
    {
      private string name;
      private int defaultLangauge;
      private string[] stringHashes;
      private string[] languageHashes;
      private StringTableLanguageStringSet[] languages;
      public static string[] langnames = new string[8]
      {
        "english_us",
        "english_uk",
        "french",
        "spanish",
        "german",
        "italian",
        "chinese",
        "traditional_chinese"
      };
      public static int[] hashes = new int[8]
      {
        StringTable.langnames[0].GetHashCode(),
        StringTable.langnames[1].GetHashCode(),
        StringTable.langnames[2].GetHashCode(),
        StringTable.langnames[3].GetHashCode(),
        StringTable.langnames[4].GetHashCode(),
        StringTable.langnames[5].GetHashCode(),
        StringTable.langnames[6].GetHashCode(),
        StringTable.langnames[7].GetHashCode()
      };

      public int GetStringIdx(string str) => Array.BinarySearch<string>(this.stringHashes, str);

      public int GetLanguageIdx(string lng)
      {
        int hashCode = lng.GetHashCode();
        int languageIdx = 0;
        foreach (int hash in StringTable.hashes)
        {
          if (hashCode == hash)
            return languageIdx;
          ++languageIdx;
        }
        return 0;
      }

      public string GetString(string str) => this.GetString(str, this.defaultLangauge);

      public string GetString(int sidx) => this.GetString(sidx, this.defaultLangauge);

      public string GetString(int sidx, string lng) => this.GetString(sidx, this.GetLanguageIdx(lng));

      public string GetString(string str, string lng) => this.GetString(str, this.GetLanguageIdx(lng));

      public string GetString(string str, int lidx) => this.GetString(this.GetStringIdx(str), lidx);

      public string GetString(int sidx, int lidx) => this.languages[lidx].strings[sidx];

      public void UpdateDefaultLanguage()
      {
        this.defaultLangauge = this.GetLanguageIdx(StringManager.GetInstance().defaultLanguage);
      }

      public void LoadHeader(string filename)
      {
        this.name = filename;
        string[][] strArray = TheGame.instance.Content.Load<string[][]>(filename + "_header.str");
        this.stringHashes = strArray[0];
        this.languageHashes = strArray[1];
        this.languages = new StringTableLanguageStringSet[this.languageHashes.Length];
        this.UpdateDefaultLanguage();
      }

      public void LoadLanguage(string lng) => this.LoadLanguage(this.GetLanguageIdx(lng));

      public void LoadLanguage(int lidx)
      {
        if (this.languages[lidx] != null)
          return;
        this.languages[lidx] = new StringTableLanguageStringSet();
        this.languages[lidx].strings = TheGame.instance.Content.Load<string[]>($"{this.name}_{StringTable.langnames[lidx]}.str");
        for (int index = 0; index < this.languages[lidx].strings.Length; ++index)
          this.languages[lidx].strings[index] = this.languages[lidx].strings[index].Replace("%i", "{0}");
      }

      public void UnloadLanguage(int lidx)
      {
        this.languages[lidx] = (StringTableLanguageStringSet) null;
      }

      public void LoadAllLanguages()
      {
        foreach (string languageHash in this.languageHashes)
          this.LoadLanguage(languageHash);
      }

      public void UnloadAllLanguages()
      {
        this.languages = new StringTableLanguageStringSet[this.languageHashes.Length];
      }
    }
}
