using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Coins
{
    public class CoinManager
    {
        private List<Coin> coins;
        private Texture2D coinTexture;
        private SoundEffect coinSoundEffect;

        public CoinManager()
        {
            coins = new List<Coin>();
        }

        public void LoadContent(ContentManager content)
        {
            coinTexture = content.Load<Texture2D>("images\\coin");
            coinSoundEffect = content.Load<SoundEffect>("audio\\pointSound");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }
        }

        public void CheckCollisionWithHero(Hero hero)
        {
            foreach(var coin in coins)
            {
                if (coin.CheckCollisionWithHero(hero))
                {
                    coins.Remove(coin);
                    coin.PickupCoin();
                    Score.getInstance().AddPoint();
                }
            }
        }

        public void SpawnCoin(Vector2 position)
        {
            Coin coin = new Coin(coinTexture, coinSoundEffect, position);
            coins.Add(coin);
        }
    }
}
