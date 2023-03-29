﻿namespace PawnGame.GameObjects.Enemies
{
    internal abstract class Enemy : Entity
    {
        protected bool _isAlerted;
        protected int _speed;

        public Enemy(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _isAlerted = false;
        }
        public abstract void Update();
    }
}