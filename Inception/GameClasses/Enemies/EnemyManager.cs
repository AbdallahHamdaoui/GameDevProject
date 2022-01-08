using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Enemies
{
    public class EnemyManager
    {
        public List<Enemy> enemies;
        private Texture2D enemyTexture;
        private SoundEffect enemyDeathSoundEffect;

        public EnemyManager()
        {
            enemies = new List<Enemy>();
        }

        public void LoadContent(ContentManager content)
        {
            enemyDeathSoundEffect = content.Load<SoundEffect>("audio\\deathSound");
            enemyTexture = content.Load<Texture2D>("images\\enemyOneRun");
        }

        public void Update(GameTime gameTime, Hero hero)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(hero.heroRectangle);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch, gameTime);
            }
        }

        public bool CheckCollisionWithHero(Hero hero)
        {
            foreach(var enemy in enemies)
            {
                if (enemy.CheckCollisionWithHero(hero))
                {
                    return true;
                }
            }
            return false;
        }

        public void SpawnEnemy(Rectangle position, GraphicsDeviceManager graphics)
        {
            Enemy enemy = new Enemy(enemyTexture, enemyDeathSoundEffect, position, 1, graphics);
            enemies.Add(enemy);
        }
    }
}
