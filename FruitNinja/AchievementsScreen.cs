// Decompiled with JetBrains decompiler
// Type: FruitNinja.AchievementsScreen
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortar;
using System;
using System.Collections.Generic;

namespace FruitNinja
{

    internal class AchievementsScreen : HUDControl3d
    {
      private float m_time;
      private int m_state;
      private MenuButton m_quitButton;
      private float prevTextOffset;
      private float textOffset;
      private float offset;
      private float snapY;
      private float tx;
      private float ty;
      private float initialX;
      private float initialY;
      private bool down;
      private bool prevTouch;
      private bool currTouch;
      private float renderSize = 1584f;
      private AchievementsScreen.ScrollState scrollState;
      private static Texture2D m_backing;
      private static Mortar.Texture m_title;
      private static Mortar.Texture m_box;
      private static Texture2D m_live;
      private static float liveX;
      private static float liveY;
      public static int[] achIconMap = new int[20]
      {
        8,
        7,
        6,
        2,
        9,
        5,
        13,
        4,
        14,
        3,
        1,
        15,
        10,
        12,
        16 /*0x10*/,
        17,
        11,
        18,
        19,
        20
      };
      private static AchievementsScreen.MapInfo[] mapdata = new AchievementsScreen.MapInfo[20]
      {
        new AchievementsScreen.MapInfo("Fruit Blitz", 4),
        new AchievementsScreen.MapInfo("Year Of The Dragon", 16 /*0x10*/),
        new AchievementsScreen.MapInfo("Fruit Fight", 3),
        new AchievementsScreen.MapInfo("Deja Vu", 12),
        new AchievementsScreen.MapInfo("Wake Up", 15),
        new AchievementsScreen.MapInfo("Great Fruit Ninja", 1),
        new AchievementsScreen.MapInfo("Ultimate Fruit Ninja", 2),
        new AchievementsScreen.MapInfo("Fruit Frenzy", 5),
        new AchievementsScreen.MapInfo("Fruit Rampage", 6),
        new AchievementsScreen.MapInfo("Fruit Annihilation", 7),
        new AchievementsScreen.MapInfo("Lucky Ninja", 8),
        new AchievementsScreen.MapInfo("Almost A Century", 9),
        new AchievementsScreen.MapInfo("Mango Magic", 10),
        new AchievementsScreen.MapInfo("Combo Mambo", 13),
        new AchievementsScreen.MapInfo("Moment Of Zen", 14),
        new AchievementsScreen.MapInfo("The Lovely Bunch", 17),
        new AchievementsScreen.MapInfo("Its All Pear Shaped", 11),
        new AchievementsScreen.MapInfo("Underachiever", 18),
        new AchievementsScreen.MapInfo("Overachiever", 19),
        new AchievementsScreen.MapInfo("Bomb Magnet", 20)
      };
      private static Dictionary<string, Texture2D> imgMapInfo = (Dictionary<string, Texture2D>) null;
      private static float py = 0.0f;
      private static float sy = 0.0f;

      public static int ABOUT_CENTRE_X => 190;

      public static int ABOUT_CENTRE_Y => 97;

      public static float ABOUT_SCREEN_HEIGHT => 320f;

      public AchievementsScreen()
      {
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
        this.m_quitButton.m_entity.m_vel = new Vector3(Mortar.Math.g_random.RandF(5f) + 5f, -Mortar.Math.g_random.RandF(5f), 0.0f);
        Game.game_work.tutorialControl.ResetTutePos();
      }

      public static void LoadContent()
      {
        if (AchievementsScreen.m_backing == null)
          AchievementsScreen.m_backing = TextureManager.GetInstance().Load("textureswp7/leaderboard_back.tex").intex;
        if (AchievementsScreen.m_title == null)
          AchievementsScreen.m_title = TextureManager.GetInstance().Load("achievements.tex", true);
        if (AchievementsScreen.m_box != null)
          return;
        AchievementsScreen.m_box = TextureManager.GetInstance().Load("achievements_box.tex", true);
      }

      public static void UnLoadContent()
      {
      }

      public override void Reset()
      {
      }

      public override void Release()
      {
        this.m_texture = (Mortar.Texture) null;
        this.m_time = 0.0f;
      }

      public override void Init()
      {
        this.Reset();
        AchievementsScreen.imgMapInfo = new Dictionary<string, Texture2D>();
        foreach (AchievementsScreen.MapInfo mapInfo in AchievementsScreen.mapdata)
          AchievementsScreen.imgMapInfo.Add(mapInfo.key, TheGame.instance.Content.Load<Texture2D>($"textureswp7/achicon_{(object) mapInfo.index}.tga"));
      }

      private bool InTextBox()
      {
        return Mortar.Math.BETWEEN((int) this.tx, 100, 582) && Mortar.Math.BETWEEN((int) this.ty, 172, 410);
      }

