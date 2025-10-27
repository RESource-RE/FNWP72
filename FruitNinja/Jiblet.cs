// Decompiled with JetBrains decompiler
// Type: FruitNinja.Jiblet
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System;

namespace FruitNinja
{

    internal class Jiblet : Entity
    {
      private Model m_model;
      private uint m_particleHash;
      private PSPParticleEmitter m_emmitter;
      public Matrix m_orientation;
      private float m_timeTillSplat;
      private float m_splatFrequency;
      private int m_fruitType;
      private Vector3 m_acc;
      private Vector3 m_rotation_speed;
      public float m_time;

      public void Init(
        int fruitType,
        Vector3 pos,
        float scale,
        Vector3 vel,
        Model model,
        uint particles,
        float splatFrequency,
        Vector3 acc)
      {
        this.m_pos = pos;
        this.m_model = model;
        this.m_vel = vel;
        this.m_acc = acc;
        this.m_cur_scale = Vector3.One * scale;
        this.m_time = -0.04f;
        this.m_orientation = Matrix.Identity;
        this.m_orientation *= Matrix.CreateRotationX(Mortar.Math.IDX_TO_RADIANS((ushort) Mortar.Math.g_random.Rand32()));
        this.m_orientation *= Matrix.CreateRotationY(Mortar.Math.IDX_TO_RADIANS((ushort) Mortar.Math.g_random.Rand32()));
        this.m_orientation *= Matrix.CreateRotationZ(Mortar.Math.IDX_TO_RADIANS((ushort) Mortar.Math.g_random.Rand32()));
        this.m_dormant = false;
        this.m_destroy = false;
        this.m_rotation_speed = new Vector3(Utils.GetRandBetween(-100f, 100f), Utils.GetRandBetween(-100f, 100f), Utils.GetRandBetween(-100f, 100f));
        this.m_fruitType = fruitType;
        this.m_splatFrequency = splatFrequency;
        this.m_timeTillSplat = (double) this.m_splatFrequency <= 0.0 ? 100f : Utils.GetRandBetween(0.0f, 1f / splatFrequency);
        this.m_particleHash = particles;
        this.m_emmitter = (PSPParticleEmitter) null;
      }

      public override void Update(float dt)
      {
        this.m_time += dt;
        if (this.m_emmitter == null && (double) this.m_time > 0.05000000074505806 && PSPParticleManager.GetInstance().EmitterExists(this.m_particleHash))
        {
          this.m_emmitter = PSPParticleManager.GetInstance().AddEmitter(this.m_particleHash, (Action<PSPParticleEmitter>) null);
          if (this.m_emmitter != null)
          {
            this.m_emmitter.canNotBeRepulsed = true;
            Vector3 vel = this.m_vel;
            vel.Normalize();
            this.m_emmitter.pos = this.m_pos;
            this.m_emmitter.cosz = -vel.Y;
            this.m_emmitter.sinz = -vel.X;
          }
        }
        if ((double) this.m_time >= 0.0)
        {
          Jiblet jiblet1 = this;
          jiblet1.m_pos = jiblet1.m_pos + (this.m_vel + this.m_acc * 0.5f * dt) * dt;
          Jiblet jiblet2 = this;
          jiblet2.m_vel = jiblet2.m_vel + this.m_acc * dt;
          this.m_orientation *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new Vector3(1f, 0.0f, 0.0f), (float) Mortar.Math.DEGREE_TO_IDX(this.m_rotation_speed.X * dt)) * Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), (float) Mortar.Math.DEGREE_TO_IDX(this.m_rotation_speed.Y * dt)) * Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), (float) Mortar.Math.DEGREE_TO_IDX(this.m_rotation_speed.Z * dt)));
          for (this.m_timeTillSplat -= dt; (double) this.m_timeTillSplat < 0.0 && (double) this.m_splatFrequency > 0.0; this.m_timeTillSplat += 1f / this.m_splatFrequency)
          {
            ushort idx = (ushort) Mortar.Math.g_random.Rand32();
            float randBetween = Utils.GetRandBetween(1f, 40f);
            SplatEntity.GetFree().MakeSplat(this.m_pos, new Vector3(Mortar.Math.SinIdx(idx) * randBetween, Mortar.Math.CosIdx(idx) * randBetween, 0.0f), false, true, this.m_fruitType);
          }
        }
        if (this.m_emmitter != null)
          this.m_emmitter.pos = this.m_pos;
        if (Mortar.Math.BETWEEN(this.m_pos.X, (float) (-(double) Game.SCREEN_WIDTH * 0.60000002384185791), Game.SCREEN_WIDTH * 0.6f) && Mortar.Math.BETWEEN(this.m_pos.Y, (float) (-(double) Game.SCREEN_HEIGHT * 0.60000002384185791), Game.SCREEN_HEIGHT * 0.6f))
          return;
        this.Kill();
      }

      private void Kill()
      {
        if (this.m_emmitter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_emmitter);
          this.m_emmitter = (PSPParticleEmitter) null;
        }
        this.m_model = (Model) null;
        this.m_destroy = true;
      }

      public override void Draw()
      {
        if (this.m_model == null)
          return;
        this.m_model.Draw(new Matrix?(this.m_orientation * Matrix.CreateScale(this.m_cur_scale) * Matrix.CreateTranslation(this.m_pos)));
      }

      public override void DrawUpdate(float dt)
      {
      }
    }
}
