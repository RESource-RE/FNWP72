// Decompiled with JetBrains decompiler
// Type: FNWP72.Resource
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace FNWP72
{

    [CompilerGenerated]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode]
    internal class Resource
    {
      private static ResourceManager resourceMan;
      private static CultureInfo resourceCulture;

      internal Resource()
      {
      }

      [EditorBrowsable(EditorBrowsableState.Advanced)]
      internal static ResourceManager ResourceManager
      {
        get
        {
          if (object.ReferenceEquals((object) Resource.resourceMan, (object) null))
            Resource.resourceMan = new ResourceManager("FNWP72.Resource", typeof (Resource).Assembly);
          return Resource.resourceMan;
        }
      }

      [EditorBrowsable(EditorBrowsableState.Advanced)]
      internal static CultureInfo Culture
      {
        get => Resource.resourceCulture;
        set => Resource.resourceCulture = value;
      }

      internal static string String1
      {
        get => Resource.ResourceManager.GetString(nameof (String1), Resource.resourceCulture);
      }
    }
}
