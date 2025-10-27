// Decompiled with JetBrains decompiler
// Type: FruitNinja.SplatEntity
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortar;

namespace FruitNinja
{

    public class SplatEntity
    {
      protected bool m_hasSeeds;
      protected float m_criticalFadeTime;
      protected Color m_colour;
      protected float m_startAlpha;
      protected float m_rotation;
      protected int m_fruitType;
      protected bool m_canBeLong;
      protected Vector3 m_xVec;
      protected Vector3 m_yVec;
      protected bool m_flip;
      public ushort m_dir_angle;
      public Vector3 m_pos;
      public Vector3 m_cur_scale;
      public Vector3 m_vel;
      public int m_onWall;
      public Vector3 m_orig_scale;
      public float m_fadeTime;
      public float m_fadeSpeed;
      public bool m_update;
      public bool m_noSound;
      public static VertexPositionColorTexture[] m_points = ArrayInit.CreateFilledArray<VertexPositionColorTexture>(3072 /*0x0C00*/);
      public static int m_curr_drawing_splat = 0;
      public static SplatEntity[] pool;
      public static int poolCount;
      public static int currentFree;
      public static int WALL_DIST = 50;
      public static int WALL_SPLASH_VARIANTS = 6;
      public static float SPLAT_FRAME_LENGTH = 0.05f;
      public static int SPLAT_FRAMES = 2;
      public static float SPLAT_FRAME_WIDTH = 1f / (float) SplatEntity.SPLAT_FRAMES;
      public static float SPLAT_ANGLE_VARIANT = 45f;
      public static float SPLAT_FADE_LENGTH = 5f;
      public static float SPLAT_FADE_SPEED = 0.5f;
      public static float SPLAT_SPRINKLE_WAIT = 0.25f;
      public static float SPLAT_SPRINKLE_SEPARATION = 0.5f;
      public static float CRITICAL_FADE_TIME = 0.5f;
      private static Mortar.Texture splatTexture;
      private static float[] wallMaxDrop = new float[6]
      {
        2.5f,
        2.5f,
        2.5f,
        2.9f,
        0.0f,
        0.0f
      };
      private static float[] splatScale = new float[6]
      {
        1.6f,
        1.6f,
        1.6f,
        1.6f,
        2.9f,
        2.9f
      };
      private static float[] splatUV = new float[24]
      {
        0.0f,
        0.5f,
        0.0f,
        0.25f,
        0.5f,
        1f,
        0.0f,
        0.25f,
        0.0f,
        0.5f,
        0.25f,
        0.5f,
        0.5f,
        1f,
        0.25f,
        0.5f,
        0.0f,
        1f,
        0.5f,
        0.75f,
        0.0f,
        1f,
        0.75f,
        1f
      };
      private static bool loadedSplat = false;
      private static float sprinkleWait = 0.0f;
      public static float SPLAT_SFX_WAIT = 0.5f;
      private static float splatSpeed = 0.0f;
      private static float[] sfxWait = new float[4];
      private static int numActiveSplats = 0;
      private static int coconut = Fruit.FruitType(nameof (coconut));

      public static int RAND_SPLAT => Math.g_random.Rand32(6) <= 0 ? 0 : 1;

      public static int RAND_SPRINKLE => Math.g_random.Rand32(2) != 0 ? 3 : 2;

      public static int RAND_LONG_SPLAT => Math.g_random.Rand32(2) != 0 ? 5 : 4;

      public static bool IS_LONG_SPLAT(int s) => s == 4 || s == 5;

      private void PlaySplat(int smallMediumLarge)
      {
        smallMediumLarge = Math.CLAMP(smallMediumLarge, 0, 2);
        SplatEntity.sfxWait[smallMediumLarge] -= 0.05f;
        if ((double) SplatEntity.sfxWait[smallMediumLarge] > 0.0)
          return;
        SplatEntity.sfxWait[smallMediumLarge] = SplatEntity.SPLAT_SFX_WAIT;
        switch (smallMediumLarge)
        {
          case 0:
            SoundManager.GetInstance().SFXPlay(SoundDef.SND_DRIP);
            break;
          case 1:
            SoundManager.GetInstance().SFXPlay(Math.g_random.Rand32(2) != 0 ? SoundDef.SND_SPLATTER_SMALL_1 : SoundDef.SND_SPLATTER_SMALL_2);
            break;
          case 2:
            SoundManager.GetInstance().SFXPlay(Math.g_random.Rand32(2) != 0 ? SoundDef.SND_SPLATTER_MEDIUM_1 : SoundDef.SND_SPLATTER_MEDIUM_2);
            break;
          case 3:
            SoundManager.GetInstance().SFXPlay(Math.g_random.Rand32(2) != 0 ? SoundDef.SND_SPLATTER_LARGE_1 : SoundDef.SND_SPLATTER_LARGE_2);
            break;
        }
      }

