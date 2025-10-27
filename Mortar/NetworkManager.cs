// Decompiled with JetBrains decompiler
// Type: Mortar.NetworkManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class NetworkManager
    {
      private static NetworkManager instance;

      public static NetworkManager GetInstance()
      {
        if (NetworkManager.instance == null)
          NetworkManager.instance = new NetworkManager();
        return NetworkManager.instance;
      }

      public bool UserHasEnabledNetwork() => true;

      public bool UnlockAchievement(string id) => true;

      public bool IsOnline() => false;
    }
}
