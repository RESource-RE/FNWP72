// Decompiled with JetBrains decompiler
// Type: FruitNinja.SuperFruitGlow
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    internal class SuperFruitGlow : HUDControl3d
    {
      private bool m_shouldFadeAway;
      private Fruit m_fruit;
      private MortarSound m_loopSound;
      private float m_fadeOutTime;
      public static Texture GlowTexture;

      public SuperFruitGlow(Fruit fruit)
      {
        this.m_fruit = fruit;
        this.m_fadeOutTime = 0.0f;
        this.m_loopSound = (MortarSound) null;
        this.m_shouldFadeAway = false;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        this.m_fruit.m_fruitKilled += new Fruit.FruitEvent(this.FruitWasKilled);
        this.m_texture = SuperFruitGlow.GlowTexture;
        this.m_rotation = Utils.GetRandBetween(10f, 170f);
        this.m_loopSound = new MortarSound();
        SoundManager.GetInstance().SFXPlay("pome-lp", 0U, this.m_loopSound, (byte) 0, 0, true);
        this.m_scale = Vector3.One * 150f;
      }

      private void FruitWasKilled(Fruit fruit)
      {
        if (fruit == this.m_fruit)
          this.m_fruit = (Fruit) null;
        this.m_shouldFadeAway = true;
        this.StopSound();
      }

      ~SuperFruitGlow() => this.Release();

      public override void Release()
      {
        this.m_texture = (Texture) null;
        if (this.m_fruit != null)
          this.m_fruit.m_fruitKilled -= new Fruit.FruitEvent(this.FruitWasKilled);
        this.StopSound();
      }

      public void StopSound()
      {
        if (this.m_loopSound == null)
          return;
        this.m_loopSound.Stop(0.0f);
        this.m_loopSound = (MortarSound) null;
      }

      public override void Update(float dt)
      {
        if (!Game.game_work.pause)
        {
          this.m_rotation += dt * 60f;
          if (this.m_fruit != null && this.m_fruit.Sliced())
            this.m_shouldFadeAway = true;
          if (this.m_fruit == null)
            this.m_terminate = true;
          if (!this.m_shouldFadeAway)
          {
            this.m_fadeOutTime = Math.MIN(this.m_fadeOutTime + dt * 2f, 1f);
          }
          else
          {
            this.m_fadeOutTime -= dt * 2f;
            if ((double) this.m_fadeOutTime <= 0.0)
            {
              this.StopSound();
              this.m_terminate = true;
            }
          }
        }
        if (this.m_fruit != null)
        {
          this.m_pos = this.m_fruit.m_pos;
          this.m_pos.Z = this.m_fruit.m_z - 40f;
          this.m_pos = Game.game_work.camera.TranslatePos(this.m_pos, false, true);
        }
        this.m_color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 75f * this.m_fadeOutTime);
        if (this.m_loopSound == null)
          return;
        this.m_loopSound.SetVolume(Game.game_work.pause ? 0.0f : this.m_fadeOutTime);
      }

      public override void DrawOrder(float[] tintChannels, int order)
      {
        Vector3 scale = this.m_scale;
        SuperFruitGlow superFruitGlow = this;
        superFruitGlow.m_scale = superFruitGlow.m_scale * this.m_fadeOutTime;
        this.Draw(tintChannels);
        this.m_rotation = -this.m_rotation;
        this.Draw(tintChannels);
        this.m_rotation = -this.m_rotation;
        this.m_scale = scale;
      }
    }
}
