// Decompiled with JetBrains decompiler
// Type: FruitNinja.PurchaseInfo
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;
using System.Xml.Linq;

namespace FruitNinja
{

    public class PurchaseInfo
    {
      protected int m_games;
      protected int m_cost;
      protected string m_description;
      protected string m_title;
      protected Texture m_texture;
      protected Texture m_inUseTexture;
      protected Texture m_greyTexture;
      public int CurrentGames;

      public PurchaseInfo Duplicate()
      {
        return new PurchaseInfo()
        {
          m_games = this.m_games,
          m_cost = this.m_cost,
          m_description = this.m_description,
          m_title = this.m_title,
          m_texture = this.m_texture,
          m_inUseTexture = this.m_inUseTexture,
          m_greyTexture = this.m_greyTexture
        };
      }

      public PurchaseInfo()
      {
        this.m_games = 0;
        this.CurrentGames = 0;
        this.m_cost = 0;
      }

      public string GetDescription() => this.m_description;

      public string GetTitle() => this.m_title;

      public Texture GetTexture() => this.m_texture;

      public Texture GetInUseTexture() => this.m_inUseTexture;

      public Texture GetGreyTexture() => this.m_greyTexture;

      public int GetCost() => this.m_cost;

      public int GetGames() => this.m_games;

      public void Parse(XElement element)
      {
        element.QueryIntAttribute("games", ref this.m_games);
        this.CurrentGames = this.m_games;
        element.QueryIntAttribute("cost", ref this.m_cost);
        string str = element.AttributeStr("title");
        if (str != null)
          this.m_title = str;
        string texture1 = $"textureswp7/{element.AttributeStr("texture") ?? "arcade_item_01_buy"}.tex";
        this.m_texture = TextureManager.GetInstance().Load(texture1);
        string texture2 = $"textureswp7/{element.AttributeStr("selectedTexture") ?? "arcade_item_01_selected"}.tex";
        this.m_inUseTexture = TextureManager.GetInstance().Load(texture2);
        string texture3 = $"textureswp7/{element.AttributeStr("usedTexture") ?? "arcade_item_01_used"}.tex";
        this.m_greyTexture = TextureManager.GetInstance().Load(texture3);
        XElement element1 = element.FirstChildElement("description");
        string text = element1 != null ? element1.GetText() : (string) null;
        if (text == null)
          return;
        this.m_description = text;
      }
    }
}
