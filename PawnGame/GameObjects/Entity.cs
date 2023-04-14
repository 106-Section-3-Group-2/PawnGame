using System.Collections.Generic;

namespace PawnGame.GameObjects
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

        //*** public void Update() should be implemented in player and enemy


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
        /// Determines if an entity is in bounds of the screen or colliding with an
        /// exit or solid tile. Then resolves those collisions
        /// </summary>
        /// <param name="entity">The entity to look for</param>
        private void ResolveCollisions(Entity entity)
        {
            #region Checking Solid Tiles
            List<Tile> collisions = new List<Tile>();

            // Looping through the 2D array of tiles
            for (int i = 0; i < _currLevel.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _currLevel.Tiles.GetLength(1); j++)
                {
                    // Seeing if the current tile is a solid wall or exit tile
                    // and the entity collides with either
                    if (_currLevel.Tiles[i, j].Hitbox.Intersects(entity.Hitbox))
                    {
                        if (_currLevel.Tiles[i, j].IsSolid)
                        {
                            collisions.Add(_currLevel.Tiles[i, j]);
                        }
                        else if (_currLevel.Tiles[i, j].IsExit)
                        {
                            _currLevel = _levels[1];
                        }
                    }
                }
            }

            // Checking horizontal collisions
            for (int i = 0; i < collisions.Count; i++)
            {
                Vectangle collisionVect = collisions[i].Hitbox.GetOverlap(entity.Hitbox);

                // If the height is greater than the width, then the collision is horizontal
                if (collisionVect.Height >= collisionVect.Width)
                {
                    if (_player.X < collisions[i].X)
                    {
                        _player.X -= collisionVect.Width;
                    }
                    else
                    {
                        _player.X += collisionVect.Width;
                    }
                }
            }

            // Checking vertical collisions
            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle collisionVect = collisions[i].Hitbox.GetOverlap(entity.Hitbox);

                // If the width is greater than the height, then the collision is vertical
                if (collisionVect.Width > collisionVect.Height)
                {
                    if (_player.Y < collisions[i].Y)
                    {
                        _player.Y -= collisionVect.Height;
                    }
                    else
                    {
                        _player.Y += collisionVect.Height;
                    }
                }
            }
            #endregion

            #region Checking Outside Bounds 
            // Checking bounds of entire level just in case
            // there is no wall tile outside the perimeter

            // Left
            if (entity.X < _currLevel.Location.X)
            {
                entity.X = _currLevel.Location.X;
            }

            // Up (broken)
            if (entity.Y < _currLevel.Location.Y)
            {
                entity.Y = _currLevel.Location.Y;
            }

            // Right
            if (entity.X + entity.Width > _currLevel.Location.X + _currLevel.Width)
            {
                entity.X = _currLevel.Location.X + _currLevel.Width - entity.Width;
            }

            // Down (broken)
            if (entity.Y + entity.Height > _currLevel.Location.Y + _currLevel.Height)
            {
                entity.Y = _currLevel.Location.Y + _currLevel.Height - entity.Height;
            }
            #endregion

        }
    }
}
