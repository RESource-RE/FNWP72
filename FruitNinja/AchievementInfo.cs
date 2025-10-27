// Decompiled with JetBrains decompiler
// Type: FruitNinja.AchievementInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    public class AchievementInfo
    {
      public string name;
      public string id;
      public uint idHash;
      public Texture texture;
      public string description;
      public int total;
      public int score;
      public bool isGameOver;
      public AchievementUnlockType type;
      public uint modeMask;
      public SpecificOrder specificOrder;

      public AchievementInfo()
      {
        this.texture = (Texture) null;
        this.specificOrder = (SpecificOrder) null;
        this.total = 0;
        this.score = 0;
        this.type = AchievementUnlockType.UNLOCK_TYPE_MAX;
      }
    }
}
