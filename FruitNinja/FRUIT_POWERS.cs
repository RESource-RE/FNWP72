// Decompiled with JetBrains decompiler
// Type: FruitNinja.FRUIT_POWERS
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    public class FRUIT_POWERS
    {
      public FRUIT_POWER[] powerUps;
      public int numPowerUpTypes;

      public uint RandomPower()
      {
        int num = Math.g_random.Rand32(this.powerUps[this.numPowerUpTypes - 1].totalChance);
        for (int index = 0; index < this.numPowerUpTypes; ++index)
        {
          if (num < this.powerUps[index].totalChance)
            return this.powerUps[index].powerHash;
        }
        return this.powerUps[0].powerHash;
      }

      public bool AnyActivePowers()
      {
        for (int index = 0; index < this.numPowerUpTypes; ++index)
        {
          if (PowerUpManager.GetInstance().GetActiveSingle(this.powerUps[index].powerHash) != null)
            return true;
        }
        return false;
      }

      public FRUIT_POWERS() => this.numPowerUpTypes = 0;
    }
}
