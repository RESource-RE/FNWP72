// Decompiled with JetBrains decompiler
// Type: FruitNinja.SpecificOrder
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace FruitNinja
{

    public class SpecificOrder
    {
      private uint[,] orderList = new uint[AchievementManager.MAX_ORDER_LIST_TYPES, AchievementManager.MAX_ORDER_LIST_TYPES + 1];

      public SpecificOrder(string text)
      {
      }

      public bool Check(uint hash) => false;

      public uint GetFirstFruitTypeHash() => 0;
    }
}
