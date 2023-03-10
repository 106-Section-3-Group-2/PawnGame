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
        private Tile[] _paletteTiles;
        private Enemy[] _paletteEnemies;
        private GameObject _selected;
        private Level _level;
        private Vector2 _cameraPosition;
        private bool _canClick;
        private MouseState _mState;
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
            initialize();
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
            catch(Exception e)
            {
                _level = new Level(new Tile[8, 8], new List<Enemy>(), new Vector2());
                throw e;
            }
            initialize();
        }
        #endregion

        /// <summary>
        /// runs initialization code
        /// </summary>
        private void initialize()
        {
            _paletteTiles = new Tile[]{
                //one of each kind of tile here
            };
            _paletteEnemies = new Enemy[]{
                //one of each kind of tile here
            };
            _selected = _paletteTiles[0];
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
            if(!_canClick && _mState.LeftButton == ButtonState.Released)
            {
                _canClick = true;
            }
            //check for clicks on tile palette
            for(int i = 0; i < _paletteTiles.Length; i++)
            {
                if(CheckClicked(_paletteTiles[i]))
                {
                    _selected = _paletteTiles[i];
                }
            }

            //check for clicks on enemy palette
            for (int i = 0; i < _paletteEnemies.Length; i++)
            {
                if (CheckClicked(_paletteEnemies[i]))
                {
                    _selected = _paletteEnemies[i];
                }
            }

            //check for clicks on tiles in the level
            for (int i = 0; i < _level.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _level.Tiles.GetLength(1); j++)
                {
                    if (CheckMouseOn(_level.Tiles[i, j]))
                    {
                        if(_mState.LeftButton == ButtonState.Pressed)
                        {
                            Tile newTile = 
                        }
                        if (_mState.RightButton == ButtonState.Pressed)
                        {

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
            for (int i = 0; i < _paletteTiles.Length; i++)
            {

            }

            //draw enemy palette
            for (int i = 0; i < _paletteEnemies.Length; i++)
            {

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
            return (_mState.X > g.X && _mState.Y > g.Y && _mState.X < g.X + g.Width && _mState.Y < g.Y + g.Height);
        }

        /// <summary>
        /// return whether a GameObject has been clicked
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool CheckClicked(GameObject g)
        {
            return (CheckMouseOn(g) && _mState.LeftButton == ButtonState.Pressed && _canClick);
        }
    }
}
