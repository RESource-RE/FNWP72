// Decompiled with JetBrains decompiler
// Type: Mortar.InputEvent
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class InputEvent
    {
      public uint eventType;
      public ushort chr;
      public ButtonEvent button;
      public axisEvent axis;
      public uint timeStamp;

      public enum Propagation
      {
        Continue = 0,
        Stop = 255, // 0x000000FF
      }
    }
}
