// Decompiled with JetBrains decompiler
// Type: FruitNinja.PToDraw
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    public class PToDraw
    {
      public int depth;
      public int numPoints;
      public int offset;
      public Texture tex;

      public PToDraw(int points, int ofs, Texture t, int d)
      {
        this.numPoints = points;
        this.offset = ofs;
        this.tex = t;
        this.depth = d;
      }
    }
}
