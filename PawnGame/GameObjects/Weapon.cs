using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeUtils;

namespace PawnGame.GameObjects
{
    public class Weapon : GameObject
    {

        //If true, it hurts enemies
        private bool _isActive;

        //The amount of frames the attack takes, overall
        private int _timeToAttack;

        //How many frames of the attack the weapon is active for
        private int _timeActive;

        //A timer to keep track of how long an attack has taken
        private int _attackTimer;

        private List<Vector2> _collisionVectors;

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
            _collisionVectors = new List<Vector2>();
        }

        public void Update(Player player, MouseState currMouseState)
        {
            _hitbox.Location = player.Hitbox.Location + new Vector2(player.Hitbox.Width/2,player.Hitbox.Height/2);
            if(_attackTimer > 0)
            {
                Attack();
                _attackTimer--;
            }

            //Create vectors for collision
            if (IsActive)
            {

                Vector2 vector1 = new Vector2(currMouseState.X - player.X, currMouseState.Y - player.Y);
                vector1.Normalize();
                vector1 *= 145;
                Vector2 vector2 = vector1*0.6f;
                Vector2 vector3 = vector1*0.3f;

                _collisionVectors.Add(vector1);
                _collisionVectors.Add(vector2);
                _collisionVectors.Add(vector3);
                ///TODO: Ask chris how to do triangle collision :(
                /*
                Vector2 vector1 = new Vector2(currMouseState.X - player.X, currMouseState.Y - player.Y);
                vector1.Normalize();
                vector1*=20;
                //20 is a length property for the weapon

                Vector2 vector2 = Vector2.Transform(vector1, Matrix.CreateRotationZ(MathF.PI * 0.125f));
                Vector2 vector3 = Vector2.Transform(vector1, Matrix.CreateRotationZ(MathF.PI * -0.125f));
                _collisionVectors.Add(vector2);
                _collisionVectors.Add(vector3);
                */
            }
        }
        public bool IsColliding(Vectangle vectangle)
        {
            for (int i = 0; i < _collisionVectors.Count; i++)
            {
                if (vectangle.Contains(_collisionVectors[i]+Hitbox.Location))
                {
                    return true;
                }
            }
            return false;
        }

        public void Attack()
        {
            if (_attackTimer == 0)
            {
                _isActive = true;
                _attackTimer = _timeToAttack;
            }
            else if (_isActive && _attackTimer < _timeToAttack-_timeActive)
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
        }
    }
}
