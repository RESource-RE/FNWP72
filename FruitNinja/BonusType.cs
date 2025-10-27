// Decompiled with JetBrains decompiler
// Type: FruitNinja.BonusType
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FruitNinja
{

    internal class BonusType
    {
      private Dictionary<uint, int> totals = new Dictionary<uint, int>();
      private List<Bonus> bonuses = new List<Bonus>();

      public void Parse(XElement parent)
      {
        List<string> words = new List<string>();
        int num = StringFunctions.SplitWords(parent.AttributeStr("total"), ref words);
        for (int index = 0; index < num; ++index)
          this.totals[StringFunctions.StringHash(words[index])] = 0;
        Texture texture = StringFunctions.LoadTexture(parent.AttributeStr("texture"));
        for (XElement xelement = parent.FirstChildElement("bonus"); xelement != null; xelement = xelement.NextSiblingElement("bonus"))
        {
          Bonus bonus = new Bonus();
          bonus.Parse(xelement);
          if (bonus.texture == null)
            bonus.texture = texture;
          this.bonuses.Add(bonus);
        }
      }

      public Bonus GetBest()
      {
        int num = 0;
        int index1 = -1;
        int total1 = 0;
        Dictionary<uint, int> dictionary = new Dictionary<uint, int>();
        foreach (KeyValuePair<uint, int> total2 in this.totals)
        {
          int bonusTotal = BonusManager.GetBonusTotal(total2.Key);
          dictionary[total2.Key] = bonusTotal;
          total1 += bonusTotal;
        }
        this.totals = dictionary;
        for (int index2 = 0; index2 < this.bonuses.Count; ++index2)
        {
          int points = this.bonuses[index2].GetPoints();
          if (points > num && this.bonuses[index2].IsAchieved(total1, this.totals) != 0)
          {
            index1 = index2;
            num = points;
          }
        }
        return index1 < 0 ? (Bonus) null : this.bonuses[index1];
      }
    }
}
