// Decompiled with JetBrains decompiler
// Type: FruitNinja.TransitionFunctions
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    internal class TransitionFunctions
    {
      public static float SinTransition(float amt, float full)
      {
        return Math.SinIdx((ushort) ((double) amt * (double) Math.DEGREE_TO_IDX(full))) / Math.SinIdx(Math.DEGREE_TO_IDX(full));
      }

      public static float SquareTransition(float amt, float parameter) => amt * amt;

      public static float InverseSquareTransition(float amt)
      {
        return TransitionFunctions.InverseSquareTransition(amt, 0.0f);
      }

      public static float InverseSquareTransition(float amt, float full)
      {
        amt = 1f - amt;
        amt *= amt;
        return 1f - amt;
      }

      public static float FullTransition(float amt, float parameter) => 1f;

      public static float StraightTransition(float amt, float parameter)
      {
        return Math.CLAMP(amt * parameter, 0.0f, 1f);
      }

      public static float SinPulse(float amt, float parameter)
      {
        return Math.SinIdx((ushort) ((double) amt * 32768.0 * (double) parameter));
      }

      public static float InAndOut(float amt, float parameter)
      {
        if ((double) amt < (double) parameter)
          return amt / parameter;
        if ((double) amt <= 1.0 - (double) parameter)
          return 1f;
        amt = 1f - amt;
        return amt / parameter;
      }

      public static float SinInAndOut(float amt, float parameter)
      {
        return TransitionFunctions.SinTransition(TransitionFunctions.InAndOut(amt, parameter), 115f);
      }

      public static float JumpyPulse(float amt, float parameter)
      {
        return (double) amt <= (double) parameter ? Math.SinIdx((ushort) ((double) amt / (double) parameter * 32768.0)) : (float) (-(double) Math.SinIdx((ushort) (((double) amt - (double) parameter) / (1.0 - (double) parameter) * 32768.0)) * 0.20000000298023224);
      }

      public static float JumpySinPulse(float amt, float parameter)
      {
        return Math.SinIdx((ushort) ((double) parameter * 0.5 * (double) parameter * (((double) amt + 1.0) * ((double) amt + 1.0) * ((double) amt + 1.0) - 1.0) / 7.0 * 32768.0)) * (1f - amt);
      }

      public static float GetProgressBetween(float time, float start, float end)
      {
        return TransitionFunctions.GetProgressBetween(time, start, end, false);
      }

      public static float GetProgressBetween(float time, float start, float end, bool clamp)
      {
        return (double) start == (double) end ? ((double) time >= (double) start ? 1f : 0.0f) : (!clamp ? (float) (((double) time - (double) start) / ((double) end - (double) time)) : Math.CLAMP((float) (((double) time - (double) start) / ((double) end - (double) start)), 0.0f, 1f));
      }

      public static Vector3 LerpF(Vector3 start, Vector3 end, float amt) => start + (end - start) * amt;

      public static float LerpF(float start, float end, float amt) => start + (end - start) * amt;
    }
}
