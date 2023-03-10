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
        }

        /// <summary>
        /// update the level editor
        /// </summary>
        public void Update()
        {
            //check for clicks on tile palette
            for(int i = 0; i < _paletteTiles.Length; i++)
            {
                
            }

            //check for clicks on enemy palette
            for (int i = 0; i < _paletteEnemies.Length; i++)
            {

            }

            //check for clicks on tiles in the level
            for (int i = 0; i < _level.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _level.Tiles.GetLength(1); j++)
                {

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
        private void CheckMouseOn(GameObject g)
        {
            MouseState mState = Mouse.GetState();
            //return (mState.X > g.X && mState.Y > g. && mState.X < g.X + width &&)
        }
    }
}
