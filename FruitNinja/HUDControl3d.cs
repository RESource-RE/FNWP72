// Decompiled with JetBrains decompiler
// Type: FruitNinja.HUDControl3d
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;

namespace FruitNinja
{

    public class HUDControl3d : HUDControl
    {
      public Texture m_texture;

      public override void Save()
      {
      }

      public override void Release()
      {
      }

      public override void PreDraw(float[] tintChannels)
      {
      }

      public override void Draw(float[] tintChannels)
      {
        if (this.m_color.A <= (byte) 0 || this.m_texture == null)
          return;
        this.m_texture.Set();
        MatrixManager.GetInstance().Reset();
        Matrix scale = Matrix.CreateScale(this.m_scale);
        if ((double) this.m_rotation != 0.0)
          scale *= Matrix.CreateRotationZ(MathHelper.ToRadians(this.m_rotation));
        Matrix mtx = scale * Matrix.CreateTranslation(this.m_pos);
        MatrixManager.GetInstance().SetMatrix(mtx);
        MatrixManager.GetInstance().UploadCurrentMatrices(true);
        Mesh.DrawQuad(HUDControl.TintColor(this.m_color, tintChannels), this.m_uvs[0].X, this.m_uvs[1].X, this.m_uvs[0].Y, this.m_uvs[1].Y);
      }

      public override void Update(float dt) => base.Update(dt);

      public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_3D;

      public override void Skip()
      {
      }
    }
}
