namespace PawnGame.GameObjects.Enemies
{

    internal abstract class Enemy : Entity
    {
        protected bool _isAlerted;

        public Enemy(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _isAlerted = false;
            //AbilityToDrop = null;
        }
        public abstract void Update();
    }
}
