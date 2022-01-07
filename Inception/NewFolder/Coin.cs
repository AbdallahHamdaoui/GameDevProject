﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Coin
    {
        private Texture2D coinTexture;
        private Rectangle coinSourceRectangle;
        public Rectangle coinRectangle;

        public Coin(Texture2D coinTexture, Vector2 coinPosition)
        {
            Random r = new Random();
            int randomNumber = r.Next(0, 3);

            this.coinTexture = coinTexture;
            coinSourceRectangle = new Rectangle(0, 0, 16, 16);
            coinRectangle = new Rectangle((int)coinPosition.X, (int)coinPosition.Y, 16, 16);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(coinTexture, coinRectangle, coinSourceRectangle, Color.White);
        }
    }
}