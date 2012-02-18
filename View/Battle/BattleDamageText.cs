using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.Battle {
    public class BattleDamageText : IDrawableObject {
        private SpriteFont _font;
        private int _damageText;
        private double _verticalOffset = 0;

        public bool Enabled { get; private set; }

        public BattleDamageText(int damageText) {
            _damageText = damageText;
            Enabled = true;
        }

        public void LoadContent(Game game) {
            _font = game.Content.Load<SpriteFont>("Fonts/MenuFont");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            if(Enabled) {
                var position = offset;
                position.Y += (int)_verticalOffset;
                var colour = ColourReference.Orange;
                colour.A += (byte)(_verticalOffset * 2);
                spriteBatch.DrawString(_font, _damageText.ToString(), position, colour, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime gameTime) {
            _verticalOffset -= gameTime.ElapsedGameTime.Milliseconds * 0.05;

            if(_verticalOffset < -70) {
                Enabled = false;
            }
        }
    }
}
