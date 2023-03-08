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

        private KeyboardState _currKbState;
        private KeyboardState _prevKbState;

        private int _width;
        private int _height;

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

        /* Not sure whether it would be better to store buttons are seperate
         variables, or under one list
         Right now, it's under a list, but I'll change it if necessary
         - Troy

         private Button _newGameBtn;
         private Button _loadGameBtn;
         private Button _lvlEditorBtn;*/
         
        // This font is temporary
        // Will use for creating menu skeleton
        // for implementation of clicking from menu to game
        private SpriteFont font;


        public Game1()
        {
            _width = 0;
            _height = 0;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;     
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _prevKbState = Keyboard.GetState();
            base.Initialize();
        } 

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Debug font
            font = this.Content.Load<SpriteFont>("Arial");

            // Adding all 3 buttons on the menu screen to the list
            _menuButtons.Add(new(font, "New Game",
                    new Vector2(WindowWidth / 2 - font.MeasureString("New Game").X / 2, WindowHeight - 100),
                Color.LightGray));
            _menuButtons.Add(new(font, "Load Game",
                    new Vector2(WindowWidth / 2 - font.MeasureString("Load Game").X / 2, WindowHeight - 75),
                    Color.LightGray));
            _menuButtons.Add(new(font, "Level Editor",
                    new Vector2(WindowWidth / 2 - font.MeasureString("Level Editor").X / 2, WindowHeight - 50),
                    Color.LightGray));
            
            //
        }

        protected override void Update(GameTime gameTime)
        {
            _currKbState = Keyboard.GetState();

            // Toggling fullscreen
            if (_currKbState.IsKeyDown(Keys.F11) && _prevKbState.IsKeyUp(Keys.F11))
            {
                
                if (_graphics.IsFullScreen)
                {
                    // Changes the width and height back to the original size
                    _graphics.PreferredBackBufferWidth = _width;
                    _graphics.PreferredBackBufferHeight = _height;
                }
                else
                {
                    // Stores the width and height of the screen when it is not full screen
                    // so that it is easy to revert it
                    _width = Window.ClientBounds.Width;
                    _height = Window.ClientBounds.Height;

                    // Updating the width and the height to the resolution of the user's screen
                    _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                }

                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:

                    _menuButtons.Clear();

                    // Adding all 3 buttons on the menu screen to the list
                    _menuButtons.Add(new(font, "New Game",
                            new Vector2(WindowWidth / 2 - font.MeasureString("New Game").X / 2, WindowHeight - 100),
                        Color.LightGray));
                    _menuButtons.Add(new(font, "Load Game",
                            new Vector2(WindowWidth / 2 - font.MeasureString("Load Game").X / 2, WindowHeight - 75),
                            Color.LightGray));
                    _menuButtons.Add(new(font, "Level Editor",
                            new Vector2(WindowWidth / 2 - font.MeasureString("Level Editor").X / 2, WindowHeight - 50),
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
                    _player.Update(_currKbState,_prevKbState);
                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:
                    // Update logic for level editor

                    #endregion
                    break;

                #region Victory State
                case GameState.Victory:
                    // Check for buttons clicked to go back to menu

                    #endregion
                    break;
            }

            // Regradless of state, you can get to the debug menu
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
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            _spriteBatch.Begin();

            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:

                    // Menu skeleton containing the buttons
                    // that will be able to be clicked

                    

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
                    _spriteBatch.DrawString(font, "Debug Menu",
                        new Vector2(WindowWidth / 2 - font.MeasureString("Debug Menu").X/2, 100), Color.White);
                    #endregion
                    break;

                #region Game State
                case GameState.Game:
                    // Draw.. the game?

                    _spriteBatch.DrawString(font, "HIPUR (the game)",
                        new Vector2(WindowWidth / 2 - font.MeasureString("HIPUR (the game)").X / 2, WindowHeight / 2), Color.White);
                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:
                    // Draw level editor interface

                    _spriteBatch.DrawString(font, "Level Editor",
                        new Vector2(WindowWidth / 2 - font.MeasureString("LevelEditor").X / 2, WindowHeight / 2), Color.White);
                    #endregion
                    break;

                #region Victory State
                case GameState.Victory:
                    // Draw victory screen

                    _spriteBatch.DrawString(font, "You win",
                        new Vector2(WindowWidth / 2 - font.MeasureString("You win").X / 2, WindowHeight / 2), Color.White);
                    #endregion
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}