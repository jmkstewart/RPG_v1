using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Microsoft.Xna.Framework.Input;

namespace View.Battle {
    public interface MenuItemSelectListener {
        void ItemClicked(string item);
    }

    public class BattleMenuItemView : IBattleObjectView {
        private MenuItemSelectListener _clickListener = null;

        private List<BattleMenuItemView> _childMenuItems;

        private Texture2D _timeOutlineTexture;
        private Texture2D _timeInnerTexture;
        private SpriteFont _menuFont;
        private Game _game;

        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool HighLighted { get; set; }
        public bool Root { get; set; }

        public BattleMenuItemView(Game game, MenuItemSelectListener clickListener) {
            _childMenuItems = new List<BattleMenuItemView>();
            _game = game;
            _clickListener = clickListener;

            LoadView();
        }

        private void LoadView() {
            _timeOutlineTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeOutlineTexture.SetData(new Color[] { Color.White });
            _timeInnerTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _timeInnerTexture.SetData(new Color[] { Color.White });
            _menuFont = _game.Content.Load<SpriteFont>("Fonts/MenuFont");
        }

        public void Add(BattleMenuItemView childMenu) {
            _childMenuItems.Add(childMenu);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            if(!Root) {
                var position = new Vector2(0, 0);
                Color mainColour = HighLighted ? Color.SandyBrown : ColourReference.Green;
                if(Selected) mainColour = Color.Silver;
                spriteBatch.Draw(_timeOutlineTexture, GetMenuOutlineTextureRect(position, offset), null, ColourReference.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(_timeInnerTexture, GetMenuInnerTextureRect(position, offset), null, mainColour, 0, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.DrawString(_menuFont, Text, position + offset + new Vector2(20, 10), ColourReference.Orange, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            // if selected then draw the child items
            if(Selected) {
                foreach(var childMenuItem in _childMenuItems) {
                    childMenuItem.Draw(gameTime, spriteBatch, offset + new Vector2(220, 30));
                    offset.Y += 60;
                }
            }
        }

        private Rectangle GetMenuOutlineTextureRect(Vector2 position, Vector2 offset) {
            return new Rectangle((int)position.X + (int)offset.X, (int)position.Y + (int)offset.Y, 220, 60);
        }

        private Rectangle GetMenuInnerTextureRect(Vector2 position, Vector2 offset) {
            var rect = GetMenuOutlineTextureRect(position, offset);
            rect.Inflate(-10, -10);
            return rect;
        }

        public void Select() {
            if(_childMenuItems.Count > 0) {
                Selected = true;
                HighLighted = false;
                _childMenuItems[0].HighLighted = true;
            }
        }

        public void SetTree(Tree<string> optionTree) {
            foreach(var child in optionTree.Children) {
                var battleItem = new BattleMenuItemView(_game, _clickListener) { Text = child.Value };
                battleItem.SetTree(child);
                Add(battleItem);
            }
        }

        public void Update(GameTime gameTime) {
        }

        public void KeyPressed(Keys key) {
            // check if one of the current children is pressed
            var oneIsHighlighted = _childMenuItems.Any(x => x.HighLighted);

            if(oneIsHighlighted) {
                if(key == Keys.D) {
                    MoveRight();
                } else if(key == Keys.W) {
                    MoveUp();
                } else if(key == Keys.S) {
                    MoveDown();
                } else if(key == Keys.A) {
                    MoveLeft();
                } else if(key == Keys.E) {
                    Click();
                }
            } else {
                _childMenuItems.ForEach(x => x.KeyPressed(key));
            }
        }

        private void Click() {
            var highlighted = GetHighlightedIndex();
            _clickListener.ItemClicked(_childMenuItems[highlighted].Text);
        }

        private void MoveRight() {
            var highlighted = GetHighlightedIndex();
            _childMenuItems[highlighted].Select();
        }

        private void MoveLeft() {
            if(!Root) {
                var highlighted = GetHighlightedIndex();
                _childMenuItems[highlighted].HighLighted = false;
                Selected = false;
                HighLighted = true;
            }
        }

        private void MoveUp() {
            var highlighted = GetHighlightedIndex();
            if(highlighted > 0) {
                _childMenuItems[highlighted].HighLighted = false;
                _childMenuItems[highlighted - 1].HighLighted = true;
            }
        }

        private void MoveDown() {
            var highlighted = GetHighlightedIndex();
            if(highlighted < _childMenuItems.Count - 1) {
                _childMenuItems[highlighted].HighLighted = false;
                _childMenuItems[highlighted + 1].HighLighted = true;
            }
        }

        private int GetHighlightedIndex() {
            int index = 0;
            int i = 0;
            foreach(var child in _childMenuItems) {
                if(child.HighLighted) {
                    index = i;
                }
                i++;
            }
            return index;
        }

        public void SetFirstSelected() {
            _childMenuItems[0].HighLighted = true;
        }
    }
}
