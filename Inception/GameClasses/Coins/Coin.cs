using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Coins
{
    public class Coin
    {
        private Texture2D coinTexture;
        private SoundEffect coinSoundEffect;
        private Rectangle coinSourceRectangle;
        public Rectangle coinRectangle;

        public Coin(Texture2D texture, SoundEffect soundEffect, Vector2 coinPosition)
        {
            coinTexture = texture;
            coinSoundEffect = soundEffect;

            Random r = new Random();
            int randomNumber = r.Next(0, 3);

            coinSourceRectangle = new Rectangle(0, 0, 16, 16);
            coinRectangle = new Rectangle((int)coinPosition.X, (int)coinPosition.Y, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(coinTexture, coinRectangle, coinSourceRectangle, Color.White);
        }

        public bool CheckCollisionWithHero(Hero hero)
        {
            if (coinRectangle.Intersects(hero.heroRectangle))
            {
                return true;
            }
            return false;
        }

        public void PickupCoin()
        {
            coinSoundEffect.Play();
        }
    }
}
