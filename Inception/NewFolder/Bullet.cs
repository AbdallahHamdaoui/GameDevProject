using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Bullet
    {
        private Texture2D bulletTexture;
        private float bulletSpeed;
        public Rectangle bulletRectangle;
        public Vector2 bulletPosition;

        public Bullet(Texture2D bulletTexture, float bulletSpeed, Vector2 bulletPosition)
        {
            this.bulletTexture = bulletTexture;
            this.bulletSpeed = bulletSpeed;
            this.bulletPosition = bulletPosition;
            bulletRectangle = new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, 16, 16);
        }

        public void Update()
        {
            bulletPosition.X += bulletSpeed;
            bulletRectangle.X = (int)bulletPosition.X;
            bulletRectangle.Y = (int)bulletPosition.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, bulletPosition, Color.White);
        }
    }
}
