// Decompiled with JetBrains decompiler
// Type: Mortar.DisplayManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mortar
{

    public class DisplayManager
    {
      public Matrix currentViewMtx;
      public Matrix currentWorldMtx;
      public Matrix currentProjMtx;
      public Matrix currentTextureMtx;
      public Matrix topMatrix;
      public Point Res;
      public Texture currentTexture;
      private Color clearcolor = Color.Black;
      private bool denabled;
      private bool dwriteenabled = true;
      private DepthStencilState[] depthStates;
      private BlendState bsOff;
      private BlendState bsDef;
      private RasterizerState rsCullOff;
      private RasterizerState rsCullCwise;
      public static DisplayManager instance = new DisplayManager();

      public DisplayManager()
      {
        this.Res = new Point(800, 480);
        this.depthStates = ArrayInit.CreateFilledArray<DepthStencilState>(4);
        this.depthStates[0].DepthBufferEnable = false;
        this.depthStates[0].DepthBufferWriteEnable = false;
        this.depthStates[1].DepthBufferEnable = false;
        this.depthStates[1].DepthBufferWriteEnable = true;
        this.depthStates[2].DepthBufferEnable = true;
        this.depthStates[2].DepthBufferWriteEnable = false;
        this.depthStates[3].DepthBufferEnable = true;
        this.depthStates[3].DepthBufferWriteEnable = true;
        this.bsOff = new BlendState();
        this.bsDef = new BlendState();
        this.bsDef.ColorBlendFunction = BlendFunction.Add;
        this.bsDef.ColorSourceBlend = Blend.SourceAlpha;
        this.bsDef.ColorDestinationBlend = Blend.InverseSourceAlpha;
        this.bsDef.AlphaBlendFunction = BlendFunction.Add;
        this.bsDef.AlphaSourceBlend = Blend.SourceAlpha;
        this.bsDef.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        this.rsCullCwise = new RasterizerState();
        this.rsCullCwise.CullMode = CullMode.CullClockwiseFace;
        this.rsCullOff = new RasterizerState();
        this.rsCullOff.CullMode = CullMode.None;
      }

      public static DisplayManager GetInstance() => DisplayManager.instance;

      public void SetWindowSize(int xpos, int xsize, int ypos, int ysize)
      {
      }

      public void Init(string name)
      {
        this.topMatrix = Matrix.CreateScale(new Vector3(1.66666663f, 1.5f, 1f));
      }

      public void SetClearColor(Color color) => this.clearcolor = color;

      public void SetLightDirection(Vector3 ld)
      {
      }

      public void SetGlobalAmbience(Color col)
      {
      }

      public void DidUploadMatrixies()
      {
      }

      public void BeginFrame()
      {
        TheGame.instance.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, this.clearcolor, 1f, 0);
        Mesh.newframe = true;
        this.SetBlendStateOff();
        Mesh.vertsCurrentOffset = 0;
      }

      public void EndFrame()
      {
      }

      public void SwapBuffers()
      {
      }

      public void SetNewDeptStensileState(DepthStencilState d)
      {
        TheGame.instance.GraphicsDevice.DepthStencilState = d;
      }

      public void SetNewBlendState(BlendState b) => TheGame.instance.GraphicsDevice.BlendState = b;

      public void SetNewRasterizeState(RasterizerState r)
      {
        TheGame.instance.GraphicsDevice.RasterizerState = r;
      }

      private void GetNewDepthStensileState()
      {
        this.SetNewDeptStensileState(this.depthStates[(this.denabled ? 2 : 0) + (this.dwriteenabled ? 1 : 0)]);
      }

      public void SetBlendStateOff() => this.SetNewBlendState(this.bsOff);

      public void SetBlendStateDefault() => this.SetNewBlendState(BlendState.NonPremultiplied);

      public void SetBlendStateDefault2() => this.SetNewBlendState(BlendState.NonPremultiplied);

      public void SetRasterizeStateCullOff() => this.SetNewRasterizeState(this.rsCullOff);

      public void SetRasterizeStateCullCwise() => this.SetNewRasterizeState(this.rsCullCwise);

      public void SetDepthBuffer(bool en)
      {
        this.denabled = en;
        this.GetNewDepthStensileState();
      }

      public void SetDepthBufferWrite(bool en)
      {
        this.dwriteenabled = en;
        this.GetNewDepthStensileState();
      }

      public MortarRectangle GetWindowSize()
      {
        MortarRectangle windowSize;
        windowSize.left = 0;
        windowSize.top = 0;
        windowSize.right = this.Res.X;
        windowSize.bottom = this.Res.Y;
        return windowSize;
      }
    }
}
