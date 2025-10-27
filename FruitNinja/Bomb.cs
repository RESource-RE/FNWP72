// Decompiled with JetBrains decompiler
// Type: FruitNinja.Bomb
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;

namespace FruitNinja
{

    public class Bomb : Entity
    {
      protected float m_newBlastWait;
      protected MenuButton.MenuCallback m_callback;
      protected int m_forPlayer;
      public bool m_hit;
      public float m_z;
      public ushort[] m_rotation_speed = new ushort[2];
      public ushort[] m_rotation_piece = new ushort[2];
      public bool m_disabled;
      public byte PAD00;
      public short PAD01;
      public PSPParticleEmitter m_emitter;
      public bool m_gravityEnabled;
      public MenuButton m_hudControl;
      public bool m_isMenuItem;
      public Vector3 m_gravity;
      public float m_chuckWait;
      public Vector3 m_orig_scale;
      public float m_explosionTime;
      public static Model[] m_bombModel = new Model[Game.MAX_PLAYERS * 2];
      public static uint[] particleHash = new uint[Game.MAX_PLAYERS * 2];
      public static Model s_arcadeBombModel = (Model) null;
      public static bool s_isContentLoaded = false;
      public static Texture s_minus_10 = (Texture) null;
      public static Texture m_blastTexture = (Texture) null;
      public static Texture[] s_flashTexture = new Texture[2];
      public static Bomb highestBomb = (Bomb) null;
      public static bool s_sfxPlayedThisFrame = false;
      public float m_dtMod;

      public static int MAX_ROTATION_SPEED => 8;

      public static int MAX_ROTATION_ANGLE => 359;

      public static float SIXTY_FPS_DT => 0.0166666675f;

      public static float FUSE_DIST(float mcsx)
      {
        return (float) (0.89999997615814209 * (double) mcsx * 100.0);
      }

      public static float BOMB_CHUCK_WAIT => 0.2f;

      public bool GetHit() => this.m_hit;

      public static void LoadContent()
      {
        if (Bomb.s_isContentLoaded)
          return;
        Bomb.m_bombModel[0] = MeshManager.GetInstance().Load("models/Fruit/Bomb.mmd");
        Bomb.particleHash[0] = StringFunctions.StringHash("bomb_smoke");
        int index1 = 1;
        Bomb.m_bombModel[index1] = MeshManager.GetInstance().Load("models/Fruit/Bomb_purple.mmd");
        Bomb.s_minus_10 = TextureManager.GetInstance().Load("textureswp7/minus_10.tex");
        for (int index2 = 1; index2 < index1; ++index2)
          Bomb.particleHash[index2] = Bomb.particleHash[0];
        Bomb.particleHash[index1] = StringFunctions.StringHash("purple_bomb_smoke");
        Bomb.s_isContentLoaded = true;
      }

      public static float GetHeighestBomb()
      {
        float heighestBomb = -10000f;
        uint num1 = 0;
        ActorManager instance = ActorManager.GetInstance();
        int idx = (int) num1;
        uint num2 = (uint) (idx + 1);
        for (Entity entity = instance.GetEntity(EntityTypes.ENTITY_BOMB, (uint) idx); entity != null; entity = ActorManager.GetInstance().GetEntity(EntityTypes.ENTITY_BOMB, num2++))
        {
          float num3 = entity.m_pos.Y + Game.SCREEN_HEIGHT / 2f;
          if (Game.IsMultiplayer())
            num3 = Game.SCREEN_WIDTH / 2f - Mortar.Math.ABS(entity.m_pos.X);
          if (!((Bomb) entity).m_isMenuItem && (double) num3 > (double) heighestBomb)
            heighestBomb = num3;
        }
        return heighestBomb;
      }

      public override void Init(byte[] tpl_data, int tpl_size, Vector3? size)
      {
        if (this.m_col_box == null)
          this.m_col_box = (Col) new ColSphere();
        float num = size.HasValue ? size.Value.X : 1f;
        this.m_col_box.centre = new Vector3(this.m_pos.X, this.m_pos.Y, 0.0f);
        ((ColSphere) this.m_col_box).Radius = Game.game_work.bombCollision * 0.5f * num;
        if (Bomb.m_blastTexture == null)
          Bomb.m_blastTexture = TextureManager.GetInstance().Load("Textureswp7/bomb_explode.tex");
        this.m_forPlayer = 0;
        this.m_updateAlways = true;
        this.m_destroy = false;
        this.m_disabled = false;
        this.m_gravityEnabled = true;
        this.m_hit = false;
        this.m_dtMod = 1f;
        this.m_explosionTime = 0.0f;
        this.m_newBlastWait = 0.6f;
        for (int index = 0; index < 2; ++index)
        {
          this.m_rotation_speed[index] = (ushort) (Mortar.Math.g_random.Rand32(Bomb.MAX_ROTATION_SPEED - 1) + 1);
          this.m_rotation_piece[index] = (ushort) Mortar.Math.g_random.Rand32(Bomb.MAX_ROTATION_ANGLE);
        }
        this.m_isMenuItem = false;
        this.m_emitter = (PSPParticleEmitter) null;
        this.m_hudControl = (MenuButton) null;
        this.m_cur_scale = Vector3.One * Game.game_work.bombSize * 0.01f * num;
        this.m_orig_scale = this.m_cur_scale;
        this.m_chuckWait = 0.0f;
        this.m_gravity = new Vector3(0.0f, (float) -((double) Game.GRAVITY / (double) Bomb.SIXTY_FPS_DT), 0.0f);
        this.m_z = GameTask.GetBombZPosition();
      }

