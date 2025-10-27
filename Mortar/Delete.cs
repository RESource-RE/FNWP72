// Decompiled with JetBrains decompiler
// Type: Mortar.Delete
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class Delete
    {
      public static void SAFE_DELETE<T>(ref T dl) => dl = default (T);

      public static void SAFE_DELETE_ARRAY<T>(ref T dl) => dl = default (T);
    }
}
