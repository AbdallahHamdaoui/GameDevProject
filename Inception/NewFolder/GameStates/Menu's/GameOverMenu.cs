﻿using Inception.NewFolder.GameStates.Levels;
using Inception.NewFolder.GameStates.MenuComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder.GameStates.Menu_s
{
    public class GameOverMenu : GameState
    {
        private Texture2D _background;
        private List<MenuComponent> _components;

        public GameOverMenu(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDevice, graphicsDeviceManager)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background, new Rectangle(0, 0, 1024, 640), Color.CornflowerBlue);

            foreach (MenuComponent comp in _components)
            {
                comp.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            Game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("background/menuBackground");
            Texture2D bttnTexture = content.Load<Texture2D>("ui/StoneButton");
            SpriteFont bttnFont = content.Load<SpriteFont>("Fonts/Algerian");

            Button restartBttn = new Button(bttnTexture, bttnFont, "Restart Game", new Vector2(350, 300));
            restartBttn.Click += RestartBttn_Click;

            Button quitBttn = new Button(bttnTexture, bttnFont, "Quit Game", new Vector2(350, 450));
            quitBttn.Click += QuitBttn_Click;

            _components = new List<MenuComponent>()
            {
                restartBttn,
                quitBttn
            };
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MenuComponent comp in _components)
            {
                comp.Update(gameTime);
            }
        }

        private void RestartBttn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start Game");
            GameState FirstLevel = new LevelOne(Game, _graphicsDevice, _graphicsDeviceManager);
            GameStateManager.Instance.ChangeScreen(FirstLevel);
        }
        private void QuitBttn_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }
    }
}
