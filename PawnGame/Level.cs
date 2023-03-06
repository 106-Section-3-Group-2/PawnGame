using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
