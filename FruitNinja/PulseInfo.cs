// Decompiled with JetBrains decompiler
// Type: FruitNinja.PulseInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;

namespace FruitNinja
{

    internal class PulseInfo
    {
      private float start;
      private float end;
      private float frequency;
      private float def;
      private TranisitionInfo transition;

      private float GetPulseAmt(float time)
      {
        return (double) time > (double) this.start && (double) this.frequency > 0.0 && ((double) time <= (double) this.end || (double) this.end <= (double) this.start) ? this.transition.GetAmt((float) Math.IEEERemainder((double) time - (double) this.start, (double) this.frequency) / this.frequency) : this.def;
      }

      internal PulseInfo(float s, float f, float e, TranisitionInfo t)
      {
        this.def = 1f;
        this.start = s;
        this.frequency = f;
        this.end = e;
        this.transition = t;
        this.transition.empty = 1f;
        this.transition.full = 1.25f;
      }

      public PulseInfo()
        : this(0.0f, 0.0f, 0.0f, new TranisitionInfo(new TranisitionInfo.transitionFunc(TransitionFunctions.FullTransition), 0.0f))
      {
      }

      public PulseInfo(float s, float f, float e)
        : this(s, f, e, new TranisitionInfo(new TranisitionInfo.transitionFunc(TransitionFunctions.FullTransition), 0.0f))
      {
      }

      public PulseInfo(TranisitionInfo t, float s, float f, float e)
        : this(s, f, e, t)
      {
        this.Init(s, f, e);
        this.transition = t;
      }

      private void Init(float s, float f, float e)
      {
        this.def = 1f;
        this.start = s;
        this.frequency = f;
        this.end = e;
        this.transition = new TranisitionInfo(new TranisitionInfo.transitionFunc(TransitionFunctions.FullTransition), 0.0f);
        this.transition.empty = 1f;
        this.transition.full = 1.25f;
      }
    }
}
