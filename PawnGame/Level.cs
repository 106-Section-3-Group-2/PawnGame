using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PawnGame
{
    /// <summary>
    /// 
    /// </summary>
    internal class Level
    {
        #region properties
        /// <summary>
        /// the array of tiles which make up the level
        /// </summary>
        public Tile[,] Tiles { get; set; }
        /// <summary>
        /// indexer property to access tile array
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public Tile this[int index1, int index2]
        {
            get
            {
                return Tiles[index1, index2];
            }
            set
            {
                Tiles[index1, index2] = value;
            }
        }
        /// <summary>
        /// the spawn locations of enemies
        /// </summary>
        public List<Enemy> EnemySpawns { get; set; }
        /// <summary>
        /// the spawn location of the player
        /// </summary>
        public Vector2 SpawnPoint { get; set; }
        /// <summary>
        /// the width of the entire level
        /// </summary>
        public float Width
        {
            get
            {
                return this[0, 0].Width * Tiles.GetLength(0);
            }
        }
        /// <summary>
        /// the height of the entire level
        /// </summary>
        public float Height
        {
            get
            {
                return this[0, 0].Height * Tiles.GetLength(1);
            }
        }
        /// <summary>
        /// the top-left corner of the level
        /// </summary>
        public Vector2 Location
        {
            get
            {
                return new Vector2(this[0, 0].X, this[0, 0].Y);
            }
        }
        #endregion

        /// <summary>
        /// create a level that holds an array of tiles, an array of enemies, and a spawnpoint.
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="enemies"></param>
        /// <param name="spawnPoint"></param>
        public Level(Tile[,] tiles, List<Enemy> enemies, Vector2 spawnPoint)
        {
            Tiles = tiles;
            EnemySpawns = enemies;
            SpawnPoint = spawnPoint;
        }

        /// <summary>
        /// update the level
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draw the level
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb, Vector2 location)
        {
            foreach (Tile tile in Tiles)
            {
                if(tile != null)
                {
                    tile.Draw(sb);
                }
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
                new StreamWriter(filePath, false).Write(JsonConvert.SerializeObject(level));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
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
                using StreamReader reader = new(filePath);
                return JsonConvert.DeserializeObject<Level>(reader.ReadLine());

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new FileLoadException("The file could not be found, or was corrupted.");
            }
        }
    }
}
