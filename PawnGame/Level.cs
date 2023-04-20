using Newtonsoft.Json;
using System;
using System.IO;

namespace PawnGame
{
    internal class Level
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
        #endregion

        #region Constructors
        /// <summary>
        /// Makes a constructor with the entered values. If none are entered, defaults are picked.
        /// </summary>
        /// <param name="levelSize">Max size of the level. Default is 5x5.</param>
        /// <param name="spawnRoomIndex">Index of the starting room. Default is room 3,3.</param>
        public Level(Point? levelSize = null, Point? spawnRoomIndex = null)
        {
            #region Fill out values if any arent entered
            levelSize ??= new(5);
            spawnRoomIndex ??= new(3);
            #endregion

            _rooms = new Room[levelSize.Value.X, levelSize.Value.Y];
            _activeRoomIndex = new(spawnRoomIndex.Value.X, spawnRoomIndex.Value.Y);
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
        }
        #endregion

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
                    if (_activeRoomIndex.Y == 0 || _rooms[_activeRoomIndex.X,_activeRoomIndex.Y--] == null) 
                        throw new InvalidOperationException("No room is north of the current room.");

                    _activeRoomIndex.Y--;
                    break;
                case Direction.East:
                    if (_activeRoomIndex.X == _rooms.GetLength(0) - 1 || _rooms[_activeRoomIndex.X++, _activeRoomIndex.Y] == null) 
                        throw new InvalidOperationException("No room is east of the current room.");

                    _activeRoomIndex.X++;
                    break;
                case Direction.South:
                    if (_activeRoomIndex.Y == _rooms.GetLength(1) - 1 || _rooms[_activeRoomIndex.X, _activeRoomIndex.Y++] == null) 
                        throw new InvalidOperationException("No room is south of the current room.");

                    _activeRoomIndex.Y++;
                    break;
                case Direction.West:
                    if (_activeRoomIndex.X == 0 || _rooms[_activeRoomIndex.X--, _activeRoomIndex.Y] == null) 
                        throw new InvalidOperationException("No room is west of the current room.");

                    _activeRoomIndex.X--;
                    break;
            }
        }

        /// <summary>
        /// Saves the current level to both the dev and user level folders
        /// </summary>
        public void Save(string filePath)
        {
            string jsonLevel = JsonConvert.SerializeObject(this);

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
            return JsonConvert.DeserializeObject<Level>(reader.ReadLine());
        }
    }
}
