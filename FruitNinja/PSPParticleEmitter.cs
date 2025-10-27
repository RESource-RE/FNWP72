// Decompiled with JetBrains decompiler
// Type: FruitNinja.PSPParticleEmitter
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;

namespace FruitNinja
{

    public class PSPParticleEmitter
    {
      public float time;
      public ushort enabled;
      public Vector3 pos;
      public Vector3 _vel;
      public float rateScale;
      public float lifeScale;
      public float sizeScale;
      public float dtMod;
      public float cosz;
      public float sinz;
      public float scale;
      public bool canNotBeRepulsed;
      public PSPEmitterTemplate tmplt;
      public Action<PSPParticleEmitter> zeroer;
      public bool updateEvenIfPaused;

      public void Update(float dt)
      {
        PSPParticleManager instance = PSPParticleManager.GetInstance();
        for (int index1 = 0; index1 < (int) this.tmplt.particle_set_num; ++index1)
        {
          PSPParticleSet set = this.tmplt.sets[index1];
          if ((double) this.time >= (double) set.time_start && ((double) set.time_end == 0.0 || (double) this.time <= (double) set.time_end))
          {
            int num1 = (int) ((double) set.number_per_second * ((double) this.time - (double) set.time_start));
            int num2 = (int) ((double) set.number_per_second * ((double) this.time + (double) dt * (double) this.rateScale - (double) set.time_start)) - num1;
            for (int index2 = 0; index2 < num2; ++index2)
              this.AddParticle(set, instance);
          }
          if ((double) this.time == (double) set.time_start)
          {
            for (int index3 = 0; index3 < (int) set.number_start; ++index3)
              this.AddParticle(set, instance);
            if ((double) this.rateScale == 0.0)
              this.time += dt;
          }
        }
        this.time += dt * this.rateScale;
        this.pos += this._vel;
      }

