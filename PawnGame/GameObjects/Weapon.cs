using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects
{
    internal class Weapon : GameObject
    {
        public override Vectangle Hitbox { get { return _hitbox; } set { _hitbox.X = value.X; _hitbox.Y = value.Y; } }

        //If true, it hurts enemies
        private bool _isActive;

        //The amount of frames the attack takes, overall
        private int _timeToAttack;

        //How many frames of the attack the weapon is active for
        private int _timeActive;

        //A timer to keep track of how long an attack has taken
        private int _attackTimer;

        public bool IsActive
        {
            get { return _isActive; }
        }

        //TODO: Add functionality for other weapons (probably a currentWeapon enum), chanages attack timer, active timer

        public Weapon(Texture2D texture, Vectangle vectangle) : base(texture,vectangle)
        {
            _isActive = false;
            _timeToAttack = 45;
            _timeActive = 30;
            _attackTimer = 0;
        }

        public void Update(Player player)
        {
            _hitbox.Location = player.Hitbox.Location + new Vector2(player.Hitbox.Width/2,player.Hitbox.Height/2);
            if(_attackTimer > 0)
            {
                Attack();
                _attackTimer--;
            }

        }

        public void Attack()
        {
            Matrix.CreateRotationZ;
            if (_attackTimer == 0)
            {
                _isActive = true;
                _attackTimer = _timeToAttack;
            }
            else if (_attackTimer < _timeToAttack-_timeActive)
            {
                _isActive = false;
            }

        }

        public void Draw(SpriteBatch sb, Player player, MouseState mouse)
        {
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);

            //For debug, this draws the weapon red if the attack is active
            if (_isActive)
            {
                sb.Draw(_texture, _hitbox, null, Color.Red, MathF.Atan2((mouse.Y - player.Hitbox.Y), (mouse.X - player.Hitbox.X)) + MathF.PI / 2, new Vector2(16, 32), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(_texture, _hitbox, null, Color.White, MathF.Atan2((mouse.Y - player.Hitbox.Y), (mouse.X - player.Hitbox.X)) + MathF.PI / 2, new Vector2(16, 32), SpriteEffects.None, 0);
            }
            sb.Draw(Game1._debugTexture, _hitbox, Color.Pink);
        }
    }
}
