// Decompiled with JetBrains decompiler
// Type: FruitNinja.FNHighscoreList
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace FruitNinja
{

    internal class FNHighscoreList
    {
      private bool m_reorderRanks;
      private bool m_allowDuplicateUsers;

      private bool AddScore(string user, int score, int rank) => false;

      public void ClearScores(int mode, int type)
      {
      }

      public bool GetHighscoreForUser(
        string user,
        FNHighscore userScore,
        FNHighscore before,
        FNHighscore after)
      {
        return false;
      }

      public void PrepareForDataRetrieval()
      {
      }

      public void SetReorderRanks(bool reorderRanks) => this.m_reorderRanks = reorderRanks;

      public bool GetReorderRanks() => this.m_reorderRanks;

      public void SetAllowDuplicateUsers(bool allowDuplicates)
      {
        this.m_allowDuplicateUsers = allowDuplicates;
      }

      public bool AllowDuplicateUsers() => this.m_allowDuplicateUsers;
    }
}
