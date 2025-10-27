// Decompiled with JetBrains decompiler
// Type: FruitNinja.SuperFruitState
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class SuperFruitState
    {
      public float time;
      public int hits;
      public float sliceTime;
      public float rotation;

      private bool QueryIntAttribute(XElement element, string AttributeName, ref int value)
      {
        XAttribute xattribute = element.Attribute((XName) AttributeName);
        if (xattribute == null)
          return false;
        value = MParser.ParseInt(xattribute.Value);
        return true;
      }

      private bool QueryFloatAttribute(XElement element, string AttributeName, ref float value)
      {
        XAttribute xattribute = element.Attribute((XName) AttributeName);
        if (xattribute == null)
          return false;
        value = MParser.ParseFloat(xattribute.Value);
        return true;
      }

      public void Parse(XElement element)
      {
        if (element == null)
          return;
        this.QueryFloatAttribute(element, "time", ref this.time);
        this.QueryFloatAttribute(element, "sliceTime", ref this.sliceTime);
        this.QueryIntAttribute(element, "hits", ref this.hits);
        this.QueryFloatAttribute(element, "rot", ref this.rotation);
      }

      public XElement WriteToElement()
      {
        XElement element = new XElement((XName) "superFruitState");
        element.Add((object) new XAttribute((XName) "time", (object) this.time));
        element.Add((object) new XAttribute((XName) "sliceTime", (object) this.sliceTime));
        element.Add((object) new XAttribute((XName) "hits", (object) this.hits));
        element.Add((object) new XAttribute((XName) "rot", (object) this.rotation));
        return element;
      }
    }
}
