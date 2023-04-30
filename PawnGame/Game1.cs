global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.IO;
using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using static PawnGame.GameObjects.Enemies.EnemyManager;
using static PawnGame.VirtualMouse;

namespace PawnGame
{
    public class Game1 : Game
    {
        public const int RenderTargetWidth = 1920;
        public const int RenderTargetHeight = 1080;

        #region enums
        /// <summary>
        /// Represents the current screen that the game is on
        /// </summary>
        public enum GameState
        {
            Menu,
            Game,
            LevelEditor,
            Victory,
            DebugMenu
        }

        /// <summary>
        /// Represents the various graphical assets used
        /// </summary>
        public enum AssetNames
        {
            //UI
            GameLogo,
            IconSave,
            IconLoad,
            /*ButtonUp,
            ButtonDown,
            ButtonLeft,
            ButtonRight,*/

            //Tiles
            TileBlack,
            TileWhite,
            WallBlack,
            WallWhite,
            HoleBlack,
            HoleWhite,
            ExitBlack,
            ExitWhite,
            DoorBlack,
            DoorWhite,

            //Pieces
            PawnBlack,
            PawnWhite,

            //Weapons
            WeaponSword,

            //Debug
            DebugError,

            //Abilities
            AbilityDash,

            //UI
            SpacebarActive,
            SpacebarInactive,
            StartGame,
            LoadGame,
            LevelEditor,
            Crosshair,
        }
        #endregion

        #region fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private float _scale = 0;


        #region GameStates and level
        private static GameState s_gameState;
        private static GameState _prevGameState;
        private static Level[] s_levels;
        private static int s_levelIndex;
        #endregion

        #region Keyboard and mouse states
        private KeyboardState _currKbState;
        private KeyboardState _prevKbState;
        private MouseState _currMouseState;
        private MouseState _prevMouseState;
        #endregion

        private int _prevWidth;
        private int _prevHeight;

        private LevelEditor _levelEditor;

        //Entities
        private static Player s_player;
        private Texture2D _heldAbilityTexture;
        private Weapon _weapon;
        private int _playerScale;

        // Menu buttons
        private List<Button> _menuButtons = new List<Button>();
        private List<Button> _debugButtons = new List<Button>();

        // UI things
        private int _spacebarBlinkFrames = 0;
        private bool _spacebarActive;

        // This font is temporary
        // Will use for creating menu skeleton
        // for implementation of clicking from menu to game
        private SpriteFont _font;
        #endregion

        #region properties
        /// <summary>
        /// Dictionary containing all assets used for the game
        /// </summary>
        public static Dictionary<AssetNames, Texture2D> Assets;

        /// <summary>
        /// the currently active level
        /// </summary>
        public static Level CurrentLevel
        {
            get { return s_levels[s_levelIndex]; }
        }

        public static GameState State
        {
            get
            {
                return s_gameState;
            }
            set
            {
                s_gameState = value;
            }
        }

        public static int LevelIndex
        {
            get
            {
                return s_levelIndex;
            }
            set
            {
                if (value >= s_levels.Length) throw new ArgumentOutOfRangeException("There is no level at that index.");
                s_levelIndex = value;
            }
        }

        /// <summary>
        /// the player object
        /// </summary>
        public static Player Player
        {
            get { return s_player; }
        }

        /// <summary>
        /// Gets the width of the window
        /// </summary>
        public int WindowWidth
        {
            get { return Window.ClientBounds.Width; }
        }

        /// <summary>
        /// Gets the height of the window
        /// </summary>
        public int WindowHeight
        {
            get { return Window.ClientBounds.Height; }
        }

        public float Scale => _scale;

        #endregion

