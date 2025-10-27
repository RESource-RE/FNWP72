// Decompiled with JetBrains decompiler
// Type: Mortar.AnimationManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

namespace Mortar
{

    public class AnimationManager
    {
      private static AnimationManager instance;

      public static AnimationManager GetInstance()
      {
        if (AnimationManager.instance == null)
          AnimationManager.instance = new AnimationManager();
        return AnimationManager.instance;
      }

      public void Initialise()
      {
      }

      public void Initialise(int heapsize)
      {
      }
    }
}
