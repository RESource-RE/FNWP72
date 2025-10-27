// Decompiled with JetBrains decompiler
// Type: FruitNinja.Leaderboards
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;

namespace FruitNinja
{

    internal class Leaderboards
    {
      private const int ENTRIES_TO_READ = 25;
      public const int LEADERBOARD_TYPE_CLASSIC = 0;
      public const int LEADERBOARD_TYPE_ZEN = 1;
      public const int LEADERBOARD_TYPE_ARCADE = 2;
      public const int LEADERBOARD_TYPE_TOTAL_SCORES = 3;
      public const int LEADERBOARD_TYPE_TOTAL_FRUIT = 4;
      public const int LEADERBOARD_RESULT_NO_PLAYER = 0;
      public const int LEADERBOARD_RESULT_TIMED_OUT = 1;
      public const int LEADERBOARD_RESULT_SUCCESS = 2;
      private static int gameMode;
      private static Leaderboards.ReadFinishedEventHandler notifier;
      private static Leaderboards.ReadMode mode = Leaderboards.ReadMode.None;

      public static bool StartRead(int leaderboard, Leaderboards.ReadFinishedEventHandler callback)
      {
        return false;
      }

      public static void Write(int mode, long value)
      {
      }

      private static void LeaderboardReadCallback(IAsyncResult result)
      {
      }

      public static void PageUp()
      {
      }

      public static void PageDown()
      {
      }

      public delegate void ReadFinishedEventHandler(int result);

      private enum ReadMode
      {
        None,
        Reading,
      }
    }
}
