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
        /// Name of assets
        /// </summary>
        public enum AssetNames
        {
            //General stuff
            GameLogo,
            IconSave,
            IconLoad,

            //Tiles
            TileBlack,
            TileWhite,
            WallBlack,
            WallWhite,

            //Pieces
            PawnBlack,
            PawnWhite,

            //Wepons
            WeaponSword,

            //Debug
            DebugError,
        }
        #endregion

        #region fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region GameStates and level
        private GameState _gameState;
        private GameState _prevGameState;
        private Level[] _levels;
        private Level _currLevel;
        private int _prevLevelIndex;
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

        private int testTimer = 300;
        private Random random;

        //Entities
        private Player _player;
        private Weapon _weapon;
        private int _playerScale;

        // Menu buttons
        private List<Button> _menuButtons = new List<Button>();
        private List<Button> _debugButtons = new List<Button>();

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
        /// 
        /// </summary>
        public static int LevelIndex; 


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
            random = new Random();
            _prevKbState = Keyboard.GetState();
            Assets = new Dictionary<AssetNames, Texture2D>();
            LevelIndex = 0;
            _prevLevelIndex = 0;
            _playerScale = 4;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Debug font
            _font = Content.Load<SpriteFont>("Arial");

            //load textures
            Assets.Add(AssetNames.GameLogo, Content.Load<Texture2D>("logo"));
            Assets.Add(AssetNames.IconSave, Content.Load<Texture2D>("IconSave"));
            Assets.Add(AssetNames.IconLoad, Content.Load<Texture2D>("IconLoad"));
            Assets.Add(AssetNames.PawnBlack, Content.Load<Texture2D>("PawnBlack"));
            Assets.Add(AssetNames.PawnWhite, Content.Load<Texture2D>("PawnWhite"));
            Assets.Add(AssetNames.TileBlack, Content.Load<Texture2D>("TileBlack"));
            Assets.Add(AssetNames.TileWhite, Content.Load<Texture2D>("TileWhite"));
            Assets.Add(AssetNames.WallWhite, Content.Load<Texture2D>("WallBlack"));
            Assets.Add(AssetNames.WallBlack, Content.Load<Texture2D>("WallWhite"));
            Assets.Add(AssetNames.WeaponSword, Content.Load<Texture2D>("Sword"));
            Assets.Add(AssetNames.DebugError, Content.Load<Texture2D>("Error"));

            _weapon = new Weapon(AssetNames.WeaponSword, new Rectangle(WindowWidth / 2, WindowHeight / 2, Assets[AssetNames.WeaponSword].Width / 2, Assets[AssetNames.WeaponSword].Height / 2));
            _player = new Player(AssetNames.PawnBlack, new Rectangle(WindowWidth / 2, WindowHeight / 2, Assets[AssetNames.PawnBlack].Width/_playerScale, Assets[AssetNames.PawnBlack].Height/ _playerScale), _weapon);

            //initialize level editor (needs textures loaded)
            _levelEditor = new LevelEditor(8, 8, this);

            //get all the levels from the levels folder, deserialize and store them
            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Levels");
            _levels = new Level[fileNames.Length];
            for(int i = 0; i < _levels.Length; i++)
            {
                _levels[i] = Level.Read(fileNames[i]);
            }
            _currLevel = _levels[0];
            
            
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

            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:

                    IsMouseVisible = true;
                    _menuButtons.Clear();

                    // Adding all 3 buttons on the menu screen
                    // Note: Doing this in update so that their position updates
                    // when  the window is fullscreened
                    _menuButtons.Add(new(_font, "New Game",
                            new Vector2(WindowWidth / 2 - _font.MeasureString("New Game").X / 2, WindowHeight - 100),
                        Color.LightGray));
                    _menuButtons.Add(new(_font, "Load Game",
                            new Vector2(WindowWidth / 2 - _font.MeasureString("Load Game").X / 2, WindowHeight - 75),
                            Color.LightGray));
                    _menuButtons.Add(new(_font, "Level Editor",
                            new Vector2(WindowWidth / 2 - _font.MeasureString("Level Editor").X / 2, WindowHeight - 50),
                            Color.LightGray));

                    // Updating the states depending on what button is clicked
                    // or what key is pressed
                    for (int i = 0; i < _menuButtons.Count; i++)
                    {
                        if (_menuButtons[i].Clicked())
                        {
                            if (i == 0)
                            {
                                // Start a new game
                                // (whatever that means)
                                ResetLevel();
                                NextLevel();
                                Mouse.SetPosition(WindowWidth / 2, WindowHeight / 2);
                                _gameState = GameState.Game;
                            }
                            else if (i == 1)
                            {
                                // Load a level from a file
                                _gameState = GameState.Game;
                            }
                            else
                            {
                                // Open the level editor
                                _gameState = GameState.LevelEditor;
                            }
                        }
                    }

                    #endregion
                    break;

                #region DebugMenu State
                case GameState.DebugMenu:

                    IsMouseVisible = true;

                    // Make a button to each screen
                    // but make the previous one disabled since they were already on it
                    // If the user presses escape, goes back to the previous state
                    if (_currKbState.IsKeyDown(Keys.Escape) && _prevKbState.IsKeyUp(Keys.Escape))
                    {
                        _gameState = _prevGameState;
                        _prevGameState = GameState.DebugMenu;
                    }

                    _debugButtons.Clear();

                    // Decide which button to disable
                    #region Debug Button Adding

                    _debugButtons.Add(new(_font, "Menu",
                        new Vector2(WindowWidth / 4 - _font.MeasureString("Menu").X / 2, WindowHeight / 2),
                        Color.LightGray));
                    

                    _debugButtons.Add(new(_font, "Game",
                        new Vector2(WindowWidth / 2 - _font.MeasureString("Game").X / 2, WindowHeight /2),
                        Color.LightGray));

                    _debugButtons.Add(new(_font, "Level Editor",
                            new Vector2(WindowWidth - WindowWidth / 4 - _font.MeasureString("Level Editor").X / 2, WindowHeight /2),
                            Color.LightGray));

                    _debugButtons.Add(new(_font, "Victory",
                        new Vector2(WindowWidth / 2 - _font.MeasureString("Victory").X / 2, WindowHeight /2 + 100),
                        Color.LightGray));

                    // Looping through to disable the previous screen's button
                    // as it doesn't make sense for the user to click it
                    for (int i = 0; i < _debugButtons.Count; i++)
                    {
                        if (i == (int)_prevGameState)
                        {
                            Button curr = _debugButtons[i];
                            curr.Enabled = false;
                            _debugButtons[i] = curr;

                            break;
                        }
                    }
                    #endregion

                    // Transitions to the corresponding screen
                    for (int i = 0; i < _debugButtons.Count; i++)
                    {
                        if (_debugButtons[i].Clicked())
                        {
                            _gameState = (GameState)i;
                            _prevGameState = GameState.DebugMenu;
                        }
                    }

                    #endregion
                    break;

                #region Game State
                case GameState.Game:

                    IsMouseVisible = false;

                    // Play the game here
                    //TODO: Ask chris how GameTime works
                    testTimer--;
                    if (testTimer <= 0)
                    {
                        //Adds a random pawn, for the demo
                        // Commented for bug testing
                        Manager.Add(new Pawn(AssetNames.PawnWhite, new Rectangle(random.Next(0, 2) * WindowWidth, random.Next(0, 2) * WindowHeight, Assets[AssetNames.PawnWhite].Width/6, Assets[AssetNames.PawnWhite].Height/6)));
                        testTimer = 300;
                    }
                    //Virtual mouse stuff
                    
                    VMouse.Update(Mouse.GetState(), WindowWidth,WindowHeight);

                    Manager.Update(_player);
                    _player.Update(_currKbState, _prevKbState,_currMouseState,_prevMouseState, _currLevel.Tiles);

                    _weapon.Update(_player,VMouse,gameTime);
                    _weapon.Update(_player,VMouse);

                    if (!_player.IsAlive)
                    {
                        ResetLevel();
                    }

                    if (LevelIndex > _prevLevelIndex)
                    {
                        NextLevel();
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
            if (_currKbState.IsKeyDown(Keys.F) && _prevKbState.IsKeyUp(Keys.F) && _gameState != GameState.DebugMenu)
            {
                _prevGameState = _gameState;
                _gameState = GameState.DebugMenu;
            }

            _prevMouseState = _currMouseState;
            _prevKbState = _currKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            _spriteBatch.Begin();
            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:
                    // Menu skeleton containing the buttons
                    // that will be able to be clicked

                    // Drawing the logo to the screen
                    // Note: Doesn't scale properly on fullscreen
                    _spriteBatch.Draw(Assets[AssetNames.GameLogo],
                        new Rectangle(WindowWidth / 4 - Assets[AssetNames.GameLogo].Width / 4,
                        WindowHeight / 4 - Assets[AssetNames.GameLogo].Height / 2,
                        Assets[AssetNames.GameLogo].Width * 2, Assets[AssetNames.GameLogo].Height * 2), Color.White);

                    foreach (Button b in _menuButtons)
                    {
                        b.Draw(_spriteBatch);
                    }
                    #endregion
                    break;

                #region DebugMenu State
                case GameState.DebugMenu:
                    // Draw debug menu on top of last frame of previous
                    // state
                    // Replace these tests later obviously
                    _spriteBatch.DrawString(_font, "Debug Menu",
                        new Vector2(WindowWidth / 2 - _font.MeasureString("Debug Menu").X / 2, 100), Color.White);

                    foreach(Button b in _debugButtons)
                    {
                        b.Draw(_spriteBatch);
                    }

                    
                    #endregion
                    break;

                #region Game State
                case GameState.Game:
                    // Draw.. the game?
                    _currLevel.Draw(_spriteBatch);
                    _player.Draw(_spriteBatch);
                    Manager.Draw(_spriteBatch);
                    _weapon.Draw(_spriteBatch, _player, Mouse.GetState(),WindowWidth,WindowHeight);
                    _weapon.Draw(_spriteBatch, _player, Mouse.GetState(),WindowWidth,WindowHeight);
                    _spriteBatch.DrawString(_font, "X: " + VMouse.X, new Vector2(30, 50), Color.Red);
                    _spriteBatch.DrawString(_font, "Y: " + VMouse.Y, new Vector2(30, 70), Color.Red);
                    _spriteBatch.DrawString(_font, "Speed: " + VMouse.Speed, new Vector2(30, 90), Color.Red);
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
                    _spriteBatch.DrawString(_font, "You win",
                        new Vector2(WindowWidth / 2 - _font.MeasureString("You win").X / 2, WindowHeight / 2), Color.White);
                    #endregion
                    break;
            }

            _spriteBatch.End();
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
        /// 
        /// </summary>
        private void NextLevel()
        {
            if (LevelIndex < _levels.Length)
            {
                _currLevel = _levels[LevelIndex];

                if (LevelIndex > _prevLevelIndex)
                {
                    _prevLevelIndex++;
                }
                
                _player.X = _currLevel.SpawnPoint.X;
                _player.Y = _currLevel.SpawnPoint.Y;

            }
            else
            {
                _gameState = GameState.Victory;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetLevel()
        {
            _currLevel = _levels[0];
            LevelIndex = 0;
            _prevLevelIndex = 0;
        }
    }
}