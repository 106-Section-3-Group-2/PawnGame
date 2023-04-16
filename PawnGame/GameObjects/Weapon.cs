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

        //Counts how long the weapon has been active
        private float _activeCounter;

        //A counter for how long the weapon has been inactive, to make it a little more forgiving
        private float _forgivenessCounter;

        private List<Vector2> _collisionVectors;

        private Color _color;

        public bool IsActive
        {
            get { return _isActive; }
        }

        


        //TODO: Add functionality for other weapons (probably a currentWeapon enum), chanages attack timer, active timer

        public Weapon(Game1.AssetNames textureKey, Vectangle vectangle) : base(textureKey, vectangle)
        {
            _isActive = false;
            _activeCounter = 0;
            _forgivenessCounter = 0;
            _collisionVectors = new List<Vector2>();
            _color = Color.White;
        }

        public void Update(Player player, VirtualMouse VMouse, GameTime gameTime)
        {
            _hitbox.Location = player.Hitbox.Location + new Vector2(player.Hitbox.Width/2,player.Hitbox.Height/2);

            if (VMouse.Speed > MathF.PI/26)
            {

                MakeCollisionVectors(VMouse);

                _isActive = true;
                _activeCounter += 1;
                _forgivenessCounter = 0;
            }
            else if (_forgivenessCounter <=10)
            {
                MakeCollisionVectors(VMouse);

                _isActive = true;
                _activeCounter += 1;
                _forgivenessCounter += 1;
            }
            else
            {
                _isActive = false;
                _activeCounter = 0;
            }

            switch (_activeCounter)
            {
                case 0:
                    _color = Color.White;
                    break;
                case > 210:
                    _color = Color.Red;
                    break;
                case >180:
                    _color = Color.Blue;
                    break;
                case > 1:
                    _color = new Color(255, 255 - (int)_activeCounter, 255 - (int)_activeCounter);
                    break;
            }

        }

        public void MakeCollisionVectors(VirtualMouse VMouse)
        {
            _collisionVectors.Clear();
            Vector2 vector1 = new Vector2(VMouse.X, VMouse.Y);
            vector1.Normalize();
            vector1 *= 100;
            Vector2 vector2 = vector1 / 5 * 4;
            Vector2 vector3 = vector1 / 5 * 3;
            Vector2 vector4 = vector1 / 5 * 2;
            Vector2 vector5 = vector1 / 5;
            _collisionVectors.Add(vector1);
            _collisionVectors.Add(vector2);
            _collisionVectors.Add(vector3);
            _collisionVectors.Add(vector4);
            _collisionVectors.Add(vector3);
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
        public void Draw(SpriteBatch sb, Player player, MouseState mouse, int windowWidth, int windowHeight)
        {
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
            //For debug, this draws the weapon red if the attack is active
            if (_isActive)
            {
                
                sb.Draw(Texture, _hitbox, null, _color, MathF.Atan2((mouse.Y - windowHeight/2), (mouse.X - windowWidth/2)) + MathF.PI / 2f, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
                //Debug.WriteLine("R:"+color.R +"G:"+ color.G + "B:"+color.B);
            }
            else
            {

                sb.Draw(Texture, _hitbox, null, Color.White, MathF.Atan2((mouse.Y - windowHeight/2), (mouse.X - windowWidth/2)) + MathF.PI /2f , new Vector2(Texture.Width/2, Texture.Height), SpriteEffects.None, 0);
            }
        }
    }
}
