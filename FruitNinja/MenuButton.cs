// Decompiled with JetBrains decompiler
// Type: FruitNinja.MenuButton
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class MenuButton : HUDControl3d
    {
      public bool m_isTrialLockable;
      protected int m_goto;
      protected Entity m_link;
      protected int m_fruitType;
      protected MenuButton.MenuCallback m_callback;
      protected MenuButton.MenuCallback m_clickedCallback;
      protected int m_scaleInOut;
      protected bool m_inRegion;
      protected bool m_inOuterRegion;
      protected bool m_prevInRegion;
      protected bool m_prevInOuterRegion;
      protected int m_touchInRegion;
      protected Vector3 m_touchPos;
      protected float m_scratchRot;
      protected float m_scratchScale;
      protected bool m_scratchFlip;
      protected float m_rotationSpeed;
      protected static Texture m_scratchTexture;
      protected static Texture s_newTexture;
      protected float m_loadingSymbolTime;
      protected float m_newSymbol;
      protected Vector3 m_originalEntityScale;
      public static Texture m_loadingTexture;
      public bool m_growOnPressed;
      public bool m_clearOthers;
      public Vector3 m_originalScale;
      public bool m_hasOwnScale;
      public bool m_enabled;
      public Entity m_entity;
      public bool m_triggerOnBackPress;
      public float m_overallScratchScale;
      public Vector3 m_newSymbolOffset;
      public float m_innerBoundX;
      public float m_innerBoundY;
      public float m_outerBound;
      public float m_shakeTime;
      private static GameVertex[] symbo_tris = new GameVertex[48 /*0x30*/];

      public static void MenuCallbackClicked()
      {
      }

      public static float POP_IN_OUT_TIME => 0.15f;

      public static ushort POP_SINE => Math.DEGREE_TO_IDX(90f);

      public static float SCRATCH_SCALE => 1.125f;

      public static float BOB_TIME => 0.5f;

      public static float BOB_AMT => 6f;

      public void SetInnerBound(float bound)
      {
        this.m_innerBoundX = bound;
        this.m_innerBoundY = bound;
      }

      public void SetInnerBound(float boundX, float boundY)
      {
        this.m_innerBoundX = boundX;
        this.m_innerBoundY = boundY;
      }

      private void UpdateTouchPosition()
      {
        if (this.m_touchInRegion == -1)
          return;
        this.m_touchPos = Game.game_work.touchPositions[this.m_touchInRegion];
      }

      public MenuButton(Texture texture, Vector3 pos, MenuButton.MenuCallback call)
        : this(texture, pos, call, -1)
      {
      }

      public MenuButton(Texture texture, Vector3 pos, MenuButton.MenuCallback call, int fruitType)
        : this(texture, pos, call, fruitType, Vector3.Zero)
      {
      }

      public MenuButton(
        Texture texture,
        Vector3 pos,
        MenuButton.MenuCallback call,
        int fruitType,
        Vector3 scale)
        : this(texture, pos, call, fruitType, scale, new MenuButton.MenuCallback(MenuButton.MenuCallbackClicked))
      {
      }

      public MenuButton(
        Texture texture,
        Vector3 pos,
        MenuButton.MenuCallback call,
        int fruitType,
        Vector3 scale,
        MenuButton.MenuCallback clickedCall)
      {
        this.m_texture = texture;
        this.Init(pos, call, fruitType, scale, clickedCall);
      }

      public MenuButton(string texture, Vector3 pos, MenuButton.MenuCallback call)
        : this(texture, pos, call, -1)
      {
      }

      public MenuButton(string texture, Vector3 pos, MenuButton.MenuCallback call, int fruitType)
        : this(texture, pos, call, fruitType, Vector3.Zero, false)
      {
      }

      public MenuButton(
        string texture,
        Vector3 pos,
        MenuButton.MenuCallback call,
        int fruitType,
        Vector3 scale,
        bool localise)
        : this(texture, pos, call, fruitType, scale, localise, new MenuButton.MenuCallback(MenuButton.MenuCallbackClicked))
      {
      }

      public MenuButton(
        string texture,
        Vector3 pos,
        MenuButton.MenuCallback call,
        int fruitType,
        Vector3 scale,
        bool localise,
        MenuButton.MenuCallback clickedCall)
      {
        if (localise)
        {
          this.m_texture = TextureManager.GetInstance().Load(texture, localise);
        }
        else
        {
          string texture1 = "textureswp7/" + texture;
          try
          {
            this.m_texture = TextureManager.GetInstance().Load(texture1);
          }
          catch
          {
          }
        }
        this.Init(pos, call, fruitType, scale, clickedCall);
      }

      public void Init(
        Vector3 pos,
        MenuButton.MenuCallback call,
        int fruitType,
        Vector3 scale,
        MenuButton.MenuCallback clickedCall)
      {
        this.m_pos = pos;
        this.m_fruitType = fruitType;
        this.m_callback = call;
        this.m_clickedCallback = clickedCall;
        this.m_selfCleanUp = false;
        this.m_clearOthers = true;
        this.m_triggerOnBackPress = false;
        this.m_hasOwnScale = (double) Math.Abs(scale.X) + (double) Math.Abs(scale.Y) > 0.0;
        this.m_scale = Vector3.Zero;
        this.m_originalScale = scale;
        this.m_link = (Entity) null;
        this.m_originalEntityScale = scale;
        this.m_touchInRegion = -1;
        this.m_inRegion = false;
        this.m_prevInRegion = false;
        this.m_inOuterRegion = false;
        this.m_prevInOuterRegion = false;
        this.m_rotationSpeed = 0.0f;
        this.m_overallScratchScale = 1f;
        this.m_growOnPressed = true;
        this.m_enabled = true;
        this.m_loadingSymbolTime = -1f;
        this.m_newSymbol = -1f;
        this.m_innerBoundX = 5f;
        this.m_innerBoundY = 5f;
        this.m_outerBound = 100f;
        this.m_shakeTime = 0.0f;
        this.m_rotation = 0.0f;
        if (this.m_fruitType >= 0)
        {
          this.m_scratchFlip = Math.g_random.Rand32(2) != 0;
          this.m_scratchRot = (float) (Math.g_random.Rand32(40) - 20);
          this.m_link = ActorManager.GetInstance().Add(this.m_fruitType < Fruit.MAX_FRUIT_TYPES ? EntityTypes.ENTITY_BEGIN : EntityTypes.ENTITY_BOMB);
          this.m_link.m_pos = pos;
          this.m_link.m_vel = Vector3.Zero;
          this.m_link.Init((byte[]) null, this.m_fruitType, new Vector3?());
          this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT;
          this.m_rotationSpeed = Math.g_random.RandF(4f) + 8f;
          if (Math.g_random.Rand32(2) == 0)
            this.m_rotationSpeed = -this.m_rotationSpeed;
          if (this.m_fruitType < Fruit.MAX_FRUIT_TYPES)
          {
            ((Fruit) this.m_link).m_rotation_speed[0] *= 0.2f;
            ((Fruit) this.m_link).m_rotation_speed[0].X = Math.MAX(Math.Abs(((Fruit) this.m_link).m_rotation_speed[0].X), 0.75f) * (float) Math.MATH_SIGN(((Fruit) this.m_link).m_rotation_speed[0].X);
            ((Fruit) this.m_link).m_rotation_speed[0].Y = Math.MAX(Math.Abs(((Fruit) this.m_link).m_rotation_speed[0].Y), 0.5f) * (float) Math.MATH_SIGN(((Fruit) this.m_link).m_rotation_speed[0].Y);
            ((Fruit) this.m_link).m_rotation_speed[1] = ((Fruit) this.m_link).m_rotation_speed[0];
            ((Fruit) this.m_link).EnableGravity(false);
            ((Fruit) this.m_link).m_hudControl = this;
            ((Fruit) this.m_link).m_isMenuItem = true;
            ((Fruit) this.m_link).m_z = 150f;
            if (!this.m_hasOwnScale)
              this.m_originalScale = this.m_link.m_cur_scale * 200f;
          }
          else
          {
            ((Bomb) this.m_link).EnableGravity(false);
            ((Bomb) this.m_link).SetCallback(call, this);
            ((Bomb) this.m_link).m_z = 150f;
            this.m_link.m_cur_scale *= 0.85f;
            if (!this.m_hasOwnScale)
              this.m_originalScale = Vector3.One * 2f * Game.game_work.bombSize;
          }
        }
        else if (!this.m_hasOwnScale)
        {
          this.m_originalScale.X = (float) (this.m_texture.GetWidth() + 1U);
          this.m_originalScale.Y = (float) (this.m_texture.GetHeight() + 1U);
          this.m_scale = this.m_originalScale;
        }
        this.m_entity = this.m_link;
        this.m_originalEntityScale = Vector3.Zero;
        this.m_scaleInOut = 0;
        this.m_newSymbolOffset = new Vector3(0.85f, 0.85f, 0.0f);
      }

      ~MenuButton() => this.Release();

      public void SetCallback(MenuButton.MenuCallback call) => this.m_callback = call;

      public virtual void Clicked()
      {
      }

      public override void Reset()
      {
        if (this.m_link != null && this.m_fruitType < Fruit.MAX_FRUIT_TYPES)
          ((Fruit) this.m_link).m_isLockedMenuButton = this.m_isTrialLockable;
        if (!this.partOfPopup || this.m_link == null)
          return;
        if (this.m_fruitType < Fruit.MAX_FRUIT_TYPES)
          this.m_link.partOfPopup = true;
        else
          this.m_link.partOfPopup = true;
      }

      public override void Release()
      {
        if (this.m_entity != null && this.m_fruitType < Fruit.MAX_FRUIT_TYPES)
          ((Fruit) this.m_entity).m_hudControl = (MenuButton) null;
        if (this.m_entity != null && this.m_fruitType == Fruit.MAX_FRUIT_TYPES)
          ((Bomb) this.m_entity).m_hudControl = (MenuButton) null;
        this.m_texture = (Texture) null;
      }

      public override void Init() => this.Reset();

      public void TouchReleased()
      {
        if (PopOverControl.IsInPopup && !this.partOfPopup)
          return;
        if (this.m_fruitType < 0)
          this.m_callback();
        else if (this.m_link != null)
          Game.game_work.tutorialControl.ButtonPressedAtPos(this);
        this.m_clickedCallback();
      }

      public override void Update(float dt)
      {
        if ((double) this.m_loadingSymbolTime >= 0.0)
        {
          this.m_loadingSymbolTime += dt * 8f;
          if ((double) this.m_loadingSymbolTime >= 8.0)
            this.m_loadingSymbolTime = 0.0f;
        }
        if ((double) this.m_newSymbol >= 0.0)
        {
          this.m_newSymbol += dt / MenuButton.BOB_TIME;
          if ((double) this.m_loadingSymbolTime >= 1.0)
            this.m_newSymbol = 0.0f;
        }
        if (this.m_fruitType >= 0)
        {
          if ((double) dt > 0.0)
          {
            this.m_rotation += this.m_rotationSpeed * dt;
            if ((double) this.m_rotation < 0.0)
              this.m_rotation += 360f;
            if ((double) this.m_rotation > 360.0)
              this.m_rotation -= 360f;
          }
          this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT;
          if (this.m_link != null)
          {
            if ((double) this.m_originalEntityScale.X == 0.0)
            {
              this.m_originalEntityScale = this.m_link.m_cur_scale;
              this.m_link.m_cur_scale = Vector3.Zero;
            }
            else
              this.m_link.m_cur_scale = this.m_originalEntityScale * (this.m_scale.X / this.m_originalScale.X);
            this.m_link.m_pos = this.m_pos;
            if (this.m_fruitType < Fruit.MAX_FRUIT_TYPES)
            {
              ((Fruit) this.m_link).m_pos2 = this.m_pos;
              if (((Fruit) this.m_link).m_isSliced)
              {
                if ((double) (this.m_link.m_vel - ((Fruit) this.m_link).m_vel2).LengthSquared() > 0.10000000149011612)
                {
                  this.m_callback();
                  Game.game_work.tutorialControl.ResetTutePos();
                  this.m_link.m_cur_scale = this.m_originalEntityScale;
                  if (this.m_clearOthers)
                    Game.ClearMenuItems();
                }
                this.m_link = (Entity) null;
              }
            }
            else
            {
              this.m_pos.Z = -5f;
              this.m_link.m_pos.Z = 0.0f;
              if (!((Bomb) this.m_link).Enabled())
              {
                this.m_link.m_cur_scale = this.m_originalEntityScale;
                this.m_link = (Entity) null;
              }
            }
            if (this.m_scaleInOut < (int) MenuButton.POP_SINE)
            {
              this.m_scaleInOut = Math.MIN((int) MenuButton.POP_SINE, (int) ((double) this.m_scaleInOut + (double) dt * ((double) MenuButton.POP_SINE / (double) MenuButton.POP_IN_OUT_TIME)));
              this.m_scale = this.m_originalScale * Math.SinIdx((ushort) this.m_scaleInOut) / Math.SinIdx(MenuButton.POP_SINE);
            }
            else
              this.m_scale = this.m_originalScale;
          }
          else
          {
            if (this.m_entity != null && (double) this.m_entity.m_vel.X == 0.0 && (double) this.m_entity.m_vel.Y == 0.0)
              this.m_entity.m_cur_scale = this.m_originalEntityScale * (this.m_scale.X / this.m_originalScale.X);
            else if (this.m_entity != null)
              this.m_entity.m_cur_scale = this.m_originalEntityScale;
            this.m_scaleInOut -= (int) ((double) dt * ((double) MenuButton.POP_SINE / (double) MenuButton.POP_IN_OUT_TIME));
            if (this.m_scaleInOut <= 0)
            {
              this.m_scaleInOut = 0;
              this.m_terminate = true;
            }
            this.m_pos.Z = -5f;
            this.m_scale = this.m_originalScale * Math.SinIdx((ushort) this.m_scaleInOut) / Math.SinIdx(MenuButton.POP_SINE);
          }
        }
        if (this.m_enabled)
        {
          if (this.m_link != null && TheGame.instance.BackButtonWasPressed && this.m_triggerOnBackPress)
          {
            Vector3 proj = new Vector3(1f, 0.0f, 0.0f);
            this.m_link.CollisionResponse((Entity) null, 0U, 0U, ref proj);
          }
          else if (TheGame.instance.BackButtonWasPressed && this.m_triggerOnBackPress)
            this.TouchReleased();
          float innerBoundX = this.m_innerBoundX;
          float innerBoundY = this.m_innerBoundY;
          float xMin = this.m_pos.X - this.m_originalScale.X * 0.5f - innerBoundX;
          float xMax = this.m_pos.X + this.m_originalScale.X * 0.5f + innerBoundX;
          float yMin = this.m_pos.Y - this.m_originalScale.Y * 0.5f - innerBoundY;
          float yMax = this.m_pos.Y + this.m_originalScale.Y * 0.5f + innerBoundY;
          if (this.m_touchInRegion == -1)
          {
            this.m_touchInRegion = Game.TouchInRegion(xMin, xMax, yMin, yMax);
            if (this.m_touchInRegion != -1)
            {
              if (Game.IsTouchDown(this.m_touchInRegion) != 2)
                this.m_touchInRegion = -1;
            }
            else
              this.UpdateTouchPosition();
            if (this.m_fruitType < 0)
              this.m_scale = this.m_originalScale;
          }
          else if (Game.IsTouchDown(this.m_touchInRegion) == 0)
          {
            this.m_touchInRegion = -1;
            if ((double) this.m_touchPos.X >= (double) xMin && (double) this.m_touchPos.X <= (double) xMax && (double) this.m_touchPos.Y >= (double) yMin && (double) this.m_touchPos.Y <= (double) yMax)
              this.TouchReleased();
          }
          else
          {
            this.UpdateTouchPosition();
            if (this.m_fruitType < 0 && this.m_growOnPressed)
            {
              if ((double) this.m_touchPos.X >= (double) xMin && (double) this.m_touchPos.X <= (double) xMax && (double) this.m_touchPos.Y >= (double) yMin && (double) this.m_touchPos.Y <= (double) yMax)
                this.m_scale = this.m_originalScale * 1.25f;
              else
                this.m_scale = this.m_originalScale;
            }
          }
        }
        else if (this.m_fruitType < 0)
          this.m_scale = this.m_originalScale;
        this.m_scratchScale = this.m_scale.X * MenuButton.SCRATCH_SCALE * this.m_overallScratchScale;
        if ((double) this.m_shakeTime <= 0.0)
          return;
        this.m_shakeTime -= dt;
        if ((double) this.m_shakeTime >= 0.0)
          return;
        this.m_shakeTime = 0.0f;
      }

      public override void PreDraw(float[] tintChannels)
      {
      }

      public override void Draw(float[] tintChannels)
      {
        if (PopOverControl.IsInPopup && !this.partOfPopup)
          return;
        int a = (int) byte.MaxValue;
        if (this.m_fruitType >= 0)
          a = Math.CLAMP((int) (256.0 * (double) this.m_scaleInOut / (double) MenuButton.POP_SINE), 0, (int) byte.MaxValue);
        if (this.m_drawOrder == HUD.HUD_ORDER.HUD_ORDER_BEFORE_SPLAT && a > 0)
        {
          this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
          if (MenuButton.m_scratchTexture == null)
            return;
          MenuButton.m_scratchTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(new Vector3(this.m_scratchFlip ? -1f : 1f, 1f, 1f) * this.m_scratchScale);
          MatrixManager.GetInstance().Translate(new Vector3(this.m_pos.X, this.m_pos.Y, -5500f));
          MatrixManager.GetInstance().UploadCurrentMatrices(true);
          var tempTint = HUDControl.TintWhite(tintChannels);
          tempTint.A = (byte)a;

          Mesh.DrawQuad(tempTint, 0.0f, 1f, 0.0f, 1f);
            }
        else
        {
          if (a > 0 && this.m_texture != null)
          {
            if ((double) this.m_pos.X < (double) -(240U /*0xF0*/ + this.m_texture.w) || (double) this.m_pos.X > (double) (240U /*0xF0*/ + this.m_texture.w) || (double) this.m_pos.Y < (double) -(160U /*0xA0*/ + this.m_texture.h) || (double) this.m_pos.Y > (double) (160U /*0xA0*/ + this.m_texture.h))
              return;
            this.m_texture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(this.m_scale);
            if ((double) this.m_rotation != 0.0)
              MatrixManager.GetInstance().RotZ(this.m_rotation);
            MatrixManager.GetInstance().Scale(new Vector3(0.9f, 1f, 1f));
            Vector3 zero = Vector3.Zero;
            if ((double) this.m_shakeTime > 0.0)
              zero += new Vector3(Math.g_random.RandF(6f) - 3f, Math.g_random.RandF(6f) - 3f, 0.0f);
            MatrixManager.GetInstance().Translate(this.m_pos + zero);
            MatrixManager.GetInstance().UploadCurrentMatrices(true);
            Mesh.DrawQuad(this.m_enabled ? HUDControl.TintColor(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, a), tintChannels) : HUDControl.TintColor(new Color(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/, a), tintChannels), this.m_uvs[0].X, this.m_uvs[1].X, this.m_uvs[0].Y, this.m_uvs[1].Y);
          }
          if ((double) this.m_newSymbol >= 0.0 && !Game.isWP7TrialMode())
          {
            float num = this.m_scale.X / this.m_originalScale.X;
            MenuButton.s_newTexture.Set();
            MatrixManager.GetInstance().Reset();
            MatrixManager.GetInstance().Scale(new Vector3(64f * num, 32f * num, 0.0f));
            MatrixManager.GetInstance().Translate(this.m_pos + new Vector3((float) ((double) this.m_newSymbolOffset.X * (double) this.m_originalScale.X * 0.5), (float) ((double) this.m_newSymbolOffset.Y * (double) this.m_originalScale.Y * 0.5 + (double) Math.ABS(Math.SinIdx(Math.DEGREE_TO_IDX(180f * this.m_newSymbol))) * (double) MenuButton.BOB_AMT), 0.0f) * num);
            MatrixManager.GetInstance().UploadCurrentMatrices(true);
            Mesh.DrawQuad(this.m_enabled ? HUDControl.TintColor(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, a), tintChannels) : HUDControl.TintColor(new Color(128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/, a), tintChannels), 0.0f, 1f, 0.0f, 1f);
          }
          if ((double) this.m_loadingSymbolTime < 0.0 || MenuButton.m_loadingTexture == null)
            return;
          int num1 = 7 - (int) this.m_loadingSymbolTime % 8;
          if (true)
          {
            float num2 = 0.6f;
            float num3 = 0.075f;
            for (int index1 = 0; index1 < 8; ++index1)
            {
              Vector3 vector3_1 = new Vector3(Math.SinIdx(Math.DEGREE_TO_IDX((float) (index1 * 45))) * 0.5f, Math.CosIdx(Math.DEGREE_TO_IDX((float) (index1 * 45))) * 0.5f, 0.0f);
              Vector3 vector3_2 = new Vector3(Math.SinIdx(Math.DEGREE_TO_IDX((float) (index1 * 45 + 90))) * num3, Math.CosIdx(Math.DEGREE_TO_IDX((float) (index1 * 45 + 90))) * num3, 0.0f);
              MenuButton.symbo_tris[index1 * 6].X = vector3_1.X - vector3_2.X;
              MenuButton.symbo_tris[index1 * 6].Y = vector3_1.Y - vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6].u = 0.0f;
              MenuButton.symbo_tris[index1 * 6].v = 0.0f;
              MenuButton.symbo_tris[index1 * 6 + 1].X = vector3_1.X + vector3_2.X;
              MenuButton.symbo_tris[index1 * 6 + 1].Y = vector3_1.Y + vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6 + 1].u = 1f;
              MenuButton.symbo_tris[index1 * 6 + 1].v = 0.0f;
              MenuButton.symbo_tris[index1 * 6 + 2].X = vector3_1.X * num2 - vector3_2.X;
              MenuButton.symbo_tris[index1 * 6 + 2].Y = vector3_1.Y * num2 - vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6 + 2].u = 0.0f;
              MenuButton.symbo_tris[index1 * 6 + 2].v = 1f;
              MenuButton.symbo_tris[index1 * 6 + 3].X = vector3_1.X * num2 - vector3_2.X;
              MenuButton.symbo_tris[index1 * 6 + 3].Y = vector3_1.Y * num2 - vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6 + 3].u = 0.0f;
              MenuButton.symbo_tris[index1 * 6 + 3].v = 1f;
              MenuButton.symbo_tris[index1 * 6 + 4].X = vector3_1.X + vector3_2.X;
              MenuButton.symbo_tris[index1 * 6 + 4].Y = vector3_1.Y + vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6 + 4].u = 1f;
              MenuButton.symbo_tris[index1 * 6 + 4].v = 0.0f;
              MenuButton.symbo_tris[index1 * 6 + 5].X = vector3_1.X * num2 + vector3_2.X;
              MenuButton.symbo_tris[index1 * 6 + 5].Y = vector3_1.Y * num2 + vector3_2.Y;
              MenuButton.symbo_tris[index1 * 6 + 5].u = 1f;
              MenuButton.symbo_tris[index1 * 6 + 5].v = 1f;
              for (int index2 = 0; index2 < 6; ++index2)
              {
                MenuButton.symbo_tris[index1 * 6 + index2].nz = 1f;
                MenuButton.symbo_tris[index1 * 6 + index2].Z = 0.0f;
              }
            }
          }
          for (int index3 = 0; index3 < 8; ++index3)
          {
            int num4 = Math.CLAMP((num1 + index3) % 8 * 32 /*0x20*/, 64 /*0x40*/, (int) byte.MaxValue);
            Color col = new Color(num4, num4, num4, 200);
            col = HUDControl.TintColor(col, tintChannels);
            for (int index4 = 0; index4 < 6; ++index4)
              MenuButton.symbo_tris[index3 * 6 + index4].color = col;
          }
          MenuButton.m_loadingTexture.Set();
          MatrixManager.GetInstance().Reset();
          MatrixManager.GetInstance().Scale(Vector3.One * this.m_scale.Y * 0.75f);
          MatrixManager.GetInstance().Translate(this.m_pos);
          MatrixManager.GetInstance().UploadCurrentMatrices(true);
          Mesh.DrawTriList(MenuButton.symbo_tris, 48 /*0x30*/, false);
        }
      }

      public void Shake(float time) => this.m_shakeTime = time;

      public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_BUTTON;

      public override void Skip() => this.m_scaleInOut = (int) MenuButton.POP_SINE;

      public static void LoadContent()
      {
        MenuButton.m_scratchTexture = TextureManager.GetInstance().Load("textureswp7/scratchs.tex");
        MenuButton.m_loadingTexture = (Texture) null;
        MenuButton.s_newTexture = TextureManager.GetInstance().Load("new_item.tex", true);
      }

      public static void UnLoadContent()
      {
        MenuButton.m_scratchTexture = (Texture) null;
        MenuButton.m_loadingTexture = (Texture) null;
        MenuButton.s_newTexture = (Texture) null;
      }

      public void SetLoadingSymbol(bool loading)
      {
        if ((double) this.m_loadingSymbolTime >= 0.0)
        {
          if (loading)
            return;
          this.m_loadingSymbolTime = -1f;
        }
        else
        {
          if (!loading)
            return;
          this.m_loadingSymbolTime = 0.0f;
        }
      }

      public bool IsLoadingSymbol() => (double) this.m_loadingSymbolTime >= 0.0;

      public void SetNewSymbol(bool hasNew)
      {
        if (hasNew)
        {
          if ((double) this.m_newSymbol >= 0.0)
            return;
          this.m_newSymbol = 0.0f;
        }
        else
        {
          if ((double) this.m_newSymbol < 0.0)
            return;
          this.m_newSymbol = -1f;
        }
      }

      public bool HasNewSymbol() => (double) this.m_newSymbol >= 0.0;

      public void Remove()
      {
        if (this.m_entity == null || ((Fruit) this.m_entity).Sliced())
          return;
        ((Fruit) this.m_entity).m_isSliced = true;
        this.m_entity.m_vel = new Vector3(Math.g_random.RandF(10f), -Math.g_random.RandF(5f), 0.0f);
        ((Fruit) this.m_entity).m_vel2 = this.m_entity.m_vel;
        ((Fruit) this.m_entity).m_hudControl = (MenuButton) null;
        this.m_entity = (Entity) null;
        this.m_link = (Entity) null;
      }

      public void RemoveNoShow()
      {
        if (this.m_entity == null || ((Fruit) this.m_entity).Sliced())
          return;
        ((Fruit) this.m_entity).m_isSliced = true;
        this.m_entity.m_vel = new Vector3(Math.g_random.RandF(10f), -Math.g_random.RandF(5f), 0.0f);
        ((Fruit) this.m_entity).m_vel2 = this.m_entity.m_vel;
        ((Fruit) this.m_entity).m_hudControl = (MenuButton) null;
        this.m_entity.m_pos.Y += 400f;
        this.m_entity = (Entity) null;
        this.m_link = (Entity) null;
      }

      public delegate void MenuCallback();
    }
}
