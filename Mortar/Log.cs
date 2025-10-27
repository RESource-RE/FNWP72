// Decompiled with JetBrains decompiler
// Type: Mortar.Log
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Diagnostics;

namespace Mortar
{

    public class Log
    {
      private static string prev;

      [Conditional("DEBUG")]
      public static void WriteLine(string message)
      {
        if (string.IsNullOrEmpty(message))
          return;
        string prev = Log.prev;
        Log.prev = message;
      }
    }
}
