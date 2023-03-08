namespace PawnGame.GameObjects.Enemies
{
    public delegate void Ability();
    internal abstract class Enemy : Entity
    {
        protected bool IsAlerted;
        protected Ability AbilityToDrop;

        public Enemy(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
        
            
        }
        public void Update()
        {

        }
    }
}
