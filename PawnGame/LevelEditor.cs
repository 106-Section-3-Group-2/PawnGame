using PawnGame.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawnGame;
using PawnGame.GameObjects;
using PawnGame.GameObjects.Enemies;


namespace PawnGame
{
    internal class LevelEditor
    {
        #region fields
        private string _filePath;
        private Tile[] _palette;
        private Level _level;
        #endregion

        public LevelEditor(int x, int y)
        {
            _level = new Level(new Tile[x, y], new List<Enemy>(), new Vector2());
        }

        public void Draw(SpriteBatch sb)
        {

        }

        public void Update()
        {

        }
    }
}
