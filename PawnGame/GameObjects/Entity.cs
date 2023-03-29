namespace PawnGame.GameObjects
{
    internal abstract class Entity : GameObject
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
        protected Entity(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _isAlive = true;
            _velocity = Vector2.Zero;
        }

        //*** public void Update() should be implemented in player and enemy


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
