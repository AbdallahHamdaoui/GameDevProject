using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.Enemies
{
    public class EnemyFactory
    {
        private Texture2D basicEnemyTexture;
        private Texture2D fastEnemyTexture;
        private Texture2D trapTexture;

        private SoundEffect enemyDeathSoundEffect;

        public EnemyFactory()
        { }

        public void LoadContent(ContentManager content)
        {
            enemyDeathSoundEffect = content.Load<SoundEffect>("audio\\deathSound");
            basicEnemyTexture = content.Load<Texture2D>("images\\enemyOneRun");
            fastEnemyTexture = content.Load<Texture2D>("images\\enemyTwoRun");
            trapTexture = content.Load<Texture2D>("images\\enemyThreeIdle");
        }

        public Enemy CreateEnemy(string type, Rectangle position, GraphicsDeviceManager graphics)
        {
            if (type.Equals("basic"))
            {
                return new Enemy(basicEnemyTexture, enemyDeathSoundEffect, position, 1, graphics);
            }
            else if (type.Equals("fast"))
            {
                return new Enemy(fastEnemyTexture, enemyDeathSoundEffect, position, 3, graphics);
            }
            else if (type.Equals("trap"))
            {
                return new Enemy(trapTexture, enemyDeathSoundEffect, position, 0, graphics);
            }
            throw new Exception("Could not create Enemy with type: " + type);
        }
    }

}
