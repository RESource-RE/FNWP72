// Decompiled with JetBrains decompiler
// Type: FruitNinja.Utils
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace FruitNinja
{

    internal class Utils
    {
      public static float GetRandBetween(float start, float end)
      {
        return Utils.GetRandBetween(start, end, 0.0f);
      }

      public static float GetRandBetween(float start, float end, float negatePercentage)
      {
        float randBetween = start;
        if ((double) start != (double) end)
          randBetween += (end - start) * Mortar.Math.g_random.RandF(1f);
        if ((double) negatePercentage > 0.0 && (double) Mortar.Math.g_random.RandF(1f) <= (double) negatePercentage)
          randBetween = -randBetween;
        return randBetween;
      }

      public static Vector3 VectorFromMagAngle(float magnitude, ushort angle)
      {
        return new Vector3(Mortar.Math.CosIdx(angle) * magnitude, Mortar.Math.SinIdx(angle) * magnitude, 0.0f);
      }

      public static Vector3 VectorFromMagDegree(float magnitude, float angle)
      {
        return new Vector3(Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX(angle)) * magnitude, Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(angle)) * magnitude, 0.0f);
      }

      public static Color LerpColourFromArray(float t, Color[] colours, int count)
      {
        if ((double) t >= 1.0)
          return colours[count - 1];
        if ((double) t <= 0.0 || count == 1)
          return colours[0];
        int index = (int) ((double) t * (double) (count - 1));
        float amount = (float) System.Math.IEEERemainder((double) t * (double) (count - 1), 1.0);
        return Color.Lerp(colours[index + 1], colours[index], amount);
      }
    }
}
