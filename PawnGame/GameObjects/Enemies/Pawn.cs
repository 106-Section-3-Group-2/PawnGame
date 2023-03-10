using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects.Enemies
{
    internal class Pawn : Enemy
    {
        public Pawn(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
        }

        public override void Update()
        {
<<<<<<< HEAD
            Move();

            if (!_isAlive) OnDeath();
=======
            throw new NotImplementedException();
>>>>>>> parent of 7b2df1c (Merge branch 'main' of https://github.com/106-Section-3-Group-2/PawnGame)
        }

        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }

        protected override void OnDeath()
        {
<<<<<<< HEAD
            _isAlive = false;
=======
            throw new NotImplementedException();
>>>>>>> parent of 7b2df1c (Merge branch 'main' of https://github.com/106-Section-3-Group-2/PawnGame)
        }
    }
}