      public void MakeSplat(Vector3 pos, Vector3 dir, bool canBeLong)
      {
        this.MakeSplat(pos, dir, canBeLong, 0);
      }

      public void MakeSplat(Vector3 pos, Vector3 dir, bool canBeLong, int fruitType)
      {
        this.MakeSplat(pos, dir, canBeLong, false, fruitType);
      }

      public void MakeSplat(Vector3 pos, Vector3 dir, bool canBeLong, bool noSound, int fruitType)
      {
        this.m_noSound = noSound;
        this.m_canBeLong = canBeLong;
        if (fruitType >= Fruit.MAX_FRUIT_TYPES)
        {
          this.m_criticalFadeTime = 1f + SplatEntity.SPLAT_SPRINKLE_SEPARATION;
          this.m_colour = Fruit.CRITICAL_COLOUR;
        }
        else
        {
          this.m_criticalFadeTime = 0.0f;
          this.m_colour = Fruit.FruitTypeColor(fruitType);
        }
        this.m_flip = Math.g_random.Rand32(2) != 0;
        this.m_startAlpha = (float) this.m_colour.A;
        this.m_pos = pos;
        this.m_pos.Z = 0.0f;
        this.m_vel = dir;
        this.m_vel.Z = (float) (-(double) this.m_vel.Length() * 0.5 - 150.0) - Math.g_random.RandF(10f);
        this.m_vel.Y *= 1.5f;
        this.m_vel *= 6f;
        this.m_rotation = (float) Math.g_random.Rand32(360);
        this.m_fruitType = fruitType;
        this.m_hasSeeds = Fruit.FruitInfo(this.m_fruitType % Fruit.MAX_FRUIT_TYPES).hasSplatSeeds;
        this.m_cur_scale.X = this.m_cur_scale.Y = this.m_cur_scale.Z = Math.g_random.RandF(10f) + 15f;
        this.m_cur_scale.Y *= -1f;
        this.m_orig_scale = this.m_cur_scale;
        this.m_onWall = -1;
        Vector3 size = new Vector3();
        this.Init((byte[]) null, out size);
        if (Math.g_random.Rand32(4) == 0 || this.m_colour.A == (byte) 0 || this.m_fruitType < Fruit.MAX_FRUIT_TYPES && Fruit.FruitInfo(this.m_fruitType).onlySprinkle && Math.g_random.Rand32(3) == 0)
          this.m_update = false;
        this.m_xVec = new Vector3(Math.CosIdx(Math.DEGREE_TO_IDX(this.m_rotation)), Math.SinIdx(Math.DEGREE_TO_IDX(this.m_rotation)), 0.0f) * 0.5f;
        this.m_yVec = new Vector3(Math.CosIdx(Math.DEGREE_TO_IDX(this.m_rotation + 90f)), Math.SinIdx(Math.DEGREE_TO_IDX(this.m_rotation + 90f)), 0.0f) * 0.5f;
      }

      public SplatEntity()
      {
        this.m_onWall = -1;
        this.m_update = false;
      }

      public static void LoadContent()
      {
        if (SplatEntity.loadedSplat)
          return;
        SplatEntity.loadedSplat = true;
        SplatEntity.splatTexture = TextureManager.GetInstance().Load("textureswp7/white_splash.tex");
      }

      public virtual void Init(byte[] tpl_data, out Vector3 size)
      {
        size = new Vector3();
        this.m_onWall = -1;
        this.m_update = true;
      }

      public virtual void Release()
      {
      }

      public static void CreatePool() => SplatEntity.CreatePool(512 /*0x0200*/);

      public static void CreatePool(int num)
      {
        Delete.SAFE_DELETE_ARRAY<SplatEntity[]>(ref SplatEntity.pool);
        SplatEntity.pool = ArrayInit.CreateFilledArray<SplatEntity>(num);
        SplatEntity.poolCount = num;
        SplatEntity.currentFree = 0;
      }

