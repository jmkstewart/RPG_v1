using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Microsoft.Xna.Framework.Input;
using View.Utility;

namespace View.Battle {
    public interface MenuItemSelectListener {
        void ItemClicked(string item);
        void ItemSelected(string objectName);
    }

    public class BattleMenuItemView : IBattleObjectView {
        private MenuItemSelectListener _clickListener = null;

        private List<BattleMenuItemView> _childMenuItems;

        private Texture2D _singleColourTexture;
        private SpriteFont _menuFont;
        private Game _game;

        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool HighLighted { get; set; }
        public bool Root { get; set; }

        public readonly int Width = 220;

        public BattleMenuItemView(Game game, MenuItemSelectListener clickListener) {
            _childMenuItems = new List<BattleMenuItemView>();
            _game = game;
            _clickListener = clickListener;

            LoadView();
        }

        private void LoadView() {
            _singleColourTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _singleColourTexture.SetData(new Color[] { Color.White });
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

                var outerRect = GetMenuOutlineTextureRect(position, offset);
                DrawingHelper.DrawBlackOutline(spriteBatch, _singleColourTexture, outerRect);
                spriteBatch.Draw(_singleColourTexture, outerRect, null, ColourReference.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);

                var innerRect = GetMenuInnerTextureRect(position, offset);
                spriteBatch.Draw(_singleColourTexture, innerRect, null, mainColour, 0, Vector2.Zero, SpriteEffects.None, 0);

                spriteBatch.DrawString(_menuFont, Text, position + offset + new Vector2(20, 10), ColourReference.Orange, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            if(Selected) {
                var currentMiddle = offset.Y + 30;
                var heightOfNextRow = _childMenuItems.Count * 60;
                var newStart = currentMiddle - (heightOfNextRow / 2);
                offset.Y = newStart;

                foreach(var childMenuItem in _childMenuItems) {
                    childMenuItem.Draw(gameTime, spriteBatch, offset + new Vector2(Width + 2, 0));
                    offset.Y += 60;
                }
            }
        }

        private Rectangle GetMenuOutlineTextureRect(Vector2 position, Vector2 offset) {
            return new Rectangle((int)position.X + (int)offset.X, (int)position.Y + (int)offset.Y, Width, 60);
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
                    if(IsLeaf()) {
                        Click();
                    } else {
                        MoveRight();
                    }
                }
            } else {
                _childMenuItems.ForEach(x => x.KeyPressed(key));
            }
        }

        private bool IsLeaf() {
            var highlighted = GetHighlightedIndex();
            return _childMenuItems[highlighted]._childMenuItems.Count == 0;
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

        public int CurrentWidth {
            get {
                var depth = GetDepthOfHighlightedItem();
                return (depth - 1) * Width;  // minus 1 to ignore the depth of the root node
            }
        }

        public int GetDepthOfHighlightedItem() {
            if(HighLighted) {
                return 1;
            } else {
                foreach(var childMenuItemView in _childMenuItems) {
                    var depth = childMenuItemView.GetDepthOfHighlightedItem();
                    if(depth > 0) {
                        return depth + 1;
                    }
                }
            }

            return 0;
        }
    }
}
