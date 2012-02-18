using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace View.Battle {
    public interface IBattleObjectView : IDrawableObject {
        void Update(GameTime gameTime);
    }
}
