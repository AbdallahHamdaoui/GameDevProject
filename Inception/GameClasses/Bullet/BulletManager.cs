using Inception.GameClasses.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Bullet
{
    public class BulletManager
    {
        private List<Bullet> bulletList;
        private Texture2D bulletTexture;
        private SoundEffect bulletSoundEffect;
        private float timeBetweenBullets = 2;

        public BulletManager()
        {
            bulletList = new List<Bullet>();
        }

        public void LoadContent(ContentManager content)
        {
            bulletTexture = content.Load<Texture2D>("images\\bullet");
            bulletSoundEffect = content.Load<SoundEffect>("audio\\shootSound");
        }

        public void Update(GameTime gameTime, Hero hero)
        {
            foreach (var bullet in bulletList)
            {
                bullet.Update();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && timeBetweenBullets > 1)
            {
                Shoot(hero);
            }
            else
            {
                timeBetweenBullets += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void Shoot(Hero hero)
        {
            bulletSoundEffect.Play();

            if (hero.heroIsFacingLeft)
            {
                bulletList.Add(new Bullet(bulletTexture, -2, new Vector2(hero.heroMovement.X, hero.heroMovement.Y + 15)));
            }
            else
            {
                bulletList.Add(new Bullet(bulletTexture, 2, new Vector2(hero.heroMovement.X + 16, hero.heroMovement.Y + 15)));
            }
            timeBetweenBullets = 0;
        }

        public void CheckCollision(List<Rectangle> colliders)
        {
            foreach (var bullet in bulletList)
            {
                if (bullet.CheckCollision(colliders))
                {
                    bulletList.Remove(bullet);
                }
            }
        }

        public bool CheckEnemyCollision(List<Enemy> enemies)
        {
            foreach (var bullet in bulletList)
            {
                Enemy enemy = bullet.CheckEnemyCollision(enemies);
                if (enemy != null)
                {
                    bulletList.Remove(bullet);
                    enemies.Remove(enemy);
                    enemy.Dies();
                    return true;
                }
            }
            return false;
        }
    }
}
