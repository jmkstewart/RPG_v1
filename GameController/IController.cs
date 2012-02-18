using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameController {
    interface IController {
        void LoadContent(Game game);
        void Update(GameTime gameTime);
        void DrawComponents(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
