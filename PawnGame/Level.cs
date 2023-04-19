using Newtonsoft.Json;
using System.IO;

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
        public Room this[int i, int j] => _rooms[i, j];

        public Level()
        {
            _rooms = new Room[5,5];
        }

        /// <summary>
        /// Saves the current level to both the dev and user level folders
        /// </summary>
        public void Save(string fileName)
        {
            string jsonLevel = JsonConvert.SerializeObject(this);

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Levels\")))
            {
                using StreamWriter devWriter = new(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Levels\" + fileName));
                devWriter.Write(jsonLevel);
            }

            using StreamWriter gameWriter = new(Directory.GetCurrentDirectory() + @"\Levels" + fileName);
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
