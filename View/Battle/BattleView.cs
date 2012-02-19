using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Model.Battle;

namespace View.Battle {
    public class BattleView : IDrawableObject {
        public List<InteractableBattleObjectView> PlayerPartyView;
        public List<InteractableBattleObjectView> EnemyPartyView;

        private BattleBackgroundView _battleBackgroundView;
        private MenuItemSelectListener _menuItemSelectListener;

        public BattleView(Game game, List<BattleObject> playerParty, List<BattleObject> enemyParty, MenuItemSelectListener menuItemSelectListener) {
            _battleBackgroundView = new BattleBackgroundView(game);
            _menuItemSelectListener = menuItemSelectListener;
            PlayerPartyView = new List<InteractableBattleObjectView>();
            EnemyPartyView = new List<InteractableBattleObjectView>();

            var position = 0;
            foreach(var enemy in enemyParty) {
                var enemyView = new BattleObjectView(game, enemy,(BattlePosition)position);
                EnemyPartyView.Add(enemyView);
                position++;
            }

            position = 3;
            foreach(var player in playerParty) {
                var playerView = new BattleObjectView(game, player, (BattlePosition)position);
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

            for(int i = 0; i < PlayerPartyView.Count; i++) {
                var position = new Vector2(200 * (i + 1), 500) + offset;
                PlayerPartyView[i].Draw(gameTime, spriteBatch, position);
            }
            for(int i = 0; i < EnemyPartyView.Count; i++) {
                var position = new Vector2(200 * (i + 1), 100) + offset;
                EnemyPartyView[i].Draw(gameTime, spriteBatch, position);
            }
        }

        public void RemoveEnemy(string name) {
            var enemyToRemove = EnemyPartyView.First(x => x.Name == name);
            EnemyPartyView.Remove(enemyToRemove);
        }

        public void RemovePlayer(string name) {
            var playerToRemove = PlayerPartyView.First(x => x.Name == name);
            PlayerPartyView.Remove(playerToRemove);
        }

        public void SelectionKeyPressed(Keys key) {
            if(key == Keys.E) {
                var selectedObject = GetAllBattleObjectViews().First(x => x.Selected);
                _menuItemSelectListener.ItemSelected(selectedObject.Name);
            } else if(key == Keys.A) {
                MoveSelectionLeft();
            } else if(key == Keys.D) {
                MoveSelectionRight();
            } else if(key == Keys.S) {
            } else if(key == Keys.W) {
            }
        }

        private void MoveSelectionLeft() {
            var partyViews = GetAllBattleObjectViews();
            var position = partyViews.FindIndex(x => x.Selected);

            if(position - 1 >= 0) {
                partyViews[position - 1].Selected = true;
                partyViews[position].Selected = false;
            }
        }

        private void MoveSelectionRight() {
            var partyViews = GetAllBattleObjectViews();
            var position = partyViews.FindIndex(x => x.Selected);

            if(position + 1 < partyViews.Count) {
                partyViews[position + 1].Selected = true;
                partyViews[position].Selected = false;
            }
        }

        public List<InteractableBattleObjectView> GetAllBattleObjectViews() {
            return EnemyPartyView.Union(PlayerPartyView).ToList();
        }
    }
}
