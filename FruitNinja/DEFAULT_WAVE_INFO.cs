// Decompiled with JetBrains decompiler
// Type: FruitNinja.DEFAULT_WAVE_INFO
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace FruitNinja
{

    public class DEFAULT_WAVE_INFO
    {
      public int waveChance;
      public float waveChanceRegrowth;
      public float criticalChance;
      public float dt;
      public float dtInc;
      public float dtSpInc;
      public float beforeDelay;
      public float beforeDelayInc;
      public float nextDelay;
      public float nextDelayInc;
      public float nextDelaySpInc;
      public bool waitForEntities;
      public float speedLoss;
      public int overideProbabilty;
      public int[] players = new int[Game.MAX_PLAYERS + 1];

      public void Reset()
      {
        this.speedLoss = 0.0f;
        this.players[0] = 0;
        this.players[1] = -1;
        this.waveChance = 10;
        this.waveChanceRegrowth = 0.25f;
        this.criticalChance = 1f;
        this.dt = 1f;
        this.dtInc = 0.0f;
        this.dtSpInc = 0.0f;
        this.nextDelay = 0.0f;
        this.nextDelayInc = 0.0f;
        this.nextDelaySpInc = 0.0f;
        this.beforeDelay = 2f;
        this.beforeDelayInc = 0.0f;
        this.overideProbabilty = 100;
        this.waitForEntities = true;
      }

      public DEFAULT_WAVE_INFO() => this.Reset();
    }
}
