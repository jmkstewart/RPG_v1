using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.Battle;
using View.Utility;

namespace View.Battle {
    public class BattleTrayCharacterView : IDrawableObject {
        private Game _game;
        private BattleObject _battleObject;

        private Texture2D _portraitTexture;
        private Texture2D _singleColourTexture;

        private SpriteFont _font;
        private DisplayBarAndText _activeTimeDisplay;
        private DisplayBarAndText _healthDisplay;
        private DisplayBarAndText _magicDisplay;

        public BattleTrayCharacterView(Game game, BattleObject player) {
            _game = game;
            _battleObject = player;

            LoadTextures();
            LoadDisplayBars();
        }

        private void LoadTextures() {
            _portraitTexture = _game.Content.Load<Texture2D>(@"Player/Duder");
            _singleColourTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _singleColourTexture.SetData(new Color[] { Color.White });
            _font = _game.Content.Load<SpriteFont>("Fonts/TrayNameFont");
        }

        private void LoadDisplayBars() {
            _activeTimeDisplay = new DisplayBarAndText(_game, ColourReference.Blue, () => _battleObject.CurrentTimeToAction, () => BattleObject.TimeToAction);
            _activeTimeDisplay.DisplayText = false;
            _healthDisplay = new DisplayBarAndText(_game, Color.Red, () => _battleObject.HP, () => _battleObject.HPMax);
            _magicDisplay = new DisplayBarAndText(_game, Color.Blue, () => _battleObject.MP, () => _battleObject.MPMax);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            var rect = new Rectangle((int)offset.X, (int)offset.Y, 373, 170);
            DrawingHelper.DrawBlackOutline(spriteBatch, _singleColourTexture, rect);
            spriteBatch.Draw(_singleColourTexture, rect, ColourReference.LightBlue);

            spriteBatch.Draw(_portraitTexture, new Rectangle((int)offset.X + 10, (int)offset.Y + 10, 140, 150), Color.White);

            offset.X += 160;
            offset.Y += 10;
            var name = _battleObject.Name + (_battleObject.Status.Length > 0 ? " - " + _battleObject.Status : "");
            spriteBatch.DrawString(_font, name, offset, Color.White);

            offset.Y += 40;
            _activeTimeDisplay.Draw(gameTime, spriteBatch, offset);
            offset.Y += 40;
            _healthDisplay.Draw(gameTime, spriteBatch, offset);
            offset.Y += 40;
            _magicDisplay.Draw(gameTime, spriteBatch, offset);
        }
    }
}
