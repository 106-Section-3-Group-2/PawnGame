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
        private Ability _activeAbility;
        private Weapon _currentWeapon;
        private int _speed;
        private PlayerState playerState;


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

            switch (playerState)
            {
                case PlayerState.Moving: //If the player is able to move, but is not taking other special actions
                                         //Check WASD, Space, and mouse
                    //Up
                    if (currentState.IsKeyDown(Keys.W))
                    {
                        Velocity.Y -= _speed;
                    }
                    //Down
                    if (currentState.IsKeyDown(Keys.S))
                    {
                        Velocity.Y += _speed;
                    }
                    //Right
                    if (currentState.IsKeyDown(Keys.D))
                    {
                        Velocity.X += _speed;
                    }
                    //Left
                    if (currentState.IsKeyDown(Keys.A))
                    {
                        Velocity.X -= _speed;
                    }
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility();
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
                        Velocity.Y -= _speed;
                    }
                    //Down
                    if (currentState.IsKeyDown(Keys.S))
                    {
                        Velocity.Y += _speed;
                    }
                    //Right
                    if (currentState.IsKeyDown(Keys.D))
                    {
                        Velocity.X += _speed;
                    }
                    //Left
                    if (currentState.IsKeyDown(Keys.A))
                    {
                        Velocity.X -= _speed;
                    }
                    //Space
                    if (currentState.IsKeyDown(Keys.Space))
                    {
                        UseAbility();
                    }


                    break;

                case PlayerState.Dashing: //If the player is using an ability that makes them dash
                                          //Check Space
                    break;

                case PlayerState.NoControl: //If the player is not able to move (cutscenes)

                    break;
                

                

                default:
            }

            

            

        }

        private void UseAbility()
        {
            
            throw new System.NotImplementedException();
        }

        private void GetAbility(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDeath()
        {
            throw new System.NotImplementedException();
        }
    }
}
