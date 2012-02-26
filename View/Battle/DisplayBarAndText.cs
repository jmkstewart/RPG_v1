using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.Battle {
    public class DisplayBarAndText : IDrawableObject {
        private Game _game;

        private SpriteFont _font;
        private Texture2D _singleColourTexture;
        private Color _colour;
        private Func<int> _getCurrentValue;
        private Func<int> _getMaxValue;

        public bool DisplayText { get; set; }

        public DisplayBarAndText(Game game, Color colour, Func<int> getCurrentValue, Func<int> getMaxValue) {
            _game = game;
            _colour = colour;
            _getCurrentValue = getCurrentValue;
            _getMaxValue = getMaxValue;

            LoadTextures();

            DisplayText = true;
        }

        private void LoadTextures() {
            _singleColourTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _singleColourTexture.SetData(new Color[] { Color.White });
            _font = _game.Content.Load<SpriteFont>("Fonts/DisplayBarFont");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            var timeRect = GetBarTextureRect(offset);
            spriteBatch.Draw(_singleColourTexture, timeRect, Color.Silver);
            spriteBatch.Draw(_singleColourTexture, GetInnerTextureRect(offset, timeRect, _getCurrentValue(), _getMaxValue()), _colour);

            if(DisplayText) {
                offset.X += timeRect.Width;
                spriteBatch.DrawString(_font, _getCurrentValue().ToString() + "/" + _getMaxValue().ToString(), offset, Color.White);
            }
        }

        private Rectangle GetBarTextureRect(Vector2 position) {
            return new Rectangle((int)position.X, (int)position.Y, 120, 30);
        }

        private Rectangle GetInnerTextureRect(Vector2 position, Rectangle rect, int current, int max) {
            rect.X += 5;
            rect.Y += 5;
            rect.Width -= 10;
            rect.Height -= 10;

            var widthOverPossibleTime = (float)rect.Width / max;
            rect.Width = (int)(widthOverPossibleTime * current);

            return rect;
        }
    }
}
