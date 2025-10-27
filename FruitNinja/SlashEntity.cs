// Decompiled with JetBrains decompiler
// Type: FruitNinja.SlashEntity
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

    public class SlashEntity : Entity
    {
      private const int MAX_POINTS_REMEMBERED = 6;
      private const int MAX_MOD_COLOURS = 16 /*0x10*/;
      public static float START_THICKNESS = 9f;
      public static float MAX_POINTS = 110f;
      public static float ALPHA_OUT_PERCENT = 0.99f;
      public static float SWIPE_SFX_WAIT = 0.5f;
      public static float COMBO_TIME = 0.1f;
      public static float NEW_WHOA_TIME = 6f;
      public static float FRUIT_HIT_SHOUT_COUNT = 1f;
      public static float CRIT_SHOUT_COUNT = 3f;
      public static float SWIPE_SHOUT_COUNT = 0.5f;
      public static float SHOUT_VARIANTS = 15f;
      protected PSPParticleEmitter m_emitter;
      protected float m_criticalTime;
      protected Color m_slashColor;
      protected Color m_slashModColor;
      protected bool m_hitBomb;
      protected byte PAD00;
      protected short PAD01;
      protected int m_pointsCount;
      protected int m_currentIndex;
      protected int m_drawCount;
      protected VertexPositionColorTexture[] m_points;
      protected VertexPositionColorTexture[] m_pointPoints = ArrayInit.CreateFilledArray<VertexPositionColorTexture>(4);
      protected Vector3 m_lastDir;
      protected Vector3 m_lastPos;
      protected float m_col_length;
      protected float m_slashLength;
      public int m_splatsToMake;
      protected float m_timeTillNextSplat;
      protected float m_splatInterval;
      protected Vector3 m_lastSlashDir;
      protected Vector3 m_lastSlashPos;
      protected int m_lastSlashFruitType;
      protected float m_swipeWait;
      protected Vector3[] m_pointTrail = ArrayInit.CreateFilledArray<Vector3>(6);
      protected int m_currentPoint;
      protected int m_pointsRemembered;
      protected Vector3 m_averageDir;
      protected float m_timeSinceLastHit;
      protected int m_comboLength;
      protected int m_comboPlayer;
      protected MissControl missControl;
      public byte m_touchDown;
      public float m_shoutTime;
      public int[] m_lastShouts = new int[2];
      public int[] m_comboFruitTypes = new int[11];
      public static float ModColorTime = 0.0f;
      public static float ModColorChangeSpeed = 1f;
      public static Color[] ModColors = ArrayInit.CreateFilledArray<Color>(16 /*0x10*/);
      public static int NumModColors = 1;
      public static int ModColorType = 0;
      public static bool ModHasPartilces = false;
      public static uint ModPartilcesHash = 0;
      private static Mortar.Texture s_slashTexture = (Mortar.Texture) null;
      private static Mortar.Texture s_slashAltTexture = (Mortar.Texture) null;
      private static bool loaded = false;
      private static bool updated = false;
      private static bool updated_last_frame = false;
      private static int slashes = 0;
      private static Color colourOut = Color.White;
      public static SlashEntity.ComboCanceled ComboCanceledEvent;
      private Vector3[] m_oldPositions = new Vector3[3];
      public static uint ModPowerMask;
      private static float[] pointTotals = new float[100];
      private static bool STOP = false;
      private static int STOP_COUNTER = 0;
      private static COMBO_TYPE s_combo = COMBO_TYPE.CT_NONE;
      private static uint[] hashes = new uint[6]
      {
        StringFunctions.StringHash("PUSH_FRUIT"),
        StringFunctions.StringHash("PULL_FRUIT"),
        StringFunctions.StringHash("PUSH_BOMB"),
        StringFunctions.StringHash("PULL_BOMB"),
        StringFunctions.StringHash("BOMB_HIT"),
        StringFunctions.StringHash("FRUIT_BOUNCE")
      };
      private static uint[] ParseSlashModColorTypehashes = new uint[4]
      {
        StringFunctions.StringHash("NONE"),
        StringFunctions.StringHash("LERP"),
        StringFunctions.StringHash("PER_SLASH"),
        StringFunctions.StringHash("CONTINUOUS")
      };

      public static float START_MP_THICKNESS => SlashEntity.START_THICKNESS * Game.SPLIT_SCREEN_SCALE;

      public static float FADE_OUT_TIME => 0.2f;

      public static float FADE_FRAME_AMOUNT => SlashEntity.START_THICKNESS / SlashEntity.FADE_OUT_TIME;

      public static float FADE_MP_FRAME_AMOUNT
      {
        get => SlashEntity.START_MP_THICKNESS / SlashEntity.FADE_OUT_TIME;
      }

      public static float START_CHANGED_DIST => 50f * Game.GAME_MODE_SCALE_FIX;

      public static float CHANGED_DIST => 5f * Game.GAME_MODE_SCALE_FIX;

      protected void InitPoints(int numPoints)
      {
        Color white = Color.White;
        this.m_lastDir = Vector3.Zero;
        this.m_drawCount = 0;
        this.m_pointsCount = numPoints;
        this.m_points = new VertexPositionColorTexture[numPoints];
        this.m_lastPos = new Vector3(-65535f, -65535f, -65535f);
        for (int index = 0; index < numPoints; ++index)
        {
          this.m_points[index].Position.X = 0.0f;
          this.m_points[index].Position.Y = 0.0f;
          this.m_points[index].Position.Z = 0.0f;
          this.m_points[index].Color = white;
          this.m_points[index].TextureCoordinate.X = 0.0f;
          this.m_points[index].TextureCoordinate.Y = 0.0f;
        }
      }

      protected void AddPoint(Vector3 pos, Vector3 dir)
      {
        if (!(dir != Vector3.Zero) && !(this.m_lastDir != Vector3.Zero))
          return;
        if (dir == Vector3.Zero)
        {
          dir = this.m_lastDir;
          this.m_pointTrail[this.m_currentPoint] = Vector3.Zero;
        }
        else if (this.m_pointsRemembered == 0)
        {
          this.m_pointTrail[this.m_currentPoint] = Vector3.Zero;
        }
        else
        {
          this.m_pointTrail[this.m_currentPoint] = dir;
          this.m_pointTrail[this.m_currentPoint].Normalize();
        }
        this.m_lastDir = dir;
        this.m_averageDir = Vector3.Zero;
        if (this.m_pointsRemembered >= 2)
        {
          for (int index = 1; index < this.m_pointsRemembered; ++index)
            this.m_averageDir += this.m_pointTrail[(this.m_currentPoint - index + 18) % 6];
          this.m_averageDir /= (float) (this.m_pointsRemembered - 1);
          if ((double) (this.m_averageDir - this.m_pointTrail[this.m_currentPoint]).LengthSquared() > 1.6899998188018799)
            this.m_timeSinceLastHit = SlashEntity.COMBO_TIME - 0.005f;
        }
        ++this.m_pointsRemembered;
        this.m_pointsRemembered = Mortar.Math.MIN(this.m_pointsRemembered, 6);
        this.m_currentPoint = (this.m_currentPoint + 1) % 6;
        Vector3 vector3_1 = Vector3.Cross(dir, new Vector3(0.0f, 0.0f, 1f));
        vector3_1.Normalize();
        if (this.m_drawCount >= this.m_pointsCount)
        {
          for (int index = 2; index < this.m_pointsCount; index += 2)
          {
            this.m_points[index - 2].Position = this.m_points[index].Position;
            this.m_points[index - 1].Position = this.m_points[index + 1].Position;
          }
          this.m_drawCount = this.m_pointsCount - 2;
        }
        if ((double) this.m_slashLength < 1.0)
        {
          this.m_slashLength += 45f * Game.game_work.dt;
          if ((double) this.m_slashLength > 1.0)
            this.m_slashLength = 1f;
        }
        Vector3 vector3_2 = !Game.IsMultiplayer() ? vector3_1 * (SlashEntity.START_THICKNESS * this.m_slashLength) : vector3_1 * (SlashEntity.START_MP_THICKNESS * this.m_slashLength);
        Vector3 vector3_3 = pos - vector3_2;
        this.ValFloat(vector3_3.X);
        this.ValFloat(vector3_3.Y);
        this.m_points[this.m_drawCount].Position = vector3_3;
        this.m_points[this.m_drawCount].TextureCoordinate = new Vector2(0.5f, 0.0f);
        this.m_points[this.m_drawCount++].Color = this.m_slashColor;
        Vector3 vector3_4 = pos + vector3_2;
        this.ValFloat(vector3_4.X);
        this.ValFloat(vector3_4.Y);
        this.m_points[this.m_drawCount].Position = vector3_4;
        this.m_points[this.m_drawCount].TextureCoordinate = new Vector2(0.5f, 1f);
        this.m_points[this.m_drawCount++].Color = this.m_slashColor;
      }

      private void ValFloat(float f)
      {
        if (float.IsNaN(f))
          throw new Exception();
      }

      private Vector3 GetV3(ref GameVertex vertex) => new Vector3(vertex.X, vertex.Y, vertex.Z);

      protected void UpdatePoints(float dt)
      {
        if ((double) dt == 0.0 && (double) Game.game_work.hitBombTime > 0.0)
        {
          if (this.m_hitBomb)
          {
            Color red = Color.Red;
            for (int index = 0; index < this.m_drawCount; ++index)
              this.m_points[index].Color = red;
            this.m_pointPoints[0].Color = red;
            this.m_pointPoints[1].Color = red;
            this.m_pointPoints[2].Color = red;
          }
          this.m_col_length = -1f;
        }
        else
        {
          if (this.m_drawCount >= 4 && this.m_touchDown != (byte) 0)
          {
            Vector3 vector3_1 = Game.game_work.camera.TranslatePos(this.m_oldPositions[1], true, false);
            Vector3 vector3_2 = Game.game_work.camera.TranslatePos(this.m_oldPositions[0], true, false);
            Vector3 vector3_3 = (vector3_1 + vector3_2) * 0.5f;
            this.m_col_box.centre = vector3_3;
            ((ColLine) this.m_col_box).Direction = vector3_2;
            this.m_col_length = (float) (((double) vector3_3.X - (double) vector3_2.X) * ((double) vector3_3.X - (double) vector3_2.X) + ((double) vector3_3.Y - (double) vector3_2.Y) * ((double) vector3_3.Y - (double) vector3_2.Y));
          }
          else
            this.m_col_length = -1f;
          if (this.m_drawCount < 4)
            this.m_lastDir = Vector3.Zero;
          if (SlashEntity.ModColorType == 0)
            SlashEntity.ModColorTime = 0.0f;
          SlashEntity.pointTotals[0] = 0.0f;
          int v2 = 0;
          int num1 = 0;
          for (int index = 0; index < this.m_drawCount; index += 2)
          {
            Vector3 vector3_4 = (this.m_points[index].Position + this.m_points[index + 1].Position) / 2f;
            float num2 = (vector3_4 - this.m_points[index].Position).Length();
            float num3 = !Game.IsMultiplayer() ? num2 - SlashEntity.FADE_FRAME_AMOUNT * dt : num2 - SlashEntity.FADE_MP_FRAME_AMOUNT * dt;
            if ((double) num3 <= 0.0)
              num1 += 2;
            else if (this.m_drawCount > 2 && index + 3 < this.m_drawCount)
            {
              ++v2;
              Vector3 vector1 = (this.m_points[index + 2].Position + this.m_points[index + 3].Position) / 2f - vector3_4;
              float num4 = vector1.Length();
              vector1.Normalize();
              Vector3 vector3_5 = Vector3.Cross(vector1, new Vector3(0.0f, 0.0f, 1f)) * num3;
              if (v2 > 0)
                SlashEntity.pointTotals[v2] = SlashEntity.pointTotals[v2 - 1] + num4;
              Vector3 vector3_6 = vector3_4 - vector3_5;
              this.ValFloat(vector3_6.X);
              this.ValFloat(vector3_6.Y);
              this.m_points[index - num1].Position = vector3_6;
              vector3_6 = vector3_4 + vector3_5;
              this.ValFloat(vector3_6.X);
              this.ValFloat(vector3_6.Y);
              this.m_points[index - num1 + 1].Position = vector3_6;
              float num5 = (float) ((double) index / (double) this.m_drawCount * 0.98000001907348633);
              this.m_points[index - num1].TextureCoordinate.X = num5;
              this.m_points[index - num1 + 1].TextureCoordinate.X = num5;
              if (SlashEntity.ModColorType == 0)
              {
                SlashEntity.UpdateModColor(ref this.m_slashModColor, -2f / (float) this.m_drawCount);
                if ((double) this.m_criticalTime > 0.0)
                {
                  float num6 = 1f - this.m_criticalTime;
                  this.m_slashColor.R = (byte) ((double) Fruit.CRITICAL_COLOUR.R + ((double) this.m_slashModColor.R - (double) Fruit.CRITICAL_COLOUR.R) * (double) num6);
                  this.m_slashColor.G = (byte) ((double) Fruit.CRITICAL_COLOUR.G + ((double) this.m_slashModColor.G - (double) Fruit.CRITICAL_COLOUR.G) * (double) num6);
                  this.m_slashColor.B = (byte) ((double) Fruit.CRITICAL_COLOUR.B + ((double) this.m_slashModColor.B - (double) Fruit.CRITICAL_COLOUR.B) * (double) num6);
                  this.m_slashColor.A = byte.MaxValue;
                }
                else
                  this.m_slashColor = this.m_slashModColor;
              }
              this.m_points[index - num1].Color = this.m_slashColor;
              this.m_points[index - num1 + 1].Color = this.m_slashColor;
            }
            else
            {
              this.m_points[index - num1].TextureCoordinate.X = this.m_points[index - num1 + 1].TextureCoordinate.X = 0.98f;
              Vector3 vector3_7 = Vector3.Cross(this.m_lastDir, new Vector3(0.0f, 0.0f, 1f));
              if ((double) vector3_7.X != 0.0 || (double) vector3_7.Y != 0.0 || (double) vector3_7.Z != 0.0)
                vector3_7.Normalize();
              vector3_7 *= num3;
              Vector3 vector3_8 = vector3_4 - vector3_7;
              this.ValFloat(vector3_8.X);
              this.ValFloat(vector3_8.Y);
              this.m_points[index - num1].Position = vector3_8;
              vector3_8 = vector3_4 + vector3_7;
              this.ValFloat(vector3_8.X);
              this.ValFloat(vector3_8.Y);
              this.m_points[index - num1 + 1].Position = vector3_8;
            }
          }
          float pointTotal = SlashEntity.pointTotals[v2];
          for (int index = 0; index < this.m_drawCount; index += 2)
          {
            float num7 = SlashEntity.pointTotals[Mortar.Math.MIN(index / 2, v2)] / pointTotal;
            this.m_points[index].TextureCoordinate.X = num7;
            this.m_points[index + 1].TextureCoordinate.X = num7;
          }
          if (this.m_drawCount > 2)
          {
            ++SlashEntity.slashes;
            Vector3 vector3_9 = (this.m_points[this.m_drawCount - 2].Position + this.m_points[this.m_drawCount - 1].Position) / 2f;
            Vector3 vector1 = vector3_9 - this.m_points[this.m_drawCount - 2].Position;
            this.m_pointPoints[0] = this.m_points[this.m_drawCount - 2];
            Vector3 vector3_10 = Vector3.Cross(vector1, new Vector3(0.0f, 0.0f, 1f)) * 2.5f;
            this.m_pointPoints[0].Position.X = vector3_9.X - vector3_10.X;
            this.m_pointPoints[0].Position.Y = vector3_9.Y - vector3_10.Y;
            this.ValFloat(this.m_pointPoints[0].Position.X);
            this.ValFloat(this.m_pointPoints[0].Position.Y);
            this.m_pointPoints[1] = this.m_points[this.m_drawCount - 2];
            this.m_pointPoints[2] = this.m_points[this.m_drawCount - 1];
            this.m_pointPoints[0].Color = this.m_points[this.m_drawCount - 1].Color;
            this.m_pointPoints[1].Color = this.m_points[this.m_drawCount - 1].Color;
            this.m_pointPoints[2].Color = this.m_points[this.m_drawCount - 1].Color;
            this.m_pointPoints[0].TextureCoordinate = new Vector2(0.98f, 0.5f);
            this.m_pointPoints[1].TextureCoordinate = new Vector2(0.98f, 0.0f);
            this.m_pointPoints[2].TextureCoordinate = new Vector2(0.98f, 1f);
          }
          this.m_drawCount -= num1;
        }
      }

      protected void PlaySwipe()
      {
        string filename = $"Sword-swipe-{1 + Mortar.Math.g_random.Rand32(6)}";
        SoundManager.GetInstance().SFXPlay(filename);
        this.m_shoutTime -= (float) (((double) Mortar.Math.g_random.RandF(0.5f) + 0.75) * (double) SlashEntity.SWIPE_SHOUT_COUNT * (1.0 + (double) (ActorManager.GetInstance().GetNumEntities(0) + ActorManager.GetInstance().GetNumEntities(1)) * 0.40000000596046448));
        this.m_shoutTime = SlashEntity.NEW_WHOA_TIME;
      }

      public void Reset()
      {
        Color white = Color.White;
        this.m_lastDir = Vector3.Zero;
        this.m_drawCount = 0;
        this.m_hitBomb = false;
        this.m_lastPos = new Vector3(-65535f, -65535f, -65535f);
        this.m_currentPoint = 0;
        this.m_pointsRemembered = 0;
        for (int index = 0; index < 6; ++index)
          this.m_pointTrail[index] = Vector3.Zero;
        if (this.m_points != null)
        {
          for (int index = 0; index < this.m_pointsCount; ++index)
          {
            this.m_points[index].Position.X = 0.0f;
            this.m_points[index].Position.Y = 0.0f;
            this.m_points[index].Position.Z = 0.0f;
            this.m_points[index].Color = white;
            this.m_points[index].TextureCoordinate.X = 0.0f;
            this.m_points[index].TextureCoordinate.Y = 0.0f;
          }
        }
        for (int index = 0; index < 11; ++index)
          this.m_comboFruitTypes[index] = -1;
        for (int index = 0; index < this.m_oldPositions.Length; ++index)
          this.m_oldPositions[index] = new Vector3(-65535f, -65535f, -65535f);
      }

      public override void Init(byte[] tpl_data, int tpl_size, Vector3? size)
      {
        this.m_col_box = (Col) new ColLine();
        this.m_updateAlways = true;
        this.m_col_length = -1f;
        this.InitPoints(80 /*0x50*/);
        this.m_splatsToMake = -1;
        this.m_slashLength = 0.0f;
        this.m_emitter = (PSPParticleEmitter) null;
        this.m_criticalTime = 0.0f;
        this.m_slashModColor = Color.White;
        this.m_slashColor = Color.White;
        this.m_comboLength = 0;
        this.m_comboPlayer = 0;
        this.m_timeSinceLastHit = SlashEntity.COMBO_TIME;
        this.m_averageDir = Vector3.Zero;
        this.m_hitBomb = false;
        this.m_pointsRemembered = 0;
        this.m_currentPoint = 0;
        for (int index = 0; index < 6; ++index)
          this.m_pointTrail[index] = Vector3.Zero;
        for (int index = 0; index < 11; ++index)
          this.m_comboFruitTypes[index] = -1;
        this.m_shoutTime = SlashEntity.NEW_WHOA_TIME;
        this.m_lastShouts[0] = -1;
        this.m_lastShouts[1] = -1;
        this.m_swipeWait = 0.0f;
        this.m_comboLength = 0;
        this.m_comboPlayer = 0;
        for (int index = 0; index < this.m_oldPositions.Length; ++index)
          this.m_oldPositions[index] = new Vector3(-65535f, -65535f, -65535f);
      }

      public static void LoadContent()
      {
        if (SlashEntity.loaded)
          return;
        SlashEntity.loaded = true;
        SlashEntity.s_slashTexture = TextureManager.GetInstance().Load("textureswp7/blade.tex");
      }

      public override void Release()
      {
        this.m_points = (VertexPositionColorTexture[]) null;
        if (this.m_emitter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
          this.m_emitter = (PSPParticleEmitter) null;
        }
        base.Release();
      }

      public static void PreUpdate(float dt)
      {
        if (SlashEntity.STOP_COUNTER < 5)
          ++SlashEntity.STOP_COUNTER;
        else
          SlashEntity.STOP = false;
        if (SlashEntity.ModColorType != 1)
          return;
        Color black = Color.Black;
        SlashEntity.UpdateModColor(ref black, dt);
      }

      private bool CombosEnabled() => Game.game_work.gameMode != Game.GAME_MODE.GM_CASINO;

      public override void Update(float dt)
      {
        float num1 = 0.0f;
        if ((double) dt > 0.0)
        {
          SlashEntity.updated = true;
          dt = Game.game_work.dt;
          num1 = Game.game_work.dt;
          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
          {
            num1 *= 0.666f;
            if ((double) PowerUpManager.GetInstance().GetDtMod() < 0.89999997615814209)
              num1 *= PowerUpManager.GetInstance().GetDtMod();
          }
        }
        this.UpdatePoints(dt);
        if (this.m_touchDown != (byte) 0 && ((int) SlashEntity.ModPowerMask & 64 /*0x40*/) == 0)
        {
          if ((double) Game.game_work.hitBombTime <= 0.0)
          {
            LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
            for (Entity ent = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); ent != null && !SlashEntity.STOP; ent = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
            {
              Fruit fruit = (Fruit) ent;
              if (!fruit.Sliced() && fruit.IsActive())
              {
                if (this.CollideWithEntity(ent))
                {
                  Vector3 lastDir = this.m_lastDir;
                  ++this.m_splatsToMake;
                  this.m_splatInterval = 0.0f;
                  this.m_timeTillNextSplat = 0.0f;
                  this.m_lastSlashDir = lastDir;
                  this.m_lastSlashFruitType = fruit.GetFruitType();
                  this.m_lastSlashPos = ent.m_pos;
                  if (!fruit.m_isMenuItem && (this.m_comboLength == 0 || this.m_comboLength > 0 && this.m_comboPlayer == fruit.ForPlayer()))
                  {
                    this.m_timeSinceLastHit = 0.0f;
                    this.m_comboFruitTypes[this.m_comboLength] = this.m_lastSlashFruitType;
                    this.m_comboPlayer = fruit.ForPlayer();
                    ++this.m_comboLength;
                    if (this.m_comboLength >= 10)
                    {
                      this.m_timeSinceLastHit = SlashEntity.COMBO_TIME - 0.005f;
                      SlashEntity.STOP = true;
                    }
                    this.m_shoutTime -= (float) this.m_comboLength * (Mortar.Math.g_random.RandF(0.5f) + 0.75f) * SlashEntity.FRUIT_HIT_SHOUT_COUNT;
                    if (this.m_comboLength > 2 && this.CombosEnabled())
                    {
                      if (this.missControl == null)
                      {
                        this.missControl = MissControl.GetFree();
                        this.missControl.MakeCombo(this.m_lastSlashPos, this.m_comboLength, this.m_comboPlayer);
                        this.missControl.m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.MissControlDeleted);
                      }
                      else
                        this.missControl.MakeCombo(this.missControl.m_pos, this.m_comboLength, this.m_comboPlayer);
                    }
                  }
                  else if ((double) this.m_timeSinceLastHit < (double) SlashEntity.COMBO_TIME)
                    this.m_timeSinceLastHit = SlashEntity.COMBO_TIME - 0.005f;
                  ent.CollisionResponse((Entity) this, 0U, 0U, ref lastDir);
                  if (fruit.m_isCritical)
                  {
                    this.m_lastSlashFruitType += Fruit.MAX_FRUIT_TYPES;
                    this.m_shoutTime -= (Mortar.Math.g_random.RandF(0.5f) + 0.75f) * SlashEntity.CRIT_SHOUT_COUNT;
                  }
                  if (fruit.m_isMenuItem)
                  {
                    SlashEntity.STOP_COUNTER = 0;
                    SlashEntity.STOP = true;
                  }
                }
                else if (((int) SlashEntity.ModPowerMask & 2) != 0)
                {
                  Vector3 vector3 = ent.m_pos - this.m_pos;
                  float num2 = vector3.Length();
                  vector3.Normalize();
                  ent.m_vel -= vector3 * Mortar.Math.MIN(num2 / 2f, 50f) * dt;
                  float v1 = ent.m_vel.Length();
                  ent.m_vel.Normalize();
                  ent.m_vel *= Mortar.Math.MIN(v1, 8f);
                }
                else if (((int) SlashEntity.ModPowerMask & 1) == 1)
                {
                  Vector3 vector3 = ent.m_pos - this.m_pos;
                  float num3 = vector3.Length();
                  vector3.Normalize();
                  ent.m_vel += vector3 * Mortar.Math.MIN(num3 / 2f, 50f) * dt;
                  float v1 = ent.m_vel.Length();
                  ent.m_vel.Normalize();
                  ent.m_vel *= Mortar.Math.MIN(v1, 8f);
                }
              }
            }
            for (Entity ent = ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); ent != null && !SlashEntity.STOP; ent = ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
            {
              Vector3 lastDir1 = this.m_lastDir;
              if (((Bomb) ent).IsActive())
              {
                if (this.CollideWithEntity(ent))
                {
                  if (((int) SlashEntity.ModPowerMask & 16 /*0x10*/) == 16 /*0x10*/)
                  {
                    ent.m_vel += this.m_lastDir * dt * 10f;
                  }
                  else
                  {
                    Vector3 lastDir2 = this.m_lastDir;
                    ent.CollisionResponse((Entity) this, 0U, 0U, ref lastDir2);
                    SlashEntity.STOP_COUNTER = 0;
                    SlashEntity.STOP = true;
                    if (((Bomb) ent).m_hit && !((Bomb) ent).m_isMenuItem)
                      this.m_hitBomb = true;
                  }
                }
                else if (((int) SlashEntity.ModPowerMask & 4) == 4)
                {
                  Vector3 vector3 = ent.m_pos - this.m_pos;
                  float num4 = vector3.Length();
                  vector3.Normalize();
                  ent.m_vel += vector3 * Mortar.Math.MIN(num4 / 2f, 50f) * dt;
                  float v1 = ent.m_vel.Length();
                  ent.m_vel.Normalize();
                  ent.m_vel *= Mortar.Math.MIN(v1, 8f);
                }
                else if (((int) SlashEntity.ModPowerMask & 8) == 8)
                {
                  Vector3 vector3 = ent.m_pos - this.m_pos;
                  float num5 = vector3.Length();
                  vector3.Normalize();
                  ent.m_vel -= vector3 * Mortar.Math.MIN(num5 / 2f, 50f) * dt;
                  float v1 = ent.m_vel.Length();
                  ent.m_vel.Normalize();
                  ent.m_vel *= Mortar.Math.MIN(v1, 8f);
                }
              }
            }
          }
        }
        else
        {
          if (this.m_touchDown == (byte) 0 && this.m_emitter != null)
          {
            PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
            this.m_emitter = (PSPParticleEmitter) null;
          }
          this.m_slashLength = 0.0f;
        }
        this.m_criticalTime = this.m_emitter == null || !WaveManager.GetInstance().CriticalMode() ? Mortar.Math.MAX(0.0f, this.m_criticalTime - dt * 2f) : Mortar.Math.MIN(1f, this.m_criticalTime + dt * 2f);
        if (SlashEntity.ModColorType < 2)
          SlashEntity.UpdateModColor(ref this.m_slashModColor, dt);
        if ((double) this.m_criticalTime > 0.0)
        {
          float num6 = 1f - this.m_criticalTime;
          this.m_slashColor.R = (byte) ((double) Fruit.CRITICAL_COLOUR.R + (double) ((int) this.m_slashModColor.R - (int) Fruit.CRITICAL_COLOUR.R) * (double) num6);
          this.m_slashColor.G = (byte) ((double) Fruit.CRITICAL_COLOUR.G + (double) ((int) this.m_slashModColor.G - (int) Fruit.CRITICAL_COLOUR.G) * (double) num6);
          this.m_slashColor.B = (byte) ((double) Fruit.CRITICAL_COLOUR.B + (double) ((int) this.m_slashModColor.B - (int) Fruit.CRITICAL_COLOUR.B) * (double) num6);
          this.m_slashColor.A = byte.MaxValue;
        }
        else
          this.m_slashColor = this.m_slashModColor;
        if ((double) this.m_timeSinceLastHit < (double) SlashEntity.COMBO_TIME)
        {
          this.m_timeSinceLastHit += num1;
          if ((double) this.m_timeSinceLastHit >= (double) SlashEntity.COMBO_TIME)
          {
            if (SlashEntity.ComboCanceledEvent != null)
              SlashEntity.ComboCanceledEvent(this);
            if (this.m_comboLength > 1 && this.m_comboFruitTypes[0] >= 0)
            {
              Game.game_work.criticalChance = Mortar.Math.MAX(2, Game.game_work.criticalChance - this.m_comboLength);
              if (this.m_comboLength > 2 && this.m_comboFruitTypes[1] >= 0)
              {
                if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                {
                  WaveManager.GetInstance().AddSpeed((float) this.m_comboLength / 3f);
                  Game.AddToCurrentScore(this.m_comboLength, this.m_comboPlayer);
                  BonusManager.GetInstance().AddCombo(this.m_comboLength);
                }
                else
                  Game.AddToCurrentScore(this.m_comboLength, this.m_comboPlayer);
                int numberOfCoins = 0;
                for (int index = 0; index < this.m_comboLength; ++index)
                {
                  if (Fruit.FruitInfo(this.m_comboFruitTypes[index]).coinsMax > 0)
                  {
                    numberOfCoins = this.m_comboLength;
                    break;
                  }
                }
                Vector3 pos = this.m_lastSlashPos;
                if (this.missControl != null)
                  pos = this.missControl.m_pos;
                Coin.MakeCoins(numberOfCoins, 1, pos, (ushort) 0, Mortar.Math.DEGREE_TO_IDX(359f));
              }
              AchievementManager.GetInstance().UnlockComboAchievement(this.m_comboLength, this.m_comboFruitTypes);
              if (this.m_comboLength >= 3)
              {
                int num7 = Fruit.FruitType("strawberry");
                for (int index = 0; index < this.m_comboLength; ++index)
                {
                  if (this.m_comboFruitTypes[index] == num7)
                  {
                    string str = "strawberry_combo_total";
                    uint hash = StringFunctions.StringHash(str);
                    Game.game_work.saveData.AddToTotal(str, hash, 1, true, false);
                    break;
                  }
                }
              }
              if (this.m_comboLength > Game.game_work.saveData.numFruitTypesInSliceCombo)
              {
                for (int index = 0; index < 11; ++index)
                  Game.game_work.saveData.sliceComboFruitTypes[index] = this.m_comboFruitTypes[index];
                Game.game_work.saveData.numFruitTypesInSliceCombo = this.m_comboLength;
                SlashEntity.s_combo = ComboChecker.CheckCombo(this.m_comboFruitTypes, this.m_comboLength);
              }
              else if (this.m_comboLength == Game.game_work.saveData.numFruitTypesInSliceCombo)
              {
                if (SlashEntity.s_combo == COMBO_TYPE.CT_NONE)
                  SlashEntity.s_combo = ComboChecker.CheckCombo(Game.game_work.saveData.sliceComboFruitTypes, Game.game_work.saveData.numFruitTypesInSliceCombo);
                if (ComboChecker.CheckCombo(this.m_comboFruitTypes, this.m_comboLength) > SlashEntity.s_combo)
                {
                  for (int index = 0; index < 11; ++index)
                    Game.game_work.saveData.sliceComboFruitTypes[index] = this.m_comboFruitTypes[index];
                  Game.game_work.saveData.numFruitTypesInSliceCombo = this.m_comboLength;
                }
              }
            }
            this.m_comboLength = 0;
            this.m_comboPlayer = 0;
            this.missControl = (MissControl) null;
            for (int index = 0; index < 11; ++index)
              this.m_comboFruitTypes[index] = -1;
          }
        }
        else
        {
          this.m_comboLength = 0;
          this.m_comboPlayer = 0;
          for (int index = 0; index < 11; ++index)
            this.m_comboFruitTypes[index] = -1;
          this.missControl = (MissControl) null;
        }
        bool flag = (double) this.m_lastDir.Length() > 35.0 * (double) Game.GAME_MODE_SCALE_FIX;
        if ((double) this.m_swipeWait > 0.0 && (double) this.m_swipeWait < (double) SlashEntity.SWIPE_SFX_WAIT || (double) this.m_lastDir.Length() < 20.0 * (double) Game.GAME_MODE_SCALE_FIX)
          this.m_swipeWait -= Game.game_work.dt;
        else if ((double) this.m_swipeWait <= 0.0 && flag)
        {
          this.PlaySwipe();
          this.m_swipeWait = SlashEntity.SWIPE_SFX_WAIT;
        }
        if ((double) this.m_timeTillNextSplat > -1.0)
          this.m_timeTillNextSplat -= dt;
        while (this.m_splatsToMake >= 0 && (double) this.m_timeTillNextSplat <= 0.0)
        {
          if ((double) this.m_lastDir.LengthSquared() > 1.0 && (double) this.m_lastDir.LengthSquared() < 10000.0)
            this.m_lastSlashDir = this.m_lastDir;
          --this.m_splatsToMake;
          this.m_splatInterval = Mortar.Math.MIN((float) ((double) this.m_splatInterval + (double) Mortar.Math.g_random.RandF(0.05f) + 0.0099999997764825821), 0.03f);
          this.m_timeTillNextSplat += this.m_splatInterval;
          SplatEntity free = SplatEntity.GetFree();
          if (free != null)
          {
            bool noSound = this.m_lastSlashFruitType < Fruit.MAX_FRUIT_TYPES && Fruit.FruitInfo(this.m_lastSlashFruitType).superFruit;
            free.MakeSplat(Game.game_work.camera.TranslatePos(this.m_pos, true, true), new Vector3(this.m_lastSlashDir.X * (Mortar.Math.g_random.RandF(0.75f) + 0.75f), this.m_lastSlashDir.Y * (Mortar.Math.g_random.RandF(0.75f) + 0.75f), 0.0f), true, noSound, this.m_lastSlashFruitType);
          }
        }
      }

      public override void Draw()
      {
      }

      public void DrawSlice()
      {
        if (SlashEntity.slashes > 0)
          SlashEntity.slashes = 0;
        if (this.m_drawCount < 4)
          return;
        if (SlashEntity.s_slashAltTexture != null)
          SlashEntity.s_slashAltTexture.Set();
        else
          SlashEntity.s_slashTexture.Set();
        MatrixManager.instance.Reset();
        MatrixManager.instance.Translate(Vector3.UnitZ);
        MatrixManager.instance.UploadCurrentMatrices(true);
        Mesh.DrawTriStripEx(this.m_points, this.m_drawCount, false);
        Mesh.DrawTriStripEx(this.m_pointPoints, 3, false);
      }

      public override void DrawUpdate(float dt)
      {
        this.m_touchDown = (byte) ((int) this.m_touchDown << 1 & 3);
        SlashEntity.updated_last_frame = SlashEntity.updated;
        SlashEntity.updated = false;
      }

      public override bool CollisionResponse(Entity p_ent2, uint col_1, uint col_2, ref Vector3 proj)
      {
        return false;
      }

      public static uint ParseSlashPowerMask(string text)
      {
        if (text != null)
        {
          uint num = StringFunctions.StringHash(text);
          for (int index = 0; index < SlashEntity.hashes.Length; ++index)
          {
            if ((int) SlashEntity.hashes[index] == (int) num)
              return (uint) (1 << index);
          }
        }
        return 0;
      }

      public bool CollideWithEntity(Entity ent)
      {
        if (this.m_col_box != null && (double) this.m_col_length > 0.0 && ent != null && ent.m_col_box != null && !Game.game_work.pause && !Game.game_work.inRetrySequence)
        {
          if (ent.m_col_box.GetType() != COLISIONOBJECT.COL_SPHERE)
            return this.m_col_box.Collide(ent.m_col_box, out Vector3 _);
          Vector3 proj;
          if (this.m_col_box.Collide(ent.m_col_box, out proj))
          {
            float num1 = ((ColSphere) ent.m_col_box).Radius * ((ColSphere) ent.m_col_box).Radius;
            if ((double) (this.m_col_box.centre - ent.m_col_box.centre).LengthSquared() < (double) num1)
              return true;
            Vector3 vector3_1 = proj + ent.m_col_box.centre;
            Vector3 vector3_2 = Vector3.Zero;
            float num2 = proj.LengthSquared();
            if ((double) num2 < (double) num1)
            {
              float num3 = Mortar.Math.Sqrt(num1 - num2);
              Vector3 vector3_3 = Vector3.Cross(proj, new Vector3(0.0f, 0.0f, 1f));
              vector3_3.Normalize();
              vector3_2 = vector3_3 * (((ColSphere) ent.m_col_box).Radius - num3);
            }
            Vector3 vector3_4 = this.m_col_box.centre - (vector3_1 + vector3_2);
            Vector3 vector3_5 = this.m_col_box.centre - (vector3_1 - vector3_2);
            if ((double) vector3_4.LengthSquared() < (double) this.m_col_length || (double) vector3_5.LengthSquared() < (double) this.m_col_length)
              return true;
          }
        }
        return false;
      }

      public bool TouchMoveX(InputEvent e)
      {
        if ((double) Game.game_work.hitBombTime > 0.0)
          return false;
        MortarRectangle windowSize = DisplayManager.GetInstance().GetWindowSize();
        Vector2 vector2 = new Vector2((float) (windowSize.right - windowSize.left), (float) (windowSize.bottom - windowSize.top));
        this.m_pos.X = (float) (((double) e.axis.absolutePos - (double) vector2.X / 2.0) * ((double) Game.SCREEN_WIDTH / (double) Game.SCREEN_SIZE_X));
        return true;
      }

      public bool TouchMoveY(InputEvent e)
      {
        if ((double) Game.game_work.hitBombTime > 0.0)
          return false;
        MortarRectangle windowSize = DisplayManager.GetInstance().GetWindowSize();
        Vector2 vector2 = new Vector2((float) (windowSize.right - windowSize.left), (float) (windowSize.bottom - windowSize.top));
        this.m_pos.Y = (float) (-((double) e.axis.absolutePos - (double) vector2.Y / 2.0) * ((double) Game.SCREEN_HEIGHT / (double) Game.SCREEN_SIZE_Y));
        return true;
      }

      public bool TouchDown(InputEvent e)
      {
        if (WaveManager.GetInstance().CriticalMode())
        {
          if (this.m_emitter == null)
          {
            uint hash = StringFunctions.StringHash("crit_hit_stars");
            this.m_emitter = PSPParticleManager.GetInstance().AddEmitter(hash, (Action<PSPParticleEmitter>) null);
            if (this.m_emitter != null)
              this.m_emitter.updateEvenIfPaused = true;
          }
          this.m_emitter.pos = Game.game_work.camera.TranslatePos(this.m_pos, true, false);
        }
        else if (SlashEntity.ModHasPartilces)
        {
          if (this.m_emitter == null)
          {
            this.m_emitter = PSPParticleManager.GetInstance().AddEmitter(SlashEntity.ModPartilcesHash, (Action<PSPParticleEmitter>) null);
            if (this.m_emitter != null)
              this.m_emitter.updateEvenIfPaused = true;
          }
        }
        else if (this.m_emitter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
          this.m_emitter = (PSPParticleEmitter) null;
        }
        if (this.m_emitter != null)
          this.m_emitter.pos = Game.game_work.camera.TranslatePos(this.m_pos, true, false);
        if ((double) Game.game_work.hitBombTime > 0.0 || !SlashEntity.updated_last_frame)
          return false;
        if (this.m_touchDown == (byte) 0)
        {
          this.Reset();
          if (SlashEntity.ModColorType == 2)
            SlashEntity.UpdateModColor(ref this.m_slashModColor, 1f);
        }
        Vector3 dir = this.m_pos - this.m_oldPositions[0];
        if (this.m_touchDown == (byte) 0 && (double) dir.X * (double) dir.X + (double) dir.Y * (double) dir.Y >= (double) SlashEntity.START_CHANGED_DIST * (double) SlashEntity.START_CHANGED_DIST || this.m_touchDown != (byte) 0 && (double) dir.X * (double) dir.X + (double) dir.Y * (double) dir.Y >= (double) SlashEntity.CHANGED_DIST * (double) SlashEntity.CHANGED_DIST)
        {
          this.AddPoint(this.m_pos, dir);
          this.m_lastPos = this.m_pos;
          for (int index = this.m_oldPositions.Length - 1; index > 0; --index)
            this.m_oldPositions[index] = this.m_oldPositions[index - 1];
          this.m_oldPositions[0] = this.m_pos;
        }
        this.m_touchDown |= (byte) 1;
        return true;
      }

      public void MissControlDeleted(HUDControl control)
      {
        if (this.missControl != control)
          return;
        this.missControl = (MissControl) null;
      }

      public static void InitModColors()
      {
        SlashEntity.ModHasPartilces = false;
        SlashEntity.ModPartilcesHash = 0U;
        SlashEntity.ModColorTime = 0.0f;
        SlashEntity.NumModColors = 1;
        SlashEntity.ModColorType = 0;
        SlashEntity.s_slashAltTexture = (Mortar.Texture) null;
        for (int index = 0; index < 16 /*0x10*/; ++index)
          SlashEntity.ModColors[index] = Color.White;
      }

      public static void UpdateModColor(ref Color colourIn, float dt)
      {
        if ((double) dt != 0.0)
        {
          if (SlashEntity.NumModColors == 1)
          {
            SlashEntity.colourOut = SlashEntity.ModColors[0];
          }
          else
          {
            SlashEntity.ModColorTime += dt * SlashEntity.ModColorChangeSpeed;
            while ((double) SlashEntity.ModColorTime > (double) SlashEntity.NumModColors)
              SlashEntity.ModColorTime -= (float) SlashEntity.NumModColors;
            if ((double) SlashEntity.ModColorTime < 0.0)
            {
              if (SlashEntity.ModColorType == 0)
              {
                while ((double) SlashEntity.ModColorTime < 0.0)
                  SlashEntity.ModColorTime += (float) SlashEntity.NumModColors;
              }
              else
                SlashEntity.ModColorTime = 0.0f;
            }
            if ((double) Mortar.Math.ABS(SlashEntity.ModColorTime - (float) (int) ((double) SlashEntity.ModColorTime + 0.5)) < 0.0099999997764825821)
            {
              SlashEntity.colourOut = SlashEntity.ModColors[(int) ((double) SlashEntity.ModColorTime + 0.5) % SlashEntity.NumModColors];
            }
            else
            {
              float num = SlashEntity.ModColorTime - (float) (int) SlashEntity.ModColorTime;
              Color modColor1 = SlashEntity.ModColors[(int) SlashEntity.ModColorTime % SlashEntity.NumModColors];
              Color modColor2 = SlashEntity.ModColors[((int) SlashEntity.ModColorTime + 1) % SlashEntity.NumModColors];
              SlashEntity.colourOut.R = (byte) ((double) modColor1.R + (double) ((int) modColor2.R - (int) modColor1.R) * (double) num);
              SlashEntity.colourOut.G = (byte) ((double) modColor1.G + (double) ((int) modColor2.G - (int) modColor1.G) * (double) num);
              SlashEntity.colourOut.B = (byte) ((double) modColor1.B + (double) ((int) modColor2.B - (int) modColor1.B) * (double) num);
              SlashEntity.colourOut.A = (byte) ((double) modColor1.A + (double) ((int) modColor2.A - (int) modColor1.A) * (double) num);
            }
          }
        }
        colourIn = SlashEntity.colourOut;
      }

      public static void SetModColors(Color[] colours, int numColors, int changeType, float speed)
      {
        SlashEntity.SetModColors(colours, numColors, changeType, speed, (string) null);
      }

      public static void SetModColors(
        Color[] colours,
        int numColors,
        int changeType,
        float speed,
        string particles)
      {
        SlashEntity.SetModColors(colours, numColors, changeType, speed, particles, (string) null);
      }

      public static void SetModColors(
        Color[] colours,
        int numColors,
        int changeType,
        float speed,
        string particles,
        string texture)
      {
        SlashEntity.ModColorType = changeType;
        SlashEntity.NumModColors = numColors;
        SlashEntity.ModColorChangeSpeed = speed;
        for (int index = 0; index < SlashEntity.NumModColors; ++index)
          SlashEntity.ModColors[index] = colours[index];
        SlashEntity.colourOut = SlashEntity.ModColors[0];
        SlashEntity.ModColorTime = 0.0f;
        SlashEntity.ModHasPartilces = false;
        SlashEntity.ModPartilcesHash = 0U;
        SlashEntity.s_slashAltTexture = texture == null ? (Mortar.Texture) null : TextureManager.GetInstance().Load(texture);
        if (particles == null)
          return;
        SlashEntity.ModPartilcesHash = StringFunctions.StringHash(particles);
        if (!PSPParticleManager.GetInstance().EmitterExists(SlashEntity.ModPartilcesHash))
          return;
        SlashEntity.ModHasPartilces = true;
      }

      public static int ParseSlashModColorType(string text)
      {
        if (text != null)
        {
          uint num = StringFunctions.StringHash(text);
          for (int slashModColorType = 0; slashModColorType < 4; ++slashModColorType)
          {
            if ((int) num == (int) SlashEntity.ParseSlashModColorTypehashes[slashModColorType])
              return slashModColorType;
          }
        }
        return 0;
      }

      public static void CleanupSlash()
      {
        SlashEntity.s_slashTexture = (Mortar.Texture) null;
        SlashEntity.s_slashAltTexture = (Mortar.Texture) null;
        SlashEntity.loaded = false;
      }

      public delegate void ComboCanceled(SlashEntity e);

      public enum SLASH_POWER
      {
        SLASH_POWER_PUSH_FRUIT = 1,
        SLASH_POWER_PULL_FRUIT = 2,
        SLASH_POWER_PUSH_BOMB = 4,
        SLASH_NUM_POWERS = 7,
        SLASH_POWER_PULL_BOMB = 8,
        SLASH_POWER_BOMB_HIT = 16, // 0x00000010
        SLASH_POWER_FRUIT_BOUNCE = 32, // 0x00000020
        SLASH_POWER_NO_COLLISION = 64, // 0x00000040
      }

      public enum SlashColorChangeType
      {
        SCCT_NONE,
        SCCT_LERP,
        SCCT_PER_SLASH,
        SCCT_CONTINUOUS,
        SCCT_MAX,
      }
    }
}
