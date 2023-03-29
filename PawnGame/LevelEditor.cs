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
using System.Windows.Forms;

namespace PawnGame
{
    internal class LevelEditor
    {
        #region fields
        private string _filePath;
        private List<Button> _palette;
        private List<Button> _options;
        private int _selected;
        private Level _level;
        private Vector2 _cameraPosition;
        private bool _canClick;
        private MouseState _mState;
        private Game1 _game;
        #endregion

        #region spacing variables
        private Vector2 _paletteTopLeft;
        private int _ButtonSpacing;
        #endregion

        #region constructors
        /// <summary>
        /// load the level editor to create a new x*y level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public LevelEditor(int x, int y, Game1 game)
        {
            _level = new Level(new Tile[x, y], new List<Enemy>(), new Vector2());
            _game = game;
            Initialize();
        }
        /// <summary>
        /// load the level editor with a file path. If the file is not read, creates a new 8*8 level instead and throws an exception with a relevant message.
        /// </summary>
        /// <param name="filePath"></param>
        public LevelEditor(string filePath, Game1 game)
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
            _game = game;
            Initialize();
        }
        #endregion

        /// <summary>
        /// runs initialization code
        /// </summary>
        private void Initialize()
        {
            //set up buttons and variables
            #region spacing variables
            _paletteTopLeft = new Vector2(10, 10);
            _ButtonSpacing = 10;
            #endregion
            _options = new List<Button>();
            _palette = new List<Button>();
            _palette.Add(new Button(Game1.Textures["logo"], _paletteTopLeft, Color.Green));
            _palette.Add(new Button(Game1.Textures["logo"], _paletteTopLeft + new Vector2(0, Game1.Textures["logo"].Height + _ButtonSpacing), Color.Green));
            float optionsX = _game.WindowWidth - _paletteTopLeft.X - Game1.Textures["IconLoad"].Width;
            _options.Add(new Button(Game1.Textures["IconLoad"], new Vector2(optionsX, _paletteTopLeft.Y), Color.Green));
            _options.Add(new Button(Game1.Textures["IconSave"], new Vector2(optionsX, _paletteTopLeft.Y + (Game1.Textures["IconLoad"].Height + _ButtonSpacing) /* times n*/), Color.Green));
            _selected = 0;
            _cameraPosition = new Vector2(100, 100);
            _canClick = true;

            //populate tile array
            int sideLength = (int)_game.WindowHeight / _level.Tiles.GetLength(1);
            int margin = ((int)_game.WindowWidth / 2) - _level.Tiles.GetLength(0) * sideLength / 2;
            for(int x = 0; x < _level.Tiles.GetLength(0); x++)
            {
                for(int y = 0; y < _level.Tiles.GetLength(1); y++)
                {
                    _level.Tiles[x, y] = new Tile(Game1.Textures["logo"], new Rectangle(margin + x * sideLength, y * sideLength, sideLength, sideLength), false);
                }
            }
        }

        /// <summary>
        /// update the level editor
        /// </summary>
        public void Update()
        {
            //manage clicking
            _mState = Mouse.GetState();

            if (!_canClick && _mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
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

            //check for clicks on options
            for (int i = 0; i < _options.Count; i++)
            {
                if (_options[i].Clicked())
                {
                    switch (i)
                    {
                        //save
                        case 0:
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    _level = Level.Read(openFileDialog.FileName);
                                }
                            }
                            break;
                        //load
                        case 1:
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    Level.Write(_level, openFileDialog.FileName);
                                }
                            }
                            break;
                    }
                }
            }

            //check for clicks on tiles in the level
            for (int i = 0; i < _level.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _level.Tiles.GetLength(1); j++)
                {
                    if (CheckMouseOn(_level.Tiles[i, j]))
                    {
                        if (_mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            //set the tile corresponding to palette
                            switch (_selected)
                            {
                                case 0:
                                    _level.Tiles[i, j] = new Tile(Game1.Textures["logo"], new Vectangle(_level.Tiles[i, j].X, _level.Tiles[i, j].Y, _level.Tiles[i, j].Width, _level.Tiles[i, j].Height), false);
                                    break;
                                case 1:
                                    _level.Tiles[i, j] = new Tile(Game1.Textures["logo"], new Vectangle(_level.Tiles[i, j].X, _level.Tiles[i, j].Y, _level.Tiles[i, j].Width, _level.Tiles[i, j].Height), true);
                                    break;
                            }
                        }
                        if (_mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            //should spawn an empty texture tile
                            _level.Tiles[i, j] = new Tile(Game1.Textures["logo"], new Vectangle(_level.Tiles[i, j].X, _level.Tiles[i, j].Y, _level.Tiles[i, j].Width, _level.Tiles[i, j].Height), true);
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
            _level.Draw(sb, _cameraPosition);
            //draw tile palette
            for (int i = 0; i < _palette.Count; i++)
            {
                _palette[i].Draw(sb);
            }
            //draw options
            for (int i = 0; i < _options.Count; i++)
            {
                _options[i].Draw(sb);
            }
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