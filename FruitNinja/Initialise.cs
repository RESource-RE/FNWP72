// Decompiled with JetBrains decompiler
// Type: FruitNinja.Initialise
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    internal class Initialise
    {
      private static string[] modeNames = new string[5]
      {
        "CLASSIC",
        "CASINO",
        "ARCADE",
        "ZEN",
        "ALL"
      };
      private static uint[] names = new uint[4]
      {
        StringFunctions.StringHash("CLASSIC"),
        StringFunctions.StringHash("CASINO"),
        StringFunctions.StringHash("ARCADE"),
        StringFunctions.StringHash("ZEN")
      };

      public static Vector3 ScreenPos(float x, float y, Vector3 anchor)
      {
        return Initialise.ScreenPos(x, y, anchor, false);
      }

      public static Vector3 ScreenPos(float x, float y, Vector3 anchor, bool splitscreen)
      {
        return new Vector3(x, y, 0.0f) + new Vector3(Game.SCREEN_WIDTH, Game.SCREEN_HEIGHT, 0.0f) * anchor;
      }

      public static Vector3 ScreenPos(Vector3 pos, Vector3 anchor)
      {
        return Initialise.ScreenPos(pos, anchor, false);
      }

      public static Vector3 ScreenPos(Vector3 pos, Vector3 anchor, bool splitscreen)
      {
        return pos + new Vector3(Game.SCREEN_WIDTH, Game.SCREEN_HEIGHT, 0.0f) * anchor;
      }

      public static string GetModeName(Game.GAME_MODE mode) => Initialise.modeNames[(int) mode];

      public static Game.GAME_MODE ParseGameMode(uint hash)
      {
        for (int gameMode = 0; gameMode < Initialise.names.Length; ++gameMode)
        {
          if ((int) Initialise.names[gameMode] == (int) hash)
            return (Game.GAME_MODE) gameMode;
        }
        return Game.GAME_MODE.GM_MAX;
      }

      public static uint GetModeBitMask(Game.GAME_MODE mode)
      {
        return mode == Game.GAME_MODE.GM_MAX ? uint.MaxValue : 1U << (int) (mode & (Game.GAME_MODE) 31 /*0x1F*/);
      }
    }
}
