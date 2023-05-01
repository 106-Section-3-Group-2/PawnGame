using Newtonsoft.Json;
using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.IO;

namespace PawnGame
{
    /// <summary>
    /// holds a 2D array of rooms the player can traverse between
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Cardinal directions used for telling the level which room to move to
        /// </summary>
        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        #region Fields
        /// <summary>
        /// Grid of rooms in the level
        /// </summary>
        [JsonProperty]
        private readonly Room[,] _rooms;

        /// <summary>
        /// Vector storing the active room's X and Y index
        /// </summary>
        [JsonProperty]
        private Point _activeRoomIndex;

        //drawing values for minimap
        private static int _mapBoxSize;
        private static Point _margin;
        private static Point _bottomRight;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the room at the specified cordinates
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public Room this[int i, int j] => _rooms[i, j];

        /// <summary>
        /// The current active room in the level
        /// </summary>
        [JsonIgnore]
        public Room ActiveRoom => _rooms[_activeRoomIndex.X, _activeRoomIndex.Y];
        /// <summary>
        /// the index of the active room
        /// </summary>
        public Point ActiveRoomIndex => _activeRoomIndex;
        #endregion

        #region Constructors
        /// <summary>
        /// Makes a constructor with the entered values. If none are entered, defaults are picked.
        /// </summary>
        /// <param name="levelSize">Max size of the level. Default is 5x5.</param>
        /// <param name="spawnRoomIndex">Index of the starting room. Default is room 3,3.</param>
        /// <param name="game">requires a Game1 reference</param>
        public Level(Game1 game, Point? levelSize = null, Point? spawnRoomIndex = null)
        {
            #region Fill out values if any arent entered
            levelSize ??= new(5);
            spawnRoomIndex ??= new(3);
            #endregion

            _rooms = new Room[levelSize.Value.X, levelSize.Value.Y];
            _activeRoomIndex = new(spawnRoomIndex.Value.X, spawnRoomIndex.Value.Y);
            //populate rooms
            for(int x = 0; x < levelSize.Value.X; x++)
            {
                for(int y = 0; y < levelSize.Value.Y; y++)
                {
                    _rooms[x, y] = new Room(8, 8);
                }
            }
            Initialize();
        }

        /// <summary>
        /// Creates a room with the tile layout and starting position enterd. This constructor is used for JSON deserialization
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="spawnRoomIndex"></param>
        [JsonConstructor]
        private Level(Room[,] rooms, Point spawnRoomIndex)
        {
            _rooms = rooms;
            _activeRoomIndex = spawnRoomIndex;
            Initialize();
        }
        #endregion

        /// <summary>
        /// initialize values for the Level
        /// </summary>
        private void Initialize()
        {
            _mapBoxSize = 30;
            _bottomRight = new Point(Game1.RenderTargetWidth, Game1.RenderTargetHeight);
            _margin = new Point(10, 10);
        }

