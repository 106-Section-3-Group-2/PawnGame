using Newtonsoft.Json;

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
        /// If the tile is an exit
        /// </summary>
        [JsonProperty]
        private bool _isDoor;

        /// <summary>
        /// If the tile is an exit
        /// </summary>
        [JsonProperty]
        private bool _isExit;

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
        /// represents whether the tile is an exit and can trigger a level advancement
        /// </summary>
        [JsonIgnore]
        public bool IsExit
        {
            get 
            {
                return _isExit;
            }
            set
            {
                _isExit = value;
            }
        }
        /// <summary>
        /// represents whether the tile is a door and can trigger a room movement
        /// </summary>
        [JsonIgnore]
        public bool IsDoor
        {
            get
            {
                return _isDoor;
            }
            set
            {
                _isDoor = value;
            }
        }

        /// <summary>
        /// create a new tile, specifying whether it is solid and whether it is an exit
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        /// <param name="solid"></param>
        [JsonConstructor]
        public Tile(Game1.AssetNames textureKey, Vectangle hitbox, bool solid, bool exit = false, bool door = false) : base(textureKey, hitbox)
        {
            _isSolid = solid;
            _isExit = exit;
            _isDoor = door;
        }
    }
}
