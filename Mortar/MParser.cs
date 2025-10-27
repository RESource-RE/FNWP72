// Decompiled with JetBrains decompiler
// Type: Mortar.MParser
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;
using System.Globalization;

namespace Mortar
{

    public static class MParser
    {
      public static int ParseInt(string astr)
      {
        return int.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }

      public static float ParseFloat(string astr)
      {
        return float.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }

      public static uint ParseUInt(string astr)
      {
        return uint.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }

      public static ushort ParseUShort(string astr)
      {
        return ushort.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }

      public static ulong ParseULong(string astr)
      {
        return ulong.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }

      public static byte ParseByte(string astr)
      {
        return byte.Parse(astr, (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }
}
