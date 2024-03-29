﻿using PawnGame.GameObjects.Enemies;
using System;

namespace PawnGame.GameObjects
{
    public class Player : Entity
    {
        public enum PlayerState
        {
            Moving,
            Abilitying,
            NoControl,
            Dizzy,
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
        private Vector2 _abilityMove;
        private Weapon _currentWeapon;
        private int _speed;
        private PlayerState _playerState;
        private int _abilityTimer;
        private bool _isInvincible;
        private bool _weaponOverride;
        private int _dizzyCounter;

        public Weapon Weapon
        {
            get { return _currentWeapon; }
        }

        public bool IsInvincible
        {
            get { return _isInvincible; }
        }

        public bool WeaponOverride
        {
            get { return _weaponOverride; }
        }

        public int Dizzy
        {
            get { return _dizzyCounter; }
            set { _dizzyCounter = value; }
        }

        public PlayerState State
        {
            get { return _playerState; }
        }
        public Ability HeldAbility { get { return _heldAbility; } set { _heldAbility = value; } }


        public Player(Game1.AssetNames textureKey, Rectangle hitbox, Weapon weapon) : base(textureKey, hitbox)
        {
            _abilityTimer = 0;
            _speed = 8;
            _heldAbility = Ability.None;
            _activeAbility = Ability.None;
            _currentWeapon = weapon;
            _isInvincible = false;
            _weaponOverride = false;
            _dizzyCounter = 0;
        }

        public void Update(KeyboardState currentKBState, KeyboardState previousKBState, MouseState currentMouseState, MouseState prevMouseState)
        {
            if (_isAlive == false)
            {
                OnDeath();
            }

            ReadInputs(currentKBState, previousKBState,currentMouseState,prevMouseState);
            Move();
            KeepInBounds();

            ManageTileCollisions();

            Weapon.Update(this,VirtualMouse.VMouse);            
        }

        /// <summary>
        /// Moves the player based off its velocity and uses its ability
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        protected void Move()
        {
            switch (_playerState)
            {
                case PlayerState.Dizzy:
                    if (!_isAlive)
                    {
                        _playerState = PlayerState.NoControl;
                    }
                    _hitbox.X += _velocity.X/3;
                    _hitbox.Y += _velocity.Y/3;
                    if (_dizzyCounter > 0)
                    {
                        _dizzyCounter--;
                    }
                    else
                    {
                        _playerState = PlayerState.Moving;
                    }
                    break;

                case PlayerState.Moving:
                    if (!_isAlive)
                    {
                        _playerState = PlayerState.NoControl;
                    }
                    if (Dizzy > 0)
                    {
                        _playerState = PlayerState.Dizzy;
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
                                _isInvincible = true;
                                _weaponOverride = true;
                                _hitbox.X += _abilityMove.X*15;
                                _hitbox.Y += _abilityMove.Y*15;
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
                        _isInvincible = false;
                        _weaponOverride = false;
                        _playerState = PlayerState.Moving;
                    }

                    break;

                case PlayerState.NoControl:
                    if (_isAlive)
                    {
                        _playerState = PlayerState.Moving;
                    }
                    break;


            }

            
        }

        /// <summary>
        /// Takes Keyboard inputs and modifies player information
        /// </summary>
        /// <param name="currentKBState"></param>
        /// <param name="previousKBState"></param>
        /// <param name="currentMouseState"></param>
        /// <param name="prevMouseState"></param>
        private void ReadInputs(KeyboardState currentKBState, KeyboardState previousKBState,MouseState currentMouseState, MouseState prevMouseState)
        {
            //Returns a normalized 2D vector of player's input direction
            Vector2 direction = GetDirection(currentKBState);

            switch (_playerState)
            {
                case PlayerState.Moving: //If the player is able to move, but is not taking other special actions
                                         //Check WASD, Space
                    _velocity = _speed * direction;
                    //Space
                    if (currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
                    {
                        UseAbility(direction);
                    }

                    break;

                case PlayerState.Dizzy:

                    _velocity = _speed * direction;
                    if (currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
                    {
                        UseAbility(direction);
                        _dizzyCounter = 0;
                    }
                    break;

                case PlayerState.Abilitying: //If the player is using an ability that makes them dash
                                             //WASD do not change velocity, but still change direction for abilities
                                             //Check WASD and Space
                                             //Space
                    if (currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
                    {
                        /*
                        UseAbility(direction);
                        */
                    }
                    break;

                case PlayerState.NoControl: //If the player is not able to move (cutscenes)
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Draws the player to the screen.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (_isAlive)
            {
                sb.Draw(Texture, _hitbox, Color.White);
            }
            
        }

        /// <summary>
        /// Takes keyboard state and returns the input vector
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

        /// <summary>
        /// Executes code based on the player's current ability.
        /// Pawn effect: 
        /// </summary>
        /// <param name="direction"></param>
        private void UseAbility(Vector2 direction)
        {
            if (_heldAbility != Ability.None)
            {
                _playerState = PlayerState.Abilitying;
                switch (_heldAbility)
                {
                    case Ability.Pawn:
                        _abilityMove = direction;
                        _activeAbility = Ability.Pawn;
                        _abilityTimer = 18;
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

        /// <summary>
        /// If the player doesnt currently have an ability, give the player the entered ability.
        /// </summary>
        /// <param name="ability"></param>
        public void GetAbility(Ability ability)
        {
            if (_heldAbility == Ability.None)
            {
                _heldAbility = ability;
            }
        }

        protected void OnDeath()
        {
            Game1.CurrentLevel.Reset();
        }

    }
}