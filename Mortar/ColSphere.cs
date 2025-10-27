// Decompiled with JetBrains decompiler
// Type: Mortar.ColSphere
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;

namespace Mortar
{

    public class ColSphere : Col
    {
      public float Radius;

      public ColSphere()
      {
        this.centre = Vector3.Zero;
        this.Radius = 1f;
        this.ClearCollideFlag();
      }

      public ColSphere(Vector3 pos, float rad)
      {
        this.centre = pos;
        this.Radius = rad;
        this.ClearCollideFlag();
      }

      public override COLISIONOBJECT GetType() => COLISIONOBJECT.COL_SPHERE;

      public override bool Collide(Col obj2, out Vector3 proj)
      {
        proj = new Vector3();
        return false;
      }

      public override void DrawDebug() => throw new MissingMethodException();

      public static bool ColSphereSphere(ColSphere obj1, ColSphere obj2, out Vector3 proj)
      {
        proj = new Vector3();
        return false;
      }

      public static bool ColSphereLine(ColSphere obj1, ColLine obj2, out Vector3 proj)
      {
        proj = new Vector3();
        Vector3 zero = Vector3.Zero;
        Math.ClosestPointOnLine(ref obj2.centre, ref obj2.Direction, ref obj1.centre, ref zero);
        Vector3 vector3 = zero - obj1.centre;
        if ((double) vector3.LengthSquared() >= (double) obj1.Radius * (double) obj1.Radius)
          return false;
        float num = vector3.Length();
        vector3.Normalize();
        vector3 *= Math.ABS(obj1.Radius - num);
        proj = vector3;
        return true;
      }
    }
}
