using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects.Enemies
{
    internal class EnemyManager
    {
        #region Singleton stuff
        private static EnemyManager _manager;
        public static EnemyManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                if (_manager != null) return;
                _manager = new();
            }
        }
        #endregion
        private List<Enemy> _enemies;
        private Vector2 playerPosition;

        private EnemyManager()
        {
            _enemies = null;
        }
        /// <summary>
        /// Sets the enemy list to the entered lists
        /// </summary>
        /// <param name="enemies"></param>
        public void SetActiveEnemies(List<Enemy> enemies)
        {
            _enemies = enemies;
        }
        /// <summary>
        /// Loops through all living enemies and updates them
        /// </summary>
        public void Update(Player player)
        {
            playerPosition = new Vector2(player.X,player.Y);
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (!_enemies[i].IsAlive)
                {
                    continue;
                }
                _enemies[i].Update();
            }
        }
        /// <summary>
        /// Draws all living enemies
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (!_enemies[i].IsAlive)
                {
                    continue;
                }
                _enemies[i].Draw(sb);
            }
        }
    }
}