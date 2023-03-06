namespace PawnGame.GameObjects
{
    internal abstract class Entity : GameObject
    {
        protected bool IsAlive;
        protected Vector2 Velocity;

        /// <summary>
        /// Makes a new entity object that is alive and has a velocity of 0
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected Entity(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            IsAlive = true;
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Should update all values needed for the entety, then move/attack
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Should move the entity based on velocity
        /// </summary>
        protected abstract void Move();
        /// <summary>
        /// Should cause the entity to attack
        /// </summary>
        protected abstract void Attack();
        /// <summary>
        /// Should occur when the entity dies
        /// </summary>
        protected abstract void OnDeath();
    }
}
