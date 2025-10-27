// Decompiled with JetBrains decompiler
// Type: Mortar.MatrixStack
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;

namespace Mortar
{

    public class MatrixStack
    {
      protected Matrix[] m_mtxStack;
      protected Matrix m_currentMtx;
      private byte m_currentStackIndex;
      public uint version;

      public MatrixStack()
      {
        this.m_mtxStack = ArrayInit.CreateFilledArray<Matrix>(64 /*0x40*/);
        this.Reset();
        this.version = 1U;
      }

      public void Push()
      {
        this.m_mtxStack[(int) this.m_currentStackIndex] = this.m_currentMtx;
        ++this.m_currentStackIndex;
      }

      public void Pop(int num)
      {
        this.m_currentStackIndex -= (byte) num;
        this.m_currentMtx = this.m_mtxStack[(int) this.m_currentStackIndex];
        ++this.version;
      }

      public void Store(int num) => this.m_mtxStack[num] = this.m_currentMtx;

      public void Restore(int num)
      {
        this.m_currentMtx = this.m_mtxStack[num];
        ++this.version;
      }

      public void Reset()
      {
        this.m_currentStackIndex = (byte) 0;
        this.m_mtxStack[(int) this.m_currentStackIndex] = Matrix.Identity;
        this.m_currentMtx = Matrix.Identity;
        ++this.version;
      }

      public Matrix GetCurrentMatrix() => this.m_currentMtx;

      public void SetCurrentMatrix(Matrix mtx)
      {
        this.m_currentMtx = mtx;
        ++this.version;
      }

      public void Translate(Vector3 amount)
      {
        this.m_currentMtx *= Matrix.CreateTranslation(amount);
        ++this.version;
      }

      public void Translate2D(Vector2 amount)
      {
        this.m_currentMtx.M31 += amount.X;
        this.m_currentMtx.M32 += amount.Y;
        ++this.version;
      }

      public void TranslateLocal(Vector3 amount)
      {
        this.m_currentMtx = Matrix.CreateTranslation(amount) * this.m_currentMtx;
        ++this.version;
      }

      public void Scale(Vector3 amount)
      {
        this.m_currentMtx *= Matrix.CreateScale(amount);
        ++this.version;
      }

      public void RotX(float amount)
      {
        this.m_currentMtx *= Matrix.CreateRotationX(MathHelper.ToRadians(amount));
        ++this.version;
      }

      public void RotY(float amount)
      {
        this.m_currentMtx *= Matrix.CreateRotationY(MathHelper.ToRadians(amount));
        ++this.version;
      }

      public void RotZ(float amount)
      {
        this.m_currentMtx *= Matrix.CreateRotationZ(MathHelper.ToRadians(amount));
        ++this.version;
      }

      public void MtxMul(Matrix mul)
      {
        this.m_currentMtx *= mul;
        ++this.version;
      }
    }
}
