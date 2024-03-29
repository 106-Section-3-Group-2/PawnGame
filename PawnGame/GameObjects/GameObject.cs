﻿using Newtonsoft.Json;
using System;
using static PawnGame.Game1;

namespace PawnGame.GameObjects
{
    public abstract class GameObject : ICloneable
    {
        #region Fields
        [JsonProperty]
        protected Vectangle _hitbox;
        [JsonProperty]
        protected AssetNames _textureKey;
        #endregion

        #region Properties
        /// <summary>
        /// Texture used for drawing the game object
        /// </summary>
        [JsonIgnore]
        protected Texture2D Texture => Assets[_textureKey];

        /// <summary>
        /// X position of the game object's hitbox
        /// </summary>
        [JsonIgnore]
        public float X 
        { 
            get { return _hitbox.X; }
            set { _hitbox.X = value; }
        }

        /// <summary>
        /// Y position of the game object's hitbox
        /// </summary>
        [JsonIgnore]
        public float Y 
        { 
            get { return _hitbox.Y; }
            set { _hitbox.Y = value; }
        }

        /// <summary>
        /// Width of the game object's hitbox
        /// </summary>
        [JsonIgnore]
        public float Width { get { return _hitbox.Width; } }

        /// <summary>
        /// Height of the game object's hitbox
        /// </summary>
        [JsonIgnore]
        public float Height { get { return _hitbox.Height; } }

        /// <summary>
        /// Hitbox of the game object
        /// </summary>
        [JsonIgnore]
        public virtual Vectangle Hitbox { get { return _hitbox; } set { _hitbox.X = value.X; _hitbox.Y = value.Y; } }
        #endregion

        /// <summary>
        /// Create a GameObject with an image key and a hitbox
        /// </summary>
        /// <param name="imageKey"></param>
        /// <param name="hitbox"></param>
        protected GameObject(AssetNames textureKey, Vectangle hitbox)
        {
            _hitbox = hitbox;
            _textureKey = textureKey;
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

        /// <summary>
        /// Returns a shallow copy of the game object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}