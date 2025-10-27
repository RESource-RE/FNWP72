// Decompiled with JetBrains decompiler
// Type: FruitNinja.ExplodyFruitModifier
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    internal class ExplodyFruitModifier : GameModifier
    {
      private float m_radius;
      private float m_growTime;
      private float m_waitTime;
      private float m_fadeTime;
      private int m_comboType;
      private static uint[] hashes = new uint[3]
      {
        StringFunctions.StringHash("none"),
        StringFunctions.StringHash("cumulative"),
        StringFunctions.StringHash("total")
      };

      public ExplodyFruitModifier()
      {
        this.m_radius = 100f;
        this.m_waitTime = 0.0f;
        this.m_growTime = 0.25f;
        this.m_fadeTime = 0.2f;
        this.m_comboType = 0;
      }

      public override void ParseSpecific(XElement parent)
      {
        parent.QueryFloatAttribute("radius", ref this.m_radius);
        parent.QueryFloatAttribute("growTime", ref this.m_growTime);
        parent.QueryFloatAttribute("waitTime", ref this.m_waitTime);
        parent.QueryFloatAttribute("fadeTime", ref this.m_fadeTime);
        this.m_comboType = StringFunctions.FindIndex(parent.Attribute((XName) "comboType").Value, ExplodyFruitModifier.hashes, 3);
        this.m_waitTime += this.m_growTime;
        this.m_fadeTime += this.m_waitTime;
      }

      public override GameModifier Clone()
      {
        ExplodyFruitModifier explodyFruitModifier = new ExplodyFruitModifier();
        return (GameModifier) this;
      }

      public override void ApplyModifier(bool fromSave, float? length)
      {
        if ((double) this.m_currentTime <= 0.0)
          Fruit.FruitSliced += new Fruit.FruitSliceEvent(this.FruitWasSliced);
        base.ApplyModifier(fromSave, length);
      }

      public override void RemoveModifier()
      {
        Fruit.FruitSliced -= new Fruit.FruitSliceEvent(this.FruitWasSliced);
      }

      public override bool UpdateSpecific(float dt) => false;

      public override void ResetSpecific()
      {
      }

      private void FruitWasSliced(Fruit fruit, int score, Entity otherEnt)
      {
        if (Fruit.FruitInfo(fruit.GetFruitType()).superFruit)
          return;
        FruitSplosion control = new FruitSplosion(fruit, this.m_radius, this.m_growTime, this.m_waitTime, this.m_fadeTime, this.m_comboType);
        Game.game_work.hud.AddControl((HUDControl) control);
      }

      public override int GetType() => 5;

      public enum ComboType
      {
        COMBOTYPE_NONE,
        COMBOTYPE_CUMULATIVE,
        COMBOTYPE_TOTAL,
      }
    }
}
