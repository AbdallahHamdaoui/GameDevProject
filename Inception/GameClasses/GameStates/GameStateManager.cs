using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inception.GameClasses.GameStates
{
    public class GameStateManager
    {
        //instance of the game state manager
        private static GameStateManager _instance;
        //Stack of the screens
        private Stack<GameState> _screens = new Stack<GameState>();
        private ContentManager _content;

        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }

        public void SetContent(ContentManager content)
        {
            _content = content;
        }

        public void AddScreen(GameState screen)
        {
            try
            {
                //Add screen to the stack
                _screens.Push(screen);

                _screens.Peek().Initialize();

                if (_content != null)
                {
                    _screens.Peek().LoadContent(_content);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Removes the top screen from the stack
        public void RemoveScreen()
        {
            if (_screens.Count > 0)
            {
                try
                {
                    var screen = _screens.Peek();
                    _screens.Pop();
                }
                catch (Exception e)
                {
                    //throw new Exception(e.Message);
                }
            }
        }

        //Clears all the screens from the list
        public void ClearScreens()
        {
            while (_screens.Count > 0)
            {
                _screens.Pop();
            }
        }

        //Removes all screens from the stack and adds a new one
        public void ChangeScreen(GameState screen)
        {
            try
            {
                ClearScreens();
                AddScreen(screen);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //Updates the top screen
        public void Update(GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime);
                }
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
            }
        }

        //Renders the top screen
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Draw(spriteBatch, gameTime);
                }
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
            }
        }

        public void UnloadContent()
        {
            foreach (GameState state in _screens)
            {
                state.UnloadContent();
            }
        }
    }
}
