// Decompiled with JetBrains decompiler
// Type: FruitNinja.BonusAwardHud
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class BonusAwardHud
    {
      public string text;
      public int points;
      public int multiplyer;
      public int count;
      public int visiblePoints;
      public Color colour;
      public float numberScale;
      public Color numberColour;
      public Texture texture;

      public BonusAwardHud()
      {
        this.points = 0;
        this.multiplyer = 1;
        this.text = "";
        this.colour = Color.White;
        this.visiblePoints = 0;
        this.texture = (Texture) null;
      }
    }
}
