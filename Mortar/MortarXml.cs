// Decompiled with JetBrains decompiler
// Type: Mortar.MortarXml
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System;
using System.Xml.Linq;

namespace Mortar
{

    public class MortarXml
    {
      public static void Save(XDocument doc, string name)
      {
      }

      public static XDocument Load(string name)
      {
        string text;
        try
        {
          text = TheGame.instance.Content.Load<string>(name);
        }
        catch (Exception ex)
        {
          return (XDocument) null;
        }
        if (text == null)
          return (XDocument) null;
        return XDocument.Parse(text);
      }
    }
}
