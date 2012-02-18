using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Model.World {
    public class CameraOffset {
        private Point _centre;
        private Vector2 _offset;

        public Vector2 Offset { get { return _offset; } }

        public CameraOffset(Point centre) {
            _centre = centre;
            _offset = Vector2.Zero;
        }

        public void CentreOnPoint(Vector2 centreOn) {
            _offset.X = -(centreOn.X - _centre.X);
            _offset.Y = -(centreOn.Y - _centre.Y);
        }
    }
}
