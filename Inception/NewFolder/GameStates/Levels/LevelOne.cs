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
        private Texture2D heroIdleTexture;
        private Texture2D heroRunTexture;
        private Rectangle heroStartPoint;
        private float heroSpeed = 1f;
        private int heroPoints = 0;
        private Rectangle heroEndPoint;
        private SpriteFont heroHasWon;
        public static bool heroHasLost = false;
        private bool heroHasReached = false;

        // Bullet      
        private Texture2D bulletTexture;
        private SoundEffect bulletSoundEffect;

        // Hitbox
        private Hitbox hitbox;

        public LevelOne(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDevice, graphicsDeviceManager)
        {
            hitbox = new Hitbox(graphicsDeviceManager);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            tileMapManager.Draw(spriteBatch);

            if (hero.heroIsFacingLeft)
            {
                hero.Draw(spriteBatch, SpriteEffects.FlipHorizontally, gameTime);
            }
            else
            {
                hero.Draw(spriteBatch, SpriteEffects.None, gameTime);
            }

            foreach (var bullet in hero.bulletList)
            {
                bullet.Draw(spriteBatch);
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
            //heroHasWon = Content.Load<SpriteFont>("WinScreen");

            heroIdleTexture = content.Load<Texture2D>("images\\heroIdle");
            heroRunTexture = content.Load<Texture2D>("images\\heroRun");
            bulletSoundEffect = content.Load<SoundEffect>("audio\\shootSound");
            hero = new Hero(heroIdleTexture, heroRunTexture, bulletSoundEffect, new Vector2(heroStartPoint.X, heroStartPoint.Y));

            // Bullet
            bulletTexture = content.Load<Texture2D>("images\\bullet");

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
                //    else if (obj.Name == "enemyPathway")
                //    {
                //        enemyPathways.Add(new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height));
                //    }
                //    else if (obj.Name == "coin")
                //    {
                //        coins.Add(new Coin(coin, new Vector2((int)obj.X, (int)obj.Y)));
                //    }
            }
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
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

                        //foreach (var enemy in enemies.ToArray())
                        //{
                        //    if (enemy.enemyRectangle.Intersects(bullet.bulletRectangle))
                        //    {
                        //        enemies.Remove(enemy);
                        //        hero.bulletList.Remove(bullet);
                        //        enemyDeathSoundEffect.Play();
                        //        heroPoints++;
                        //        break;
                        //    }
                        //}
                    }
                }

                hero.Jump();

                foreach (var bullet in hero.bulletList)
                {
                    bullet.Update();
                }
            }
        }
    }
}
