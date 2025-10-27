// Decompiled with JetBrains decompiler
// Type: Mortar.ColRay
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace Mortar
{
    public struct ColRay
    {
        private Vector3 o;
        private Vector3 d;

        public ColRay(Vector3 v1, Vector3 v2)
        {
            o = v1;
            d = v2;
        }
    }

}
