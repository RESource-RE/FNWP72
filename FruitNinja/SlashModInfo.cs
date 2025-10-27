// Decompiled with JetBrains decompiler
// Type: FruitNinja.SlashModInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class SlashModInfo : ItemInfo
    {
      private Color[] colours;
      private int numColors;
      private int slashType;
      private float speed;
      private string particles;
      private string slashTexture;

      public override void SetEquipped()
      {
        SlashEntity.SetModColors(this.colours, this.numColors, this.slashType, this.speed, this.particles, this.slashTexture);
      }

      public override void Parse(XElement el)
      {
        base.Parse(el);
        this.numColors = 0;
        this.colours = (Color[]) null;
        this.slashType = 0;
        this.ParseSlashModInfo(el.FirstChildElement("slashModInfo"));
        if (this.colours != null)
          return;
        this.numColors = 1;
        this.colours = new Color[this.numColors];
        this.colours[0] = this.colour;
      }

      public void ParseSlashModInfo(XElement slashModInfo)
      {
        if (slashModInfo == null)
          return;
        slashModInfo.QueryFloatAttribute("speed", ref this.speed);
        this.slashType = SlashEntity.ParseSlashModColorType(slashModInfo.AttributeStr("type"));
        this.particles = slashModInfo.AttributeStr("particles");
        string str = slashModInfo.AttributeStr("texture");
        if (str != null)
          this.slashTexture = $"textureswp7/{str}.tex";
        for (XElement element = slashModInfo.FirstChildElement("colour"); element != null; element = element.NextSiblingElement("colour"))
          ++this.numColors;
        if (this.numColors <= 0)
          return;
        this.colours = new Color[this.numColors];
        int index = 0;
        for (XElement element = slashModInfo.FirstChildElement("colour"); element != null; element = element.NextSiblingElement("colour"))
        {
          StringFunctions.ParseColour(ref this.colours[index], element.Value);
          ++index;
        }
      }

      public SlashModInfo()
      {
        this.colours = (Color[]) null;
        this.numColors = 0;
        this.slashType = 0;
        this.speed = 1f;
        this.particles = (string) null;
        this.slashTexture = (string) null;
      }
    }
}
