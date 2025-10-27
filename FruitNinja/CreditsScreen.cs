// Decompiled with JetBrains decompiler
// Type: FruitNinja.CreditsScreen
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mortar;

namespace FruitNinja
{

    public class CreditsScreen : HUDControl3d
    {
      protected float m_time;
      protected Vector3 m_originalScale;
      protected MenuButton m_quitButton;
      protected static Texture s_boardTexture;
      protected static Texture m_creditsTexture;
      protected static Texture m_senseiTexture;
      public DojoScreen m_dojoScreen;
      private int m_state;
      private static float sx = 38f;
      private static float sy = 80f;

      public static int SENSEI_CENTRE_X => 424;

      public static int SENSEI_CENTRE_Y => 104;

      public static int CREDITS_CENTRE_X => 180;

      public static int CREDITS_CENTRE_Y => 160 /*0xA0*/;

      public static int ABOUT_CENTRE_X => 190;

      public static int ABOUT_CENTRE_Y => 97;

      public static float ABOUT_SCREEN_HEIGHT => 320f;

      public static string VERSION_TITLE => "VERSION:";

      public CreditsScreen(DojoScreen dojo)
      {
        this.m_dojoScreen = dojo;
        this.m_texture = CreditsScreen.s_boardTexture;
        this.m_selfCleanUp = false;
        this.m_quitButton = (MenuButton) null;
        this.m_state = 0;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        this.m_time = 0.0f;
      }

      public void QuitGameCallback()
      {
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
        this.m_state = 2;
        ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
        this.m_quitButton.m_entity.m_vel = new Vector3(Math.g_random.RandF(5f) + 5f, -Math.g_random.RandF(5f), 0.0f);
        Game.game_work.tutorialControl.ResetTutePos();
      }

      public static void LoadContent()
      {
        CreditsScreen.m_creditsTexture = TextureManager.GetInstance().Load("credits_screen.tex", true);
        CreditsScreen.m_senseiTexture = TextureManager.GetInstance().Load("textureswp7/sensei.tex");
      }

      public static void UnLoadContent()
      {
        CreditsScreen.s_boardTexture = (Texture) null;
        CreditsScreen.m_creditsTexture = (Texture) null;
        CreditsScreen.m_senseiTexture = (Texture) null;
      }

      public override void Reset()
      {
      }

      public override void Release()
      {
        this.m_texture = (Texture) null;
        this.m_time = 0.0f;
      }

      public override void Init() => this.Reset();

      public override void Update(float dt)
      {
        switch (this.m_state)
        {
          case 0:
            this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
            if ((double) this.m_time <= 0.99900001287460327)
              break;
            this.m_time = 1f;
            this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
            this.m_quitButton.Init();
            this.m_quitButton.m_triggerOnBackPress = true;
            Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
            Game.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
            this.m_quitButton.m_originalScale *= 0.825f;
            this.m_quitButton.m_entity.m_cur_scale *= 0.825f;
            this.m_state = 1;
            break;
          case 1:
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed)
              break;
            this.QuitGameCallback();
            break;
          case 2:
            this.m_time *= 0.75f;
            if ((double) this.m_time >= 1.0 / 1000.0)
              break;
            this.m_dojoScreen.Reset();
            this.m_terminate = true;
            break;
        }
      }

      public override void Draw(float[] tintChannels)
      {
        if (this.m_texture != null)
        {
          this.m_texture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) ((double) this.m_texture.GetWidth() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) this.m_texture.GetHeight() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 1f));
          float num1 = (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 + (double) this.m_texture.GetHeight() * 0.5);
          float num2 = (float) ((double) CreditsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - (double) CreditsScreen.ABOUT_CENTRE_Y - 60.0);
          float y = num1 - (num1 - num2) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3((float) CreditsScreen.ABOUT_CENTRE_X - Game.SCREEN_WIDTH / 2f, y, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_texture.UnSet();
        }
        if (CreditsScreen.m_creditsTexture != null)
        {
          CreditsScreen.m_creditsTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3((float) (((double) CreditsScreen.m_creditsTexture.GetWidth() - (double) CreditsScreen.sx) * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) (((double) CreditsScreen.m_creditsTexture.GetHeight() - (double) CreditsScreen.sy) * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 1f));
          float num3 = (float) (-((double) Game.SCREEN_HEIGHT / 2.0) - (double) CreditsScreen.m_creditsTexture.GetHeight() * 0.5 * (double) Game.GAME_MODE_SCALE_FIX);
          float num4 = (float) -((double) Game.SCREEN_HEIGHT / 2.0) + (float) (320 - CreditsScreen.CREDITS_CENTRE_Y);
          float y = num3 - (num3 - num4) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3((float) CreditsScreen.CREDITS_CENTRE_X - Game.SCREEN_WIDTH / 2f, y, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          CreditsScreen.m_creditsTexture.UnSet();
        }
        if (CreditsScreen.m_senseiTexture == null)
          return;
        CreditsScreen.m_senseiTexture.Set();
        MatrixManager.GetInstance().Reset();
        MatrixManager.GetInstance().Scale(new Vector3((float) ((double) CreditsScreen.m_senseiTexture.GetWidth() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), (float) ((double) CreditsScreen.m_senseiTexture.GetHeight() * (double) Game.GAME_MODE_SCALE_FIX + 1.0), 1f));
        float num5 = (float) ((double) CreditsScreen.m_senseiTexture.GetWidth() * 0.5 + (double) Game.SCREEN_WIDTH / 2.0);
        float num6 = (float) CreditsScreen.SENSEI_CENTRE_X - Game.SCREEN_WIDTH / 2f;
        float x = num5 - (num5 - num6) * this.m_time;
        MatrixManager.GetInstance().Translate(new Vector3(x, CreditsScreen.ABOUT_SCREEN_HEIGHT / 2f - (float) CreditsScreen.SENSEI_CENTRE_Y, 0.0f));
        MatrixManager.GetInstance().UploadCurrentMatrices();
        Mesh.DrawQuad(Color.White);
        CreditsScreen.m_senseiTexture.UnSet();
      }

      public enum AS
      {
        AS_IN,
        AS_WAIT,
        AS_OUT,
      }
    }
}
