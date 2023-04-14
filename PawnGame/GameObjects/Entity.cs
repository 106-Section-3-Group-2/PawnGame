﻿namespace PawnGame.GameObjects
{
    public abstract class Entity : GameObject
    {
        protected bool _isAlive;
        protected Vector2 _velocity;

        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
            }
        }

        /// <summary>
        /// Makes a new entity object that is alive and has a velocity of 0
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected Entity(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
            _isAlive = true;
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// Should cause the entity to attack
        /// </summary>
        protected virtual void Attack()
        {

        }

        /// <summary>
        /// Should occur when the entity dies
        /// </summary>
        protected abstract void OnDeath();

        /// <summary>
        /// Resolves any collisions with the game object entered.
        /// </summary>
        /// <param name="other">Game object that is coliding with the current one</param>
        protected void ResolveCollisions(Tile other)
        {
            Vectangle overlap = _hitbox.GetOverlap(other.Hitbox);

            if (overlap.Width < overlap.Height && other.X < X)
            {
                X += overlap.Width;
            }
            else if (overlap.Width < overlap.Height && other.X > X)
            {
                X -= overlap.Width;
            }

            if (overlap.Height <= overlap.Width && other.Y < Y)
            {
                Y += overlap.Height;
            }
            else if (overlap.Height <= overlap.Width && other.Y > Y)
            {
                Y -= overlap.Height;
            }
        }
    }
}
