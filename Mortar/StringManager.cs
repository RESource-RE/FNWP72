// Decompiled with JetBrains decompiler
// Type: Mortar.StringManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    internal class StringManager
    {
      public static StringTable[] tables;
      public string defaultLanguage;
      public static StringManager instance;

      public static StringManager GetInstance()
      {
        if (StringManager.instance == null)
          StringManager.instance = new StringManager();
        return StringManager.instance;
      }

      private StringManager() => this.defaultLanguage = "english_us";

      public void Init(int numTables) => StringManager.tables = new StringTable[numTables];

      public void LoadTable(string filename, int idx)
      {
        StringManager.tables[idx] = new StringTable();
        StringManager.tables[idx].LoadHeader(filename);
      }

      public void UnloadTable(int idx) => StringManager.tables[idx] = (StringTable) null;

      public void UnloadAll() => StringManager.tables = new StringTable[StringManager.tables.Length];

      public void SetDefaultLanguage(string lng) => this.defaultLanguage = lng;
    }
}
