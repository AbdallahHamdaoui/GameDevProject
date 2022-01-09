using Inception.GameClasses.Bullet;
using Inception.GameClasses.Coins;
using Inception.GameClasses.Enemies;
using Inception.GameClasses.GameStates.Menu_s;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace Inception.GameClasses.GameStates.Levels
{
    public class LevelTwo : GameState
    {
        // Tileset
        private TmxMap tmxMap;
        private TileMapManager tileMapManager;
        private Texture2D tilesetTexture;
        private List<Rectangle> colliders;

        // Hero
        private Hero hero;
        private Rectangle heroStartPoint;
        private float heroSpeed = 3f;
        private Rectangle heroEndPoint;
        private SpriteFont score;
        public static bool heroHasLost = false;
        private bool heroHasReachedEnd = false;

        // Enemy
        private EnemyManager enemyManager;
        private List<Rectangle> enemyPathways;

        // Bullet      
        private BulletManager bulletManager;

        // Coin
        private CoinManager coinManager;

        // Hitbox
        private Hitbox hitbox;

        // Camera
        private Camera heroCamera;

        public LevelTwo(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDevice, graphicsDeviceManager) 
        {
            hero = new Hero(new Vector2(heroStartPoint.X, heroStartPoint.Y));
            hitbox = new Hitbox(graphicsDeviceManager);
            heroCamera = new Camera(graphicsDeviceManager);
            enemyManager = new EnemyManager();
            colliders = new List<Rectangle>();
            enemyPathways = new List<Rectangle>();
            bulletManager = new BulletManager();
            coinManager = new CoinManager();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate, transformMatrix: heroCamera.Transform);

            //Tileset
            tileMapManager.Draw(spriteBatch);

            //Hero
            if (hero.heroIsFacingLeft)
            {
                hero.Draw(spriteBatch, SpriteEffects.FlipHorizontally, gameTime);
            }
            else
            {
                hero.Draw(spriteBatch, SpriteEffects.None, gameTime);
            }

            //Score
            spriteBatch.DrawString(score, $"Points: {Score.getInstance().points}", new Vector2(0, 0), Color.White);

            //Bullet
            bulletManager.Draw(spriteBatch);

            //Coin
            coinManager.Draw(spriteBatch);

            //Enemy
            enemyManager.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public override void Initialize()
        {
            Game.IsMouseVisible = false;
        }

        public override void LoadContent(ContentManager content)
        {
            // Tileset
            tmxMap = new TmxMap("Content\\background\\backgroundLevelTwo.tmx");
            tilesetTexture = content.Load<Texture2D>("background\\" + tmxMap.Tilesets[0].Name.ToString());
            int tileWidth = tmxMap.Tilesets[0].TileWidth;
            int tileHeight = tmxMap.Tilesets[0].TileHeight;
            int tilesetTileWidth = tilesetTexture.Width / tileWidth;
            tileMapManager = new TileMapManager(tmxMap, tilesetTexture, tilesetTileWidth, tileWidth, tileHeight);

            // Hero
            heroHasLost = false;
            heroHasReachedEnd = false;
            hitbox.Load(16, 5);
            hitbox.isEnabled = true;
            hero.LoadContent(content);

            // Collisions
            foreach (var obj in tmxMap.ObjectGroups["Collisions"].Objects)
            {
                if (obj.Name == "")
                {
                    colliders.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
                else if (obj.Name == "startPoint")
                {
                    heroStartPoint = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
                else if (obj.Name == "endPoint")
                {
                    heroEndPoint = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
                else if (obj.Name == "enemyPathway")
                {
                    enemyPathways.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                }
                else if (obj.Name == "coin")
                {
                    coinManager.LoadContent(content);
                    coinManager.SpawnCoin(new Vector2((int)obj.X, (int)obj.Y));
                }
            }

            // Enemy
            enemyManager.LoadContent(content);
            enemyManager.SpawnEnemy(enemyPathways[0], _graphicsDeviceManager);
            enemyManager.SpawnEnemy(enemyPathways[1], _graphicsDeviceManager);
            enemyManager.SpawnEnemy(enemyPathways[2], _graphicsDeviceManager);

            // Bullet
            bulletManager.LoadContent(content);

            //Score
            score = content.Load<SpriteFont>("WinScreen");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            heroCamera.Follow(hero, hitbox, tmxMap);

            var initialPosition = hero.heroMovement;
            hero.heroIsFalling = true;
            hero.Update(gameTime, heroSpeed);

            if (heroEndPoint.Intersects(hero.heroRectangle))
            {
                heroHasReachedEnd = true;
            }

            foreach (var rectangle in colliders)
            {
                if (hero.heroRectangle.Intersects(rectangle))
                {
                    hero.heroMovement.X = initialPosition.X;
                }

                if (rectangle.Intersects(hero.heroJumpPoint))
                {
                    hero.heroIsFalling = !rectangle.Intersects(hero.heroJumpPoint);
                }

                coinManager.CheckCollisionWithHero(hero);
            }

            hero.Jump();

            bulletManager.Update(gameTime, hero);
            bulletManager.CheckCollision(colliders);
            bulletManager.CheckEnemyCollision(enemyManager.enemies);

            enemyManager.Update(gameTime, hero);

            if (enemyManager.CheckCollisionWithHero(hero))
            {
                heroHasLost = true;
            }

            CheckHeroDeath();
            CheckHeroReachedEndOfLevel();
        }

        private void CheckHeroDeath()
        {
            if (heroHasLost)
            {
                Score.getInstance().ResetPoints();
                GameState gameOverMenu = new GameOverMenu(Game, _graphicsDevice, _graphicsDeviceManager);
                GameStateManager.Instance.AddScreen(gameOverMenu);
            }
        }
        private void CheckHeroReachedEndOfLevel()
        {
            if (heroHasReachedEnd)
            {
                GameState victoryMenu = new VictoryMenu(Game, _graphicsDevice, _graphicsDeviceManager);
                GameStateManager.Instance.ChangeScreen(victoryMenu);
            }
        }

    }
}
