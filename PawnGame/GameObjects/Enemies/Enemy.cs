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

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        protected override void Attack()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}
