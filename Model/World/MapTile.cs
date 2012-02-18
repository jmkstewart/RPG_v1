using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model;

namespace Model.World {
    public class MapTile : GameObject {
        public static Point TileSize = new Point(128, 128);
        
        public float Rotation { get; set; }
        public Vector2 Position { get { return _position; } }

        public MapTile(Game game, string textureName, Vector2 position, float rotation)
            : base(game, textureName, position, TileSize) {
                this.Rotation = rotation;
        }
    }
}
