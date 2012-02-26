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
        private List<Texture2D> _textures;
        private int _texture;

        private SpriteFont _font;
        private Texture2D _selectTexture;
        private BattleObject _battleObject;
        private BattleDamageText _damageText = null;

        public BattlePosition CurrentBattlePosition { get; set; }
        public string Name { get { return _battleObject.Name; } }
        public bool Selected { get; set; }

        private readonly int _timeBetweenFrames = 250;
        private int _timeToFrameChange = 250;

        public BattleObjectView(Game game, BattleObject battleObject, BattlePosition battlePosition) {
            _game = game;
            _battleObject = battleObject;
            LoadView(battleObject.Id);
            CurrentBattlePosition = battlePosition;
        }
        
        private void LoadView(int id) {
            // load from a file based on the id
            _textures = new List<Texture2D>();
            foreach(var texture in _battleObject.Texture.Split(',')) {
                _textures.Add(_game.Content.Load<Texture2D>(texture));
            }
            _texture = 0;

            _selectTexture = _game.Content.Load<Texture2D>(@"Misc/Select");
            _font = _game.Content.Load<SpriteFont>("Fonts/TrayNameFont");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(_textures[_texture], offset, _textures[_texture].Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            // selection
            if(Selected) {
                var rect = _selectTexture.Bounds;
                rect.Offset((int)offset.X, (int)offset.Y - (_selectTexture.Bounds.Height + 10));
                spriteBatch.Draw(_selectTexture, rect, Color.White);
                spriteBatch.DrawString(_font, _battleObject.Name, new Vector2(rect.X, rect.Y + rect.Height), Color.White);
            }

            if(_damageText != null) {
                _damageText.Draw(gameTime, spriteBatch, offset + new Vector2(-30, -30));
            }
        }

        public void Update(GameTime gameTime) {
            _timeToFrameChange -= gameTime.ElapsedGameTime.Milliseconds;
            if(_timeToFrameChange <= 0) {
                _timeToFrameChange += _timeBetweenFrames;
                _texture++;
                if(_texture >= _textures.Count) {
                    _texture = 0;
                }
            }

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
    }
}
