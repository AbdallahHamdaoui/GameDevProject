using Inception.NewFolder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Inception
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch UISpriteBatch;

        // Screen
        public static int ScreenWidth;
        public static int ScreenHeight;

        // Tileset
        private TmxMap tmxMap;
        private TileMapManager tileMapManager;
        private Texture2D tilesetTexture;
        private List<Rectangle> colliders;

        // Hero
        private Hero hero;
        private Texture2D heroIdleTexture;
        private Texture2D heroRunTexture;
        private Rectangle heroStartPoint;
        private float heroSpeed = 1f;
        private int heroPoints = 0;
        private Rectangle heroEndPoint;
        private SpriteFont heroHasWon;
        public static bool heroHasLost = false;
        private bool heroHasReached = false;

        // Enemy
        private Enemy enemy1;
        private Enemy enemy2;
        private Enemy enemy3;
        private Texture2D enemyRunTexture;
        private List<Enemy> enemies;
        private List<Rectangle> enemyPathways;
        private SoundEffect enemyDeathSoundEffect;

        // Bullet      
        private Texture2D bulletTexture;
        private SoundEffect bulletSoundEffect;

        // Coin
        private List<Coin> coins;
        private SoundEffect coinSoundEffect;

        // Hitbox
        private Hitbox hitbox;

        // Camera
        //private Camera camera;
        private Camera heroCamera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            hitbox = new Hitbox(_graphics);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();

            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            // Hero
            heroHasLost = false;
            heroHasReached = false;
            hitbox.Load(16, 5);
            hitbox.isEnabled = true;
            heroPoints = 0;
            heroHasWon = Content.Load<SpriteFont>("WinScreen");

            // Coin
            var coin = Content.Load<Texture2D>("images\\coin");
            coinSoundEffect = Content.Load<SoundEffect>("audio\\pointSound");

            // Tileset
            tmxMap = new TmxMap("Content\\background\\backgroundLevelOne.tmx");
            tilesetTexture = Content.Load<Texture2D>("background\\" + tmxMap.Tilesets[0].Name.ToString());           
            int tileWidth = tmxMap.Tilesets[0].TileWidth;
            int tileHeight = tmxMap.Tilesets[0].TileHeight;     
            int tilesetTileWidth = tilesetTexture.Width / tileWidth;
            tileMapManager = new TileMapManager(_spriteBatch, tmxMap, tilesetTexture, tilesetTileWidth, tileWidth, tileHeight);

            // Enemy
            enemyPathways = new List<Rectangle>();

            // Coin
            coins = new List<Coin>();

            // Collisions
            colliders = new List<Rectangle>();

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
                    coins.Add(new Coin(coin, new Vector2((int)obj.X, (int)obj.Y)));
                }
            }

            // Hero
            heroIdleTexture = Content.Load<Texture2D>("images\\heroIdle");
            heroRunTexture = Content.Load<Texture2D>("images\\heroRun");
            bulletSoundEffect = Content.Load<SoundEffect>("audio\\shootSound");
            hero = new Hero(heroIdleTexture, heroRunTexture, bulletSoundEffect, new Vector2(heroStartPoint.X, heroStartPoint.Y));

            // Bullet
            bulletTexture = Content.Load<Texture2D>("images\\bullet");

            // Enenmy
            enemyDeathSoundEffect = Content.Load<SoundEffect>("audio\\deathSound");
            enemyRunTexture = Content.Load<Texture2D>("images\\enemyOneRun");
            enemies = new List<Enemy>();
            enemy1 = new Enemy(enemyRunTexture, enemyPathways[0], 1, _graphics);
            enemies.Add(enemy1);
            enemy2 = new Enemy(enemyRunTexture, enemyPathways[1], 1, _graphics);
            enemies.Add(enemy2);
            enemy3 = new Enemy(enemyRunTexture, enemyPathways[2], 1, _graphics);
            enemies.Add(enemy3);

            // Camera
            //camera = new Camera();
            heroCamera = new Camera(_graphics);
        }

        protected override void UnloadContent()
        {
            hitbox.Unload();
            enemy1.Unload();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            heroCamera.Follow(hero, hitbox, tmxMap);

            if (!heroHasLost && !heroHasReached)
            {
                //camera.Follow(hero.heroRectangle);
                //heroCamera.Follow(hero, tmxMap);
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
                                enemyDeathSoundEffect.Play();
                                heroPoints++;
                                break;
                            }
                        }
                    }
                }

                foreach (var coin in coins.ToArray())
                {
                    if (coin.coinRectangle.Intersects(hero.heroRectangle))
                    {
                        heroPoints++;
                        coinSoundEffect.Play();
                        coins.Remove(coin);
                    }
                }

                hero.Jump();

                foreach (var enemy in enemies)
                {
                    enemy.Update(hero.heroRectangle);
                }

                foreach (var bullet in hero.bulletList)
                {
                    bullet.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            new SpriteBatch(GraphicsDevice);


            //_spriteBatch.Begin(transformMatrix: camera.Matrix);
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate, transformMatrix: heroCamera.Transform);

            tileMapManager.Draw();

            foreach (var coin in coins)
            {
                coin.Draw(_spriteBatch, gameTime);
            }

            if (hero.heroIsFacingLeft)
            {
                hero.Draw(_spriteBatch, SpriteEffects.FlipHorizontally, gameTime);
            }
            else
            {
                hero.Draw(_spriteBatch, SpriteEffects.None, gameTime);
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw(_spriteBatch, gameTime);
            }

            foreach (var bullet in hero.bulletList)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.End();


            UISpriteBatch.Begin();

            UISpriteBatch.DrawString(heroHasWon, $"Points: {heroPoints}", new Vector2(0, 0), Color.White);

            if (heroHasReached)
            {
                UISpriteBatch.DrawString(heroHasWon, "You have won! \n Click R to Restart or click", new Vector2(ScreenWidth / 3, ScreenHeight / 2), Color.White);
            }

            if (heroHasLost && !heroHasReached)
            {
                UISpriteBatch.DrawString(heroHasWon, "Game over! \n Click R to Restart", new Vector2(ScreenWidth / 3, ScreenHeight / 2), Color.White);
            }

            UISpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
