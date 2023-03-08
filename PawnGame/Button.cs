using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame
{
    /// <summary>
    /// Represents a text box that is able to be clicked
    /// (In the future we pwant to be able to also use textures)
    /// </summary>
    internal struct Button
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
        /// Creates a new text button that changes color when hovered over
        /// </summary>
        /// <param name="font">What font the text is in</param>
        /// <param name="text">What the button will say</param>
        /// <param name="position">The x and y position of the button</param>
        /// <param name="hoverColor">The color that displays when the button is hovered over</param>
        public Button(SpriteFont font, string text, Vector2 position, Color hoverColor)
        {
            _text = text;
            _font = font;
            _hoverColor = hoverColor;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);

        }

        /// <summary>
        /// Determines if the mouse is hovered over the button
        /// </summary>
        /// <returns>Whether or the mouse is over the button</returns>
        public bool MouseOver()
        {
            MouseState mState = Mouse.GetState();

            return ButtonBox.Contains(mState.X, mState.Y);
        }

        /// <summary>
        /// Determines if the mouse is hovering over a button, and is clicked
        /// </summary>
        /// <returns>Whether the button has been clicked</returns>
        public bool Clicked()
        {
            return MouseOver() && (Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Draws the button with an appropriate color depending on if
        /// the it is being hovered over or not
        /// </summary>
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
