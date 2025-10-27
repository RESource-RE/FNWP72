// Decompiled with JetBrains decompiler
// Type: FruitNinja.LeaderboardsScreen
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortar;
using System;
using System.Collections.Generic;

namespace FruitNinja
{

    internal class LeaderboardsScreen : HUDControl3d
    {
      public const int GAME_MODE_WEEKLY = -2;
      public const int GAME_MODE_TOTAL_FRUIT = -1;
      protected float m_time;
      private static int MAX_PLAYERS = 25;
      private static int TIMEOUT_DELAY = 30000;
      private static int TABLE_Y = 138;
      private static int ADJUST = 50;
      private static int TAB1 = 118;
      private static int TAB3 = 510;
      private MenuButton m_modeClass;
      private MenuButton m_modeZen;
      private MenuButton m_modeArcade;
      private MenuButton m_modeWeekly;
      private MenuButton m_modeTotalFruit;
      private static Texture2D m_backing;
      private static Texture2D m_title;
      private static Mortar.Texture m_boxTexture;
      private static Mortar.Texture[] m_caption;
      private static Texture2D[] m_gamerPics;
      private static Texture2D m_live;
      private static Mortar.Texture m_noScoreThisWeek;
      private static float liveX;
      private static float liveY;
      private bool slide_on;
      private bool slide_off;
      private float slide_transition;
      private int gameMode;
      private int caption;
      private static int startWith = 0;
      private float prevTextOffset;
      private float textOffset;
      private float offset;
      private float snapY;
      private float tx;
      private float ty;
      private float initialX;
      private float initialY;
      private bool down;
      private float renderSize;
      private int m_timeout;
      private float panelY;
      private LeaderboardsScreen.ScrollState scrollState;
      private int m_state;
      private MenuButton m_quitButton;
      private List<LeaderboardsScreen.LeaderboardData> entries;
      private bool prevTouch;
      private bool currTouch;
      private float py;
      private float ssy;
      public static int bcx = 188;
      public static int bcy = 176 /*0xB0*/;
      public static int sx = 628;
      public static int sy = 344;

      public static int ABOUT_CENTRE_X => 190;

      public static int ABOUT_CENTRE_Y => 97;

      public static float ABOUT_SCREEN_HEIGHT => 320f;

      public LeaderboardsScreen()
      {
        this.m_selfCleanUp = false;
        this.m_quitButton = (MenuButton) null;
        this.m_state = 0;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        this.m_time = 0.0f;
        this.entries = new List<LeaderboardsScreen.LeaderboardData>();
      }

      public static void LoadContent()
      {
        if (LeaderboardsScreen.m_backing == null)
          LeaderboardsScreen.m_backing = TextureManager.GetInstance().Load("textureswp7/leaderboard_back.tex").intex;
        if (LeaderboardsScreen.m_title == null)
          LeaderboardsScreen.m_title = TextureManager.GetInstance().Load("textureswp7/leaderboard.tex").intex;
        LeaderboardsScreen.m_boxTexture = TextureManager.GetInstance().Load("leaderboard_box.tex", true);
        LeaderboardsScreen.m_caption = new Mortar.Texture[5];
        LeaderboardsScreen.m_caption[0] = TextureManager.GetInstance().Load("classic_title.tex", true);
        LeaderboardsScreen.m_caption[1] = TextureManager.GetInstance().Load("zen_title.tex", true);
        LeaderboardsScreen.m_caption[2] = TextureManager.GetInstance().Load("arcade_total_title.tex", true);
        LeaderboardsScreen.m_caption[3] = TextureManager.GetInstance().Load("arcade_weekly_title.tex", true);
        LeaderboardsScreen.m_caption[4] = TextureManager.GetInstance().Load("total_fruit_title.tex", true);
        LeaderboardsScreen.m_gamerPics = new Texture2D[LeaderboardsScreen.MAX_PLAYERS];
        LeaderboardsScreen.m_noScoreThisWeek = TextureManager.GetInstance().Load("no_score_this_week.tex", true);
      }

      public static void UnLoadContent()
      {
      }

      public override void Reset() => this.entries.Clear();

      public override void Release()
      {
        this.m_texture = (Mortar.Texture) null;
        this.m_time = 0.0f;
      }

