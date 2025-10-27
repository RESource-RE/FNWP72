// Decompiled with JetBrains decompiler
// Type: Mortar.ArrayInit
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.Collections.Generic;

namespace Mortar
{

    public static class ArrayInit
    {
      public static List<T> CreateInitedList<T>(int initsize)
      {
        List<T> initedList = new List<T>(initsize);
        for (int index = 0; index < initsize; ++index)
          initedList.Add(default (T));
        return initedList;
      }

      public static T[] CreateFilledArray<T>(int size) where T : new()
      {
        T[] filledArray = new T[size];
        for (int index = 0; index < size; ++index)
          filledArray[index] = new T();
        return filledArray;
      }

      public static T[,] CreateFilledArray<T>(int size, int size2) where T : new()
      {
        T[,] filledArray = new T[size, size2];
        for (int index1 = 0; index1 < size; ++index1)
        {
          for (int index2 = 0; index2 < size2; ++index2)
            filledArray[index1, index2] = new T();
        }
        return filledArray;
      }
    }
}
