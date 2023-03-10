using PawnGame.GameObjects.Enemies;

namespace PawnGame.GameObjects
{
    internal class Player : Entity
    {
        private enum PlayerState
        {
            Moving,
            Attacking,
            Dashing,
            NoControl,
        }
        public enum Ability
        {
            Pawn,
            Knight,
            Bishop,
            Rook,
            Queen,
            None,
        }

        private Ability _activeAbility;
        private Weapon _currentWeapon;
        private int _speed;
        private PlayerState _playerState;


        public Player(Texture2D texture, Rectangle hitbox) : base(texture, hitbox)
        {
            _speed = 3;
        }

        public void Update(KeyboardState currentState, KeyboardState previousState)
        {
            ReadInputs(currentState, previousState);
            Move();
            
        }

        protected override void Attack()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            Hitbox.Location += Velocity.ToPoint();

        }        

        private void ReadInputs(KeyboardState currentState, KeyboardState previousState)
        {
            //dirY and dirX are given to UseAbility() for directional abilities
            int dirY = 0;
            int dirX = 0;
            switch (_playerState)
            {
                case PlayerState.Moving: //If the player is able to move, but is not taking other special actions
                                         //Check WASD, Space, and mouse


                    //Up
                    if (currentState.IsKeyDown(Keys.W))
                    {
                        dirY -= 1;
                        Velocity.Y -= _speed;
                    }
                    //Down
                    if (currentState.IsKeyDown(Keys.S))
                    {
                        dirY += 1;
                        Velocity.Y += _speed;
                    }
                    //Right
                    if (currentState.IsKeyDown(Keys.D))
                    {
                        dirX += 1;
                        Velocity.X += _speed;
                    }
                    //Left
                    if (currentState.IsKeyDown(Keys.A))
                    {
                        dirX -= 1;
                        Velocity.X -= _speed;
                    }
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(dirX,dirY);
                    }
                    //Mouse
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Attack();
                    }
                    break;

                case PlayerState.Attacking: //If an attack is in process, check wasd and space
                    //Up
                    if (currentState.IsKeyDown(Keys.W))
                    {
                        dirY -= 1;
                        Velocity.Y -= _speed;
                    }
                    //Down
                    if (currentState.IsKeyDown(Keys.S))
                    {
                        dirY += 1;
                        Velocity.Y += _speed;
                    }
                    //Right
                    if (currentState.IsKeyDown(Keys.D))
                    {
                        dirX += 1;
                        Velocity.X += _speed;
                    }
                    //Left
                    if (currentState.IsKeyDown(Keys.A))
                    {
                        dirX -= 1;
                        Velocity.X -= _speed;
                    }
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(dirX, dirY);
                    }


                    break;

                case PlayerState.Dashing: //If the player is using an ability that makes them dash
                                          //WASD do not change velocity, but still change direction for abilities
                                          //Check Space
                    //Up
                    if (currentState.IsKeyDown(Keys.W))
                    {
                        dirY -= 1;
                    }
                    //Down
                    if (currentState.IsKeyDown(Keys.S))
                    {
                        dirY += 1;
                    }
                    //Right
                    if (currentState.IsKeyDown(Keys.D))
                    {
                        dirX += 1;
                    }
                    //Left
                    if (currentState.IsKeyDown(Keys.A))
                    {
                        dirX -= 1;
                    }
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility(dirX, dirY);
                    }
                    break;

                case PlayerState.NoControl: //If the player is not able to move (cutscenes)

                    break;
                

                

                default:
                    break;
            }

            

            

        }

        private void UseAbility(int dirX, int dirY)
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

            }
            
        }

        private void GetAbility(Ability ability)
        {
            if (_activeAbility == Ability.None)
            {
                _activeAbility = ability;
            }
        }

        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }
    }
}
