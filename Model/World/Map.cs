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
using System.Xml;
using System.Xml.Linq;

namespace Model.World {
    public class Map {
        private List<List<MapTile>> _mapTiles;
        public List<List<MapTile>> MapTiles { get { return _mapTiles; } }

        public Map(Game game, string filename) {
            _mapTiles = new List<List<MapTile>>();
            LoadMapFromFile(game, filename);
        }

        private void LoadMapFromFile(Game game, string filename) {
            var xdoc = XDocument.Load(filename);

            var usableTiles = GetDistinctTextures(game, xdoc);

            LoadMapFromPreLoadedTiles(game, xdoc, usableTiles);
        }

        private Dictionary<string, Texture2D> GetDistinctTextures(Game game, XDocument xdoc) {
            var usableTiles = new Dictionary<string, Texture2D>();

            var distinctTexture = xdoc.Descendants("tile").Select(x => x.Value).Distinct();
            foreach(var textureString in distinctTexture) {
                usableTiles.Add(textureString, game.Content.Load<Texture2D>(@"MapTiles/" + textureString));
            }

            return usableTiles;
        }

        private void LoadMapFromPreLoadedTiles(Game game, XDocument xdoc, Dictionary<string, Texture2D> usableTiles) {
            _mapTiles = new List<List<MapTile>>();

            var i = 0;
            var j = 0;
            foreach(var row in xdoc.Descendants("row")) {
                _mapTiles.Add(new List<MapTile>());
                j = 0;

                foreach(var tile in row.Descendants("tile")) {
                    var rotation = MathHelper.ToRadians(float.Parse(tile.Attribute("rotation").Value));
                    var hflip = tile.Attribute("hflip").Value;
                    var vflip = tile.Attribute("vflip").Value;

                    var position = new Vector2(i * MapTile.TileSize.X, j * MapTile.TileSize.Y);
                    _mapTiles[i].Add(new MapTile(game, tile.Value, position, rotation));

                    j++;
                }
                i++;
            }
        }
    }
}
