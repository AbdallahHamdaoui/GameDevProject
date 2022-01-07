using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Enemy
    {
        private Texture2D enemyTexture;
        private Animation enemyCurrentAnimation;
        private bool enemyIsFacingLeft = true;

        public float enemySpeed;
        public Vector2 enemyPosition;
        public Rectangle enemyRectangle;
        public Rectangle enemyPathway;

        public Enemy(Texture2D enemytexture, Rectangle enemyposition, float enemyspeed, GraphicsDeviceManager _graphics = null)
        {
            enemyTexture = enemytexture;
            enemySpeed = enemyspeed;
            enemyCurrentAnimation = new Animation(enemytexture, 0, 32, 32);
            enemyPosition = new Vector2(enemyposition.X, enemyposition.Y);
            enemyRectangle = new Rectangle(enemyposition.X, enemyposition.Y, 32, 32);
            enemyPathway = enemyposition;
        }

        public void Unload() { }

        public void Update(Rectangle heroRectangle)
        {
            if (!enemyPathway.Contains(enemyRectangle))
            {
                enemySpeed = -enemySpeed;
                enemyIsFacingLeft = !enemyIsFacingLeft;
            }

            if (enemyRectangle.Intersects(heroRectangle))
            {
                Game1.heroHasLost = true;
            }

            enemyPosition.X += enemySpeed;
            enemyRectangle.X = (int)enemyPosition.X;
            enemyRectangle.Y = (int)enemyPosition.Y;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (enemyIsFacingLeft)
            {
                enemyCurrentAnimation.Draw(spriteBatch, enemyPosition, gameTime, SpriteEffects.None, 100);
            }
            else
            {
                enemyCurrentAnimation.Draw(spriteBatch, enemyPosition, gameTime, SpriteEffects.FlipHorizontally, 100);
            }
        } 
    }
}
