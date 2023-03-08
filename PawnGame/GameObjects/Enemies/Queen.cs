using System;

namespace PawnGame.GameObjects.Enemies
{
    internal class Queen : Enemy
    {
        public Queen(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
        }

        public override void Update()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