      public static SplatEntity GetFree()
      {
        int num = 0;
        while (SplatEntity.pool[SplatEntity.currentFree].m_update && num < SplatEntity.poolCount)
        {
          ++num;
          if (++SplatEntity.currentFree >= SplatEntity.poolCount)
            SplatEntity.currentFree = 0;
        }
        return SplatEntity.pool[SplatEntity.currentFree];
      }

      public static void CleanUp()
      {
        Delete.SAFE_DELETE_ARRAY<SplatEntity[]>(ref SplatEntity.pool);
        SplatEntity.poolCount = 0;
      }

      public static void UpdateActiveSplats(float dt)
      {
        for (int index = 0; index < 3; ++index)
        {
          if ((double) SplatEntity.sfxWait[index] > 0.0)
            SplatEntity.sfxWait[index] -= dt;
        }
        if ((double) SplatEntity.sprinkleWait > 0.0)
        {
          SplatEntity.sprinkleWait -= dt;
          if ((double) SplatEntity.sprinkleWait <= 0.0)
            SoundManager.GetInstance().SFXPlay(SoundDef.SND_DRIP);
        }
        else if ((double) SplatEntity.sprinkleWait >= -(double) SplatEntity.SPLAT_SPRINKLE_SEPARATION)
          SplatEntity.sprinkleWait -= dt;
        SplatEntity.splatSpeed = 1.25f + Math.CLAMP((float) ((double) ((long) ActorManager.GetInstance().GetNumEntities() + (long) SplatEntity.NumActiveSplats()) / 15.0 - 0.15000000596046448), 0.0f, 3f);
        if (!Game.IsFastHardware() || Game.IsMultiplayer())
          SplatEntity.splatSpeed *= 1.5f;
        SplatEntity.splatSpeed *= 1.5f;
        int num = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < SplatEntity.poolCount; ++index2)
        {
          SplatEntity splatEntity = SplatEntity.pool[index1];
          if (splatEntity.m_update)
          {
            splatEntity.Update(dt);
            ++num;
          }
          ++index1;
        }
        SplatEntity.numActiveSplats = num;
      }

