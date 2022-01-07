using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace Inception.NewFolder
{
    public class Camera
    {
        public Vector2 CameraPos { get; set; }
        public Matrix Transform { get; private set; }
        public GraphicsDeviceManager Graphics { get; set; }

        private float _positionX;
        private float _positionY;
        private float _horizontalOffset;
        private float _verticalOffset;
        private float _zoomScale = 1f;

        public Camera(GraphicsDeviceManager graphics)
        {
            Graphics = graphics;
            _horizontalOffset = (Graphics.PreferredBackBufferWidth / 2 - 300);
            _verticalOffset = (Graphics.PreferredBackBufferHeight / 2);
        }

        public void Follow(Hero target, Hitbox hbox, TmxMap tmxMap)
        {
            _positionX = target.heroMovement.X - (hbox.hitbox.Width / 2)
                            - _horizontalOffset / _zoomScale;
            _positionY = target.heroMovement.Y - (hbox.hitbox.Height / 2)
                            - _verticalOffset / _zoomScale;

            ClampCamera(tmxMap);

            CameraPos = new Vector2(_positionX, _positionY);
            Transform = Matrix.CreateTranslation(new Vector3(-CameraPos, 0)) * Matrix.CreateScale(_zoomScale);
        }

        private void ClampCamera(TmxMap tmxMap)
        {
            int mapWidth = tmxMap.Width * tmxMap.TileWidth;
            int mapHeight = tmxMap.Height * tmxMap.TileHeight;

            if (_positionX < 0)
                _positionX = 0;
            if (_positionX > mapWidth - (int)(_horizontalOffset * 2) - 600)
                _positionX = mapWidth - (int)(_horizontalOffset * 2) - 600;
            if (_positionY < 0)
                _positionY = 0;
            if (_positionY > mapHeight - (int)(_verticalOffset * 2) - 385)
                _positionY = mapHeight - (int)(_verticalOffset * 2) - 385;
        }
    }
}
