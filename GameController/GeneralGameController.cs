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

namespace GameController {
    public class GeneralGameController : IMonsterCollisionListener {
        private enum GameState { World, Battle, Menu, Start, Pause }

        private GameState _gameState = GameState.Start;
        private WorldController _worldController;
        private BattleController _battleController;

        private Player _player = null;

        private Game _game;

        public GeneralGameController() {
            _worldController = new WorldController(this);
            _battleController = new BattleController();
            _gameState = GameState.World;
        }

        public void LoadContent(Game game) {
            _game = game;
            _worldController.LoadContent(game);
        }

        public void Update(GameTime gameTime) {
            if(_gameState == GameState.World) {
                _worldController.Update(gameTime);
            } else if(_gameState == GameState.Battle) {
                _battleController.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if(_gameState == GameState.World) {
                _worldController.DrawComponents(gameTime, spriteBatch);
            } else if(_gameState == GameState.Battle) {
                _battleController.DrawComponents(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public void MosterCollision(int worldMonsterId) {
            // When we collide with a monster we want to change into the battle screen
            SwitchStateToBattle(worldMonsterId);
        }

        private void SwitchStateToBattle(int encounterId) {
            // change the game state
            _gameState = GameState.Battle;

            // initialize the battle controller (pass it the worldMonsterId)
            _battleController.Init(encounterId, _player);
            _battleController.LoadContent(_game);
        }
    }
}
