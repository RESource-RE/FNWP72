// Decompiled with JetBrains decompiler
// Type: Mortar.Model
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mortar
{

    public class Model
    {
      public Model.Submesh[] meshes;
      public Matrix amatrix = Matrix.Identity;
      private static BasicEffect basicEffect;

      public void Draw(Matrix? mtx)
      {
        if (Model.basicEffect == null)
        {
          Model.basicEffect = new BasicEffect(TheGame.instance.GraphicsDevice);
          Model.basicEffect.VertexColorEnabled = true;
        }
        DisplayManager.instance.SetRasterizeStateCullCwise();
        bool flag = false;
        Model.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
        Matrix identity = Matrix.Identity;
        if (mtx.HasValue)
          identity = mtx.Value;
        Model.basicEffect.World = this.amatrix * identity;
        Model.basicEffect.View = DisplayManager.instance.currentViewMtx;
        for (int index = 0; index < this.meshes.Length; ++index)
        {
          if (this.meshes[index].tex == null)
          {
            Model.basicEffect.TextureEnabled = false;
          }
          else
          {
            Model.basicEffect.TextureEnabled = true;
            Model.basicEffect.Texture = this.meshes[index].tex.intex;
            flag = this.meshes[index].tex.hasAlpha;
          }
          Model.basicEffect.CurrentTechnique.Passes[0].Apply();
          if (flag)
            DisplayManager.instance.SetBlendStateDefault();
          else
            DisplayManager.instance.SetBlendStateOff();
          TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, this.meshes[index].vertecies, 0, this.meshes[index].vertecies.Length / 3);
        }
      }

      public struct Submesh
      {
        public VertexPositionColorTexture[] vertecies;
        public Texture tex;
      }
    }
}
