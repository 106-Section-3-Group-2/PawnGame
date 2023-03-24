using PawnGame.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawnGame;
using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
namespace PawnGame
{
    internal class LevelEditor
    {
        #region fields
        private string _filePath;
        private List<Button> _palette;
        private int _selected;
        private Level _level;
        private Vector2 _cameraPosition;
        private bool _canClick;
        private MouseState _mState;
        #endregion

        #region spacing variables
        private Vector2 _paletteTopLeft;
        private int _paletteSpacing;
        #endregion

        #region constructors
        /// <summary>
        /// load the level editor to create a new x*y level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public LevelEditor(int x, int y)
        {
            _level = new Level(new Tile[x, y], new List<Enemy>(), new Vector2());
            Initialize();
        }
        /// <summary>
        /// load the level editor with a file path. If the file is not read, creates a new 8*8 level instead and throws an exception with a relevant message.
        /// </summary>
        /// <param name="filePath"></param>
        public LevelEditor(string filePath)
        {
            try
            {
                _level = Level.Read(filePath);
                _filePath = filePath;
            }
            catch (Exception e)
            {
                _level = new Level(new Tile[8, 8], new List<Enemy>(), new Vector2());
                throw e;
            }
            Initialize();
        }
        #endregion

        /// <summary>
        /// runs initialization code
        /// </summary>
        private void Initialize()
        {
            _palette = new List<Button>();
            //one of each kind of tile/enemy here
            _palette.Add(new Button(Game1.Textures["logo"], _paletteTopLeft, Color.Green));
            _selected = 0;
            _cameraPosition = new Vector2(100, 100);
            _canClick = true;
        }

        /// <summary>
        /// update the level editor
        /// </summary>
        public void Update()
        {
            //manage clicking
            _mState = Mouse.GetState();

            if (!_canClick && _mState.LeftButton == ButtonState.Released)
            {
                _canClick = true;
            }

            //check for clicks on palette
            for (int i = 0; i < _palette.Count; i++)
            {
                if (_palette[i].Clicked())
                {
                    _selected = i;
                }
            }

            //check for clicks on tiles in the level
            for (int i = 0; i < _level.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _level.Tiles.GetLength(1); j++)
                {
                    if (CheckMouseOn(_level.Tiles[i, j]))
                    {
                        if (_mState.LeftButton == ButtonState.Pressed)
                        {

                        }
                        if (_mState.RightButton == ButtonState.Pressed)
                        {
                            //_level.Tiles[i, j] = new Tile()
                        }
                    }
                }
            }
        }

        /// <summary>
        /// draw the level editor
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            #region graphical elements
            _level.Draw(sb, _cameraPosition);
            //draw tile palette
            for (int i = 0; i < _palette.Count; i++)
            {
                _palette[i].Draw(sb);
            }
            #endregion
            #region buttons
            //no buttons yet
            #endregion
        }

        /// <summary>
        /// return whether the mouse is over a GameObject
        /// </summary>
        /// <param name="g"></param>
        private bool CheckMouseOn(GameObject g)
        {
            if(g == null)
            {
                return false;
            }
            return _mState.X > g.X && _mState.Y > g.Y && _mState.X < g.X + g.Width && _mState.Y < g.Y + g.Height;
        }
    }
}