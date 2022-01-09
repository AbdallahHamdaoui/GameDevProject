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
        private EnemyFactory enemyFactory;

        public EnemyManager()
        {
            enemies = new List<Enemy>();
            enemyFactory = new EnemyFactory();
        }

        public void LoadContent(ContentManager content)
        {
            enemyFactory.LoadContent(content);
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
            foreach (var enemy in enemies)
            {
                if (enemy.CheckCollisionWithHero(hero))
                {
                    return true;
                }
            }
            return false;
        }

        public void SpawnEnemy(string type, Rectangle position, GraphicsDeviceManager graphics)
        {
            enemies.Add(enemyFactory.CreateEnemy(type, position, graphics));
        }
    }
}
