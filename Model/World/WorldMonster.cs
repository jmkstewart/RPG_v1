using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Model.World {
    public class WorldMonster {
        public int Id;
        public Vector2 Postion;
        public Point Size { get; set; }
        public Rectangle CollisionRect { get { return new Rectangle((int)Postion.X, (int)Postion.Y, Size.X, Size.Y); } }
        public Vector2 WalkingSpeed = new Vector2(4, 4);
        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get { return WalkingSpeed * Direction; } }
    }
}
