using System.IO;

namespace PawnGame
{
    /// <summary>
    /// Represents a text box that is able to be clicked
    /// </summary>
    internal class Button
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
        private bool _mouseOver;
        private bool _clicked;

        /// <summary>
        /// Returns a rectangle that holds the posiiton and size
        /// of the button
        /// </summary>
        public Vectangle ButtonBox { get; set; }

        /// <summary>
        /// Determines if a button is enabled or not
        /// </summary>
        public bool Enabled { get; set; }

        public bool Clicked => _clicked;

        #region Constructors
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
            Enabled = true;
            _mouseOver = false;
            _clicked = false;
        }

        /// <summary>
        /// Creates a button with a texture that changes color when hovered over
        /// with a specified size
        /// </summary>
        /// <param name="image">The initial image of the button</param>
        /// <param name="position">The location to draw the button</param>
        /// <param name="width">The width of the button</param>
        /// <param name="height">The height of the button</param>
        /// <param name="hoverColor">The color the button changes to when hovered over</param>
        public Button(Texture2D image, Vector2 position, int width, int height, Color hoverColor)
        {
            _btnImage = image;
            _hoverColor = hoverColor;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                width, height);

            _text = null!;
            _font = null!;
            _btnHoverImage = null;
            btnType = ButtonType.ColorImage;
            Enabled = true;
            _mouseOver = false;
            _clicked = false;
        }

        /// <summary>
        /// Creates a button with a texture that changes texture when hovered over
        /// with a specified size
        /// </summary>
        /// <param name="image">The initial texture of the butotn</param>
        /// <param name="position">The location to draw the button</param>
        /// <param name="width">The width of the button</param>
        /// <param name="height">The height of the button</param>
        /// <param name="hoverImage">The texture the button changes to when hovered over</param>
        public Button(Texture2D image, Vector2 position, int width, int height, Texture2D hoverImage)
        {
            _btnImage = image;
            _btnHoverImage = hoverImage;
            ButtonBox = new Rectangle((int)position.X, (int)position.Y,
                width, height);

            _text = null!;
            _font = null!;
            _hoverColor = Color.White;
            btnType = ButtonType.HoverImage;
            Enabled = true;
            _mouseOver = false;
            _clicked = false;
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
            Enabled = true;
            _mouseOver = false;
            _clicked = false;
        }

        /// <summary>
        /// Creates a button with a texture that changes texture when hovered over
        /// </summary>
        /// <param name="image">The initial image of the button</param>
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
            Enabled = true;
            _mouseOver = false;
            _clicked = false;
        }
        #endregion

        /// <summary>
        /// Updates if the mouse is being hovered over
        /// </summary>
        /// <param name="scale"></param>
        public void Update(float scale)
        {
            if (MouseOver(scale))
            {
                _mouseOver = true;
                _clicked = IsClickedOn();
                return;
            }
            _mouseOver = false;
            _clicked = false;
        }

        /// <summary>
        /// Determines if the mouse is hovered over the button
        /// </summary>
        /// <returns>Whether or the mouse is over the button</returns>
        public bool MouseOver(float scale)
        {
            MouseState mState = Mouse.GetState();

            if (scale < 1)
            {
                return (ButtonBox * scale).Contains(mState.X, mState.Y);
            }
            else
            {
                return (ButtonBox / scale).Contains(mState.X, mState.Y);
            }             

            
        }

        /// <summary>
        /// Should only call if mouse over == true, then when called if mouse is left clicking and the button is active, returns true
        /// </summary>
        /// <returns>Whether the button has been clicked</returns>
        private bool IsClickedOn()
        {
            return Enabled && (Mouse.GetState().LeftButton == ButtonState.Pressed);            
        }

        /// <summary>
        /// Draws the button with an appropriate color or image depending on if
        /// the it is being hovered over or not
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            // Depending on the button type, draws the text or image with the specified color
            // to the screen
            if (Enabled)
            {
                switch (btnType)
                {
                    // Text buttons are strings
                    case ButtonType.Text:
                        if (_mouseOver)
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
                        if (_mouseOver)
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
                        if (_mouseOver)
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
            else
            {
                switch(btnType)
                {
                    case ButtonType.Text:
                        sb.DrawString(_font, _text, new Vector2(ButtonBox.X, ButtonBox.Y), Color.DarkGray);

                        break;

                    case ButtonType.ColorImage:
                        sb.Draw(_btnImage, ButtonBox, Color.DarkGray);

                        break;

                    case ButtonType.HoverImage:
                        sb.Draw(_btnImage, ButtonBox, Color.DarkGray);

                        break;
                }
            }
            

        }
    }
}