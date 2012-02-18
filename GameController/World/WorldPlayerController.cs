using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Model.World;

namespace GameController.World {
    public class WorldPlayerController {
        private WorldPlayer _worldPlayer;

        public WorldPlayerController(WorldPlayer worldPlayer) {
            _worldPlayer = worldPlayer;
        }

        public void Update(GameTime gameTime) {
            UpdateDirection();
            UpdatePosition(gameTime);
        }

        private void UpdateDirection() {
            Vector2 inputDirection = Vector2.Zero;

            if(DownKeyPressed()) {
                inputDirection.Y += 1;
            }
            if(UpKeyPressed()) {
                inputDirection.Y -= 1;
            }
            if(LeftKeyPressed()) {
                inputDirection.X -= 1;
            }
            if(RightKeyPressed()) {
                inputDirection.X += 1;
            }

            if(_worldPlayer.Walking) {
                if(inputDirection == Vector2.Zero) {
                    _worldPlayer.Walking = false;
                }
            } else {
                if(inputDirection != Vector2.Zero) {
                    _worldPlayer.Walking = true;
                }
            }
            _worldPlayer.Direction = inputDirection;
        }

        private void UpdatePosition(GameTime gameTime) {
            var time = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            var speedOffset = time * 60;

            _worldPlayer.Postion += _worldPlayer.Velocity * speedOffset;
        }

        private bool LeftKeyPressed() { return Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A); }
        private bool RightKeyPressed() { return Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D); }
        private bool UpKeyPressed() { return Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W); }
        private bool DownKeyPressed() { return Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S); }
    }
}
