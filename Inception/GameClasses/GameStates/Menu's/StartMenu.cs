using Inception.GameClasses.GameStates.Levels;
using Inception.GameClasses.GameStates.MenuComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.GameStates.Menu_s
{
    public class StartMenu : GameState
    {
        private Texture2D _background;
        private List<MenuComponent> _components;
        private SpriteFont font;

        public StartMenu(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
            : base(game, graphicsDevice, graphicsDeviceManager)
        {
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background, new Rectangle(0, 0, 1024, 640), Color.CornflowerBlue);

            //spriteBatch.DrawString(font, "INCEPTION", new Vector2(300, 100), Color.White);
            spriteBatch.DrawString(font, "INCEPTION", new Vector2(350, 100), Color.White, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 1);

            foreach (MenuComponent comp in _components)
            {
                comp.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            //Game.IsMouseVisible = true;
        }

        public override void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("background/menuBackground");
            Texture2D bttnTexture = content.Load<Texture2D>("ui/StoneButton");
            font = content.Load<SpriteFont>("Fonts/Algerian");

            Button startBttn = new Button(bttnTexture, font, "Start Game", new Vector2(350, 300));
            startBttn.Click += StartBttn_Click;

            Button quitBttn = new Button(bttnTexture, font, "Quit Game", new Vector2(350, 450));
            quitBttn.Click += QuitBttn_Click;

            _components = new List<MenuComponent>()
            {
                startBttn,
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

        private void StartBttn_Click(object sender, EventArgs e)
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
