namespace PawnGame.GameObjects.Enemies
{
    public class Bishop : Enemy
    {
        public Bishop(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
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

        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }
        protected override void TakeDamage(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}
