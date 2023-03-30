namespace PawnGame.GameObjects
{
    public abstract class GameObject
    {
        #region Fields
        protected Texture2D Texture
        {
            get
            {
                return Game1.Textures[_imageKey];
            }
            set
            {
                _imageKey = Game1.TexturesReverse[value];
            }
        }
        protected Vectangle _hitbox;
        protected string _imageKey;
        #endregion

        #region Properties
        public float Width { get { return _hitbox.Width; } }
        public float Height { get { return _hitbox.Height; } }
        public float X { get { return _hitbox.X; } }
        public float Y { get { return _hitbox.Y; } }
        public virtual Vectangle Hitbox { get { return _hitbox; } set { _hitbox.X = value.X; _hitbox.Y = value.Y; } }
        #endregion

        /// <summary>
        /// Create a GameObject with a Texture2D and a hitbox
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected GameObject(Texture2D texture, Vectangle hitbox)
        {
            Texture = texture;
            _hitbox = hitbox;
            _imageKey = Game1.TexturesReverse[texture];
        }

        /// <summary>
        /// Create a GameObject with an image key and a hitbox
        /// </summary>
        /// <param name="imageKey"></param>
        /// <param name="hitbox"></param>
        protected GameObject(string imageKey, Vectangle hitbox)
        {
            _hitbox = hitbox;
            _imageKey = imageKey;
        }

        /// <summary>
        /// Checks if the game object is coliding with another game object
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if game objects are intersecting, otherwise false</returns>
        public virtual bool CheckCollision(GameObject other) { return _hitbox.Intersects(other._hitbox); }
        public virtual bool CheckCollision(Weapon weapon) { return weapon.IsColliding(Hitbox); }
        /// <summary>
        /// Draws the game object to the screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, _hitbox, Color.White);
        }
    }
}