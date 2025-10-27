// Decompiled with JetBrains decompiler
// Type: FruitNinja.MetricData
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    public class MetricData
    {
      public int highScore;
      public int totalTotals;
      public SliceTotal[] totals = ArrayInit.CreateFilledArray<SliceTotal>(5);
      public int AchievementsUnlocked;
      public uint[] Achievements = new uint[20];
    }
}
