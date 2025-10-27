// Decompiled with JetBrains decompiler
// Type: FruitNinja.SuperFruitControl
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FruitNinja
{

    public class SuperFruitControl : HUDControl3d
    {
      public const float SUPER_SOUND_FADE_AMOUNT = 0.125f;
      public const float BACK_TEXT_SIZE = 50f;
      public const float POPIN_START_TIME = -2f;
      public const float POPIN_END_TIME = -1.85f;
      public const float CAM_ZOOM = 0.625f;
      public const float CAM_MOVE_MIN_DIST = 7f;
      public const float CAM_MOVE_MAX_DIST = 15f;
      public const float CAM_MOVE_MAX_RADIUS = 44f;
      public const int MIN_SLICES_BEFORE_NEXT_CAM_MOVE = 0;
      public const int MAX_SLICES_BEFORE_NEXT_CAM_MOVE = 0;
      public const float DIRECTION_CHANGES_MIN = 4f;
      public const float DIRECTION_CHANGES_MAX = 5f;
      public const float DIRECTION_CHANGE_MIN = 40f;
      public const float DIRECTION_CHANGE_MAX = 60f;
      public const float DT_MOD_DURING = 0.1f;
      public const float CAM_MOVE_SPEED = 0.175f;
      public const float SLICES_PER_RAY = 2f;
      public const float JIB_SPLAT_FREQUENCY = 20f;
      public const float SUPER_FRUIT_RAY_SPEED = 60f;
      public const float COLLISION_SCALE = 0.775f;
      public const float MAX_SLICES_PER_SEC = 17.5f;
      public Fruit m_fruit;
      public Fruit m_fruitHandle;
      public float m_timeSinceHit;
      public float m_time;
      public float m_prevTime;
      public int m_hits;
      public SuperFruitHitControl m_lastCreatedHitControl;
      public float m_sliceTime;
      public float m_jumpTime;
      public Vector3 m_shakeDir;
      public Vector3 m_cameraOffset;
      public Vector3 m_cameraOffsetPrev;
      public Vector3 m_cameraOffsetDest;
      public float m_cameraOffsetMoveTime;
      public int m_slicesTillCameraMove;
      public ushort m_cameraOffsetMoveAngle;
      public int m_cameraOffsetDirectionChangeCount;
      private string m_finalAddText;
      private string m_bakedString;
      private Color m_colour;
      private static Dictionary<Fruit, SuperFruitControl> SuperFruitControls = new Dictionary<Fruit, SuperFruitControl>();
      private static Texture ShockWaveTexture;
      private static Model JibletModel = (Model) null;
      private float m_splosionRadius;
      private float m_secondarySplosionRadius;
      private Vector3 m_splosionCenter;
      private Vector3 m_splosionCenterBounded;
      private static Color[] baseColours = new Color[3]
      {
        new Color((int) byte.MaxValue, 218, 46),
        new Color((int) byte.MaxValue, 119, 54),
        new Color(137, 46, (int) byte.MaxValue)
      };
      private static int rayNum = 0;
      private static int[] sectorRandomness = new int[8]
      {
        1,
        3,
        5,
        7,
        2,
        4,
        0,
        6
      };

      public float SLICE_TIME => this.m_sliceTime;

      public float ZOOM_OUT_SOUND_TIME => this.m_sliceTime - 0.1f;

      public float EXPLODE_TIME => this.SLICE_TIME + 0.5f;

      public float EXPLODE_FULL => this.EXPLODE_TIME + 0.35f;

      public float EXPLODE_FADED => this.EXPLODE_FULL + 0.25f;

      public float SECONDARY_EXPLODE_TIME => this.EXPLODE_FULL + 0.55f;

      public float SECONDARY_EXPLODE_FULL => this.SECONDARY_EXPLODE_TIME + 0.65f;

      public float SECONDARY_EXPLODE_FADED => this.SECONDARY_EXPLODE_FULL + 0.25f;

      public float DT_MOD_RAMP_UP_START => this.EXPLODE_FULL + 0.4f;

      public float DT_MOD_RAMP_UP_FINISH => this.EXPLODE_FULL + 0.6f;

      public float EXPLODE_END => this.SECONDARY_EXPLODE_FULL + 0.5f;

      public float FINAL_SCORE_IN_TIME => this.SECONDARY_EXPLODE_TIME + 0.1f;

      public float FINAL_SCORE_FULL_TIME => this.FINAL_SCORE_IN_TIME + 0.25f;

      public float KILL_FRUIT_TIME => this.SECONDARY_EXPLODE_FADED + 0.55f;

      public float POPOUT_START_TIME
      {
        get
        {
          return this.SECONDARY_EXPLODE_FADED + (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE ? 1.5f : 0.5f);
        }
      }

      public float POPOUT_END_TIME => this.POPOUT_START_TIME + 0.15f;

      public float SPLOSION_RADIUS_FULL
      {
        get
        {
          return Mortar.Math.Sqrt((float) ((double) Game.SCREEN_WIDTH * (double) Game.SCREEN_WIDTH + (double) Game.SCREEN_HEIGHT * (double) Game.SCREEN_WIDTH));
        }
      }

      public SuperFruitControl(Fruit fruit)
      {
        this.m_fruit = fruit;
        this.m_fruitHandle = fruit;
        this.m_time = -2f;
        this.m_prevTime = -2f;
        this.m_lastCreatedHitControl = (SuperFruitHitControl) null;
        this.m_finalAddText = "";
        SlashEntity.ComboCanceledEvent += new SlashEntity.ComboCanceled(this.ComboCancel);
        this.m_cameraOffset = this.m_cameraOffsetPrev = this.m_cameraOffsetDest = Vector3.Zero;
        this.m_cameraOffsetMoveTime = 1f;
        this.m_slicesTillCameraMove = (int) Utils.GetRandBetween(0.0f, 0.0f);
        this.m_hits = 1;
        float num = Mortar.Math.g_random.RandF(10f) + 15f;
        if (Mortar.Math.g_random.Rand32(2) > 0)
          num *= -1f;
        float zoom = 0.625f;
        Vector3 vector3 = new Vector3(Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(num)), Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX(num)), 0.0f);
        Game.game_work.camera.Transition(fruit.m_pos + vector3 * Game.SCREEN_HEIGHT * 0.15f * zoom, zoom, num, new FruitCamera.TransitionFinished(this.TransitionFin));
        fruit.m_waitingForCombo = true;
        this.m_fruit.m_sliceWait = 1f;
        ((ColSphere) fruit.m_col_box).Radius *= 0.775f;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        fruit.m_z = 10f;
        this.ChangeText("SLICE!", false);
        this.m_scale = Vector3.One * 0.5f;
        this.m_pos = new Vector3(fruit.m_pos.X, fruit.m_pos.Y, 0.0f);
        this.m_rotation = -num;
        this.m_jumpTime = 1f;
        this.m_shakeDir = Vector3.Zero;
        SuperFruitControl superFruitControl = this;
        superFruitControl.m_pos = superFruitControl.m_pos + vector3 * Game.SCREEN_HEIGHT * 0.4f * zoom;
        WaveManager.GetInstance().paused = true;
        Fruit.ClearUnspawned();
        Bomb.ClearUnspawned();
        Bomb.DeactivateAll();
        SoundManager.GetInstance().SFXPlay("pome-rampdown");
        this.m_sliceTime = Utils.GetRandBetween(2f, 3f);
        this.m_timeSinceHit = 0.0f;
        this.m_colour = Color.Violet;
      }

      ~SuperFruitControl() => this.Release();

      public static void LoadContent()
      {
        Fruit.FruitThrown += new Fruit.FruitEvent(SuperFruitControl.SuperFruitThrown);
        Fruit.FruitSliced += new Fruit.FruitSliceEvent(SuperFruitControl.SuperFruitSliced);
        SuperFruitGlow.GlowTexture = Texture.Load("textureswp7/rays.tex");
        SuperFruitControl.ShockWaveTexture = Texture.Load("textureswp7/explosion_radius.tex");
        FruitRay.RayTexture = Texture.Load("textureswp7/pomegranate_rays.tex");
        SuperFruitControl.JibletModel = MeshManager.GetInstance().Load("models/fruit/pomegranate_jiblet.mmd");
      }

      public static void UnLoadContent()
      {
        FruitRay.RayTexture = (Texture) null;
        SuperFruitControl.ShockWaveTexture = (Texture) null;
        SuperFruitControl.JibletModel = (Model) null;
      }

      public static void SuperFruitThrown(Fruit fruit)
      {
        if (!Fruit.FruitInfo(fruit.GetFruitType()).superFruit || fruit.m_destroy)
          return;
        fruit.m_z = -5f;
        if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
        {
          fruit.m_pos = new Vector3(-35f, (float) (-(double) Game.SCREEN_HEIGHT / 2.0 - 100.0), 0.0f);
          fruit.m_vel = new Vector3(0.5f, 8.5f, 0.0f);
          fruit.m_gravity.Y = -7.5f;
          fruit.m_gravity.X = 0.0f;
        }
        else
        {
          fruit.m_pos = new Vector3((float) (-(double) Game.SCREEN_WIDTH / 2.0 - 100.0), -100f, 0.0f);
          fruit.m_vel = new Vector3(5f, 5f, 0.0f);
          fruit.m_gravity.Y = -4.5f;
          fruit.m_gravity.X = 0.01f;
        }
        if (WaveManager.GetInstance().w_random.Rand32(100) <= 50)
        {
          fruit.m_gravity.X *= -1f;
          fruit.m_pos.X *= -1f;
          fruit.m_vel.X *= -1f;
        }
        Game.game_work.saveData.AddToTotal("super_pomegranates_spawned", StringFunctions.StringHash("super_pomegranates_spawned"), 1, false, false);
        SuperFruitGlow control = new SuperFruitGlow(fruit);
        Game.game_work.hud.AddControl((HUDControl) control);
        TheGame.pomegranateThrown = true;
      }

      public static void SuperFruitSliced(Fruit fruit, int points, Entity otherEnt)
      {
        if (!Fruit.FruitInfo(fruit.GetFruitType()).superFruit)
          return;
        SuperFruitControl superFruitControl = (SuperFruitControl) null;
        try
        {
          if (SuperFruitControl.SuperFruitControls.Count > 0)
            superFruitControl = SuperFruitControl.SuperFruitControls[fruit];
        }
        catch
        {
        }
        if (superFruitControl != null)
          superFruitControl.Sliced(otherEnt);
        else if (Game.FailureEnabled() && Game.game_work.gameOver)
        {
          fruit.m_sliceWait = 0.0f;
        }
        else
        {
          SuperFruitControl control = new SuperFruitControl(fruit);
          Game.game_work.hud.AddControl((HUDControl) control, false);
          SuperFruitControl.SuperFruitControls[fruit] = control;
        }
      }

      public static bool IsInSuperFruitState() => SuperFruitControl.SuperFruitControls.Count > 0;

      public static void Clear() => SuperFruitControl.SuperFruitControls.Clear();

      private void Sliced(Entity otherEnt)
      {
        if ((double) this.m_time < 0.0)
          this.m_time = 0.0f;
        else if ((double) this.m_time >= (double) this.SLICE_TIME || (double) this.m_timeSinceHit > 1.5)
          return;
        ++this.m_timeSinceHit;
        ++this.m_hits;
        if (this.m_slicesTillCameraMove <= 0)
        {
          float num1 = TransitionFunctions.LerpF(7f, 15f, Mortar.Math.g_random.RandF(1f));
          float num2 = this.m_cameraOffsetDest.Length();
          ushort idx;
          if ((double) num2 < 1.0)
            idx = (ushort) Mortar.Math.g_random.Rand32();
          else if ((double) num2 > 44.0)
          {
            idx = (ushort) ((uint) Mortar.Math.Atan2Idx(this.m_cameraOffsetDest.Y, this.m_cameraOffsetDest.X) + (uint) Mortar.Math.DEGREE_TO_IDX(135f) + (uint) (ushort) Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(90f)));
          }
          else
          {
            Vector3 vector3 = this.m_cameraOffsetDest - this.m_cameraOffsetPrev;
            idx = Mortar.Math.Atan2Idx(vector3.Y, vector3.X);
          }
          this.m_cameraOffsetDest += new Vector3(Mortar.Math.CosIdx(idx), Mortar.Math.SinIdx(idx), 0.0f) * num1;
          this.m_cameraOffsetPrev = this.m_cameraOffset;
          this.m_cameraOffsetMoveTime = 0.0f;
          this.m_slicesTillCameraMove = 0;
        }
        else
          --this.m_slicesTillCameraMove;
        WaveManager.GetInstance().AddToSpeedLossTime(0.1f);
        this.m_fruit.RemoveTrailParticles();
        this.m_shakeDir = this.m_fruit.GetSliceDir(Mortar.Math.DEGREE_TO_IDX(-this.m_rotation)) * this.m_fruit.m_slicedMag * 3f;
        Vector3 vector3_1 = otherEnt.m_col_box.centre - ((ColLine) otherEnt.m_col_box).Direction;
        Vector3 pos = this.m_fruit.m_pos;
        pos.Z = this.m_fruit.m_z - 5f;
        float randBetween = Utils.GetRandBetween(0.8f, 1.1f);
        GameTask.AddSlice(pos, (float) Mortar.Math.Atan2Idx(vector3_1.Y, vector3_1.X) / 182f, randBetween, 0, (Fruit) null, 0.65f);
        GameTask.AddSlice(pos, (float) Mortar.Math.Atan2Idx(vector3_1.Y, vector3_1.X) / 182f, randBetween, 3, this.m_fruit, 0.65f);
        if (otherEnt != null && otherEnt.m_type == (byte) 3)
          ((SlashEntity) otherEnt).m_splatsToMake = -1;
        if (this.m_lastCreatedHitControl != null)
        {
          this.m_lastCreatedHitControl.m_parent = (SuperFruitControl) null;
          this.m_lastCreatedHitControl.RemoveQuickly();
        }
        uint hash = Fruit.fruitInfo[this.m_fruit.GetFruitType()].hash[0];
        if (PSPParticleManager.GetInstance().EmitterExists(hash))
        {
          for (int index = 0; index < 2; ++index)
          {
            PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(hash, (Action<PSPParticleEmitter>) null);
            if (pspParticleEmitter != null)
            {
              pspParticleEmitter.sinz = -Mortar.Math.SinIdx((ushort)-this.m_fruit.m_slicedAngle);
              pspParticleEmitter.cosz = Mortar.Math.CosIdx((ushort)-this.m_fruit.m_slicedAngle);
              pspParticleEmitter.pos = otherEnt.m_col_box.centre;
              pspParticleEmitter.dtMod /= WaveManager.GetInstance().GetAbsoluteDtMod();
              pspParticleEmitter.sizeScale *= 0.5f;
            }
          }
        }
        this.ChangeText($"{this.m_hits} HITS", true);
        string filename = $"pome-slice-{(int) Utils.GetRandBetween(1f, 3f)}";
        SoundManager.GetInstance().SFXPlay(filename, 0U, (MortarSound) null, (byte) 0, -1);
        if ((double) this.m_hits % 2.0 != 1.0)
          return;
        this.SpawnRay();
      }

      private void ChangeText(string text, bool pop)
      {
        this.m_colour = Utils.LerpColourFromArray(Mortar.Math.CLAMP((float) this.m_hits / 70f, 0.0f, 1f), SuperFruitControl.baseColours, 3);
        if (pop)
          this.m_jumpTime = 0.0f;
        this.m_bakedString = text;
        GameTask.Flash();
      }

      private void ComboCancel(SlashEntity slash)
      {
        if (this.m_fruit == null || (double) this.m_time >= (double) this.SLICE_TIME)
          return;
        this.m_fruit.m_sliceWait = -1f;
      }

      private void SpawnRay()
      {
        ++SuperFruitControl.rayNum;
        int index = SuperFruitControl.rayNum % 8;
        int num = SuperFruitControl.sectorRandomness[index];
        float start1 = num < 4 ? -35f : 5f;
        float end1 = num < 4 ? -5f : 70f;
        float start2 = (float) (num % 4 * 90);
        float end2 = (float) (num % 4 * 90 + 80 /*0x50*/);
        ushort idx1 = Mortar.Math.DEGREE_TO_IDX(Utils.GetRandBetween(0.0f, 180f));
        ushort idx2 = Mortar.Math.DEGREE_TO_IDX(Utils.GetRandBetween(start1, end1));
        ushort idx3 = Mortar.Math.DEGREE_TO_IDX(Utils.GetRandBetween(start2, end2));
        Quaternion offset = Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), (float) idx1) * Quaternion.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), (float) idx2) * Quaternion.CreateFromAxisAngle(new Vector3(1f, 0.0f, 0.0f), (float) idx3);
        ((FruitRay) ActorManager.GetInstance().Add(EntityTypes.ENTITY_FRUIT_RAY)).Init(this.m_fruit, offset);
      }

      public override void Reset()
      {
        WaveManager.GetInstance().paused = false;
        WaveManager.GetInstance().SetAbsoluteDtMod(1f);
        PSPParticleManager.GetInstance().RepulsiveRadius = 0.0f;
        PSPParticleManager.GetInstance().RepulsiveStrength = 1f;
        Game.game_work.hud.m_hudAmount = 1f;
        Game.game_work.camera.TransitionOut();
        Game.game_work.camera.m_transitionFinished = (FruitCamera.TransitionFinished) null;
        GameTask.UnpauseSlices();
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (FruitRay fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_FRUIT_RAY, ref iterator); fruitRay != null; fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_FRUIT_RAY, ref iterator))
          fruitRay.m_destroy = true;
        this.m_terminate = true;
      }

      public static bool StopAllPomegranates()
      {
        foreach (KeyValuePair<Fruit, SuperFruitControl> superFruitControl in SuperFruitControl.SuperFruitControls)
          superFruitControl.Value.Reset();
        return true;
      }

      private void StopRays()
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (FruitRay fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_FRUIT_RAY, ref iterator); fruitRay != null; fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_FRUIT_RAY, ref iterator))
          fruitRay.m_fadeOut = true;
        if (this.m_fruit == null)
          return;
        for (int index = 0; index < 2; ++index)
          this.m_fruit.m_rotation_speed[index] = Vector3.Zero;
      }

      private void SpawnJibs()
      {
        string s = $"{Fruit.FruitInfo(this.m_fruit.GetFruitType()).fruitName}_explode";
        if (PSPParticleManager.GetInstance().EmitterExists(StringFunctions.StringHash(s)))
        {
          PSPParticleEmitter pspParticleEmitter = PSPParticleManager.GetInstance().AddEmitter(StringFunctions.StringHash(s), (Action<PSPParticleEmitter>) null);
          if (pspParticleEmitter != null)
          {
            pspParticleEmitter.canNotBeRepulsed = true;
            pspParticleEmitter.pos = this.m_splosionCenter;
            pspParticleEmitter.cosz = Mortar.Math.CosIdx(this.m_fruit.m_slicedAngle);
            pspParticleEmitter.sinz = Mortar.Math.SinIdx(this.m_fruit.m_slicedAngle);
          }
        }
        int num = 12;
        float randBetween = Utils.GetRandBetween(0.0f, 360f / (float) num);
        uint particles = StringFunctions.StringHash($"{Fruit.FruitInfo(this.m_fruit.GetFruitType()).fruitName}_jiblet");
        for (int index = 0; index < num; ++index)
        {
          Jiblet jiblet = (Jiblet) ActorManager.GetInstance().Add(EntityTypes.ENTITY_JIBLET);
          float start = (float) (((double) index + 0.20000000298023224) * 360.0) / (float) num;
          float end = (float) (((double) index + 0.800000011920929) * 360.0) / (float) num;
          Vector3 vector3 = Utils.VectorFromMagDegree(1f, randBetween + Utils.GetRandBetween(start, end));
          jiblet.Init(this.m_fruit.GetFruitType(), this.m_splosionCenter, Utils.GetRandBetween(0.8f, 1.25f), vector3 * Utils.GetRandBetween(500f, 900f), SuperFruitControl.JibletModel, particles, 20f, vector3 * 45f);
        }
      }

      private void ExplodeSuperFruit()
      {
        Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.m_fruit.m_rotation_piece[0]);
        float slicedMag = this.m_fruit.m_slicedMag;
        int num1 = 14;
        for (int index = 0; index < num1; ++index)
        {
          ushort idx = (ushort) Mortar.Math.g_random.Rand32((int) Mortar.Math.DEGREE_TO_IDX(360f));
          float num2 = (float) (((double) Mortar.Math.g_random.RandF(0.5f) + 1.0) * (double) slicedMag * (5.0 + (double) index * 0.20000000298023224));
          SplatEntity free = SplatEntity.GetFree();
          int fruitType = this.m_fruit.GetFruitType();
          free.MakeSplat(this.m_fruit.m_pos, new Vector3(Mortar.Math.SinIdx(idx) * num2, Mortar.Math.CosIdx(idx) * num2, 0.0f), false, true, fruitType);
          free.m_vel.Z *= Mortar.Math.CLAMP((float) (1.0 - (double) (index - 2) / (double) num1), 0.3f, 1f);
        }
        GameTask.CriticalFlash(this.m_fruit.m_pos, Color.White);
        SoundManager.GetInstance().SFXPlay("pome-burst");
        for (int index = 0; index < 8; ++index)
        {
          new Vector3()
          {
            X = (index / 2 % 2 == 0 ? 1f : -1f),
            Y = (index % 2 == 0 ? -1f : 1f),
            Z = (index / 4 == 0 ? 1f : -1f)
          }.Normalize();
          Vector3 vector3_1 = Vector3.UnitX * fromQuaternion.Translation;
          Jiblet jiblet = (Jiblet) ActorManager.GetInstance().Add(EntityTypes.ENTITY_JIBLET);
          Vector3 vector3_2 = vector3_1;
          vector3_2.Z = 0.0f;
          vector3_2.Normalize();
          string modelFileName = $"models/Fruit/{Fruit.FruitInfo(this.m_fruit.GetFruitType()).modelName}_{Fruit.FruitInfo(this.m_fruit.GetFruitType()).modelName[0]}_piece_{index + 1}.mmd";
          jiblet.Init(this.m_fruit.GetFruitType(), this.m_splosionCenter, 1f, vector3_2 * Utils.GetRandBetween(600f, 1000f), MeshManager.GetInstance().Load(modelFileName), 0U, 0.0f, vector3_2 * 700f);
          jiblet.m_cur_scale = this.m_fruit.m_cur_scale;
          jiblet.m_time = 0.0f;
          jiblet.m_orientation = fromQuaternion;
        }
        this.m_fruit.m_cur_scale = Vector3.Zero;
        TheGame.pomegranateThrown = false;
      }

      private void TransitionFin()
      {
        if (!Game.game_work.camera.IsTransitionIn() || this.m_fruit == null)
          return;
        this.m_fruit.m_sliceWait = -1f;
      }

      public override void Release()
      {
        if (SlashEntity.ComboCanceledEvent != null)
          SlashEntity.ComboCanceledEvent -= new SlashEntity.ComboCanceled(this.ComboCancel);
        SuperFruitControl.SuperFruitControls.Clear();
        this.StopRays();
        if (this.m_fruit == null)
          return;
        this.m_fruit.KillFruit();
        this.m_fruit = (Fruit) null;
      }

      public override void Save()
      {
      }

      public static void SaveSuperFruitState(Fruit fruit, XElement element)
      {
        if (fruit == null)
          return;
        foreach (KeyValuePair<Fruit, SuperFruitControl> superFruitControl in SuperFruitControl.SuperFruitControls)
          element.Add((object) new SuperFruitState()
          {
            time = superFruitControl.Value.m_time,
            sliceTime = superFruitControl.Value.m_sliceTime,
            hits = superFruitControl.Value.m_hits,
            rotation = superFruitControl.Value.m_rotation
          }.WriteToElement());
      }

      public SuperFruitControl(Fruit fruit, SuperFruitState state)
      {
        this.m_prevTime = -2f;
        this.m_lastCreatedHitControl = (SuperFruitHitControl) null;
        this.m_fruit = this.m_fruitHandle = fruit;
        SuperFruitControl.SuperFruitControls[this.m_fruit] = this;
        this.m_prevTime = this.m_time = state.time;
        this.m_sliceTime = state.sliceTime;
        this.m_hits = state.hits;
        this.m_rotation = state.rotation;
        SlashEntity.ComboCanceledEvent += new SlashEntity.ComboCanceled(this.ComboCancel);
        this.m_cameraOffset = this.m_cameraOffsetPrev = this.m_cameraOffsetDest = Vector3.Zero;
        this.m_cameraOffsetMoveTime = 1f;
        WaveManager.GetInstance().paused = true;
        this.m_scale = Vector3.One * 0.5f;
        float angle = -this.m_rotation;
        float num = 0.625f;
        Vector3 vector3 = new Vector3(Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(angle)), Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX(angle)), 0.0f);
        this.m_pos = new Vector3(this.m_fruit.m_pos.X, this.m_fruit.m_pos.Y, 0.0f);
        SuperFruitControl superFruitControl = this;
        superFruitControl.m_pos = superFruitControl.m_pos + vector3 * Game.SCREEN_HEIGHT * 0.25f * num;
        this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
        ((ColSphere) fruit.m_col_box).Radius *= 0.775f;
        if ((double) this.m_time >= (double) this.EXPLODE_TIME)
        {
          this.m_fruit.m_cur_scale = Vector3.Zero;
          this.m_finalAddText = $"+{this.m_hits}";
          this.m_splosionCenter = this.m_fruit.m_pos;
        }
        if ((double) this.m_time < 0.0)
          this.ChangeText("SLICE!", false);
        else
          this.ChangeText(string.Format("%i HITS", (object) this.m_hits), true);
        this.m_jumpTime = 1f;
        this.m_shakeDir = Vector3.Zero;
      }

      public static bool CanSpawnFinalPomegranate()
      {
        return Fruit.NumberOfPowerupFruits() <= 0 && (double) PowerUpManager.GetInstance().GetActiveProgression() >= 2.0;
      }

      public static bool SpawnFinalPomegranate()
      {
        WaveManager.GetInstance().SpawnFruit(1).Chuck(0.01f);
        WaveManager.GetInstance().SpawnFruit(1).Chuck(0.01f);
        Game.game_work.saveData.AddToTotal("super_pomegranates_spawned", StringFunctions.StringHash("super_pomegranates_spawned"), 1, false, false);
        WaveManager.GetInstance().SpawnFruit(1, Fruit.FruitType("super_pomegranate")).Chuck(0.1f);
        return true;
      }

      public static int NumPomegranatesSpawnedThisGame()
      {
        return Game.game_work.saveData == null ? 0 : Game.game_work.saveData.GetTotal(StringFunctions.StringHash("super_pomegranates_spawned"));
      }

      private void StopAllFruit()
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        Fruit fruit = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator);
        Fruit.ClearUnspawned();
        Bomb.ClearUnspawned();
        for (; fruit != null; fruit = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
        {
          fruit.m_dtMod = 0.0f;
          fruit.m_gravity = Vector3.Zero;
          fruit.EnableGravity(false);
          if (fruit.Sliced())
          {
            Vector3 vector3_1 = fruit.m_pos - this.m_splosionCenter;
            vector3_1.Normalize();
            Vector3 vector3_2 = vector3_1 * 5f;
            fruit.m_vel = (fruit.m_vel + vector3_2) / 2f;
            Vector3 vector3_3 = fruit.m_pos2 - this.m_splosionCenter;
            vector3_3.Normalize();
            Vector3 vector3_4 = vector3_3 * 5f;
            fruit.m_vel2 = (fruit.m_vel2 + vector3_4) / 2f;
          }
        }
        for (Bomb bomb = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); bomb != null; bomb = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
        {
          Vector3 vector3_5 = bomb.m_pos - this.m_splosionCenter;
          vector3_5.Normalize();
          Vector3 vector3_6 = vector3_5 * 5f;
          bomb.m_vel = (bomb.m_vel + vector3_6) / 2f;
          bomb.m_gravity = Vector3.Zero;
          bomb.EnableGravity(false);
          bomb.m_dtMod = 0.0f;
        }
      }

      private void PushBombsAway(float dt)
      {
        float num1 = dt * WaveManager.GetInstance().GetWavedt();
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        Bomb bomb1 = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator);
        float num2 = 200f;
        float num3 = 100f;
        for (; bomb1 != null; bomb1 = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
        {
          Vector3 vector3 = bomb1.m_pos - this.m_fruit.m_pos;
          float num4 = vector3.Length();
          vector3.Normalize();
          if ((double) num4 < (double) num2)
          {
            Bomb bomb2 = bomb1;
            bomb2.m_vel = bomb2.m_vel + vector3 * num1 * num3;
          }
        }
      }

      private void UpdateExplosion(float dt)
      {
        float num1 = this.SPLOSION_RADIUS_FULL * 1.2f;
        this.m_drawOrder |= HUD.HUD_ORDER.HUD_ORDER_AFTER_BOMB;
        this.m_splosionRadius = TransitionFunctions.GetProgressBetween(this.m_time, this.EXPLODE_TIME, this.EXPLODE_FULL, true) * num1;
        float mod = TransitionFunctions.LerpF(0.1f, 1f, TransitionFunctions.GetProgressBetween(this.m_time, this.DT_MOD_RAMP_UP_START, this.DT_MOD_RAMP_UP_FINISH, true));
        WaveManager.GetInstance().SetAbsoluteDtMod(mod);
        this.m_secondarySplosionRadius = TransitionFunctions.GetProgressBetween(this.m_time, this.SECONDARY_EXPLODE_TIME, this.SECONDARY_EXPLODE_FULL, true) * num1;
        PSPParticleManager.GetInstance().RepulsivePosition = this.m_splosionCenter;
        PSPParticleManager.GetInstance().RepulsiveRadius = this.m_splosionRadius * 1.6f;
        PSPParticleManager.GetInstance().RepulsiveStrength = TransitionFunctions.GetProgressBetween(this.m_time, this.POPOUT_START_TIME, this.SECONDARY_EXPLODE_FULL, true);
        float num2 = dt * WaveManager.GetInstance().GetWavedt();
        if ((double) this.m_time > (double) this.EXPLODE_END)
          return;
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (Fruit fruit1 = (Fruit) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BEGIN, ref iterator); fruit1 != null; fruit1 = (Fruit) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BEGIN, ref iterator))
        {
          if (fruit1 != this.m_fruit)
          {
            if ((double) this.m_splosionRadius > 0.0)
              fruit1.m_dtMod = 1f;
            Vector3 vector3_1 = fruit1.m_pos - this.m_splosionCenter;
            float num3 = vector3_1.Length();
            vector3_1.Normalize();
            if (fruit1.Sliced())
            {
              Vector3 vector3_2 = fruit1.m_pos2 - this.m_splosionCenter;
              float num4 = vector3_2.Length();
              vector3_2.Normalize();
              if ((double) num4 < (double) this.m_secondarySplosionRadius && (double) this.m_time > (double) this.EXPLODE_FADED)
                fruit1.m_vel2 += vector3_2 * Mortar.Math.MAX(0.0f, this.m_secondarySplosionRadius - num4) * num2 * 4f;
              if ((double) num3 < (double) this.m_secondarySplosionRadius)
              {
                if ((double) fruit1.m_sliceWait > 9.9999997473787516E-05)
                  fruit1.m_sliceWait = 0.0001f;
                if ((double) this.m_time > (double) this.EXPLODE_FADED)
                {
                  Fruit fruit2 = fruit1;
                  fruit2.m_vel = fruit2.m_vel + vector3_1 * Mortar.Math.MAX(0.0f, this.m_secondarySplosionRadius - num3) * num2 * 4f;
                }
              }
              else if ((double) fruit1.m_sliceWait > 0.0 && (double) this.m_time < (double) this.SECONDARY_EXPLODE_FULL)
                fruit1.m_sliceWait = 0.5f;
            }
            else if (fruit1.IsActive() && (double) num3 < (double) this.m_splosionRadius && (double) this.m_splosionRadius < (double) num1 && (double) this.m_time < (double) this.EXPLODE_FADED)
            {
              Vector3 proj = vector3_1 * 2f;
              fruit1.CollisionResponse((Entity) null, 0U, 0U, ref proj);
              Game.AddToCurrentScore(1, 0);
              fruit1.m_sliceWait = 1E-05f;
            }
          }
          else
          {
            this.m_fruit.m_sliceWait = 0.5f;
            this.m_fruit.m_vel = Vector3.Zero;
            this.m_fruit.m_vel2 = Vector3.Zero;
          }
        }
        for (Bomb bomb1 = (Bomb) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_BOMB, ref iterator); bomb1 != null; bomb1 = (Bomb) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_BOMB, ref iterator))
        {
          Vector3 vector3 = bomb1.m_pos - this.m_splosionCenter;
          float num5 = vector3.Length();
          vector3.Normalize();
          if ((double) this.m_splosionRadius > 0.0)
            bomb1.m_dtMod = 1f;
          if ((double) num5 < (double) this.m_secondarySplosionRadius)
          {
            Bomb bomb2 = bomb1;
            bomb2.m_vel = bomb2.m_vel + vector3 * (this.m_secondarySplosionRadius - num5) * num2 * 5f;
          }
        }
        for (Jiblet jiblet1 = (Jiblet) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_JIBLET, ref iterator); jiblet1 != null; jiblet1 = (Jiblet) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_JIBLET, ref iterator))
        {
          Vector3 vector3 = jiblet1.m_pos - this.m_splosionCenter;
          float num6 = vector3.Length();
          vector3.Normalize();
          if ((double) num6 < (double) this.m_secondarySplosionRadius)
          {
            Jiblet jiblet2 = jiblet1;
            jiblet2.m_vel = jiblet2.m_vel + vector3 * (this.m_secondarySplosionRadius - num6) * num2 * 5f;
          }
        }
      }

      private void DrawExplosion()
      {
        float progressBetween1 = TransitionFunctions.GetProgressBetween(this.m_time, this.EXPLODE_FADED, this.EXPLODE_FULL, true);
        if ((double) progressBetween1 > 0.0 && (double) this.m_splosionRadius > 0.0 && SuperFruitControl.ShockWaveTexture != null)
        {
          Matrix mtx = Matrix.CreateScale(Vector3.One * this.m_splosionRadius * 2f / (27f / 32f)) * Matrix.CreateTranslation(this.m_splosionCenter);
          MatrixManager.GetInstance().SetMatrix(mtx);
          MatrixManager.GetInstance().UploadCurrentMatrices();
          SuperFruitControl.ShockWaveTexture.Set();
          Mesh.DrawQuad(new Color((int) byte.MaxValue, 150, 175, Mortar.Math.CLAMP((int) ((double) progressBetween1 * 128.0), 0, (int) byte.MaxValue)));
        }
        float progressBetween2 = TransitionFunctions.GetProgressBetween(this.m_time, this.SECONDARY_EXPLODE_FADED, this.SECONDARY_EXPLODE_FULL, true);
        if ((double) progressBetween2 <= 0.0 || (double) this.m_secondarySplosionRadius <= 0.0 || SuperFruitControl.ShockWaveTexture == null)
          return;
        Matrix mtx1 = Matrix.CreateScale(Vector3.One * this.m_secondarySplosionRadius * 2f / (27f / 32f)) * Matrix.CreateTranslation(this.m_splosionCenter);
        MatrixManager.GetInstance().SetMatrix(mtx1);
        MatrixManager.GetInstance().UploadCurrentMatrices();
        SuperFruitControl.ShockWaveTexture.Set();
        Mesh.DrawQuad(new Color((int) byte.MaxValue, 150, 175, Mortar.Math.CLAMP((int) ((double) progressBetween2 * 128.0), 0, (int) byte.MaxValue)));
      }

      public override void Update(float dt)
      {
        if (this.m_terminate)
          return;
        if (!Game.game_work.pause)
        {
          this.m_time += dt;
          if ((double) this.m_timeSinceHit > 0.0)
          {
            this.m_timeSinceHit -= dt * 17.5f;
            if ((double) this.m_timeSinceHit <= 0.0)
              this.m_timeSinceHit = 0.0f;
          }
        }
        if (this.m_fruit != null && (double) this.m_fruit.m_sliceWait > 0.0)
          this.m_fruit.m_sliceWait = 1f;
        if ((double) this.m_prevTime < 0.0 && (double) this.m_time >= 0.0 && this.m_hits == 1)
          this.ChangeText("1 HIT", false);
        if ((double) this.m_time < (double) this.EXPLODE_TIME)
        {
          WaveManager.GetInstance().SetAbsoluteDtMod(0.1f);
          MissControl.MakeEmAllDissappear();
        }
        if ((double) this.m_time < (double) this.POPOUT_START_TIME)
          Bomb.DeactivateAll();
        if ((double) this.m_time >= (double) this.ZOOM_OUT_SOUND_TIME && (double) this.m_prevTime < (double) this.ZOOM_OUT_SOUND_TIME)
          SoundManager.GetInstance().SFXPlay("pome-rampdown");
        if ((double) this.m_time >= (double) this.SLICE_TIME)
        {
          if ((double) this.m_prevTime < (double) this.SLICE_TIME)
          {
            this.m_splosionCenter = this.m_fruit.m_pos;
            Game.game_work.camera.TransitionOut();
            this.StopAllFruit();
            GameTask.UnpauseSlices();
            if (this.m_lastCreatedHitControl != null)
              this.m_lastCreatedHitControl.m_parent = (SuperFruitControl) null;
            this.m_splosionCenterBounded = new Vector3(Mortar.Math.CLAMP(this.m_fruit.m_pos.X, (float) (-(double) Game.SCREEN_WIDTH * 0.42500001192092896), Game.SCREEN_WIDTH * 0.425f), Mortar.Math.CLAMP(this.m_fruit.m_pos.Y, (float) (-(double) Game.SCREEN_HEIGHT * 0.40000000596046448), Game.SCREEN_HEIGHT * 0.4f), 0.0f);
          }
          if ((double) this.m_prevTime < (double) this.EXPLODE_TIME)
          {
            this.m_splosionCenter = this.m_fruit.m_pos;
            if ((double) this.m_time >= (double) this.EXPLODE_TIME)
            {
              Game.game_work.camera.CreateCameraShake(this.m_pos, 1f, 2f);
              this.ExplodeSuperFruit();
              this.SpawnJibs();
              this.StopRays();
              this.m_finalAddText = $"+{this.m_hits}";
            }
          }
          if ((double) this.m_time >= (double) this.EXPLODE_TIME)
          {
            this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_NORMAL;
            this.UpdateExplosion(dt);
            if ((double) this.m_prevTime < (double) this.DT_MOD_RAMP_UP_START && (double) this.m_time >= (double) this.DT_MOD_RAMP_UP_START)
              Game.game_work.camera.CreateCameraShake(this.m_pos, 1.6f, 2f);
            Game.game_work.hud.m_hudAmount = Game.TAILY_TRANSITION_TO(Game.game_work.hud.m_hudAmount, 1f, dt, 0.75f);
          }
          if ((double) this.m_prevTime < (double) this.FINAL_SCORE_IN_TIME && (double) this.m_time >= (double) this.FINAL_SCORE_IN_TIME)
          {
            if (Game.game_work.gameMode == Game.GAME_MODE.GM_CLASSIC)
              Game.game_work.saveData.AddToTotal("super_fruit_gp_classic", StringFunctions.StringHash("super_fruit_gp_classic"), this.m_hits, false, false);
            Game.AddToCurrentScore(this.m_hits, 0);
          }
          if ((double) this.m_time > (double) this.KILL_FRUIT_TIME)
          {
            this.m_drawOrder = HUD.HUD_ORDER.HUD_ORDER_AFTER_SPLAT;
            if (this.m_fruit != null && (double) this.m_prevTime <= (double) this.KILL_FRUIT_TIME)
            {
              this.m_fruit.KillFruit();
              this.m_fruit = (Fruit) null;
            }
          }
          if ((double) this.m_time > (double) this.POPOUT_END_TIME)
          {
            WaveManager.GetInstance().SetAbsoluteDtMod();
            WaveManager.GetInstance().paused = false;
            WaveManager.GetInstance().GetNextWave();
            PSPParticleManager.GetInstance().RepulsiveRadius = 0.0f;
            PSPParticleManager.GetInstance().RepulsiveStrength = 1f;
            Game.game_work.hud.m_hudAmount = 1f;
            this.m_terminate = true;
            this.Release();
          }
        }
        else
        {
          if ((double) this.m_fruit.m_gravity.X != 0.0)
          {
            float end = Game.SCREEN_WIDTH * 0.3f;
            float start = Game.SCREEN_WIDTH * 0.45f;
            if ((double) this.m_fruit.m_vel.X < 0.0)
            {
              end = -end;
              start = -start;
            }
            this.m_fruit.m_dtMod = TransitionFunctions.GetProgressBetween(this.m_fruit.m_pos.X, start, end, true);
          }
          else if ((double) this.m_fruit.m_vel.Y < 0.0)
          {
            float end = (float) (-(double) Game.SCREEN_HEIGHT * 0.30000001192092896);
            this.m_fruit.m_dtMod = TransitionFunctions.GetProgressBetween(this.m_fruit.m_pos.Y, (float) (-(double) Game.SCREEN_HEIGHT * 0.40000000596046448), end, true);
          }
          this.PushBombsAway(dt);
          Game.game_work.hud.m_hudAmount = Game.TAILY_TRANSITION_TO(Game.game_work.hud.m_hudAmount, -1f, dt, 0.75f);
          float num = -this.m_rotation;
          float zoom = 0.625f;
          Vector3 vector3 = new Vector3(Mortar.Math.SinIdx(Mortar.Math.DEGREE_TO_IDX(num)), Mortar.Math.CosIdx(Mortar.Math.DEGREE_TO_IDX(num)), 0.0f);
          Vector3 pos1 = this.m_fruit.m_pos;
          if ((double) this.m_jumpTime < 1.0)
            pos1 += this.m_shakeDir * TransitionFunctions.JumpySinPulse(Mortar.Math.CLAMP(this.m_jumpTime * 2f, 0.0f, 1f), 2f);
          this.m_cameraOffset = TransitionFunctions.LerpF(this.m_cameraOffsetPrev, this.m_cameraOffsetDest, TransitionFunctions.SinTransition(Mortar.Math.MIN(this.m_cameraOffsetMoveTime, 1f), 105f));
          Vector3 pos2 = pos1 + this.m_cameraOffset;
          Game.game_work.camera.Transition(pos2, zoom, num, new FruitCamera.TransitionFinished(this.TransitionFin));
          this.m_pos = new Vector3(this.m_fruit.m_pos.X, this.m_fruit.m_pos.Y, 0.0f);
          SuperFruitControl superFruitControl = this;
          superFruitControl.m_pos = superFruitControl.m_pos + vector3 * Game.SCREEN_HEIGHT * 0.25f * zoom;
        }
        if ((double) this.m_jumpTime < 1.0)
        {
          this.m_jumpTime += 3f * dt;
          if ((double) this.m_jumpTime >= 1.0)
            this.m_jumpTime = 1f;
        }
        if ((double) this.m_cameraOffsetMoveTime < 1.0)
        {
          this.m_cameraOffsetMoveTime += dt / 0.175f;
          if ((double) this.m_cameraOffsetMoveTime > 1.0)
            this.m_cameraOffsetMoveTime = 1f;
        }
        this.m_prevTime = this.m_time;
      }

      public override void Draw(float[] tintChannels) => this.DrawOrder(tintChannels, (int) HUD.ORDER);

      public override void DrawOrder(float[] tintChannels, int order)
      {
        float num1 = TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.m_time, this.POPOUT_END_TIME, this.POPOUT_START_TIME, true), 115f) * TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.m_time, -2f, -1.85f, true), 115f);
        Vector3 pos1 = Initialise.ScreenPos(this.m_pos, this.m_posAnchor);
        float num2 = this.m_scale.X * (float) (1.0 + 0.25 * (double) TransitionFunctions.JumpySinPulse(this.m_jumpTime, 3f)) * num1;
        if ((double) this.m_time < 0.0)
          num2 *= 1f + Mortar.Math.MAX(0.0f, TransitionFunctions.SinPulse(this.m_time, 8f) * 0.125f);
        if ((double) this.m_time > (double) this.SLICE_TIME)
          pos1 += (this.m_splosionCenterBounded - this.m_splosionCenter) * (1f - Game.game_work.camera.m_transitionAmount);
        pos1.Z = 1f;
        Vector3 pos2 = Game.game_work.camera.TranslatePos(pos1, false, true);
        Color black = Color.Black;
        black.A = this.m_colour.A;
        Game.game_work.pGameFont.DrawString(this.m_bakedString, pos2 + new Vector3(-1f, -1f, 0.0f), black, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        Game.game_work.pGameFont.DrawString(this.m_bakedString, pos2 + new Vector3(1f, 1f, 0.0f), black, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        Game.game_work.pGameFont.DrawString(this.m_bakedString, pos2, this.m_colour, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        if (this.m_finalAddText.Length > 0)
        {
          double num3 = (double) TransitionFunctions.SinTransition(TransitionFunctions.GetProgressBetween(this.m_time, this.FINAL_SCORE_IN_TIME, this.FINAL_SCORE_FULL_TIME, true), 115f);
          Vector3 pos3 = TransitionFunctions.LerpF(this.m_splosionCenterBounded, this.m_splosionCenter, Game.game_work.camera.m_transitionAmount);
          Game.game_work.pGameFont.DrawString(this.m_finalAddText, pos3 + new Vector3(-1f, -1f, 0.0f), black, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
          Game.game_work.pGameFont.DrawString(this.m_finalAddText, pos3 + new Vector3(1f, 1f, 0.0f), black, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
          Game.game_work.pGameFont.DrawString(this.m_finalAddText, pos3, this.m_colour, num2 * 100f, Vector2.Zero, ALIGNMENT_TYPE.ALIGN_CENTER);
        }
        if (order != 5)
          return;
        this.DrawExplosion();
      }
    }
}