      public override void Release()
      {
        if (this.m_emitter != null)
        {
          PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
          this.m_emitter = (PSPParticleEmitter) null;
        }
        if (this.m_hudControl != null && this.m_hudControl.m_entity == this)
          this.m_hudControl.m_entity = (Entity) null;
        if (Bomb.highestBomb == this)
          Bomb.highestBomb = (Bomb) null;
        base.Release();
      }

      public override void Update(float dt)
      {
        dt *= this.m_dtMod;
        float num1 = dt / Bomb.SIXTY_FPS_DT;
        if (this.m_hit)
        {
          if (!this.m_isMenuItem)
          {
            this.m_newBlastWait -= Game.game_work.dt;
            if ((double) this.m_newBlastWait < 0.0)
            {
              Entity entity = ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB_BLAST);
              entity.m_pos = this.m_pos;
              entity.Init((byte[]) null, 0, new Vector3?());
              this.m_newBlastWait = 0.05f;
            }
          }
          else
          {
            if (this.m_gravityEnabled)
            {
              Bomb bomb = this;
              bomb.m_vel = bomb.m_vel - this.m_gravity * dt;
              if ((double) this.m_gravity.Y < 0.0 && (double) this.m_vel.Y < 0.0 || (double) this.m_gravity.Y > 0.0 && (double) this.m_vel.Y > 0.0 || (double) this.m_gravity.X < 0.0 && (double) this.m_vel.X < 0.0 || (double) this.m_gravity.X > 0.0 && (double) this.m_vel.X > 0.0)
              {
                float num2 = this.m_gravity.Length();
                this.m_gravity.Normalize();
                this.m_gravity *= num2 + (float) ((double) Game.GRAVITY * (double) num1 * 2.0);
              }
            }
            Bomb bomb1 = this;
            bomb1.m_pos = bomb1.m_pos + this.m_vel * num1;
            if ((double) dt > 0.0)
            {
              for (int index = 0; index < 2; ++index)
                this.m_rotation_piece[index] += this.m_rotation_speed[index];
            }
          }
          this.m_col_box.centre = new Vector3(1000f, 1000f, 0.0f);
          ((ColSphere) this.m_col_box).Radius = 0.01f;
        }
        else
        {
          if ((double) this.m_chuckWait > 0.0)
          {
            if ((double) Game.game_work.hitBombTime > 0.0 || Game.game_work.gameOver)
            {
              this.m_chuckWait = 0.0f;
              this.m_pos.Y = -Game.SCREEN_HEIGHT;
              this.m_vel = new Vector3(0.0f, -1f, 0.0f);
            }
            float chuckWait = this.m_chuckWait;
            if (!Game.game_work.pause)
              this.m_chuckWait -= Game.game_work.dt;
            if ((double) this.m_chuckWait <= 0.20000000298023224 && (double) chuckWait > 0.20000000298023224 && !Bomb.s_sfxPlayedThisFrame && !Game.game_work.gameOver)
            {
              SoundManager.GetInstance().SFXPlay(SoundDef.SND_THROW_BOMB);
              Bomb.s_sfxPlayedThisFrame = true;
            }
            if ((double) this.m_chuckWait > 0.0)
              return;
            float num3 = WaveManager.GetInstance().BombMultiplyer();
            int num4 = (int) num3;
            if ((double) num3 > (double) num4 + 0.0099999997764825821 && ((double) num3 - (double) num4) * 100.0 > (double) Mortar.Math.g_random.Rand32(100))
              ++num4;
            if (num4 <= 0)
            {
              this.m_chuckWait = 0.0f;
              this.m_pos.Y = -Game.SCREEN_HEIGHT;
              this.m_vel = new Vector3(0.0f, -1f, 0.0f);
            }
            else if (num4 > 1)
              WaveManager.GetInstance().SpawnBomb(num4 - 1);
          }
          if (this.m_gravityEnabled)
          {
            Bomb bomb = this;
            bomb.m_vel = bomb.m_vel + this.m_gravity * dt;
            if (this.m_gravityEnabled && ((double) this.m_gravity.Y < 0.0 && (double) this.m_vel.Y < 0.0 || (double) this.m_gravity.Y > 0.0 && (double) this.m_vel.Y > 0.0 || (double) this.m_gravity.X < 0.0 && (double) this.m_vel.X < 0.0 || (double) this.m_gravity.X > 0.0 && (double) this.m_vel.X > 0.0))
            {
              float num5 = this.m_gravity.Length();
              this.m_gravity.Normalize();
              this.m_gravity *= num5 + (float) ((double) Game.GRAVITY * (double) num1 * 2.0);
            }
          }
          Bomb bomb2 = this;
          bomb2.m_pos = bomb2.m_pos + this.m_vel * num1;
          if ((double) dt > 0.0)
          {
            for (int index = 0; index < 2; ++index)
              this.m_rotation_piece[index] += this.m_rotation_speed[index];
          }
          this.m_col_box.centre = this.m_pos;
          this.m_col_box.centre.Z = 0.0f;
        }
        if ((double) this.m_pos.Y <= -(double) Game.SCREEN_HEIGHT * 0.75 || (double) this.m_pos.Y >= (double) Game.SCREEN_HEIGHT * 0.75 || (double) this.m_pos.X <= -(double) Game.SCREEN_WIDTH * 0.75 || (double) this.m_pos.X >= (double) Game.SCREEN_WIDTH * 0.75)
        {
          this.KillBomb();
        }
        else
        {
          if (this.m_emitter != null)
            return;
          this.m_emitter = PSPParticleManager.GetInstance().AddEmitter(Bomb.particleHash[this.m_forPlayer], (Action<PSPParticleEmitter>) null);
          if (this.m_emitter == null)
            return;
          this.m_emitter.pos = this.m_pos;
        }
      }

