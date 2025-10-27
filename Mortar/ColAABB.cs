// Decompiled with JetBrains decompiler
// Type: Mortar.ColAABB
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;

namespace Mortar
{

    public class ColAABB : Col
    {
      public Vector3 extents;

      public ColAABB()
      {
        this.centre = Vector3.Zero;
        this.extents = Vector3.One;
        this.ClearCollideFlag();
      }

      public ColAABB(Vector3 pos, Vector3 ext)
      {
        this.centre = pos;
        this.extents = ext;
        this.ClearCollideFlag();
      }

      public override COLISIONOBJECT GetType() => COLISIONOBJECT.COL_AABB;

      public override bool Collide(Col obj2, out Vector3 proj)
      {
        bool flag = false;
        switch (obj2.GetType())
        {
          case COLISIONOBJECT.COL_AABB:
            proj = Vector3.Zero;
            throw new MissingMethodException();
          case COLISIONOBJECT.COL_SPHERE:
            proj = Vector3.Zero;
            throw new MissingMethodException();
          case COLISIONOBJECT.COL_LINE:
            proj = Vector3.Zero;
            break;
          default:
            flag = obj2.Collide((Col) this, out proj);
            break;
        }
        return flag;
      }

      public override void DrawDebug() => throw new MissingMethodException();
    }
}
