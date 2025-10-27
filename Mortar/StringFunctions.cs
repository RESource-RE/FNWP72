// Decompiled with JetBrains decompiler
// Type: Mortar.StringFunctions
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Mortar
{

    public class StringFunctions
    {
      public static uint StringHash(string s) => s != null ? (uint) s.GetHashCode() : 0U;

      public static bool CompareWords(string text, string word)
      {
        return text != null && word != null && text == word;
      }

      public static Texture LoadTexture(string file)
      {
        if (file == null)
          return (Texture) null;
        string texture = $"textureswp7/{file}.tex";
        return TextureManager.GetInstance().Load(texture);
      }

      public static void ParseColour(ref Color color, string text)
      {
        if (text == null)
          return;
        int[] numArray = new int[4]
        {
          (int) byte.MaxValue,
          (int) byte.MaxValue,
          (int) byte.MaxValue,
          (int) byte.MaxValue
        };
        string[] strArray = text.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < 4 && index < strArray.Length; ++index)
          numArray[index] = MParser.ParseInt(strArray[index].Trim());
        color.R = (byte) numArray[0];
        color.G = (byte) numArray[1];
        color.B = (byte) numArray[2];
        color.A = (byte) numArray[3];
      }

      public static int SplitWords(string text, ref List<string> words)
      {
        if (text == null)
          return 0;
        string str1 = text;
        char[] separator = new char[1]{ ',' };
        foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          words.Add(str2.Trim());
        return words.Count;
      }

      public static uint ParseMaskWords(string text, uint[] hashes, int numTypes)
      {
        if (text == null)
          return 0;
        uint maskWords = 0;
        string str1 = text;
        char[] separator = new char[1]{ ',' };
        foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          uint num1 = StringFunctions.StringHash(str2.Trim());
          int num2 = Array.IndexOf<uint>(hashes, num1);
          if (num2 != -1)
            maskWords |= (uint) (1 << num2);
        }
        return maskWords;
      }

      public static int FindIndex(string text, uint[] hashes, int numHashes)
      {
        if (text != null)
        {
          uint num = StringFunctions.StringHash(text);
          for (int index = 0; index < numHashes; ++index)
          {
            if ((int) hashes[index] == (int) num)
              return index;
          }
        }
        return 0;
      }

      public static bool StartsWithWord(string text, string word)
      {
        if (text == null || word == null || text.Length < word.Length)
          return false;
        for (int index = 0; index < word.Length; ++index)
        {
          if ((int) word[index] != (int) text[index])
            return false;
        }
        return true;
      }

      public static void ParseFloats(string text, float[] floats, int max)
      {
        if (text == null)
          return;
        string[] strArray = text.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < max; ++index)
          floats[index] = index >= strArray.Length ? floats[index - 1] : MParser.ParseFloat(strArray[index].Trim());
      }
    }
}
