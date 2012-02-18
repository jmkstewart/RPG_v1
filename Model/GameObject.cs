using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Model {
    public class GameObject {
        public string TextureName { get; set; }
        public Point Size { get; private set; }

        protected Vector2 _position;

        public GameObject(Game game, string textureName, Vector2 position, Point size) {
            this.TextureName = textureName;
            this._position = position;
            this.Size = size;
        }

        private Rectangle GetCurrentSourceRectangle() {
            return new Rectangle(0, 0, Size.X, Size.Y);
        }
    }
}
