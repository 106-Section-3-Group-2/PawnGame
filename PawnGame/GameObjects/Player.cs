using PawnGame.GameObjects.Enemies;
namespace PawnGame.GameObjects
{
    internal class Player : Entity
    {
        private enum PlayerState
        {
            Moving,
            Abilitying,
            NoControl,
            Attacking,
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
        private float _posX;
        private float _posY;
        public float X
        {
            get
            {
                return _posX;
            }
        }

        public float Y
        {
            get
            {
                return _posY;
            }
        }

        public Player(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _abilityTimer = 0;
            _speed = 3;
            _heldAbility = Ability.None;
            _activeAbility = Ability.None;
        }
        public void Update(KeyboardState currentState, KeyboardState previousState)
        {
            ReadInputs(currentState, previousState);
            Move();

        }
        protected override void Attack()
        {
            if (_activeAbility == Ability.None)
            {
            }
        }
        protected override void Move()
        {
            switch (_playerState)
            {
                case PlayerState.Moving:
                    _posX += _velocity.X;
                    _posY += _velocity.Y;

                    _hitbox.Location = new Point((int)_posX, (int)_posY);
                    break;

                case PlayerState.Abilitying:

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
                                break;
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

        private void ReadInputs(KeyboardState currentState, KeyboardState previousState)
        {
            //Returns a normalized 2D vector of player's input direction
            Vector2 direction = GetDirection(currentState);

            switch (_playerState)
            {
                case PlayerState.Moving: //If the player is able to move, but is not taking other special actions
                                         //Check WASD, Space, and mouse
                    _velocity = _speed * direction;
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(direction);
                    }
                    //Mouse
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Attack();
                    }
                    break;

                case PlayerState.Attacking: //If the player is able to move, but is not taking other special actions
                                            //Check WASD, Space, and mouse
                    _velocity = _speed * direction;
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(direction);
                    }
                    break;

                case PlayerState.Abilitying: //If the player is using an ability that makes them dash
                                             //WASD do not change velocity, but still change direction for abilities
                                             //Check WASD and Space
                                             //Space
                    if (currentState.IsKeyDown(Keys.Space))
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
        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private Vector2 GetDirection(KeyboardState currentState)
        {

            Vector2 output = Vector2.Zero;
            //Up
            if (currentState.IsKeyDown(Keys.W))
            {
                output.Y -= 1;
            }
            //Down
            if (currentState.IsKeyDown(Keys.S))
            {
                output.Y += 1;
            }
            //Right
            if (currentState.IsKeyDown(Keys.D))
            {
                output.X += 1;
            }
            //Left
            if (currentState.IsKeyDown(Keys.A))
            {
                output.X -= 1;
            }
            output.Normalize();
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
        private void GetAbility(Ability ability)
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