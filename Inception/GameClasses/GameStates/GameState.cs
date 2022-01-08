using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.GameStates
{
    public abstract class GameState : IGameState
    {
        public Game1 Game { get; }
        protected GraphicsDevice _graphicsDevice;
        protected GraphicsDeviceManager _graphicsDeviceManager;
        public GameState(Game1 game, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
        {
            Game = game;
            _graphicsDevice = graphicsDevice;
            _graphicsDeviceManager = graphicsDeviceManager;
        }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
