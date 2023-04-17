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

            for (int i = 0; i < Game1.CurrentLevel.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Game1.CurrentLevel.Tiles.GetLength(1); j++)
                {
                    if (CheckCollision(Game1.CurrentLevel.Tiles[i, j]))
                    {
                        if (Game1.CurrentLevel.Tiles[i, j].IsSolid)
                        {
                            ResolveCollisions(Game1.CurrentLevel.Tiles[i, j]);
                        }
                    }
                }
            }

            CheckPlayerCollision(player);
            CheckWeaponCollision(player.Weapon);
            if (!_isAlive)
            {
                player.GetAbility(Player.Ability.Pawn);
                OnDeath();
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

        protected override void OnDeath()
        {
        }
        protected override void TakeDamage(int amount)
        {
            _isAlive = false;
        }
    }
}