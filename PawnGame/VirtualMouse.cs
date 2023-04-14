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
        

        public void Update(MouseState mouse, int windowWidth, int windowHeight)
        {
            
            Vector2 mouseVector = new Vector2(mouse.X-windowWidth/2, mouse.Y-windowHeight/2);
            mouseVector.Normalize();
            mouseVector *= 200;
            Mouse.SetPosition((int)(mouseVector.X+windowWidth/2),(int)(mouseVector.Y+windowHeight/2));
            x = mouse.X;
            y = mouse.Y;
        }

    }
}
