﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.Battle {
    public class BattleBackgroundView : IDrawableObject {
        private Texture2D _texture;

        public BattleBackgroundView(Game game) {
            _texture = game.Content.Load<Texture2D>("Background/Battle Background Cliff");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(_texture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
        }
    }
}
