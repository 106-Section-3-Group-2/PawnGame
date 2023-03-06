using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects
{
    public abstract class GameObject
    {
        protected Texture2D Texture;
        protected Rectangle Hitbox;

        #region Properties (might not use)
        public int Width { get { return Hitbox.Width; } }

        public int Height { get { return Hitbox.Height; } }

        public int X { get { return Hitbox.X; } }

        public int Y { get { return Hitbox.Y; } }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        public GameObject(Texture2D texture,
            Rectangle hitbox)
        {
            Texture = texture;
            Hitbox = hitbox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool CheckCollision(GameObject other) { return Hitbox.Intersects(other.Hitbox); }
    }
}
