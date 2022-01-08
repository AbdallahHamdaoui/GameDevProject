using Inception.GameClasses.GameStates;
using Inception.GameClasses.GameStates.Levels;
using Inception.GameClasses.GameStates.Menu_s;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Enemies
{
    public class Enemy
    {
        private Animation runAnimation;
        private bool enemyIsFacingLeft = true;
        private SoundEffect deathSoundEffect;
        private float enemySpeed;
        public Vector2 enemyPosition;
        public Rectangle enemyRectangle;
        public Rectangle enemyPathway;

        public Enemy(Texture2D texture, SoundEffect soundEffect, Rectangle enemyposition, float enemyspeed, GraphicsDeviceManager _graphics = null)
        {
            this.enemySpeed = enemyspeed;
            enemyPosition = new Vector2(enemyposition.X, enemyposition.Y);
            enemyRectangle = new Rectangle(enemyposition.X, enemyposition.Y, 32, 32);
            runAnimation = new Animation(texture, 0, 32, 32);
            deathSoundEffect = soundEffect;
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

            enemyPosition.X += enemySpeed;
            enemyRectangle.X = (int)enemyPosition.X;
            enemyRectangle.Y = (int)enemyPosition.Y;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (enemyIsFacingLeft)
            {
                runAnimation.Draw(spriteBatch, enemyPosition, gameTime, SpriteEffects.None, 100);
            }
            else
            {
                runAnimation.Draw(spriteBatch, enemyPosition, gameTime, SpriteEffects.FlipHorizontally, 100);
            }
        }

        public bool CheckCollisionWithHero(Hero hero)
        {
            if (enemyRectangle.Intersects(hero.heroRectangle))
            {
                return true;
            }
            return false;
        }

        public void Dies()
        {
            deathSoundEffect.Play();
        }
    }
}
