global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;

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

        private MouseState _currMState;
        private MouseState _prevMState;

        // This font is temporary
        // Will use for creating menu skeleton
        // for implementation of clicking from menu to game
        private SpriteFont font;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_gameState)
            {
                #region Menu State
                case GameState.Menu:

                    #endregion
                    break;

                #region DebugMenu State
                case GameState.DebugMenu:

                    #endregion
                    break;

                #region Game State
                case GameState.Game:

                    #endregion
                    break;

                #region LevelEditor State
                case GameState.LevelEditor:

                    #endregion
                    break;

                #region Victory State
                case GameState.Victory:

                    #endregion
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}