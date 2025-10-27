// Decompiled with JetBrains decompiler
// Type: FruitNinja.PSPEmitterTemplate
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Collections.Generic;

namespace FruitNinja
{

    public class PSPEmitterTemplate
    {
      public string name;
      public uint hash;
      public float life;
      public byte size_start;
      public byte size_end;
      public byte pad00;
      public byte particle_set_num;
      public List<PSPParticleSet> sets = new List<PSPParticleSet>();

      public bool Ends()
      {
        for (int index = 0; index < (int) this.particle_set_num; ++index)
        {
          if ((double) this.sets[index].time_end <= 0.0 && this.sets[index].number_per_second > (byte) 0)
            return false;
        }
        return true;
      }
    }
}
