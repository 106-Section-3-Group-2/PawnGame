using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawnGame.GameObjects;

namespace PawnGame
{
    public class VirtualMouse
    {
        #region singleton stuff
        private static VirtualMouse _virtualMouse = new();
        //THOMAS HOW DO YOU INITIALIZE VALUES?!?!? >:(
        
        public static VirtualMouse VMouse => _virtualMouse;
        #endregion

        float x;
        float y;
        float rotation;
        float lastRotation;
        float speed;
        Texture2D crosshair;

        public float X
        {
            get { return x; }
        }
        public float Y
        {
            get { return y; }
        }

        public float Rotation
        {
            get { return rotation; }
        }
        public float Speed
        {
            get { return speed; }
        }
        public Texture2D SetCrosshair
        {
            set { crosshair = value; }
        }

        public void Update(MouseState mouse, Player player)
        {
            /*
            #region Locking to circle
            Vector2 mouseVector = new Vector2(mouse.X - windowWidth / 2, mouse.Y - windowHeight / 2);
            mouseVector.Normalize();
            mouseVector *= 400;
            Mouse.SetPosition((int)(mouseVector.X + windowWidth / 2), (int)(mouseVector.Y + windowHeight / 2));
            #endregion
            */

            x = mouse.X;
            y = mouse.Y;
            rotation = MathF.Atan2(y - player.Y-player.Height/2, x - player.X-player.Width/2);
            speed = Rotation - lastRotation;

            if (Math.Sign(lastRotation) != Math.Sign(Rotation) && Math.Sign(x - player.X - player.Width / 2)<0)
            {
                speed *= -1;
            }

            lastRotation = rotation;

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(crosshair, new Vectangle(X-15, Y-15,30,30), Color.Red);
        }
    }
}
