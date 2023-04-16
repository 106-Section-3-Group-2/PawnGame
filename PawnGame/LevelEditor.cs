using PawnGame.GameObjects;
using System;
using System.Collections.Generic;
using PawnGame.GameObjects.Enemies;
using System.Windows.Forms;
using static PawnGame.Game1;
using Microsoft.Xna.Framework.Input;

namespace PawnGame
{
    internal class LevelEditor
    {
        #region Fields
        private string _filePath;
        private List<Button> _palette;
        private List<Button> _options;
        private int _selected;
        private Level _level;
        private Vector2 _cameraPosition;
        private bool _canClick;
        private MouseState _mState;
        private MouseState _mStatePrev;
        private Game1 _game;
        private int _playerScale;
        private Point _pawnDimensions;
        #endregion

        #region Spacing variables
        private Vector2 _paletteTopLeft;
        private int _ButtonSpacing;
        #endregion

        #region Constructors
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
            _paletteTopLeft = new Vector2(10, 10);
            _ButtonSpacing = 10;

            _options = new List<Button>();
            _palette = new List<Button>();

            _playerScale = 4;
            _pawnDimensions = new Point(Game1.Assets[AssetNames.PawnWhite].Width / _playerScale, Game1.Assets[AssetNames.PawnWhite].Height / _playerScale);

            int paletteDownscale = 4;

