using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Animation
    {
        private Texture2D texture;
        private int column;
        private int width;
        private int height;
        private int frames;
        private int counter = 0;
        private int timeSinceLastFrame = 0;

        public Animation(Texture2D texture, int c, int w, int h)
        {
            this.texture = texture;
            column = c;
            width = w;
            height = h;
            frames = texture.Width / width;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, SpriteEffects spriteEffects, int millisecondsPerFrame = 500) 
        {
            if (counter < frames)
            {
                var rectangle = new Rectangle(width * counter, height * column, 32, 32);
                spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, new Vector2(), 1f, spriteEffects, 1f);
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    counter++;

                    if (counter == frames)
                    {
                        counter = 0;
                    }
                }
            }
            //else
            //{
            //    counter = 0;
            //}
        }
    }
}
