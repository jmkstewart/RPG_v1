using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View.World {
    public class WorldComponentDrawer {
        public IDrawableObject MapView { get; set; }
        public IDrawableObject PlayerView { get; set; }
        public List<IDrawableObject> EncounterableView { get; set; }

        public WorldComponentDrawer() {
            EncounterableView = new List<IDrawableObject>();
        }

        public void DrawComponents(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            if(MapView != null) {
                MapView.Draw(gameTime, spriteBatch, offset);
            }
            if(PlayerView != null) {
                PlayerView.Draw(gameTime, spriteBatch, offset);
            }

            EncounterableView.ForEach(x => x.Draw(gameTime, spriteBatch, offset));
        }
    }
}
