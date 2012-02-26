using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.Battle;
using Utilities;
using Microsoft.Xna.Framework.Input;

namespace View.Battle {
    public class BattleObjectMenuView : IDrawableObject {
        private BattleObject _activeBattleObject;
        private BattleMenuItemView _menuItemView;

        public int CurrentWidth { get { return _menuItemView.CurrentWidth; } }

        public BattleObjectMenuView(Game game, BattleObject activeBattleObject, MenuItemSelectListener selectListener) {
            _activeBattleObject = activeBattleObject;
            _menuItemView = new BattleMenuItemView(game, selectListener) { Root = true, Selected = true };

            LoadTreeFromBattleObject(game, _activeBattleObject);
        }

        private void LoadTreeFromBattleObject(Game game, BattleObject activeBattleObject) {
            Tree<string> optionTree = new Tree<string>("root");
            optionTree.AddChild("Attack");
            foreach(var attack in activeBattleObject.AttackList) {
                optionTree.GetFirstChild("Attack").AddChild(attack);
            }

            optionTree.AddChild("Magic");
            foreach(var magic in activeBattleObject.MagicList) {
                optionTree.GetFirstChild("Magic").AddChild(magic);
            }

            optionTree.AddChild("Item");
            foreach(var item in activeBattleObject.ItemList) {
                optionTree.GetFirstChild("Item").AddChild(item);
            }

            _menuItemView.SetTree(optionTree);
            _menuItemView.SetFirstSelected();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            _menuItemView.Draw(gameTime, spriteBatch, offset);
        }

        public void KeyPressed(Keys key) {
            _menuItemView.KeyPressed(key);
        }
    }
}
