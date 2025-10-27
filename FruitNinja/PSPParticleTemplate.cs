// Decompiled with JetBrains decompiler
// Type: FruitNinja.PSPParticleTemplate
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class PSPParticleTemplate
    {
      public float life;
      public ushort first_particle;
      public Vector3 friction_start;
      public Vector3 friction_end;
      public Vector3 gravity_min;
      public Vector3 gravity_max;
      public byte particle_type;
      public byte coord_type;
      public byte size_start_min;
      public byte size_start_max;
      public byte size_mid_min;
      public byte size_mid_max;
      public byte size_end_min;
      public byte size_end_max;
      public short cycleX_start_min;
      public short cycleX_start_max;
      public short cycleX_end_min;
      public short cycleX_end_max;
      public short cycleY_start_min;
      public short cycleY_start_max;
      public short cycleY_end_min;
      public short cycleY_end_max;
      public short spin_start_min;
      public short spin_start_max;
      public short spin_end_min;
      public short spin_end_max;
      public ushort srcBlend;
      public ushort destBlend;
      public int angleMin;
      public int angleMax;
      public byte[] color_start_min = new byte[4];
      public byte[] color_start_max = new byte[4];
      public byte[] color_mid_min = new byte[4];
      public byte[] color_mid_max = new byte[4];
      public byte[] color_end_min = new byte[4];
      public byte[] color_end_max = new byte[4];
      public Texture tex;
      public float xScaleRatio;
      public int useDepth;
    }
}
