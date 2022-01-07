using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.Interfaces
{
    public interface IGameObject
    {
        void Update(GameTime gameTime);
        
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);  
    }
}
