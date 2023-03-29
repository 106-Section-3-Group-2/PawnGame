using System;
using System.Collections.Generic;


namespace PawnGame.GameObjects.Enemies
{
    internal class EnemyManager
    {
        #region Singleton stuff
        private static EnemyManager _manager = new();
        public static EnemyManager Manager => _manager;
        
        #endregion

        private List<Enemy> _enemies;

        private EnemyManager()
        {
            _enemies = new List<Enemy>();
        }

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
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
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (!_enemies[i].IsAlive)
                {
                    continue;
                }
                _enemies[i].Update(player);
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