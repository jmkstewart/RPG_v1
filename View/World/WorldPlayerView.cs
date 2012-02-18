using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.World;

namespace View.World {
    public class WorldPlayerView : WorldSpriteBase, IDrawableObject {
        private WorldPlayer _worldPlayer;

        private int _timeBetweenFrames = 100;
        private int _timeSinceLastFrame = 0;
        private int _currentFrame = 0;

        private List<Rectangle> _walkingFrameRectangles = new List<Rectangle>();
        private List<Rectangle> _standingFrameRectangles = new List<Rectangle>();

        public WorldPlayerView(WorldPlayer worldPlayer) {
            _worldPlayer = worldPlayer;
        }

        public void LoadContent(Game game) {
            var texture = game.Content.Load<Texture2D>(@"Player/sorlosheet");
            base.LoadContent(texture, _worldPlayer.Postion, 1);

            LoadFrames();
        }

        public void LoadFrames() {
            // standing
            _standingFrameRectangles.Add(new Rectangle(49, 15, 71, 71));
            _standingFrameRectangles.Add(new Rectangle(125, 15, 71, 71));
            _standingFrameRectangles.Add(new Rectangle(200, 15, 71, 71));

            // walking
            _walkingFrameRectangles.Add(new Rectangle(28, 104, 69, 71));
            _walkingFrameRectangles.Add(new Rectangle(107, 104, 69, 71));
            _walkingFrameRectangles.Add(new Rectangle(189, 104, 69, 71));
            _walkingFrameRectangles.Add(new Rectangle(267, 104, 69, 71));

            CurrentFrameRect = _standingFrameRectangles[_currentFrame];
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(texture, _worldPlayer.Postion + offset, CurrentFrameRect, Color.White, 0, Vector2.Zero, 1, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime) {
            UpdateFrame(gameTime);
            UpdatePlayerFlipped();
        }

        private void UpdatePlayerFlipped() {
            if(_worldPlayer.Direction.X > 0) {
                Flipped = false;
            } else if(_worldPlayer.Direction.X < 0) {
                Flipped = true;
            }
        }

        private void UpdateFrame(GameTime gameTime) {
            if(_worldPlayer.Walking) {
                UpdateFrame(gameTime, _walkingFrameRectangles, _worldPlayer.StartedWalking);
            } else {
                UpdateFrame(gameTime, _standingFrameRectangles, _worldPlayer.StoppedWalking);
            }
        }

        private void UpdateFrame(GameTime gameTime, List<Rectangle> frameRectangles, bool animationChanged) {
            if(animationChanged) {
                _currentFrame = 0;
                _timeSinceLastFrame = 0;

                CurrentFrameRect = frameRectangles[_currentFrame];
            }

            if(_timeSinceLastFrame > _timeBetweenFrames) {
                _timeSinceLastFrame = 0;

                CurrentFrameRect = frameRectangles[_currentFrame];
                _currentFrame++;

                if(_currentFrame >= frameRectangles.Count) {
                    _currentFrame = 0;
                }
            } else {
                _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
