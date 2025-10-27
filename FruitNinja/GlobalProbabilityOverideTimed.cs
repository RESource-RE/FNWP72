// Decompiled with JetBrains decompiler
// Type: FruitNinja.GlobalProbabilityOverideTimed
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Xml.Linq;

namespace FruitNinja
{

    internal class GlobalProbabilityOverideTimed : GlobalProbabilityOveride
    {
      public override void ParseSpecific(XElement element)
      {
      }

      public override bool CheckForOverride(ref int type)
      {
        if (((int) Game.GetModeBitMask(Game.game_work.gameMode) & (int) this.modeMask) != (int) this.modeMask)
          return false;
        int total = Game.game_work.saveData.GetTotal(this.totalNameHash);
        if ((double) Game.game_work.gameTime <= (double) total || total < 0 || !this.CanSpawn())
          return false;
        type = this.PickFruit();
        Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, -this.maxWait * 2, false, false);
        return true;
      }

      public override void PushbackSpawn()
      {
        Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, this.maxWait * 2, false, false);
      }

      public override void NewGameStarted()
      {
        Game.game_work.saveData.SetTotal(this.totalName, (int) Utils.GetRandBetween((float) this.minWait, (float) this.maxWait), false, false);
      }
    }
}
