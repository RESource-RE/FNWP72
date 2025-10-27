// Decompiled with JetBrains decompiler
// Type: FruitNinja.FruitCamera
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class FruitCamera : MortarCamera
    {
      private const float CAMERA_SHAKE_RADIUS = 9f;
      private const float CAMERA_SHAKE_SPEED = 0.2f;
      protected Entity m_target;
      protected FruitCamera.CameraState m_state;
      protected ushort m_rot_z;
      protected ushort m_rot_x;
      protected Vector2 m_cameraShakeDest;
      protected ushort m_cameraShakeDirection;
      public Vector2 m_cameraShake;
      private float m_cameraShakeRadius;
      public float m_cameraShakeTime;
      public float m_cameraShakeMaxTime;
      public Vector3 Eye;
      public Vector3 LookAt;
      public Vector3 m_pos;
      public float m_zoom;
      public ushort m_rotation;
      public float m_transitionAmount;
      public Vector3 m_transitionToPos;
      public float m_transitionToZoom;
      public float m_transitionToRot;
      private float m_degree;
      public FruitCamera.TransitionFinished m_transitionFinished;

      protected void UpdateIdle(float dt)
      {
      }

      protected void UpdateFollow(float dt)
      {
        if (this.m_target != null)
        {
          Vector3 vector3 = this.m_look_at - this.m_target.m_pos;
          this.m_look_at = this.m_target.m_pos;
          FruitCamera fruitCamera = this;
          fruitCamera.m_eye = fruitCamera.m_eye - vector3;
        }
        else
          this.IdleCamera();
      }

      public FruitCamera()
      {
        this.m_target = (Entity) null;
        this.m_state = FruitCamera.CameraState.CAMERA_STATE_IDLE;
        this.m_rot_z = (ushort) 0;
        this.m_rot_x = (ushort) 0;
        this.m_cameraShake = Vector2.Zero;
        this.m_cameraShakeDest = Vector2.Zero;
        this.m_cameraShakeTime = 0.0f;
        this.m_transitionAmount = 0.0f;
        this.m_transitionToPos = Vector3.Zero;
        this.m_transitionToZoom = 1f;
        this.m_zoom = 1f;
        this.m_rotation = (ushort) 0;
        this.m_cameraShakeRadius = 0.0f;
      }

      public override void UpdateCamera(float dt)
      {
        this.UpdateShake(dt);
        this.m_pos = Vector3.Zero;
        switch (this.m_state)
        {
          case FruitCamera.CameraState.CAMERA_STATE_IDLE:
            this.UpdateIdle(dt);
            break;
          case FruitCamera.CameraState.CAMERA_STATE_FOLLOW:
            this.UpdateFollow(dt);
            break;
          case FruitCamera.CameraState.CAMERA_TRANSITION_IN:
            if ((double) this.m_transitionAmount < 1.0)
            {
              this.m_transitionAmount += dt * 3f;
              if ((double) this.m_transitionAmount >= 1.0)
              {
                this.m_transitionAmount = 1f;
                if (this.m_transitionFinished != null)
                {
                  this.m_transitionFinished();
                  break;
                }
                break;
              }
              break;
            }
            break;
          case FruitCamera.CameraState.CAMERA_TRANSITION_OUT:
            if ((double) this.m_transitionAmount > 0.0)
            {
              this.m_transitionAmount -= dt * 10f;
              if ((double) this.m_transitionAmount <= 0.0)
              {
                this.m_transitionAmount = 0.0f;
                if (this.m_transitionFinished != null)
                {
                  this.m_transitionFinished();
                  break;
                }
                break;
              }
              break;
            }
            break;
        }
        this.m_changed = true;
        this.m_zoom = TransitionFunctions.LerpF(1f, this.m_transitionToZoom, TransitionFunctions.InverseSquareTransition(this.m_transitionAmount));
        this.m_pos = TransitionFunctions.LerpF(Vector3.Zero, this.m_transitionToPos, TransitionFunctions.SinTransition(this.m_transitionAmount, 90f));
        this.m_degree = TransitionFunctions.LerpF(0.0f, this.m_transitionToRot, TransitionFunctions.InverseSquareTransition(this.m_transitionAmount));
        this.m_rotation = Math.DEGREE_TO_IDX(this.m_degree);
        this.m_pos += new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f);
      }

      public Vector3 TranslatePos(Vector3 pos, bool screenToWorld, bool includeShake)
      {
        if ((double) this.m_transitionAmount <= 0.0)
          return pos;
        if (screenToWorld)
        {
          Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_degree));
          Vector3 position = pos;
          Vector3.Transform(ref position, ref rotationZ, out pos);
          pos *= this.m_zoom;
          pos += this.m_pos - (includeShake ? Vector3.Zero : new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
        }
        else
        {
          pos -= this.m_pos - (includeShake ? Vector3.Zero : new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
          pos /= this.m_zoom;
          Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_degree));
          Vector3 position = pos;
          Vector3.Transform(ref position, ref rotationZ, out pos);
        }
        return pos;
      }

      public void UpdateShake(float dt)
      {
        if ((double) this.m_cameraShakeTime > 0.0 || (double) this.m_cameraShakeRadius > 0.0)
        {
          this.m_cameraShakeTime = Math.MAX(0.0f, this.m_cameraShakeTime - dt);
          if ((double) (this.m_cameraShakeDest - this.m_cameraShake).LengthSquared() < 16.0)
          {
            this.m_cameraShakeDirection += (ushort) ((uint) Math.DEGREE_TO_IDX(140f) + (uint) Math.g_random.Rand32((int) Math.DEGREE_TO_IDX(80f)));
            float num = (float) (9.0 * ((double) this.m_cameraShakeTime / (double) this.m_cameraShakeMaxTime)) + this.m_cameraShakeRadius;
            this.m_cameraShakeDest.X = Math.CosIdx(this.m_cameraShakeDirection) * num;
            this.m_cameraShakeDest.Y = Math.SinIdx(this.m_cameraShakeDirection) * num;
          }
          this.m_cameraShake += (this.m_cameraShakeDest - this.m_cameraShake) * 0.2f * (float) (1.0 + (double) this.m_cameraShakeTime / (double) this.m_cameraShakeMaxTime);
          this.m_changed = true;
        }
        else
        {
          this.m_cameraShakeDest = Vector2.Zero;
          if ((double) Math.ABS(this.m_cameraShake.X) > 0.0099999997764825821)
            this.m_cameraShake.X *= 0.8f;
          else
            this.m_cameraShake.X = 0.0f;
          if ((double) Math.ABS(this.m_cameraShake.Y) > 0.0099999997764825821)
            this.m_cameraShake.Y *= 0.8f;
          else
            this.m_cameraShake.Y = 0.0f;
        }
      }

      public void CreateCameraShake(Vector3 origin, float length)
      {
        this.CreateCameraShake(origin, length, 1f);
      }

      public void CreateCameraShake(Vector3 origin, float length, float strength)
      {
        this.m_cameraShakeDirection = Math.Atan2Idx(origin.Y, origin.X);
        this.m_cameraShakeDest.X = Math.CosIdx(this.m_cameraShakeDirection) * 9f;
        this.m_cameraShakeDest.Y = Math.SinIdx(this.m_cameraShakeDirection) * 9f;
        this.m_cameraShakeDest *= strength;
        this.m_cameraShakeMaxTime = this.m_cameraShakeTime = length;
      }

      public Entity GetFollowEntity()
      {
        Entity followEntity = (Entity) null;
        if (this.m_state == FruitCamera.CameraState.CAMERA_STATE_FOLLOW)
          followEntity = this.m_target;
        return followEntity;
      }

      public void FollowEntity(Entity ent)
      {
        if (ent != null)
        {
          this.m_state = FruitCamera.CameraState.CAMERA_STATE_FOLLOW;
          this.m_target = ent;
        }
        this.m_rot_x = (ushort) 0;
        this.m_rot_z = (ushort) 0;
        this.m_up = new Vector3(0.0f, 1f, 0.0f);
      }

      public void IdleCamera()
      {
        this.m_state = FruitCamera.CameraState.CAMERA_STATE_IDLE;
        this.m_target = (Entity) null;
      }

      public void SetupPerspective(FruitCamera.PERSPECIVE_TYPE orientation, bool changed)
      {
        float num = this.m_zoom;
        Vector3 target = this.m_pos;
        ushort idx = this.m_rotation;
        if (orientation == FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL_NO_SHAKE)
        {
          num = 1f;
          target = Vector3.Zero;
          idx = (ushort) 0;
        }
        if (this.m_changed || changed)
        {
          float t = 240f * num;
          float r = 400f * num;
          Vector3 camUp = new Vector3(Math.SinIdx(idx), Math.CosIdx(idx), 0.0f);
          Vector3 zero = Vector3.Zero;
          switch (orientation)
          {
            case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL:
            case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_BACKGROUND:
            case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL_NO_SHAKE:
              MatrixManager.GetInstance().SetupLookAt(new Vector3(0.0f, 0.0f, 1f) + target, camUp, target);
              this.m_view_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
              MatrixManager.GetInstance().SetupOrtho(t, -t, -r, r, -6000f, 6000f);
              this.m_proj_mtx = MatrixManager.GetInstance().GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
              break;
            case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_LEFT:
              MatrixManager.instance.SetupLookAt(new Vector3(0.0f, 0.0f, 1f) + new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f), new Vector3(1f, 0.0f, 0.0f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
              this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
              MatrixManager.instance.SetupOrtho((float) -(DisplayManager.GetInstance().Res.X >> 1), (float) (DisplayManager.GetInstance().Res.X >> 1), (float) (DisplayManager.GetInstance().Res.Y >> 1), (float) -(DisplayManager.GetInstance().Res.Y >> 1) - (float) DisplayManager.GetInstance().Res.Y, -6000f, 6000f);
              break;
            case FruitCamera.PERSPECIVE_TYPE.ORIENTATION_RIGHT:
              MatrixManager.instance.SetupLookAt(new Vector3(0.0f, 0.0f, 1f) + new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f), new Vector3(1f, 0.0f, 0.0f), new Vector3(this.m_cameraShake.X, this.m_cameraShake.Y, 0.0f));
              this.m_view_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
              MatrixManager.instance.SetupOrtho((float) (DisplayManager.GetInstance().Res.X >> 1), (float) -(DisplayManager.GetInstance().Res.X >> 1), (float) -(DisplayManager.GetInstance().Res.Y >> 1) - (float) DisplayManager.GetInstance().Res.Y, (float) (DisplayManager.GetInstance().Res.Y >> 1), -6000f, 6000f);
              this.m_proj_mtx = MatrixManager.instance.GetMatrix(MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
              break;
          }
        }
        else
        {
          MatrixManager.instance.SetMatrix(this.m_view_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_VIEW);
          MatrixManager.instance.SetMatrix(this.m_proj_mtx, MatrixManager.MatrixStackTypes.MATRIXSTACK_PROJECTION);
        }
        MatrixManager.instance.Reset(MatrixManager.MatrixStackTypes.MATRIXSTACK_WORLD);
      }

      public void SetupPerspective(FruitCamera.PERSPECIVE_TYPE orientation)
      {
        this.SetupPerspective(orientation, false);
      }

      public new void SetupPerspective()
      {
        this.SetupPerspective(FruitCamera.PERSPECIVE_TYPE.ORIENTATION_NORMAL, false);
      }

      public void TransitionOut() => this.m_state = FruitCamera.CameraState.CAMERA_TRANSITION_OUT;

      public bool IsTransitionIn() => this.m_state == FruitCamera.CameraState.CAMERA_TRANSITION_IN;

      public void Transition(Vector3 pos, float zoom)
      {
        this.Transition(pos, zoom, 0.0f, (FruitCamera.TransitionFinished) null);
      }

      public void Transition(Vector3 pos, float zoom, float rot, FruitCamera.TransitionFinished del)
      {
        this.m_state = FruitCamera.CameraState.CAMERA_TRANSITION_IN;
        this.m_transitionToPos = pos;
        this.m_transitionToZoom = zoom;
        this.m_transitionToRot = rot;
        this.m_transitionFinished = del;
      }

      public bool ViewIsNormal()
      {
        return (double) this.m_cameraShake.X == 0.0 && (double) this.m_cameraShake.Y == 0.0 && (double) this.m_transitionAmount <= 0.0;
      }

      public enum CameraState
      {
        CAMERA_STATE_IDLE,
        CAMERA_STATE_FOLLOW,
        CAMERA_TRANSITION_IN,
        CAMERA_TRANSITION_OUT,
      }

      public enum PERSPECIVE_TYPE
      {
        ORIENTATION_NORMAL,
        ORIENTATION_BACKGROUND,
        ORIENTATION_LEFT,
        ORIENTATION_RIGHT,
        ORIENTATION_NORMAL_NO_SHAKE,
      }

      public delegate void TransitionFinished();
    }
}