      public void AddParticle(PSPParticleSet t, PSPParticleManager pm)
      {
        if (t.template_idx == null || pm.particleFreeList == (ushort) 0)
          return;
        ++pm.m_activeParticles;
        PSPParticleTemplate templateIdx = t.template_idx;
        ushort particleFreeList = pm.particleFreeList;
        PSPParticle particle = pm.particles[(int) particleFreeList];
        pm.particleFreeList = (ushort) particle.next_particle;
        particle.next_particle = (int) templateIdx.first_particle;
        templateIdx.first_particle = particleFreeList;
        if (particle == null)
          return;
        particle.time = templateIdx.life;
        particle.lifeScale = templateIdx.life - this.lifeScale * templateIdx.life;
        particle.pos = templateIdx.coord_type != (byte) 0 ? Vector3.Zero : this.pos;
        particle.owner = this;
        particle.gravity = templateIdx.gravity_min + (templateIdx.gravity_max - templateIdx.gravity_min) * Mortar.Math.g_random.RandF(1f);
        if (Game.IsMultiplayer() && !this.updateEvenIfPaused)
        {
          float x = particle.gravity.X;
          particle.gravity.X = particle.gravity.Y;
          particle.gravity.Y = x;
          particle.gravity.X *= (float) -Mortar.Math.MATH_SIGN(particle.pos.X);
          particle.gravity *= this.scale;
        }
        Vector3 vector3_1 = new Vector3(t.vel_min.X + (t.vel_max.X - t.vel_min.X) * Mortar.Math.g_random.RandF(1f), t.vel_min.Y + (t.vel_max.Y - t.vel_min.Y) * Mortar.Math.g_random.RandF(1f), t.vel_min.Z + (t.vel_max.Z - t.vel_min.Z) * Mortar.Math.g_random.RandF(1f)) * this.scale;
        float x1 = vector3_1.X;
        float y = vector3_1.Y;
        vector3_1.X = (float) ((double) x1 * (double) this.cosz + (double) y * (double) this.sinz);
        vector3_1.Y = (float) ((double) x1 * -(double) this.sinz + (double) y * (double) this.cosz);
        Vector3 vector3_2 = vector3_1 / 2f;
        particle.vel.X = vector3_2.X;
        particle.vel.Y = vector3_2.Y;
        particle.vel.Z = vector3_2.Z;
        particle.current_spin = Mortar.Math.DEGREE_TO_IDX(Mortar.Math.floatLERP((float) templateIdx.angleMin, (float) templateIdx.angleMax, Mortar.Math.g_random.RandF(1f)));
        if (templateIdx.particle_type == (byte) 1)
          particle.pos -= particle.vel * templateIdx.life;
        float num1 = (float) Mortar.Math.LERP((int) templateIdx.size_start_min, (int) templateIdx.size_start_max, Mortar.Math.g_random.Rand32(4095 /*0x0FFF*/) & 4095 /*0x0FFF*/);
        float num2 = (float) Mortar.Math.LERP((int) templateIdx.size_mid_min, (int) templateIdx.size_mid_max, Mortar.Math.g_random.Rand32(4095 /*0x0FFF*/) & 4095 /*0x0FFF*/);
        float num3 = (float) Mortar.Math.LERP((int) templateIdx.size_end_min, (int) templateIdx.size_end_max, Mortar.Math.g_random.Rand32(4095 /*0x0FFF*/) & 4095 /*0x0FFF*/);
        particle.sizeS = (ushort) ((double) num1 * (double) this.sizeScale);
        particle.sizeInc = (short) (((double) num2 - (double) num1) * (double) this.sizeScale);
        particle.sizeInc2 = (short) (((double) num3 - (double) num2) * (double) this.sizeScale);
        for (int index = 0; index < 4; ++index)
        {
          long num4 = (long) templateIdx.color_start_min[index];
          long num5 = (long) templateIdx.color_start_max[index];
          float num6 = Mortar.Math.g_random.RandF(1f);
          int num7 = (int) ((double) num4 + (double) (num5 - num4) * (double) num6);
          long num8 = (long) templateIdx.color_mid_min[index];
          long num9 = (long) templateIdx.color_mid_max[index];
          this.scale = Mortar.Math.g_random.RandF(1f);
          int num10 = (int) ((double) num8 + (double) (num9 - num8) * (double) num6);
          long num11 = (long) templateIdx.color_end_min[index];
          long num12 = (long) templateIdx.color_end_max[index];
          this.scale = Mortar.Math.g_random.RandF(1f);
          int num13 = (int) ((double) num11 + (double) (num12 - num11) * (double) num6);
          particle.Cs[index] = (byte) num7;
          particle.Ci[index] = (short) (num10 - num7);
          particle.Ci2[index] = (short) (num13 - num10);
        }
        if (templateIdx.particle_type == (byte) 2)
          particle.current_spin += Mortar.Math.Atan2Idx(particle.vel.X, particle.vel.Y);
        float t1 = Mortar.Math.g_random.RandF(1f);
        particle.spin_speed_start = Mortar.Math.floatLERP((float) templateIdx.spin_start_min, (float) templateIdx.spin_start_max, t1);
        particle.spin_speed_end = Mortar.Math.floatLERP((float) templateIdx.spin_end_min, (float) templateIdx.spin_end_max, t1);
        float t2 = Mortar.Math.g_random.RandF(1f);
        particle.cycleX_speed_start = Mortar.Math.floatLERP((float) templateIdx.cycleX_start_min, (float) templateIdx.cycleX_start_max, t2);
        particle.cycleX_speed_end = Mortar.Math.floatLERP((float) templateIdx.cycleX_end_min, (float) templateIdx.cycleX_end_max, t2);
        particle.current_x_scale_cycle = (double) particle.cycleX_speed_end != 0.0 || (double) particle.cycleX_speed_start != 0.0 ? (ushort) Mortar.Math.g_random.Rand32() : (ushort) 0;
        float t3 = Mortar.Math.g_random.RandF(1f);
        particle.cycleY_speed_start = Mortar.Math.floatLERP((float) templateIdx.cycleY_start_min, (float) templateIdx.cycleY_start_max, t3);
        particle.cycleY_speed_end = Mortar.Math.floatLERP((float) templateIdx.cycleY_end_min, (float) templateIdx.cycleY_end_max, t3);
        particle.current_y_scale_cycle = (double) particle.cycleY_speed_end != 0.0 || (double) particle.cycleY_speed_start != 0.0 ? (ushort) Mortar.Math.g_random.Rand32() : (ushort) 0;
        if (particle.current_spin == (ushort) 0)
        {
          particle.sinz = -1f;
          particle.cosz = 1f;
          particle.vecX = new Vector2(1f, 0.0f);
          particle.vecY = new Vector2(0.0f, 1f);
        }
        else
        {
          particle.vecX = new Vector2(Mortar.Math.SinIdx((ushort) ((uint) particle.current_spin + 16384U /*0x4000*/)), Mortar.Math.CosIdx((ushort) ((uint) particle.current_spin + 16384U /*0x4000*/)));
          particle.vecY = new Vector2(Mortar.Math.SinIdx(particle.current_spin), Mortar.Math.CosIdx(particle.current_spin));
          ushort idx = (ushort) (((int) particle.current_spin + 57330) % 65520);
          particle.sinz = Mortar.Math.SinIdx(idx) * 1.41f;
          particle.cosz = Mortar.Math.CosIdx(idx) * 1.41f;
        }
      }
    }
}
