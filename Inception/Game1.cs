using Inception.GameClasses;
using Inception.GameClasses.GameStates;
using Inception.GameClasses.GameStates.Menu_s;
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

            GameStateManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            GameStateManager.Instance.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
