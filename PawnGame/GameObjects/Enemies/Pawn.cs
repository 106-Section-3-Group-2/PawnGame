using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PawnGame.GameObjects.Enemies.EnemyManager;

namespace PawnGame.GameObjects.Enemies
{
    internal class Pawn : Enemy
    {
        public Pawn(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _speed = 4;
        }

        public override void Update()
        {
            Move();
            if (!_isAlive) OnDeath();
        }

        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            Vector2 moveVector = Manager.PlayerPosition-new Vector2(_hitbox.X,_hitbox.Y);
           
            moveVector.Normalize();
            moveVector *= _speed;
            _hitbox.Location = (_hitbox.Location.ToVector2() + moveVector).ToPoint();
        }

        protected override void OnDeath()
        {
            throw new NotImplementedException();
        }
    }
}