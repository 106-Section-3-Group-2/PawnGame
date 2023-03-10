namespace PawnGame.GameObjects
{
    public abstract class GameObject
    {
        #region Fields
        protected Texture2D Texture;
        protected Rectangle Hitbox;
        #endregion
        #region Properties (might not use)
        public int Width { get { return Hitbox.Width; } }
        public int Height { get { return Hitbox.Height; } }
        public int X { get { return Hitbox.X; } }
        public int Y { get { return Hitbox.Y; } }
        #endregion
        /// <summary>
        /// Gives values to the texture and hitbox
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        protected GameObject(Texture2D texture, Rectangle hitbox)
        {
            Texture = texture;
            Hitbox = hitbox;
        }
        /// <summary>
        /// Checks if the game object is coliding with another game object
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if game objects are intersecting, otherwise false</returns>
        public virtual bool CheckCollision(GameObject other) { return Hitbox.Intersects(other.Hitbox); }
        /// <summary>
        /// Draws the game object to the screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Hitbox, Color.White);
        }
    }
}