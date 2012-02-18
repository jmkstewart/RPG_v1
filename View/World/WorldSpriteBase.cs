using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace View.World {
    public abstract class WorldSpriteBase {
        private int collisionOffset;
        protected Texture2D texture;

        public Vector2 Position { get; protected set; }
        public Rectangle CurrentFrameRect { get; set; }
        public bool Flipped { get; set; }

        public void LoadContent(Texture2D texture, Vector2 position, int collisionOffset) {
            this.texture = texture;
            this.Position = position;
            this.collisionOffset = collisionOffset;
        }

        public virtual void Update(GameTime gameTime) {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(texture, Position + offset, CurrentFrameRect, Color.White, 0, Vector2.Zero, 1, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public Rectangle collisionRectangle {
            get {
                return new Rectangle(
                    (int)Position.X + collisionOffset,
                    (int)Position.Y + collisionOffset,
                    CurrentFrameRect.Width - (2 * collisionOffset),
                    CurrentFrameRect.Height - (2 * collisionOffset));
            }
        }
    }
}
