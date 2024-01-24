using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Final_Monogame_Assignment
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D duckTexture;
        Rectangle duckRect;
        Vector2 duckSpeed;
        MouseState mouseState = Mouse.GetState();
        MouseState oldState;
        enum Screen
        {
            Intro,
            RubberDuck
        }
        Screen screen;
        Texture2D duckIntroTexture;
        Texture2D scopeTexture;
        Rectangle scopeLocation;
        Texture2D fieldTexture;
        SpriteFont font;
        SoundEffectInstance hunting;
        SoundEffectInstance blaster;
        Random generator;
        int score;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            duckRect = new Rectangle(300, 10, 100, 100);
            duckSpeed = new Vector2(6, 5);
            scopeLocation = new Rectangle(10, 10, 75, 75);
            score = 0;
            generator = new Random();
            screen = Screen.Intro;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            duckTexture = Content.Load<Texture2D>("rubber-duck2");
            duckIntroTexture = Content.Load<Texture2D>("Duck_Hunt");
            scopeTexture = Content.Load<Texture2D>("reticle2");
            fieldTexture = Content.Load<Texture2D>("duck-hunt-11");
            hunting = Content.Load<SoundEffect>("duckhunt").CreateInstance();
            blaster = Content.Load<SoundEffect>("zapper").CreateInstance();
            font = Content.Load<SpriteFont>("Score");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            oldState = mouseState;
            MouseState newState = Mouse.GetState();
            mouseState = Mouse.GetState();



            if (screen == Screen.Intro)
            {
                hunting.Play();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    screen = Screen.RubberDuck;
                    IsMouseVisible = false;
                }
            }
            else if (screen == Screen.RubberDuck)
            {
                hunting.Stop();
                scopeLocation.X = mouseState.X;
                scopeLocation.Y = mouseState.Y;

                duckRect.X += (int)duckSpeed.X;
                duckRect.Y += (int)duckSpeed.Y;
                if (duckRect.Right > _graphics.PreferredBackBufferWidth || duckRect.Left < 0)
                {
                    duckSpeed.X *= -1;
                }
                if (duckRect.Bottom > _graphics.PreferredBackBufferHeight || duckRect.Top < 0)
                {
                    duckSpeed.Y *= -1;
                }


                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    blaster.Play();

                    if (duckRect.Contains(mouseState.Position))
                    {
                        score++;
                        duckRect.Location = new Point(generator.Next(0,300),generator.Next(20,300));
                        if (duckSpeed.X > 0)
                        {
                            duckSpeed.X += 0.5f;
                        }
                        else
                        {
                            duckSpeed.X -= 0.5f;
                        }
                        if (duckSpeed.Y > 0)
                        {
                            duckSpeed.Y += 0.5f;
                        }
                        else
                        {
                            duckSpeed.Y -= 0.5f;
                        }
                    }

                }




            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            _spriteBatch.Begin();
            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(duckIntroTexture, new Rectangle(0, 0, 800, 500), Color.White);
            }
            else if (screen == Screen.RubberDuck)
            {
                _spriteBatch.Draw(fieldTexture, new Rectangle(0, 0, 800, 500), Color.White);
                _spriteBatch.Draw(duckTexture, duckRect, Color.White);
                _spriteBatch.Draw(scopeTexture, scopeLocation, Color.White);
                _spriteBatch.DrawString(font, "Score: " + score, new Vector2(700, 50), Color.Black);
            }
            _spriteBatch.End();
        }
    }
}