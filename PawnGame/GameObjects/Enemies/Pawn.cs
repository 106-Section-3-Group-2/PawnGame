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
            Move();

            if (!IsAlive) OnDeath();
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
            IsAlive = false;
        }
    }
}
