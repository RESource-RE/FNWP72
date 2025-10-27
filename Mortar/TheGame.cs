// Decompiled with JetBrains decompiler
// Type: Mortar.TheGame
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using FNWP72.Engine;
using FruitNinja;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Mortar
{

    public class TheGame : Microsoft.Xna.Framework.Game
    {
      private const int MAGIC1 = 195800833;
      private const int MAGIC2 = 33479857;
      private static string CFG_DIRECTORY = "FruitNinja";
      private static string CFG_FILENAME = TheGame.CFG_DIRECTORY + "/setting.dat";
      private static string ANA_FILENAME = TheGame.CFG_DIRECTORY + "/analytics.dat";
      public static TheGame.Settings settings = new TheGame.Settings();
      public static TheGame.Analytics analytics = new TheGame.Analytics();
      //public static SaveRingtoneTask srtt;
      public GraphicsDeviceManager graphics;
      public SpriteBatch spriteBatch;
      public SpriteFont font1;
      public SpriteFont font2;
      public SpriteFont font3;
      public SpriteFont font4;
      public StringTable stringTable;
      public static Texture2D picture;
      public static TheGame instance;
      public static FruitGame fgame;
      public static bool emulator = false;
      public static bool exceptionThrown = false;
      public static bool exitingGame = false;
      public static bool logInSucceeded = false;
      public static bool pomegranateThrown = false;
      private static uint error_code = 2415923200 /*0x90001000*/;
      private bool init_failed;
      private bool load_failed;
      private bool quit_game;
      private static bool setup_attribute = true;
      private static Thread setup_thread = (Thread) null;
      private static ConnectionAcquire ca = (ConnectionAcquire) null;
      private static int pe_count = 0;
      private Texture hbLogoTexture;
      private Texture2D bobsCousinIt;
      public static Texture2D bobsCousinHairyMaclary;
      private Texture upsell;
      private Texture menu_purchase;
      private Texture menu_exit;
      private float timeout = 5.5f;
      private bool fadeout;
      private bool fadein;
      private FadeManager fm;
      private bool showUpsell;
      private bool exit_from_upsell;
      private Vector2 pos1;
      private Vector2 pos2;
      public static int initupdates = 5;
      public LinkedList<TheGame.MyTouchState> tstate = new LinkedList<TheGame.MyTouchState>();
      private static float dtCo = 1f;
      private static bool sb = false;
      private static int sbc = 0;
      private bool _BackButtonWasPressed;
      public bool BackButtonWasPressed;
      private bool backButtonDownLast;
      private bool gameUpdateRequired;
      private bool curr_down;
      private bool prev_down;
      private int uid = -1;
      private int tx;
      private int ty;
      private bool inExit;
      private bool inPurchase;
      private static bool ___isActiveSim = true;
      public static bool disableNetworkCalls = false;
      private GamePadState state_prev;
      private GamePadState state_curr;
      private int skip;
      private int updates;
      public static bool switchLanguage = false;
      public static StringTableUtils.Language switchToLanguage;
      public static float loadinScreen = 0.0f;
      private static Texture2D m_fruitTex;
      private static Texture2D m_ninjaTex;
      private static Texture2D m_headTex;
      private static Texture2D m_bodyTex;
      private static bool ts = false;

      public static void LoadConfig()
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
        try
        {
          if (!storeForApplication.DirectoryExists(TheGame.CFG_DIRECTORY))
            storeForApplication.CreateDirectory(TheGame.CFG_DIRECTORY);
          if (storeForApplication.FileExists(TheGame.CFG_FILENAME))
          {
            storageFileStream = storeForApplication.OpenFile(TheGame.CFG_FILENAME, FileMode.Open);
            int length = (int) storageFileStream.Length;
            if (length == 28 + FruitNinja.Game.MAX_SYSTEM_ACHIEVEMENTS * 4)
            {
              byte[] buffer = new byte[length];
              storageFileStream.Read(buffer, 0, length);
              storageFileStream.Close();
              int int32_1 = BitConverter.ToInt32(buffer, 0);
              int int32_2 = BitConverter.ToInt32(buffer, 4);
              int int32_3 = BitConverter.ToInt32(buffer, 8);
              int int32_4 = BitConverter.ToInt32(buffer, 12);
              int int32_5 = BitConverter.ToInt32(buffer, 16 /*0x10*/);
              int int32_6 = BitConverter.ToInt32(buffer, 20);
              int int32_7 = BitConverter.ToInt32(buffer, 24);
              if (int32_1 == 195800833 && int32_2 == 33479857 && int32_3 >= 0 && int32_3 <= 5)
              {
                TheGame.settings.language = int32_3;
                TheGame.settings.week = int32_4;
                TheGame.settings.tf = int32_5;
                TheGame.settings.aw = int32_6;
                TheGame.settings.bestThisWeek = int32_7;
                FruitNinja.Game.game_work.SetLanguage((StringTableUtils.Language) int32_3);
                int startIndex = 28;
                for (int index = 0; index < FruitNinja.Game.MAX_SYSTEM_ACHIEVEMENTS; ++index)
                {
                  TheGame.settings.achievements[index] = BitConverter.ToUInt32(buffer, startIndex);
                  startIndex += 4;
                }
              }
              else
                TheGame.SaveConfig();
            }
            else
            {
              storageFileStream.Close();
              TheGame.SaveConfig();
            }
          }
          else
            TheGame.SaveConfig();
        }
        catch
        {
        }
        finally
        {
          storageFileStream?.Close();
        }
      }

      public static void SaveConfig()
      {
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
        try
        {
          if (!storeForApplication.DirectoryExists(TheGame.CFG_DIRECTORY))
            storeForApplication.CreateDirectory(TheGame.CFG_DIRECTORY);
          storageFileStream = storeForApplication.OpenFile(TheGame.CFG_FILENAME, FileMode.Create);
          if (storageFileStream == null)
            return;
          TheGame.settings.language = (int) FruitNinja.Game.game_work.language;
          byte[] bytes1 = BitConverter.GetBytes(TheGame.settings.magic1);
          byte[] bytes2 = BitConverter.GetBytes(TheGame.settings.magic2);
          byte[] bytes3 = BitConverter.GetBytes(TheGame.settings.language);
          byte[] bytes4 = BitConverter.GetBytes(TheGame.settings.week);
          byte[] bytes5 = BitConverter.GetBytes(TheGame.settings.tf);
          byte[] bytes6 = BitConverter.GetBytes(TheGame.settings.aw);
          byte[] bytes7 = BitConverter.GetBytes(TheGame.settings.bestThisWeek);
          storageFileStream.Write(bytes1, 0, 4);
          storageFileStream.Write(bytes2, 0, 4);
          storageFileStream.Write(bytes3, 0, 4);
          storageFileStream.Write(bytes4, 0, 4);
          storageFileStream.Write(bytes5, 0, 4);
          storageFileStream.Write(bytes6, 0, 4);
          storageFileStream.Write(bytes7, 0, 4);
          for (int index = 0; index < FruitNinja.Game.MAX_SYSTEM_ACHIEVEMENTS; ++index)
          {
            byte[] bytes8 = BitConverter.GetBytes(TheGame.settings.achievements[index]);
            storageFileStream.Write(bytes8, 0, 4);
          }
          storageFileStream.Close();
        }
        catch
        {
        }
        finally
        {
          storageFileStream?.Close();
        }
      }

      private string ERROR_BUTTON_1 => TheGame.instance.stringTable.GetString(927);

      private string ERROR_TITLE_1 => TheGame.instance.stringTable.GetString(1143);

      private static string ERROR_MESSAGE(uint code)
      {
        return $"Error 0x{code:x} occurred. Fruit Ninja cannot continue and needs to exit.";
      }

      private static string ERROR_MESSAGE_GAME()
      {
        return $"Error 0x{TheGame.GetErrorCode():x} occurred. Please check your memory usage before continuing.";
      }

      public static void SetErrorCode(uint code) => TheGame.error_code = code;

      public static uint GetErrorCode() => TheGame.error_code;

      private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
      {
        e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
      }

        /*
      private void srtt_Completed(object sender, TaskEventArgs e)
      {
        switch ((int) e.TaskResult)
        {
        }
      }
        */

      public TheGame()
      {
        TheGame.instance = this;
        this.graphics = new GraphicsDeviceManager((Microsoft.Xna.Framework.Game) this);
        this.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.graphics_PreparingDeviceSettings);
        this.Content.RootDirectory = "Content";
        this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
        this.graphics.PreferredBackBufferWidth = 800;
        this.graphics.PreferredBackBufferHeight = 480;
        this.graphics.IsFullScreen = true;
        this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        this.graphics.ApplyChanges();
        Guide.IsScreenSaverEnabled = false;
        TheGame.fgame = new FruitGame();
        //TheGame.srtt = new SaveRingtoneTask();
        //((ChooserBase<TaskEventArgs>) TheGame.srtt).Completed += new EventHandler<TaskEventArgs>(this.srtt_Completed);
        StringTableUtils.Language languageFromCulture = StringTableUtils.ParseLanguageFromCulture(CultureInfo.CurrentCulture.Name);
        FruitNinja.Game.game_work.SetLanguage(languageFromCulture);
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        try
        {
          if (!IsolatedStorageFile.GetUserStoreForApplication().FileExists(TheGame.CFG_FILENAME))
            TheGame.SaveConfig();
        }
        catch
        {
        }
        this.stringTable = new StringTable();
      }

      public static IDictionary<string, string> MGSStatistics_Setup()
      {
        return (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Trial",
            FruitNinja.Game.isWP7TrialMode().ToString()
          },
          {
                "Tombstone",
            TheGame.ts.ToString()
          },
          {
            "Connection",
            TheGame.ca.GetInterfaceType()
          },
          {
            "Manufacturer",
            TheGame.ts.ToString()
          },
          {
            "Model",
            TheGame.ts.ToString()
          },
          {
            "FirmwareVersion",
            TheGame.ts.ToString()
          },
          {
            "HardwareVersion",
            TheGame.ts.ToString()
          },
          {
            "Memory",
            TheGame.ts.ToString()
          },
          {
            "Locale",
            CultureInfo.CurrentUICulture.Name
          },
          {
            "UtcBaseOffset",
            TimeZoneInfo.Local.BaseUtcOffset.TotalHours.ToString()
          },
          {
            "UtcCurrentOffset",
            TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours.ToString()
          },
          {
            "Experiment",
            ""
          }
        };
      }

      public static IDictionary<string, string> MGSStatistics_Teardown()
      {
        return (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Trial",
            TheGame.ts.ToString()
          }
        };
      }

      public static IDictionary<string, string> Halfbrick_Analytics()
      {
        return TheGame.analytics.GetAnalyticsData();
      }

      public static void PE_SetupAttributeMGS(
        string Trial,
        string Tombstone,
        string Connection,
        string Manufacturer,
        string Model,
        string FirmwareVersion,
        string HardwareVersion,
        string Memory,
        string Locale,
        string UtcBaseOffset,
        string UtcCurrentOffset,
        string Experiment)
      {
        ++TheGame.pe_count;
      }

      public static void PE_SetupAttribute() => ++TheGame.pe_count;

      public static void PE_TeardownAttribute() => ++TheGame.pe_count;

      public static void PE_ShowMarketplace() => ++TheGame.pe_count;

      public static void PE_AchievementAwarded(string name) => ++TheGame.pe_count;

      public static void PE_GameMode(string name)
      {
        if (FruitNinja.Game.isWP7TrialMode())
          ++TheGame.analytics.games_trial;
        else
          ++TheGame.analytics.games_full;
        switch (name)
        {
          case "Classic":
            ++TheGame.analytics.games_classic;
            break;
          case "Arcade":
            ++TheGame.analytics.games_arcade;
            break;
          case "Zen":
            ++TheGame.analytics.games_zen;
            break;
        }
        ++TheGame.pe_count;
      }

      public static void PE_GameInformation() => ++TheGame.pe_count;

      protected override void Initialize()
      {
        try
        {
         // PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Current_Deactivated);
          TheGame.LoadConfig();
          TheGame.analytics.Load();
          this.stringTable.LoadHeader("stringtables/translations");
          this.stringTable.LoadAllLanguages();
          this.SwitchToLanguage(FruitNinja.Game.game_work.language);
        }
        catch
        {
          try
          {
            MediaPlayer.IsMuted = false;
          }
          catch
          {
          }
          TheGame.exitingGame = true;
          this.Exit();
        }
        try
        {
          TheGame.emulator = true;
          int num = TheGame.emulator ? 1 : 0;
          base.Initialize();
        }
        catch (Exception ex)
        {
          Exception innerException = ex.InnerException;
          while (innerException != null)
            innerException = innerException.InnerException;
          string text = TheGame.ERROR_MESSAGE(2147487744U /*0x80001000*/);
          this.init_failed = true;
          string[] buttons = new string[1]
          {
            this.ERROR_BUTTON_1
          };
          while (Guide.IsVisible)
            Thread.Sleep(32 /*0x20*/);
          if (text.Length > (int) byte.MaxValue)
            text = text.Substring(0, 250);
          Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        }
      }

      private void InitOrLoadFailedCallback(IAsyncResult result) => this.quit_game = true;

      private void IngameException(IAsyncResult result)
      {
        if (GameTask.GameReset(0U))
          FruitNinja.Game.QuitToMenu();
        else
          this.quit_game = true;
      }

      public void DoUpsell(bool exitGame)
      {
        if (!FruitNinja.Game.isWP7TrialMode() || this.showUpsell)
          return;
        this.exit_from_upsell = exitGame;
        this.showUpsell = true;
        this.pos1 = new Vector2((float) (800 - (this.menu_exit.intex.Width + 16 /*0x10*/)), (float) (480 - (this.menu_exit.intex.Height + 20)));
        this.pos2 = new Vector2(this.pos1.X - (float) (this.menu_purchase.intex.Width + 16 /*0x10*/), this.pos1.Y);
        GameTask.SkipToPause(false);
      }

      public bool UpsellComplete() => !this.showUpsell;

      protected override void LoadContent()
      {
        try
        {
          FruitNinja.Game.SetTrialModeState();
          this.hbLogoTexture = Texture.Load(MTLocalisation.GetLocalisedTexturePath() + "/HB_logo.tex");
          this.bobsCousinIt = this.Content.Load<Texture2D>("extra/MGS_WP7_Horiz_Still");
          TheGame.bobsCousinHairyMaclary = this.Content.Load<Texture2D>("extra/black");
          this.upsell = TextureManager.GetInstance().Load("trial_upsell.tex", true);
          this.menu_purchase = TextureManager.GetInstance().Load("menu_purchase.tex", true);
          this.menu_exit = TextureManager.GetInstance().Load("quit_title.tex", true);
          this.fm = new FadeManager(TheGame.bobsCousinHairyMaclary, TheGame.bobsCousinHairyMaclary);
          TheGame.instance.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
          this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
          this.font1 = this.Content.Load<SpriteFont>("extra/mainFont");
          this.font2 = this.Content.Load<SpriteFont>("extra/detailFont");
          this.font3 = this.Content.Load<SpriteFont>("extra/titleFont");
          this.font4 = this.Content.Load<SpriteFont>("extra/titleFontSmall");
        }
        catch (Exception ex)
        {
          Exception innerException = ex.InnerException;
          while (innerException != null)
            innerException = innerException.InnerException;
          string text = TheGame.ERROR_MESSAGE(2147491840U /*0x80002000*/);
          this.load_failed = true;
          string[] buttons = new string[1]
          {
            this.ERROR_BUTTON_1
          };
          while (Guide.IsVisible)
            Thread.Sleep(32 /*0x20*/);
          if (text.Length > (int) byte.MaxValue)
            text = text.Substring(0, 250);
          Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        }
      }

      protected override void UnloadContent()
      {
      }

      public static bool TriggerShowBuyMessageBox
      {
        set
        {
          if (!value || !FruitNinja.Game.isWP7TrialMode())
            return;
          TheGame.sb = value;
          TheGame.sbc = 40;
        }
      }

      private void NotConnected(IAsyncResult result)
      {
      }

      protected override void Update(GameTime gameTime)
      {
        if (this.updates > 0)
          return;
        ++this.updates;
        float timeSinceLastUpdate = (float) gameTime.ElapsedGameTime.Milliseconds / 1000f;
        if (!this.IsActive || !TheGame.instance.IsActive)
          return;
        if (this.skip > 0)
          --this.skip;
        else if (this.gameUpdateRequired)
        {
          this.gameUpdateRequired = false;
          this.HandleGameUpdateException();
        }
        else
        {
          if (TheGame.sb)
          {
            --TheGame.sbc;
            if (TheGame.sbc <= 0)
            {
              this.DoUpsell(false);
              TheGame.sb = false;
              TheGame.sbc = 0;
              return;
            }
          }
          this._BackButtonWasPressed = false;
          bool flag = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
          if (!flag && this.backButtonDownLast)
            this.BackButtonWasPressed = true;
          this.backButtonDownLast = flag;
          if (this._BackButtonWasPressed)
          {
            if (TheGame.___isActiveSim)
            {
              TheGame.___isActiveSim = false;
              this.OnDeactivated((object) null, (EventArgs) null);
            }
            else
            {
              TheGame.___isActiveSim = true;
              this.OnActivated((object) null, (EventArgs) null);
            }
          }
          if (TheGame.instance != null && !TheGame.instance.IsActive)
            return;
          if (this.quit_game)
          {
            try
            {
              MediaPlayer.IsMuted = false;
            }
            catch
            {
            }
            TheGame.exitingGame = true;
            this.Exit();
          }
          else if (TheGame.exceptionThrown)
          {
            try
            {
              base.Update(gameTime);
            }
            catch (GameUpdateRequiredException ex)
            {
              this.gameUpdateRequired = true;
              TheGame.disableNetworkCalls = true;
            }
          }
          else
          {
            TouchPanel.EnabledGestures = GestureType.None;
            if (this.showUpsell)
            {
              Mortar.Touch.instance.Clear();
              try
              {
                this.state_prev = this.state_curr;
                this.state_curr = GamePad.GetState(PlayerIndex.One);
                if (this.state_prev.Buttons.Back == ButtonState.Pressed && this.state_curr.Buttons.Back == ButtonState.Released)
                {
                  if (this.exit_from_upsell)
                  {
                    try
                    {
                      MediaPlayer.IsMuted = false;
                    }
                    catch
                    {
                    }
                    TheGame.exitingGame = true;
                    this.Exit();
                  }
                  else
                  {
                    this.skip = 1;
                    this.showUpsell = false;
                    Thread.Sleep(32 /*0x20*/);
                    return;
                  }
                }
                TouchCollection state = TouchPanel.GetState();
                this.curr_down = state.Count > 0;
                if (this.prev_down != this.curr_down)
                {
                  this.prev_down = this.curr_down;
                  if (this.curr_down)
                  {
                    if (this.uid == -1)
                    {
                      foreach (TouchLocation touchLocation in state)
                      {
                        if (touchLocation.State == TouchLocationState.Pressed)
                        {
                          this.uid = touchLocation.Id;
                          this.tx = (int) touchLocation.Position.X;
                          this.ty = (int) touchLocation.Position.Y;
                          break;
                        }
                      }
                    }
                  }
                  else
                  {
                    if (this.inExit)
                    {
                      if (this.exit_from_upsell)
                      {
                        try
                        {
                          MediaPlayer.IsMuted = false;
                        }
                        catch
                        {
                        }
                        TheGame.exitingGame = true;
                        this.Exit();
                      }
                      else
                        this.showUpsell = false;
                    }
                    else if (this.inPurchase)
                      this.showUpsell = false;
                    this.inExit = false;
                    this.inPurchase = false;
                    this.uid = -1;
                    this.tx = -1;
                    this.ty = -1;
                  }
                }
                if (this.curr_down)
                {
                  foreach (TouchLocation touchLocation in state)
                  {
                    if (this.uid == touchLocation.Id && touchLocation.State == TouchLocationState.Moved)
                    {
                      this.tx = (int) touchLocation.Position.X;
                      this.ty = (int) touchLocation.Position.Y;
                      break;
                    }
                  }
                  Rectangle rectangle1 = new Rectangle((int) this.pos1.X, (int) this.pos1.Y, this.menu_exit.intex.Width, this.menu_exit.intex.Height);
                  Rectangle rectangle2 = new Rectangle((int) this.pos2.X, (int) this.pos2.Y, this.menu_purchase.intex.Width, this.menu_purchase.intex.Height);
                  this.inExit = rectangle1.Contains(this.tx, this.ty);
                  if (rectangle2.Contains(this.tx, this.ty))
                    this.inPurchase = true;
                  else
                    this.inPurchase = false;
                }
                else
                {
                  this.inExit = false;
                  this.inPurchase = false;
                }
              }
              catch
              {
              }
            }
            else if ((double) this.timeout > 0.0)
            {
              if ((double) this.timeout > 0.5 && TouchPanel.GetState().Count > 0)
                this.timeout = 0.5f;
              this.timeout -= (float) gameTime.ElapsedGameTime.Milliseconds / 1000f;
              if ((double) this.timeout < 0.0)
                this.timeout = 0.0f;
              if (!this.fadeout && (double) this.timeout < 0.5)
              {
                this.fm.Start(FadeState.ToBlack, 250f);
                this.fadeout = true;
              }
              if (!this.fadein && (double) this.timeout < 0.25)
              {
                this.fm.Start(FadeState.ToNormal, 250f);
                this.fadein = true;
              }
              this.fm.Update(gameTime);
            }
            else
            {
              try
              {
                if (this.load_failed || this.init_failed || this.quit_game)
                  return;
                if (TheGame.initupdates > 0)
                {
                  --TheGame.initupdates;
                  if (TheGame.initupdates != 0)
                    return;
                  TheGame.fgame.Init(0U, "");
                }
                else
                {
                  foreach (TouchLocation touchLocation in TouchPanel.GetState())
                  {
                    if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Moved)
                      Mortar.Touch.instance.__UpdateInternal((uint) touchLocation.Id, true, touchLocation.Position.X, touchLocation.Position.Y, 0.0f);
                    else if (touchLocation.State == TouchLocationState.Released || touchLocation.State == TouchLocationState.Invalid)
                      Mortar.Touch.instance.__UpdateInternal((uint) touchLocation.Id, false, touchLocation.Position.X, touchLocation.Position.Y, 0.0f);
                  }
                  Mortar.Touch.instance.Update();
                  float dt = 0.0f;
                  if (!SystemManager.GetInstance().Update(ref dt))
                  {
                    TheGame.fgame.End();
                    try
                    {
                      MediaPlayer.IsMuted = false;
                    }
                    catch
                    {
                    }
                    TheGame.exitingGame = true;
                    this.Exit();
                  }
                  TheGame.fgame.Update(timeSinceLastUpdate);
                  AchievementManager.Update(gameTime);
                  this.BackButtonWasPressed = false;
                  try
                  {
                    base.Update(gameTime);
                  }
                  catch (GameUpdateRequiredException ex)
                  {
                    this.gameUpdateRequired = true;
                  }
                }
              }
              catch (GameUpdateRequiredException ex)
              {
                this.gameUpdateRequired = true;
              }
              catch (Exception ex)
              {
                this.GameException();
                Exception innerException = ex.InnerException;
                while (innerException != null)
                  innerException = innerException.InnerException;
                string text = TheGame.ERROR_MESSAGE_GAME();
                string[] buttons = new string[1]
                {
                  this.ERROR_BUTTON_1
                };
                while (Guide.IsVisible)
                  Thread.Sleep(32 /*0x20*/);
                if (text.Length > (int) byte.MaxValue)
                  text = text.Substring(0, 250);
                Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.IngameException), (object) null);
              }
            }
          }
        }
      }

      private void HandleGameUpdateException()
      {
        string str1 = TheGame.instance.stringTable.GetString(931);
        string str2 = TheGame.instance.stringTable.GetString(926);
        string title = " ";
        string[] buttons = new string[2]{ str2, str1 };
        string text;
        switch (FruitNinja.Game.game_work.language)
        {
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            text = "Une mise à jour est disponible ! Cette mise à jour est requise pour se connecter à Xbox LIVE. Mettre à jour dès maintenant ?";
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            text = "¡Hay una actualización disponible! Esta actualización es necesaria para conectarse a Xbox LIVE. ¿Actualizar ahora?";
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            text = "Ein Update ist verfügbar! Für dieses Update muss eine Verbindung zu Xbox LIVE hergestellt werden. Jetzt updaten?";
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            text = "È disponibile un aggiornamento! Questo aggiornamento è necessario per connettersi a Xbox LIVE. Aggiorna ora?";
            break;
          default:
            text = "An update is available! This update is required to connect to Xbox LIVE. Update now?";
            break;
        }
        while (Guide.IsVisible)
          Thread.Sleep(32 /*0x20*/);
        Guide.BeginShowMessageBox(title, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(TheGame.GameUpdateCallback), (object) null);
      }

      private static void GameUpdateCallback(IAsyncResult result)
      {
        try
        {
          int? nullable = Guide.EndShowMessageBox(result);
          if (!nullable.HasValue || !nullable.HasValue || nullable.Value != 1)
            return;
          if (FruitNinja.Game.isWP7TrialMode())
          {
            if (Guide.IsVisible)
              Thread.Sleep(128 /*0x80*/);
            TheGame.PE_ShowMarketplace();
            Guide.ShowMarketplace(PlayerIndex.One);
          }
        }
        catch
        {
        }
      }

      protected override void Draw(GameTime gameTime)
      {
        this.updates = 0;
        if (this.IsActive)
        {
          if (TheGame.instance.IsActive)
          {
            if (TheGame.switchLanguage)
            {
              DisplayManager.GetInstance().currentTexture = (Texture) null;
              TheGame.switchLanguage = false;
              this.SwitchToLanguage(TheGame.switchToLanguage, true);
            }
            if (this.showUpsell && this.upsell != null)
            {
              this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
              this.spriteBatch.Draw(this.upsell.intex, new Rectangle(0, 0, 800, 480), new Rectangle?(new Rectangle(0, 0, 800, 480)), Color.White);
              if (this.inExit)
                this.spriteBatch.Draw(this.menu_exit.intex, new Vector2(this.pos1.X - (float) (this.menu_exit.intex.Width / 10), this.pos1.Y - (float) (this.menu_exit.intex.Height / 10)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0.0f);
              else
                this.spriteBatch.Draw(this.menu_exit.intex, this.pos1, Color.White);
              if (this.inPurchase)
                this.spriteBatch.Draw(this.menu_purchase.intex, new Vector2(this.pos2.X - (float) (this.menu_purchase.intex.Width / 10), this.pos2.Y - (float) (this.menu_purchase.intex.Height / 10)), new Rectangle?(), Color.White, 0.0f, Vector2.Zero, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0.0f);
              else
                this.spriteBatch.Draw(this.menu_purchase.intex, this.pos2, Color.White);
              this.spriteBatch.End();
              return;
            }
            if ((double) this.timeout > 0.0)
            {
              this.spriteBatch.Begin();
              if ((double) this.timeout < 0.25)
                this.spriteBatch.Draw(this.hbLogoTexture.intex, new Vector2(0.0f, 0.0f), new Rectangle?(), Color.White, MathHelper.ToRadians(0.0f), new Vector2(0.0f, 0.0f), new Vector2(1.66666663f, 1.5f), SpriteEffects.None, 0.0f);
              else
                this.spriteBatch.Draw(this.bobsCousinIt, new Vector2(0.0f, 0.0f), Color.White);
              this.fm.Draw(this.graphics.GraphicsDevice, this.spriteBatch);
              this.spriteBatch.End();
              return;
            }
            if (TheGame.setup_attribute)
            {
              if (TheGame.setup_thread == null)
              {
                TheGame.ca = new ConnectionAcquire();
                TheGame.setup_thread = new Thread(new ThreadStart(TheGame.ca.DoWork));
                TheGame.setup_thread.Start();
              }
              else if (TheGame.ca.IsComplete())
              {
                TheGame.setup_attribute = false;
                TheGame.PE_SetupAttribute();
                //TheGame.PE_SetupAttributeMGS(FruitNinja.Game.isWP7TrialMode().ToString(), (PhoneApplicationService.Current.StartupMode == 0).ToString(), TheGame.ca.GetInterfaceType(), DeviceExtendedProperties.GetValue("DeviceManufacturer").ToString(), DeviceExtendedProperties.GetValue("DeviceName").ToString(), DeviceExtendedProperties.GetValue("DeviceFirmwareVersion").ToString(), DeviceExtendedProperties.GetValue("DeviceHardwareVersion").ToString(), DeviceExtendedProperties.GetValue("DeviceTotalMemory").ToString(), CultureInfo.CurrentUICulture.Name, TimeZoneInfo.Local.BaseUtcOffset.TotalHours.ToString(), TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours.ToString(), "");
              }
            }
            if (TheGame.exceptionThrown)
            {
              base.Draw(gameTime);
              return;
            }
            try
            {
              if (this.load_failed || this.init_failed || this.quit_game)
                return;
              if (TheGame.initupdates == 0)
              {
                if ((double) TheGame.loadinScreen < 5.0)
                  TheGame.loadinScreen += 0.0166666675f;
                float timeSinceLastUpdate = 0.0166666675f * TheGame.dtCo;
                DisplayManager.GetInstance().BeginFrame();
                TheGame.fgame.Draw(timeSinceLastUpdate);
                if (FruitNinja.Game.isWP7TrialMode())
                  AchievementManager.UpdatePretendAchievements(this.spriteBatch);
                AchievementManager.UpdateUnlockDropdown(this.spriteBatch);
              }
              if ((double) TheGame.loadinScreen < 0.5)
              {
                float num = TheGame.loadinScreen * 2f;
                this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                this.spriteBatch.Draw(this.hbLogoTexture.intex, new Vector2(400f, 240f), new Rectangle?(), new Color(1f, 1f, 1f, 1f - num), MathHelper.ToRadians(0.0f), new Vector2(240f, 160f), new Vector2(1.66666663f, 1.5f) * (1f + num), SpriteEffects.None, 0.0f);
                this.spriteBatch.End();
              }
              base.Draw(gameTime);
              return;
            }
            catch (Exception ex)
            {
              this.GameException();
              Exception innerException = ex.InnerException;
              while (innerException != null)
                innerException = innerException.InnerException;
              string text = TheGame.ERROR_MESSAGE_GAME();
              string[] buttons = new string[1]
              {
                this.ERROR_BUTTON_1
              };
              while (Guide.IsVisible)
                Thread.Sleep(32 /*0x20*/);
              if (text.Length > (int) byte.MaxValue)
                text = text.Substring(0, 250);
              Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.IngameException), (object) null);
              return;
            }
          }
        }
        try
        {
          if (TheGame.m_fruitTex != null && TheGame.m_ninjaTex != null && TheGame.m_headTex != null)
          {
            if (TheGame.m_bodyTex != null)
              goto label_7;
          }
          TheGame.m_fruitTex = this.Content.Load<Texture2D>("textureswp7/fruit_text.tga");
          TheGame.m_ninjaTex = this.Content.Load<Texture2D>("textureswp7/ninja_text.tga");
          TheGame.m_headTex = this.Content.Load<Texture2D>("textureswp7/sensei_head_02.tga");
          TheGame.m_bodyTex = this.Content.Load<Texture2D>("textureswp7/sensei_body_02.tga");
        }
        catch (Exception ex)
        {
          TheGame.m_fruitTex = (Texture2D) null;
          TheGame.m_ninjaTex = (Texture2D) null;
          TheGame.m_headTex = (Texture2D) null;
          TheGame.m_bodyTex = (Texture2D) null;
        }
    label_7:
        this.GraphicsDevice.Clear(Color.Black);
        if (TheGame.m_fruitTex != null && TheGame.m_ninjaTex != null && TheGame.m_headTex != null && TheGame.m_bodyTex != null && this.spriteBatch != null)
        {
          this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
          this.spriteBatch.Draw(TheGame.m_fruitTex, new Rectangle(280, 260, TheGame.m_fruitTex.Width, TheGame.m_fruitTex.Height), Color.White);
          this.spriteBatch.Draw(TheGame.m_ninjaTex, new Rectangle(240 /*0xF0*/ + TheGame.m_fruitTex.Width + 40, 310, TheGame.m_ninjaTex.Width, TheGame.m_ninjaTex.Height), Color.White);
          this.spriteBatch.Draw(TheGame.m_bodyTex, new Rectangle(80 /*0x50*/, 220, TheGame.m_bodyTex.Width, TheGame.m_bodyTex.Height), Color.White);
          this.spriteBatch.Draw(TheGame.m_headTex, new Rectangle(156, 244, TheGame.m_headTex.Width, TheGame.m_headTex.Height), Color.White);
          this.spriteBatch.End();
        }
        base.Draw(gameTime);
      }

      private void SwitchToLanguage(StringTableUtils.Language switchToLanguage)
      {
        this.SwitchToLanguage(switchToLanguage, false);
      }

      private void SwitchToLanguage(StringTableUtils.Language switchToLanguage, bool bReload)
      {
        FruitNinja.Game.game_work.SetLanguage(switchToLanguage);
        switch (switchToLanguage)
        {
          case StringTableUtils.Language.LANGUAGE_ENGLISH_UK:
            StringManager.GetInstance().SetDefaultLanguage("english_uk");
            break;
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            StringManager.GetInstance().SetDefaultLanguage("french");
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            StringManager.GetInstance().SetDefaultLanguage("spanish");
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            StringManager.GetInstance().SetDefaultLanguage("german");
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            StringManager.GetInstance().SetDefaultLanguage("italian");
            break;
          case StringTableUtils.Language.LANGUAGE_CHINESE_TRADITIONAL:
            StringManager.GetInstance().SetDefaultLanguage("traditional_chinese");
            break;
          case StringTableUtils.Language.LANGUAGE_CHINESE_SIMPLIFIED:
            StringManager.GetInstance().SetDefaultLanguage("chinese");
            break;
          default:
            StringManager.GetInstance().SetDefaultLanguage("english_us");
            break;
        }
        TheGame.instance.stringTable.UpdateDefaultLanguage();
        if (!bReload)
          return;
        StringTableUtils.FontFamily fontFamily = StringTableUtils.GetFontFamily(FruitNinja.Game.game_work.language);
        FruitNinja.Game.game_work.pGameFont = MTLocalisation.GetCachedFont(fontFamily);
        TheGame.SaveConfig();
        TextureManager.GetInstance().ReloadLocalisedTextures(FruitNinja.Game.game_work.language);
      }

      private void GameException() => TheGame.exceptionThrown = true;

     // private void Current_Deactivated(object sender, DeactivatedEventArgs e) => TheGame.ts = true;

      protected override void OnExiting(object sender, EventArgs args)
      {
        TheGame.analytics.Save();
        TheGame.PE_TeardownAttribute();
        TheGame.PE_GameInformation();
        try
        {
          if (!this.load_failed && !this.init_failed)
          {
            if (!this.quit_game)
            {
              try
              {
                GameTask.SaveCurrentData(true);
              }
              catch
              {
                if ((double) this.timeout < 9.9999999747524271E-07)
                  throw;
              }
            }
          }
          base.OnExiting(sender, args);
        }
        catch (Exception ex)
        {
          Exception innerException = ex.InnerException;
          while (innerException != null)
            innerException = innerException.InnerException;
          string text = TheGame.ERROR_MESSAGE(2147495936U /*0x80003000*/);
          string[] buttons = new string[1]
          {
            this.ERROR_BUTTON_1
          };
          while (Guide.IsVisible)
            Thread.Sleep(32 /*0x20*/);
          if (text.Length > (int) byte.MaxValue)
            text = text.Substring(0, 250);
          Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        }
      }

      protected override void OnActivated(object sender, EventArgs args)
      {
        try
        {
          FruitNinja.Game.SetTrialModeState();
          Thread.Sleep(70);
          FruitNinja.Game.SetTrialModeState();
          SoundManager.GetInstance().SetSFXVolume(FruitNinja.Game.game_work.soundEnabled ? SoundDef.DEFAULT_SFX_VOL : 0.0f);
          base.OnActivated(sender, args);
        }
        catch (Exception ex)
        {
          Exception innerException = ex.InnerException;
          while (innerException != null)
            innerException = innerException.InnerException;
          string text = TheGame.ERROR_MESSAGE(2147500032U /*0x80004000*/);
          string[] buttons = new string[1]
          {
            this.ERROR_BUTTON_1
          };
          while (Guide.IsVisible)
            Thread.Sleep(32 /*0x20*/);
          if (text.Length > (int) byte.MaxValue)
            text = text.Substring(0, 250);
          Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        }
      }

      protected override void OnDeactivated(object sender, EventArgs args)
      {
        try
        {
          SoundManager.GetInstance().SetSFXVolume(0.0f);
          FruitNinja.Game.OnActivate();
          base.OnDeactivated(sender, args);
          TheGame.SaveConfig();
        }
        catch (Exception ex)
        {
          Exception innerException = ex.InnerException;
          while (innerException != null)
            innerException = innerException.InnerException;
          string text = TheGame.ERROR_MESSAGE(2147504128U /*0x80005000*/);
          string[] buttons = new string[1]
          {
            this.ERROR_BUTTON_1
          };
          while (Guide.IsVisible)
            Thread.Sleep(32 /*0x20*/);
          if (text.Length > (int) byte.MaxValue)
            text = text.Substring(0, 250);
          Guide.BeginShowMessageBox(this.ERROR_TITLE_1, text, (IEnumerable<string>) buttons, 0, MessageBoxIcon.Alert, new AsyncCallback(this.InitOrLoadFailedCallback), (object) null);
        }
      }

      public static string NotSignedInMessage()
      {
        string str;
        switch (FruitNinja.Game.game_work.language)
        {
          case StringTableUtils.Language.LANGUAGE_FRENCH:
            str = "Vous n'êtes pas connecté à XBOX live.";
            break;
          case StringTableUtils.Language.LANGUAGE_SPANISH:
            str = "No has iniciado sesión en XBOX Live.";
            break;
          case StringTableUtils.Language.LANGUAGE_GERMAN:
            str = "Du bist nicht auf XBOX Live angemeldet.";
            break;
          case StringTableUtils.Language.LANGUAGE_ITALIAN:
            str = "Non hai effettuato l'accesso a XBOX live.";
            break;
          default:
            str = "You are not signed in to XBOX live.";
            break;
        }
        return str;
      }

      public class Analytics
      {
        public int games_trial;
        public int games_full;
        public int games_classic;
        public int games_arcade;
        public int games_zen;
        public int hour;
        public int min;
        public int week;
        public int games_week;
        private string titlesPlayed = "Unknown";
        private string totalAchievements = "Unknown";
        private string region = "Unknown";

        public void Load()
        {
          IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
          IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
          try
          {
            if (!storeForApplication.DirectoryExists(TheGame.CFG_DIRECTORY))
              storeForApplication.CreateDirectory(TheGame.CFG_DIRECTORY);
            if (storeForApplication.FileExists(TheGame.ANA_FILENAME))
            {
              storageFileStream = storeForApplication.OpenFile(TheGame.ANA_FILENAME, FileMode.Open);
              int length = (int) storageFileStream.Length;
              byte[] buffer = new byte[length];
              storageFileStream.Read(buffer, 0, length);
              storageFileStream.Close();
              int startIndex1 = 0;
              this.games_trial = BitConverter.ToInt32(buffer, startIndex1);
              int startIndex2 = startIndex1 + 4;
              this.games_full = BitConverter.ToInt32(buffer, startIndex2);
              int startIndex3 = startIndex2 + 4;
              this.games_classic = BitConverter.ToInt32(buffer, startIndex3);
              int startIndex4 = startIndex3 + 4;
              this.games_arcade = BitConverter.ToInt32(buffer, startIndex4);
              int startIndex5 = startIndex4 + 4;
              this.games_zen = BitConverter.ToInt32(buffer, startIndex5);
              int num1 = startIndex5 + 4;
              this.hour = DateTime.Now.Hour;
              int num2 = num1 + 4;
              this.min = DateTime.Now.Minute;
              int startIndex6 = num2 + 4;
              this.week = BitConverter.ToInt32(buffer, startIndex6);
              int startIndex7 = startIndex6 + 4;
              this.games_week = BitConverter.ToInt32(buffer, startIndex7);
              int num3 = startIndex7 + 4;
              int num4 = DateTime.Now.DayOfYear / 7;
              if (this.week != num4)
              {
                this.week = num4;
                this.games_week = 0;
              }
              ++this.games_week;
              this.DebugPrint();
            }
            else
              this.Save();
          }
          catch
          {
          }
          finally
          {
            storageFileStream?.Close();
          }
        }

        public void Save()
        {
          IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
          IsolatedStorageFileStream storageFileStream = (IsolatedStorageFileStream) null;
          try
          {
            if (!storeForApplication.DirectoryExists(TheGame.CFG_DIRECTORY))
              storeForApplication.CreateDirectory(TheGame.CFG_DIRECTORY);
            storageFileStream = storeForApplication.OpenFile(TheGame.ANA_FILENAME, FileMode.Create);
            if (storageFileStream == null)
              return;
            byte[] bytes1 = BitConverter.GetBytes(this.games_trial);
            storageFileStream.Write(bytes1, 0, 4);
            byte[] bytes2 = BitConverter.GetBytes(this.games_full);
            storageFileStream.Write(bytes2, 0, 4);
            byte[] bytes3 = BitConverter.GetBytes(this.games_classic);
            storageFileStream.Write(bytes3, 0, 4);
            byte[] bytes4 = BitConverter.GetBytes(this.games_arcade);
            storageFileStream.Write(bytes4, 0, 4);
            byte[] bytes5 = BitConverter.GetBytes(this.games_zen);
            storageFileStream.Write(bytes5, 0, 4);
            byte[] bytes6 = BitConverter.GetBytes(this.hour);
            storageFileStream.Write(bytes6, 0, 4);
            byte[] bytes7 = BitConverter.GetBytes(this.min);
            storageFileStream.Write(bytes7, 0, 4);
            byte[] bytes8 = BitConverter.GetBytes(this.week);
            storageFileStream.Write(bytes8, 0, 4);
            byte[] bytes9 = BitConverter.GetBytes(this.games_week);
            storageFileStream.Write(bytes9, 0, 4);
            storageFileStream.Close();
          }
          catch
          {
          }
          finally
          {
            storageFileStream?.Close();
          }
        }

        public IDictionary<string, string> GetAnalyticsData()
        {
          int total = this.games_classic + this.games_arcade + this.games_zen;
          return (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Version",
              FruitNinja.Game.isWP7TrialMode() ? "Trial" : "Full"
            },
            {
              "TrialGames",
              this.games_trial.ToString()
            },
            {
              "FullGames",
              this.games_full.ToString()
            },
            {
              "ClassicTotal",
              this.games_classic.ToString()
            },
            {
              "ArcadeTotal",
              this.games_arcade.ToString()
            },
            {
              "ZenTotal",
              this.games_zen.ToString()
            },
            {
              "ClassicPercent",
              total > 0 ? this.PercentageOfTotal(this.games_classic, total).ToString() + "%" : "0%"
            },
            {
              "ArcadePercent",
              total > 0 ? this.PercentageOfTotal(this.games_arcade, total).ToString() + "%" : "0%"
            },
            {
              "ZenPercent",
              total > 0 ? this.PercentageOfTotal(this.games_zen, total).ToString() + "%" : "0%"
            },
            {
              "Time",
              $"{this.hour.ToString("00")}:{this.min.ToString("00")}"
            },
            {
              "Weekly",
              $"{this.week.ToString("00")}/{this.games_week.ToString()}"
            },
            {
              "TitlesPlayed",
              this.titlesPlayed
            },
            {
              "TotalAchievements",
              this.totalAchievements
            },
            {
              "Region",
              this.region
            }
          };
        }

        private int PercentageOfTotal(int amount, int total)
        {
          return (int) (float) ((double) amount / (double) total * 100.0);
        }

        public void DebugPrint()
        {
        }

        public void SetPrivileged()
        {
        }
      }

      public class Settings
      {
        public int magic1;
        public int magic2;
        public int language;
        public int week;
        public int tf;
        public int aw;
        public int bestThisWeek;
        public uint[] achievements;

        public Settings()
        {
          this.magic1 = 195800833;
          this.magic2 = 33479857;
          this.language = 1;
          this.week = -1;
          this.tf = 0;
          this.aw = 0;
          this.bestThisWeek = 0;
          this.achievements = new uint[FruitNinja.Game.MAX_SYSTEM_ACHIEVEMENTS];
          for (int index = 0; index < FruitNinja.Game.MAX_SYSTEM_ACHIEVEMENTS; ++index)
            this.achievements[index] = 0U;
        }
      }

      public struct inputInfo
      {
        public int updateStamp;
        public uint tid;
        public bool dn;
        public float x;
        public float y;
      }

      public class MyTouchState
      {
        public Vector2 lastPoint;
        public Vector2 midPoint1;
        public Vector2 midPoint2;
        public Vector2 midPoint3;
        public Vector2 movedToPoint;
        public int id;
        public bool MovedThisFrame;
        public bool deletedThisFrame;
        public int moves;
      }
    }
}
