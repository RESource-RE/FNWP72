// Decompiled with JetBrains decompiler
// Type: Mortar.Assert
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;
using System.Diagnostics;

namespace Mortar
{

    public class Assert
    {
      [Conditional("DEBUG")]
      public static void ASSERT(bool f)
      {
        if (!f)
          throw new MissingMethodException("From assert");
      }

      [Conditional("DEBUG")]
      public static void PANIC(string sdf) => throw new MissingMethodException(sdf);
    }
}
