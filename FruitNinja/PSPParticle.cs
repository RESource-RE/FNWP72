// Decompiled with JetBrains decompiler
// Type: FruitNinja.PSPParticle
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace FruitNinja
{

    public class PSPParticle
    {
      public Vector3 pos;
      public Vector3 vel;
      public Vector3 gravity;
      public byte[] Cs = new byte[4];
      public short[] Ci = new short[4];
      public short[] Ci2 = new short[4];
      public float time;
      public float lifeScale;
      public int next_particle;
      public ushort current_spin;
      public float spin_speed_start;
      public float spin_speed_end;
      public ushort current_x_scale_cycle;
      public float cycleX_speed_start;
      public float cycleX_speed_end;
      public ushort current_y_scale_cycle;
      public float cycleY_speed_start;
      public float cycleY_speed_end;
      public ushort sizeS;
      public short sizeInc;
      public short sizeInc2;
      public Vector2 vecX;
      public Vector2 vecY;
      public float sinz;
      public float cosz;
      public PSPParticleEmitter owner;
    }
}
