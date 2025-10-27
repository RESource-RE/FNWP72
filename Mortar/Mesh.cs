// Decompiled with JetBrains decompiler
// Type: Mortar.Mesh
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mortar
{

    public class Mesh
    {
      private static Matrix view;
      private static Matrix proj;
      public static bool newframe;
      private static BasicEffect basicEffect = (BasicEffect) null;
      private static VertexPositionColorTexture[] qverts = new VertexPositionColorTexture[4];
      public static VertexPositionColorTexture[] verts = new VertexPositionColorTexture[10000];
      public static int vertsCurrentOffset = 0;

      public static void DrawQuad(Color col) => Mesh.DrawQuad(col, 0.0f, 1f, 0.0f, 1f);

      public static void DrawQuad(Color col, float u0, float u1, float v0, float v1)
      {
        if (Mesh.basicEffect == null)
        {
          Mesh.basicEffect = new BasicEffect(TheGame.instance.GraphicsDevice);
          Mesh.basicEffect.VertexColorEnabled = true;
          Mesh.qverts[0].Position = new Vector3(-0.5f, 0.5f, 0.0f);
          Mesh.qverts[1].Position = new Vector3(-0.5f, -0.5f, 0.0f);
          Mesh.qverts[2].Position = new Vector3(0.5f, 0.5f, 0.0f);
          Mesh.qverts[3].Position = new Vector3(0.5f, -0.5f, 0.0f);
        }
        DisplayManager.instance.SetRasterizeStateCullOff();
        if (Mesh.newframe)
        {
          Mesh.view = DisplayManager.instance.currentViewMtx;
          Mesh.proj = DisplayManager.instance.currentProjMtx;
          Mesh.newframe = false;
        }
        bool flag = false;
        Mesh.basicEffect.Projection = Mesh.proj;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = Mesh.view;
        if (DisplayManager.instance.currentTexture == null)
        {
          Mesh.basicEffect.TextureEnabled = false;
        }
        else
        {
          Mesh.basicEffect.TextureEnabled = true;
          Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
          flag = DisplayManager.instance.currentTexture.hasAlpha;
        }
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        if (col.A != byte.MaxValue || flag)
          DisplayManager.instance.SetBlendStateDefault();
        else
          DisplayManager.instance.SetBlendStateOff();
        Mesh.qverts[0].TextureCoordinate = new Vector2(u0, v0);
        Mesh.qverts[0].Color = col;
        Mesh.qverts[1].TextureCoordinate = new Vector2(u0, v1);
        Mesh.qverts[1].Color = col;
        Mesh.qverts[2].TextureCoordinate = new Vector2(u1, v0);
        Mesh.qverts[2].Color = col;
        Mesh.qverts[3].TextureCoordinate = new Vector2(u1, v1);
        Mesh.qverts[3].Color = col;
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, Mesh.qverts, 0, 2);
      }

      public static void DrawQuad(Color col, float u0, float u1, float v0, float v1, bool alpha)
      {
        if (Mesh.basicEffect == null)
        {
          Mesh.basicEffect = new BasicEffect(TheGame.instance.GraphicsDevice);
          Mesh.basicEffect.VertexColorEnabled = true;
          Mesh.qverts[0].Position = new Vector3(-0.5f, 0.5f, 0.0f);
          Mesh.qverts[1].Position = new Vector3(-0.5f, -0.5f, 0.0f);
          Mesh.qverts[2].Position = new Vector3(0.5f, 0.5f, 0.0f);
          Mesh.qverts[3].Position = new Vector3(0.5f, -0.5f, 0.0f);
        }
        DisplayManager.instance.SetRasterizeStateCullOff();
        if (Mesh.newframe)
        {
          Mesh.view = DisplayManager.instance.currentViewMtx;
          Mesh.proj = DisplayManager.instance.currentProjMtx;
          Mesh.newframe = false;
        }
        Mesh.basicEffect.Projection = Mesh.proj;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = Mesh.view;
        if (DisplayManager.instance.currentTexture == null)
        {
          Mesh.basicEffect.TextureEnabled = false;
        }
        else
        {
          Mesh.basicEffect.TextureEnabled = true;
          Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
        }
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        Mesh.qverts[0].TextureCoordinate = new Vector2(u0, v0);
        Mesh.qverts[0].Color = col;
        Mesh.qverts[1].TextureCoordinate = new Vector2(u0, v1);
        Mesh.qverts[1].Color = col;
        Mesh.qverts[2].TextureCoordinate = new Vector2(u1, v0);
        Mesh.qverts[2].Color = col;
        Mesh.qverts[3].TextureCoordinate = new Vector2(u1, v1);
        Mesh.qverts[3].Color = col;
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, Mesh.qverts, 0, 2);
      }

      public static void DrawTriList(GameVertex[] data, int numPoints)
      {
        Mesh.DrawTriList(data, numPoints, false);
      }

      public static void DrawTriList(GameVertex[] data, int numPoints, bool hasAlpha)
      {
        Mesh.DrawTriList(data, numPoints, hasAlpha, 0);
      }

      public static void DrawTriList(GameVertex[] data, int numPoints, bool hasAlpha, int offset)
      {
        Mesh.DrawPrimitives(PrimitiveType.TriangleList, data, numPoints, hasAlpha, numPoints / 3, offset);
      }

      public static void DrawTriListEx(
        VertexPositionColorTexture[] data,
        int numPoints,
        bool hasAlpha,
        int offset)
      {
        Mesh.DrawPrimitivesEx(PrimitiveType.TriangleList, data, numPoints, hasAlpha, numPoints / 3, offset);
      }

      public static void DrawTriStrip(GameVertex[] data, int numPoints)
      {
        Mesh.DrawTriStrip(data, numPoints, false);
      }

      public static void DrawTriStrip(GameVertex[] data, int numPoints, bool hasAlpha)
      {
        Mesh.DrawPrimitives(PrimitiveType.TriangleStrip, data, numPoints, hasAlpha, numPoints - 2, 0);
      }

      public static void DrawTriStripEx(
        VertexPositionColorTexture[] data,
        int numPoints,
        bool hasAlpha)
      {
        Mesh.DrawPrimitivesEx(PrimitiveType.TriangleStrip, data, numPoints, hasAlpha, numPoints - 2, 0);
      }

      private static void WriteVertData(
        ref VertexPositionColorTexture vert1,
        ref GameVertex pData1,
        ref VertexPositionColorTexture vert2,
        ref GameVertex pData2,
        ref VertexPositionColorTexture vert3,
        ref GameVertex pData3)
      {
        vert1.Position.X = pData1.X;
        vert1.Position.Y = pData1.Y;
        vert1.Position.Z = pData1.Z;
        vert1.Color = pData1.color;
        vert1.TextureCoordinate.X = pData1.u;
        vert1.TextureCoordinate.Y = pData1.v;
        vert2.Position.X = pData2.X;
        vert2.Position.Y = pData2.Y;
        vert2.Position.Z = pData2.Z;
        vert2.Color = pData2.color;
        vert2.TextureCoordinate.X = pData2.u;
        vert2.TextureCoordinate.Y = pData2.v;
        vert3.Position.X = pData3.X;
        vert3.Position.Y = pData3.Y;
        vert3.Position.Z = pData3.Z;
        vert3.Color = pData3.color;
        vert3.TextureCoordinate.X = pData3.u;
        vert3.TextureCoordinate.Y = pData3.v;
      }

      private static void WriteVertData(ref VertexPositionColorTexture vert, ref GameVertex pData)
      {
        vert.Position.X = pData.X;
        vert.Position.Y = pData.Y;
        vert.Position.Z = pData.Z;
        vert.Color = pData.color;
        vert.TextureCoordinate.X = pData.u;
        vert.TextureCoordinate.Y = pData.v;
      }

      private static void DrawPrimitives(
        PrimitiveType ptype,
        GameVertex[] data,
        int numPoints,
        bool hashAlpah,
        int numPrims,
        int offset)
      {
        DisplayManager.instance.SetRasterizeStateCullOff();
        Mesh.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = DisplayManager.instance.currentViewMtx;
        if (DisplayManager.instance.currentTexture == null)
        {
          Mesh.basicEffect.TextureEnabled = false;
        }
        else
        {
          Mesh.basicEffect.TextureEnabled = true;
          Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
          if (DisplayManager.instance.currentTexture.hasAlpha)
            hashAlpah = true;
        }
        if (hashAlpah)
          DisplayManager.instance.SetBlendStateDefault();
        else
          DisplayManager.instance.SetBlendStateOff();
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        int vertsCurrentOffset = Mesh.vertsCurrentOffset;
        for (int index = 0; index < numPoints; ++index)
          Mesh.WriteVertData(ref Mesh.verts[Mesh.vertsCurrentOffset++], ref data[index + offset]);
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, Mesh.verts, vertsCurrentOffset, numPrims);
      }

      public static void DrawPrimitives2(
        PrimitiveType ptype,
        GameVertex[] data,
        int numPoints,
        bool hashAlpah,
        int numPrims,
        int offset)
      {
        DisplayManager.instance.SetRasterizeStateCullOff();
        Mesh.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = DisplayManager.instance.currentViewMtx;
        if (DisplayManager.instance.currentTexture == null)
        {
          Mesh.basicEffect.TextureEnabled = false;
        }
        else
        {
          Mesh.basicEffect.TextureEnabled = true;
          Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
          if (DisplayManager.instance.currentTexture.hasAlpha)
            hashAlpah = true;
        }
        if (hashAlpah)
          DisplayManager.instance.SetBlendStateDefault2();
        else
          DisplayManager.instance.SetBlendStateOff();
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        int vertsCurrentOffset = Mesh.vertsCurrentOffset;
        int num = 0;
        while (num < numPoints)
        {
          if (numPoints >= 3)
          {
            Mesh.WriteVertData(ref Mesh.verts[Mesh.vertsCurrentOffset++], ref data[num + offset], ref Mesh.verts[Mesh.vertsCurrentOffset++], ref data[num + offset + 1], ref Mesh.verts[Mesh.vertsCurrentOffset++], ref data[num + offset + 2]);
            num += 3;
          }
          else
          {
            Mesh.WriteVertData(ref Mesh.verts[Mesh.vertsCurrentOffset++], ref data[num + offset]);
            ++num;
          }
        }
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, Mesh.verts, vertsCurrentOffset, numPrims);
      }

      public static void DrawPrimitivesEx(
        PrimitiveType ptype,
        VertexPositionColorTexture[] data,
        int numPoints,
        bool hashAlpha,
        int numPrims,
        int offset)
      {
        DisplayManager.instance.SetRasterizeStateCullOff();
        Mesh.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = DisplayManager.instance.currentViewMtx;
        if (DisplayManager.instance.currentTexture == null)
        {
          Mesh.basicEffect.TextureEnabled = false;
        }
        else
        {
          Mesh.basicEffect.TextureEnabled = true;
          Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
          if (DisplayManager.instance.currentTexture.hasAlpha)
            hashAlpha = true;
        }
        if (hashAlpha)
          DisplayManager.instance.SetBlendStateDefault();
        else
          DisplayManager.instance.SetBlendStateOff();
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, data, offset, numPrims);
      }

      public static void DrawPrimitivesEx3(
        PrimitiveType ptype,
        VertexPositionColorTexture[] data,
        int numPoints,
        int numPrims,
        int offset,
        int cnt)
      {
        if (cnt == 0)
        {
          DisplayManager.instance.SetRasterizeStateCullOff();
          Mesh.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
          Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
          Mesh.basicEffect.View = DisplayManager.instance.currentViewMtx;
          Mesh.basicEffect.TextureEnabled = true;
        }
        if (DisplayManager.instance.currentTexture.hasAlpha)
          DisplayManager.instance.SetBlendStateDefault();
        else
          DisplayManager.instance.SetBlendStateOff();
        Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, data, offset, numPrims);
      }

      public static void DrawPrimitives2Ex(
        PrimitiveType ptype,
        VertexPositionColorTexture[] data,
        int numPoints,
        bool hashAlpah,
        int numPrims,
        int offset)
      {
        if (offset == 0)
        {
          if (DisplayManager.instance.currentTexture == null)
          {
            Mesh.basicEffect.TextureEnabled = false;
          }
          else
          {
            Mesh.basicEffect.TextureEnabled = true;
            Mesh.basicEffect.Texture = DisplayManager.instance.currentTexture.intex;
            if (DisplayManager.instance.currentTexture.hasAlpha)
              hashAlpah = true;
          }
          if (hashAlpah)
            DisplayManager.instance.SetBlendStateDefault2();
          else
            DisplayManager.instance.SetBlendStateOff();
        }
        Mesh.basicEffect.Projection = DisplayManager.instance.currentProjMtx;
        Mesh.basicEffect.World = DisplayManager.instance.currentWorldMtx;
        Mesh.basicEffect.View = DisplayManager.instance.currentViewMtx;
        Mesh.basicEffect.CurrentTechnique.Passes[0].Apply();
        TheGame.instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(ptype, data, 0, numPrims);
      }
    }
}
