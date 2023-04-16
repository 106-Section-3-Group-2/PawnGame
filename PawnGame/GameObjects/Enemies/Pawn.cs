using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PawnGame.GameObjects.Enemies.EnemyManager;

namespace PawnGame.GameObjects.Enemies
{
    public class Pawn : Enemy
    {
        public Pawn(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
            _speed = 4;
        }

        public override void Update(Player player)
        {
            Move(player);
            CheckPlayerCollision(player);
            CheckWeaponCollision(player.Weapon);
            if (!_isAlive) OnDeath();
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
            if (CheckCollision(player))
            {
                player.IsAlive = false;
            }
        }
        protected void CheckWeaponCollision(Weapon weapon)
        {
            if (CheckCollision(weapon) )//&& weapon.IsActive == true)
            {
                IsAlive = false;
            }
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