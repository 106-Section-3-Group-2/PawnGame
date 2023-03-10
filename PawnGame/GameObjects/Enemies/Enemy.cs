namespace PawnGame.GameObjects.Enemies
{
    internal abstract class Enemy : Entity
    {
        protected bool IsAlerted;
        public Enemy(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            IsAlerted = false;
            //AbilityToDrop = null;
        }
        public abstract void Update();
    }
}