using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace perlin
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private static int xRes = 800;
        private static int yRes = 480;
        private SpriteBatch spriteBatch;
        private Texture2D pixel;
        private float[,] noiseValues;
        private MouseState oldState;
        private bool noiseChanged = true;
        private int xStart = 0, yStart = 0;
        private int xEnd = xRes, yEnd = yRes;

        //start at a small offset to display zero point issue
        private int xOffset = -160, yOffset = -96;

        //start from non-zero:
        //private int xOffset = int.MaxValue /1000, yOffset = int.MaxValue /1000;

        //starting at a very high index seems to lower noise generation quality/resolution a lot (!)
        //private int xOffset = int.MaxValue /2, yOffset = int.MaxValue /2;
        
        //note: at scales over 5.0f, repeating patterns become very obvious!
        private float scale = 0.005f;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            this.Window.Title = "SimplexNoiseTest";

            noiseValues = new float[xRes, yRes];

            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //generate a pixel as texture source
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            //handle dragging
            if (mouseState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Pressed)
            {
                int x = oldState.X - mouseState.X;
                int y = oldState.Y - mouseState.Y;
                xOffset += x;
                yOffset += y;
                noiseChanged = true;
            }

            //handle zooming
            if (mouseState.ScrollWheelValue != oldState.ScrollWheelValue)
            {
                if (mouseState.ScrollWheelValue < oldState.ScrollWheelValue)
                {
                    scale = scale * 2;
                }
                else
                {
                    scale = scale / 2;
                }
                noiseChanged = true;
            }



            oldState = mouseState;

            //generate new noise because we dragged or zoomed
            if (noiseChanged)
            {

                var watch = System.Diagnostics.Stopwatch.StartNew();
                SimplexNoise.Noise.Seed = 1337;
                for (int x = xStart; x < xEnd; x++)
                {
                    for (int y = yStart; y < yEnd; y++)
                    {
                        noiseValues[x, y] = SimplexNoise.Noise.CalcPixel2D(x + xOffset, y + yOffset, scale);
                    }
                }
                watch.Stop();

                //update window title
                this.Window.Title = $"SimplexNoiseTest - Debug: [{xStart + xOffset}, {yStart + yOffset}], [{xEnd + xOffset}, {yEnd + yOffset}] - Scale: {scale} - Seed: {SimplexNoise.Noise.Seed} - GenTime: {watch.ElapsedMilliseconds}";
                noiseChanged = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //draw noise
            int res = 1;
            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    var c = (int)Math.Round(noiseValues[x, y]);
                    spriteBatch.Draw(pixel, new Rectangle((x * res), (y * res), res, res), new Color(c, c, c));
                }
            }

            //draw zero point
            spriteBatch.Draw(pixel, new Rectangle((0 - xOffset) - 1, (0 - yOffset) - 1, 3, 3), Color.Red * 0.8f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
