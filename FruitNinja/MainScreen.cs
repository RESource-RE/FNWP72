// Decompiled with JetBrains decompiler
// Type: FruitNinja.MainScreen
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Mortar;
using System;
using System.Collections.Generic;

namespace FruitNinja
{

    public class MainScreen : HUDControl3d
    {
      protected float m_time;
      protected Vector3 m_originalScale;
      protected Mortar.Texture s_newGameTex;
      protected Mortar.Texture s_dojoTex;
      protected Mortar.Texture s_helpAndSettingsTex;
      protected Mortar.Texture s_moreGamesTex;
      protected Mortar.Texture s_leaderboardsTex;
      protected MenuButton m_achivementsButton;
      protected MenuButton m_gamerProfileButton;
      protected MenuButton m_helpOptionsButton;
      protected MenuButton m_leaderboardsButton;
      protected MenuButton m_marketplaceButton;
      protected MenuButton m_purchaseButton;
      protected Texture2D m_live;
      protected MenuButton m_newGame;
      protected MenuButton m_helpAndSettingsFruit;
      protected MenuButton m_dojoButton;
      protected MenuButton m_moreGames;
      protected MenuButton m_multiplayerGame;
      protected MenuButton m_sound;
      protected MenuButton m_music;
      protected Mortar.Texture m_leaderBoardComingSoonTexture;
      protected Vector3 m_comingSoonPos;
      protected Mortar.Texture[] m_musicTextures = new Mortar.Texture[2];
      protected Mortar.Texture[] m_soundTextures = new Mortar.Texture[2];
      protected Mortar.Texture m_fruitTex;
      protected Mortar.Texture m_ninjaTex;
      protected Mortar.Model m_backing;
      protected Mortar.Texture m_tuteTex;
      protected Vector3 m_tutePos;
      protected float m_tuteTime;
      protected Vector3 m_fruitPos;
      protected Vector3 m_ninjaPos;
      protected float m_ninjaGrav;
      public MainScreen.MS m_state;
      public float m_transitionWait;
      private static float tute = 1f;
      private static float liveX;
      private static float liveY;
      private static bool madeTris = false;
      private static VertexPositionColorTexture[] top_tri = new VertexPositionColorTexture[3];

      public static float BEGINNING_WAIT => 0.15f;

      public static float BEGINNING_SCALE => 2f;

      public static float NORMAL_SCALE => 1f;

      public static float SLIDE_IN_TIME => 0.2f;

      public static float POP_SINE => (float) Mortar.Math.DEGREE_TO_IDX(110f);

      public static float TOP_BUTTONS_Y => (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 24.5);

      public static Vector3 TUTE_DIRECTION => new Vector3(-120f, -17f, 0.0f);

