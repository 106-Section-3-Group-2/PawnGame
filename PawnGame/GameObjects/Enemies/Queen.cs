﻿using System;

namespace PawnGame.GameObjects.Enemies
{
    public class Queen : Enemy
    {
        public Queen(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
        }

        public override void Update(Player player)
        {
            throw new NotImplementedException();
        }

        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Move(Player player)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeath()
        {
            throw new NotImplementedException();
        }
        protected override void TakeDamage(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
