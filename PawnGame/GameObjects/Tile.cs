using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects
{
    /// <summary>
    /// represents the space of a single square in the game. Can be solid, which prevents entities and players from passing through it.
    /// </summary>
    public class Tile : GameObject
    {
        /// <summary>
        /// represents whether the Tile is solid and can trigger collision resolutions
        /// </summary>
        public bool Solid { get; set; }

        /// <summary>
        /// create a new tile, specifying whether it is solid
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="hitbox"></param>
        /// <param name="solid"></param>
        public Tile(Texture2D texture, Rectangle hitbox, bool solid) : base(texture, hitbox)
        {
            Solid = solid;
        }
    }
}
