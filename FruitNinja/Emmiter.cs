// Decompiled with JetBrains decompiler
// Type: FruitNinja.Emmiter
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class Emmiter
    {
      public uint hash;
      public PSPParticleEmitter emmiter;
      public Vector3 pos;

      public Emmiter Duplicate()
      {
        return new Emmiter()
        {
          hash = this.hash,
          emmiter = this.emmiter,
          pos = this.pos
        };
      }

      public Emmiter()
      {
        this.pos = Vector3.Zero;
        this.hash = 0U;
        this.emmiter = (PSPParticleEmitter) null;
      }

      public void Parse(XElement parent)
      {
        this.pos = Save.ParseVector(parent.AttributeStr("pos"));
        if (parent.Attribute((XName) "particle") == null)
          return;
        this.hash = StringFunctions.StringHash(parent.AttributeStr("particle"));
      }
    }
}
