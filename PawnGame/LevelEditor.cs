using PawnGame.GameObjects;
using System;
using System.Collections.Generic;
using PawnGame.GameObjects.Enemies;
using System.Windows.Forms;
using static PawnGame.Game1;

namespace PawnGame
{
    internal class LevelEditor
    {
        private Room Room
        {
            get
            {
                return _level[_level.ActiveRoomIndex.X, _level.ActiveRoomIndex.Y];
            }
        }

        #region Fields
        private Level _level;
        private List<Button> _palette;
        private List<Button> _options;
        private int _selected;
        private bool _canClick;
        private MouseState _mState;
        private MouseState _mStatePrev;
        private KeyboardState _kbState;
        private KeyboardState _kbStatePrev;
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
            //Room = new Room(new Tile[x, y], new Vector2()); need new
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
                //Room = Room.Read(filePath); //need new
            }
            catch (Exception e)
            {
                //Room = new Room(new Tile[8, 8], new Vector2()); //need new
                throw e;
            }
            _game = game;
            Initialize();
        }

        /// <summary>
        /// load the level editor given a room to edit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public LevelEditor(Room level, Game1 game)
        {
            //Room = level; //need new
            _game = game;
            Initialize();
        }
        #endregion

        /// <summary>
        /// runs initialization code
        /// </summary>
        private void Initialize()
        {
            //initialize static level if it hasn't been
            if(_level == null)
            {
                _level = new Level(_game, new Point(3, 3), new Point(0, 0));
            }

            //set up buttons and variables
            _paletteTopLeft = new Vector2(10, 10);
            _ButtonSpacing = 10;
            _options = new List<Button>();
            _palette = new List<Button>();

            _playerScale = 2;
            _pawnDimensions = new Point(Assets[AssetNames.PawnWhite].Width / _playerScale, Assets[AssetNames.PawnWhite].Height / _playerScale);

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
                Assets[AssetNames.WallWhite],
                _paletteTopLeft + new Vector2(0,
                (_palette[0].ButtonBox.Height + _ButtonSpacing)/* times n*/),
                Assets[AssetNames.TileWhite].Width / paletteDownscale,
                Assets[AssetNames.TileWhite].Height / paletteDownscale,
                Color.Green));

            //exit
            _palette.Add(new Button(
                Assets[AssetNames.ExitWhite],
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
            float optionsX = _game.RenderTargetWidth - _paletteTopLeft.X - Assets[AssetNames.IconLoad].Width;
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
            _canClick = true;
        }

        /// <summary>
        /// update the level editor
        /// </summary>
        public void Update()
        {
            _kbStatePrev = _kbState;
            _kbState = Keyboard.GetState();
            #region move between rooms
            int inputVert = Convert.ToInt32(_kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !_kbStatePrev.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S)) - Convert.ToInt32(_kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !_kbStatePrev.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W));
            int inputHoriz = Convert.ToInt32(_kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !_kbStatePrev.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) - Convert.ToInt32(_kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !_kbStatePrev.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A));
            Point activeRoomIndex = _level.ActiveRoomIndex;
            if (inputHoriz != 0 && activeRoomIndex.X + inputHoriz >= 0 && activeRoomIndex.X + inputHoriz < _level.Length(0))
            {
                if(inputHoriz > 0)
                {
                    _level.AdvanceRoom(Level.Direction.East);
                }
                else
                {
                    _level.AdvanceRoom(Level.Direction.West);
                }
            }
            if (inputVert != 0 && activeRoomIndex.Y + inputVert >= 0 && activeRoomIndex.Y + inputVert < _level.Length(1))
            {
                if (inputVert > 0)
                {
                    _level.AdvanceRoom(Level.Direction.South);
                }
                else
                {
                    _level.AdvanceRoom(Level.Direction.North);
                }
            }
            #endregion

            //manage clicking
            _mStatePrev = _mState;
            _mState = Mouse.GetState();

            if (!_canClick && _mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                _canClick = true;
            }

            //Update and check for click
            for (int i = 0; i < _palette.Count; i++)
            {
                _palette[i].Update(_game.Scale);

                if (_palette[i].Clicked)
                {
                    _selected = i;
                }
            }

            //Updates and check for clicks on options
            for (int i = 0; i < _options.Count; i++)
            {
                _options[i].Update(_game.Scale);

                if (_options[i].Clicked)
                {
                    switch (i)
                    {
                        //load
                        case 0:
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                _level = Level.Load(openFileDialog.FileName);
                            }
                            break;
                        //save
                        case 1:
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                _level.Save(saveFileDialog.FileName);
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
            for (int x = 0; x < Room.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Room.Tiles.GetLength(1); y++)
                {
                    if (CheckMouseOn(Room.Tiles[x, y]))
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
                                        Room.Tiles[x, y] = new Tile(AssetNames.TileBlack, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), false);
                                    }
                                    else
                                    {
                                        Room.Tiles[x, y] = new Tile(AssetNames.TileWhite, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), false);
                                    }
                                    break;
                                case 1:
                                    //create a solid wall
                                    if ((x + y) % 2 == 0)
                                    {
                                        Room.Tiles[x, y] = new Tile(AssetNames.WallBlack, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), true);
                                    }
                                    else
                                    {
                                        Room.Tiles[x, y] = new Tile(AssetNames.WallWhite, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), true);
                                    }
                                    break;
                                case 2:
                                    //create an exit
                                    //place only on edges
                                    if(x != 0 && x != Room.Tiles.GetLength(0) - 1 && y != 0 && y != Room.Tiles.GetLength(1) - 1)
                                    {
                                        break;
                                    }
                                    //no place on corners
                                    if ((x == 0 && y == 0) || (x == Room.Tiles.GetLength(0) - 1 && y == 0) || (x == 0 && y == Room.Tiles.GetLength(1) - 1) || x == Room.Tiles.GetLength(0) - 1 && y == Room.Tiles.GetLength(1) - 1)
                                    {
                                        break;
                                    }
                                    if ((x + y) % 2 == 0)
                                    {
                                        Room.Tiles[x, y] = new Tile(AssetNames.ExitBlack, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), false, true);
                                    }
                                    else
                                    {
                                        Room.Tiles[x, y] = new Tile(AssetNames.ExitWhite, new Vectangle(Room.Tiles[x, y].X, Room.Tiles[x, y].Y, Room.Tiles[x, y].Width, Room.Tiles[x, y].Height), false, true);
                                    }
                                    break;
                                case 3:
                                    //make sure there isn't already an enemy or solid wall there
                                    occupied = false;
                                    foreach(Enemy enemy in Room.EnemySpawns)
                                    {
                                        if (enemy.Hitbox.Intersects(Room.Tiles[x, y].Hitbox) || Room.Tiles[x, y].IsSolid == true)
                                        {
                                            occupied = true;
                                        }
                                    }
                                    //create an pawn enemy
                                    if (!occupied)
                                    {
                                        Room.PawnSpawns.Add(new Pawn(AssetNames.PawnWhite, new Rectangle(
                                            (int)(Room.Tiles[x, y].X + Room.Tiles[x, y].Width / 2 - _pawnDimensions.X / 2),
                                            (int)(Room.Tiles[x, y].Y + Room.Tiles[x, y].Height / 2 - _pawnDimensions.Y / 2),
                                            (int)_pawnDimensions.X,
                                            (int)_pawnDimensions.Y
                                            )));
                                    }
                                    break;
                                case 4:
                                    //make sure there isn't already an enemy, solid wall, or exit there
                                    occupied = false;
                                    foreach (Enemy enemy in Room.EnemySpawns)
                                    {
                                        if (enemy.Hitbox.Contains(_mState.X, _mState.Y) || Room.Tiles[x, y].IsSolid == true)
                                        {
                                            occupied = true;
                                        }
                                    }
                                    //set spawn point
                                    if (!occupied)
                                    {
                                        Room.SpawnPoint = new Vector2((int)(Room.Tiles[x, y].X + Room.Tiles[x, y].Width / 2 - _pawnDimensions.X / 2),
                                            (int)(Room.Tiles[x, y].Y + Room.Tiles[x, y].Height / 2 - _pawnDimensions.Y / 2));
                                    }
                                    break;
                            }
                        }
                        if (_mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            //if there's an enemy on top of the tile, remove it first
                            bool occupied = false;
                            for(int i = 0; i < Room.EnemySpawns.Count; i++)
                            {
                                if (Room.EnemySpawns[i].Hitbox.Contains(_mState.X, _mState.Y))
                                {
                                    #region remove enemy spawn
                                    if (Room.EnemySpawns[i] is Pawn)
                                    {
                                        Room.PawnSpawns.Remove((Pawn)Room.EnemySpawns[i]);
                                    }
                                    else if (Room.EnemySpawns[i] is Bishop)
                                    {
                                        Room.BishopSpawns.Remove((Bishop)Room.EnemySpawns[i]);
                                    }
                                    else if (Room.EnemySpawns[i] is Knight)
                                    {
                                        Room.KnightSpawns.Remove((Knight)Room.EnemySpawns[i]);
                                    }
                                    else if (Room.EnemySpawns[i] is Rook)
                                    {
                                        Room.RookSpawns.Remove((Rook)Room.EnemySpawns[i]);
                                    }
                                    else if (Room.EnemySpawns[i] is Queen)
                                    {
                                        Room.QueenSpawns.Remove((Queen)Room.EnemySpawns[i]);
                                    }
                                    else if (Room.EnemySpawns[i] is King)
                                    {
                                        Room.KingSpawns.Remove((King)Room.EnemySpawns[i]);
                                    }
                                    #endregion
                                    occupied = true;
                                    _canClick = false;
                                    break;
                                }
                            }

                            //should spawn an empty texture tile
                            if (!occupied && _canClick)
                            {
                                AssetNames holeTexture = AssetNames.HoleBlack;
                                if ((x + y) % 2 == 0)
                                {
                                    holeTexture = AssetNames.HoleWhite;
                                }

                                Room.Tiles[x, y] = new Tile(
                                    holeTexture,
                                    new Vectangle(Room.Tiles[x, y].X,
                                    Room.Tiles[x, y].Y,
                                    Room.Tiles[x, y].Width,
                                    Room.Tiles[x, y].Height),
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
            Room.Draw(sb);
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
            for(int i = 0; i < Room.EnemySpawns.Count; i++)
            {
                Room.EnemySpawns[i].Draw(sb);
            }
            //draw spawn point
            sb.Draw(
                Game1.Assets[AssetNames.PawnBlack],
                new Rectangle(new Point((int)Room.SpawnPoint.X,(int)Room.SpawnPoint.Y),
                _pawnDimensions),
                Color.White);
            //draw minimap
            _level.Draw(sb);
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
            else if (_game.Scale < 1)
            {
                return (g.Hitbox * _game.Scale).Contains(_mState.X, _mState.Y);
            }
            else
            {
                return (g.Hitbox / _game.Scale).Contains(_mState.X, _mState.Y);
            }            
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