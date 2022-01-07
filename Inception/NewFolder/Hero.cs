using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Hero
    {
        private Texture2D heroIdleTexture;
        private Texture2D heroRunTexture;
        private Animation[] heroIdleAnimation;
        private Animation[] heroRunAnimation;       
        private Animation heroIdleCurrentAnimation;
        private Animation heroRunCurrentAnimation;
        public Vector2 heroMovement;
        public Rectangle heroRectangle;
        public Rectangle heroJumpPoint;
        public bool heroIsFalling;
        public bool heroIsJumping;
        public bool heroIsFacingLeft;
        public float heroFallSpeed = 1f;
        public float heroJumpSpeed;

        private Bullet bullet;
        private float timeBetweenBullets = 2;
        private SoundEffect bulletSoundEffect;
        public List<Bullet> bulletList;

        public Hero(Vector2 heroStartPoint)
        {
            heroMovement = heroStartPoint;
            heroRectangle = new Rectangle((int)heroMovement.X, (int)(heroMovement.Y - 1), 32, 20);
            heroJumpPoint = new Rectangle((int)(heroMovement.X + 8), (int)(heroMovement.Y + 40), 32, 1);
            bulletList = new List<Bullet>();
            heroIdleAnimation = new Animation[4];
            heroRunAnimation = new Animation[4];
            //this.heroIdleTexture = heroIdleTexture;
            //this.heroRunTexture = heroRunTexture;
            //this.bulletSoundEffect = bulletSoundEffect;
        }

        public void LoadContent(ContentManager content)
        {
            heroIdleTexture = content.Load<Texture2D>("images\\heroIdle");
            heroRunTexture = content.Load<Texture2D>("images\\heroRun");
            heroIdleAnimation[0] = new Animation(heroIdleTexture, 0, 32, 32);
            heroRunAnimation[0] = new Animation(heroRunTexture, 0, 32, 32);
            heroIdleCurrentAnimation = heroIdleAnimation[0];
            heroRunCurrentAnimation = heroIdleCurrentAnimation;
            bulletSoundEffect = content.Load<SoundEffect>("audio\\shootSound");
        }

        public void Update(GameTime gameTime, Texture2D bulletTexture, float moveSpeed)
        {
            heroRunCurrentAnimation = heroIdleCurrentAnimation;
            heroMovement.Y += heroFallSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && timeBetweenBullets > 1)
            {
                bulletSoundEffect.Play();

                if (!heroIsFacingLeft)
                {
                    bullet = new Bullet(bulletTexture, 2, new Vector2(heroMovement.X + 16, heroMovement.Y + 15));
                }
                else
                {
                    bullet = new Bullet(bulletTexture, -2, new Vector2(heroMovement.X, heroMovement.Y + 15));
                }

                bulletList.Add(bullet);
                timeBetweenBullets = 0;
            }
            else
            {
                timeBetweenBullets += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                heroMovement.X -= moveSpeed;
                heroRunCurrentAnimation = heroRunAnimation[0];
                heroIsFacingLeft = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                heroMovement.X += moveSpeed;
                heroRunCurrentAnimation = heroRunAnimation[0];
                heroIsFacingLeft = false;
            }

            heroRectangle.X = (int)heroMovement.X;
            heroRectangle.Y = (int)heroMovement.Y;
            heroJumpPoint.X = (int)heroMovement.X;
            heroJumpPoint.Y = (int)(heroMovement.Y + 35);
        }

        public void Jump()
        {

            var heroStartY = heroMovement.Y;

            if (heroIsFalling)
            {
                heroFallSpeed = 5;
            }
            else if (!heroIsFalling)
            {
                heroFallSpeed = 0;
            }

            if (heroIsJumping) // If hero is jumping
            {
                heroMovement.Y += heroJumpSpeed;
                heroJumpSpeed += 1;

                if (heroMovement.Y >= heroStartY )
                {
                    heroMovement.Y = heroStartY; // Movement on the y-axis is equal to heroStartY
                    heroIsJumping = false; // Sets jumping on false
                }
            }
            else // If hero isn't jumping
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Z) && !heroIsFalling) // If the key Z is clicked
                {
                    heroIsJumping = true; // Sets jumping on true
                    heroJumpSpeed = -20; // Goes up 
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects effects, GameTime gameTime)
        {
            heroRunCurrentAnimation.Draw(spriteBatch, heroMovement, gameTime, effects, 100);
        }

        
    }
}
