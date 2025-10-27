// Decompiled with JetBrains decompiler
// Type: FruitNinja.PROBABILITY_OVERIDE
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FruitNinja
{

    public class PROBABILITY_OVERIDE
    {
      public int percentageChance;
      public int perWave;
      public int spawnedThisWave;
      public int typeCount;
      public List<string> typeNames = new List<string>();
      public int[] types = new int[WaveManager.MAX_PROBABILITY_OVERIDE_TYPES];
      public float canSpawnWithPowerup;
      public int waveCount;
      public int numWaves;
      public List<int> powerAllowanceChances = new List<int>();
      private static uint[] bombHashes = new uint[2]
      {
        StringFunctions.StringHash("bomb"),
        StringFunctions.StringHash("Bomb")
      };
      private static uint one_fruit = StringFunctions.StringHash("1fruit");

      public PROBABILITY_OVERIDE()
      {
        this.waveCount = -1000000;
        this.numWaves = -1;
        this.percentageChance = 0;
        this.perWave = 0;
        this.spawnedThisWave = 0;
        this.typeCount = 0;
        for (int index = 0; index < WaveManager.MAX_PROBABILITY_OVERIDE_TYPES; ++index)
          this.types[index] = -1;
        this.canSpawnWithPowerup = 0.0f;
      }

      public void Parse(XElement element)
      {
        element.QueryIntAttribute("percentageChance", ref this.percentageChance);
        element.QueryIntAttribute("waveCount", ref this.waveCount);
        this.typeCount = StringFunctions.SplitWords(element.AttributeStr("types"), ref this.typeNames);
        element.QueryIntAttribute("perWave", ref this.perWave);
        element.QueryFloatAttribute("disableWhenPowered", ref this.canSpawnWithPowerup);
        element.QueryIntAttribute("numWaves", ref this.numWaves);
        for (XElement element1 = element.FirstChildElement("PowerAllowance"); element1 != null; element1 = element1.NextSiblingElement("PowerAllowance"))
        {
          int num = 0;
          element1.QueryIntAttribute("allowPercentage", ref num);
          this.powerAllowanceChances.Add(num);
        }
      }

      public void SelectType()
      {
        for (int index = 0; index < this.typeCount; ++index)
        {
          uint num = StringFunctions.StringHash(this.typeNames[index]);
          this.types[index] = (int) num == (int) PROBABILITY_OVERIDE.bombHashes[0] || (int) num == (int) PROBABILITY_OVERIDE.bombHashes[1] ? -2 : ((int) num != (int) PROBABILITY_OVERIDE.one_fruit ? Fruit.FruitType(this.typeNames[index]) : Fruit.RandomFruit(false));
        }
      }

      public virtual int GetType()
      {
        return this.types[WaveManager.GetInstance().GetRandomiser().Rand32(this.typeCount)];
      }
    }
}
