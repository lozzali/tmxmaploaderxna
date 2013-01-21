using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameTMXObjects;
namespace TMXMapEditorTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Vector2 Position = Vector2.Zero;
        Scroll scroll;
        Camera2d Camera;
        int screenHeight;
        int screenWidth;
        float nSteps;
        float zoom = 1.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Map>("desert");

            scroll = Scroll.SCROLL_RIGHT;
            screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            nSteps = 10.0f;

            Camera = new Camera2d(new Viewport(), map.MaxWidthPx, map.MaxHeightPx, zoom);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        public enum Scroll : int
        {
            SCROLL_RIGHT = 0,
            SCROLL_DOWN,
            SCROLL_LEFT,
            SCROLL_UP
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (map != null)
            {
                //Allow scrolling around the map
                switch (scroll)
                {
                    case Scroll.SCROLL_RIGHT:
                        Camera.Pos+= new Vector2(nSteps, 0);

                        if (Camera.Pos.X >= Camera.RightBarrier - screenWidth/zoom)
                        {                            
                            scroll = Scroll.SCROLL_DOWN;
                        }
                        break;
                    case Scroll.SCROLL_DOWN:
                        Camera.Pos += new Vector2(0, nSteps);
                        if (Camera.Pos.Y >=  Camera.TopBarrier - screenHeight/zoom)
                        {                            
                            scroll = Scroll.SCROLL_LEFT;
                        }
                        break;
                    case Scroll.SCROLL_LEFT:
                        Camera.Pos -= new Vector2(nSteps,0);

                        if (Camera.Pos.X <= Camera.LeftBarrier)
                        {
                            scroll = Scroll.SCROLL_UP;
                        }
                        break;
                    case Scroll.SCROLL_UP:
                        Camera.Pos -= new Vector2(0, nSteps);

                        if (Camera.Pos.Y <= Camera.BottomBarrier)
                        {
                            scroll = Scroll.SCROLL_RIGHT;
                        }
                        break;

                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        Camera.GetTransformation());

            foreach (LayerBase layer in map.LayerBaseList)
            {
                layer.Draw(spriteBatch, map.Orientation);
            }

            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
