// Decompiled with JetBrains decompiler
// Type: Mortar.Input
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public static class Input
    {
      public const uint ACTION_MASK = 65535 /*0xFFFF*/;
      public const uint EVENT_MASK = 4294901760;
      public const uint EVENTTYPE_PRESSED = 1;
      public const uint EVENTTYPE_DOWN = 2;
      public const uint EVENTTYPE_RELEASED = 4;
      public const uint EVENTTYPE_UP = 8;
      public const uint EVENTTYPE_ACTIVE = 16 /*0x10*/;
      public const uint EVENTTYPE_MOVED = 32 /*0x20*/;
      public const uint EVENTTYPE_DEAD = 64 /*0x40*/;
      public const uint EVENTTYPE_BUTTON = 65536 /*0x010000*/;
      public const uint EVENTTYPE_AXIS = 131072 /*0x020000*/;
      public const uint EVENTTYPE_AXIS2D = 262144 /*0x040000*/;
    }
}
