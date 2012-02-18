using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.World;

namespace View.World {
    public class MapView : IDrawableObject {
        private Map _map;
        private List<MapTileView> _mapTilesViews;
        
        public MapView(Game game, Map map) {
            _map = map;
            _mapTilesViews = new List<MapTileView>();

            var usableTextures = GetDistinctTextures(game);

            foreach(var row in _map.MapTiles) {
                foreach(var mapTile in row) {
                    _mapTilesViews.Add(new MapTileView(game, mapTile, usableTextures[mapTile.TextureName]));
                }
            }
        }
        
        private Dictionary<string, Texture2D> GetDistinctTextures(Game game) {
            var tileNames = _map.MapTiles.SelectMany(x => x.Select(y => y.TextureName));
            var distinctTextures = tileNames.Distinct();

            var usableTextures = new Dictionary<string, Texture2D>();

            foreach(var textureString in distinctTextures) {
                usableTextures.Add(textureString, game.Content.Load<Texture2D>(@"MapTiles/" + textureString));
            }

            return usableTextures;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset) {
            _mapTilesViews.ForEach(x => x.Draw(gameTime, spriteBatch, offset));
        }
    }
}
