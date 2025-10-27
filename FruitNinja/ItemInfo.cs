// Decompiled with JetBrains decompiler
// Type: FruitNinja.ItemInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class ItemInfo
    {
      public string name;
      public uint nameHash;
      public int cost;
      public ItemType type;
      public string shopTitle;
      public string shopDescription;
      public string unlockDescription;
      public string unlockTotal;
      public int unlockCountDownFrom;
      public string textureName;
      public Color colour;
      public Color titleColor;
      public bool hasBeenSeen;

      public bool IsLocked() => this.cost > 0;

      public ItemInfo()
      {
        this.hasBeenSeen = true;
        this.name = (string) null;
        this.shopTitle = (string) null;
        this.shopDescription = (string) null;
        this.textureName = (string) null;
        this.unlockDescription = (string) null;
        this.cost = 0;
        this.type = ItemType.ITEM_NONE;
        this.colour = Color.White;
        this.unlockTotal = (string) null;
        this.unlockCountDownFrom = 0;
      }

      public virtual void SetEquipped()
      {
      }

      public virtual void Parse(XElement el)
      {
        XElement element = el.FirstChildElement("requirements");
        if (element != null)
        {
          this.cost = 1;
          element.QueryIntAttribute("coins", ref this.cost);
          this.unlockDescription = element.AttributeStr("description");
          if (this.unlockDescription == null)
            this.unlockDescription = element.Value;
          element.QueryIntAttribute("countDownFrom", ref this.unlockCountDownFrom);
          this.unlockTotal = element.AttributeStr("total");
        }
        this.name = el.AttributeStr("name");
        this.nameHash = StringFunctions.StringHash(this.name);
        this.shopTitle = el.AttributeStr("title");
        XElement xelement = el.FirstChildElement("description");
        if (xelement != null)
          this.shopDescription = xelement.Value;
        this.textureName = el.AttributeStr("texture");
        StringFunctions.ParseColour(ref this.colour, el.AttributeStr("colour"));
        this.titleColor = this.colour;
        StringFunctions.ParseColour(ref this.titleColor, el.AttributeStr("titleolour"));
      }
    }
}
