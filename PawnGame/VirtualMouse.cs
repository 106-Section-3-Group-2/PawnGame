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
        float windowWidth;
        float windowHeight;
        float rotation;

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
        

        public void Update(MouseState mouse)
        {

            x = mouse.X-windowWidth/2;
            y = mouse.Y-windowHeight/2;

            rotation = MathF.Atan2(y, x);

            //Setting the vector size
            Vector2 mouseVector = new Vector2(x, y);
            mouseVector.Normalize();
            mouseVector *= 200;
            x = mouseVector.X+windowWidth/2;
            y = mouseVector.Y+windowHeight/2;
        }

    }
}