      public void KillBomb()
      {
        this.m_destroy = true;
        if (this.m_hudControl != null && object.ReferenceEquals((object) this.m_hudControl.m_entity, (object) this))
          this.m_hudControl.m_entity = (Entity) null;
        if (this.m_emitter == null)
          return;
        PSPParticleManager.GetInstance().ClearEmitter(this.m_emitter);
        this.m_emitter = (PSPParticleEmitter) null;
      }

      public override void Draw()
      {
        if (PopOverControl.IsInPopup && !this.partOfPopup)
          return;
        Bomb.s_sfxPlayedThisFrame = false;
        if ((double) this.m_chuckWait > 0.0)
          return;
        float num = -1000f;
        if (this != Bomb.highestBomb && !this.m_isMenuItem && (double) this.m_pos.Y > (double) num)
          Bomb.highestBomb = this;
        if (Bomb.m_bombModel[this.m_forPlayer] == null)
          return;
        Matrix matrix = Matrix.CreateScale(this.m_cur_scale) * Matrix.CreateRotationX(MathHelper.ToRadians(270f)) * Matrix.CreateRotationY(MathHelper.ToRadians((float) this.m_rotation_piece[0])) * Matrix.CreateRotationZ(MathHelper.ToRadians((float) this.m_rotation_piece[1])) * Matrix.CreateTranslation(this.m_pos + Vector3.UnitZ * this.m_z);
        Bomb.m_bombModel[this.m_forPlayer].Draw(new Matrix?(matrix));
      }

      public override void DrawUpdate(float dt)
      {
        if (this.m_emitter == null)
          return;
        this.m_emitter.pos = this.m_pos + new Vector3(Mortar.Math.SinIdx((ushort)-Mortar.Math.DEGREE_TO_IDX((float) this.m_rotation_piece[1])) * Bomb.FUSE_DIST(this.m_cur_scale.X), Mortar.Math.CosIdx((ushort)-Mortar.Math.DEGREE_TO_IDX((float) this.m_rotation_piece[1])) * Bomb.FUSE_DIST(this.m_cur_scale.X), 5f);
        this.m_emitter.cosz = Mortar.Math.CosIdx((ushort)-Mortar.Math.DEGREE_TO_IDX((float) this.m_rotation_piece[1]));
        this.m_emitter.sinz = -Mortar.Math.SinIdx((ushort)-Mortar.Math.DEGREE_TO_IDX((float) this.m_rotation_piece[1]));
      }