      public static void DrawActiveSplats()
      {
        SplatEntity.m_curr_drawing_splat = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < SplatEntity.poolCount; ++index2)
        {
          SplatEntity splatEntity = SplatEntity.pool[index1];
          if (splatEntity.m_update && splatEntity.m_onWall >= 0)
          {
            splatEntity.DrawSplat();
            ++SplatEntity.m_curr_drawing_splat;
          }
          ++index1;
        }
        if (SplatEntity.m_curr_drawing_splat <= 0 || SplatEntity.splatTexture == null || SplatEntity.m_points == null)
          return;
        SplatEntity.splatTexture.Set();
        MatrixManager.instance.Reset();
        MatrixManager.instance.Translate(Vector3.UnitZ * -5500f);
        MatrixManager.instance.UploadCurrentMatrices(true);
        Mesh.DrawTriListEx(SplatEntity.m_points, SplatEntity.m_curr_drawing_splat * 6, true, 0);
      }

      public static void RemoveAllSplats()
      {
        for (int index = 0; index < SplatEntity.poolCount; ++index)
          SplatEntity.pool[index].Destroy();
      }

      public static int NumActiveSplats() => SplatEntity.numActiveSplats;

      public void Destroy() => this.m_update = false;

      private void SetVertex(
        ref VertexPositionColorTexture vertex,
        float x,
        float y,
        float u,
        float v,
        Color c)
      {
        vertex.Position = new Vector3(x, y, 0.0f);
        vertex.TextureCoordinate = new Vector2(u, v);
        vertex.Color = c;
      }

      private void SetVertex2(ref VertexPositionColorTexture vertex, Color c)
      {
      }

      public virtual void DrawSplat()
      {
        int index = SplatEntity.m_curr_drawing_splat * 6;
        float u1 = SplatEntity.splatUV[this.m_onWall << 2] * 0.5f;
        float u2 = SplatEntity.splatUV[(this.m_onWall << 2) + 1] * 0.5f;
        if (this.m_hasSeeds)
        {
          u1 += 0.5f;
          u2 += 0.5f;
        }
        Color c = Game.TintColour(this.m_colour, Game.game_work.hud.m_backTint);
        c.R += c.R < (byte) 224 /*0xE0*/ ? (byte) 32 /*0x20*/ : (byte) 0;
        c.G += c.G < (byte) 224 /*0xE0*/ ? (byte) 32 /*0x20*/ : (byte) 0;
        c.B += c.B < (byte) 224 /*0xE0*/ ? (byte) 32 /*0x20*/ : (byte) 0;
        c.A += c.A < (byte) 224 /*0xE0*/ ? (byte) 32 /*0x20*/ : (byte) 0;
        float v1 = SplatEntity.splatUV[(this.m_onWall << 2) + 2];
        float v2 = SplatEntity.splatUV[(this.m_onWall << 2) + 3];
        if (this.m_flip)
        {
          v2 = v1;
          v1 = SplatEntity.splatUV[(this.m_onWall << 2) + 3];
        }
        this.SetVertex(ref SplatEntity.m_points[index], this.m_pos.X + (this.m_xVec.X + this.m_yVec.X) * this.m_cur_scale.X, this.m_pos.Y + (this.m_xVec.Y + this.m_yVec.Y) * this.m_cur_scale.Y, u2, v1, c);
        this.SetVertex(ref SplatEntity.m_points[index + 1], this.m_pos.X + (-this.m_xVec.X + this.m_yVec.X) * this.m_cur_scale.X, this.m_pos.Y + (-this.m_xVec.Y + this.m_yVec.Y) * this.m_cur_scale.Y, u1, v1, c);
        this.SetVertex(ref SplatEntity.m_points[index + 2], this.m_pos.X + (this.m_xVec.X - this.m_yVec.X) * this.m_cur_scale.X, this.m_pos.Y + (this.m_xVec.Y - this.m_yVec.Y) * this.m_cur_scale.Y, u2, v2, c);
        SplatEntity.m_points[index + 3] = SplatEntity.m_points[index + 2];
        SplatEntity.m_points[index + 4] = SplatEntity.m_points[index + 1];
        this.SetVertex(ref SplatEntity.m_points[index + 5], this.m_pos.X + (-this.m_xVec.X - this.m_yVec.X) * this.m_cur_scale.X, this.m_pos.Y + (-this.m_xVec.Y - this.m_yVec.Y) * this.m_cur_scale.Y, u1, v2, c);
      }

      public virtual void Update(float dt)
      {
        this.m_pos += this.m_vel * Game.game_work.dt;
        if (this.m_onWall < 0)
        {
          if ((double) this.m_pos.Z < (double) -SplatEntity.WALL_DIST)
          {
            this.m_onWall = Math.g_random.Rand32(4) > 0 ? SplatEntity.RAND_SPLAT : SplatEntity.RAND_SPRINKLE;
            if (this.m_canBeLong && Math.g_random.Rand32(2) == 0)
              this.m_onWall = SplatEntity.RAND_LONG_SPLAT;
            if (this.m_fruitType < Fruit.MAX_FRUIT_TYPES && Fruit.FruitInfo(this.m_fruitType).onlySprinkle)
              this.m_onWall = SplatEntity.RAND_SPRINKLE;
            if (SplatEntity.IS_LONG_SPLAT(this.m_onWall))
            {
              this.m_rotation = (float) (-(double) Math.Atan2Idx(this.m_vel.Y, this.m_vel.X) / 182.0);
              this.m_rotation += Math.g_random.RandF(SplatEntity.SPLAT_ANGLE_VARIANT) - SplatEntity.SPLAT_ANGLE_VARIANT / 2f;
              this.m_xVec = new Vector3(Math.CosIdx(Math.DEGREE_TO_IDX(this.m_rotation)), Math.SinIdx(Math.DEGREE_TO_IDX(this.m_rotation)), 0.0f) * 0.5f;
              this.m_yVec = new Vector3(Math.CosIdx(Math.DEGREE_TO_IDX(this.m_rotation + 90f)), Math.SinIdx(Math.DEGREE_TO_IDX(this.m_rotation + 90f)), 0.0f) * 0.25f;
            }
            this.m_pos.Z = (float) -SplatEntity.WALL_DIST;
            this.m_vel = Vector3.Zero;
            this.m_cur_scale *= 2.5f * SplatEntity.splatScale[this.m_onWall];
            if (!this.m_noSound)
            {
              if (this.m_fruitType == SplatEntity.coconut)
                this.PlaySplat(0);
              else if ((double) this.m_cur_scale.X > 100.0)
                this.PlaySplat(3);
              else if ((double) this.m_cur_scale.X > 30.0)
                this.PlaySplat(2);
              else
                this.PlaySplat(1);
            }
            if (Game.IsMultiplayer())
              this.m_cur_scale *= Game.SPLIT_SCREEN_SCALE;
            this.m_fadeTime = Math.g_random.RandF(SplatEntity.SPLAT_FADE_LENGTH * 0.5f) + SplatEntity.SPLAT_FADE_LENGTH * 0.75f;
            this.m_fadeSpeed = Math.g_random.RandF(SplatEntity.SPLAT_FADE_SPEED * 0.5f) + SplatEntity.SPLAT_FADE_SPEED * 0.75f;
            if (Math.g_random.Rand32(10) == 0 && (double) SplatEntity.sprinkleWait < -(double) SplatEntity.SPLAT_SPRINKLE_SEPARATION)
              SplatEntity.sprinkleWait = SplatEntity.SPLAT_SPRINKLE_WAIT;
          }
          if (Game.IsMultiplayer())
          {
            float num = Game.game_work.dt * 10f;
            this.m_vel.X += (double) this.m_pos.X < 0.0 ? -num : num;
          }
          else
            this.m_vel.Y -= Game.game_work.dt * 10f;
        }
        else
        {
          dt *= SplatEntity.splatSpeed;
          if ((double) this.m_fadeTime <= 1.25)
          {
            if (Game.IsMultiplayer())
            {
              float num = dt * SplatEntity.wallMaxDrop[this.m_onWall];
              this.m_vel.X += (double) this.m_pos.X < 0.0 ? -num : num;
              this.m_vel.X = Math.CLAMP(this.m_vel.X, -50f, 50f);
              this.m_cur_scale.X += (double) this.m_pos.X < 0.0 ? -num : num;
              this.m_cur_scale.X = Math.CLAMP(this.m_cur_scale.X, -200f, 200f);
            }
            else
            {
              this.m_vel.Y -= dt * SplatEntity.wallMaxDrop[this.m_onWall];
              this.m_vel.Y = Math.MAX(-50f, this.m_vel.Y);
              this.m_cur_scale.Y -= dt * SplatEntity.wallMaxDrop[this.m_onWall];
              this.m_cur_scale.Y = Math.MAX(-200f, this.m_cur_scale.Y);
            }
          }
          if ((double) this.m_criticalFadeTime > 0.0)
          {
            this.m_criticalFadeTime -= Game.game_work.dt;
            if ((double) this.m_criticalFadeTime <= 0.0)
              this.m_criticalFadeTime = 0.0f;
            float num = 1f - Math.CLAMP(this.m_criticalFadeTime / SplatEntity.CRITICAL_FADE_TIME, 0.0f, 1f);
            Color color = Fruit.FruitTypeColor(this.m_fruitType % Fruit.MAX_FRUIT_TYPES);
            this.m_colour.R = (byte) ((double) Fruit.CRITICAL_COLOUR.R + (double) ((int) color.R - (int) Fruit.CRITICAL_COLOUR.R) * (double) num);
            this.m_colour.G = (byte) ((double) Fruit.CRITICAL_COLOUR.G + (double) ((int) color.G - (int) Fruit.CRITICAL_COLOUR.G) * (double) num);
            this.m_colour.B = (byte) ((double) Fruit.CRITICAL_COLOUR.B + (double) ((int) color.B - (int) Fruit.CRITICAL_COLOUR.B) * (double) num);
            this.m_startAlpha = (float) Fruit.CRITICAL_COLOUR.A + (float) ((int) color.A - (int) Fruit.CRITICAL_COLOUR.A) * num;
          }
          this.m_fadeTime -= dt * this.m_fadeSpeed;
          if ((double) this.m_fadeTime <= 0.0)
          {
            this.m_fadeTime = 0.0f;
            this.Destroy();
          }
          this.m_colour.A = (byte) Math.MIN(this.m_startAlpha, this.m_startAlpha * this.m_fadeTime);
        }
      }

      public virtual void Draw()
      {
      }

      public virtual void DrawUpdate(float dt)
      {
      }

      public static void CleanUpSplat()
      {
        SplatEntity.CleanUp();
        SplatEntity.loadedSplat = false;
        SplatEntity.splatTexture = (Mortar.Texture) null;
      }
    }
}
