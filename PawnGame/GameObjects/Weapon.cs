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
        private int _activeCounter;

        //A counter for how long the weapon has been inactive, to make it a little more forgiving
        private float _forgivenessCounter;

        private List<Vector2> _collisionVectors;

        private List<Vector2> _lastCollisionVectors;

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
            _lastCollisionVectors = new List<Vector2>();
            _color = Color.White;
        }
        
        public void Update(Player player, VirtualMouse VMouse)
        {   if (player.IsAlive)
            {
                _hitbox.Location = player.Hitbox.Location + new Vector2(player.Hitbox.Width / 2, player.Hitbox.Height / 2);
                if (player.WeaponOverride)
                {
                    _activeCounter = 1000;
                }
                else if (Math.Abs(VMouse.Speed) > MathF.PI / 26)
                {

                    MakeCollisionVectors(VMouse);


                    _activeCounter += 1;
                    _forgivenessCounter = 0;
                }
                else if (_forgivenessCounter <= 5)
                {
                    MakeCollisionVectors(VMouse);


                    _activeCounter += 1;
                    _forgivenessCounter += 1;
                }
                else
                {
                    _activeCounter = 0;
                }
                switch (_activeCounter)
                {
                    case 0:
                        _color = Color.Black;
                        _isActive = false;
                        break;
                    case > 70:
                        _isActive = true;
                        _color = Color.Red;
                        break;
                    case > 60:
                        _isActive = true;
                        _color = Color.Blue;
                        break;
                    case > 0:
                        _isActive = false;
                        _color = new Color(0, 0, _activeCounter * 4);
                        break;
                }
            }
        }

        public void MakeCollisionVectors(VirtualMouse VMouse)
        {
            _lastCollisionVectors.Clear();
            for (int i = 0; i < _collisionVectors.Count; i++)
            {
                _lastCollisionVectors.Add(_collisionVectors[i]);
            }
            _collisionVectors.Clear();
            Vector2 vector1 = new Vector2(MathF.Cos(VMouse.Rotation), MathF.Sin(VMouse.Rotation));
            //Vector2 vector1 = new Vector2(VMouse.X, VMouse.Y);
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
            _collisionVectors.Add(vector5);
        }

        public bool IsColliding(Vectangle vectangle)
        {
            float theta = MathF.Atan2(_lastCollisionVectors[0].Y, _lastCollisionVectors[0].X) - MathF.Atan2(_collisionVectors[0].Y, _collisionVectors[0].X);
            Matrix rotation = Matrix.CreateRotationZ(theta);
            for (int i = 0; i < _collisionVectors.Count; i++)
            {
                if (vectangle.Contains(_collisionVectors[i] + Hitbox.Location))
                {
                    return true;
                }
                else if (vectangle.Contains(_lastCollisionVectors[i] + Hitbox.Location))
                {
                    return true;
                }
                else if (vectangle.Contains(Vector2.Transform(_lastCollisionVectors[i], rotation)))
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch sb, Player player, MouseState mouse, int windowWidth, int windowHeight)
        {
            if (player.IsAlive)
            {
                sb.Draw(Texture, _hitbox, null, _color, MathF.Atan2((mouse.Y - windowHeight / 2), (mouse.X - windowWidth / 2)) + MathF.PI / 2f, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
            }
            
        }
        public void Draw(SpriteBatch sb, Player player, float rotation)
        {
            if (player.IsAlive)
            {
                sb.Draw(Texture, _hitbox, null, _color, rotation + MathF.PI / 2, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
            }
        }
    }
}