      public override void Init()
      {
        this.partOfPopup = true;
        this.Reset();
        this.scrollState = LeaderboardsScreen.ScrollState.Idle;
        Vector3 scale = new Vector3(102.4f, 36f, 1f);
        if (this.m_modeClass == null)
        {
          this.m_modeClass = new MenuButton("classic_button.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Classic_Callback), -1, scale, true);
          this.m_modeClass.partOfPopup = true;
          this.m_modeClass.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_modeClass);
          this.m_modeClass.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (this.m_modeZen == null)
        {
          this.m_modeZen = new MenuButton("zen.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Zen_Callback), -1, scale, true);
          this.m_modeZen.partOfPopup = true;
          this.m_modeZen.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_modeZen);
          this.m_modeZen.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (this.m_modeArcade == null)
        {
          this.m_modeArcade = new MenuButton("arcade_total.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Arcade_Callback), -1, scale, true);
          this.m_modeArcade.partOfPopup = true;
          this.m_modeArcade.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_modeArcade);
          this.m_modeArcade.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (this.m_modeWeekly == null)
        {
          this.m_modeWeekly = new MenuButton("arcade_weekly.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Arcade_Weekly), -1, scale, true);
          this.m_modeWeekly.partOfPopup = true;
          this.m_modeWeekly.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_modeWeekly);
          this.m_modeWeekly.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        if (this.m_modeTotalFruit == null)
        {
          this.m_modeTotalFruit = new MenuButton("total_fruit.tex", new Vector3(0.0f, 0.0f, 0.0f), new MenuButton.MenuCallback(this.Arcade_TotalFruit), -1, scale, true);
          this.m_modeTotalFruit.partOfPopup = true;
          this.m_modeTotalFruit.Init();
          Game.game_work.hud.AddControl((HUDControl) this.m_modeTotalFruit);
          this.m_modeTotalFruit.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_POST;
        }
        this.slide_transition = 0.0f;
      }

      public void Classic_Callback()
      {
        if (this.m_state != 11 && this.m_state != 7 || this.slide_on || this.slide_off)
          return;
        this.entries.Clear();
        this.m_state = 4;
        this.slide_off = true;
        this.gameMode = 0;
        this.caption = 0;
        this.textOffset = 0.0f;
        this.offset = 0.0f;
        this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
        Leaderboards.StartRead(0, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
      }

      public void Zen_Callback()
      {
        if (this.m_state != 11 && this.m_state != 7 || this.slide_on || this.slide_off)
          return;
        this.entries.Clear();
        this.m_state = 4;
        this.gameMode = 3;
        this.slide_off = true;
        this.caption = 1;
        this.textOffset = 0.0f;
        this.offset = 0.0f;
        this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
        Leaderboards.StartRead(1, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
      }

      public void Arcade_Callback()
      {
        if (this.m_state != 11 && this.m_state != 7 || this.slide_on || this.slide_off)
          return;
        this.entries.Clear();
        this.m_state = 4;
        this.slide_off = true;
        this.gameMode = 2;
        this.caption = 2;
        this.textOffset = 0.0f;
        this.offset = 0.0f;
        this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
        Leaderboards.StartRead(2, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
      }

      public void Arcade_Weekly()
      {
        if (this.m_state != 11 && this.m_state != 7 || this.slide_on || this.slide_off)
          return;
        this.gameMode = -2;
        this.entries.Clear();
        this.m_state = 4;
        this.slide_off = true;
        this.caption = 3;
        this.textOffset = 0.0f;
        this.offset = 0.0f;
        this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
        Leaderboards.StartRead(3, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
      }

      public void Arcade_TotalFruit()
      {
        if (this.m_state != 11 && this.m_state != 7 || this.slide_on || this.slide_off)
          return;
        this.gameMode = -1;
        this.entries.Clear();
        this.m_state = 4;
        this.slide_off = true;
        this.caption = 4;
        this.textOffset = 0.0f;
        this.offset = 0.0f;
        this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
        Leaderboards.StartRead(4, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
      }

      public void Callback_PageUp()
      {
      }

      public void Callback_PageDown()
      {
      }

      private bool InTextBox()
      {
        return Mortar.Math.BETWEEN((int) this.tx, 132, 560) && Mortar.Math.BETWEEN((int) this.ty, 232, 448);
      }

      public override void Update(float dt)
      {
        if (this.m_state == 7 && !this.slide_on)
        {
          if (Touch.GetInstance().GetTouchCount() == 1)
          {
            Vector2 position = Touch.GetInstance().GetPosition();
            this.tx = position.X;
            this.ty = position.Y;
            this.currTouch = true;
          }
          else
            this.currTouch = false;
          if (this.prevTouch != this.currTouch)
          {
            this.prevTouch = this.currTouch;
            if (this.currTouch)
            {
              this.down = true;
              if (this.InTextBox())
              {
                this.initialX = this.tx;
                this.initialY = this.ty;
                this.textOffset = 0.0f;
                this.prevTextOffset = 0.0f;
                this.scrollState = LeaderboardsScreen.ScrollState.Scroll;
              }
            }
            else
              this.down = false;
          }
          if (this.down && this.scrollState == LeaderboardsScreen.ScrollState.Scroll && this.InTextBox() && (double) this.renderSize > 200.0)
          {
            float t = this.ty - this.py;
            if ((double) Mortar.Math.Abs(t) > 1.0 && (double) Mortar.Math.Abs(t) < 32.0)
              this.ssy = t;
            this.prevTextOffset = this.textOffset;
            this.textOffset = this.ty - this.initialY;
            if ((double) this.offset + (double) this.textOffset > 0.0)
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
            }
            if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - 200.0))
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
            }
            this.py = this.ty;
          }
          else
          {
            float num = 200f;
            if ((double) Mortar.Math.Abs(this.ssy) > 1.0)
            {
              this.prevTextOffset = this.textOffset;
              this.textOffset += this.ssy;
              this.ssy -= this.ssy / 8f;
              if ((double) this.offset + (double) this.textOffset > 0.0)
              {
                this.snapY = this.textOffset - this.prevTextOffset;
                this.textOffset = this.prevTextOffset;
                this.ssy = 0.0f;
              }
              if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - (double) num))
              {
                this.snapY = this.textOffset - this.prevTextOffset;
                this.textOffset = this.prevTextOffset;
                this.ssy = 0.0f;
              }
            }
            else
            {
              if (this.scrollState == LeaderboardsScreen.ScrollState.Scroll)
              {
                this.scrollState = LeaderboardsScreen.ScrollState.Idle;
                this.offset += this.textOffset;
                this.textOffset = 0.0f;
              }
              this.snapY -= this.snapY / 8f;
            }
          }
        }
        else
        {
          this.scrollState = LeaderboardsScreen.ScrollState.Idle;
          this.initialX = this.tx = -1f;
          this.initialY = this.ty = -1f;
          this.textOffset = 0.0f;
        }
        Game.game_work.tutorialControl.ResetTutePos();
        switch (this.m_state)
        {
          case 0:
            this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
            if ((double) this.m_time > 0.99900001287460327)
            {
              this.m_time = 1f;
              this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) ((double) LeaderboardsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
              this.m_quitButton.partOfPopup = true;
              this.m_quitButton.Init();
              this.m_quitButton.m_triggerOnBackPress = true;
              Game.game_work.hud.AddControl((HUDControl) this.m_quitButton);
              Game.game_work.tutorialControl.ResetTutePos(this.m_quitButton);
              this.m_quitButton.m_originalScale *= 0.825f;
              this.m_quitButton.m_entity.m_cur_scale *= 0.825f;
              this.m_state = 1;
              break;
            }
            break;
          case 1:
            this.m_state = 3;
            break;
          case 2:
            this.m_time *= 0.75f;
            if ((double) this.m_time < 1.0 / 1000.0 && !this.slide_off)
            {
              if (PopOverControl.IsInPopup)
                PopOverControl.Instance.Out();
              else
                Game.game_work.mainScreen.m_state = MainScreen.MS.MS_RETURN;
              this.m_terminate = true;
              break;
            }
            break;
          case 3:
            this.entries.Clear();
            this.m_state = 4;
            this.textOffset = 0.0f;
            this.offset = 0.0f;
            this.m_timeout = LeaderboardsScreen.TIMEOUT_DELAY;
            switch (LeaderboardsScreen.startWith)
            {
              case 0:
                this.gameMode = 0;
                this.caption = 0;
                Leaderboards.StartRead(0, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
                break;
              case 1:
                this.gameMode = 3;
                this.caption = 1;
                Leaderboards.StartRead(1, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
                break;
              case 2:
                this.gameMode = 2;
                this.caption = 2;
                Leaderboards.StartRead(2, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
                break;
              default:
                this.gameMode = 0;
                this.caption = 0;
                Leaderboards.StartRead(0, new Leaderboards.ReadFinishedEventHandler(this.ReadFinished));
                break;
            }
            LeaderboardsScreen.startWith = 0;
            break;
          case 4:
            this.m_timeout -= (int) ((double) dt * 1000.0);
            if (this.m_timeout < 0)
            {
              this.m_state = 6;
              break;
            }
            break;
          case 5:
            if (!this.slide_off)
            {
              this.renderSize = 0.0f;
              this.slide_on = true;
              this.m_state = 7;
              this.renderSize = (float) (50 * this.entries.Count);
              break;
            }
            break;
        }
        if (this.slide_on)
        {
          if ((double) this.slide_transition < -0.99900001287460327)
          {
            this.slide_transition = -1f;
            this.slide_on = false;
          }
          else
            this.slide_transition += (float) ((-1.0 - (double) this.slide_transition) * 0.125);
        }
        if (!this.slide_off)
          return;
        if ((double) Mortar.Math.Abs(this.slide_transition) < 1.0 / 1000.0)
        {
          this.slide_transition = 0.0f;
          this.slide_off = false;
        }
        else
          this.slide_transition += (float) ((0.0 - (double) this.slide_transition) * 0.125);
      }

      public static int BOX_CENTRE_X => LeaderboardsScreen.bcx;

      public static int BOX_CENTRE_Y => LeaderboardsScreen.bcy;

      public override void PreDraw(float[] tintChannels)
      {
      }

      public override void Draw(float[] tintChannels)
      {
        if (this.m_texture != null)
        {
          this.m_texture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3(440f * Game.GAME_MODE_SCALE_FIX, 220f * Game.GAME_MODE_SCALE_FIX, 1f));
          float num1 = (float) ((double) LeaderboardsScreen.ABOUT_SCREEN_HEIGHT / 2.0 + 110.0);
          float num2 = (float) ((double) LeaderboardsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - (double) LeaderboardsScreen.ABOUT_CENTRE_Y - 20.0);
          float y = num1 - (num1 - num2) * this.m_time;
          MatrixManager.GetInstance().Translate(new Vector3((float) LeaderboardsScreen.ABOUT_CENTRE_X - Game.SCREEN_WIDTH / 2f, y, 0.0f));
          MatrixManager.GetInstance().UploadCurrentMatrices();
          Mesh.DrawQuad(Color.White, 0.0f, 1f, 0.0f, 1f);
          this.m_texture.UnSet();
        }
        float y1 = 0.0f;
        if (LeaderboardsScreen.m_backing != null)
        {
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_backing, new Vector2(0.0f, 0.0f), Color.White);
          TheGame.instance.spriteBatch.End();
        }
        if (LeaderboardsScreen.m_boxTexture != null && this.m_state == 7)
        {
          float num3 = 480f;
          float num4 = 16f;
          float num5 = -512f;
          float num6 = 60f;
          y1 = num3 - (num3 - num4) * Mortar.Math.Abs(this.slide_transition);
          float x = num5 - (num5 - num6) * Mortar.Math.Abs(this.slide_transition);
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_boxTexture.intex, new Vector2(-196f, y1), Color.White);
          TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_caption[this.caption].intex, new Vector2(x, 0.0f), Color.White);
          if (LeaderboardsScreen.m_live != null)
          {
            LeaderboardsScreen.liveY = 64f;
            LeaderboardsScreen.liveX = (float) (320.0 + (double) x * 2.0);
            TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_live, new Vector2(LeaderboardsScreen.liveX, LeaderboardsScreen.liveY), Color.White);
          }
          this.panelY = y1;
          TheGame.instance.spriteBatch.End();
        }
        if (this.m_state == 7)
        {
          TheGame.instance.spriteBatch.Begin();
          float num7 = (float) (LeaderboardsScreen.TABLE_Y + 72);
          int num8 = 0;
          foreach (LeaderboardsScreen.LeaderboardData entry in this.entries)
          {
            bool flag = true;
            float y2 = num7 + y1 + this.textOffset + this.offset + this.snapY;
            if (!this.slide_on)
            {
              if ((double) y2 <= 188.0)
                flag = false;
              else if ((double) y2 >= 432.0)
                flag = false;
            }
            if (flag)
            {
              if (this.gameMode == -2)
              {
                if (entry.gotWeekly || entry.isPlayer)
                {
                  if (!this.slide_on)
                  {
                    if ((double) y2 < 424.0)
                      TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.gamerTag, new Vector2((float) LeaderboardsScreen.TAB1, y2), !entry.isPlayer || this.slide_on ? Color.Black : new Color(200, 0, 0));
                  }
                  else
                    TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.gamerTag, new Vector2((float) LeaderboardsScreen.TAB1, y2), !entry.isPlayer || this.slide_on ? Color.Black : new Color(200, 0, 0));
                  Vector2 vector2 = TheGame.instance.font1.MeasureString(entry.score);
                  TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.score, new Vector2((float) LeaderboardsScreen.TAB3 - vector2.X / 2f, y2), Color.Black);
                }
                else
                {
                  TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.gamerTag, new Vector2((float) LeaderboardsScreen.TAB1, y2), !entry.isPlayer || this.slide_on ? Color.Black : new Color(200, 0, 0));
                  int num9 = 440 + (int) ((double) this.panelY - 16.0) - (int) (LeaderboardsScreen.m_noScoreThisWeek.h / 2U);
                  if ((double) y2 > (double) num9)
                  {
                    int num10 = (int) ((double) y2 - (double) num9);
                    int w = (int) LeaderboardsScreen.m_noScoreThisWeek.w;
                    int h = (int) LeaderboardsScreen.m_noScoreThisWeek.h;
                    TheGame.instance.spriteBatch.End();
                    TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                    TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_noScoreThisWeek.intex, new Rectangle(LeaderboardsScreen.TAB3 - 68, (int) y2 - 16 /*0x10*/, w >> 1, (h >> 1) - num10), new Rectangle?(new Rectangle(0, 0, w, h - (num10 << 1))), Color.White);
                  }
                  else
                  {
                    TheGame.instance.spriteBatch.End();
                    TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                    TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_noScoreThisWeek.intex, new Rectangle(LeaderboardsScreen.TAB3 - 68, (int) y2 - 16 /*0x10*/, (int) (LeaderboardsScreen.m_noScoreThisWeek.w / 2U), (int) (LeaderboardsScreen.m_noScoreThisWeek.h / 2U)), Color.White);
                  }
                }
              }
              else
              {
                if (!this.slide_on)
                {
                  if ((double) y2 < 424.0)
                    TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.gamerTag, new Vector2((float) LeaderboardsScreen.TAB1, y2), !entry.isPlayer || this.slide_on ? Color.Black : new Color(200, 0, 0));
                }
                else
                  TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.gamerTag, new Vector2((float) LeaderboardsScreen.TAB1, y2), !entry.isPlayer || this.slide_on ? Color.Black : new Color(200, 0, 0));
                Vector2 vector2 = TheGame.instance.font1.MeasureString(entry.score);
                TheGame.instance.spriteBatch.DrawString(TheGame.instance.font2, entry.score, new Vector2((float) LeaderboardsScreen.TAB3 - vector2.X / 2f, y2), Color.Black);
              }
            }
            num7 += (float) LeaderboardsScreen.ADJUST;
            ++num8;
          }
          TheGame.instance.spriteBatch.End();
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_boxTexture.intex, new Rectangle(-196, (int) y1, 1024 /*0x0400*/, 204), new Rectangle?(new Rectangle(0, 0, 1024 /*0x0400*/, 204)), Color.White);
          TheGame.instance.spriteBatch.Draw(LeaderboardsScreen.m_boxTexture.intex, new Rectangle(-196, (int) y1 + 408, 1024 /*0x0400*/, 48 /*0x30*/), new Rectangle?(new Rectangle(0, 408, 1024 /*0x0400*/, 48 /*0x30*/)), Color.White);
          TheGame.instance.spriteBatch.End();
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.End();
        }
        else if (this.m_state == 3 || this.m_state == 4 || this.m_state == 5)
        {
          if (!this.slide_off && !this.slide_on)
          {
            string str1 = TheGame.instance.stringTable.GetString(1175);
            float num = 400f;
            float y3 = 220f;
            string[] strArray = str1.Split(new char[1]{ '#' }, StringSplitOptions.RemoveEmptyEntries);
            TheGame.instance.spriteBatch.Begin();
            foreach (string str2 in strArray)
            {
              string text = str2.Trim();
              Vector2 vector2 = TheGame.instance.font1.MeasureString(text);
              TheGame.instance.spriteBatch.DrawString(TheGame.instance.font3, text, new Vector2(num - vector2.X / 2f, y3), Color.Bisque);
              y3 += 30f;
            }
            TheGame.instance.spriteBatch.End();
          }
        }
        else if (this.m_state == 6 && !this.slide_off && !this.slide_on)
        {
          string str3 = TheGame.instance.stringTable.GetString(1176);
          float num = 400f;
          float y4 = 220f;
          string[] strArray = str3.Split(new char[1]{ '#' }, StringSplitOptions.RemoveEmptyEntries);
          TheGame.instance.spriteBatch.Begin();
          foreach (string str4 in strArray)
          {
            string text = str4.Trim();
            Vector2 vector2 = TheGame.instance.font1.MeasureString(text);
            TheGame.instance.spriteBatch.DrawString(TheGame.instance.font3, text, new Vector2(num - vector2.X / 2f, y4), Color.Bisque);
            y4 += 30f;
          }
          TheGame.instance.spriteBatch.End();
        }
        float num11 = (float) (350.0 + -(double) this.slide_transition * -170.0);
        float num12 = 125f;
        float num13 = -40f;
        if (this.m_modeClass != null)
        {
          this.m_modeClass.m_pos.Y = num12;
          this.m_modeClass.m_pos.X = num11;
          num12 += num13;
        }
        if (this.m_modeZen != null)
        {
          this.m_modeZen.m_pos.Y = num12;
          this.m_modeZen.m_pos.X = num11;
          num12 += num13;
        }
        if (this.m_modeArcade != null)
        {
          this.m_modeArcade.m_pos.Y = num12;
          this.m_modeArcade.m_pos.X = num11;
          num12 += num13;
        }
        if (this.m_modeWeekly != null)
        {
          this.m_modeWeekly.m_pos.Y = num12;
          this.m_modeWeekly.m_pos.X = num11;
          num12 += num13;
        }
        if (this.m_modeTotalFruit == null)
          return;
        this.m_modeTotalFruit.m_pos.Y = num12;
        this.m_modeTotalFruit.m_pos.X = num11;
        float num14 = num12 + num13;
      }

      private void QuitGameCallback()
      {
        SoundManager.GetInstance().SFXPlay(SoundDef.SND_MENU_BOMB);
        this.m_state = 2;
        this.slide_on = false;
        this.slide_off = true;
        ((Bomb) this.m_quitButton.m_entity).EnableGravity(true);
        this.m_quitButton.m_entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(5f) + 5f, -Mortar.Math.g_random.RandF(5f), 0.0f);
        Game.game_work.tutorialControl.ResetTutePos();
        Game.ClearMenuItems();
      }

      private void ReadFinished(int result)
      {
        if (this.m_state != 4)
          return;
        if (result == 2)
        {
          this.renderSize = 0.0f;
          this.m_state = 5;
        }
        else
          this.m_state = 6;
      }

      public static void SetStartLeaderboard(int start) => LeaderboardsScreen.startWith = start;

      private static void PlayerGetProfileCallback(IAsyncResult result)
      {
      }

      private enum AS
      {
        AS_IN,
        AS_WAIT,
        AS_OUT,
        AS_START_LB_READ,
        AS_WAITING_LB_READ,
        AS_READ_LB_SUCCESS,
        AS_READ_LB_FAILED,
        AS_READ_LB_DISPLAY,
        AS_READ_LB_PAGE_UP,
        AS_READ_LB_PAGE_DOWN,
        AS_READ_LB_WAIT_PAGE,
        AS_MODE_SELECT,
      }

      private enum ScrollState
      {
        Idle,
        Scroll,
      }

      public class LeaderboardData
      {
        public string score;
        public bool isPlayer;
        public bool gotWeekly;
        public string gamerTag;
        public int scoreAsInt;

        public LeaderboardData()
        {
          this.score = "110";
          this.isPlayer = false;
          this.gotWeekly = false;
          this.gamerTag = (string) null;
          this.scoreAsInt = 0;
        }
      }
    }
}
