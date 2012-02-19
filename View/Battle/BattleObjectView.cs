using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.Battle;

namespace View.Battle {
    public class BattleObjectView : InteractableBattleObjectView {
        private Game _game;
        private Texture2D _texture;
        private Texture2D _timeOutlineTexture;
        private Texture2D _timeInnerTexture;
        private Texture2D _selectTexture;
        private BattleObject _battleObject;
        private BattleDamageText _damageText = null;

        public BattlePosition CurrentBattlePosition { get; set; }
        public string Name { get { return _battleObject.Name; } }
        public bool Selected { get; set; }

        public BattleObjectView(Game game, BattleObject battleObject, BattlePosition battlePosition) {
            _game = game;
            _battleObject = battleObject;
            LoadView(battleObject.Id);
            CurrentBattlePosition = battlePosition;
        }
        
        private void LoadView(int id) {
            // load from a file based on the id
            _texture = _game.Content.Load<Texture2D>(_battleObject.Texture);
            _selectTexture = _game.Content.Load<Texture2D>(@"Misc/Select");

            _timeOutlineTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeOutlineTexture.SetData(new Color[] { Color.White });
            _timeInnerTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeInnerTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(_texture, offset, _texture.Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            // time
            spriteBatch.Draw(_timeOutlineTexture, GetTimeOutlineTextureRect(offset), ColourReference.Blue);
            spriteBatch.Draw(_timeInnerTexture, GetTimeInnerTextureRect(offset), ColourReference.Green);

            // health
            spriteBatch.Draw(_timeOutlineTexture, GetHealthOutlineTextureRect(offset), Color.Silver);
            spriteBatch.Draw(_timeInnerTexture, GetHealthInnerTextureRect(offset), Color.Red);

            // selection
            if(Selected) {
                var rect = _selectTexture.Bounds;
                rect.Offset((int)offset.X, (int)offset.Y - (_selectTexture.Bounds.Height + 10));
                spriteBatch.Draw(_selectTexture, rect, Color.White);
            }

            if(_damageText != null) {
                _damageText.Draw(gameTime, spriteBatch, offset + new Vector2(-30, -30));
            }
        }

        public void Update(GameTime gameTime) {
            if(_damageText != null) {
                _damageText.Update(gameTime);

                if(!_damageText.Enabled) {
                    _damageText = null;
                }
            }
        }

        public void StartDamageText(int damage) {
            _damageText = new BattleDamageText(damage);
            _damageText.LoadContent(_game);
        }

        private Rectangle GetTimeOutlineTextureRect(Vector2 position) {
            return new Rectangle((int)position.X + 30, (int)position.Y + _texture.Bounds.Height + 30, 120, 40);
        }

        // this can be a pixel out, blarg
        private Rectangle GetTimeInnerTextureRect(Vector2 position) {
            var rect = GetTimeOutlineTextureRect(position);
            rect.X += 5;
            rect.Y += 5;
            rect.Width -= 10;
            rect.Height -= 10;

            var widthOverPossibleTime = (float)rect.Width / BattleObject.TimeToAction;
            rect.Width = (int)(widthOverPossibleTime * _battleObject.CurrentTimeToAction);

            return rect;
        }

        private Rectangle GetHealthOutlineTextureRect(Vector2 position) {
            return new Rectangle((int)position.X, (int)position.Y + _texture.Bounds.Height + 130, 120, 40);
        }

        private Rectangle GetHealthInnerTextureRect(Vector2 position) {
            var rect = GetHealthOutlineTextureRect(position);
            rect.X += 5;
            rect.Y += 5;
            rect.Width -= 10;
            rect.Height -= 10;

            var widthOverPossibleTime = (float)rect.Width / _battleObject.HPMax;
            rect.Width = (int)(widthOverPossibleTime * _battleObject.HP);

            return rect;
        }
    }
}
