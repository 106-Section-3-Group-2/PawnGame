using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace PawnGame
{
    /// <summary>
    /// 
    /// </summary>
    internal class Level
    {
        #region fields
        /// <summary>
        /// the array of tiles which make up the level
        /// </summary>
        public Tile[,] Tiles { get; set; }
        /// <summary>
        /// the spawn locations of enemies
        /// </summary>
        public Enemy[] EnemySpawns { get; set; }
        /// <summary>
        /// the spawn location of the player
        /// </summary>
        public Vector2 SpawnPoint { get; set; }
        #endregion

        /// <summary>
        /// create a level that holds an array of tiles, an array of enemies, and a spawnpoint.
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="enemies"></param>
        /// <param name="spawnPoint"></param>
        public Level(Tile[,] tiles, Enemy[] enemies, Vector2 spawnPoint)
        {
            Tiles = tiles;
            EnemySpawns = enemies;
            SpawnPoint = spawnPoint;
        }

        /// <summary>
        /// draw the level
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach(Tile tile in Tiles)
            {
                tile.Draw(sb);
            }
        }

        /// <summary>
        /// writes the level to a filePath
        /// </summary>
        /// <param name="level"></param>
        /// <param name="filePath"></param>
        public static void Write(Level level, string filePath)
        {
            try
            {
                new StreamWriter(filePath, false).Write(JsonSerializer.Serialize(level));
            }
            catch
            {
                throw new Exception("Could not write the file.");
            }
        }

        /// <summary>
        /// returns a level loaded from the filePath. Throws an exception if there was an error writing to the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Level Read(string filePath)
        {
            try
            {
                return JsonSerializer.Deserialize<Level>(filePath);
            }
            catch
            {
                throw new FileLoadException("The file could not be found, or was corrupted.");
            }
        }
    }
}
