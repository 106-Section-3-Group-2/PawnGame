using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using static PawnGame.Game1;

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

        [JsonProperty]
        private List<Enemy> _enemies;

        [JsonProperty]
        private bool _cleared;

        [JsonIgnore]
        public List<Enemy> Enemies => _enemies;

        [JsonIgnore]
        public List<Enemy> ActiveEnemies
        {
            get
            {
                List<Enemy> enemies = new();

                for (int i = 0; i < Enemies.Count; i++)
                {
                    enemies.Add(Enemies[i].Clone() as Enemy);
                }

                return enemies;
            }
        }

        public bool Cleared
        {
            get
            {
                return _cleared;
            }
            set
            {
                _cleared = value;
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

            _enemies = new();
        }

        /// <summary>
        /// constructor that pre-populates array
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tileSize"></param>
        public Room(int width, int height)
        {
            int sideLength = RenderTargetHeight / height;
            int margin = (RenderTargetWidth / 2) - width * sideLength / 2;
            Tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if ((x + y) % 2 == 0)
                    {
                        Tiles[x, y] = new Tile(AssetNames.TileBlack, new Rectangle(margin + x * sideLength, y * sideLength, sideLength, sideLength), false);
                    }
                    else
                    {
                        Tiles[x, y] = new Tile(AssetNames.TileWhite, new Rectangle(margin + x * sideLength, y * sideLength, sideLength, sideLength), false);
                    }
                }
            }
            //default spawn point
            SpawnPoint = Tiles[0, 0].Hitbox.Location;

            _enemies = new();
        }

        /// <summary>
        /// create a room with enemies (this is for serialization)
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
        public Room(Tile[,] tiles, List<Enemy> enemies)
        {
            Tiles = tiles;
            _enemies = enemies;
        }

        /// <summary>
        /// update the level
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update()
        {
            if (EnemyManager.Manager.Count == 0)
            {
                _cleared = true;
            }
        }

        /// <summary>
        /// draw the level
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in Tiles)
            {
                tile?.Draw(sb);
            }
        }

        /// <summary>
        /// If the room has not been cleared already, set the room with its enemies
        /// </summary>
        public void ActivateRoom()
        {
            if (!_cleared)
            {
                EnemyManager.Manager.Clear();
                EnemyManager.Manager.AddRange(ActiveEnemies);
            }
        }
    }
}
