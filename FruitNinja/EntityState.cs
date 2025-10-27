// Decompiled with JetBrains decompiler
// Type: FruitNinja.EntityState
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace FruitNinja
{

    public struct EntityState
    {
      public Vector3 vel;
      public Vector3 pos;
      public Vector3 grav;
      public bool hit;
      public int type;
      public float wait;
      public SuperFruitState superState;
    }
}
