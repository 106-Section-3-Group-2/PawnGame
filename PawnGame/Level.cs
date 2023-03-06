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
        private Tile[,] tiles;
        private Enemy[] enemySpawns;
        private Vector2 spawnPoint;
        #endregion

        /// <summary>
        /// create a level that holds an array of tiles, an array of enemies, and a spawnpoint.
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="enemies"></param>
        /// <param name="spawnPoint"></param>
        public Level(Tile[,] tiles, Enemy[] enemies, Vector2 spawnPoint)
        {
            this.tiles = tiles;
            this.enemySpawns = enemies;
            this.spawnPoint = spawnPoint;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(Tile tile in tiles)
            {
                tile.Draw(sb);
            }
        }
    }
}
