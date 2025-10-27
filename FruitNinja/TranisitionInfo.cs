// Decompiled with JetBrains decompiler
// Type: FruitNinja.TranisitionInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    internal class TranisitionInfo
    {
      private TranisitionInfo.transitionFunc func;
      private float paramter;
      private float start;
      private float end;
      public float empty;
      public float full;

      public TranisitionInfo(TranisitionInfo.transitionFunc f, float p)
      {
        this.func = f;
        this.paramter = p;
        this.start = 0.0f;
        this.end = 1f;
        this.empty = 0.0f;
        this.full = 1f;
      }

      public TranisitionInfo()
      {
        this.func = (TranisitionInfo.transitionFunc) null;
        this.paramter = 0.0f;
        this.start = 0.0f;
        this.end = 1f;
        this.empty = 0.0f;
        this.full = 1f;
      }

      public float GetAmt(float amt)
      {
        amt = Math.CLAMP((float) (((double) amt - (double) this.start) / ((double) this.end - (double) this.start)), 0.0f, 1f);
        return this.empty + (float) ((this.func != null ? (double) this.func(amt, this.paramter) : (double) amt) * ((double) this.full - (double) this.empty));
      }

      public delegate float transitionFunc(float amt, float parameter);
    }
}
