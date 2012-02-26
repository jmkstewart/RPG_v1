using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View {
    public class ScreenTransition {
        private Game _game;
        private Rectangle _screenRectangle;
        private int _transparency = 0;
        private bool _increasing = true;
        private Texture2D _singleColourTexture;

        private readonly int _timeBetweenFrames = 25;
        private int _timeToFrameChange = 25;

        public bool Enabled { get; private set; }
        public bool FadedOut { get; private set; }

        public ScreenTransition(Game game, Rectangle screenRectangle) {
            _game = game;
            _screenRectangle = screenRectangle;
            Enabled = true;
            FadedOut = false;

            _singleColourTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _singleColourTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if(Enabled) {
                var fadingBlack = new Color(0, 0, 0, _transparency);
                spriteBatch.Draw(_singleColourTexture, _screenRectangle, fadingBlack);
            }
        }

        public void Update(GameTime gameTime) {
            _timeToFrameChange -= gameTime.ElapsedGameTime.Milliseconds;
            if(_timeToFrameChange <= 0) {
                _timeToFrameChange += _timeBetweenFrames;

                if(_increasing) {
                    if(_transparency > 255) {
                        _increasing = false;
                        FadedOut = true;
                    } else {
                        _transparency += 10;
                    }
                } else {
                    if(_transparency <= 0) {
                        Enabled = false;
                    } else {
                        _transparency -= 10;
                    }
                }
            }
        }
    }
}
