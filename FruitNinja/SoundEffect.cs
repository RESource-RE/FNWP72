// Decompiled with JetBrains decompiler
// Type: FruitNinja.SoundEffect
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class SoundEffect
    {
      public string file;
      public float timeStart;
      public float timeEnd;
      public MortarSound sfx;

      public SoundEffect Duplicate()
      {
        return new SoundEffect()
        {
          file = this.file,
          timeStart = this.timeStart,
          timeEnd = this.timeEnd,
          sfx = this.sfx
        };
      }

      public SoundEffect()
      {
        this.sfx = (MortarSound) null;
        this.file = "";
        this.timeStart = 1f;
        this.timeEnd = -1f;
      }

      ~SoundEffect()
      {
        if (this.sfx == null)
          return;
        SoundManager.GetInstance().Release(this.sfx);
        Delete.SAFE_DELETE<MortarSound>(ref this.sfx);
      }

      public void Parse(XElement parent)
      {
        string str = parent.AttributeStr("name");
        if (str != null)
          this.file = str;
        parent.QueryFloatAttribute("timeStart", ref this.timeStart);
        parent.QueryFloatAttribute("timeEnd", ref this.timeEnd);
      }
    }
}
