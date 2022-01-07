using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.NewFolder
{
    public class Hitbox
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public Texture2D hitbox;
        private Color[] colors; // data
        public bool isEnabled;
        
        public Hitbox(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

        public void Load(int width, int height)
        {
            hitbox = new Texture2D(graphicsDeviceManager.GraphicsDevice, width, height);
            colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
            }

            hitbox.SetData(colors);
        }

        public void Unload()
        {
            hitbox.Dispose();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (isEnabled)
            {
                spriteBatch.Draw(hitbox, position, Color.White);
            }
        }
    }
}
