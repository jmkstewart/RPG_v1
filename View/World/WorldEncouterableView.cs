using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.World {
    public class WorldEncouterableView : WorldSpriteBase, IDrawableObject {
        private WorldMonster _worldMonster;

        public WorldEncouterableView(WorldMonster worldMonster) {
            _worldMonster = worldMonster;
        }
        
        public void LoadContent(Game game) {
            var texture = game.Content.Load<Texture2D>(@"Monsters/mon027");
            base.LoadContent(texture, _worldMonster.Postion, 1);

            CurrentFrameRect = new Rectangle(0, 0, 55, 58);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(texture, _worldMonster.Postion + offset, CurrentFrameRect, Color.White, 0, Vector2.Zero, 1, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime) {
            //UpdateFrame(gameTime);
            UpdateMosterFlipped();
        }

        private void UpdateMosterFlipped() {
            if(_worldMonster.Direction.X > 0) {
                Flipped = false;
            } else if(_worldMonster.Direction.X < 0) {
                Flipped = true;
            }
        }
    }
}
