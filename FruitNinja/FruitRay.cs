// Decompiled with JetBrains decompiler
// Type: FruitNinja.FruitRay
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;

namespace FruitNinja
{

    public class FruitRay : Entity
    {
      public float m_alpha;
      public Vector3 m_startScale;
      public Vector3 m_maxScale;
      public bool m_fadeOut;
      private Matrix m_oreintation;
      private Matrix m_oreintationOffset;
      private Fruit m_fruit;
      private float m_time;
      public static Texture RayTexture = (Texture) null;
      private static GameVertex[] verts = new GameVertex[3];

      public void Init(Fruit fruit, Quaternion offset)
      {
        this.m_destroy = false;
        this.m_dormant = false;
        this.m_fadeOut = false;
        this.m_fruit = fruit;
        this.m_maxScale = Vector3.One * Utils.GetRandBetween(70f, 120f);
        this.m_startScale = Vector3.One * 40f;
        this.m_cur_scale = this.m_startScale;
        this.m_time = 0.0f;
        this.m_alpha = 1f;
        this.m_pos = this.m_fruit.m_pos;
        offset.Normalize();
        this.m_oreintationOffset = Matrix.CreateFromQuaternion(offset);
        this.m_oreintation = Matrix.CreateFromQuaternion(this.m_fruit.m_rotation_piece[0]);
      }

      public override void Draw()
      {
      }

      public override void DrawUpdate(float dt)
      {
      }

      public override void Update(float dt)
      {
        if (this.m_fadeOut)
        {
          this.m_alpha -= dt * 1.6f;
          this.m_fruit = (Fruit) null;
          if ((double) this.m_alpha > 0.0)
            return;
          this.m_destroy = true;
        }
        else
        {
          this.m_time += Game.game_work.dt;
          this.m_pos = this.m_fruit.m_pos + Vector3.UnitZ * this.m_fruit.m_z;
          this.m_oreintation = Matrix.CreateFromQuaternion(this.m_fruit.m_rotation_piece[0]);
          this.m_cur_scale = TransitionFunctions.LerpF(this.m_startScale, this.m_maxScale, TransitionFunctions.GetProgressBetween(this.m_time, 0.0f, 0.15f, true));
        }
      }

      private void DrawRay()
      {
        for (int index = 0; index < 3; ++index)
        {
          FruitRay.verts[index].X = FruitRay.verts[index].Y = FruitRay.verts[index].Z = FruitRay.verts[index].nx = FruitRay.verts[index].ny = 0.0f;
          FruitRay.verts[index].nz = 1f;
          FruitRay.verts[index].v = index == 0 ? 1f : 0.05f;
          FruitRay.verts[index].color = new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue * this.m_alpha);
        }
        FruitRay.verts[0].u = 0.5f;
        FruitRay.verts[1].u = 0.0f;
        FruitRay.verts[2].u = 1f;
        FruitRay.verts[1].X = -0.25f;
        FruitRay.verts[2].X = 0.25f;
        FruitRay.verts[1].Z = FruitRay.verts[2].Z = 1f;
        Matrix mtx = Matrix.CreateScale(this.m_cur_scale * (float) (3.0 - (double) this.m_alpha * 2.0)) * this.m_oreintationOffset * this.m_oreintation * Matrix.CreateTranslation(this.m_pos);
        if (FruitRay.RayTexture != null)
          FruitRay.RayTexture.Set();
        MatrixManager.GetInstance().SetMatrix(mtx);
        MatrixManager.GetInstance().UploadCurrentMatrices(true);
        Mesh.DrawTriStrip(FruitRay.verts, 3, true);
      }

      public static void DrawRays()
      {
        LinkedListNode<Entity> iterator = (LinkedListNode<Entity>) null;
        for (FruitRay fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityFirst(EntityTypes.ENTITY_FRUIT_RAY, ref iterator); fruitRay != null; fruitRay = (FruitRay) ActorManager.GetInstance().GetEntityNext(EntityTypes.ENTITY_FRUIT_RAY, ref iterator))
          fruitRay.DrawRay();
      }
    }
}
