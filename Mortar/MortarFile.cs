// Decompiled with JetBrains decompiler
// Type: Mortar.MortarFile
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Mortar
{

    public class MortarFile
    {
      public static Stream LoadBinStream(string fl)
      {
        try
        {
          Stream stream = TitleContainer.OpenStream($"Content/{fl}.xnb");
          if (stream != null)
          {
            stream.Seek(207L, SeekOrigin.Begin);
            return stream;
          }
        }
        catch
        {
          return (Stream) null;
        }
        return (Stream) null;
      }

      public static BinaryReader LoadBinBR(string fl)
      {
        try
        {
          return new BinaryReader(MortarFile.LoadBinStream(fl));
        }
        catch (Exception ex)
        {
          return (BinaryReader) null;
        }
      }

      public static byte[] LoadBin(string fl)
      {
        Stream stream = MortarFile.LoadBinStream(fl);
        if (stream == null)
          return (byte[]) null;
        byte[] buffer = new byte[stream.Length - stream.Position];
        stream.Read(buffer, 0, buffer.Length);
        return buffer;
      }

      public static string LoadText(string name)
      {
        try
        {
          return TheGame.instance.Content.Load<string>(name);
        }
        catch (Exception ex)
        {
          return (string) null;
        }
      }

      public class ByteStream : Stream
      {
        private byte[] data;
        private long p;

        public ByteStream(byte[] d)
        {
          this.data = d;
          this.p = 0L;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => (long) this.data.Length;

        public override long Position
        {
          get => this.p;
          set => this.p = value;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
          Array.Copy((Array) this.data, (int) this.p, (Array) buffer, offset, count);
          this.p += (long) count;
          return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
          switch (origin)
          {
            case SeekOrigin.Begin:
              this.p = offset;
              break;
            case SeekOrigin.Current:
              this.p += offset;
              break;
            case SeekOrigin.End:
              this.p = (long) this.data.Length + offset;
              break;
          }
          if (this.p > (long) this.data.Length)
            this.p = (long) this.data.Length;
          if (this.p < 0L)
            this.p = 0L;
          return this.p;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }
      }
    }
}
