// Decompiled with JetBrains decompiler
// Type: Mortar.MortarRectangleDec
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{
    public struct MortarRectangleDec
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public MortarRectangleDec(float l, float t, float r, float b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

        public float Width()
        {
            return right - left;
        }

        public float Height()
        {
            return bottom - top;
        }

        public PointDec Centre()
        {
            return new PointDec(left + Width() * 0.5f, top + Height() * 0.5f);
        }
    }
}