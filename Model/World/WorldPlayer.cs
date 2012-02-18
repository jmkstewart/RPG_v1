using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Model.World {
    public class WorldPlayer {
        public Vector2 Postion { get; set; }
        public Point Size = new Point(71, 71);
        public Rectangle CollisionRect { get { return new Rectangle((int)Postion.X, (int)Postion.Y, Size.X, Size.Y); } }
        public Vector2 WalkingSpeed = new Vector2(4, 4);
        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get { return WalkingSpeed * Direction; } }

        private bool _startedWalking = false;
        public bool StartedWalking {
            get {
                if(_startedWalking) {
                    _startedWalking = false;
                    return true;
                }
                return false;
            }
        }
        private bool _stoppedWalking = false;
        public bool StoppedWalking {
            get {
                if(_stoppedWalking) {
                    _stoppedWalking = false;
                    return true;
                }
                return false;
            }
        }

        private bool _walking = false;
        public bool Walking {
            get { return _walking; }
            set {
                if(!_walking && value) {
                    _startedWalking = true;
                } else if(_walking && !value) {
                    _stoppedWalking = true;
                }
                _walking = value;
            }
        }
        
    }
}
