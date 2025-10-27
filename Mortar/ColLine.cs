// Decompiled with JetBrains decompiler
// Type: Mortar.ColLine
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;

namespace Mortar
{

    public class ColLine : Col
    {
      public Vector3 Direction;

      public ColLine()
      {
        this.centre = Vector3.Zero;
        this.Direction = new Vector3(1f, 0.0f, 0.0f);
        this.ClearCollideFlag();
      }

      public ColLine(Vector3 pos, Vector3 dir)
      {
        this.centre = pos;
        this.Direction = dir;
        this.ClearCollideFlag();
      }

      public override COLISIONOBJECT GetType() => COLISIONOBJECT.COL_LINE;

      public override bool Collide(Col obj2, out Vector3 proj)
      {
        bool flag = false;
        proj = new Vector3();
        switch (obj2.GetType())
        {
          case COLISIONOBJECT.COL_AABB:
            proj = -proj;
            if (flag)
            {
              this.AddCollision();
              obj2.AddCollision();
              break;
            }
            break;
          case COLISIONOBJECT.COL_SPHERE:
            flag = ColSphere.ColSphereLine((ColSphere) obj2, this, out proj);
            if (flag)
            {
              this.AddCollision();
              obj2.AddCollision();
              break;
            }
            break;
          case COLISIONOBJECT.COL_LINE:
            flag = ColLine.ColLineLine(this, (ColLine) obj2, out proj);
            if (flag)
            {
              this.AddCollision();
              obj2.AddCollision();
              break;
            }
            break;
          default:
            flag = obj2.Collide((Col) this, out proj);
            break;
        }
        return flag;
      }

      public override void DrawDebug() => throw new MissingMethodException();

      public static bool ColLineLine(ColLine obj1, ColLine obj2, out Vector3 proj)
      {
        proj = new Vector3();
        Vector3 vector3_1 = obj1.Direction - obj1.centre;
        Vector3 vector3_2 = obj2.Direction - obj2.centre;
        Vector3 vector2 = obj1.centre - obj2.centre;
        float num1 = Vector3.Dot(vector3_1, vector3_1);
        float num2 = Vector3.Dot(vector3_1, vector3_2);
        float num3 = Vector3.Dot(vector3_2, vector3_2);
        float num4 = Vector3.Dot(vector3_1, vector2);
        float num5 = Vector3.Dot(vector3_2, vector2);
        float num6 = (float) ((double) num1 * (double) num3 - (double) num2 * (double) num2);
        float num7;
        float num8;
        if ((double) num6 < 9.9999999747524271E-07)
        {
          num7 = 0.0f;
          num8 = (double) num2 > (double) num3 ? num4 / num2 : num5 / num3;
        }
        else
        {
          num7 = (float) ((double) num2 * (double) num5 - (double) num3 * (double) num4) / num6;
          num8 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4) / num6;
        }
        if ((double) (vector2 + vector3_1 * num7 - vector3_2 * num8).LengthSquared() >= 9.9999997473787516E-06 || (double) num7 < 0.0 || (double) num7 > 1.0 || (double) num8 < 0.0 || (double) num8 > 1.0)
          return false;
        proj = (double) num7 < 0.5 ? vector3_1 * num7 : -vector3_1 * (1f - num7);
        return true;
      }
    }
}
