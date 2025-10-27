// Decompiled with JetBrains decompiler
// Type: FruitNinja.GlobalProbabilityOveridePointBased
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    internal class GlobalProbabilityOveridePointBased : GlobalProbabilityOveride
    {
      private int everyMin;
      private int everyMax;
      private int fromMin;
      private int fromMax;

      public override bool CheckForOverride(ref int type)
      {
        if (((int) Game.GetModeBitMask(Game.game_work.gameMode) & (int) this.modeMask) != (int) this.modeMask)
          return false;
        int total = Game.game_work.saveData.GetTotal(this.totalNameHash);
        if (Game.GetCurrentScore() < total || !this.CanSpawn())
          return false;
        if (total < 0)
          Game.game_work.saveData.SetTotal(this.totalName, -total, false, false);
        type = this.PickFruit();
        Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, (int) Utils.GetRandBetween((float) this.everyMin, (float) this.everyMax), false, false);
        return true;
      }

      public override void ParseSpecific(XElement element)
      {
        element.QueryIntAttribute("every", ref this.everyMin);
        this.everyMax = this.everyMin;
        element.QueryIntAttribute("from", ref this.fromMin);
        this.fromMax = this.fromMin;
        element.QueryIntAttribute("everyMin", ref this.everyMin);
        element.QueryIntAttribute("everyMax", ref this.everyMax);
        element.QueryIntAttribute("fromMin", ref this.fromMin);
        element.QueryIntAttribute("fromMax", ref this.fromMax);
      }

      public override void PushbackSpawn()
      {
        int total = Game.game_work.saveData.GetTotal(this.totalNameHash);
        if (total <= 0)
          return;
        Game.game_work.saveData.SetTotal(this.totalName, -total, false, false);
      }

      public override void NewGameStarted()
      {
        Game.game_work.saveData.SetTotal(this.totalName, (int) Utils.GetRandBetween((float) this.fromMin, (float) this.fromMax), false, false);
      }
    }
}
