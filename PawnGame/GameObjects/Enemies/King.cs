namespace PawnGame.GameObjects.Enemies
{
    public class King : Enemy
    {
        public King(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
        }

        public override void Update(Player player)
        {
            throw new System.NotImplementedException();
        }

        protected override void Attack()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move(Player player)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDeath(Player player)
        {
            throw new System.NotImplementedException();
        }
        protected override void TakeDamage(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
