using Newtonsoft.Json;
using System;
using System.CodeDom;

namespace PawnGame
{
    /// <summary>
    /// Rectangle that uses float values instead of int values
    /// </summary>
    public struct Vectangle
    {
        #region Fields
        [JsonProperty]
        private float _x;
        [JsonProperty]
        private float _y;
        [JsonProperty]
        private float _width;
        [JsonProperty]
        private float _height;
        #endregion

        #region Properties
        /// <summary>
        /// X location of the rectangle.
        /// </summary>
        [JsonIgnore]
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        /// <summary>
        /// Y location of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        /// <summary>
        /// Width of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        /// <summary>
        /// Height of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        /// <summary>
        /// Position of the left wall of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Left => _x;
        /// <summary>
        /// Position of the right wall of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Right => _x + _width;
        /// <summary>
        /// Position of the top wall of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Top => _y;
        /// <summary>
        /// Bottom wall of the rectangle
        /// </summary>
        [JsonIgnore]
        public float Bottom => _y + _height;
        /// <summary>
        /// Center of the rectangle
        /// </summary>
        [JsonIgnore]
        public Vector2 Center => new(_x + _width / 2, _y + _height / 2);
        /// <summary>
        /// Location of the vectangle as a vector
        /// </summary>
        [JsonIgnore]
        public Vector2 Location
        {
            get
            {
                return new Vector2(_x, _y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new rectangle with float values for position and dimentions.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Vectangle(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// If the rectangle intersects the entered rectangle
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Intersects(Vectangle value)
        {
            if (value.Left < Right && Left < value.Right && value.Top < Bottom)
            {
                return Top < value.Bottom;
            }

            return false;
        }

        /// <summary>
        /// If the rectangle contains the other rectangle.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Vectangle value)
        {
            if (_x <= value.X && value.X + value.Width <= _x + _width && _y <= value.Y)
            {
                return value.Y + value.Height <= _y + _height;
            }

            return false;
        }

        /// <summary>
        /// Checks if the int positions are in the area of the rectangle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            if (_x <= x && x < _x + _width && _y <= y)
            {
                return y < _y + _height;
            }

            return false;
        }

        /// <summary>
        /// Checks if the float positions are in the area of the rectangle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(float x, float y)
        {
            if (_x <= x && x < (_x + _width) && _y <= y)
            {
                return y < (_y + _height);
            }

            return false;
        }

        /// <summary>
        /// Checks if the vector is in the rectangle's area.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Vector2 value)
        {
            if (_x <= value.X && value.X < (_x + Width) && _y <= value.Y)
            {
                return value.Y < (_y + Height);
            }

            return false;
        }

        /// <summary>
        /// Takes another vectangle and returns the vectangle made by their overlap
        /// </summary>
        /// <param name="other">Vectangle to check</param>
        /// <returns>Overlaping vectangle</returns>
        public Vectangle GetOverlap(Vectangle other)
        {
            if (!Intersects(other))
            {
                return new(0, 0, 0, 0);
            }

            float num = Math.Min(_x + _width, other._x + other._width);
            float num2 = Math.Max(_x, other._x);
            float num3 = Math.Max(_y, other._y);
            float num4 = Math.Min(_y + _height, other._y + other._height);

            return new(num2, num3, num - num2, num4 - num3);
        }

        /// <summary>
        /// Multiplies the vectangle values by the float value
        /// </summary>
        /// <param name="v"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vectangle operator *(Vectangle v, float scalar)
        {
            return new(v.X * scalar, v.Y * scalar, v.Width * scalar, v.Height * scalar);
        }

        /// <summary>
        /// Devides the vectangle values by the float value
        /// </summary>
        /// <param name="v"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vectangle operator /(Vectangle v, float scalar)
        {
            return new(v.X / scalar, v.Y / scalar, v.Width / scalar, v.Height / scalar);
        }

        /// <summary>
        /// Rounds vectangle values to ints and returns a rectangle with these values.
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator Rectangle(Vectangle v)
        {
            return new((int)Math.Round(v.X), (int)Math.Round(v.Y), (int)Math.Round(v.Width), (int)Math.Round(v.Height));
        }
        /// <summary>
        /// Converts a rectangle into a vectangle by making ints into floats.
        /// </summary>
        /// <param name="rect"></param>
        public static implicit operator Vectangle(Rectangle rect)
        {
            return new(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