        /// <summary>
        /// draw the level's minimap
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            Point bottomRight = _bottomRight - _margin - new Point(_mapBoxSize, _mapBoxSize);
            for(int x = 0; x < _rooms.GetLength(0); x++)
            {
                for(int y = 0; y < _rooms.GetLength(1); y++)
                {
                    Color drawColor = Color.Red;
                    if (_rooms[x,y].Cleared)
                    {
                        drawColor = Color.Green;
                    }
                    if(_activeRoomIndex.X == x  && _activeRoomIndex.Y == y)
                    {
                        drawColor = Color.White;
                    }
                    sb.Draw(Game1.Assets[Game1.AssetNames.TileWhite], new Rectangle(bottomRight.X - (_rooms.GetLength(0) - 1 - x) * _mapBoxSize, bottomRight.Y - (_rooms.GetLength(1) - 1 - y) * _mapBoxSize, _mapBoxSize, _mapBoxSize), drawColor);
                }
            }
        }

        /// <summary>
        /// Moves one room in the entered direction
        /// </summary>
        /// <param name="direction">Direction to move</param>
        /// <exception cref="InvalidOperationException">Thrown if there is no room in the entered direction</exception>
        public void AdvanceRoom(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    if (_activeRoomIndex.Y == 0 || _rooms[_activeRoomIndex.X,_activeRoomIndex.Y-1] == null) 
                        throw new InvalidOperationException("No room is north of the current room.");

                    _activeRoomIndex.Y--;
                    break;
                case Direction.South:
                    if (_activeRoomIndex.Y == _rooms.GetLength(1) - 1 || _rooms[_activeRoomIndex.X, _activeRoomIndex.Y + 1] == null)
                        throw new InvalidOperationException("No room is south of the current room.");

                    _activeRoomIndex.Y++;
                    break;
                case Direction.East:
                    if (_activeRoomIndex.X == _rooms.GetLength(0) - 1 || _rooms[_activeRoomIndex.X+1, _activeRoomIndex.Y] == null) 
                        throw new InvalidOperationException("No room is east of the current room.");

                    _activeRoomIndex.X++;
                    break;
                case Direction.West:
                    if (_activeRoomIndex.X == 0 || _rooms[_activeRoomIndex.X-1, _activeRoomIndex.Y] == null) 
                        throw new InvalidOperationException("No room is west of the current room.");

                    _activeRoomIndex.X--;
                    break;
            }
            _rooms[_activeRoomIndex.X, _activeRoomIndex.Y].ActivateRoom();

            //find a suitable spawnpoint for the player
            Game1.Player.X = ActiveRoom.SpawnPoint.X;
            Game1.Player.Y = ActiveRoom.SpawnPoint.Y;
            switch (direction)
            {
                case Direction.North:
                    for(int x = 0; x < ActiveRoom.Tiles.GetLength(0) - 1; x++)
                    {
                        Tile spawnTile = ActiveRoom.Tiles[x, ActiveRoom.Tiles.GetLength(1) - 2];
                        if (ActiveRoom.Tiles[x, ActiveRoom.Tiles.GetLength(1) - 1].IsDoor && !spawnTile.IsSolid)
                        {
                            Point newSpawn = new Point((int)spawnTile.X, (int)spawnTile.Y);
                            Game1.Player.X = newSpawn.X;
                            Game1.Player.Y = newSpawn.Y;
                        }
                    }
                    break;
                case Direction.South:
                    for (int x = 0; x < ActiveRoom.Tiles.GetLength(0) - 1; x++)
                    {
                        Tile spawnTile = ActiveRoom.Tiles[x, 1];
                        if (ActiveRoom.Tiles[x, 0].IsDoor && !spawnTile.IsSolid)
                        {
                            Point newSpawn = new Point((int)spawnTile.X, (int)spawnTile.Y);
                            Game1.Player.X = newSpawn.X;
                            Game1.Player.Y = newSpawn.Y;
                        }
                    }
                    break;
                case Direction.East:
                    for (int y = 0; y < ActiveRoom.Tiles.GetLength(1) - 1; y++)
                    {
                        Tile spawnTile = ActiveRoom.Tiles[1, y];
                        if (ActiveRoom.Tiles[0,y].IsDoor && !spawnTile.IsSolid)
                        {
                            Point newSpawn = new Point((int)spawnTile.X, (int)spawnTile.Y);
                            Game1.Player.X = newSpawn.X;
                            Game1.Player.Y = newSpawn.Y;
                        }
                    }
                    break;
                case Direction.West:
                    for (int y = 0; y < ActiveRoom.Tiles.GetLength(1) - 1; y++)
                    {
                        Tile spawnTile = ActiveRoom.Tiles[ActiveRoom.Tiles.GetLength(0) - 2, y];
                        if (ActiveRoom.Tiles[ActiveRoom.Tiles.GetLength(1) - 1, y].IsDoor && !spawnTile.IsSolid)
                        {
                            Point newSpawn = new Point((int)spawnTile.X, (int)spawnTile.Y);
                            Game1.Player.X = newSpawn.X;
                            Game1.Player.Y = newSpawn.Y;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Saves the current level to both the dev and user level folders
        /// </summary>
        public void Save(string filePath)
        {
            string jsonLevel = JsonConvert.SerializeObject(this, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Levels\")))
            {
                string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                using StreamWriter devWriter = new(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Levels\" + fileName));
                devWriter.Write(jsonLevel);
            }

            using StreamWriter gameWriter = new(filePath);
            gameWriter.Write(jsonLevel);
        }

        /// <summary>
        /// Takes a file path and returns the level stored in that file
        /// </summary>
        /// <param name="filePath">path of the file to load</param>
        /// <returns>Level stored in that file</returns>
        public static Level Load(string filePath)
        {
            using StreamReader reader = new(filePath);
            return JsonConvert.DeserializeObject<Level>(reader.ReadLine(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        }

        /// <summary>
        /// get the length (in rooms) of the level in a given dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public int Length(int dimension)
        {
            return _rooms.GetLength(dimension);
        }

        /// <summary>
        /// Resets the level by setting the player to the start position, reseting 
        /// </summary>
        public void Reset()
        {
            _activeRoomIndex = new Point(0, 0);

            Game1.Player.X = _rooms[_activeRoomIndex.X, _activeRoomIndex.Y].SpawnPoint.X;
            Game1.Player.Y = _rooms[_activeRoomIndex.X, _activeRoomIndex.Y].SpawnPoint.Y;

            for (int i = 0; i < _rooms.GetLength(0); i++)
            {
                for (int j = 0; j < _rooms.GetLength(1); j++)
                {
                    _rooms[i, j].Cleared = false;
                }
            }

            ActiveRoom.ActivateRoom();

            Game1.Player.IsAlive = true;
        }
    }
}