      public override bool CollisionResponse(Entity p_ent2, uint col_1, uint col_2, ref Vector3 proj)
      {
        if (PopOverControl.IsInPopup && !this.partOfPopup || this.m_disabled)
          return false;
        if (this.m_isMenuItem)
        {
          if (this.m_hudControl == null || this.m_hudControl != null && this.m_hudControl.m_clearOthers)
            Game.ClearMenuItems();
          this.m_callback();
        }
        else if (p_ent2 != null)
        {
          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
          {
            uint hash = StringFunctions.StringHash("bombs_hit");
            Game.game_work.saveData.AddToTotal("bombs_hit", hash, 1, false, false);
            WaveManager.GetInstance().ResetSpeed();
            this.m_isMenuItem = true;
            Game.HitMenuBomb(this.m_pos);
            Game.game_work.camera.CreateCameraShake(this.m_pos, 2f, 3f);
            Game.AddToCurrentScore(-10);
            PowerUpManager.GetInstance().ClearTimedPowers();
            MissControl free = MissControl.GetFree();
            free.MakeDisappear(this.m_pos, 0, Bomb.s_minus_10);
            free.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_BOMB;
          }
          else
          {
            if (Game.game_work.gameOver)
              return false;
            Game.HitBomb(this.m_pos);
          }
        }
        this.m_hit = true;
        return false;
      }

      public void EnableGravity(bool state) => this.m_gravityEnabled = state;

      public bool GravityEnabled() => this.m_gravityEnabled;

      public void Disable() => this.m_disabled = true;

      public bool Enabled() => !this.m_disabled;

      public void SetCallback(MenuButton.MenuCallback call, MenuButton button)
      {
        this.m_isMenuItem = true;
        this.m_callback = call;
        this.m_hudControl = button;
        this.m_rotation_speed[0] = (ushort) 2;
        this.m_rotation_speed[1] = (ushort) 0;
        this.m_rotation_piece[0] = (ushort) 0;
        this.m_rotation_piece[1] = (ushort) 45;
      }

      public bool IsActive() => (double) this.m_chuckWait <= 0.0;

      public void Chuck() => this.Chuck(0.0f);

      public void Chuck(float chuck)
      {
        if ((double) chuck <= 0.0)
          chuck = Bomb.BOMB_CHUCK_WAIT;
        this.m_chuckWait = chuck;
      }

      public float GetWait() => !this.m_hit ? this.m_chuckWait : this.m_newBlastWait;

      public void SetHit(float wait)
      {
        this.m_hit = true;
        this.m_newBlastWait = wait;
      }

      public int ForPlayer() => this.m_forPlayer;

      public void SetForPlayer(int forPlayer) => this.m_forPlayer = forPlayer;

      public static int GetNumActiveForPlayer(int player) => Bomb.GetNumActiveForPlayer(player, true);

      public static int GetNumActiveForPlayer(int player, bool waitForDeath)
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        Bomb bomb = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator);
        int numActiveForPlayer = 0;
        if (waitForDeath)
        {
          for (; bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
          {
            if (player == -1 && bomb.ForPlayer() < Game.MAX_PLAYERS || player == bomb.ForPlayer())
              ++numActiveForPlayer;
          }
        }
        else
        {
          for (; bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
          {
            if ((double) bomb.GetWait() > 0.0 && !bomb.GetHit())
              ++numActiveForPlayer;
          }
        }
        return numActiveForPlayer;
      }

      public static void ClearUnspawned()
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (Bomb bomb = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
        {
          if ((double) bomb.GetWait() > 0.0 && !bomb.GetHit())
            bomb.KillBomb();
        }
      }

      public static void DeactivateAll()
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (Bomb bomb = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
          bomb.Disable();
      }

      public void MakeFat() => this.MakeFat(false);

      public void MakeFat(bool fromLoad)
      {
        this.m_dtMod = 0.666f;
        Bomb bomb = this;
        bomb.m_cur_scale = bomb.m_cur_scale * 1.33f;
        this.m_orig_scale = this.m_cur_scale;
        ((ColSphere) this.m_col_box).Radius *= 1.33f;
        if (fromLoad)
          return;
        this.m_forPlayer += Game.MAX_PLAYERS - 1;
        this.m_gravity.Normalize();
        bool flag = this.m_forPlayer == Game.MAX_PLAYERS + 1;
        PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash(flag ? "red_bomb_warning" : "blue_bomb_warning"), (Action<PSPParticleEmitter>) null);
        if (pspParticleEmitter != null)
        {
          pspParticleEmitter.pos = this.m_pos;
          pspParticleEmitter.pos += this.m_vel * 7.5f;
          pspParticleEmitter.pos.X = (float) ((double) Game.SCREEN_WIDTH * (double) Mortar.Math.MATH_SIGN(this.m_pos.X) / 2.0);
          Vector2 vector2 = new Vector2(this.m_vel.X, this.m_vel.Y);
          vector2.Normalize();
          pspParticleEmitter.cosz = vector2.Y;
          pspParticleEmitter.sinz = vector2.X;
        }
        SoundManager.GetInstance().SFXPlay("player-bomb-launch");
        this.Chuck(0.25f);
      }
    }
}