        public Game1()
        {
            _prevWidth = 0;
            _prevHeight = 0;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region Screen and graphics size setup
            _renderTarget = new(GraphicsDevice, RenderTargetWidth, RenderTargetHeight);

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _prevWidth = _graphics.PreferredBackBufferWidth;
            _prevHeight = _graphics.PreferredBackBufferHeight;
            #endregion

            _prevKbState = Keyboard.GetState();
            Assets = new Dictionary<AssetNames, Texture2D>();
            s_levelIndex = 0;
            _playerScale = 2;
            _spacebarActive = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Arial");

            #region load textures
            Assets.Add(AssetNames.GameLogo, Content.Load<Texture2D>("logo"));
            Assets.Add(AssetNames.IconSave, Content.Load<Texture2D>("IconSave"));
            Assets.Add(AssetNames.IconLoad, Content.Load<Texture2D>("IconLoad"));
            Assets.Add(AssetNames.PawnBlack, Content.Load<Texture2D>("PawnBlack"));
            Assets.Add(AssetNames.PawnWhite, Content.Load<Texture2D>("PawnWhite"));
            Assets.Add(AssetNames.TileBlack, Content.Load<Texture2D>("TileBlack"));
            Assets.Add(AssetNames.TileWhite, Content.Load<Texture2D>("TileWhite"));
            Assets.Add(AssetNames.WallBlack, Content.Load<Texture2D>("WallBlack"));
            Assets.Add(AssetNames.WallWhite, Content.Load<Texture2D>("WallWhite"));
            Assets.Add(AssetNames.HoleBlack, Content.Load<Texture2D>("HoleBlack"));
            Assets.Add(AssetNames.HoleWhite, Content.Load<Texture2D>("HoleWhite"));
            Assets.Add(AssetNames.ExitBlack, Content.Load<Texture2D>("ExitBlack"));
            Assets.Add(AssetNames.ExitWhite, Content.Load<Texture2D>("ExitWhite"));
            Assets.Add(AssetNames.DoorBlack, Content.Load<Texture2D>("DoorBlack"));
            Assets.Add(AssetNames.DoorWhite, Content.Load<Texture2D>("DoorWhite"));
            Assets.Add(AssetNames.WeaponSword, Content.Load<Texture2D>("Sword"));
            Assets.Add(AssetNames.AbilityDash, Content.Load<Texture2D>("AbilityDash"));
            Assets.Add(AssetNames.SpacebarActive, Content.Load<Texture2D>("SpacebarActive"));
            Assets.Add(AssetNames.SpacebarInactive, Content.Load<Texture2D>("SpacebarInactive"));
            Assets.Add(AssetNames.DebugError, Content.Load<Texture2D>("Error"));
            Assets.Add(AssetNames.StartGame, Content.Load<Texture2D>("StartGame"));
            Assets.Add(AssetNames.LoadGame, Content.Load<Texture2D>("LoadGame"));
            Assets.Add(AssetNames.LevelEditor, Content.Load<Texture2D>("LevelEditor"));
            /*Assets.Add(AssetNames.ButtonUp, Content.Load<Texture2D>("ButtonUp"));
            Assets.Add(AssetNames.ButtonDown, Content.Load<Texture2D>("ButtonDown"));
            Assets.Add(AssetNames.ButtonLeft, Content.Load<Texture2D>("ButtonLeft"));
            Assets.Add(AssetNames.ButtonRight, Content.Load<Texture2D>("ButtonRight"));*/
            Assets.Add(AssetNames.Crosshair, Content.Load<Texture2D>("Crosshair"));
            #endregion

            _weapon = new Weapon(AssetNames.WeaponSword, new Rectangle(RenderTargetWidth / 2, RenderTargetHeight / 2, Assets[AssetNames.WeaponSword].Width, Assets[AssetNames.WeaponSword].Height));
            VMouse.SetCrosshair = Assets[AssetNames.Crosshair];
            s_player = new Player(AssetNames.PawnBlack, new Rectangle(RenderTargetWidth / 2, RenderTargetHeight / 2, Assets[AssetNames.PawnBlack].Width/_playerScale, Assets[AssetNames.PawnBlack].Height/ _playerScale), _weapon);
            _heldAbilityTexture = null!;
            //initialize level editor (needs textures loaded)
            _levelEditor = new LevelEditor(this);

            //initialize and load the level array
            LoadLevels();

            #region Add Menu buttons
            #region Add main menu buttons
            // Add menue buttons
            _menuButtons.Add(new(Assets[AssetNames.StartGame],
                    new Vector2(RenderTargetWidth / 2 - (Assets[AssetNames.StartGame].Width/4)/2, RenderTargetHeight - 200), 
                    Assets[AssetNames.StartGame].Width / 4, Assets[AssetNames.StartGame].Height / 4,
                Color.LightGray));
            _menuButtons.Add(new(Assets[AssetNames.LoadGame],
                    new Vector2(RenderTargetWidth / 2 - (Assets[AssetNames.LoadGame].Width/4)/2, RenderTargetHeight - 150),
                    Assets[AssetNames.LoadGame].Width / 4, Assets[AssetNames.LoadGame].Height / 4,
                    Color.LightGray));
            _menuButtons.Add(new(Assets[AssetNames.LevelEditor],
                    new Vector2(RenderTargetWidth / 2 - (Assets[AssetNames.LevelEditor].Width/4)/2, RenderTargetHeight - 100),
                    Assets[AssetNames.LevelEditor].Width / 4, Assets[AssetNames.LevelEditor].Height / 4,
                    Color.LightGray));
            #endregion
            #region Add debug menu buttons
            _debugButtons.Add(new(_font, "Menu",
                        new Vector2(RenderTargetWidth / 4 - _font.MeasureString("Menu").X / 2, RenderTargetHeight / 2),
                        Color.LightGray));

            _debugButtons.Add(new(_font, "Game",
                new Vector2(RenderTargetWidth / 2 - _font.MeasureString("Game").X / 2, RenderTargetHeight / 4 + _font.MeasureString("Game").Y / 2),
                Color.LightGray));

            _debugButtons.Add(new(_font, "Level Editor",
                    new Vector2(RenderTargetWidth - RenderTargetWidth / 4 - _font.MeasureString("Level Editor").X / 2, RenderTargetHeight / 2),
                    Color.LightGray));
            #if DEBUG
            _debugButtons.Add(new(_font, "Victory",
                new Vector2(RenderTargetWidth / 2 - _font.MeasureString("Victory").X / 2, ((RenderTargetHeight / 4) + RenderTargetHeight / 2) - _font.MeasureString("Victory").Y / 2),
                Color.LightGray));
            #endif
            #endregion
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {


            _currKbState = Keyboard.GetState();
            _currMouseState = Mouse.GetState();

            // Toggling fullscreen
            // Kinda wanna make it a button in an options menu somewhere instead
            // of F11
            if (_currKbState.IsKeyDown(Keys.F11) && _prevKbState.IsKeyUp(Keys.F11))
            {
                ToggleFullscreen();
            }

            switch (s_gameState)
            {
                #region Menu State
                case GameState.Menu:

                    IsMouseVisible = true;

                    for (int i = 0; i < _menuButtons.Count; i++)
                    {
                        _menuButtons[i].Update(_scale);
                    }

                    // Updating the states depending on what button is clicked
                    // or what key is pressed
                    for (int i = 0; i < _menuButtons.Count; i++)
                    {
                        if (_menuButtons[i].Clicked)
                        {
                            if (i == 0)
                            {
                                // Start a new game
                                // (whatever that means)
                                LoadLevels();
                                s_player.HeldAbility = Player.Ability.None;
                                Mouse.SetPosition(WindowWidth / 2, WindowHeight / 2);
                                s_gameState = GameState.Game;
                                break;
                            }
                            else if (i == 1)
                            {
                                // Load a level from a file
                                s_gameState = GameState.Game;
                                break;
                            }
                            else
                            {
                                // Open the level editor
                                s_gameState = GameState.LevelEditor;
                                break;
                            }
                        }
                    }

                    #endregion
                    break;
#if DEBUG
                #region DebugMenu State
                case GameState.DebugMenu:

                    IsMouseVisible = true;

                    // If the user presses escape, goes back to the previous state
                    if (_currKbState.IsKeyDown(Keys.Escape) && _prevKbState.IsKeyUp(Keys.Escape))
                    {
                        _debugButtons[(int)_prevGameState].Enabled = true;
                        s_gameState = _prevGameState;
                        _prevGameState = GameState.DebugMenu;
                    }

                    if (_prevGameState != GameState.DebugMenu)
                    {
                        _debugButtons[(int)_prevGameState].Enabled = false;
                    }

                    for (int i = 0; i < _debugButtons.Count; i++)
                    {
                        _debugButtons[i].Update(_scale);

                        if (_debugButtons[i].Clicked)
                        {
                            _debugButtons[(int)_prevGameState].Enabled = true;
                            s_gameState = (GameState)i;

                            // Resetting the level editor if the level editor was exited out of
                            if (i == 2 && _prevGameState != (GameState)2)
                            {
                                _levelEditor = new LevelEditor(this);
                            }

                            _prevGameState = GameState.DebugMenu;

                            break;
                        }
                    }

                    #endregion
                    break;
#endif
                #region Game State
                case GameState.Game:

#if DEBUG
                    //Debug level skip
                    if (_currKbState.IsKeyDown(Keys.X) && _prevKbState.IsKeyUp(Keys.X))
                    {
                        s_levelIndex++;
                        break;
                    }
#endif

                    IsMouseVisible = false;

                    //Virtual mouse stuff
                    
                    //Vmouse has to update virst, and weapon has to update after player

                    
                    
                    s_player.Update(_currKbState, _prevKbState,_currMouseState,_prevMouseState);
                    VMouse.Update(Mouse.GetState(), s_player, _scale);
                    _weapon.Update(s_player,VMouse);
                    Manager.Update(s_player);

                    if (!s_player.IsAlive)
                    {
                        LoadLevels();
                    }

                    switch (s_player.HeldAbility)
                    {
                        case Player.Ability.Pawn:
                            _heldAbilityTexture = Assets[AssetNames.AbilityDash];
                            break;
                        default:
                            _heldAbilityTexture = null!;
                            break;
                    }

                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:
                    IsMouseVisible = true;
                    // Update logic for level editor
                    _levelEditor.Update();
                    #endregion
                    break;

                #region Victory State
                case GameState.Victory:
                    // Check for buttons clicked to go back to menu
                    #endregion
                    break;
            }

            // Regardless of state, you can get to the debug menus
            if (_currKbState.IsKeyDown(Keys.Escape) && _prevKbState.IsKeyUp(Keys.Escape) && s_gameState != GameState.DebugMenu)
            {
                _prevGameState = s_gameState;
                s_gameState = GameState.DebugMenu;
            }

            _prevMouseState = _currMouseState;
            _prevKbState = _currKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) 
        {
            #region Sets up spritebatch to draw to the render target
            _scale = 1 / ((float)_renderTarget.Height / GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.Clear(Color.Black);
            #endregion

            _spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

            switch (s_gameState)
            {
                #region Menu State
                case GameState.Menu:
                    // Menu skeleton containing the buttons
                    // that will be able to be clicked

                    // Drawing the logo to the screen
                    // Note: Doesn't scale properly on fullscreen
                    _spriteBatch.Draw(Assets[AssetNames.GameLogo],
                        new Vectangle((RenderTargetWidth / 4), (RenderTargetHeight / 20), (RenderTargetWidth / 2), (RenderTargetHeight)),
                        Color.White);

                    foreach (Button b in _menuButtons)
                    {
                        b.Draw(_spriteBatch);
                    }
                    #endregion
                    break;
#if DEBUG
                #region DebugMenu State
                case GameState.DebugMenu:
                    // Draw debug menu on top of last frame of previous
                    // state
                    // Replace these tests later obviously
                    _spriteBatch.DrawString(_font, "Menu",
                        new Vector2(RenderTargetWidth / 2 - _font.MeasureString("Menu").X / 2, RenderTargetHeight / 2 - _font.MeasureString("Menu").Y / 2), Color.Red);

                    foreach(Button b in _debugButtons)
                    {
                        b.Draw(_spriteBatch);
                    }

                    
                    #endregion
                    break;
#endif
                #region Game State
                case GameState.Game:
                    // Draw.. the game?
                    CurrentLevel.ActiveRoom.Draw(_spriteBatch);
                    s_player.Draw(_spriteBatch);
                    Manager.Draw(_spriteBatch);
                    CurrentLevel.Draw(_spriteBatch);
                    VMouse.Draw(_spriteBatch,_scale);
                    _weapon.Draw(_spriteBatch, s_player, VMouse.Rotation, _scale);

                    //UI stuff
                    Vector2 UIPos = new Vector2(0 + CurrentLevel.ActiveRoom.Location.X / 2,
                                (CurrentLevel.ActiveRoom.Location.Y + CurrentLevel.ActiveRoom.Height) / 2 + 50);

                    _spriteBatch.DrawString(_font, "Ability:", new Vector2(UIPos.X - _font.MeasureString("Ability:").X /2,
                        UIPos.Y - 50), Color.White);

#if DEBUG
                    _spriteBatch.DrawString(_font, "Speed sign: " + Math.Sign(VMouse.Speed), new Vector2(UIPos.X - _font.MeasureString("Ability:").X,
                        UIPos.Y), Color.White);
                    _spriteBatch.DrawString(_font, "Rotation: " + VMouse.Rotation, new Vector2(UIPos.X - _font.MeasureString("Ability:").X,
                        UIPos.Y+50), Color.White);
#endif
                    if (_heldAbilityTexture != null!)
                    {
                        string abilityName = _heldAbilityTexture.Name.Substring(7);
                        _spriteBatch.DrawString(_font, $"{abilityName}", new Vector2(UIPos.X - _font.MeasureString(abilityName).X / 2,
                            UIPos.Y + 50), Color.White);

                        // Making the spacebar indicator blink
                        // to make it look more dynamic
                        if (!_spacebarActive)
                        {
                            _spriteBatch.Draw(Assets[AssetNames.SpacebarInactive], 
                                new Rectangle((int)UIPos.X - Assets[AssetNames.SpacebarInactive].Width / 8, (int)UIPos.Y + 100,
                                Assets[AssetNames.SpacebarInactive].Width / 4, Assets[AssetNames.SpacebarInactive].Height/4), Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(Assets[AssetNames.SpacebarActive], 
                                new Rectangle((int)UIPos.X - Assets[AssetNames.SpacebarActive].Width / 8, (int)UIPos.Y + 100,
                                Assets[AssetNames.SpacebarActive].Width / 4, Assets[AssetNames.SpacebarActive].Height / 4), Color.White);
                        }

                        _spacebarBlinkFrames++;
                        if (_spacebarBlinkFrames >= 10)
                        {
                            _spacebarActive = !(_spacebarActive);
                            _spacebarBlinkFrames = 0;
                        }

                        

                    }
                    else
                    {
                        _spriteBatch.DrawString(_font, $"None", new Vector2(UIPos.X - _font.MeasureString("None").X / 2,
                            UIPos.Y + 100), Color.White);
                    }

                    switch (s_player.HeldAbility)
                    {
                        case Player.Ability.Pawn:
                            _spriteBatch.Draw(_heldAbilityTexture, new Rectangle((int)UIPos.X - (_heldAbilityTexture.Width / _playerScale) / 2, 
                                (int)UIPos.Y, _heldAbilityTexture.Width/_playerScale, _heldAbilityTexture.Height/_playerScale), Color.White);
                            break;

                    }
                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:
                    // Draw level editor interface
                    _levelEditor.Draw(_spriteBatch);
                    #endregion
                    break;

                #region Victory State
                case GameState.Victory:
                    // Draw victory screen
                    _spriteBatch.DrawString(_font, "You Win!",
                        new Vector2(RenderTargetWidth / 2 - _font.MeasureString("You Win!").X / 2, RenderTargetHeight / 2), Color.White);
                    #endregion
                    break;
            }
            
            _spriteBatch.End();

            #region Draws render target to the screen
            //Remove render target as location to draw to
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0);
#if DEBUG
            _spriteBatch.DrawString(_font, Math.Round((1 / gameTime.ElapsedGameTime.TotalSeconds)).ToString(), Vector2.One, Color.White);
#endif
            _spriteBatch.End();
            #endregion

            base.Draw(gameTime);

        }

        /// <summary>
        /// Fullscreens or unfullscreens the window accordingly
        /// </summary>
        private void ToggleFullscreen()
        {
            if (_graphics.IsFullScreen)
            {
                // Changes the width and height back to the original size
                _graphics.PreferredBackBufferWidth = _prevWidth;
                _graphics.PreferredBackBufferHeight = _prevHeight;
            }
            else
            {
                // Stores the width and height of the screen when it is not full screen
                // so that it is easy to revert it
                _prevWidth = Window.ClientBounds.Width;
                _prevHeight = Window.ClientBounds.Height;

                // Updating the width and the height to the resolution of the user's screen
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }

            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// load the next room in the level array, or go to win screen if conditions are met
        /// </summary>
        private void NextLevel()
        {
            Manager.Clear();
            throw new NotImplementedException();
            
        }

        /// <summary>
        /// load all levels and send player to level 1
        /// </summary>
        public void LoadLevels()
        {
            //get all the levels from the levels folder, deserialize and store them
            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Levels");
            if(fileNames.Length == 0)
            {
                s_levels = new Level[1];
                s_levels[0] = new Level(this);
            }
            else
            {
                s_levels = new Level[fileNames.Length];
                for (int i = 0; i < s_levels.Length; i++)
                {
                    s_levels[i] = Level.Load(fileNames[i]);
                }
            }
            s_levelIndex = 0;
            
            

            s_player.X = CurrentLevel.ActiveRoom.SpawnPoint.X;
            s_player.Y = CurrentLevel.ActiveRoom.SpawnPoint.Y;
            Manager.Clear();
            Manager.AddRange(CurrentLevel.ActiveRoom.Enemies);
        }
    }
}