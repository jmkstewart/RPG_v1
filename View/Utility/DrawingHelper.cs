using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.Utility {
    public static class DrawingHelper {
        public static void DrawBlackOutline(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle) {
            DrawBlackOutline(spriteBatch, texture, rectangle, 2);
        }

        public static void DrawBlackOutline(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, int outlineWidth) {
            var inkRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            inkRect.Inflate(2, 2);
            spriteBatch.Draw(texture, inkRect, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
