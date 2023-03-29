namespace PawnGame.GameObjects
{
    public abstract class GameObject
    {
        #region Fields
        protected Texture2D _texture;
        protected Vectangle _hitbox;
        #endregion

        #region Properties
        public float Width { get { return _hitbox.Width; } }
        public float Height { get { return _hitbox.Height; } }
        public float X { get { return _hitbox.X; } }
        public float Y { get { return _hitbox.Y; } }
        #endregion

        /// <summary>
        /// Gives values to the texture and hitbox
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected GameObject(Texture2D texture, Vectangle hitbox)
        {
            _texture = texture;
            _hitbox = hitbox;
        }

        /// <summary>
        /// Checks if the game object is coliding with another game object
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if game objects are intersecting, otherwise false</returns>
        public virtual bool CheckCollision(GameObject other) { return _hitbox.Intersects(other._hitbox); }

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