// Decompiled with JetBrains decompiler
// Type: Mortar.SystemManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class SystemManager
    {
      private static SystemManager instance;

      public static SystemManager GetInstance()
      {
        if (SystemManager.instance == null)
          SystemManager.instance = new SystemManager();
        return SystemManager.instance;
      }

      public void Init()
      {
      }

      public bool Update(ref float dt) => true;
    }
}
