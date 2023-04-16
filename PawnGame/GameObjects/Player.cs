using PawnGame.GameObjects.Enemies;
namespace PawnGame.GameObjects
{
    public class Player : Entity
    {
        private enum PlayerState
        {
            Moving,
            Abilitying,
            NoControl,
        }
        public enum Ability
        {
            None,
            Pawn,
            Knight,
            Bishop,
            Rook,
            Queen,
        }

        private Ability _heldAbility;
        private Ability _activeAbility;
        private Weapon _currentWeapon;
        private int _speed;
        private PlayerState _playerState;
        private int _abilityTimer;        

        public Weapon Weapon
        {
            get { return _currentWeapon; }
        }

        public Player(Game1.AssetNames textureKey, Rectangle hitbox, Weapon weapon) : base(textureKey, hitbox)
        {
            _abilityTimer = 0;
            _speed = 5;
            _heldAbility = Ability.None;
            _activeAbility = Ability.None;
            _currentWeapon = weapon;
            
        }

        public void Update(KeyboardState currentKBState, KeyboardState previousKBState, MouseState currentMouseState, MouseState prevMouseState, Level level)
        {
            ReadInputs(currentKBState, previousKBState,currentMouseState,prevMouseState);
            Move();
            KeepInBounds(level.Width, level.Height);
            
            for (int i = 0; i < level.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < level.Tiles.GetLength(1); j++)
                {
                    if (CheckCollision(level.Tiles[i, j]))
                    {
                        if (level.Tiles[i, j].IsSolid)
                        {
                            ResolveCollisions(level.Tiles[i, j]);
                        }
                        else if (level.Tiles[i, j].IsExit)
                        {
                            Game1.LevelIndex++; 
                        }
                    }
                }
            }
        }
        /*
        protected void Attack()
        {
            if (_activeAbility == Ability.None)
            {
                _currentWeapon.Attack();
            }
        }
        */
        protected void Move()
        {
            switch (_playerState)
            {
                case PlayerState.Moving:
                    if (!_isAlive)
                    {
                        _playerState = PlayerState.NoControl;
                    }


                    _hitbox.X += _velocity.X;
                    _hitbox.Y += _velocity.Y;

                    break;

                case PlayerState.Abilitying:
                    if (!_isAlive)
                    {
                        _playerState = PlayerState.NoControl;
                    }

                    if (_abilityTimer > 0)
                    {
                        switch (_activeAbility)
                        {
                            case Ability.Pawn:
                                break;

                            case Ability.Knight:
                                break;

                            case Ability.Bishop:
                                break;

                            case Ability.Rook:
                                break;

                            case Ability.Queen:
                                break;

                            case Ability.None:
                                throw new System.Exception("Error! Entered Abilitying state without an active ability");
                        }
                        _abilityTimer--;
                    }
                    else
                    {
                        _playerState = PlayerState.Moving;
                    }

                    break;

                case PlayerState.NoControl:
                    break;
            }

            
        }

        private void ReadInputs(KeyboardState currentKBState, KeyboardState previousKBState,MouseState currentMouseState, MouseState prevMouseState)
        {
            //Returns a normalized 2D vector of player's input direction
            Vector2 direction = GetDirection(currentKBState);

            switch (_playerState)
            {
                case PlayerState.Moving: //If the player is able to move, but is not taking other special actions
                                         //Check WASD, Space, and mouse
                    _velocity = _speed * direction;
                    //Space
                    if (currentKBState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(direction);
                    }
                    /*
                    //Mouse
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Attack();
                    }
                                        */
                    break;

                case PlayerState.Abilitying: //If the player is using an ability that makes them dash
                                             //WASD do not change velocity, but still change direction for abilities
                                             //Check WASD and Space
                                             //Space
                    if (currentKBState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(direction);
                    }
                    break;

                case PlayerState.NoControl: //If the player is not able to move (cutscenes)
                    break;

                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (_isAlive)
            {
                sb.Draw(Texture, _hitbox, Color.White);
            }
            
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="currentKBState"></param>
        /// <returns></returns>
        private Vector2 GetDirection(KeyboardState currentKBState)
        {

            Vector2 output = Vector2.Zero;
            //Up
            if (currentKBState.IsKeyDown(Keys.W))
            {
                output.Y -= 1;
            }
            //Down
            if (currentKBState.IsKeyDown(Keys.S))
            {
                output.Y += 1;
            }
            //Right
            if (currentKBState.IsKeyDown(Keys.D))
            {
                output.X += 1;
            }
            //Left
            if (currentKBState.IsKeyDown(Keys.A))
            {
                output.X -= 1;
            }

            if (output != Vector2.Zero)
            {
                output.Normalize();
            }
            
            return output;
        }

        private void UseAbility(Vector2 direction)
        {
            if (_heldAbility != Ability.None)
            {
                _playerState = PlayerState.Abilitying;
                switch (_heldAbility)
                {
                    case Ability.Pawn:
                        _activeAbility = Ability.Pawn;
                        _abilityTimer = 40;
                        break;

                    case Ability.Knight:
                        _activeAbility = Ability.Knight;
                        break;

                    case Ability.Bishop:
                        _activeAbility = Ability.Bishop;
                        break;

                    case Ability.Rook:
                        _activeAbility = Ability.Rook;
                        break;

                    case Ability.Queen:
                        _activeAbility = Ability.Queen;
                        break;
                }
                _heldAbility = Ability.None;
            }

        }

        public void GetAbility(Ability ability)
        {
            if (_heldAbility == Ability.None)
            {
                _heldAbility = ability;
            }
        }

        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }
    }
}