            #region Add palette information
            //standard tile
            _palette.Add(new Button(
                Assets[AssetNames.TileWhite],
                _paletteTopLeft,
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //wall
            _palette.Add(new Button(
                Assets[AssetNames.DebugError],
                _paletteTopLeft + new Vector2(0,
                (_palette[0].ButtonBox.Height + _ButtonSpacing)/* times n*/),
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //exit
            _palette.Add(new Button(
                Assets[AssetNames.IconLoad],
                _paletteTopLeft + new Vector2(0,
                (_palette[0].ButtonBox.Height + _ButtonSpacing) * 2),
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //pawn enemy
            _palette.Add(new Button(
                Assets[AssetNames.PawnWhite],
                _paletteTopLeft + new Vector2(0,
                (_palette[0].ButtonBox.Height + _ButtonSpacing) * 3),
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //spawn point
            _palette.Add(new Button(
                Assets[AssetNames.PawnBlack],
                _paletteTopLeft + new Vector2(0,
                (_palette[0].ButtonBox.Height + _ButtonSpacing) * 4),
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //create options
            float optionsX = _game.WindowWidth - _paletteTopLeft.X - Assets[AssetNames.IconLoad].Width;
            //save
            _options.Add(new Button(
                Assets[AssetNames.IconLoad],
                new Vector2(optionsX, _paletteTopLeft.Y),
                Color.Green));
            //load
            _options.Add(new Button(
                Assets[AssetNames.IconSave],
                new Vector2(optionsX, _paletteTopLeft.Y + (Assets[AssetNames.IconLoad].Height + _ButtonSpacing) /* times n*/),
                Color.Green));
            #endregion

            _selected = -1;
            _cameraPosition = new Vector2(100, 100);
            _canClick = true;

            //populate tile array
            int sideLength = _game.WindowHeight / _level.Tiles.GetLength(1);
            int margin = (_game.WindowWidth / 2) - _level.Tiles.GetLength(0) * sideLength / 2;

            for(int x = 0; x < _level.Tiles.GetLength(0); x++)
            {
                for(int y = 0; y < _level.Tiles.GetLength(1); y++)
                {
                    if((x + y) % 2 == 0)
                    {
                        _level.Tiles[x, y] = new Tile(AssetNames.TileWhite, new Rectangle(margin + x * sideLength, y * sideLength, sideLength, sideLength), false);
                    }
                    else
                    {
                        _level.Tiles[x, y] = new Tile(AssetNames.TileBlack, new Rectangle(margin + x * sideLength, y * sideLength, sideLength, sideLength), false);
                    }
                }
            }
            //default spawn point
            _level.SpawnPoint = _level.Tiles[0, 0].Hitbox.Location;
        }

        /// <summary>
        /// update the level editor
        /// </summary>
        public void Update()
        {
            //manage clicking
            _mStatePrev = _mState;
            _mState = Mouse.GetState();

            if (!_canClick && _mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
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
                        //load
                        case 0:
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                //try
                                //{
                                    _level = Level.Read(openFileDialog.FileName);
                                //}
                                /*catch
                                {
                                    We should put a popup on screen to let the user know what is happening
                                    _options.Add(new Button(Assets[AssetNames.DebugError], new Vector2(_options[0].ButtonBox.X - Assets[AssetNames.DebugError].Width, _options[0].ButtonBox.Y), Color.Blue));
                                }*/
                            }
                            break;
                        //save
                        case 1:
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                Level.Write(_level, saveFileDialog.FileName);
                            }
                            break;
                        //load error, should always be last case
                        case 3:
                            _options.RemoveAt(i);
                            break;
                    }
                }
            }

            //check for clicks on tiles in the level
            for (int x = 0; x < _level.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _level.Tiles.GetLength(1); y++)
                {
                    if (CheckMouseOn(_level.Tiles[x, y]))
                    {
                        if (_mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            //set the tile corresponding to palette
                            bool occupied;
                            switch (_selected)
                            {
                                case -1:
                                    break;
                                case 0:
                                    if ((x + y) % 2 == 0)
                                    {
                                        _level.Tiles[x, y] = new Tile(AssetNames.TileWhite, new Vectangle(_level.Tiles[x, y].X, _level.Tiles[x, y].Y, _level.Tiles[x, y].Width, _level.Tiles[x, y].Height), false);
                                    }
                                    else
                                    {
                                        _level.Tiles[x, y] = new Tile(AssetNames.TileBlack, new Vectangle(_level.Tiles[x, y].X, _level.Tiles[x, y].Y, _level.Tiles[x, y].Width, _level.Tiles[x, y].Height), false);
                                    }
                                    break;
                                case 1:
                                    //create a solid wall
                                    if ((x + y) % 2 == 0)
                                    {
                                        _level.Tiles[x, y] = new Tile(AssetNames.DebugError, new Vectangle(_level.Tiles[x, y].X, _level.Tiles[x, y].Y, _level.Tiles[x, y].Width, _level.Tiles[x, y].Height), true);
                                    }
                                    else
                                    {
                                        _level.Tiles[x, y] = new Tile(AssetNames.DebugError, new Vectangle(_level.Tiles[x, y].X, _level.Tiles[x, y].Y, _level.Tiles[x, y].Width, _level.Tiles[x, y].Height), true);
                                    }
                                    break;
                                case 2:
                                    //create an exit
                                    _level.Tiles[x, y] = new Tile(AssetNames.IconLoad, new Vectangle(_level.Tiles[x, y].X, _level.Tiles[x, y].Y, _level.Tiles[x, y].Width, _level.Tiles[x, y].Height), false, true);
                                    break;
                                case 3:
                                    //make sure there isn't already an enemy or solid wall there
                                    occupied = false;
                                    foreach(Enemy enemy in _level.EnemySpawns)
                                    {
                                        if (enemy.Hitbox.Contains(_mState.X, _mState.Y) || _level.Tiles[x,y].IsSolid == true)
                                        {
                                            occupied = true;
                                        }
                                    }
                                    //create an pawn enemy
                                    if (!occupied)
                                    {
                                        _level.EnemySpawns.Add(new Pawn(AssetNames.PawnWhite, new Rectangle(
                                            (int)(_level.Tiles[x, y].X + _level.Tiles[x, y].Width / 2 - _pawnDimensions.X / 2),
                                            (int)(_level.Tiles[x, y].Y + _level.Tiles[x, y].Height / 2 - _pawnDimensions.Y / 2),
                                            (int)_pawnDimensions.X,
                                            (int)_pawnDimensions.Y
                                            )));
                                    }
                                    break;
                                case 4:
                                    //make sure there isn't already an enemy, solid wall, or exit there
                                    occupied = false;
                                    foreach (Enemy enemy in _level.EnemySpawns)
                                    {
                                        if (enemy.Hitbox.Contains(_mState.X, _mState.Y) || _level.Tiles[x, y].IsSolid == true)
                                        {
                                            occupied = true;
                                        }
                                    }
                                    //set spawn point
                                    if (!occupied)
                                    {
                                        _level.SpawnPoint = new Vector2((int)(_level.Tiles[x, y].X + _level.Tiles[x, y].Width / 2 - _pawnDimensions.X / 2),
                                            (int)(_level.Tiles[x, y].Y + _level.Tiles[x, y].Height / 2 - _pawnDimensions.Y / 2));
                                    }
                                    break;
                            }
                        }
                        if (_mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            //if there's an enemy on top of the tile, remove it first
                            bool occupied = false;
                            for(int i = 0; i < _level.EnemySpawns.Count; i++)
                            {
                                if (_level.EnemySpawns[i].Hitbox.Contains(_mState.X, _mState.Y))
                                {
                                    _level.EnemySpawns.RemoveAt(i);
                                    occupied = true;
                                    _canClick = false;
                                    break;
                                }
                            }

                            //should spawn an empty texture tile
                            if (!occupied && _canClick)
                            {
                                _level.Tiles[x, y] = new Tile(
                                    AssetNames.GameLogo,
                                    new Vectangle(_level.Tiles[x, y].X,
                                    _level.Tiles[x, y].Y,
                                    _level.Tiles[x, y].Width,
                                    _level.Tiles[x, y].Height),
                                    true);
                            }
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
            _level.Draw(sb);
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
            //draw enemies
            for(int i = 0; i < _level.EnemySpawns.Count; i++)
            {
                _level.EnemySpawns[i].Draw(sb);
            }
            //draw spawn point
            sb.Draw(
                Game1.Assets[AssetNames.PawnBlack],
                new Rectangle(new Point((int)_level.SpawnPoint.X,(int)_level.SpawnPoint.Y),
                _pawnDimensions),
                Color.White);
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

        /// <summary>
        /// return whether the mouse was just clicked
        /// </summary>
        /// <returns></returns>
        private bool JustClicked()
        {
            return _mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _mStatePrev.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed;
        }

        /// <summary>
        /// return whether the mouse was just right clicked
        /// </summary>
        /// <returns></returns>
        private bool JustRightClicked()
        {
            return _mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _mStatePrev.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed;
        }
    }
}