// Decompiled with JetBrains decompiler
// Type: FruitNinja.SuperFruitHitControl
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class SuperFruitHitControl : HUDControl
    {
      public const float HIT_POP_SCALE_TIME = 0.2f;
      public const float HIT_TOTAL_TIME = 1f;
      public float m_time;
      public SuperFruitControl m_parent;

      public SuperFruitHitControl(int hits, Vector3 pos, SuperFruitControl parent)
      {
        this.m_time = 0.0f;
        this.m_parent = parent;
      }

      ~SuperFruitHitControl()
      {
      }

      public override void Update(float dt)
      {
        if (!Game.game_work.pause)
          this.m_time += dt;
        if ((double) this.m_time <= 1.0)
          return;
        if (this.m_parent != null)
          this.m_parent.m_lastCreatedHitControl = (SuperFruitHitControl) null;
        this.m_terminate = true;
      }

      public float GetScale()
      {
        if ((double) this.m_time < 0.20000000298023224)
          return TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.m_time, 0.0f, 0.2f, true), 115f);
        return (double) this.m_time > 0.800000011920929 ? TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.m_time, 1f, 0.8f, true), 115f) : 1f;
      }

      public void RemoveQuickly() => this.m_time = Math.MAX(this.m_time, 0.8f);

      public override void DrawOrder(float[] tintChannels, int order)
      {
      }
    }
}
