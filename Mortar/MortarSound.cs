// Decompiled with JetBrains decompiler
// Type: Mortar.MortarSound
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework.Audio;

namespace Mortar
{

    public class MortarSound
    {
      public SoundEffectInstance inst;

      public void SetVolume(float v)
      {
        if (this.inst == null)
          return;
        this.inst.Volume = Math.CLAMP(v, 0.0f, 1f);
      }

      public void Stop(float v)
      {
        if (this.inst == null)
          return;
        this.inst.Stop();
      }

      public void Repeat()
      {
        if (this.inst == null || this.inst.State != SoundState.Stopped)
          return;
        this.inst.Play();
      }
    }
}
