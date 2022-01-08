using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace Inception.NewFolder.GameStates.Levels
{
    public class LevelOne : GameState
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
        private int heroPoints = 0;
        private Rectangle heroEndPoint;
        private SpriteFont heroHasWon;
        public static bool heroHasLost = false;
        private bool heroHasReached = false;

        // Enemy
        private Enemy enemy1;
        private Enemy enemy2;
        private Enemy enemy3;
        private List<Enemy> enemies;
        private List<Rectangle> enemyPathways;
        private SoundEffect enemyDeathSoundEffect;

        // Bullet      
        private Texture2D bulletTexture;

        // Coin
        private List<Coin> coins;

        // Hitbox
        private Hitbox hitbox;

        // Camera
        private Camera heroCamera;

        public LevelOne(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDevice, graphicsDeviceManager)
        {
            hero = new Hero(new Vector2(heroStartPoint.X, heroStartPoint.Y));
            hitbox = new Hitbox(graphicsDeviceManager);
            heroCamera = new Camera(graphicsDeviceManager);
            coins = new List<Coin>();
            enemies = new List<Enemy>();
            colliders = new List<Rectangle>();
            enemyPathways = new List<Rectangle>();
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

            //Bullet
            foreach (var bullet in hero.bulletList)
            {
                bullet.Draw(spriteBatch);
            }

            //Coin
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch, gameTime);
            }

            //Enemy
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            Game.IsMouseVisible = false;
        }

        public override void LoadContent(ContentManager content)
        {
            // Tileset
            tmxMap = new TmxMap("Content\\background\\backgroundLevelOne.tmx");
            tilesetTexture = content.Load<Texture2D>("background\\" + tmxMap.Tilesets[0].Name.ToString());
            int tileWidth = tmxMap.Tilesets[0].TileWidth;
            int tileHeight = tmxMap.Tilesets[0].TileHeight;
            int tilesetTileWidth = tilesetTexture.Width / tileWidth;
            tileMapManager = new TileMapManager(tmxMap, tilesetTexture, tilesetTileWidth, tileWidth, tileHeight);

            // Hero
            heroHasLost = false;
            heroHasReached = false;
            hitbox.Load(16, 5);
            hitbox.isEnabled = true;
            heroPoints = 0;
            hero.LoadContent(content);
            //heroHasWon = Content.Load<SpriteFont>("WinScreen");

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
                    Coin coin = new Coin(new Vector2((int)obj.X, (int)obj.Y));
                    coin.LoadContent(content);
                    coins.Add(coin);
                }
            }
            // Enemy
            
            //enemyDeathSoundEffect = content.Load<SoundEffect>("audio\\deathSound");

            enemy1 = new Enemy(enemyPathways[0], 1, _graphicsDeviceManager);
            enemy1.LoadContent(content);
            enemies.Add(enemy1);
            enemy2 = new Enemy(enemyPathways[1], 1, _graphicsDeviceManager);
            enemy2.LoadContent(content);
            enemies.Add(enemy2);
            enemy3 = new Enemy(enemyPathways[2], 1, _graphicsDeviceManager);
            enemy3.LoadContent(content);
            enemies.Add(enemy3);

            // Bullet
            bulletTexture = content.Load<Texture2D>("images\\bullet");            
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            heroCamera.Follow(hero, hitbox, tmxMap);

            if (!heroHasLost && !heroHasReached)
            {
                var initialPosition = hero.heroMovement;
                hero.heroIsFalling = true;
                hero.Update(gameTime, bulletTexture, heroSpeed);

                if (heroEndPoint.Intersects(hero.heroRectangle))
                {
                    heroHasReached = true;
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

                    foreach (var bullet in hero.bulletList.ToArray())
                    {
                        if (bullet.bulletRectangle.Intersects(rectangle))
                        {
                            hero.bulletList.Remove(bullet);
                            break;
                        }

                        foreach (var enemy in enemies.ToArray())
                        {
                            if (enemy.enemyRectangle.Intersects(bullet.bulletRectangle))
                            {
                                enemies.Remove(enemy);
                                hero.bulletList.Remove(bullet);
                                //enemyDeathSoundEffect.Play();
                                heroPoints++;
                                break;
                            }
                        }
                    }

                    foreach (var coin in coins.ToArray())
                    {
                        if (coin.coinRectangle.Intersects(hero.heroRectangle))
                        {
                            heroPoints++;
                            coin.PlaySound();
                            coins.Remove(coin);
                        }
                    }
                }

                hero.Jump();

                foreach (var bullet in hero.bulletList)
                {
                    bullet.Update();
                }

                foreach (var enemy in enemies)
                {
                    enemy.Update(hero.heroRectangle);
                }
            }
        }
    }
}
