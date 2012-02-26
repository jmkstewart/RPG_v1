using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Model;
using GameController.World;
using GameController.Battle;
using View;

namespace GameController {
    public class GeneralGameController : IMonsterCollisionListener {
        private enum GameState { World, Battle, BattleTransition, Menu, Start, Pause }

        private GameState _gameState = GameState.Start;
        private WorldController _worldController;
        private BattleController _battleController;
        private ScreenTransition _screenTransition;

        private Player _player = null;
        private int _upcomingEncounterId;

        private Game _game;

        public GeneralGameController() {
            _worldController = new WorldController(this);
            _battleController = new BattleController();
            _gameState = GameState.World;
        }

        public void LoadContent(Game game) {
            _game = game;
            _worldController.LoadContent(game);
            _screenTransition = new ScreenTransition(_game, new Rectangle(0, 0, _game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height));
        }

        public void Update(GameTime gameTime) {
            if(_gameState == GameState.World) {
                _worldController.Update(gameTime);
            } else if(_gameState == GameState.Battle) {
                _battleController.Update(gameTime);
            } else if(_gameState == GameState.BattleTransition) {
                _screenTransition.Update(gameTime);

                if(!_screenTransition.Enabled) {
                    SwitchStateToBattle(_upcomingEncounterId);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if(DrawWorld()) {
                _worldController.DrawComponents(gameTime, spriteBatch);
            } else if(DrawBattle()) {
                _battleController.DrawComponents(gameTime, spriteBatch);
            }

            if(_gameState == GameState.BattleTransition) {
                _screenTransition.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private bool DrawWorld() {
            if(_gameState == GameState.World) return true;
            if(_gameState == GameState.BattleTransition && !_screenTransition.FadedOut) return true;
            return false;
        }

        private bool DrawBattle() {
            if(_gameState == GameState.Battle) return true;
            if(_gameState == GameState.BattleTransition && _screenTransition.FadedOut) return true;
            return false;
        }

        public void MosterCollision(int worldMonsterId) {
            // When we collide with a monster we want to transition
            SwitchStateToBattleTransition(worldMonsterId);
        }

        private void SwitchStateToBattleTransition(int encounterId) {
            _gameState = GameState.BattleTransition;
            _upcomingEncounterId = encounterId;

            // initialize the battle controller (pass it the worldMonsterId)
            _battleController.Init(encounterId, _player);
            _battleController.LoadContent(_game);
        }

        private void SwitchStateToBattle(int encounterId) {
            // change the game state
            _gameState = GameState.Battle;
        }
    }
}
