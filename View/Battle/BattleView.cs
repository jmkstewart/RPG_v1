using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.Battle;

namespace View.Battle {
    public class BattleView : IDrawableObject {
        public List<InteractableBattleObjectView> PlayerPartyView;
        public List<InteractableBattleObjectView> EnemyPartyView;

        private BattleBackgroundView _battleBackgroundView;

        public BattleView(Game game, List<BattleObject> playerParty, List<BattleObject> enemyParty) {
            _battleBackgroundView = new BattleBackgroundView(game);
            PlayerPartyView = new List<InteractableBattleObjectView>();
            EnemyPartyView = new List<InteractableBattleObjectView>();

            var position = 0;
            foreach(var enemy in enemyParty) {
                var enemyView = new BattleObjectView(game, enemy,(BattleObjectView.BattlePosition)position);
                EnemyPartyView.Add(enemyView);
                position++;
            }

            position = 3;
            foreach(var player in playerParty) {
                var playerView = new BattleObjectView(game, player, (BattleObjectView.BattlePosition)position);
                PlayerPartyView.Add(playerView);
                position++;
            }
        }

        public void Update(GameTime gameTime) {
            PlayerPartyView.ForEach(x => x.Update(gameTime));
            EnemyPartyView.ForEach(x => x.Update(gameTime));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            _battleBackgroundView.Draw(gameTime, spriteBatch, offset);

            PlayerPartyView.ForEach(x => x.Draw(gameTime, spriteBatch, offset));
            EnemyPartyView.ForEach(x => x.Draw(gameTime, spriteBatch, offset));
        }
    }
}
