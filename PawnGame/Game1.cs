global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using PawnGame.GameObjects;

namespace PawnGame
{
    /// <summary>
    /// Represents the current screen that the game is on
    /// </summary>
    public enum GameState
    {
        Menu,
        Game,
        DebugMenu,
        Victory,
        LevelEditor
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _gameState;
        private GameState _prevGameState;
        private Level _currLevel;

        private KeyboardState _currKbState;
        private KeyboardState _prevKbState;

        private int _prevWidth;
        private int _prevHeight;

        private LevelEditor _levelEditor;

        // Textures
        public static Dictionary<string, Texture2D> Textures;
        private Texture2D _logo;

        private Texture2D _playerTexture;

        /// <summary>
        /// Gets the width of the window
        /// </summary>
        public float WindowWidth
        {
            get { return Window.ClientBounds.Width; }
        }

        /// <summary>
        /// Gets the height of the window
        /// </summary>
        public float WindowHeight
        {
            get { return Window.ClientBounds.Height; }
        }

        //Entities
        private Player _player;

        // Menu buttons
        private List<Button> _menuButtons = new List<Button>();

        // This font is temporary
        // Will use for creating menu skeleton
        // for implementation of clicking from menu to game
        private SpriteFont _font;

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
            _prevKbState = Keyboard.GetState();
            Textures = new Dictionary<string, Texture2D>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Debug font
            _font = this.Content.Load<SpriteFont>("Arial");

            //load textures
            _logo = LoadTexture("logo");

            _playerTexture = LoadTexture("Pawn-placeholder");
            _player = new Player(_playerTexture, new Rectangle((int)WindowWidth / 2, (int)WindowHeight / 2,
                _playerTexture.Width/6, _playerTexture.Height/6));

            //initialize level editor (needs textures loaded)
            _levelEditor = new LevelEditor(8, 8, this);
        }

        protected override void Update(GameTime gameTime)
        {
            _currKbState = Keyboard.GetState();

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

                    // Make a button to each screen
                    // but make the previous one disabled since they were already on it
                    // If the user presses escape, goes back to the previous state
                    if (_currKbState.IsKeyDown(Keys.Escape) && _prevKbState.IsKeyUp(Keys.Escape))
                    {
                        _gameState = _prevGameState;
                    }

                    #endregion
                    break;

                #region Game State
                case GameState.Game:

                    // Play the game here
                    _player.Update(_currKbState, _prevKbState);

                    // Make an enemy manager class that handles collisions
                    /*for (int i = 0; i < _currLevel.Enemies.Count; i++)
                    {
                        if (_currLevel.Enemies[i].CheckCollision(_player))
                        {
                            //_currLevel.Enemies[i].
                        }
                    }*/
                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:
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

            // Regardless of state, you can get to the debug menu
            if (_currKbState.IsKeyDown(Keys.F) && _prevKbState.IsKeyUp(Keys.F))
            {
                _prevGameState = _gameState;
                _gameState = GameState.DebugMenu;
            }

            _prevKbState = _currKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:
                    // Menu skeleton containing the buttons
                    // that will be able to be clicked

                    // Drawing the logo to the screen
                    // Note: Doesn't scale properly on fullscreen
                    _spriteBatch.Draw(_logo,
                        new Rectangle((int)WindowWidth / 4 - _logo.Width / 4,
                        (int)WindowHeight / 4 - _logo.Height / 2,
                        _logo.Width * 2, _logo.Height * 2), Color.White);

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
                    #endregion
                    break;

                #region Game State
                case GameState.Game:
                    // Draw.. the game?
                    _player.Draw(_spriteBatch);
                    

                    _spriteBatch.DrawString(_font, "HIPUR (the game)",
                        new Vector2(WindowWidth / 2 - _font.MeasureString("HIPUR (the game)").X / 2, WindowHeight / 2), Color.White);
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
        /// load a texture and store it in the dictionary with a key that corresponds to its filename
        /// </summary>
        /// <param name="fileName"></param>
        private Texture2D LoadTexture(string fileName)
        {
            Texture2D output = this.Content.Load<Texture2D>(fileName);

            if (!Textures.ContainsKey(fileName))
            {
                Textures.Add(fileName, output);
            }
            else
            {
                Textures[fileName] = output;
            }
        
            return output;
        }
    }
}