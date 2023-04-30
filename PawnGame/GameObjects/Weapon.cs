using System;
using System.Collections.Generic;


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

        private List<Vector4> _lastFramesSword;


        private int _currentSpeed;

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
            _currentSpeed = 1;
            _collisionVectors = new List<Vector2>();
            _lastCollisionVectors = new List<Vector2>();
            _lastFramesSword = new List<Vector4>();
            _color = Color.White;

        }
        
        public void Update(Player player, VirtualMouse VMouse)
        {   if (player.IsAlive)
            {
                _hitbox.Location = player.Hitbox.Location + new Vector2(player.Hitbox.Width / 2, player.Hitbox.Height / 2);

                Vector4 swordVector = new Vector4(MathF.Cos(VMouse.Rotation), MathF.Sin(VMouse.Rotation),_hitbox.X,_hitbox.Y);
                if (_lastFramesSword.Count < 10)
                {
                    _lastFramesSword.Add(swordVector);
                }
                else
                {
                    _lastFramesSword.RemoveAt(0);
                    _lastFramesSword.Add(swordVector);
                }
                
                
                if (player.State == Player.PlayerState.Dizzy)
                {
                    _activeCounter = -2;
                }
                else if (player.WeaponOverride)
                {
                    MakeCollisionVectors(VMouse);
                    _activeCounter = -1;
                }
                else if (Math.Abs(VMouse.Speed) > MathF.PI / 26)
                {
                    if (_activeCounter == 0)
                    {
                        _currentSpeed = Math.Sign(VMouse.Speed);
                    }
                    
                    if (Math.Sign(_currentSpeed)!=Math.Sign(VMouse.Speed))
                    {
                        _activeCounter += 10;
                    }

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
                    _currentSpeed = 0;
                }
                switch (_activeCounter)
                {
                    case -2:
                        _color = Color.Black;
                        _isActive = false;
                        break;
                    case -1:
                        _color = Color.Green;
                        _isActive = true;
                        break;
                    case 0:
                        _color = Color.White;
                        _isActive = false;
                        break;
                    case > 250:
                        _color = Color.White;
                        _isActive = false;
                        _activeCounter = 0;
                        player.Dizzy = 150;
                        break;
                    case > 0:
                        _color = new Color(255*_activeCounter/250, 255-255*_activeCounter / 250, 0);
                        _isActive = true;
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
            vector1 *= 200;
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
            Matrix rotation = Matrix.CreateRotationZ(theta/2);
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
                else if (vectangle.Contains(Vector2.Transform(_lastCollisionVectors[i], rotation) + Hitbox.Location))
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch sb, Player player, float rotation,float scale)
        {
            if (player.IsAlive)
            {
                if(player.State == Player.PlayerState.Abilitying)
                {
                    for (int i = 0; i < _lastFramesSword.Count; i++)
                    {
                        Color alphaColor = new Color(_color.R,_color.G,_color.B,(0 + (255 * i / _lastFramesSword.Count)));
                        //Color alphaColor = new Color(255, 255, 255, (0 + (255 * i / _lastFramesSword.Count)));
                        Vectangle tempPosition = new Vectangle(_lastFramesSword[i].Z, _lastFramesSword[i].W, 100, 100);
                        sb.Draw(Texture, tempPosition, null, alphaColor, MathF.Atan2(_lastFramesSword[i].Y, _lastFramesSword[i].X) + MathF.PI / 2, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < _lastFramesSword.Count; i++)
                    {
                        Color alphaColor = new Color(_color.R,_color.G,_color.B,(0 + (255 * i / _lastFramesSword.Count)));
                        //Color alphaColor = new Color(255, 255, 255, (0 + (255 * i / _lastFramesSword.Count)));
                        sb.Draw(Texture, _hitbox, null, alphaColor, MathF.Atan2(_lastFramesSword[i].Y, _lastFramesSword[i].X) + MathF.PI / 2, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);
                    }
                }
                
                
                
                sb.Draw(Texture, _hitbox, null, _color, rotation + MathF.PI / 2, new Vector2(_hitbox.Width / 2, _hitbox.Height), SpriteEffects.None, 0);

                /*
                foreach (Vector2 vector in _collisionVectors)
                {
                    sb.Draw(Texture, _hitbox, null, _color, rotation + MathF.PI / 2, new Vector2(Texture.Width / 2, vector.Length()), SpriteEffects.None, 0);
                }
                */

            }
        }
    }
}
