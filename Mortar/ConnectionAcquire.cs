// Decompiled with JetBrains decompiler
// Type: Mortar.ConnectionAcquire
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    internal class ConnectionAcquire
    {
      private bool complete;
      private string info;

      public ConnectionAcquire() => this.complete = false;

      public bool IsComplete() => this.complete;

      public void DoWork()
      {
        this.info = "Unknown";
        this.complete = true;
      }

      public string GetInterfaceType() => this.info;
    }
}
