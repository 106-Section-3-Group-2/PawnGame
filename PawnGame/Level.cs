using Newtonsoft.Json;

namespace PawnGame
{
    internal class Level
    {
        /// <summary>
        /// Grid of rooms in the level
        /// </summary>
        [JsonProperty]
        private Room[,] _rooms;

        /// <summary>
        /// Gets the room at the specified cordinates
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        [JsonIgnore]
        public Room this[int i, int j]
        {
            get
            {
                return _rooms[i, j];
            }
        }

        public Level()
        {
            _rooms = new Room[5,5];
        }
    }
}
