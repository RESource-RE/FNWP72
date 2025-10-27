// Decompiled with JetBrains decompiler
// Type: Mortar.FadeManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mortar
{

    public class FadeManager
    {
      private const float MINIMUM_FADE_TIME = 17f;
      private Texture2D m_black;
      private Texture2D m_white;
      private FadeManager.FadeColour m_colour;
      private FadeState m_state;
      private FadeManager.FadeType m_type;
      private float m_alpha;
      private float m_time;
      private float m_step;
      private bool m_fading;

      public FadeManager(Texture2D black, Texture2D white)
      {
        this.Reset();
        this.m_black = black;
        this.m_white = white;
        this.m_colour = FadeManager.FadeColour.None;
      }

      public void Start(FadeState state, float time)
      {
        lock (this)
        {
          if (this.m_state != FadeState.None)
            return;
          if (state == FadeState.ToBlack)
          {
            this.m_colour = FadeManager.FadeColour.Black;
            this.m_time = time;
            this.m_fading = true;
            this.m_step = this.m_time / (float) byte.MaxValue;
            this.m_type = FadeManager.FadeType.Colour;
          }
          else if (state == FadeState.ToWhite)
          {
            this.m_colour = FadeManager.FadeColour.White;
            this.m_time = time;
            this.m_fading = true;
            this.m_step = this.m_time / (float) byte.MaxValue;
            this.m_type = FadeManager.FadeType.Colour;
          }
          else
          {
            if (state != FadeState.ToNormal)
              throw new ArgumentException("Invald fade state");
            this.m_time = time * 2f;
            this.m_fading = true;
            this.m_step = this.m_time / (float) byte.MaxValue;
            this.m_type = FadeManager.FadeType.Normal;
          }
        }
      }

      public void Update(GameTime gameTime)
      {
        if (!this.m_fading)
          return;
        this.m_time -= (float) gameTime.ElapsedGameTime.Milliseconds;
        if ((double) this.m_time < 0.0)
        {
          this.m_fading = false;
          this.m_time = 0.0f;
          this.m_step = 0.0f;
          this.m_alpha = this.m_type == FadeManager.FadeType.Colour ? (float) byte.MaxValue : 0.0f;
        }
        else
        {
          float num = this.m_time / this.m_step;
          this.m_alpha = this.m_type == FadeManager.FadeType.Colour ? (float) byte.MaxValue - num : num;
        }
      }

      public void Draw(GraphicsDevice device, SpriteBatch batch)
      {
        if ((double) this.m_alpha <= 0.0)
          return;
        Texture2D texture = this.m_colour == FadeManager.FadeColour.Black ? this.m_black : this.m_white;
        batch.Draw(texture, new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height), new Color(1f, 1f, 1f, this.m_alpha / (float) byte.MaxValue));
      }

      private void Reset()
      {
        this.m_colour = FadeManager.FadeColour.None;
        this.m_state = FadeState.None;
        this.m_type = FadeManager.FadeType.None;
        this.m_alpha = 0.0f;
        this.m_time = 0.0f;
        this.m_step = 0.0f;
        this.m_fading = false;
      }

      public bool IsFading
      {
        get
        {
          lock (this)
            return this.m_fading;
        }
      }

      public bool IsFadeVisible => (double) this.m_alpha != 0.0;

      public void DrawFade(GraphicsDevice device, SpriteBatch batch)
      {
        Color white = Color.White;
        white.A = 128 /*0x80*/;
        batch.Draw(this.m_black, new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height), white);
      }

      public void SetBlack()
      {
        lock (this)
        {
          this.m_state = FadeState.None;
          this.m_colour = FadeManager.FadeColour.Black;
          this.m_alpha = (float) byte.MaxValue;
          this.m_fading = false;
        }
      }

      private enum FadeColour
      {
        None,
        Black,
        White,
      }

      private enum FadeType
      {
        None,
        Colour,
        Normal,
      }
    }
}