      public override void Update(float dt)
      {
        if (this.m_state == 1)
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
          float num = 268f;
          if (this.prevTouch != this.currTouch)
          {
            this.prevTouch = this.currTouch;
            if (this.currTouch)
            {
              if ((double) Mortar.Math.Abs(AchievementsScreen.sy) < 4.0)
              {
                this.down = true;
                AchievementsScreen.sy = 0.0f;
                if (this.InTextBox())
                {
                  this.initialX = this.tx;
                  this.initialY = this.ty;
                  this.textOffset = 0.0f;
                  this.prevTextOffset = 0.0f;
                  this.scrollState = AchievementsScreen.ScrollState.Scroll;
                }
              }
            }
            else
              this.down = false;
          }
          if (this.down && this.scrollState == AchievementsScreen.ScrollState.Scroll && this.InTextBox())
          {
            this.prevTextOffset = this.textOffset;
            this.textOffset = this.ty - this.initialY;
            float t = this.ty - AchievementsScreen.py;
            if ((double) Mortar.Math.Abs(t) > 1.0 && (double) Mortar.Math.Abs(t) < 64.0)
              AchievementsScreen.sy = t;
            if ((double) this.offset + (double) this.textOffset > 0.0)
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
            }
            if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - (double) num))
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
            }
            AchievementsScreen.py = this.ty;
          }
          else if ((double) Mortar.Math.Abs(AchievementsScreen.sy) > 1.0)
          {
            this.prevTextOffset = this.textOffset;
            this.textOffset += AchievementsScreen.sy;
            AchievementsScreen.sy -= AchievementsScreen.sy / 4f;
            if ((double) this.offset + (double) this.textOffset > 0.0)
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
              AchievementsScreen.sy = 0.0f;
            }
            if ((double) this.offset + (double) this.textOffset < -((double) this.renderSize - (double) num))
            {
              this.snapY = this.textOffset - this.prevTextOffset;
              this.textOffset = this.prevTextOffset;
              AchievementsScreen.sy = 0.0f;
            }
          }
          else
          {
            if (this.scrollState == AchievementsScreen.ScrollState.Scroll)
            {
              this.scrollState = AchievementsScreen.ScrollState.Idle;
              this.offset += this.textOffset;
              this.textOffset = 0.0f;
            }
            this.snapY -= this.snapY / 8f;
          }
        }
        else
        {
          this.scrollState = AchievementsScreen.ScrollState.Idle;
          this.initialX = this.tx = -1f;
          this.initialY = this.ty = -1f;
          this.textOffset = 0.0f;
        }
        Game.game_work.tutorialControl.ResetTutePos();
        switch (this.m_state)
        {
          case 0:
            this.m_time += (float) ((1.0 - (double) this.m_time) * 0.125);
            if ((double) this.m_time <= 0.99900001287460327)
              break;
            this.m_time = 1f;
            this.m_quitButton = new MenuButton("back_icon.tex", new Vector3((float) (425.0 - (double) Game.SCREEN_WIDTH / 2.0), (float) ((double) AchievementsScreen.ABOUT_SCREEN_HEIGHT / 2.0 - 266.0), 0.0f), new MenuButton.MenuCallback(this.QuitGameCallback), Fruit.MAX_FRUIT_TYPES, Vector3.Zero, true);
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
            Game.game_work.mainScreen.m_state = MainScreen.MS.MS_RETURN;
            this.m_terminate = true;
            break;
        }
      }

      public override void Draw(float[] tintChannels)
      {
        if (AchievementsScreen.m_backing != null)
        {
          TheGame.instance.spriteBatch.Begin();
          TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_backing, new Vector2(0.0f, 0.0f), Color.White);
          TheGame.instance.spriteBatch.End();
        }
        if (AchievementsScreen.m_title != null)
        {
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_title.intex, new Vector2(-80f, 0.0f), Color.White);
          TheGame.instance.spriteBatch.End();
        }
        float y1 = 0.0f;
        if (AchievementsScreen.m_box != null)
        {
          float num1 = 480f;
          float num2 = 16f;
          y1 = num1 - (num1 - num2) * Mortar.Math.Abs(this.m_time);
          TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) y1, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16 /*0x10*/, AchievementsScreen.m_box.intex.Height), Color.White);
          TheGame.instance.spriteBatch.End();
        }
        TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        if (AchievementsScreen.m_live != null)
        {
          AchievementsScreen.liveY = 64f;
          AchievementsScreen.liveX = (float) (400.0 + (double) y1 * 2.0);
          TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_live, new Vector2(AchievementsScreen.liveX, AchievementsScreen.liveY), Color.White);
        }
        float num3 = y1 + 138f;
        float num4 = 272f;
        Vector2 vector2_1 = TheGame.instance.font3.MeasureString(" ");
        for (int index = 0; index < 20; ++index)
        {
          Color color = AchievementManager.Unlocked(index) ? Color.Black : new Color(0, 0, 0, 96 /*0x60*/);
          bool flag1 = false;
          float y2 = y1 + 138f + (float) (index * 80 /*0x50*/) + this.textOffset + this.offset + this.snapY;
          float num5 = y2 + 64f;
          int num6 = 0;
          int num7 = 0;
          int y3 = 0;
          float num8 = y2;
          string[] strArray = AchievementManager.GetDescription(index).Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
          string text1 = "";
          string text2 = "";
          string str1 = "";
          float num9 = 0.0f;
          int num10 = 0;
          foreach (string str2 in strArray)
          {
            string text3 = str2.Trim();
            Vector2 vector2_2 = TheGame.instance.font3.MeasureString(text3);
            if ((double) num9 + (double) vector2_2.X + (double) vector2_1.X <= 380.0)
            {
              num9 += vector2_2.X + vector2_1.X;
            }
            else
            {
              num9 = vector2_2.X + vector2_1.X;
              ++num10;
            }
            switch (num10)
            {
              case 0:
                text1 = $"{text1}{str2} ";
                break;
              case 1:
                text2 = $"{text2}{str2} ";
                break;
              default:
                str1 = $"{str1}{str2} ";
                break;
            }
          }
          if ((double) num8 + 16.0 < (double) num3 - 20.0)
            flag1 = true;
          if ((double) num8 + 16.0 > (double) num3 + (double) num4 - 12.0)
            flag1 = true;
          if (!flag1)
          {
            TheGame.instance.spriteBatch.DrawString(TheGame.instance.font3, text1, new Vector2(100f, num8 + 16f), color);
            TheGame.instance.spriteBatch.DrawString(TheGame.instance.font3, AchievementManager.GetGamerScore(index).ToString(), new Vector2(510f, num8 + 16f), color);
          }
          bool flag2 = false;
          if ((double) num8 + 36.0 < (double) num3 - 20.0)
            flag2 = true;
          if ((double) num8 + 36.0 > (double) num3 + (double) num4 - 12.0)
            flag2 = true;
          if (!flag2 && text2.Length > 0)
            TheGame.instance.spriteBatch.DrawString(TheGame.instance.font3, text2, new Vector2(100f, num8 + 36f), color);
          Texture2D texture = AchievementsScreen.imgMapInfo[AchievementManager.GetKey(index)];
          bool flag3 = false;
          if ((double) num5 > (double) num3 + (double) num4)
          {
            float num11 = num5 - (num3 + num4);
            if ((double) num11 > 64.0)
            {
              flag3 = true;
            }
            else
            {
              num6 = (int) -(double) num11;
              num7 = num6 / 2;
              if (texture.Height == 64 /*0x40*/)
                num7 = num6;
            }
          }
          if ((double) y2 < (double) num3)
          {
            float num12 = num3 - y2;
            if ((double) num12 > 64.0)
            {
              flag3 = true;
            }
            else
            {
              y2 = num3;
              y3 = (int) num12;
              num7 = -y3;
              num6 = num7;
              if (texture.Height == 32 /*0x20*/)
              {
                y3 /= 2;
                num7 /= 2;
              }
            }
          }
          if (!flag3)
            TheGame.instance.spriteBatch.Draw(texture, new Rectangle(26, (int) y2, 64 /*0x40*/, 64 /*0x40*/ + num6), new Rectangle?(new Rectangle(0, y3, texture.Width, texture.Height + num7)), Color.White);
        }
        TheGame.instance.spriteBatch.End();
        TheGame.instance.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) y1, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16 /*0x10*/, 137), new Rectangle?(new Rectangle(0, 0, AchievementsScreen.m_box.intex.Width, 137)), Color.White);
        TheGame.instance.spriteBatch.Draw(AchievementsScreen.m_box.intex, new Rectangle(-170, (int) y1 + 400, AchievementsScreen.m_box.intex.Width - AchievementsScreen.m_box.intex.Width / 16 /*0x10*/, 48 /*0x30*/), new Rectangle?(new Rectangle(0, 400, AchievementsScreen.m_box.intex.Width, 48 /*0x30*/)), Color.White);
        TheGame.instance.spriteBatch.End();
      }

      private class MapInfo
      {
        public string key;
        public int index;

        public MapInfo(string s, int i)
        {
          this.key = s;
          this.index = i;
        }
      }

      private enum ScrollState
      {
        Idle,
        Scroll,
      }

      public enum AS
      {
        AS_IN,
        AS_WAIT,
        AS_OUT,
      }
    }
}
