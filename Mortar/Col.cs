// Decompiled with JetBrains decompiler
// Type: Mortar.Col
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace Mortar
{

    public abstract class Col
    {
      public Vector3 centre;
      public static Color normalColor;
      public static Color colideColor;
      protected bool m_collided;

      public abstract COLISIONOBJECT GetType();

      public abstract bool Collide(Col obj2, out Vector3 proj);

      public abstract void DrawDebug();

      public void AddCollision() => this.m_collided = true;

      public void ClearCollideFlag() => this.m_collided = false;
    }
}
