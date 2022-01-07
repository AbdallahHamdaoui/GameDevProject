using Inception.NewFolder;
using Inception.NewFolder.GameStates;
using Inception.NewFolder.GameStates.Menu_s;
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

        // Screen
        public static int ScreenWidth;
        public static int ScreenHeight;

        //// Enemy
        //private Enemy enemy1;
        //private Enemy enemy2;
        //private Enemy enemy3;
        //private Texture2D enemyRunTexture;
        //private List<Enemy> enemies;
        //private List<Rectangle> enemyPathways;
        //private SoundEffect enemyDeathSoundEffect;

        //// Coin
        //private List<Coin> coins;
        //private SoundEffect coinSoundEffect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //// Coin
            //var coin = Content.Load<Texture2D>("images\\coin");
            //coinSoundEffect = Content.Load<SoundEffect>("audio\\pointSound");

            //// Enemy
            //enemyPathways = new List<Rectangle>();

            //// Coin
            //coins = new List<Coin>();

            //// Enenmy
            //enemyDeathSoundEffect = Content.Load<SoundEffect>("audio\\deathSound");
            //enemyRunTexture = Content.Load<Texture2D>("images\\enemyOneRun");
            //enemies = new List<Enemy>();
            //enemy1 = new Enemy(enemyRunTexture, enemyPathways[0], 1, _graphics);
            //enemies.Add(enemy1);
            //enemy2 = new Enemy(enemyRunTexture, enemyPathways[1], 1, _graphics);
            //enemies.Add(enemy2);
            //enemy3 = new Enemy(enemyRunTexture, enemyPathways[2], 1, _graphics);
            //enemies.Add(enemy3);

            GameStateManager.Instance.SetContent(Content);
            GameState startMenu = new StartMenu(this, GraphicsDevice, _graphics);
            GameStateManager.Instance.AddScreen(startMenu);
        }

        //protected override void UnloadContent()
        //{
        //    hitbox.Unload();
        //    enemy1.Unload();
        //    base.UnloadContent();
        //}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //if (!heroHasLost && !heroHasReached)
            //{
            //    foreach (var coin in coins.ToArray())
            //    {
            //        if (coin.coinRectangle.Intersects(hero.heroRectangle))
            //        {
            //            heroPoints++;
            //            coinSoundEffect.Play();
            //            coins.Remove(coin);
            //        }
            //    }

            //    foreach (var enemy in enemies)
            //    {
            //        enemy.Update(hero.heroRectangle);
            //    }
            //}

            GameStateManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //_spriteBatch.Begin();

            //foreach (var coin in coins)
            //{
            //    coin.Draw(_spriteBatch, gameTime);
            //}

            //foreach (var enemy in enemies)
            //{
            //    enemy.Draw(_spriteBatch, gameTime);
            //}

            //_spriteBatch.End();

            GameStateManager.Instance.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
