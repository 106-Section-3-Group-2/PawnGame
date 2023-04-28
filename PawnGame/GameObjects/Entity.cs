using Newtonsoft.Json;
using PawnGame.GameObjects.Enemies;

namespace PawnGame.GameObjects
{
    public abstract class Entity : GameObject
    {
        protected bool _isAlive;
        protected Vector2 _velocity;

        /// <summary>
        /// If the entity is alive or not
        /// </summary>
        [JsonIgnore]
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
            }
        }

        /// <summary>
        /// Makes a new entity object that is alive and has a velocity of 0
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected Entity(Game1.AssetNames textureKey, Rectangle hitbox) : base(textureKey, hitbox)
        {
            _isAlive = true;
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// Should cause the entity to attack
        /// </summary>
        protected virtual void Attack()
        {

        }

        /// <summary>
        /// Gets the index of the tile that the entity is currently on, then checks all tiles in a 3x3 grid for collisions.
        /// </summary>
        protected void ManageTileCollisions()
        {
            //Get curent entity tile
            Point tilePos = new((int)((X - Game1.CurrentLevel.ActiveRoom.Tiles[0, 0].X) / Game1.CurrentLevel.ActiveRoom.Tiles[0, 0].Width), (int)(Y / Game1.CurrentLevel.ActiveRoom.Tiles[0, 0].Height));

            //Prepare for the check
            tilePos.X--;
            tilePos.Y--;

            //Loop through the rows
            for (int i = 0; i < 3; i++)
            {
                //Loop through each column 
                for (int j = 0; j < 3; j++)
                {
                    //If the row index is smaller than the min, skip this row
                    if (tilePos.Y < 0)
                    {
                        break;
                    }

                    //If the row is larger than the max row, quit the method as no more checks are needed
                    else if (tilePos.Y >= Game1.CurrentLevel.ActiveRoom.Tiles.GetLength(1))
                    {
                        return;
                    }

                    //If the column index is less than the min index then move to the next column
                    else if (tilePos.X < 0)
                    {
                        tilePos.X++;
                        continue;
                    }

                    //If the column index is larger than the max, reset the column and change row
                    else if (tilePos.X >= Game1.CurrentLevel.ActiveRoom.Tiles.GetLength(0))
                    {
                        tilePos.X -= j;
                        break;
                    }

                    Tile tileToCheck = Game1.CurrentLevel.ActiveRoom.Tiles[tilePos.X, tilePos.Y];

                    if (CheckCollision(tileToCheck))
                    {
                        if (tileToCheck.IsSolid)
                        {
                            ResolveCollisions(tileToCheck);
                        }
                        //old exit code
                        //else if (tileToCheck.IsExit && EnemyManager.Manager.Count <= 0)
                        //{
                        //    Game1.LevelIndex++;
                        //}
                    }

                    //If the column index needs to be reset, reset it, else increase it
                    if (j == 2) tilePos.X -= 2;
                    else tilePos.X++;
                }

                //After all columns in the row have been checked (or no more valid tiles exist in the row), move to the next row
                tilePos.Y++;
            }
        }

        /// <summary>
        /// Resolves any collisions with the game object entered.
        /// </summary>
        /// <param name="other">Game object that is coliding with the current one</param>
        protected void ResolveCollisions(Tile other)
        {
            Vectangle overlap = _hitbox.GetOverlap(other.Hitbox);

            if (overlap.Width < overlap.Height && other.X < X)
            {
                X += overlap.Width;
            }
            else if (overlap.Width < overlap.Height && other.X > X)
            {
                X -= overlap.Width;
            }

            if (overlap.Height <= overlap.Width && other.Y < Y)
            {
                Y += overlap.Height;
            }
            else if (overlap.Height <= overlap.Width && other.Y > Y)
            {
                Y -= overlap.Height;
            }
        }

        /// <summary>
        /// Checks to see if the player is out of the bounds entered and returns them to in bounds if yes
        /// </summary>
        /// <param name="levelWidth">Width of the level</param>
        /// <param name="levelHeight">Height of the level</param>
        protected void KeepInBounds()
        {
            if (X < Game1.CurrentLevel.ActiveRoom.Location.X)
            {
                X += Game1.CurrentLevel.ActiveRoom.Location.X - X;
            }
            else if (X + Width > Game1.CurrentLevel.ActiveRoom.Location.X + Game1.CurrentLevel.ActiveRoom.Width)
            {
                X -= (X + Width) - (Game1.CurrentLevel.ActiveRoom.Location.X + Game1.CurrentLevel.ActiveRoom.Width);
            }

            if (Y < Game1.CurrentLevel.ActiveRoom.Location.Y)
            {
                Y += Game1.CurrentLevel.ActiveRoom.Location.Y - Y;
            }
            else if (Y + Height > Game1.CurrentLevel.ActiveRoom.Location.Y + Game1.CurrentLevel.ActiveRoom.Height)
            {
                Y -= (Y + Height) - (Game1.CurrentLevel.ActiveRoom.Location.Y + Game1.CurrentLevel.ActiveRoom.Height);
            }
        }
    }
}
