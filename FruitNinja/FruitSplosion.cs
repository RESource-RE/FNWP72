// Decompiled with JetBrains decompiler
// Type: FruitNinja.FruitSplosion
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;

namespace FruitNinja
{

    internal class FruitSplosion : HUDControl3d
    {
      private static FruitSplosion controlThatMadeMe;
      private Fruit fruit;
      private float time;
      private float m_maxRadius;
      private float m_growTime;
      private float m_waitTime;
      private float m_fadeTime;
      private int m_comboType;
      private FruitSplosion m_lastCreatedChild;
      private FruitSplosion m_root;
      private int m_comboCount;

      public FruitSplosion(
        Fruit f,
        float rad,
        float growTime,
        float waitTime,
        float fadeTime,
        int comboType)
      {
        this.fruit = f;
        this.time = 0.0f;
        this.m_root = (FruitSplosion) null;
        this.m_lastCreatedChild = (FruitSplosion) null;
        this.m_maxRadius = rad;
        this.m_growTime = growTime;
        this.m_waitTime = waitTime;
        this.m_fadeTime = fadeTime;
        this.m_comboType = comboType;
        this.m_pos = this.fruit.m_pos;
        this.fruit.m_fruitKilled += new Fruit.FruitEvent(this.FruitWasKilled);
        this.m_pos.Z = -4999f;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        if (FruitSplosion.controlThatMadeMe != null)
        {
          this.m_comboCount = 0;
          this.m_root = FruitSplosion.controlThatMadeMe.m_root != null ? FruitSplosion.controlThatMadeMe.m_root : FruitSplosion.controlThatMadeMe;
          this.m_root.m_lastCreatedChild = this;
          ++this.m_root.m_comboCount;
          if (this.m_root.m_comboCount >= 3 && this.m_comboType == 1)
          {
            MissControl.GetFree().MakeCombo(this.m_pos, this.m_root.m_comboCount);
            Game.AddToCurrentScore(this.m_root.m_comboCount, 0, false, false);
          }
          this.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.ADingoAteMyBaby);
        }
        else
          this.m_comboCount = 1;
        this.m_texture = Texture.Load("textureswp7/explosion_radius.tex");
      }

      ~FruitSplosion()
      {
        if (this.fruit == null)
          return;
        this.fruit.m_fruitKilled -= new Fruit.FruitEvent(this.FruitWasKilled);
      }

      private void ADingoAteMyBaby(HUDControl control)
      {
        if (control != this.m_lastCreatedChild)
          return;
        if (this.m_comboCount >= 3 && this.m_comboType == 2)
        {
          MissControl.GetFree().MakeCombo(this.m_pos, this.m_comboCount);
          Game.AddToCurrentScore(this.m_comboCount, 0, false, false);
        }
        this.m_terminate = true;
      }

      private void FruitWasKilled(Fruit _fruit)
      {
        if (this.fruit != _fruit)
          return;
        this.fruit = (Fruit) null;
      }

      public override void Update(float dt)
      {
        if (Game.game_work.pause)
          return;
        dt *= WaveManager.GetInstance().GetWavedt();
        this.time += dt;
        if ((double) this.time > (double) this.m_waitTime)
        {
          if ((double) this.time < (double) this.m_fadeTime || this.m_lastCreatedChild != null)
            return;
          this.m_terminate = true;
        }
        else
        {
          LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
          Fruit fruit = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator);
          float num = this.m_maxRadius * TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.time, 0.0f, this.m_growTime, true), 90f);
          for (; fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
          {
            if (fruit != this.fruit && fruit.IsActive() && !fruit.Sliced())
            {
              Vector3 vector3 = fruit.m_pos - this.m_pos;
              vector3.Z = 0.0f;
              if ((double) vector3.LengthSquared() < (double) num * (double) num)
              {
                vector3.Normalize();
                Vector3 proj = vector3 * 10f;
                FruitSplosion.controlThatMadeMe = this;
                fruit.CollisionResponse((Entity) this.fruit, 0U, 0U, ref proj);
                FruitSplosion.controlThatMadeMe = (FruitSplosion) null;
              }
            }
          }
          this.m_scale = Vector3.One * num * 2f * (27f / 32f);
        }
      }

      public override void DrawOrder(float[] tintChannels, int order)
      {
        base.DrawOrder(tintChannels, order);
      }
    }
}
