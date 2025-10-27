// Decompiled with JetBrains decompiler
// Type: Mortar.MeshManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Mortar
{

    public class MeshManager
    {
      private static MeshManager instance = new MeshManager();

      public static MeshManager GetInstance() => MeshManager.instance;

      public Model Load(string modelFileName)
      {
        modelFileName = Path.ChangeExtension(modelFileName, ".wp7mesh");
        string directoryName = Path.GetDirectoryName(modelFileName);
        Model model = new Model();
        BinaryReader binaryReader1 = MortarFile.LoadBinBR(modelFileName);
        if (binaryReader1 == null)
          return model;
        byte[] d = binaryReader1.ReadBytes((int) (binaryReader1.BaseStream.Length - binaryReader1.BaseStream.Position));
        binaryReader1.Close();
        BinaryReader binaryReader2 = new BinaryReader((Stream) new MortarFile.ByteStream(d));
        Matrix matrix1 = new Matrix();
        Vector3 position = new Vector3();
        Quaternion quaternion = new Quaternion();
        Matrix identity = Matrix.Identity;
        matrix1.M11 = binaryReader2.ReadSingle();
        matrix1.M12 = binaryReader2.ReadSingle();
        matrix1.M13 = binaryReader2.ReadSingle();
        matrix1.M14 = binaryReader2.ReadSingle();
        matrix1.M21 = binaryReader2.ReadSingle();
        matrix1.M22 = binaryReader2.ReadSingle();
        matrix1.M23 = binaryReader2.ReadSingle();
        matrix1.M24 = binaryReader2.ReadSingle();
        matrix1.M31 = binaryReader2.ReadSingle();
        matrix1.M32 = binaryReader2.ReadSingle();
        matrix1.M33 = binaryReader2.ReadSingle();
        matrix1.M34 = binaryReader2.ReadSingle();
        matrix1.M41 = binaryReader2.ReadSingle();
        matrix1.M42 = binaryReader2.ReadSingle();
        matrix1.M43 = binaryReader2.ReadSingle();
        matrix1.M44 = binaryReader2.ReadSingle();
        position.X = binaryReader2.ReadSingle();
        position.Y = binaryReader2.ReadSingle();
        position.Z = binaryReader2.ReadSingle();
        quaternion.X = binaryReader2.ReadSingle();
        quaternion.Y = binaryReader2.ReadSingle();
        quaternion.Z = binaryReader2.ReadSingle();
        quaternion.W = binaryReader2.ReadSingle();
        identity.M11 = binaryReader2.ReadSingle();
        identity.M12 = binaryReader2.ReadSingle();
        identity.M13 = binaryReader2.ReadSingle();
        identity.M21 = binaryReader2.ReadSingle();
        identity.M22 = binaryReader2.ReadSingle();
        identity.M23 = binaryReader2.ReadSingle();
        identity.M31 = binaryReader2.ReadSingle();
        identity.M32 = binaryReader2.ReadSingle();
        identity.M33 = binaryReader2.ReadSingle();
        Matrix matrix2 = identity * Matrix.Transpose(Matrix.CreateFromQuaternion(quaternion)) * Matrix.Transpose(Matrix.CreateTranslation(position));
        model.amatrix = matrix1 * matrix2;
        int length1 = binaryReader2.ReadInt32();
        Texture[] textureArray = new Texture[length1];
        for (int index = 0; index < length1; ++index)
        {
          int count = binaryReader2.ReadInt32();
          if (count > 0)
          {
            string str = new string(binaryReader2.ReadChars(count));
            textureArray[index] = Texture.Load($"{directoryName}\\{str}");
          }
        }
        int length2 = binaryReader2.ReadInt32();
        model.meshes = new Model.Submesh[length2];
        for (int index1 = 0; index1 < length2; ++index1)
        {
          int index2 = binaryReader2.ReadInt32();
          int length3 = binaryReader2.ReadInt32();
          model.meshes[index1].tex = textureArray[index2];
          model.meshes[index1].vertecies = new VertexPositionColorTexture[length3];
          for (int index3 = 0; index3 < length3; ++index3)
          {
            float x1 = binaryReader2.ReadSingle();
            float y1 = binaryReader2.ReadSingle();
            float z = binaryReader2.ReadSingle();
            float x2 = binaryReader2.ReadSingle();
            float y2 = binaryReader2.ReadSingle();
            uint num = binaryReader2.ReadUInt32();
            model.meshes[index1].vertecies[index3].Color = new Color()
            {
              PackedValue = num
            };
            model.meshes[index1].vertecies[index3].Position = new Vector3(x1, y1, z);
            model.meshes[index1].vertecies[index3].TextureCoordinate = new Vector2(x2, y2);
          }
        }
        return model;
      }

      public void Initialise()
      {
      }

      public void Initialise(int heapsize)
      {
      }
    }
}
