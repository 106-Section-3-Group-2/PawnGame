using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PawnGame
{
    /// <summary>
    /// holds an array of tiles and lists of enemy. Serializable and has static functions to save and load.
    /// </summary>
    public class Room
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

        //lists of enemies
        public List<Pawn> PawnSpawns { get; set; }
        public List<Bishop> BishopSpawns { get; set; }
        public List<Rook> RookSpawns { get; set; }
        public List<Knight> KnightSpawns { get; set; }
        public List<Queen> QueenSpawns { get; set; }
        public List<King> KingSpawns { get; set; }
        [JsonIgnore]
        public List<Enemy> EnemySpawns
        {
            get
            {
                List<Enemy> enemies = new();
                enemies.AddRange(PawnSpawns);
                enemies.AddRange(BishopSpawns);
                enemies.AddRange(RookSpawns);
                enemies.AddRange(KnightSpawns);
                enemies.AddRange(QueenSpawns);
                enemies.AddRange(KingSpawns);
                return enemies;
            }
        }

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
        /// create a level that holds an array of tiles, and a spawnpoint.
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="enemies"></param>
        /// <param name="spawnPoint"></param>
        public Room(Tile[,] tiles, Vector2 spawnPoint)
        {
            Tiles = tiles;
            SpawnPoint = spawnPoint;
            PawnSpawns = new();
            BishopSpawns = new();
            RookSpawns = new();
            KnightSpawns = new();
            QueenSpawns = new();
            KingSpawns = new();
        }

        /// <summary>
        /// create a level with enemies (this is for serialization)
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="pawnSpawns"></param>
        /// <param name="bishopSpawns"></param>
        /// <param name="rookSpawns"></param>
        /// <param name="knightSpawns"></param>
        /// <param name="queenSpawns"></param>
        /// <param name="kingSpawns"></param>
        /// <param name="spawnPoint"></param>
        [JsonConstructor]
        public Room(Tile[,] tiles, List<Pawn> pawnSpawns, List<Bishop> bishopSpawns, List<Rook> rookSpawns, List<Knight> knightSpawns, List<Queen> queenSpawns, List<King> kingSpawns, Vector2 spawnPoint)
        {
            Tiles = tiles;
            PawnSpawns = pawnSpawns;
            BishopSpawns = bishopSpawns;
            RookSpawns = rookSpawns;
            KnightSpawns = knightSpawns;
            QueenSpawns = queenSpawns;
            KingSpawns = kingSpawns;
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
        public void Draw(SpriteBatch sb)
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
        public static void Write(Room level, string filePath)
        {
            try
            {
                using StreamWriter writer = new(filePath);
                writer.Write(JsonConvert.SerializeObject(level));
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
        public static Room Read(string filePath)
        {
            using StreamReader reader = new(filePath);
            return JsonConvert.DeserializeObject<Room>(reader.ReadLine());
        }
    }
}
