using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Model.World;
using View.World;

namespace GameController.World {
    class WorldMonsterController : IController {
        private List<WorldMonster> _worldMonsters;
        private List<WorldEncouterableView> _worldEncounterableViews;

        private CameraOffset _cameraOffset;
        private WorldComponentDrawer _componentDrawer = new WorldComponentDrawer();

        public WorldMonsterController(CameraOffset cameraOffset) {
            _cameraOffset = cameraOffset;
            _worldMonsters = new List<WorldMonster>();
            _worldEncounterableViews = new List<WorldEncouterableView>();
        }

        public void LoadContent(Game game) {
            var worldMonster = new WorldMonster();
            worldMonster.Id = 1;
            worldMonster.Postion = new Vector2(1000, 550);
            worldMonster.Size = new Point(55, 58);
            _worldMonsters.Add(worldMonster);

            var worldEncounterableView = new WorldEncouterableView(worldMonster);
            worldEncounterableView.LoadContent(game);
            _worldEncounterableViews.Add(worldEncounterableView);

            _componentDrawer.EncounterableView.AddRange(_worldEncounterableViews);
        }

        public void Update(GameTime gameTime) {
            //throw new NotImplementedException();
        }

        public void DrawComponents(GameTime gameTime, SpriteBatch spriteBatch) {
            _componentDrawer.DrawComponents(gameTime, spriteBatch, _cameraOffset.Offset);
        }

        public int GetFirstCollision(WorldPlayer worldPlayer) {
            foreach(var monster in _worldMonsters) {
                if(worldPlayer.CollisionRect.Intersects(monster.CollisionRect)) {
                    return monster.Id;
                }
            }
            return 0;
        }
    }
}
