// Decompiled with JetBrains decompiler
// Type: FruitNinja.GlobalProbabilityOveride
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FruitNinja
{

    internal class GlobalProbabilityOveride
    {
      public int chanceTotal;
      public List<GlobalProbabilityOveride.TypeChance> types = new List<GlobalProbabilityOveride.TypeChance>();
      public string totalName;
      public uint totalNameHash;
      public uint modeMask;
      public int minWait;
      public int maxWait;
      private bool canSpawnWithPowers;
      private int dontSpawnBeforeWave;

      public GlobalProbabilityOveride()
      {
        this.totalName = "";
        this.totalNameHash = 0U;
        this.modeMask = uint.MaxValue;
        this.minWait = 0;
        this.maxWait = 0;
        this.chanceTotal = 0;
        this.canSpawnWithPowers = true;
        this.dontSpawnBeforeWave = 0;
      }

      public virtual bool CanSpawn()
      {
        if (WaveManager.GetInstance().GetCurrentWave() != null && WaveManager.GetInstance().GetCurrentWave().gamesMin == 0)
          return true;
        if (WaveManager.GetInstance().GetNumberOfWavesSpawned() <= this.dontSpawnBeforeWave)
          return false;
        if (this.canSpawnWithPowers)
          return true;
        return Fruit.NumberOfPowerupFruits() <= 0 && (double) PowerUpManager.GetInstance().GetActiveProgression() >= 2.0;
      }

      public void Parse(XElement element)
      {
        this.totalName = element.AttributeStr("name");
        this.totalNameHash = StringFunctions.StringHash(this.totalName);
        this.modeMask = Game.ParseModeMask(element.AttributeStr("mode"));
        element.QueryIntAttribute("minWait", ref this.minWait);
        element.QueryIntAttribute("maxWait", ref this.maxWait);
        element.QueryIntAttribute("dontSpawnBeforeWave", ref this.dontSpawnBeforeWave);
        if (element.AttributeStr("canSpawnWithPowers") != null)
          this.canSpawnWithPowers = string.Compare("true", element.AttributeStr("canSpawnWithPowers")) == 0;
        this.chanceTotal = 0;
        for (XElement element1 = element.FirstChildElement("fruit"); element1 != null; element1 = element1.NextSiblingElement("fruit"))
        {
          GlobalProbabilityOveride.TypeChance typeChance = new GlobalProbabilityOveride.TypeChance();
          typeChance.type = element1.AttributeStr("type");
          element1.QueryIntAttribute("chance", ref typeChance.chance);
          this.chanceTotal += typeChance.chance;
          typeChance.chanceTotal = this.chanceTotal;
          this.types.Add(typeChance);
        }
        this.ParseSpecific(element);
      }

      public virtual void ParseSpecific(XElement element)
      {
      }

      public virtual bool CheckForOverride(ref int type)
      {
        if (((int) Game.GetModeBitMask(Game.game_work.gameMode) & (int) this.modeMask) != (int) this.modeMask)
          return false;
        int total = Game.game_work.saveData.GetTotal(this.totalNameHash);
        if (total == 0)
          Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, (int) TransitionFunctions.LerpF((float) this.minWait, (float) this.maxWait, Math.g_random.RandF(1f)), true, true);
        else if (total > 0)
        {
          if (total > 1)
            Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, -1, true, true);
          if (total == 1 && this.CanSpawn())
          {
            Game.game_work.saveData.AddToTotal(this.totalName, this.totalNameHash, -1, true, true);
            type = this.PickFruit();
            return true;
          }
        }
        return false;
      }

      public int PickFruit()
      {
        int num1 = Math.g_random.Rand32(this.chanceTotal);
        for (int index = 0; index < this.types.Count; ++index)
        {
          if (num1 < this.types[index].chanceTotal)
          {
            int num2 = Fruit.FruitType(this.types[index].type);
            if (num2 >= 0)
              return num2;
          }
        }
        return 0;
      }

      public void FruitWasKilled(Fruit fruit)
      {
        if (fruit == null || fruit.IsActive())
          return;
        this.PushbackSpawn();
      }

      public void FruitWasThrown(Fruit fruit)
      {
        if (fruit == null || this.CanSpawn())
          return;
        fruit.KillFruit();
        this.PushbackSpawn();
      }

      public virtual void PushbackSpawn() => Game.game_work.saveData.SetTotal(this.totalName, 3);

      public virtual void NewGameStarted()
      {
      }

      public class TypeChance
      {
        public string type;
        public int chance;
        public int chanceTotal;

        public TypeChance() => this.chance = 100;
      }
    }
}
