using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.Battle;

namespace View.Battle {
    public class BattleObjectView : InteractableBattleObjectView {
        public enum BattlePosition {
            EnemyLeft = 0, EnemyMiddle = 1, EnemyRight = 2,
            PlayerLeft = 3, PlayerMiddle = 4, PlayerRight = 5
        }

        private Game _game;
        private Texture2D _texture;
        private Texture2D _timeOutlineTexture;
        private Texture2D _timeInnerTexture;

        private BattlePosition _battlePosition;
        private BattleObject _battleObject;

        private BattleDamageText _damageText = null;

        public string Name {
            get {
                return _battleObject.Name;
            }
        }

        public BattleObjectView(Game game, BattleObject battleObject, BattlePosition battlePosition) {
            _game = game;
            LoadView(battleObject.Id);
            _battleObject = battleObject;
            _battlePosition = battlePosition;
        }
        
        private void LoadView(int id) {
            // load from a file based on the id
            if(id == 0) {
                _texture = _game.Content.Load<Texture2D>(@"Monsters/mon027");
            } else if(id == 1) {
                _texture = _game.Content.Load<Texture2D>(@"Player/Duder");
            }

            _timeOutlineTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeOutlineTexture.SetData(new Color[] { Color.White });
            _timeInnerTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeInnerTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            var position = GetPositionVector() + offset;
            spriteBatch.Draw(_texture, position, _texture.Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_timeOutlineTexture, GetTimeOutlineTextureRect(position), ColourReference.Blue);
            spriteBatch.Draw(_timeInnerTexture, GetTimeInnerTextureRect(position), ColourReference.Green);

            if(_damageText != null) {
                _damageText.Draw(gameTime, spriteBatch, GetPositionVector() + offset + new Vector2(-30, -30));
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

        private Vector2 GetPositionVector() {
            switch(_battlePosition) {
                case BattlePosition.EnemyLeft: return new Vector2(200, 200);
                case BattlePosition.EnemyMiddle: return new Vector2(400, 200);
                case BattlePosition.EnemyRight: return new Vector2(600, 200);
                case BattlePosition.PlayerLeft: return new Vector2(200, 600);
                case BattlePosition.PlayerMiddle: return new Vector2(400, 600);
                case BattlePosition.PlayerRight: return new Vector2(600, 600);
            }
            return Vector2.Zero;
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
    }
}
