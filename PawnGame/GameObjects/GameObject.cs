namespace PawnGame.GameObjects
{
    public abstract class GameObject
    {
        #region Fields
        protected Texture2D _texture;
        protected Rectangle _hitbox;
        #endregion

        #region Properties (might not use)
        public int Width { get { return _hitbox.Width; } }

        public int Height { get { return _hitbox.Height; } }

        public int X { get { return _hitbox.X; } }

        public int Y { get { return _hitbox.Y; } }
        #endregion

        /// <summary>
        /// Gives values to the texture and hitbox
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected GameObject(Texture2D texture, Rectangle hitbox)
        {
            _texture = texture;
            _hitbox = hitbox;
        }

        /// <summary>
        /// Checks if the game object is coliding with another game object
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if game objects are intersecting, otherwise false</returns>
<<<<<<< HEAD
        public virtual bool CheckCollision(GameObject other) { return _hitbox.Intersects(other._hitbox); }
=======
        protected virtual bool CheckCollision(GameObject other) { return Hitbox.Intersects(other.Hitbox); }
>>>>>>> parent of 7b2df1c (Merge branch 'main' of https://github.com/106-Section-3-Group-2/PawnGame)

        /// <summary>
        /// Draws the game object to the screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, _hitbox, Color.White);
        }
    }
}
