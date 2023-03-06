using PawnGame.GameObjects.Enemies;

namespace PawnGame.GameObjects
{
    internal class Player : Entity
    {
        private Ability _activeAbility;
        private Weapon _currentWeapon;

        public Player(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
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

        private void ReadInputs()
        {
            throw new System.NotImplementedException();
        }

        private void UseAbility()
        {
            throw new System.NotImplementedException();
        }

        private void GetAbility(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }
    }
}
