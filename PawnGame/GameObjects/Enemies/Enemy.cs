using Newtonsoft.Json;

namespace PawnGame.GameObjects.Enemies
{
    public abstract class Enemy : Entity
    {
        [JsonIgnore]
        protected bool _isAlerted;

        [JsonIgnore]
        protected int _speed;

        public Enemy(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
            _isAlerted = false;
        }
        public abstract void Update(Player player);
        protected abstract void Move(Player player);
        protected abstract void TakeDamage(int amount);

        /// <summary>
        /// Should occur when the entity dies
        /// </summary>
        protected abstract void OnDeath(Player player);
    }
}