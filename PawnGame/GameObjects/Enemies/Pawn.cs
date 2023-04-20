using System;

namespace PawnGame.GameObjects.Enemies
{
    public class Pawn : Enemy
    {
        public Pawn(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
            _speed = 2;
        }

        public override void Update(Player player)
        {
            Move(player);
            KeepInBounds();
            ManageTileCollisions();

            CheckPlayerCollision(player);
            CheckWeaponCollision(player.Weapon);

            if (!_isAlive)
            {
                OnDeath(player);
            }
        }

        protected override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void Move(Player player)
        {
            Vector2 moveVector = new Vector2(player.X,player.Y) -new Vector2(_hitbox.X,_hitbox.Y);
            if(moveVector.Length()> _speed)
            {
                moveVector.Normalize();
                moveVector *= _speed;
            }
            _hitbox.Location = _hitbox.Location + moveVector;
        }
        protected void CheckPlayerCollision(Player player)
        {
            if (CheckCollision(player)&&(player.IsInvincible==false))
            {
                player.IsAlive = false;
            }
        }
        protected void CheckWeaponCollision(Weapon weapon)
        {
            if (CheckCollision(weapon) && weapon.IsActive == true)
            {
                IsAlive = false;
            }
            /*
            if (CheckCollision(weapon) && weapon.IsActive == false)
            {
                Vector2 launch = new Vector2(X-weapon.Hitbox.X, Y-weapon.Hitbox.Y);
                launch.Normalize();
                Matrix rotation = Matrix.CreateRotationZ(VirtualMouse.VMouse.Speed);
                launch *= 100;
                launch = Vector2.Transform(launch, rotation);

                X += launch.X;
                Y += launch.Y;
            }
            */
            
        }

        protected override void OnDeath(Player player)
        {
            player.GetAbility(Player.Ability.Pawn);
        }
        protected override void TakeDamage(int amount)
        {
            _isAlive = false;
        }
    }
}