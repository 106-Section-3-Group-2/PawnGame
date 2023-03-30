using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame.GameObjects
{
    internal class Weapon
    {
        Texture2D swordTexture;


        //TODO: Add functionality for other weapons (probably a currentWeapon 
        public Weapon(Texture2D texture)
        {
            this.swordTexture = texture;
        }

        public void Draw(SpriteBatch sb, Player player, MouseState mouse)
        {
            sb.Draw(swordTexture,player.Hitbox,null,Color.White,MathF.Atan(mouse.Y-player.Hitbox.Y/mouse.X-player.Hitbox.X),player.Hitbox.Location,SpriteEffects.None,0);
        }
    }
}