      public static Vector3 TUTE_POS
      {
        get
        {
          return new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 + 65.0), (float) ((double) Game.SCREEN_HEIGHT / 2.0 - 134.0), 0.0f);
        }
      }

      public static float BOMB_FLASH_FULL => 1.55f;

      public static float OVERALL_BUTTON_SCALE => 1f;

      public static Vector3 HELP_AND_SETTINGS_POS
      {
        get => new Vector3((float) (403.0 - (double) Game.SCREEN_WIDTH / 2.0), -45f, 0.0f);
      }

      public static Vector3 NEW_GAME_POS
      {
        get => new Vector3((float) (244.0 - (double) Game.SCREEN_WIDTH / 2.0), -66f, 0.0f);
      }

      public static Vector3 DOJO_POS
      {
        get => new Vector3((float) (77.0 - (double) Game.SCREEN_WIDTH / 2.0), -87f, 0.0f);
      }

      public static Vector3 LEADERBOARD_BUTTON_POS
      {
        get => new Vector3((float) (388.0 - (double) Game.SCREEN_WIDTH / 2.0), 7f, 0.0f);
      }

      public static Vector3 MORE_GAMES_POS
      {
        get => new Vector3((float) (422.0 - (double) Game.SCREEN_WIDTH / 2.0), -106f, 0.0f);
      }

      public MainScreen()
      {
        this.m_fruitTex = TextureManager.GetInstance().Load("textureswp7/fruit_text.tex");
        this.m_ninjaTex = TextureManager.GetInstance().Load("textureswp7/ninja_text.tex");
        this.m_tuteTex = TextureManager.GetInstance().Load("slice_fruit.tex", true);
        this.m_backing = (Mortar.Model) null;
        this.s_newGameTex = TextureManager.GetInstance().Load("newgame.tex", true);
        this.s_helpAndSettingsTex = TextureManager.GetInstance().Load("menu_help_ring.tex", true);
        this.s_dojoTex = TextureManager.GetInstance().Load("textureswp7/dojo_icon.tex");
        this.m_moreGames = (MenuButton) null;
        this.m_originalScale = this.m_scale = new Vector3(480f, 138f, 1f);
        this.m_state = MainScreen.MS.MS_IN;
        this.m_selfCleanUp = false;
        this.m_time = 0.0f;
        this.m_sound = (MenuButton) null;
        this.m_music = (MenuButton) null;
        this.m_newGame = (MenuButton) null;
        this.m_multiplayerGame = (MenuButton) null;
        this.m_dojoButton = (MenuButton) null;
        this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT - (double) this.m_scale.Y) * 0.5), 0.0f);
        this.m_tuteTime = 1f;
        this.m_ninjaPos.Y = Game.SCREEN_HEIGHT / 2f + (float) (this.m_ninjaTex.GetHeight() / 2U);
        this.m_ninjaGrav = 0.0f;
        this.m_transitionWait = 0.0f;
        this.m_terminate = false;
        for (int index = 0; index < 2; ++index)
        {
          this.m_soundTextures[index] = (Mortar.Texture) null;
          this.m_musicTextures[index] = (Mortar.Texture) null;
        }
      }

      public void NewGameCallback()
      {
        this.m_state = MainScreen.MS.MS_OUT;
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_DANANANA_SCHWING);
        if (this.m_newGame == null)
          return;
        Mortar.Math.g_random.Seed((uint) ((double) int.MinValue * (double) this.m_newGame.m_rotation));
      }

      public void LeaderboardsCallback()
      {
      }

      public void MoreGamesCallback() => this.m_state = MainScreen.MS.MS_IN;

      public void AboutCallback()
      {
        this.m_state = MainScreen.MS.MS_DOJO;
        this.m_time = 1f;
        Game.game_work.tutorialControl.ResetTutePos();
      }

      private void NotConnected(IAsyncResult result)
      {
      }

      public void MarketplaceCallback()
      {
      }

      public void ShowCardCallback()
      {
      }

      public void AchievementCallback()
      {
      }

      public void BlankCallback()
      {
      }

      public void GameModeCallback()
      {
        this.m_state = MainScreen.MS.MS_GAME_MODE;
        this.m_time = 1f;
        Game.game_work.tutorialControl.ResetTutePos();
        if (this.m_newGame == null)
          return;
        Mortar.Math.g_random.Seed((uint) ((double) int.MinValue * (double) this.m_newGame.m_rotation));
      }

      public void HelpAndOptionsCallback()
      {
        this.m_state = MainScreen.MS.MS_ABOUT;
        this.m_time = 1f;
        Game.game_work.tutorialControl.ResetTutePos();
      }

      private static void MuteCallback(IAsyncResult result)
      {
        if (false)
          return;
        Game.game_work.musicEnabled = !Game.game_work.musicEnabled;
        try
        {
          MediaPlayer.IsMuted = !Game.game_work.musicEnabled;
        }
        catch
        {
        }
      }

      public void MusicCallback()
      {
        if (Game.game_work.musicEnabled && SoundManager.GetInstance().CustomMusic)
        {
          TheGame.instance.stringTable.GetString(926);
          TheGame.instance.stringTable.GetString(931);
          try
          {
            MainScreen.MuteCallback((IAsyncResult) null);
          }
          catch
          {
          }
        }
        else
        {
          Game.game_work.musicEnabled = !Game.game_work.musicEnabled;
          try
          {
            MediaPlayer.IsMuted = !Game.game_work.musicEnabled;
          }
          catch
          {
          }
        }
      }

      public void SoundCallback()
      {
        Game.game_work.soundEnabled = !Game.game_work.soundEnabled;
        SoundManager.GetInstance().SetSFXVolume(Game.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
      }

      public void MultiplayerGameModeCallback()
      {
      }

      public void ClickedCallback() => throw new MissingMethodException();

      public override void Reset()
      {
      }

      public override void Release()
      {
        if (this.m_dojoButton != null)
          this.m_dojoButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(HUDControl.DefaultDeleteCallback);
        this.m_texture = (Mortar.Texture) null;
        this.m_backing = (Mortar.Model) null;
        for (int index = 0; index < 2; ++index)
        {
          this.m_soundTextures[index] = (Mortar.Texture) null;
          this.m_musicTextures[index] = (Mortar.Texture) null;
        }
        this.s_newGameTex = (Mortar.Texture) null;
        this.s_dojoTex = (Mortar.Texture) null;
        this.s_helpAndSettingsTex = (Mortar.Texture) null;
        this.s_moreGamesTex = (Mortar.Texture) null;
        this.s_leaderboardsTex = (Mortar.Texture) null;
      }

      public override void Init() => this.Reset();

      private string YES => TheGame.instance.stringTable.GetString(931);

      private string NO => TheGame.instance.stringTable.GetString(926);

      private void ExitGameCallback(IAsyncResult result)
      {
        bool flag = false;
        int? nullable = Guide.EndShowMessageBox(result);
        if (nullable.HasValue && nullable.HasValue && nullable.Value == 1)
          flag = true;
        if (!flag)
          return;
        if (Game.isWP7TrialMode())
        {
          TheGame.instance.DoUpsell(true);
        }
        else
        {
          try
          {
            MediaPlayer.IsMuted = false;
          }
          catch
          {
          }
          TheGame.exitingGame = true;
          TheGame.instance.Exit();
        }
      }

      public override void Update(float dt)
      {
        float num1 = -Game.game_work.gameOverTransition;
        Vector3 vector3 = new Vector3(128f, 40f, 1f);
        MenuButton gamerProfileButton1 = this.m_gamerProfileButton;
        Game.isWP7TrialMode();
        if (this.m_sound == null)
        {
          this.m_sound = new MenuButton("sound.tex", new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 24.0), MainScreen.TOP_BUTTONS_Y, 0.0f), new MenuButton.MenuCallback(this.SoundCallback), -1, new Vector3(32f, 32f, 1f), false);
          this.m_sound.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_sound);
          this.m_soundTextures[0] = this.m_sound.m_texture;
          this.m_soundTextures[1] = TextureManager.GetInstance().Load("textureswp7/sound_cross.tex");
          this.m_sound.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (this.m_music == null)
        {
          this.m_music = new MenuButton("music.tex", new Vector3((float) ((double) Game.SCREEN_WIDTH / 2.0 - 24.0 - 40.0), MainScreen.TOP_BUTTONS_Y, 0.0f), new MenuButton.MenuCallback(this.MusicCallback), -1, new Vector3(32f, 32f, 1f), false);
          this.m_music.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_music);
          this.m_musicTextures[0] = this.m_music.m_texture;
          this.m_musicTextures[1] = TextureManager.GetInstance().Load("textureswp7/music_cross.tex");
          this.m_music.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        this.m_music.m_texture = this.m_musicTextures[Game.game_work.musicEnabled ? 0 : 1];
        this.m_sound.m_texture = this.m_soundTextures[Game.game_work.soundEnabled ? 0 : 1];
        switch (this.m_state)
        {
          case MainScreen.MS.MS_IN:
            Game.game_work.gameMode = Game.GAME_MODE.GM_CLASSIC;
            TheGame.pomegranateThrown = false;
            if (this.m_achivementsButton != null)
              this.m_achivementsButton.SetActive(true);
            if (this.m_gamerProfileButton != null)
              this.m_gamerProfileButton.SetActive(true);
            if (this.m_helpOptionsButton != null)
              this.m_helpOptionsButton.SetActive(true);
            if (this.m_leaderboardsButton != null)
              this.m_leaderboardsButton.SetActive(true);
            if (this.m_marketplaceButton != null)
              this.m_marketplaceButton.SetActive(true);
            if (this.m_purchaseButton != null)
              this.m_purchaseButton.SetActive(true);
            this.m_moreGames = (MenuButton) null;
            if ((double) this.m_transitionWait > 0.0 || (double) Game.game_work.hitBombTime > (double) MainScreen.BOMB_FLASH_FULL - 0.10000000149011612)
            {
              this.m_transitionWait -= dt;
              Game.game_work.gameOverTransition += (float) ((-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
              if ((double) Game.game_work.gameOverTransition < 0.0)
                Game.game_work.gameOverTransition = 0.0f;
            }
            else
            {
              this.m_time += dt;
              if ((double) Game.game_work.gameOverTransition < -0.99900001287460327)
                Game.game_work.gameOverTransition = -1f;
              else
                Game.game_work.gameOverTransition += (float) ((-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
            }
            if ((double) this.m_time > (double) MainScreen.BEGINNING_WAIT && (double) Game.game_work.gameOverTransition < 0.0)
            {
              this.m_state = MainScreen.MS.MS_WAIT;
              this.m_helpAndSettingsFruit = new MenuButton(this.s_helpAndSettingsTex, MainScreen.HELP_AND_SETTINGS_POS, new MenuButton.MenuCallback(this.HelpAndOptionsCallback), Fruit.FruitType("pear", true));
              this.m_helpAndSettingsFruit.Init();
              Game.game_work.hud.AddControl((HUDControl) this.m_helpAndSettingsFruit);
              this.m_helpAndSettingsFruit.m_originalScale = new Vector3((float) (this.m_helpAndSettingsFruit.m_texture.GetWidth() + 1U), (float) (this.m_helpAndSettingsFruit.m_texture.GetHeight() + 1U), 1f) * Game.GAME_MODE_SCALE_FIX * MainScreen.OVERALL_BUTTON_SCALE;
              this.m_helpAndSettingsFruit.m_entity.m_cur_scale *= MainScreen.OVERALL_BUTTON_SCALE;
              this.m_helpAndSettingsFruit.m_overallScratchScale = 0.5f;
              this.m_helpAndSettingsFruit.m_texture.texture_filename = "menu_help_ring";
              this.m_newGame = new MenuButton(this.s_newGameTex, MainScreen.NEW_GAME_POS, new MenuButton.MenuCallback(this.GameModeCallback), 3);
              this.m_newGame.Init();
              Game.game_work.hud.AddControl((HUDControl) this.m_newGame);
              this.m_newGame.m_originalScale = new Vector3((float) (this.m_newGame.m_texture.GetWidth() + 1U), (float) (this.m_newGame.m_texture.GetHeight() + 1U), 1f) * Game.GAME_MODE_SCALE_FIX * MainScreen.OVERALL_BUTTON_SCALE;
              this.m_newGame.m_entity.m_cur_scale *= MainScreen.OVERALL_BUTTON_SCALE;
              this.m_newGame.m_overallScratchScale = 0.5f;
              this.m_newGame.m_texture.texture_filename = "newgame";
              Game.game_work.tutorialControl.ResetTutePos(this.m_newGame);
              this.m_dojoButton = new MenuButton(this.s_dojoTex, MainScreen.DOJO_POS, new MenuButton.MenuCallback(this.AboutCallback), Fruit.FruitType("mango"));
              this.m_dojoButton.Init();
              this.m_dojoButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
              this.m_dojoButton.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.ButtonDeleted);
              if (this.m_dojoButton.m_entity != null)
                this.m_dojoButton.m_entity.m_cur_scale *= 0.9f * MainScreen.OVERALL_BUTTON_SCALE;
              this.m_dojoButton.m_originalScale *= 1.05f * MainScreen.OVERALL_BUTTON_SCALE;
              this.m_dojoButton.m_clearOthers = true;
              this.m_dojoButton.m_texture.texture_filename = "dojo";
              Game.game_work.hud.AddControl((HUDControl) this.m_dojoButton);
              break;
            }
            break;
          case MainScreen.MS.MS_WAIT:
            if (!Guide.IsVisible && TheGame.instance.BackButtonWasPressed)
            {
              string[] buttons = new string[2]
              {
                this.NO,
                this.YES
              };
              Guide.BeginShowMessageBox(TheGame.instance.stringTable.GetString(929), TheGame.instance.stringTable.GetString(1009), (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.ExitGameCallback), (object) null);
              return;
            }
            if (this.m_dojoButton != null)
              this.m_dojoButton.SetNewSymbol(ItemManager.GetInstance().AreNewItems());
            MenuButton moreGames = this.m_moreGames;
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            if ((double) Game.game_work.gameOverTransition < -0.99900001287460327)
            {
              Game.game_work.gameOverTransition = -1f;
              break;
            }
            Game.game_work.gameOverTransition += (float) ((-1.0 - (double) Game.game_work.gameOverTransition) * 0.125);
            break;
          case MainScreen.MS.MS_OUT:
            if ((double) num1 > 0.99900001287460327)
            {
              Game.game_work.levelStartCoins = Game.game_work.coins;
              WaveManager.GetInstance().Reset();
              Game.game_work.gameOver = true;
            }
            Game.game_work.gameOverTransition *= 0.75f;
            if ((double) Mortar.Math.Abs(Game.game_work.gameOverTransition) < 1.0 / 1000.0)
            {
              Game.game_work.gameOverTransition = 0.0f;
              Game.game_work.gameOver = false;
              this.m_state = MainScreen.MS.MS_GAME;
            }
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
          case MainScreen.MS.MS_ABOUT:
          case MainScreen.MS.MS_DOJO:
            int numEntities1 = (int) ActorManager.GetInstance().GetNumEntities(0);
            this.m_time *= 0.75f;
            if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
            {
              this.m_time = 0.0f;
              HUDControl control = this.m_state != MainScreen.MS.MS_ABOUT ? (HUDControl) new DojoScreen() : (HUDControl) new AboutScreen(this);
              control.Init();
              Game.game_work.hud.AddControl(control);
            }
            num1 = this.m_time;
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
          case MainScreen.MS.MS_RETURN:
            if ((double) this.m_time > 0.99900001287460327)
            {
              num1 = 1f;
              this.m_time += dt;
              if ((double) this.m_time > 1.5)
              {
                this.m_time = MainScreen.BEGINNING_WAIT;
                this.m_transitionWait = 0.0f;
                this.m_state = MainScreen.MS.MS_IN;
              }
            }
            else
            {
              this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
              num1 = this.m_time;
            }
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
          case MainScreen.MS.MS_OPENFEINT:
          case MainScreen.MS.MS_OPENFEINT_GAMES:
            int numEntities2 = (int) ActorManager.GetInstance().GetNumEntities(0);
            this.m_time *= 0.75f;
            if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
            {
              this.m_time = 0.0f;
              LeaderboardsScreen control = new LeaderboardsScreen();
              control.Init();
              Game.game_work.hud.AddControl((HUDControl) control);
            }
            num1 = this.m_time;
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
          case MainScreen.MS.MS_GAME_MODE:
          case MainScreen.MS.MS_GAME_MODE_MP:
            float time = this.m_time;
            this.m_time *= 0.85f;
            if ((double) time > 0.25 && (double) this.m_time <= 0.25)
            {
              this.m_time = 0.0f;
              GameModeScreen control = new GameModeScreen(false);
              control.Init();
              Game.game_work.hud.AddControl((HUDControl) control);
              if (this.m_achivementsButton != null)
                this.m_achivementsButton.SetActive(false);
              if (this.m_gamerProfileButton != null)
                this.m_gamerProfileButton.SetActive(false);
              if (this.m_helpOptionsButton != null)
                this.m_helpOptionsButton.SetActive(false);
              if (this.m_leaderboardsButton != null)
                this.m_leaderboardsButton.SetActive(false);
              if (this.m_marketplaceButton != null)
                this.m_marketplaceButton.SetActive(false);
              if (this.m_purchaseButton != null)
                this.m_purchaseButton.SetActive(false);
              GC.Collect();
            }
            num1 = this.m_time;
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
          case MainScreen.MS.MS_ACHIEVEMENTS:
            int numEntities3 = (int) ActorManager.GetInstance().GetNumEntities(0);
            this.m_time *= 0.75f;
            if ((double) this.m_time != 0.0 && (double) this.m_time < 1.0 / 1000.0)
            {
              this.m_time = 0.0f;
              AchievementsScreen control = new AchievementsScreen();
              control.Init();
              Game.game_work.hud.AddControl((HUDControl) control);
            }
            num1 = this.m_time;
            this.m_pos = new Vector3(0.0f, (float) (((double) Game.SCREEN_HEIGHT + (double) this.m_scale.Y - (double) this.m_scale.Y * (double) num1 * 2.0) * 0.5), 0.0f);
            break;
        }
        if (this.m_state != MainScreen.MS.MS_WAIT && this.m_state != MainScreen.MS.MS_IN && this.m_newGame != null && (double) this.m_newGame.m_scale.X < 45.0 * (double) MainScreen.OVERALL_BUTTON_SCALE)
          this.m_newGame = (MenuButton) null;
        if (this.m_state != MainScreen.MS.MS_WAIT || this.m_moreGames != null && this.m_moreGames.m_entity == null && (double) this.m_moreGames.m_scale.X < 45.0)
          this.m_moreGames = (MenuButton) null;
        dt = Mortar.Math.MIN(dt, 0.04f);
        this.m_fruitPos.X = (float) (-(double) Game.SCREEN_WIDTH / 4.0);
        this.m_fruitPos.Y = this.m_pos.Y + 18f;
        this.m_fruitPos.Z = this.m_ninjaPos.Z = 0.0f;
        this.m_ninjaPos.X = this.m_fruitPos.X + 180f;
        this.m_ninjaGrav -= dt * 55f;
        this.m_ninjaPos.Y += (float) ((double) this.m_ninjaGrav * (double) dt * 15.0);
        this.m_tutePos = this.m_fruitPos;
        if ((double) dt > 0.0)
          MainScreen.tute = 1f;
        if ((double) this.m_ninjaPos.Y < (double) this.m_fruitPos.Y - 15.0)
        {
          this.m_ninjaPos.Y = this.m_fruitPos.Y - 15f;
          this.m_ninjaGrav *= -0.25f;
          if ((double) Mortar.Math.Abs(this.m_ninjaGrav) < 5.0 && (double) num1 > 0.99000000953674316 && (double) dt > 0.0)
            MainScreen.tute = 0.0f;
        }
        this.m_tuteTime += (float) (((double) MainScreen.tute - (double) this.m_tuteTime) * 0.25);
        this.m_tutePos = MainScreen.TUTE_POS + MainScreen.TUTE_DIRECTION * this.m_tuteTime * 2f;
        if (this.m_sound != null && this.m_music != null)
        {
          this.m_sound.m_pos.X = -180f;
          this.m_music.m_pos.X = -140f;
          this.m_sound.SetActive(true);
          this.m_music.SetActive(true);
          this.m_sound.m_pos.Y = AboutScreen.AboutScreenTime;
          this.m_music.m_pos.Y = AboutScreen.AboutScreenTime;
        }
        float num2 = (float) (340.0 + (double) num1 * -170.0);
        float num3 = 30f;
        float num4 = -50f;
        if ((double) GameTask.GetPauseAmount() != 0.0)
          return;
        MenuButton gamerProfileButton2 = this.m_gamerProfileButton;
        if (this.m_live != null)
        {
          MainScreen.liveY = 12f;
          MainScreen.liveX = (float) (130.0 + (double) num2 * 2.0);
        }
        if (this.m_achivementsButton != null)
        {
          if (Game.isWP7TrialMode())
          {
            this.m_achivementsButton.m_pos.Y = -12333f;
            this.m_achivementsButton.m_pos.X = -12333f;
          }
          else
          {
            this.m_achivementsButton.m_pos.Y = num3;
            this.m_achivementsButton.m_pos.X = num2;
            num3 += num4;
          }
        }
        if (this.m_leaderboardsButton != null)
        {
          if (Game.isWP7TrialMode())
          {
            this.m_leaderboardsButton.m_pos.Y = -12333f;
            this.m_leaderboardsButton.m_pos.X = -12333f;
          }
          else
          {
            this.m_leaderboardsButton.m_pos.Y = num3;
            this.m_leaderboardsButton.m_pos.X = num2;
            num3 += num4;
          }
        }
        if (this.m_helpOptionsButton != null)
        {
          this.m_helpOptionsButton.m_pos.Y = num3;
          this.m_helpOptionsButton.m_pos.X = num2;
          num3 += num4;
        }
        if (!Game.isWP7TrialMode() && this.m_marketplaceButton != null)
        {
          this.m_marketplaceButton.m_pos.Y = num3;
          this.m_marketplaceButton.m_pos.X = num2;
          num3 += num4;
        }
        if (this.m_purchaseButton == null)
          return;
        if (!Game.isWP7TrialMode())
        {
          this.m_purchaseButton.m_pos.X = -12333f;
        }
        else
        {
          this.m_purchaseButton.m_pos.Y = num3;
          this.m_purchaseButton.m_pos.X = num2;
        }
        float num5 = num3 + num4;
      }

      private void ReadFinished(int result)
      {
      }

      public override void Draw(float[] tintChannels)
      {
        if (this.m_state == MainScreen.MS.MS_GAME || (this.m_state == MainScreen.MS.MS_ABOUT || this.m_state == MainScreen.MS.MS_DOJO) && (double) this.m_time == 0.0)
          return;
        if (!MainScreen.madeTris)
        {
          MainScreen.madeTris = true;
          Color color = new Color(0, 0, 0, 128 /*0x80*/);
          for (int index = 0; index < 3; ++index)
          {
            MainScreen.top_tri[index].Position.X = MainScreen.top_tri[index].Position.Y = MainScreen.top_tri[index].Position.Z = 0.0f;
            MainScreen.top_tri[index].Color = color;
            MainScreen.top_tri[index].TextureCoordinate.X = 0.5f;
            MainScreen.top_tri[index].TextureCoordinate.Y = 0.5f;
          }
          MainScreen.top_tri[0].Position.X = -0.5f;
          MainScreen.top_tri[0].Position.Y = -0.5f;
          MainScreen.top_tri[1].Position.X = 3.5f;
          MainScreen.top_tri[1].Position.Y = 1f;
          MainScreen.top_tri[2].Position.X = -0.5f;
          MainScreen.top_tri[2].Position.Y = 1f;
        }
        if (this.m_fruitTex != null)
        {
          this.m_fruitTex.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(this.m_scale);
          MatrixManager.GetInstance().Translate(this.m_pos);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawTriListEx(MainScreen.top_tri, 3, true, 0);
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) this.m_fruitTex.GetWidth(), (float) this.m_fruitTex.GetHeight(), 0.0f) * Game.GAME_MODE_SCALE_FIX * 0.85f);
          MatrixManager.GetInstance().Translate(this.m_fruitPos);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_fruitTex.UnSet();
        }
        if (this.m_ninjaTex != null)
        {
          this.m_ninjaTex.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) this.m_ninjaTex.GetWidth(), (float) this.m_ninjaTex.GetHeight(), 0.0f) * Game.GAME_MODE_SCALE_FIX);
          MatrixManager.GetInstance().Translate(this.m_ninjaPos);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_ninjaTex.UnSet();
        }
        if (this.m_tuteTex != null)
        {
          this.m_tuteTex.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) this.m_tuteTex.GetWidth(), (float) this.m_tuteTex.GetHeight(), 0.0f) * Game.GAME_MODE_SCALE_FIX);
          MatrixManager.GetInstance().Translate(this.m_tutePos);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_tuteTex.UnSet();
        }
        if (this.m_leaderBoardComingSoonTexture == null || this.m_newGame == null)
          return;
        this.m_leaderBoardComingSoonTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3(0.5f, (float) (0.5 * ((double) this.m_leaderBoardComingSoonTexture.GetHeight() / (double) this.m_leaderBoardComingSoonTexture.GetWidth())), 1f) * this.m_newGame.m_scale.X);
        MatrixManager.GetInstance().Translate(MainScreen.LEADERBOARD_BUTTON_POS);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
        this.m_leaderBoardComingSoonTexture.UnSet();
      }

      public override void PreDraw(float[] tintChannels)
      {
      }

      public void Hide()
      {
        this.m_pos = new Vector3(-1000f, 1000f, -1000f);
        this.m_state = MainScreen.MS.MS_GAME;
      }

      public void ButtonDeleted(HUDControl control)
      {
        if (control != this.m_dojoButton)
          return;
        this.m_dojoButton = (MenuButton) null;
      }

      public enum MS
      {
        MS_IN,
        MS_WAIT,
        MS_OUT,
        MS_ABOUT,
        MS_DOJO,
        MS_RETURN,
        MS_OPENFEINT,
        MS_OPENFEINT_GAMES,
        MS_GAME_MODE,
        MS_GAME_MODE_MP,
        MS_GAME,
        MS_ACHIEVEMENTS,
      }
    }
}
