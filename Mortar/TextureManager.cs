// Decompiled with JetBrains decompiler
// Type: Mortar.TextureManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using FNWP72.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Mortar
{

    public class TextureManager
    {
      private static TextureManager instance;
      private static List<Texture> loadedTextures = new List<Texture>();

      public static TextureManager GetInstance()
      {
        if (TextureManager.instance == null)
          TextureManager.instance = new TextureManager();
        return TextureManager.instance;
      }

      public void Initialise()
      {
      }

      public void Initialise(int heapsize)
      {
      }

      public static bool TextureFileExists(string fileName) => Texture.FileExists(fileName);

      public Texture Load(string texture)
      {
        Texture texture1 = Texture.Load(texture);
        if (texture1 != null)
          TextureManager.loadedTextures.Add(texture1);
        return texture1;
      }

      public Texture Load(string texture, bool localise)
      {
        Texture texture1 = Texture.Load($"{MTLocalisation.GetLocalisedTexturePath()}/{texture}");
        if (texture1 != null)
        {
          TextureManager.loadedTextures.Add(texture1);
          texture1.localise = localise;
          texture1.texture_filename = texture;
        }
        return texture1;
      }

      public void ReloadLocalisedTextures(StringTableUtils.Language language)
      {
        foreach (Texture loadedTexture in TextureManager.loadedTextures)
        {
          if (loadedTexture.localise)
            loadedTexture.intex = (Texture2D) null;
        }
        GC.Collect();
        List<Texture>.Enumerator enumerator = TextureManager.loadedTextures.GetEnumerator();
        enumerator.MoveNext();
        while (enumerator.Current != null)
        {
          Texture current = enumerator.Current;
          if (current.localise)
          {
            string fileName = $"{MTLocalisation.GetLocalisedTexturePath(language)}/{current.texture_filename}";
            if (!fileName.EndsWith(".tex"))
              fileName += ".tex";
            Texture.Reload(fileName, current);
          }
          enumerator.MoveNext();
        }
      }
    }
}
