﻿using Newtonsoft.Json;

namespace PawnGame.GameObjects
{
    /// <summary>
    /// represents the space of a single square in the game. Can be solid, which prevents entities and players from passing through it.
    /// </summary>
    public class Tile : GameObject
    {
        /// <summary>
        /// If the tile is colidable or not
        /// </summary>
        [JsonProperty]
        private bool _isSolid;
        /// <summary>
        /// represents whether the Tile is solid and can trigger collision resolutions
        /// </summary>
        [JsonIgnore]
        public bool IsSolid 
        {
            get
            {
                return _isSolid;
            }
            set
            {
                _isSolid = value;
            } 
        }

        /// <summary>
        /// create a new tile, specifying whether it is solid
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        /// <param name="solid"></param>
        public Tile(Game1.AssetNames textureKey, Vectangle hitbox, bool solid) : base(textureKey, hitbox)
        {
            _isSolid = solid;
        }
    }
}
