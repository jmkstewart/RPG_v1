using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.World;

namespace View.World {
    public class MapTileView : IDrawableObject {
        private MapTile _mapTile;
        private Texture2D _texture;

        public MapTileView(Game game, MapTile mapTile) : this(game, mapTile, game.Content.Load<Texture2D>(mapTile.TextureName)) { }

        public MapTileView(Game game, MapTile mapTile, Texture2D texture) {
            _mapTile = mapTile;
            _texture = texture;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(_texture, GetPosition(offset), GetCurrentSourceRectangle(), Color.White, _mapTile.Rotation, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
        
        private Vector2 GetPosition(Vector2 offset) {
            var position = _mapTile.Position + offset;

            if(_mapTile.Rotation == MathHelper.Pi / 2) {
                position.X += _mapTile.Size.X;
            } else if(_mapTile.Rotation == MathHelper.Pi) {
                position.X += _mapTile.Size.X;
                position.Y += _mapTile.Size.Y;
            } else if(_mapTile.Rotation == (MathHelper.Pi / 2) + MathHelper.Pi) {
                position.Y += _mapTile.Size.Y;
            }
            
            return position;
        }

        private Rectangle GetCurrentSourceRectangle() {
            return new Rectangle(0, 0, _mapTile.Size.X, _mapTile.Size.Y);
        }

    }
}
