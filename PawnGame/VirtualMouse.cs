using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawnGame;

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
        float maxSpeed = 1f;
        float rotation;
        float lastRotation;
        float speed;
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

        public void Update(MouseState mouse, int windowWidth, int windowHeight)
        {


            //Comment out this region
            #region Locking to circle
            Vector2 mouseVector = new Vector2(mouse.X - windowWidth / 2, mouse.Y - windowHeight / 2);
            mouseVector.Normalize();
            mouseVector *= 400;
            //Mouse.SetPosition((int)(mouseVector.X + windowWidth / 2), (int)(mouseVector.Y + windowHeight / 2));
            #endregion

            x = mouse.X;
            y = mouse.Y;
            rotation = MathF.Atan2(y - windowHeight / 2, x - windowWidth / 2);
            speed = Rotation - lastRotation;
            lastRotation = rotation;

        }

    }
}
