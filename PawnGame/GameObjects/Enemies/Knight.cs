namespace PawnGame.GameObjects.Enemies
{
    public class Knight : Enemy
    {
        public Knight(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
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

        protected override void OnDeath(Player p)
        {
            throw new System.NotImplementedException();
        }
        protected override void TakeDamage(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
