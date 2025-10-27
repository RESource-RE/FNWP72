// Decompiled with JetBrains decompiler
// Type: FruitNinja.WaveState
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Collections.Generic;

namespace FruitNinja
{

    public struct WaveState
    {
      public LinkedList<SpawnState> spawners;
      public float inc;
      public int index;
    }
}
