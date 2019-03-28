using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Laser laser;
        Laser laser2;
        Texture2D playerTexture;
        Texture2D laserTexture;
        Texture2D laser2Texture;
        Vector2 playerPosition;
        Vector2 laserPosition;
        Vector2 laser2Position;
        Enemy[] enemies = new Enemy[11];
        Texture2D[] enemiesTexture;
        int j = 0;
        Random random = new Random();
        SpriteFont font;
        int score;
        String gameState = "gameMenu";
        Texture2D textureGameOver;
        Texture2D textureGameStart;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Space Invaders";
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
            playerPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight - 100);
            laserPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            laser2Position = laserPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            player = new Player();
            laser = new Laser();

            laser2 = new Laser();
            laser2.Speed = 250f;
            
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new Enemy();
            }
            enemiesTexture = new Texture2D[11];

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

            playerTexture = Content.Load<Texture2D>("player");
            player.Initialize(playerTexture, playerPosition);

            laserTexture = Content.Load<Texture2D>("laser");
            laser.Initialize(laserTexture, laserPosition);

            laser2Texture = Content.Load<Texture2D>("laser");
            laser2.Initialize(laser2Texture, laser2Position);

            for (int i = 0; i < enemies.Length; i++)
            {   
                enemiesTexture[i] = Content.Load<Texture2D>("enemy");
                enemies[i].Initialize(enemiesTexture[i], new Vector2(j, 100));
                j = j + enemiesTexture[i].Width + 10;
            }

            font = Content.Load<SpriteFont>("Score");

            textureGameOver = Content.Load<Texture2D>("gameOver");

            textureGameStart = Content.Load<Texture2D>("gameStart");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            // Animations

            if (gameState == "gameStart")
            {
                // Déplacement du joueur vers la gauche si flèche de gauche appuyée
                if (kstate.IsKeyDown(Keys.Left) && player.Position.X > 0)
                    player.Position.X -= player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Déplacement du joueur vers la droit si flèche de droite appuyée
                if (kstate.IsKeyDown(Keys.Right) && player.Position.X < graphics.PreferredBackBufferWidth - playerTexture.Width)
                    player.Position.X += player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (kstate.IsKeyDown(Keys.Space))
                {
                    if (!laser.Active)
                    {
                        laser.Position.X = player.Position.X + playerTexture.Width / 2;
                        laser.Position.Y = player.Position.Y - 20;
                    }
                    laser.Active = true;
                }

                if (laser.Position.Y < 0)
                    laser.Active = false;

                // Collisions

                for (int i = 0; i < enemies.Length; i++)
                {
                    if (laser.Position.X > enemies[i].Position.X && laser.Position.X <= enemies[i].Position.X + enemiesTexture[i].Width && laser.Position.Y <= enemies[i].Position.Y && enemies[i].Active)
                    {
                        laser.Active = false;
                        laser.Position.X = graphics.PreferredBackBufferWidth;
                        laser.Position.Y = graphics.PreferredBackBufferHeight;
                        enemies[i].Active = false;
                        score = score + 10;
                    }
                }

                if (laser2.Position.Y > graphics.PreferredBackBufferHeight - laser2Texture.Height)
                    laser2.Active = false;

                // Vérification collision entre laser de l'ennemi(laser2) et le joueur
                if (laser2.Position.X >= player.Position.X + playerTexture.Width || laser2.Position.X + laser2Texture.Width <= player.Position.X || laser2.Position.Y >= player.Position.Y + playerTexture.Height || laser2.Position.Y + laser2Texture.Height <= player.Position.Y)
                {

                }
                else
                {
                    laser2.Active = false;
                    gameState = "gameOver";
                }

                // Vérification collision entre laser de l'ennemi(laser2) et laser du joueur(laser)
                if (laser2.Position.X >= laser.Position.X + laserTexture.Width || laser2.Position.X + laser2Texture.Width <= laser.Position.X || laser2.Position.Y >= laser.Position.Y + laserTexture.Height || laser2.Position.Y + laser2Texture.Height <= laser.Position.Y)
                {

                }
                else
                {
                    laser.Active = false;
                    laser2.Active = false;
                }

                // Lancement du tir de l'ennemi
                int r = random.Next(enemies.Length);

                if (!laser2.Active && enemies[r].Active)
                {
                    laser2.Position.Y = enemies[r].Position.Y + enemiesTexture[r].Height + 10;
                    laser2.Position.X = enemies[r].Position.X + enemiesTexture[r].Width / 2;
                    laser2.Active = true;
                }
            }

            if (laser.Active)
                laser.Position.Y -= laser.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (laser2.Active)
                laser2.Position.Y += laser2.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            updateEnemy(gameTime);
            updateOver(gameTime);
            updateMenu(gameTime);
            base.Update(gameTime);
        }

        public void updateEnemy(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[10].Position.X > graphics.PreferredBackBufferWidth - enemiesTexture[10].Width || enemies[0].Position.X < 0)
                {
                    enemies[i].Speed = -enemies[i].Speed;
                }
                enemies[i].Position.X += enemies[i].Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void updateOver(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Enter))
            {
                score = 0;
                gameState = "gameStart";
            }
        }

        public void updateMenu(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Enter))
            {
                gameState = "gameStart";
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (gameState == "gameOver")
            {
                spriteBatch.Draw(textureGameOver, new Vector2((graphics.PreferredBackBufferWidth - textureGameOver.Width) / 2, (graphics.PreferredBackBufferHeight - textureGameOver.Height) / 2), Color.White);
                spriteBatch.DrawString(font, "Score : " + score.ToString(), new Vector2(0, 0), Color.White);
            }
            else if (gameState == "gameStart")
            {
                player.Draw(spriteBatch);

                if (laser.Active)
                    laser.Draw(spriteBatch);
                if (laser2.Active)
                    laser2.Draw(spriteBatch);

                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].Active)
                        enemies[i].Draw(spriteBatch);
                }

                spriteBatch.DrawString(font, "Score : " + score.ToString(), new Vector2(0, 0), Color.White);
            }
            else if (gameState == "gameMenu")
            {
                spriteBatch.Draw(textureGameStart, new Vector2((graphics.PreferredBackBufferWidth - textureGameStart.Width) / 2, (graphics.PreferredBackBufferHeight - textureGameStart.Height) / 2), Color.White);
            }
            else if (gameState == "gameFinish")
            {
                spriteBatch.DrawString(font, "Score : " + score.ToString(), new Vector2(0, 0), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
