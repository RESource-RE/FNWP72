// Decompiled with JetBrains decompiler
// Type: Mortar.MortarRectangle
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public struct MortarRectangle
    {
      public int left;
      public int top;
      public int right;
      public int bottom;

      public int Width() => this.right - this.left;

      public int Height() => this.bottom - this.top;

      public Point Centre()
      {
        return new Point(this.left + (this.Width() >> 1), this.top + (this.Height() >> 1));
      }
    }
}
