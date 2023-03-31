using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame
{
    

    internal class VirtualMouse
    {
        #region singleton stuff
        private static VirtualMouse _virtualMouse = new();
        //THOMAS HOW DO YOU INITIALIZE VALUES?!?!? >:(
        
        public static VirtualMouse VMouse => _virtualMouse;
        #endregion

        float currRotation;
        int maxSpeed = 300;

        public float Rotation
        {
            get { return currRotation; }
        }

        public void Update(MouseState mouse)
        {
            Vector2 newVector = new Vector2(mouse.X, mouse.Y);
            newVector = Vector2.Clamp(newVector, Vector2.Zero, Vector2.UnitX * maxSpeed);
            
            float newAngle = MathF.Atan2(newVector.Y, newVector.X);
            float angleDifference = currRotation - newAngle;

            float speedFactor = newVector.LengthSquared() / (MathF.PI - angleDifference + 0.01f);
            //If newAngle - currRotation is positive, rotate in positive, else rotate in negative
            //currRotation += (Constant * speedFactor * directionFactor).clamp to difference;
        }

        //Find radian angle difference between current angle and vector mouse movement
        //The further the angle the slower it moves
    }
}
