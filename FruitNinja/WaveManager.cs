// Decompiled with JetBrains decompiler
// Type: FruitNinja.WaveManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FruitNinja
{

    public class WaveManager
    {
      public const float COMBOS_REQUIRED_FOR_NEW_BLITZ = 2.5f;
      public const float MAX_COMBO_BONUS = 6f;
      public bool paused;
      public static float MAX_SPEEDIE_GONZALES = 14f;
      public Math.Random w_random = new Math.Random();
      private Math.Random m_random = new Math.Random();
      public static int MAX_PROBABILITY_OVERIDE_TYPES = 20;
      public static float FRUIT_SPAWN_MIN_X = -150f;
      public static float FRUIT_SPAWN_MAX_X = 150f;
      public static float FRUIT_ANGLE_RANGE = 20f;
      public static float FRUIT_MAX_VEL_X = 15f;
      public static float FRUIT_MIN_VEL_Y = 9f;
      public static float FRUIT_MAX_VEL_Y = 11f;
      public static float FRUIT_MIN_VEL = 9.5f;
      public static float FRUIT_MAX_VEL = 11f;
      public static float FRUIT_SPAWN_Z_OFFSET = 32f;
      private string[] gameModeWaveListXMLs = new string[4]
      {
        "xml/originalWaveList.xml",
        "xml/comboWaveList.xml",
        "xml/arcadeWaveList.xml",
        "xml/zenWaveList.xml"
      };
      private static SPAWNER_INFO[] spinfos = ArrayInit.CreateFilledArray<SPAWNER_INFO>(3);
      private static bool made_spawners = false;
      private static bool still_spawning;
      private int[] m_numberOfWavesSpawned = new int[Game.MAX_PLAYERS];
      private List<GlobalProbabilityOveride> m_globalProbabilityOverides = new List<GlobalProbabilityOveride>();
      private float m_absoluteGlobalDtMod;
      private static uint[] stringHashesddd = new uint[5]
      {
        StringFunctions.StringHash("BOTTOM"),
        StringFunctions.StringHash("TOP"),
        StringFunctions.StringHash("LEFT"),
        StringFunctions.StringHash("RIGHT"),
        StringFunctions.StringHash("LEFT_RIGHT")
      };
      private static WaveManager instance = new WaveManager();
      private SpeedControl[] m_speedControl = new SpeedControl[Game.MAX_PLAYERS];
      private float[] m_speedLossTime = new float[Game.MAX_PLAYERS];
      private float[] m_speed = new float[Game.MAX_PLAYERS];
      private float[] m_desiredSpeed = new float[Game.MAX_PLAYERS];
      private int[] m_comboBonus = new int[Game.MAX_PLAYERS];
      private float[] m_nextComboBonus = new float[Game.MAX_PLAYERS];
      private float m_bombScale;
      private float m_bombMultiplyer;
      private float m_fruitMultiplyer;
      private float m_criticalChanceMod;
      private float m_currentGlobalDt;
      private float m_currentGlobalDtMod;
      private float[] m_globalDtInc = new float[4];
      private float[] m_globalDtStart = new float[4];
      private float[] m_globalDtMax = new float[4];
      private List<WAVE_INFO>[,] m_lists = ArrayInit.CreateFilledArray<List<WAVE_INFO>>(Game.MAX_PLAYERS, 4);
      private DEFAULT_WAVE_INFO[] m_defaultInfo = ArrayInit.CreateFilledArray<DEFAULT_WAVE_INFO>(4);
      private COIN_CHANCEINATOR[] m_coinChanceLists = ArrayInit.CreateFilledArray<COIN_CHANCEINATOR>(4);
      private List<PROBABILITY_OVERIDE>[,] m_probabilityOverides = ArrayInit.CreateFilledArray<List<PROBABILITY_OVERIDE>>(Game.MAX_PLAYERS, 4);
      private WAVE_INFO[] m_currentWave = ArrayInit.CreateFilledArray<WAVE_INFO>(Game.MAX_PLAYERS);
      private int[] m_waveCount = new int[Game.MAX_PLAYERS];
      private float[] m_waveDelay = new float[Game.MAX_PLAYERS];
      private float[] m_waveWait = new float[Game.MAX_PLAYERS];
      private bool[] processingWave = new bool[Game.MAX_PLAYERS];
      private byte m_blitzSpawnedThisGame;
      private byte m_blitzForceSpawnedCounter;
      private float m_blitzSpawnTime;
      private int[,] m_fruitTypesToPickFrom = new int[Game.MAX_PLAYERS, 32 /*0x20*/];
      private int[] m_fruitTypesToPickFromWave = new int[Game.MAX_PLAYERS];
      private int[] m_numFruitTypesToPickFrom = new int[Game.MAX_PLAYERS];

      public static float FRUIT_SPAWN_RANGE
      {
        get => WaveManager.FRUIT_SPAWN_MAX_X - WaveManager.FRUIT_SPAWN_MIN_X;
      }

      public static float FRUIT_SPAWN_Y => (float) (-(double) Game.SCREEN_HEIGHT / 2.0);

      public static float SPEED_START => 2.9f;

      public float GetAbsoluteDtMod() => this.m_absoluteGlobalDtMod;

      public void SetAbsoluteDtMod() => this.m_absoluteGlobalDtMod = 1f;

      public void SetAbsoluteDtMod(float mod) => this.m_absoluteGlobalDtMod = mod;

      private bool ParseCoinChanceinator(COIN_CHANCEINATOR coinChanceinater, XElement pParent) => true;

      private SPAWN_PLACEMENTS ParsePlacement(string text)
      {
        uint num = StringFunctions.StringHash(text);
        for (int placement = 0; placement < 5; ++placement)
        {
          if ((int) num == (int) WaveManager.stringHashesddd[placement])
            return (SPAWN_PLACEMENTS) placement;
        }
        return SPAWN_PLACEMENTS.SPAWNER_BOTTOM;
      }

      public static WaveManager GetInstance() => WaveManager.instance;

      public void Init()
      {
        this.m_absoluteGlobalDtMod = 1f;
        Fruit.LoadInfo();
        for (int index1 = 0; index1 < 4; ++index1)
        {
          XDocument element1 = MortarXml.Load(this.gameModeWaveListXMLs[index1]);
          if (element1 != null)
          {
            XElement xelement = element1.FirstChildElement("waveManagerFile").FirstChildElement();
            this.m_defaultInfo[index1].Reset();
            int num1 = 0;
            for (; xelement != null; xelement = xelement.NextSiblingElement())
            {
              switch (xelement.Name.LocalName)
              {
                case "WaveInfo":
                  WAVE_INFO waveInfo1 = new WAVE_INFO(this.m_defaultInfo[index1]);
                  waveInfo1.idx = num1;
                  ++num1;
                  XElement element2 = xelement;
                  int num2 = -1;
                  element2.QueryIntAttribute("waveNo", ref num2);
                  if (num2 >= 0 || index1 == 2)
                    waveInfo1.waveNo = num2;
                  element2.QueryIntAttribute("overideProbabiltyPool", ref waveInfo1.overideProbabilty);
                  string str1 = (string) null;
                  if (element2.Attribute((XName) "until") != null)
                  {
                    string astr = element2.Attribute((XName) "until").Value;
                    if (astr == "forever")
                    {
                      waveInfo1.waveNoRange = -2;
                    }
                    else
                    {
                      num2 = MParser.ParseInt(astr);
                      if (num2 >= 0 && num2 >= waveInfo1.waveNo)
                        waveInfo1.waveNoRange = num2;
                    }
                  }
                  num2 = -1;
                  element2.QueryIntAttribute("chance", ref num2);
                  if (num2 >= 0)
                    waveInfo1.chance = num2;
                  element2.QueryFloatAttribute("criticalChance", ref waveInfo1.criticalChance);
                  element2.QueryFloatAttribute("chanceRegrowth", ref waveInfo1.chanceRegrowth);
                  element2.QueryIntAttribute("games", ref waveInfo1.gamesMin);
                  element2.QueryIntAttribute("gamesMin", ref waveInfo1.gamesMin);
                  element2.QueryIntAttribute("gamesMax", ref waveInfo1.gamesMax);
                  if (waveInfo1.gamesMin < 0)
                    waveInfo1.gamesMin = waveInfo1.gamesMax;
                  if (waveInfo1.gamesMax < 0)
                    waveInfo1.gamesMax = waveInfo1.gamesMin;
                  XElement pParent = element2.FirstChildElement("coin_chances");
                  if (pParent != null)
                  {
                    waveInfo1.coinChanceinator = new COIN_CHANCEINATOR();
                    this.ParseCoinChanceinator(waveInfo1.coinChanceinator, pParent);
                  }
                  XElement element3 = element2.FirstChildElement("Spawn");
                  uint num3 = StringFunctions.StringHash("Spawn");
                  for (; element3 != null; element3 = element3.NextSiblingElement())
                  {
                    if ((int) StringFunctions.StringHash(element3.Name.LocalName) == (int) num3)
                      ++waveInfo1.spawnerCount;
                  }
                  if (waveInfo1.spawnerCount > 0)
                    waveInfo1.spawners = ArrayInit.CreateFilledArray<SPAWNER_INFO>(waveInfo1.spawnerCount);
                  XElement element4 = element2.FirstChildElement("Spawn");
                  str1 = (string) null;
                  int index2 = 0;
                  SPAWNER_INFO spawnerInfo = (SPAWNER_INFO) null;
                  if (waveInfo1.spawners != null)
                    spawnerInfo = waveInfo1.spawners[index2];
                  while (element4 != null)
                  {
                    SPAWNER_INFO spawner = waveInfo1.spawners[index2];
                    if ((int) StringFunctions.StringHash(element4.Name.LocalName) == (int) num3)
                    {
                      string text1 = element4.Attribute((XName) "type").Value;
                      spawner.typeCount = StringFunctions.SplitWords(text1, ref spawner.types);
                      if (spawner.typeCount > 0)
                      {
                        spawner.randomTypes = new int[spawner.typeCount];
                        for (int index3 = 0; index3 < spawner.typeCount; ++index3)
                          spawner.randomTypes[index3] = -1;
                      }
                      element4.QueryFloatAttribute("min", ref spawner.minSpawn);
                      element4.QueryFloatAttribute("max", ref spawner.maxSpawn);
                      element4.QueryFloatAttribute("mininc", ref spawner.maxInc);
                      element4.QueryFloatAttribute("maxinc", ref spawner.maxInc);
                      element4.QueryFloatAttribute("delay", ref spawner.delay);
                      element4.QueryFloatAttribute("delayinc", ref spawner.delayInc);
                      element4.QueryFloatAttribute("horizmin", ref spawner.horizontalMin);
                      element4.QueryFloatAttribute("horizmax", ref spawner.horizontalMax);
                      element4.QueryFloatAttribute("velscale", ref spawner.velXscale);
                      spawner.velYscale = spawner.velXscale;
                      element4.QueryFloatAttribute("velXscale", ref spawner.velXscale);
                      element4.QueryFloatAttribute("velYscale", ref spawner.velYscale);
                      if (element4.Attribute((XName) "gravity") != null)
                      {
                        string text2 = element4.Attribute((XName) "gravity").Value;
                        spawner.gravity = Save.ParseVector(text2);
                      }
                      if (element4.Attribute((XName) "placement") != null)
                      {
                        string text3 = element4.Attribute((XName) "placement").Value;
                        spawner.placement = this.ParsePlacement(text3);
                      }
                      else
                        spawner.placement = SPAWN_PLACEMENTS.SPAWNER_BOTTOM;
                      string str2 = element4.AttributeStr("mirror");
                      spawner.mirror = str2 != null && "true" == str2;
                    }
                    element4 = element4.NextSiblingElement("Spawn");
                    ++index2;
                  }
                  XElement element5 = element2.FirstChildElement("Wave_dt");
                  if (element5 != null)
                  {
                    element5.QueryFloatAttribute("dt", ref waveInfo1.deltaT);
                    element5.QueryFloatAttribute("inc", ref waveInfo1.deltaTinc);
                    element5.QueryFloatAttribute("spinc", ref waveInfo1.deltaSpinc);
                  }
                  XElement element6 = element2.FirstChildElement("NextWaveDelay");
                  if (element6 != null)
                  {
                    element6.QueryFloatAttribute("wait", ref waveInfo1.nextDelay);
                    element6.QueryFloatAttribute("waitSpinc", ref waveInfo1.nextDelaySpInc);
                    element6.QueryFloatAttribute("speedLoss", ref waveInfo1.speedLoss);
                    if ((double) waveInfo1.nextDelay > 0.0)
                    {
                      waveInfo1.beforeDelay = 0.0f;
                      waveInfo1.beforeDelayInc = 0.0f;
                    }
                    element6.QueryFloatAttribute("delay", ref waveInfo1.beforeDelay);
                    element6.QueryFloatAttribute("inc", ref waveInfo1.beforeDelayInc);
                    string str3 = element6.AttributeStr("waitForEntities");
                    waveInfo1.waitForEntities = str3 == null || !("false" == str3);
                  }
                  XElement element7 = element2.FirstChildElement("ChooseFrom");
                  waveInfo1.typesToPickFrom.Clear();
                  waveInfo1.typesToPickFromCount = 0;
                  if (element7 != null)
                    StringFunctions.SplitWords(element7.AttributeStr("types"), ref waveInfo1.typesToPickFrom);
                  this.m_lists[this.m_defaultInfo[index1].players[0], index1].Add(waveInfo1);
                  for (int index4 = 1; index4 < Game.MAX_PLAYERS && this.m_defaultInfo[index1].players[index4] >= 0; index4 = index4 + 1 + 1)
                  {
                    WAVE_INFO waveInfo2 = waveInfo1.Clone();
                    this.m_lists[this.m_defaultInfo[index1].players[index4], index1].Add(waveInfo2);
                  }
                  break;
                case "defaults":
                  this.m_defaultInfo[index1].Reset();
                  xelement.QueryIntAttribute("waveChance", ref this.m_defaultInfo[index1].waveChance);
                  xelement.QueryFloatAttribute("waveChanceRegrowth", ref this.m_defaultInfo[index1].waveChanceRegrowth);
                  xelement.QueryFloatAttribute("criticalChance", ref this.m_defaultInfo[index1].criticalChance);
                  xelement.QueryFloatAttribute("dt", ref this.m_defaultInfo[index1].dt);
                  xelement.QueryFloatAttribute("dtInc", ref this.m_defaultInfo[index1].dtInc);
                  xelement.QueryFloatAttribute("dtSpInc", ref this.m_defaultInfo[index1].dtSpInc);
                  xelement.QueryFloatAttribute("beforeDelay", ref this.m_defaultInfo[index1].beforeDelay);
                  xelement.QueryFloatAttribute("beforeDelayInc", ref this.m_defaultInfo[index1].beforeDelayInc);
                  xelement.QueryFloatAttribute("nextDelay", ref this.m_defaultInfo[index1].nextDelay);
                  xelement.QueryFloatAttribute("nextDelayInc", ref this.m_defaultInfo[index1].nextDelayInc);
                  xelement.QueryFloatAttribute("nextDelaySpInc", ref this.m_defaultInfo[index1].nextDelaySpInc);
                  xelement.QueryFloatAttribute("speedLoss", ref this.m_defaultInfo[index1].speedLoss);
                  xelement.QueryIntAttribute("overideProbabiltyPool", ref this.m_defaultInfo[index1].overideProbabilty);
                  string str4 = xelement.AttributeStr("waitForEntities");
                  this.m_defaultInfo[index1].waitForEntities = str4 == null || !("false" == str4);
                  if (xelement.Attribute((XName) "players") != null && xelement.Attribute((XName) "players").Value == "1,2")
                  {
                    this.m_defaultInfo[index1].players[0] = 1;
                    this.m_defaultInfo[index1].players[1] = 2;
                    this.m_defaultInfo[index1].players[2] = -1;
                  }
                  xelement.QueryFloatAttribute("globalDtInc", ref this.m_globalDtInc[index1]);
                  xelement.QueryFloatAttribute("globalDtStart", ref this.m_globalDtStart[index1]);
                  xelement.QueryFloatAttribute("globalDtMax", ref this.m_globalDtMax[index1]);
                  break;
                case "coin_chances":
                  this.ParseCoinChanceinator(this.m_coinChanceLists[index1], xelement);
                  break;
                case "OverideProbability":
                  PROBABILITY_OVERIDE probabilityOveride = new PROBABILITY_OVERIDE();
                  probabilityOveride.Parse(xelement);
                  this.m_probabilityOverides[this.m_defaultInfo[index1].players[0], index1].Add(probabilityOveride);
                  for (int index5 = 1; index5 < Game.MAX_PLAYERS && this.m_defaultInfo[index1].players[index5] >= 0; ++index5)
                    this.m_probabilityOverides[this.m_defaultInfo[index1].players[index5], index1].Add(probabilityOveride);
                  break;
              }
            }
            for (int index6 = 0; index6 < Game.MAX_PLAYERS; ++index6)
            {
              foreach (WAVE_INFO waveInfo3 in this.m_lists[index6, index1])
              {
                if (waveInfo3.waveNoRange == -1)
                {
                  int num4 = 1000000;
                  waveInfo3.waveNoRange = -2;
                  foreach (WAVE_INFO waveInfo4 in this.m_lists[index6, index1])
                  {
                    if (waveInfo4.waveNo > waveInfo3.waveNo && waveInfo4.waveNo - 1 < num4)
                      num4 = waveInfo4.waveNo - 1;
                  }
                  if (num4 < 1000000)
                    waveInfo3.waveNoRange = num4;
                }
              }
            }
          }
        }
        this.ParseGlobalProbabilityOverides("xml/globalprobabilities.xml");
        for (int index = 0; index < Game.MAX_PLAYERS; ++index)
          this.m_speedControl[index] = (SpeedControl) null;
      }

      public WAVE_INFO GetCurrentWave() => this.GetCurrentWave(0);

      public WAVE_INFO GetCurrentWave(int player) => this.m_currentWave[player];

      public int GetNumberOfWavesSpawned() => this.GetNumberOfWavesSpawned(0);

      public int GetNumberOfWavesSpawned(int player) => this.m_numberOfWavesSpawned[player];

      private void ParseGlobalProbabilityOverides(string file)
      {
        XDocument element1 = MortarXml.Load(file);
        if (element1 == null)
          return;
        string ename = "probabilityFile" + (object) Game.game_work.saveData.GetTotal(StringFunctions.StringHash("super_fruit_probability_system"));
        XElement element2 = element1.FirstChildElement("probabilityFile").FirstChildElement(ename);
        if (element2 == null)
          return;
        for (XElement element3 = element2.FirstChildElement("globalProbability"); element3 != null; element3 = element3.NextSiblingElement("globalProbability"))
        {
          GlobalProbabilityOveride probabilityOveride = new GlobalProbabilityOveride();
          probabilityOveride.Parse(element3);
          this.m_globalProbabilityOverides.Add(probabilityOveride);
        }
        for (XElement element4 = element2.FirstChildElement("globalProbabilityPointBased"); element4 != null; element4 = element4.NextSiblingElement("globalProbabilityPointBased"))
        {
          GlobalProbabilityOveridePointBased overidePointBased = new GlobalProbabilityOveridePointBased();
          overidePointBased.Parse(element4);
          this.m_globalProbabilityOverides.Add((GlobalProbabilityOveride) overidePointBased);
        }
        for (XElement element5 = element2.FirstChildElement("globalProbabilityTimed"); element5 != null; element5 = element5.NextSiblingElement("globalProbabilityTimed"))
        {
          GlobalProbabilityOverideTimed probabilityOverideTimed = new GlobalProbabilityOverideTimed();
          probabilityOverideTimed.Parse(element5);
          this.m_globalProbabilityOverides.Add((GlobalProbabilityOveride) probabilityOverideTimed);
        }
      }

      private GlobalProbabilityOveride CheckForGlobalProbabilityOveride(ref int type)
      {
        for (int index = 0; index < this.m_globalProbabilityOverides.Count; ++index)
        {
          if (this.m_globalProbabilityOverides[index].CheckForOverride(ref type))
            return this.m_globalProbabilityOverides[index];
        }
        return (GlobalProbabilityOveride) null;
      }

      public void Reset() => this.Reset(false);

      public void Reset(bool newGame)
      {
        Game.game_work.gameOver = false;
        this.m_blitzSpawnedThisGame = (byte) 0;
        this.m_blitzForceSpawnedCounter = (byte) 0;
        this.m_blitzSpawnTime = 10f + Math.g_random.RandF(10f);
        Game.game_work.currentMissCount = (byte) 0;
        Game.game_work.currentScore = 0;
        Game.game_work.hasDroppedFruit = false;
        Fruit.s_consecutiveCount = 0;
        for (int index = 0; index < Game.MAX_PLAYERS; ++index)
        {
          this.m_waveCount[index] = -1;
          this.m_waveWait[index] = 0.0f;
          this.m_speed[index] = 0.0f;
          this.m_desiredSpeed[index] = 0.0f;
          this.m_speedLossTime[index] = 0.0f;
          this.m_nextComboBonus[index] = 0.0f;
          this.m_comboBonus[index] = 0;
          this.m_numberOfWavesSpawned[index] = 0;
        }
        float foVy = Game.game_work.camera.GetFOVy();
        Vector3 zero = Vector3.Zero;
        float num1 = Math.TanIdx(Math.DEGREE_TO_IDX(foVy));
        zero.Z = Game.SCREEN_HEIGHT / 2f / num1;
        Game.game_work.camera.SetLookAt(Vector3.Zero);
        Game.game_work.camera.SetPos(zero);
        Game.game_work.camera.SetUp(new Vector3(0.0f, 1f, 0.0f));
        if (Game.game_work.hud != null)
          Game.game_work.hud.ResetControls();
        uint num2 = 0;
        Fruit.ClearUnspawned();
        Bomb.ClearUnspawned();
        ActorManager instance1 = ActorManager.GetInstance();
        int idx1 = (int) num2;
        uint num3 = (uint) (idx1 + 1);
        for (Entity entity = instance1.GetEntity(EntityTypes.ENTITY_BEGIN, (uint) idx1); entity != null; entity = ActorManager.GetInstance().GetEntity(EntityTypes.ENTITY_BEGIN, num3++))
          ((Fruit) entity).Disable();
        uint num4 = 0;
        ActorManager instance2 = ActorManager.GetInstance();
        int idx2 = (int) num4;
        uint num5 = (uint) (idx2 + 1);
        for (Entity entity = instance2.GetEntity(EntityTypes.ENTITY_BOMB, (uint) idx2); entity != null; entity = ActorManager.GetInstance().GetEntity(EntityTypes.ENTITY_BOMB, num5++))
          ((Bomb) entity).Disable();
        this.ResetWaveChances();
        for (int player = 0; player < Game.MAX_PLAYERS; ++player)
        {
          foreach (PROBABILITY_OVERIDE probabilityOveride in this.m_probabilityOverides[player, (int) Game.game_work.gameMode])
            probabilityOveride.SelectType();
          this.m_fruitTypesToPickFromWave[player] = 0;
          this.m_numFruitTypesToPickFrom[player] = 1;
          for (int index = 0; index < 32 /*0x20*/; ++index)
            this.m_fruitTypesToPickFrom[player, index] = -1;
          if (this.m_lists[player, (int) Game.game_work.gameMode].Count > 0)
          {
            this.GetNextWave(player);
            if (Game.IsMultiplayer())
              ++this.m_waveDelay[player];
          }
        }
        this.m_currentGlobalDtMod = 1f;
        this.m_currentGlobalDt = this.m_globalDtStart[(int) Game.game_work.gameMode];
        for (int index = 0; index < this.m_globalProbabilityOverides.Count; ++index)
          this.m_globalProbabilityOverides[index].NewGameStarted();
        if (!newGame)
          return;
        this.NewGame();
      }

      public void Resume()
      {
        Game.game_work.currentScore = (int) (ushort) Game.game_work.saveData.score;
        Game.game_work.currentMissCount = (byte) Game.game_work.saveData.misses;
        Fruit.s_consecutiveType = Game.game_work.saveData.consecutiveType;
        Fruit.s_consecutiveCount = Game.game_work.saveData.consecutiveCount;
        Game.game_work.hasDroppedFruit = Game.game_work.saveData.hasDropped;
        bool flag = false;
        for (int index1 = 0; index1 < Game.MAX_PLAYERS; ++index1)
        {
          foreach (PROBABILITY_OVERIDE probabilityOveride in this.m_probabilityOverides[index1, (int) Game.game_work.gameMode])
            probabilityOveride.SelectType();
          this.m_fruitTypesToPickFromWave[index1] = 0;
          this.m_numFruitTypesToPickFrom[index1] = 1;
          for (int index2 = 0; index2 < 32 /*0x20*/; ++index2)
            this.m_fruitTypesToPickFrom[index1, index2] = -1;
        }
        if (Game.game_work.saveData.entities.Count > 0)
        {
          foreach (EntityState entity1 in Game.game_work.saveData.entities)
          {
            EntityState entityState = entity1;
            Entity entity2 = ActorManager.GetInstance().Add(entityState.type < Fruit.MAX_FRUIT_TYPES ? (entityState.type >= 0 ? EntityTypes.ENTITY_BEGIN : EntityTypes.ENTITY_BOMB_BLAST) : EntityTypes.ENTITY_BOMB);
            Vector3 one = Vector3.One;
            entity2.Init((byte[]) null, entityState.type, new Vector3?(one));
            entity2.m_pos = entityState.pos;
            entity2.m_vel = entityState.vel;
            if (entity2.m_type == (byte) 1)
            {
              ((Bomb) entity2).m_gravity = entityState.grav;
              if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
                ((Bomb) entity2).SetForPlayer(Game.MAX_PLAYERS * 2 - 1);
            }
            else if (entity2.m_type == (byte) 0)
            {
              ((Fruit) entity2).m_gravity = entityState.grav;
              if (entityState.superState != null)
              {
                SuperFruitControl control = new SuperFruitControl((Fruit) entity2, entityState.superState);
                Game.game_work.hud.AddControl((HUDControl) control);
                entityState.superState = (SuperFruitState) null;
              }
            }
            if ((double) entityState.wait > 0.0)
            {
              if (entity2.m_type == (byte) 0)
                ((Fruit) entity2).Chuck(entityState.wait);
              else if (entity2.m_type == (byte) 1)
              {
                if (entityState.hit)
                  ((Bomb) entity2).SetHit(entityState.wait);
                else
                  ((Bomb) entity2).Chuck(entityState.wait);
              }
              else if (entity2.m_type == (byte) 4)
                entity2.Update(entity1.wait);
            }
          }
          flag = true;
        }
        ActorManager.GetInstance().Update(0.0f);
        int misses = Game.game_work.saveData.misses;
        if ((double) Game.game_work.saveData.go_bombHitTime > 0.0 && Game.game_work.gameMode != Game.GAME_MODE.GM_ARCADE || Game.game_work.saveData.go_state > -1)
        {
          int winner = -1;
          GameTask.SkipToGameOver(Game.game_work.saveData.go_state, Game.game_work.saveData.go_time, Game.game_work.saveData.go_transition, Game.game_work.saveData.go_bombHitTime, winner);
        }
        else if ((flag || Game.game_work.saveData.waves.Count > 0) && misses < 3)
        {
          GameTask.SkipToPause();
          int index3 = 0;
          this.m_numFruitTypesToPickFrom[index3] = Game.game_work.saveData.numFruitTypesToPickFrom;
          for (int index4 = 0; index4 < 32 /*0x20*/; ++index4)
            this.m_fruitTypesToPickFrom[index3, index4] = Game.game_work.saveData.fruitTypesToPickFrom[index4];
          this.processingWave[index3] = true;
          this.m_waveCount[index3] = Game.game_work.saveData.currentWave;
          this.m_numberOfWavesSpawned[index3] = Game.game_work.saveData.numberOfWavesSpawned;
          this.m_waveDelay[index3] = Game.game_work.saveData.currentWaveDelay;
          this.m_waveWait[index3] = Game.game_work.saveData.currentWaveWait;
          this.m_currentGlobalDt = Game.game_work.saveData.globalWaveDt;
          this.m_blitzSpawnedThisGame = (byte) Game.game_work.saveData.m_blitzSpawnedThisGame;
          this.m_blitzForceSpawnedCounter = (byte) Game.game_work.saveData.m_blitzForceSpawnedCounter;
          this.m_blitzSpawnTime = Game.game_work.saveData.m_blitzSpawnTime;
          this.m_speedLossTime[index3] = Game.game_work.saveData.speedLossTime;
          this.m_nextComboBonus[index3] = Game.game_work.saveData.nextComboBonus;
          this.m_speed[index3] = this.m_desiredSpeed[index3] = Game.game_work.saveData.desiredSpeed;
          foreach (WaveState wave in Game.game_work.saveData.waves)
          {
            WAVE_INFO waveInfo = this.m_lists[index3, (int) Game.game_work.gameMode][wave.index];
            waveInfo.m_inc = wave.inc;
            if (wave.spawners.Count<SpawnState>() > 0)
            {
              this.m_currentWave[index3] = waveInfo;
              int index5 = 0;
              foreach (SpawnState spawner in wave.spawners)
              {
                this.m_currentWave[index3].spawners[index5].toSpawnThisWave = spawner.toSpawnThisWave;
                this.m_currentWave[index3].spawners[index5].maxToSpawnThisWave = spawner.toSpawnThisWave;
                this.m_currentWave[index3].spawners[index5].bombsSpawnedThisWave = 0;
                this.m_currentWave[index3].spawners[index5].delayWait = spawner.delayWait;
                this.m_currentWave[index3].spawners[index5].SelectTypes();
                ++index5;
              }
            }
          }
          this.ResetWaveChances();
        }
        Game.game_work.camera.m_cameraShakeMaxTime = Game.game_work.saveData.shake_max_time;
        Game.game_work.camera.m_cameraShakeTime = Game.game_work.saveData.shake_time;
        Game.game_work.saveData.entities.Clear();
      }

      private void DeleteSpeedControl(HUDControl control)
      {
        for (int index = 0; index < Game.MAX_PLAYERS; ++index)
        {
          if (this.m_speedControl[index] == control)
            this.m_speedControl[index] = (SpeedControl) null;
        }
      }

      private SPAWNER_INFO GetRandomPowerSpawner() => this.GetRandomPowerSpawner(true);

      public Math.Random GetRandomiser() => this.m_random;

      private SPAWNER_INFO GetRandomPowerSpawner(bool allowTop)
      {
        if (!WaveManager.made_spawners)
        {
          WaveManager.spinfos[0].placement = SPAWN_PLACEMENTS.SPAWNER_TOP;
          WaveManager.spinfos[0].gravity = new Vector3(0.0f, -1.1f, 0.0f);
          WaveManager.spinfos[0].horizontalMin = -0.5f;
          WaveManager.spinfos[0].horizontalMax = 0.5f;
          WaveManager.spinfos[0].velYscale = 0.0f;
          WaveManager.spinfos[0].velXscale = 0.666f;
          WaveManager.spinfos[0].delayWait = -3f;
          WaveManager.spinfos[0].dt = 0.75f;
          WaveManager.spinfos[1].placement = SPAWN_PLACEMENTS.SPAWNER_RIGHT;
          WaveManager.spinfos[1].horizontalMin = -1f;
          WaveManager.spinfos[1].horizontalMax = -0.5f;
          WaveManager.spinfos[1].velYscale = 0.75f;
          WaveManager.spinfos[1].delayWait = -3f;
          WaveManager.spinfos[2].placement = SPAWN_PLACEMENTS.SPAWNER_LEFT;
          WaveManager.spinfos[2].horizontalMin = -1f;
          WaveManager.spinfos[2].horizontalMax = -0.5f;
          WaveManager.spinfos[2].velYscale = 0.75f;
          WaveManager.spinfos[2].delayWait = -3f;
          WaveManager.made_spawners = true;
        }
        return WaveManager.spinfos[WaveManager.GetInstance().GetRandomiser().Rand32(allowTop ? 3 : 2) + (allowTop ? 0 : 1)];
      }

      public void Update(float dt)
      {
        WaveManager.still_spawning = false;
        if (this.paused)
          return;
        this.m_bombScale = 1f;
        this.m_bombMultiplyer = 1f;
        this.m_fruitMultiplyer = 1f;
        this.m_criticalChanceMod = 1f;
        if ((double) Game.game_work.gameOverTransition < 1.0 && Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
        {
          PowerUpManager.GetInstance().Update(dt);
          double timedPowerProgress = (double) PowerUpManager.GetInstance().GetCurrentTimedPowerProgress();
          this.m_currentGlobalDtMod = PowerUpManager.GetInstance().GetDtMod();
        }
        else
        {
          PowerUpManager.GetInstance().SetDefaults();
          this.m_currentGlobalDtMod = 1f;
        }
        if (Game.game_work.gameOver && this.m_waveCount[0] > 0)
        {
          this.UpdateComboSpeed(dt);
        }
        else
        {
          this.m_currentGlobalDt = Math.CLAMP(this.m_currentGlobalDt + this.m_globalDtInc[(int) Game.game_work.gameMode] * dt, this.m_globalDtStart[(int) Game.game_work.gameMode], this.m_globalDtMax[(int) Game.game_work.gameMode]);
          for (int player = 0; player < Game.MAX_PLAYERS; ++player)
          {
            this.UpdateComboSpeed(dt);
            WaveManager.still_spawning = false;
            if (this.m_lists[player, (int) Game.game_work.gameMode].Count<WAVE_INFO>() > 0)
            {
              if (this.m_currentWave[player] != null)
              {
                if ((double) this.m_waveDelay[player] <= 0.0)
                {
                  this.m_waveDelay[player] = 0.0f;
                  for (int index1 = 0; index1 < this.m_currentWave[player].spawnerCount; ++index1)
                  {
                    this.m_currentWave[player].spawners[index1].delayWait -= dt * ((double) this.m_currentGlobalDtMod > 1.0 ? this.m_currentGlobalDtMod : 1f);
                    int toSpawnThisWave = this.m_currentWave[player].spawners[index1].toSpawnThisWave;
                    int num1 = 0;
                    List<PROBABILITY_OVERIDE> probabilityOveride1 = this.m_probabilityOverides[player, (int) Game.game_work.gameMode];
                    while (this.m_currentWave[player].spawners[index1].toSpawnThisWave > 0)
                    {
                      SPAWNER_INFO spawner = this.m_currentWave[player].spawners[index1];
                      if (this.m_currentWave[player].spawners[index1].typeCount > 0)
                      {
                        int index2 = Math.g_random.Rand32(this.m_currentWave[player].spawners[index1].typeCount);
                        int type1 = this.m_currentWave[player].spawners[index1].randomTypes[index2];
                        if (this.m_currentWave[player].spawners[index1].typeCount > 1 && num1 >= toSpawnThisWave / 2)
                        {
                          for (; type1 == -2; type1 = this.m_currentWave[player].spawners[index1].randomTypes[index2])
                            index2 = Math.g_random.Rand32(this.m_currentWave[player].spawners[index1].typeCount);
                        }
                        bool flag1 = false;
                        if (type1 == -1 || this.m_currentWave[player].spawners[index1].types[index2] == "1fruit")
                        {
                          bool flag2 = false;
                          if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE && (double) Game.game_work.timeControl.GetTime() <= (double) Game.game_work.timeControl.GetCountDown() - (double) this.m_blitzSpawnTime)
                          {
                            if (this.m_blitzForceSpawnedCounter == (byte) 0)
                            {
                              ++this.m_blitzForceSpawnedCounter;
                              if (this.m_blitzSpawnedThisGame == (byte) 0)
                                flag2 = true;
                              this.m_blitzSpawnTime = 35f + Math.g_random.RandF(10f);
                            }
                            else if (this.m_blitzForceSpawnedCounter == (byte) 1 && this.m_blitzSpawnedThisGame == (byte) 1)
                            {
                              ++this.m_blitzForceSpawnedCounter;
                              flag2 = true;
                            }
                          }
                          int num2 = Math.g_random.Rand32(this.m_currentWave[player].overideProbabilty);
                          int num3 = 0;
                          int index3 = 0;
                          int blitzSpawnedThisGame1 = (int) this.m_blitzSpawnedThisGame;
                          for (; index3 < probabilityOveride1.Count; ++index3)
                          {
                            bool flag3 = true;
                            if (probabilityOveride1[index3].percentageChance <= 0 || !flag2)
                            {
                              if (probabilityOveride1[index3].spawnedThisWave >= probabilityOveride1[index3].perWave && probabilityOveride1[index3].perWave > 0)
                                flag3 = false;
                              float activeProgression = PowerUpManager.GetInstance().GetActiveProgression(Math.MAX(0.0f, spawner.delayWait) + 0.21f);
                              flag3 = flag3 && (double) probabilityOveride1[index3].canSpawnWithPowerup < (double) activeProgression;
                              if (this.m_waveCount[player] >= 0 && this.m_waveCount[player] < probabilityOveride1[index3].waveCount)
                                flag3 = false;
                            }
                            if (flag3)
                            {
                              int percentageChance = probabilityOveride1[index3].percentageChance;
                              if (this.m_blitzSpawnedThisGame >= (byte) 6)
                                percentageChance >>= 1;
                              num3 += percentageChance;
                              if (num2 < num3 || probabilityOveride1[index3].percentageChance > 0 && flag2)
                              {
                                int count = probabilityOveride1[index3].powerAllowanceChances.Count;
                                if (count > 0)
                                {
                                  int index4 = Math.MIN(count - 1, PowerUpManager.GetInstance().GetNumActiveTimedPowers());
                                  if (this.m_random.Rand32(100) >= probabilityOveride1[index3].powerAllowanceChances[index4])
                                    break;
                                }
                                int type2 = probabilityOveride1[index3].GetType();
                                FRUIT_POWERS powers = Math.BETWEEN(type2, 0, Fruit.MAX_FRUIT_TYPES) ? Fruit.FruitInfo(type2).powers : (FRUIT_POWERS) null;
                                for (int index5 = 30; index5 > 0 && powers != null && powers.AnyActivePowers(); --index5)
                                {
                                  type2 = probabilityOveride1[index3].GetType();
                                  powers = Math.BETWEEN(type2, 0, Fruit.MAX_FRUIT_TYPES) ? Fruit.FruitInfo(type2).powers : (FRUIT_POWERS) null;
                                }
                                if (Math.BETWEEN(type2, 0, Fruit.MAX_FRUIT_TYPES) && Fruit.FruitInfo(type2).superFruit)
                                  spawner = this.GetRandomPowerSpawner(false);
                                if (powers != null)
                                {
                                  if (!powers.AnyActivePowers())
                                  {
                                    spawner = this.GetRandomPowerSpawner();
                                    if ((double) this.m_fruitMultiplyer > 0.0 && Fruit.NumberOfPowerupFruits() <= 0 && ((double) Game.game_work.saveData.timer >= 8.0 || (int) powers.powerUps[0].powerHash == (int) StringFunctions.StringHash("freeze")) && !powers.AnyActivePowers())
                                    {
                                      int num4 = flag2 ? 1 : 0;
                                      ++this.m_blitzSpawnedThisGame;
                                    }
                                    else
                                    {
                                      int num5 = flag2 ? 1 : 0;
                                    }
                                  }
                                  else
                                    break;
                                }
                                ++probabilityOveride1[index3].spawnedThisWave;
                                type1 = type2;
                                flag1 = true;
                                if (this.m_blitzSpawnedThisGame >= (byte) 6)
                                  ;
                                int blitzSpawnedThisGame2 = (int) this.m_blitzSpawnedThisGame;
                                break;
                              }
                            }
                          }
                        }
                        if (type1 == -2)
                        {
                          ++num1;
                          if ((double) this.m_bombMultiplyer > 0.0)
                            this.SpawnBomb(1, spawner, player);
                        }
                        else if ((double) this.m_fruitMultiplyer > 0.0)
                        {
                          GlobalProbabilityOveride probabilityOveride2 = (GlobalProbabilityOveride) null;
                          if (!flag1)
                            probabilityOveride2 = this.CheckForGlobalProbabilityOveride(ref type1);
                          Fruit fruit = this.SpawnFruit(1, type1, spawner, player);
                          if (fruit != null && probabilityOveride2 != null)
                          {
                            fruit.m_fruitKilled += new Fruit.FruitEvent(probabilityOveride2.FruitWasKilled);
                            fruit.m_fruitThrown += new Fruit.FruitEvent(probabilityOveride2.FruitWasThrown);
                          }
                        }
                        this.processingWave[player] = true;
                        --this.m_currentWave[player].spawners[index1].toSpawnThisWave;
                      }
                      else
                      {
                        this.m_currentWave[player].spawners[index1].delayWait = 0.0f;
                        this.m_currentWave[player].spawners[index1].toSpawnThisWave = 0;
                      }
                      this.m_currentWave[player].spawners[index1].delayWait += Math.MAX(0.0f, this.m_currentWave[player].spawners[index1].delay + this.m_currentWave[player].spawners[index1].delayInc * this.m_currentWave[player].m_inc);
                    }
                    if (this.m_currentWave[player].spawners[index1].toSpawnThisWave > 0)
                      WaveManager.still_spawning = true;
                  }
                }
                else
                {
                  this.m_waveDelay[player] -= dt;
                  WaveManager.still_spawning = true;
                }
              }
              if (!this.IsWaveProcessing(player) && !WaveManager.still_spawning)
              {
                bool flag = true;
                if (this.m_currentWave[player] != null && (double) this.m_waveWait[player] > 0.0)
                {
                  this.m_waveWait[player] -= dt;
                  if ((double) this.m_waveWait[player] > 0.0)
                    flag = false;
                }
                if (flag)
                  this.GetNextWave(player);
              }
            }
          }
        }
      }

      public void Draw() => this.Draw(0);

      public void Draw(int player)
      {
        if (player != 0)
          return;
        PowerUpManager.GetInstance().Draw();
      }

      public void Destroy()
      {
        for (int index1 = 0; index1 < Game.MAX_PLAYERS; ++index1)
        {
          for (int index2 = 0; index2 < 4; ++index2)
            this.m_lists[index1, index2].Clear();
        }
      }

      public void ClearUnspawned()
      {
        Fruit.ClearUnspawned();
        Bomb.ClearUnspawned();
      }

      private void UpdateComboSpeed(float dt)
      {
        int player = 0;
        float num = this.m_desiredSpeed[player];
        if ((double) num < (double) WaveManager.SPEED_START)
          num = 0.0f;
        this.m_speed[player] += (double) num > (double) this.m_speed[player] ? Math.MIN(num - this.m_speed[player], dt * 5f) : ((double) num < (double) this.m_speed[player] ? Math.MAX(num - this.m_speed[player], (float) (-(double) dt * 5.0)) : 0.0f);
        if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
        {
          if (this.m_speedControl[player] != null)
          {
            this.m_speedControl[player].m_pulseSpeed = this.m_speed[player];
            this.m_speedControl[player].m_lossTime = this.m_speedLossTime[player];
          }
          else
          {
            this.m_speedControl[player] = new SpeedControl();
            this.m_speedControl[player].m_deleteCall = new HUDControl.HUDControlDeletedCallback(this.DeleteSpeedControl);
            Game.game_work.hud.AddControl((HUDControl) this.m_speedControl[player]);
            this.m_speedControl[player].m_pulseSpeed = this.m_speed[player];
            this.m_speedControl[player].m_lossTime = this.m_speedLossTime[player];
          }
        }
        if ((double) this.m_speedLossTime[player] <= 0.0 || this.m_currentWave[player] == null || (double) this.m_currentWave[player].speedLoss <= 0.0)
          return;
        this.m_speedLossTime[player] -= Math.MIN(1f, this.GetWavedt(player)) * dt / this.m_currentWave[player].speedLoss;
        if ((double) this.m_speedLossTime[player] > 0.0)
          return;
        this.ResetSpeed(player);
      }

      public bool IsWaveProcessing() => this.IsWaveProcessing(0);

      public bool IsWaveProcessing(int player)
      {
        if (this.processingWave[player])
        {
          if (player == 0)
          {
            if (this.m_currentWave[player] != null && !this.m_currentWave[player].waitForEntities)
            {
              if (Fruit.GetNumActiveForPlayer(-1, false) > 0 || Bomb.GetNumActiveForPlayer(-1, false) > 0)
                return true;
            }
            else if (ActorManager.GetInstance().GetNumEntities(0) > 0U || Game.IsMultiplayer() && Bomb.GetNumActiveForPlayer(-1) > 0 || !Game.IsMultiplayer() && ActorManager.GetInstance().GetNumEntities(1) > 0U)
              return true;
          }
          else if (Fruit.GetNumActiveForPlayer(player) > 0 || Bomb.GetNumActiveForPlayer(player) > 0)
            return true;
          this.processingWave[player] = false;
        }
        return this.processingWave[player];
      }

      public float GetWavedt() => this.GetWavedt(0);

      public float GetWavedt(int player)
      {
        float num = 1f;
        if (this.m_currentWave[player] != null)
          num = (float) ((double) this.m_currentWave[player].deltaT + (double) this.m_currentWave[player].m_inc * (double) this.m_currentWave[player].deltaTinc + (double) this.m_currentWave[player].deltaSpinc * (double) this.m_speed[player]);
        return Math.CLAMP(num * (player != 0 ? 1f : this.m_currentGlobalDt * this.m_currentGlobalDtMod), 0.0f, WaveManager.MAX_DELTA_TIME_SCALE);
      }

      public bool SaveWaveInfo(FruitSaveData saveData)
      {
        int index1 = 0;
        saveData.speedLossTime = 0.0f;
        saveData.nextComboBonus = 0.0f;
        saveData.desiredSpeed = 0.0f;
        saveData.m_blitzSpawnedThisGame = (int) this.m_blitzSpawnedThisGame;
        saveData.m_blitzForceSpawnedCounter = (int) this.m_blitzForceSpawnedCounter;
        saveData.m_blitzSpawnTime = this.m_blitzSpawnTime;
        saveData.waves.Clear();
        if (Game.game_work.gameOver && this.m_waveCount[index1] >= 0 || this.m_lists[index1, (int) Game.game_work.gameMode].Count <= 0)
          return false;
        saveData.globalWaveDt = this.m_currentGlobalDt;
        WAVE_INFO[] filledArray = ArrayInit.CreateFilledArray<WAVE_INFO>(20);
        int[] numArray = new int[20];
        int num1 = 0;
        int index2 = 0;
        int num2 = 0;
        foreach (WAVE_INFO waveInfo in this.m_lists[index1, (int) Game.game_work.gameMode])
        {
          if (this.m_waveCount[index1] >= waveInfo.waveNo && (this.m_waveCount[index1] <= waveInfo.waveNoRange || waveInfo.waveNoRange == -2))
          {
            filledArray[index2] = waveInfo;
            numArray[index2] = num1;
            num2 += waveInfo.chance;
            ++index2;
          }
          ++num1;
        }
        for (int index3 = 0; index3 < index2; ++index3)
        {
          WaveState waveState;
          waveState.inc = filledArray[index3].m_inc;
          waveState.index = numArray[index3];
          waveState.spawners = new LinkedList<SpawnState>();
          waveState.spawners.Clear();
          if (filledArray[index3] == this.m_currentWave[index1])
          {
            for (int index4 = 0; index4 < filledArray[index3].spawnerCount; ++index4)
            {
              SpawnState spawnState;
              spawnState.toSpawnThisWave = filledArray[index3].spawners[index4].toSpawnThisWave;
              spawnState.delayWait = filledArray[index3].spawners[index4].delayWait;
              waveState.spawners.AddLast(spawnState);
            }
          }
          saveData.waves.AddLast(waveState);
        }
        saveData.currentWave = this.m_waveCount[index1];
        saveData.currentWaveDelay = this.m_waveDelay[index1];
        saveData.currentWaveWait = this.m_waveWait[index1];
        saveData.speedLossTime = this.m_speedLossTime[index1];
        saveData.nextComboBonus = this.m_nextComboBonus[index1];
        saveData.desiredSpeed = this.m_desiredSpeed[index1];
        saveData.numberOfWavesSpawned = this.m_numberOfWavesSpawned[index1];
        saveData.numFruitTypesToPickFrom = this.m_numFruitTypesToPickFrom[index1];
        for (int index5 = 0; index5 < 32 /*0x20*/; ++index5)
          saveData.fruitTypesToPickFrom[index5] = this.m_fruitTypesToPickFrom[index1, index5];
        return true;
      }

      public float GetCriticalChance() => this.GetCriticalChance(0);

      public bool CriticalMode() => this.CriticalMode(0);

      public bool CriticalMode(int player)
      {
        return (double) this.GetCriticalChance(player) > (double) (Fruit.CRITICAL_CHANCE / 2);
      }

      public int RequestCoins()
      {
        int index = 0;
        int num = 0;
        if (this.m_currentWave != null && this.m_currentWave[index].coinChanceinator != null)
          num = this.m_currentWave[index].coinChanceinator.GetCoins();
        if (num <= 0)
          num = this.m_coinChanceLists[(int) Game.game_work.gameMode].GetCoins();
        return num;
      }

      public Fruit SpawnFruit(int fruitCount) => this.SpawnFruit(fruitCount, -1);

      public Fruit SpawnFruit(int fruitCount, int type)
      {
        return this.SpawnFruit(fruitCount, type, (SPAWNER_INFO) null);
      }

      public Fruit SpawnFruit(int fruitCount, int type, SPAWNER_INFO spawner)
      {
        return this.SpawnFruit(fruitCount, type, spawner, 0);
      }

      public Fruit SpawnFruit(int fruitCount, int type, SPAWNER_INFO spawner, int player)
      {
        for (int index = 0; index < fruitCount; ++index)
        {
          float num1 = spawner != null ? spawner.horizontalMin : -1f;
          float num2 = spawner != null ? spawner.horizontalMax : 1f;
          int num3 = (int) ((double) WaveManager.FRUIT_SPAWN_MAX_X * (double) num1 + (double) Math.g_random.Rand32((uint) ((double) num2 * (double) WaveManager.FRUIT_SPAWN_MAX_X - (double) num1 * (double) WaveManager.FRUIT_SPAWN_MAX_X)));
          int num4 = (int) WaveManager.FRUIT_SPAWN_Y;
          float fruitAngleRange = WaveManager.FRUIT_ANGLE_RANGE;
          if (spawner != null && spawner.placement >= SPAWN_PLACEMENTS.SPAWNER_LEFT)
            fruitAngleRange *= 0.6f;
          int num5 = (int) (-((double) num3 / ((double) WaveManager.FRUIT_SPAWN_RANGE / 2.0)) * ((double) fruitAngleRange / 2.0));
          float num6 = Math.g_random.RandF(1f) - 0.5f;
          float num7 = (float) (0.5 - (0.5 - (double) Math.ABS(num6)) * (0.5 - (double) Math.ABS(num6)) * 2.0) * (float) Math.MATH_SIGN(num6);
          ushort idx = Math.DEGREE_TO_IDX((float) (int) ((double) num5 + (double) fruitAngleRange * (double) num7));
          float num8 = WaveManager.FRUIT_MIN_VEL + Math.g_random.RandF(WaveManager.FRUIT_MAX_VEL - WaveManager.FRUIT_MIN_VEL);
          float num9 = (float) ((double) Math.SinIdx(idx) * (double) num8 * (spawner != null ? (double) spawner.velXscale : 1.0));
          float num10 = (float) ((double) Math.CosIdx(idx) * (double) num8 * 1.0750000476837158 * (spawner != null ? (double) spawner.velYscale : 1.0));
          int tpl_size = type;
          float num11 = 1f;
          if (spawner != null)
            num11 = spawner.dt;
          if (tpl_size == -1 && this.m_numFruitTypesToPickFrom[player] > 0)
            tpl_size = this.m_fruitTypesToPickFrom[player, Math.g_random.Rand32(this.m_numFruitTypesToPickFrom[player])];
          Vector3 vector3_1 = Vector3.One;
          Vector3 vector3_2 = Vector3.UnitY * -1f;
          SPAWN_PLACEMENTS spawnPlacements = spawner != null ? spawner.placement : SPAWN_PLACEMENTS.SPAWNER_BOTTOM;
          switch (spawnPlacements)
          {
            case SPAWN_PLACEMENTS.SPAWNER_TOP:
              vector3_1 = Vector3.One * -1f;
              num10 /= 2f;
              vector3_2 = Vector3.UnitY;
              break;
            case SPAWN_PLACEMENTS.SPAWNER_LEFT:
              float num12 = (float) num3;
              num3 = -num4 * (int) Game.SCREEN_WIDTH / (int) Game.SCREEN_HEIGHT;
              num4 = (int) ((double) num12 * (double) Game.SCREEN_HEIGHT / (double) Game.SCREEN_WIDTH);
              float num13 = num9;
              num9 = -0.75f * num10;
              num10 = num13 - (float) ((double) num8 * (double) spawner.gravity.Y * 0.64999997615814209);
              if (spawnPlacements == SPAWN_PLACEMENTS.SPAWNER_LEFT)
              {
                vector3_1 = new Vector3(-1f, 1f, 1f);
                vector3_2 = Vector3.UnitX * -1f;
                break;
              }
              break;
            case SPAWN_PLACEMENTS.SPAWNER_RIGHT:
              vector3_1 = new Vector3(1f, 1f, 1f);
              vector3_2 = Vector3.UnitX;
              goto case SPAWN_PLACEMENTS.SPAWNER_LEFT;
            case SPAWN_PLACEMENTS.SPAWNER_LEFT_RIGHT:
              spawnPlacements = Math.g_random.Rand32(2) != 0 ? SPAWN_PLACEMENTS.SPAWNER_RIGHT : SPAWN_PLACEMENTS.SPAWNER_LEFT;
              goto case SPAWN_PLACEMENTS.SPAWNER_RIGHT;
          }
          int x1 = num3 * (int) vector3_1.X;
          int y1 = num4 * (int) vector3_1.Y;
          float x2 = num9 * vector3_1.X;
          float y2 = num10 * vector3_1.Y;
          float v2 = spawner != null ? spawner.delayWait : 0.0f;
          if (Game.IsMultiplayer())
          {
            float num14 = 0.975f * Game.SPLIT_SCREEN_SCALE;
            float x3 = vector3_2.X;
            vector3_2.X = vector3_2.Y * num14;
            vector3_2.Y = -x3 * num14;
            float num15 = (float) x1;
            int x4 = y1 * (int) ((double) num14 - (double) Game.SCREEN_WIDTH / 4.0);
            int y3 = (int) (-(double) num15 * (double) num14);
            float num16 = x2;
            float x5 = y2 * num14;
            float y4 = -num16 * num14;
            Vector3 vector3_3 = Vector3.One * num14;
            if (player == 0 || player == 1)
            {
              Fruit fruit1 = (Fruit) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BEGIN);
              fruit1.m_pos = new Vector3((float) x4, (float) y3, 0.0f);
              fruit1.m_vel = new Vector3(x5, y4, 0.0f);
              fruit1.Init((byte[]) null, tpl_size, new Vector3?(vector3_3));
              Fruit fruit2 = fruit1;
              fruit2.m_pos = fruit2.m_pos + vector3_2 * fruit1.m_cur_scale.Y * 100f;
              fruit1.m_gravity.X = fruit1.m_gravity.Y * num14;
              fruit1.m_gravity.Y = 0.0f;
              fruit1.Chuck(Math.MAX(0.0f, v2) + 0.21f);
              fruit1.SetForPlayer(1);
              tpl_size = fruit1.GetFruitType();
            }
            if (player == 0 || player == 2)
            {
              Fruit fruit3 = (Fruit) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BEGIN);
              fruit3.m_pos = new Vector3((float) -x4, (float) -y3, 0.0f);
              fruit3.m_vel = new Vector3(-x5, -y4, 0.0f);
              fruit3.Init((byte[]) null, tpl_size, new Vector3?(vector3_3));
              Fruit fruit4 = fruit3;
              fruit4.m_pos = fruit4.m_pos - vector3_2 * fruit3.m_cur_scale.Y * 100f;
              fruit3.m_gravity.X = -fruit3.m_gravity.Y * num14;
              fruit3.m_gravity.Y = 0.0f;
              fruit3.Chuck(Math.MAX(0.0f, v2) + 0.21f);
              fruit3.SetForPlayer(2);
            }
          }
          else
          {
            Fruit fruit5 = (Fruit) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BEGIN);
            Vector3 one = Vector3.One;
            fruit5.m_pos = new Vector3((float) x1, (float) y1, 0.0f);
            fruit5.m_vel = new Vector3(x2, y2, 0.0f);
            fruit5.Init((byte[]) null, tpl_size, new Vector3?(one));
            fruit5.m_dtMod = num11;
            if (spawner != null)
            {
              fruit5.m_gravity = spawner.gravity * -fruit5.m_gravity.Y;
              switch (spawnPlacements)
              {
                case SPAWN_PLACEMENTS.SPAWNER_LEFT:
                  fruit5.m_gravity.X += 0.01f;
                  break;
                case SPAWN_PLACEMENTS.SPAWNER_RIGHT:
                  fruit5.m_gravity.X -= 0.01f;
                  break;
              }
            }
            Fruit fruit6 = fruit5;
            fruit6.m_pos = fruit6.m_pos + vector3_2 * fruit5.m_cur_scale.Y * 100f;
            fruit5.Chuck(Math.MAX(0.0f, v2) + 0.21f);
            if (spawner != null && spawner.mirror)
            {
              Vector3 pos = fruit5.m_pos;
              Vector3 gravity = fruit5.m_gravity;
              Vector3 vel = fruit5.m_vel;
              int fruitType = fruit5.GetFruitType();
              fruit5 = (Fruit) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BEGIN);
              fruit5.Init((byte[]) null, fruitType, new Vector3?(one));
              fruit5.Chuck(Math.MAX(0.0f, v2) + 0.21f);
              fruit5.m_pos = -pos;
              fruit5.m_vel = -vel;
              fruit5.m_gravity = -gravity;
            }
            return fruit5;
          }
        }
        return (Fruit) null;
      }

      public void SpawnBomb(int bombCount) => this.SpawnBomb(bombCount, (SPAWNER_INFO) null, 0);

      public void SpawnBomb(int bombCount, SPAWNER_INFO spawner, int player)
      {
        for (int index = 0; index < bombCount; ++index)
        {
          float num1 = spawner != null ? spawner.horizontalMin : -1f;
          float num2 = spawner != null ? spawner.horizontalMax : 1f;
          int num3 = (int) ((double) WaveManager.FRUIT_SPAWN_MAX_X * (double) num1 + (double) Math.g_random.Rand32((uint) ((double) num2 * (double) WaveManager.FRUIT_SPAWN_MAX_X - (double) num1 * (double) WaveManager.FRUIT_SPAWN_MAX_X)));
          int num4 = (int) WaveManager.FRUIT_SPAWN_Y;
          float fruitAngleRange = WaveManager.FRUIT_ANGLE_RANGE;
          if (spawner != null && spawner.placement >= SPAWN_PLACEMENTS.SPAWNER_LEFT)
            fruitAngleRange *= 0.6f;
          int num5 = (int) (-((double) num3 / (double) WaveManager.FRUIT_SPAWN_RANGE) * ((double) fruitAngleRange / 2.0));
          int num6 = num5 + (int) ((double) fruitAngleRange / 2.0);
          int num7 = num5 - (int) ((double) fruitAngleRange / 2.0);
          ushort idx = Math.DEGREE_TO_IDX((float) (num7 + Math.g_random.Rand32(num6 - num7)));
          float num8 = WaveManager.FRUIT_MIN_VEL + Math.g_random.RandF(WaveManager.FRUIT_MAX_VEL - WaveManager.FRUIT_MIN_VEL);
          float num9 = (float) ((double) Math.SinIdx(idx) * (double) num8 * (spawner != null ? (double) spawner.velXscale : 1.0));
          float num10 = (float) ((double) Math.CosIdx(idx) * (double) num8 * 1.0750000476837158 * (spawner != null ? (double) spawner.velYscale : 1.0));
          float v2 = spawner != null ? spawner.delayWait : 0.0f;
          Vector3 vector3_1 = Vector3.One;
          Vector3 vector3_2 = Vector3.UnitY * -1f;
          SPAWN_PLACEMENTS spawnPlacements = spawner != null ? spawner.placement : SPAWN_PLACEMENTS.SPAWNER_BOTTOM;
          switch (spawnPlacements)
          {
            case SPAWN_PLACEMENTS.SPAWNER_TOP:
              vector3_1 = Vector3.One * -1f;
              num10 /= 2f;
              vector3_2 = Vector3.UnitY;
              break;
            case SPAWN_PLACEMENTS.SPAWNER_LEFT:
              float num11 = (float) num3;
              num3 = -num4 * (int) ((double) Game.SCREEN_WIDTH / (double) Game.SCREEN_HEIGHT);
              num4 = (int) ((double) num11 * (double) Game.SCREEN_HEIGHT / (double) Game.SCREEN_WIDTH);
              float num12 = num9;
              num9 = -0.75f * num10;
              num10 = num12 - (float) ((double) num8 * (double) spawner.gravity.Y * 0.64999997615814209);
              if (spawnPlacements == SPAWN_PLACEMENTS.SPAWNER_LEFT)
              {
                vector3_1 = new Vector3(-1f, 1f, 1f);
                vector3_2 = Vector3.UnitX * -1f;
                break;
              }
              break;
            case SPAWN_PLACEMENTS.SPAWNER_RIGHT:
              vector3_1 = new Vector3(1f, 1f, 1f);
              vector3_2 = Vector3.UnitX;
              goto case SPAWN_PLACEMENTS.SPAWNER_LEFT;
            case SPAWN_PLACEMENTS.SPAWNER_LEFT_RIGHT:
              spawnPlacements = Math.g_random.Rand32(2) != 0 ? SPAWN_PLACEMENTS.SPAWNER_RIGHT : SPAWN_PLACEMENTS.SPAWNER_LEFT;
              goto case SPAWN_PLACEMENTS.SPAWNER_RIGHT;
          }
          int x1 = num3 * (int) vector3_1.X;
          int y1 = num4 * (int) vector3_1.Y;
          float x2 = num9 * vector3_1.X;
          float y2 = num10 * vector3_1.Y;
          if (Game.IsMultiplayer())
          {
            float num13 = 0.975f * Game.SPLIT_SCREEN_SCALE;
            float x3 = vector3_2.X;
            vector3_2.X = vector3_2.Y * num13;
            vector3_2.Y = -x3 * num13;
            float num14 = (float) x1;
            int x4 = y1 * (int) ((double) num13 - (double) Game.SCREEN_WIDTH / 4.0);
            int y3 = (int) (-(double) num14 * (double) num13);
            float num15 = x2;
            float x5 = y2 * num13;
            float y4 = -num15 * num13;
            Bomb bomb = (Bomb) null;
            Vector3 vector3_3 = Vector3.One * num13 * this.m_bombScale;
            if (player == 1 || player == 0)
            {
              bomb = (Bomb) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB);
              bomb.m_pos = new Vector3((float) x4, (float) y3, (float) (index + 1) * WaveManager.FRUIT_SPAWN_Z_OFFSET);
              bomb.m_vel = new Vector3(x5, y4, 0.0f);
              bomb.Init((byte[]) null, 0, new Vector3?(vector3_3));
              bomb.m_gravity.X = bomb.m_gravity.Y * num13;
              bomb.m_gravity.Y = 0.0f;
              bomb.m_pos.X -= bomb.m_cur_scale.Y * 100f;
              bomb.SetForPlayer(1);
              bomb.Chuck(Math.MAX(0.0f, v2) + 0.21f);
            }
            if (player == 2 || player == 0)
            {
              bomb = (Bomb) ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB);
              bomb.m_pos = new Vector3((float) -x4, (float) -y3, (float) (index + 1) * WaveManager.FRUIT_SPAWN_Z_OFFSET);
              bomb.m_vel = new Vector3(-x5, -y4, 0.0f);
              bomb.Init((byte[]) null, 0, new Vector3?(vector3_3));
              bomb.m_gravity.X = -bomb.m_gravity.Y * num13;
              bomb.m_gravity.Y = 0.0f;
              bomb.m_pos.X += bomb.m_cur_scale.Y * 100f;
              bomb.SetForPlayer(2);
              bomb.Chuck(Math.MAX(0.0f, v2) + 0.21f);
            }
            if (spawner == null && bomb != null && player > 0)
              bomb.MakeFat();
          }
          else
          {
            Entity entity = ActorManager.GetInstance().Add(EntityTypes.ENTITY_BOMB);
            Vector3 vector3_4 = Vector3.One * this.m_bombScale;
            entity.m_pos = new Vector3((float) x1, (float) y1, (float) (index + 1) * WaveManager.FRUIT_SPAWN_Z_OFFSET);
            entity.m_vel = new Vector3(x2, y2, 0.0f);
            entity.Init((byte[]) null, 0, new Vector3?(vector3_4));
            entity.m_pos.Y -= entity.m_cur_scale.Y * 100f;
            ((Bomb) entity).Chuck(Math.MAX(0.0f, v2) + 0.21f);
            if (Game.game_work.gameMode == Game.GAME_MODE.GM_ARCADE)
              ((Bomb) entity).SetForPlayer(Game.MAX_PLAYERS * 2 - 1);
          }
        }
      }

      public void NewGame()
      {
        this.ResetGlobalDt();
        PowerUpManager.GetInstance().Reset(true);
      }

      private void GameOver()
      {
        this.ResetGlobalDt();
        PowerUpManager.GetInstance().Reset();
      }

      private void ResetGlobalDt(float to)
      {
        for (int index1 = 0; index1 < Game.MAX_PLAYERS; ++index1)
        {
          List<PROBABILITY_OVERIDE> probabilityOveride = this.m_probabilityOverides[index1, (int) Game.game_work.gameMode];
          int index2 = 0;
          while (index2 < probabilityOveride.Count)
          {
            if (probabilityOveride[index2].numWaves >= 0)
              probabilityOveride.RemoveAt(index2);
            else
              ++index2;
          }
        }
        this.m_currentGlobalDt = to;
      }

      private float GetCriticalChance(int player)
      {
        return (this.m_currentWave[player] != null ? this.m_currentWave[player].criticalChance : 1f) * this.m_criticalChanceMod;
      }

      protected void ResetWaveChances()
      {
        int index = 0;
        int key = 0;
        foreach (WAVE_INFO waveInfo in this.m_lists[index, (int) Game.game_work.gameMode])
        {
          waveInfo.m_inc = 1f;
          waveInfo.currentChance = waveInfo.chance;
          waveInfo.currentChanceRegrowth = waveInfo.chanceRegrowth;
          if (waveInfo.gamesMin > 0)
          {
            int v1;
            if (Game.game_work.saveData.waveGameCount[(int) Game.game_work.gameMode].TryGetValue(key, out v1))
            {
              if (v1 > 0)
              {
                Game.game_work.saveData.waveGameCount[(int) Game.game_work.gameMode][key] = Math.MIN(v1, waveInfo.gamesMax);
                waveInfo.currentChance = 0;
                waveInfo.currentChanceRegrowth = 0.0f;
              }
            }
            else
            {
              int num1 = (waveInfo.gamesMax + waveInfo.gamesMin) / 2;
              int num2 = Math.g_random.Rand32(Math.MAX(1, Math.g_random.Rand32(num1 / 2))) + num1 / 4;
              Game.game_work.saveData.waveGameCount[(int) Game.game_work.gameMode][key] = num2;
              waveInfo.currentChance = 0;
              waveInfo.currentChanceRegrowth = 0.0f;
            }
          }
          ++key;
        }
      }

      public void GetNextWave() => this.GetNextWave(0);

      protected void GetNextWave(int player)
      {
        AchievementManager instance = AchievementManager.GetInstance();
        Game.game_work.saveData.UnlockTotals();
        instance.UnlockScoreAchievement(Game.game_work.currentScore);
        instance.UnlockTotalFruitAchievement((int) Game.game_work.totalScore);
        ++this.m_waveCount[player];
        ++this.m_numberOfWavesSpawned[player];
        if (this.m_waveCount[player] > 1 && this.m_currentWave != null)
          ++this.m_currentWave[player].m_inc;
        this.m_currentWave[player] = this.m_lists[player, (int) Game.game_work.gameMode][0];
        WAVE_INFO[] filledArray = ArrayInit.CreateFilledArray<WAVE_INFO>(20);
        int index1 = 0;
        int num1 = 0;
        foreach (WAVE_INFO waveInfo in this.m_lists[player, (int) Game.game_work.gameMode])
        {
          if ((double) waveInfo.currentChanceRegrowth > 0.0)
          {
            if (waveInfo.currentChance < waveInfo.chance)
            {
              waveInfo.currentChance += Math.MAX(1, waveInfo.chance * (int) waveInfo.currentChanceRegrowth);
              if (waveInfo.currentChance > waveInfo.chance)
                waveInfo.currentChance = waveInfo.chance;
            }
            else
              waveInfo.currentChance = waveInfo.chance;
          }
          if (this.m_waveCount[player] >= waveInfo.waveNo && (this.m_waveCount[player] <= waveInfo.waveNoRange || waveInfo.waveNoRange == -2))
          {
            if (index1 == 0)
              this.m_currentWave[player] = waveInfo;
            filledArray[index1] = waveInfo;
            num1 += waveInfo.currentChance;
            ++index1;
          }
        }
        if (index1 > 0 && filledArray[0].typesToPickFromCount > 0 && filledArray[0].waveNo > this.m_fruitTypesToPickFromWave[player])
        {
          this.m_fruitTypesToPickFromWave[player] = filledArray[0].waveNo;
          this.m_numFruitTypesToPickFrom[player] = Math.MIN(32 /*0x20*/, filledArray[0].typesToPickFromCount);
          for (int index2 = 0; index2 < this.m_numFruitTypesToPickFrom[player]; ++index2)
          {
            int num2 = Fruit.FruitType(filledArray[0].typesToPickFrom[index2]);
            if (num2 < 0)
            {
              num2 = Fruit.RandomFruit(false);
              bool flag = index2 < Fruit.MAX_FRUIT_TYPES - 2;
              while (flag)
              {
                flag = false;
                for (int index3 = 0; index3 < index2; ++index3)
                {
                  if (num2 == this.m_fruitTypesToPickFrom[player, index3])
                  {
                    flag = true;
                    num2 = Fruit.RandomFruit(false);
                    break;
                  }
                }
              }
            }
            this.m_fruitTypesToPickFrom[player, index2] = num2;
          }
        }
        if (index1 > 1)
        {
          int num3 = Math.g_random.Rand32(num1 * 10);
          int num4 = 0;
          for (int index4 = 0; index4 < index1; ++index4)
          {
            num4 += filledArray[index4].currentChance * 10;
            if (num3 < num4)
            {
              this.m_currentWave[player] = filledArray[index4];
              break;
            }
          }
        }
        if (this.m_currentWave[player].gamesMin > 0)
        {
          Game.game_work.saveData.waveGameCount[(int) Game.game_work.gameMode][this.m_currentWave[player].idx] = this.m_currentWave[player].gamesMin == this.m_currentWave[player].gamesMax ? this.m_currentWave[player].gamesMin : Math.g_random.Rand32(this.m_currentWave[player].gamesMax - this.m_currentWave[player].gamesMin) + this.m_currentWave[player].gamesMin;
          this.m_currentWave[player].currentChanceRegrowth = 0.0f;
        }
        this.m_currentWave[player].currentChance = 0;
        this.m_waveDelay[player] = (double) this.m_currentWave[player].beforeDelay > 0.0 ? Math.MAX(0.05f, this.m_currentWave[player].beforeDelay + this.m_currentWave[player].beforeDelayInc * this.m_currentWave[player].m_inc) : 0.0f;
        this.m_waveWait[player] = this.m_currentWave[player].nextDelay;
        if ((double) this.m_currentWave[player].nextDelaySpInc != 0.0)
          this.m_waveWait[player] = Math.MAX(this.m_waveWait[player] + this.m_speed[player] * this.m_currentWave[player].nextDelaySpInc, 0.05f);
        for (int index5 = 0; index5 < this.m_currentWave[player].spawnerCount; ++index5)
          this.m_currentWave[player].spawners[index5].Reset(this.m_currentWave[player].m_inc);
        List<PROBABILITY_OVERIDE> probabilityOveride = this.m_probabilityOverides[player, (int) Game.game_work.gameMode];
        int index6 = 0;
        while (index6 < probabilityOveride.Count)
        {
          if (probabilityOveride[index6].numWaves > 0 && this.m_waveCount[player] != 0)
          {
            --probabilityOveride[index6].numWaves;
            if (probabilityOveride[index6].numWaves <= 0)
            {
              probabilityOveride.RemoveAt(index6);
              continue;
            }
          }
          probabilityOveride[index6].spawnedThisWave = 0;
          ++index6;
        }
      }

      public void SetCurrentWave(int wave, float extraTime, int player)
      {
        this.ClearUnspawned();
        this.m_waveCount[player] = wave - 1;
        this.GetNextWave();
        this.m_waveDelay[player] = Math.MAX(0.0f, this.m_waveDelay[player] + extraTime);
      }

      public void ResetGlobalDt() => this.ResetGlobalDt(1f);

      public float BombScale() => this.BombScale(1f);

      public float BombScale(float value) => this.m_bombScale *= value;

      public float BombMultiplyer() => this.BombMultiplyer(1f);

      public float FruitMultiplyer() => this.FruitMultiplyer(1f);

      public float CriticalChanceMod() => this.CriticalChanceMod(1f);

      public float BombMultiplyer(float value) => this.m_bombMultiplyer *= value;

      public float FruitMultiplyer(float value) => this.m_fruitMultiplyer *= value;

      public float CriticalChanceMod(float value) => this.m_criticalChanceMod *= value;

      public List<PROBABILITY_OVERIDE> GetCurrentOverideList() => this.GetCurrentOverideList(0);

      public void SetCurrentWave(int wave, float extraTime) => this.SetCurrentWave(wave, extraTime, 0);

      public float GetSpeed() => this.GetSpeed(0);

      public void ResetSpeed() => this.ResetSpeed(0);

      public void AddSpeed(float val) => this.AddSpeed(val, 0);

      public int GetComboBonus() => this.GetComboBonus(0);

      public int GetComboBonus(int player) => this.m_comboBonus[player];

      public void AddToSpeedLossTime(float val) => this.AddToSpeedLossTime(val, 0);

      public int WaveCount() => this.WaveCount(0);

      public int WaveCount(int player) => this.m_waveCount[player];

      public List<PROBABILITY_OVERIDE> GetCurrentOverideList(int player)
      {
        return this.m_probabilityOverides[player, (int) Game.game_work.gameMode];
      }

      public static float MAX_DELTA_TIME_SCALE => 100f;

      public float GetSpeed(int player) => this.m_speed[player];

      public void ResetSpeed(int player)
      {
        this.m_desiredSpeed[player] = 0.0f;
        this.m_speed[player] = 0.0f;
        this.m_speedLossTime[player] = 0.0f;
        uint hash = StringFunctions.StringHash("blitz_bonus");
        Game.game_work.saveData.ClearTotal(hash);
        this.m_comboBonus[player] = 0;
        this.m_nextComboBonus[player] = 0.0f;
        if (this.m_speedControl[player] == null)
          return;
        this.m_speedControl[player].m_pulseSpeed = 0.0f;
        this.m_speedControl[player].m_lossTime = 0.0f;
      }

      public void AddSpeed(float val, int player)
      {
        this.m_desiredSpeed[player] = Math.CLAMP(val + this.m_desiredSpeed[player], 0.0f, WaveManager.MAX_SPEEDIE_GONZALES);
        if ((double) val <= 0.0)
          return;
        uint hash1 = StringFunctions.StringHash("blitz_bonus");
        this.m_speedLossTime[player] = 1f;
        if ((double) this.m_nextComboBonus[player] > 0.0)
        {
          this.m_nextComboBonus[player] -= val;
          if ((double) this.m_nextComboBonus[player] <= 0.0)
          {
            this.m_comboBonus[player] = Game.game_work.saveData.AddToTotal("blitz_bonus", hash1, 1, false, false);
            string s = $"blitz_{Math.MIN(this.m_comboBonus[player], 6)}";
            PowerUpManager.GetInstance().ActivateScreenEffect(StringFunctions.StringHash(s));
            Game.AddToCurrentScore(5 * Math.MIN(this.m_comboBonus[player], 6), player);
            this.m_nextComboBonus[player] = 2.5f;
            if (this.m_comboBonus[player] > 3)
              this.m_nextComboBonus[player] = 2.5f;
          }
        }
        else if ((double) this.m_desiredSpeed[player] > (double) WaveManager.SPEED_START)
        {
          this.m_nextComboBonus[player] = 2.5f;
          Game.game_work.saveData.ClearTotal(hash1);
          this.m_comboBonus[player] = Game.game_work.saveData.AddToTotal("blitz_bonus", hash1, 1, false, false);
          Game.AddToCurrentScore(5, player);
          uint hash2 = StringFunctions.StringHash("blitz_1");
          PowerUpManager.GetInstance().ActivateScreenEffect(hash2);
        }
        uint hash3 = StringFunctions.StringHash("best_blitz");
        int total = Game.game_work.saveData.GetTotal(hash3);
        Game.game_work.saveData.AddToTotal("best_blitz", hash3, Math.MAX(0, this.m_comboBonus[player] - total), false, false);
      }

      public void AddToSpeedLossTime(float val, int player)
      {
        if ((double) this.m_speedLossTime[player] <= 0.0)
          return;
        this.m_speedLossTime[player] = Math.MIN(this.m_speedLossTime[player] + val, 1f);
      }

      public float GetComboBonusProgression(int player)
      {
        return Math.MIN(((float) this.m_comboBonus[player] + Math.CLAMP((float) (1.0 - (double) this.m_nextComboBonus[player] / 2.5), 0.0f, 1f)) / 6f, 1f);
      }

      public WaveManager()
      {
        this.m_currentGlobalDtMod = 1f;
        this.m_currentGlobalDt = 1f;
        this.m_blitzSpawnedThisGame = (byte) 0;
        this.m_blitzForceSpawnedCounter = (byte) 0;
        this.m_blitzSpawnTime = 10f + Math.g_random.RandF(10f);
        for (int index = 0; index < 4; ++index)
        {
          this.m_globalDtInc[index] = 0.0f;
          this.m_globalDtStart[index] = 1f;
          this.m_globalDtMax[index] = 1f;
        }
      }
    }
}
