using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.Battle;
using View.Utility;

namespace View.Battle {
    public class BattleTrayView : IDrawableObject {
        private Game _game;
        private List<BattleTrayCharacterView> _characterViews;

        private Texture2D _battleMenuViewTexture;

        public BattleTrayView(Game game, List<BattleObject> playerParty) {
            _game = game;

            _characterViews = new List<BattleTrayCharacterView>();
            foreach(var player in playerParty) {
                _characterViews.Add(new BattleTrayCharacterView(_game, player));
            }

            LoadTextures();
        }

        private void LoadTextures() {
            _battleMenuViewTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _battleMenuViewTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            var outlineRect = GetMenuOutlineTextureRect(offset);
            DrawingHelper.DrawBlackOutline(spriteBatch, _battleMenuViewTexture, outlineRect);
            spriteBatch.Draw(_battleMenuViewTexture, outlineRect, null, ColourReference.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);

            var characterOffset = offset + new Vector2(10, 10);
            foreach(var character in _characterViews) {
                character.Draw(gameTime, spriteBatch, characterOffset);
                characterOffset.X += 383;
            }
            /*
            // TODO:  remove these when we have other characters
            foreach(var character in _characterViews) {
                character.Draw(gameTime, spriteBatch, characterOffset);
                characterOffset.X += 383;
            }
            foreach(var character in _characterViews) {
                character.Draw(gameTime, spriteBatch, characterOffset);
                characterOffset.X += 383;
            }*/
        }

        private Rectangle GetMenuOutlineTextureRect(Vector2 offset) {
            return new Rectangle((int)offset.X, (int)offset.Y, 1160, 190);
        }
    }
}
