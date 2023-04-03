using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawnGame;

namespace PawnGame
{
    

    internal class VirtualMouse
    {
        #region singleton stuff
        private static VirtualMouse _virtualMouse = new();
        //THOMAS HOW DO YOU INITIALIZE VALUES?!?!? >:(
        
        public static VirtualMouse VMouse => _virtualMouse;
        #endregion

        float currRotation = 0f;
        float maxSpeed = 1f;
        float windowWidth;
        float windowHeight;


        public float Rotation
        {
            get { return currRotation; }
        }
        

        public void Update(MouseState mouse)
        {
            Vector2 newVector = new Vector2(mouse.X-400, mouse.Y-240);
            MathF.Atan2(newVector.Y, newVector.X);
           

        }

        //Find radian angle difference between current angle and vector mouse movement
        //The further the angle the slower it moves
    }
}
