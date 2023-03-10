using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PawnGame
{
    /// <summary>
    /// Represents a text box that is able to be clicked
    /// </summary>
    internal struct Button
    {
        /// <summary>
        /// The type of button that has been created
        /// Each button type has different behavior
        /// </summary>
        private enum ButtonType
        {
            Text,
            ColorImage,
            HoverImage
        }
        private ButtonType btnType;
        // Used for text buttons
        private string _text;
        private SpriteFont _font;

        // Used for image buttons
        private Texture2D _btnImage;
        private Texture2D _btnHoverImage;
        // The color of the button when it is hovered
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
        /// <param name="position">The location to draw the button</param>
        /// <param name="hoverColor">The color that displays when the button is hovered over</param>
        public Button(SpriteFont font, string text, Vector2 position, Color hoverColor)
        {
            _text = text;
            _font = font;
            _hoverColor = hoverColor;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
            _btnImage = null!;
            _btnHoverImage = null;
            btnType = ButtonType.Text;
        }
        /// <summary>
        /// Creates a button with a texture that changes color when hovered over
        /// </summary>
        /// <param name="image">The initial image of the button</param>
        /// <param name="position">The location to draw the button</param>
        /// <param name="hoverColor">The color the button changes to when hovered over</param>
        public Button(Texture2D image, Vector2 position, Color hoverColor)
        {
            _btnImage = image;
            _hoverColor = hoverColor;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                image.Width, image.Height);
            _text = null!;
            _font = null!;
            _btnHoverImage = null;
            btnType = ButtonType.ColorImage;
        }
        /// <summary>
        /// Creates a button with a texture that changes texture when hovered over
        /// </summary>
        /// <param name="image">The initial texture of the butotn</param>
        /// <param name="position">The location to draw the button</param>
        /// <param name="hoverImage">The texture the button changes to when hovered over</param>
        public Button(Texture2D image, Vector2 position, Texture2D hoverImage)
        {
            _btnImage = image;
            _btnHoverImage = hoverImage;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                image.Width, image.Height);
            _text = null!;
            _font = null!;
            _hoverColor = Color.White;
            btnType = ButtonType.HoverImage;
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
        /// Draws the button with an appropriate color or image depending on if
        /// the it is being hovered over or not
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            bool mouseOver = MouseOver();
            // Depending on the button type, draws the text or image with the specified color
            // to the screen
            switch (btnType)
            {
                // Text buttons are strings
                case ButtonType.Text:
                    if (mouseOver)
                    {
                        sb.DrawString(_font, _text, new Vector2(ButtonBox.X, ButtonBox.Y), _hoverColor);
                    }
                    else
                    {
                        sb.DrawString(_font, _text, new Vector2(ButtonBox.X, ButtonBox.Y), Color.White);
                    }
                    break;
                // Color image buttons change color when hovered
                case ButtonType.ColorImage:
                    if (mouseOver)
                    {
                        sb.Draw(_btnImage, ButtonBox, _hoverColor);
                    }
                    else
                    {
                        sb.Draw(_btnImage, ButtonBox, Color.White);
                    }
                    break;
                // Hover Image buttons change texture when hovered
                case ButtonType.HoverImage:
                    if (mouseOver)
                    {
                        sb.Draw(_btnHoverImage, ButtonBox, Color.White);
                    }
                    else
                    {
                        sb.Draw(_btnImage, ButtonBox, Color.White);
                    }

                    break;
            }

        }
    }
}