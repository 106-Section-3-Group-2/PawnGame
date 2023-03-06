using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame
{
    /// <summary>
    /// 
    /// </summary>
    internal class Button
    {
        private string _text;
        private SpriteFont _font;
        private Color _hoverColor;

        /// <summary>
        /// Returns a rectangle that holds the posiiton and size
        /// of the button
        /// </summary>
        public Rectangle ButtonBox { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Button(SpriteFont font, string text, Vector2 position, Color hoverColor)
        {
            _text = text;
            _font = font;
            _hoverColor = hoverColor;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
        }

        public bool MouseOver()
        {
            MouseState mState = Mouse.GetState();
            return ButtonBox.Contains(mState.X, mState.Y);
        }

        public bool Clicked()
        {
            return MouseOver() && (Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public void Draw(SpriteBatch sb)
        {
            if (MouseOver())
            {
                sb.DrawString(_font, _text, new Vector2(ButtonBox.X, ButtonBox.Y), _hoverColor);
            }
            else
            {
                sb.DrawString(_font, _text, new Vector2(ButtonBox.X, ButtonBox.Y), Color.White);
            }       
        }
    }
}
