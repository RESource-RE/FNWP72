// Decompiled with JetBrains decompiler
// Type: FruitNinja.WAVE_INFO
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Collections.Generic;

namespace FruitNinja
{

    public class WAVE_INFO
    {
      public int waveNo;
      public int waveNoRange;
      public SPAWNER_INFO[] spawners;
      public int spawnerCount;
      public float deltaT;
      public float deltaTinc;
      public float deltaSpinc;
      public float speedLoss;
      public float beforeDelay;
      public float beforeDelayInc;
      public float nextDelay;
      public float nextDelayInc;
      public float nextDelaySpInc;
      public float m_inc;
      public bool waitForEntities;
      public int chance;
      public int currentChance;
      public float chanceRegrowth;
      public float currentChanceRegrowth;
      public int gamesMin;
      public int gamesMax;
      public List<string> typesToPickFrom = new List<string>();
      public int typesToPickFromCount;
      public float criticalChance;
      public int idx;
      public COIN_CHANCEINATOR coinChanceinator;
      public int overideProbabilty;

      public WAVE_INFO Clone() => (WAVE_INFO) null;

      public WAVE_INFO()
      {
        this.m_inc = 0.0f;
        this.overideProbabilty = 100;
        this.gamesMin = -1;
        this.gamesMax = -1;
        this.spawnerCount = 0;
        this.waveNo = 0;
        this.waveNoRange = -1;
        this.deltaT = 1f;
        this.deltaTinc = 0.0f;
        this.beforeDelay = 2f;
        this.beforeDelayInc = 0.0f;
        this.nextDelay = 0.0f;
        this.nextDelayInc = 0.0f;
        this.nextDelaySpInc = 0.0f;
        this.deltaSpinc = 0.0f;
        this.speedLoss = 0.0f;
        this.chance = 10;
        this.chanceRegrowth = 0.25f;
        this.currentChanceRegrowth = 0.25f;
        this.spawners = (SPAWNER_INFO[]) null;
        this.criticalChance = 1f;
        this.coinChanceinator = (COIN_CHANCEINATOR) null;
        this.idx = 0;
        this.waitForEntities = true;
      }

      public WAVE_INFO(DEFAULT_WAVE_INFO defaultInfo)
      {
        this.m_inc = 0.0f;
        this.overideProbabilty = 100;
        this.gamesMin = -1;
        this.gamesMax = -1;
        this.spawnerCount = 0;
        this.waveNo = 0;
        this.waveNoRange = -1;
        this.deltaT = 1f;
        this.deltaTinc = 0.0f;
        this.beforeDelay = 2f;
        this.beforeDelayInc = 0.0f;
        this.nextDelay = 0.0f;
        this.nextDelayInc = 0.0f;
        this.nextDelaySpInc = 0.0f;
        this.deltaSpinc = 0.0f;
        this.chance = 10;
        this.chanceRegrowth = 0.25f;
        this.currentChanceRegrowth = 0.25f;
        this.spawners = (SPAWNER_INFO[]) null;
        this.criticalChance = 1f;
        this.idx = 0;
        this.coinChanceinator = (COIN_CHANCEINATOR) null;
        this.waitForEntities = true;
        this.speedLoss = 0.0f;
        this.deltaT = defaultInfo.dt;
        this.deltaTinc = defaultInfo.dtInc;
        this.deltaSpinc = defaultInfo.dtSpInc;
        this.beforeDelay = defaultInfo.beforeDelay;
        this.beforeDelayInc = defaultInfo.beforeDelayInc;
        this.nextDelay = defaultInfo.nextDelay;
        this.nextDelayInc = defaultInfo.nextDelayInc;
        this.nextDelaySpInc = defaultInfo.nextDelaySpInc;
        this.chance = defaultInfo.waveChance;
        this.chanceRegrowth = defaultInfo.waveChanceRegrowth;
        this.currentChanceRegrowth = defaultInfo.waveChanceRegrowth;
        this.criticalChance = defaultInfo.criticalChance;
        this.waitForEntities = defaultInfo.waitForEntities;
        this.speedLoss = defaultInfo.speedLoss;
        this.overideProbabilty = defaultInfo.overideProbabilty;
      }
    }
}
