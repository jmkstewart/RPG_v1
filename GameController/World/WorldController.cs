using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using View.World;
using Model.World;

namespace GameController.World {
    interface IMonsterCollisionListener {
        void MosterCollision(int worldMonsterId);
    }

    class WorldController : IController {
        private Map _map;
        private MapView _mapView;
        private WorldComponentDrawer _componentDrawer = new WorldComponentDrawer();

        private WorldPlayer _worldPlayer;
        private WorldPlayerView _worldPlayerView;
        private WorldPlayerController _worldPlayerController;

        private WorldMonsterController _worldMonsterController;
        
        private CameraOffset _cameraOffset;

        // listeners
        IMonsterCollisionListener _monsterCollisionListener;

        public WorldController(IMonsterCollisionListener monsterCollisionListener) {
            _monsterCollisionListener = monsterCollisionListener;
        }

        // when this gets too big spin off a new controller
        public void LoadContent(Game game) {
            var screenCentre = new Point(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            _cameraOffset = new CameraOffset(screenCentre);
            
            _worldPlayer = new WorldPlayer();
            _worldPlayer.Postion = new Vector2(850, 550);
            _worldPlayerView = new WorldPlayerView(_worldPlayer);
            _worldPlayerView.LoadContent(game);
            _worldPlayerController = new WorldPlayerController(_worldPlayer);

            _worldMonsterController = new WorldMonsterController(_cameraOffset);
            _worldMonsterController.LoadContent(game);

            _map = new Map(game, @"C:\Users\Jake\Documents\Games\RPG Content\Maps\map2.xml");
            _mapView = new MapView(game, _map);

            // add views to ComponentDrawer
            _componentDrawer.MapView = _mapView;
            _componentDrawer.PlayerView = _worldPlayerView;
        }

        public void Update(GameTime gameTime) {
            // get the camera offset by looking at the player position
            _cameraOffset.CentreOnPoint(_worldPlayer.Postion);

            _worldPlayerController.Update(gameTime);
            _worldMonsterController.Update(gameTime);

            var collidedMonsterId = _worldMonsterController.GetFirstCollision(_worldPlayer);
            if(collidedMonsterId != 0) {
                _monsterCollisionListener.MosterCollision(collidedMonsterId);
            }

            _worldPlayerView.Update(gameTime);
        }

        public void DrawComponents(GameTime gameTime, SpriteBatch spriteBatch) {
            _componentDrawer.DrawComponents(gameTime, spriteBatch, _cameraOffset.Offset);
            _worldMonsterController.DrawComponents(gameTime, spriteBatch);
        }
    }
}
