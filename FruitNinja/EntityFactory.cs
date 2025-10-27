// Decompiled with JetBrains decompiler
// Type: FruitNinja.EntityFactory
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Mortar;

namespace FruitNinja
{

    public static class EntityFactory
    {
      private static EntityFactory.EntityHash[] hashes = new EntityFactory.EntityHash[7]
      {
        new EntityFactory.EntityHash(true, "fruit", EntityTypes.ENTITY_BEGIN),
        new EntityFactory.EntityHash(true, "bomb", EntityTypes.ENTITY_BOMB),
        new EntityFactory.EntityHash(true, "slash", EntityTypes.ENTITY_SLASH),
        new EntityFactory.EntityHash(true, "blast", EntityTypes.ENTITY_BOMB_BLAST),
        new EntityFactory.EntityHash(true, "coin", EntityTypes.ENTITY_COIN),
        new EntityFactory.EntityHash(true, "fruitray", EntityTypes.ENTITY_FRUIT_RAY),
        new EntityFactory.EntityHash(true, "jiblet", EntityTypes.ENTITY_JIBLET)
      };

      public static Entity CreateEntity(int type)
      {
        Entity entity = (Entity) null;
        switch (type)
        {
          case 0:
            entity = (Entity) new Fruit();
            break;
          case 1:
            entity = (Entity) new Bomb();
            break;
          case 2:
            entity = (Entity) new Coin();
            break;
          case 3:
            entity = (Entity) new SlashEntity();
            break;
          case 4:
            entity = (Entity) new BombBlast();
            break;
          case 5:
            entity = (Entity) new Jiblet();
            break;
          case 6:
            entity = (Entity) new FruitRay();
            break;
        }
        return entity;
      }

      public static int HashTypeConvert(uint hash, ref bool update)
      {
        for (int index = 0; index < EntityFactory.hashes.Length; ++index)
        {
          if ((int) hash == (int) EntityFactory.hashes[index].hash)
          {
            update = EntityFactory.hashes[index].update;
            return (int) EntityFactory.hashes[index].type;
          }
        }
        update = false;
        return -1;
      }

      public struct EntityHash
      {
        public bool update;
        public uint hash;
        public uint type;

        public EntityHash(bool u, string h, EntityTypes t)
        {
            update = u;
            hash = StringFunctions.StringHash(h);
            type = (uint)t;
        }
      }
    }
}
