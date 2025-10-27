// Decompiled with JetBrains decompiler
// Type: FruitNinja.FruitGame
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace FruitNinja
{

    public class FruitGame
    {
      public void Init(uint instance, string startUpCommandLine) => Game.GameInitialise(instance);

      public void End()
      {
        Game.GameTaskExit();
        Game.GameDestroy();
      }

      public void Update(float timeSinceLastUpdate) => Game.GameTaskUpdate(timeSinceLastUpdate);

      public void Draw(float timeSinceLastUpdate) => Game.GameTaskDraw(timeSinceLastUpdate);

      public void Paused() => GameTask.SkipToPause(false);

      public void UnPaused()
      {
        if ((double) Game.game_work.gameOverTransition == 0.0)
          return;
        GameTask.UnpauseGame();
      }

      public void LoadContent()
      {
      }

      public string SelfVersion() => "1.21";
    }
}
