using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WumpusXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class GUI : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameControl gControl;
        Diver diver;
        Wumpus _wumpus;
        Ghost[] ghosts;
        Door[] doors;
        Coin[] availableCoins;
        Vector2[] cornerPositions;
        public int wumpusLocation;
        private int numGhosts = 3;
        public float timer = 0;
        private int timeCounter = 0;
        private int timeLimit = 5;
        KeyboardState keyboard;
        int score;
        int arrows;
        int coins;
        int turns;
        int roomNumber;
        int prevDoor;
        const int MOVE_UP = 1;
        const int MOVE_DOWN = -1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        Button btnStartGame;
        Button btnHighScores;

        enum State
        {
            TitleScreen,
            Game,
            Facebook,
            Pause,
            Minigame
        }

        State mCurrentState = State.Game;

        public GUI()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            gControl = new GameControl(this);
            ghosts = new Ghost[numGhosts];
            for (int i = 0; i < numGhosts; i++)
            {
                ghosts[i] = new Ghost();
            }
            doors = new Door[6];
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door();
            }
            cornerPositions = new Vector2[6];
            setCornerPositions();
            score = 0;
            arrows = 0;
            coins = 0;
            turns = 0;
            roomNumber = 0;
            prevDoor = -1;

            mCurrentState = State.Game; //try playing around with this, swap it between Game and Minigame. I'll remove this eventually, but this is the key to how we use GameStates.
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

            diver = new Diver();
            _wumpus = new Wumpus();
            diver.LoadContent(this.Content);
            _wumpus.LoadContent(this.Content);
            Random r = new Random();
            int positions = r.Next(6);
            for (int i = 0; i < ghosts.Length; i++)
            {
                Vector2 ghostPosition = cornerPositions[(positions*i)%6];
                ghosts[i].Initialize(Content.Load<Texture2D>("ghost"), ghostPosition);
            }

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Initialize(Content.Load<Texture2D>("door"), new Vector2((cornerPositions[i].X + cornerPositions[(i + 1) % cornerPositions.Length].X) / 2,
                    (cornerPositions[i].Y + cornerPositions[(i + 1) % cornerPositions.Length].Y) / 2));

                int yOffset = -1;
                if (i == 0 || i == 1 || i == 5)
                {
                    yOffset = 1;
                }

                int xOffset = 0;
                if (i == 1 || i == 2)
                {
                    xOffset = -1;
                }
                else if (i == 4 || i == 5)
                {
                    xOffset = 1;
                }

                doors[i].setPosition(new Vector2((cornerPositions[i].X + cornerPositions[(i + 1) % cornerPositions.Length].X - doors[i].Width) / 2 + (doors[i].Width + 5) / 2 * xOffset,
                    (cornerPositions[i].Y + cornerPositions[(i + 1) % cornerPositions.Length].Y - doors[i].Height) / 2 + (doors[i].Height + 5) / 2 * yOffset));

                doors[i].setVisible();
            }
            gControl.initializeFirstRoom();

            btnStartGame = new Button();
            btnHighScores = new Button();
            btnStartGame.Initialize(Content.Load<Texture2D>("button"), new Vector2(500,500));
            btnHighScores.Initialize(Content.Load<Texture2D>("button"), new Vector2(700, 500));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        /// 

        private Vector2 positionCheck;
        protected override void Update(GameTime gameTime)
        {
            
            if(mCurrentState == State.TitleScreen)
            {
                btnStartGame.setVisible();
                btnHighScores.setVisible();
                spriteBatch.Begin();
                btnStartGame.Draw(spriteBatch);
                btnHighScores.Draw(spriteBatch);
                if(updateButtonCollision() == 1)
                {
                    mCurrentState = State.Game;
                }
                spriteBatch.End();
                if(updateButtonCollision() == 2)
                {
                    gControl.highScoresRequest();
                }
            }
            if (mCurrentState == State.Minigame)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeCounter += (int)timer;
                if (timer >= 1.0F)
                {
                    timer = 0F;
                }
                if (!updateGhostCollision() && timeCounter < timeLimit)
                {
                    for (int i = 0; i < ghosts.Length; i++)
                    {
                        ghosts[i].moveTowards(diver.Position);
                    }
                    if (isWithinHexagon(diver.Position, diver.Size.Width, diver.Size.Height))
                    {
                        positionCheck = diver.Position;
                        diver.Update(gameTime);
                    }
                    else
                    {
                        diver.Position = positionCheck;
                    }
                }
                else
                {
                    gControl.minigame(!updateGhostCollision());
                    timeCounter = 0;
                    Random gen = new Random();
                    int position = gen.Next(6);
                    for (int i = 0; i < ghosts.Length; i++)
                    {
                        Vector2 ghostPosition = cornerPositions[(position * i)%6];
                        ghosts[i].Initialize(Content.Load<Texture2D>("ghost"), ghostPosition);
                    }
                    mCurrentState = State.Game;
                }
            }
            if (mCurrentState == State.Game)
            {
                keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Escape))
                    mCurrentState = State.Pause;
                if (isWithinHexagon(diver.Position, diver.Size.Width, diver.Size.Height))
                {
                    positionCheck = diver.Position;
                    diver.Update(gameTime);
                }
                else
                {
                    diver.Position = positionCheck;
                }


                arrows = gControl.numArrows();
                int newNumArrows = diver.UpdateHarpoon(gameTime, keyboard, arrows);
                if (newNumArrows < arrows)
                {
                    gControl.arrowsShot();
                    
                }
                updateHarpoonCollision();

                _wumpus.moveTowards(diver.Position);
                _wumpus.Update(gameTime);
                updateWumpusCollision();

                base.Update(gameTime);
                spriteBatch.Begin();
                foreach (Door d in doors)
                {
                    d.Draw(spriteBatch);
                    //btnStartGame.setVisible();
                    //btnStartGame.Draw(spriteBatch);
                }
                spriteBatch.End();

                if (availableCoins != null)
                {
                    updateCoinCollision();
                }

                int doorNumber = updateDoorCollision();
                if (doorNumber >= 0 && doorNumber <= 5 && doorNumber != prevDoor && prevDoor != (doorNumber + 3) % 6)
                {
                    gControl.moveDirection(doorNumber);
                }
                prevDoor = doorNumber;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(Content.Load<Texture2D>("background"), new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            drawStats();
            diver.Draw(spriteBatch);

            if (mCurrentState == State.Minigame)
            {
                _wumpus.setInvisible();
                foreach (Door d in doors)
                {
                    d.setInvisible();
                }
                availableCoins = null;
                foreach (Ghost g in ghosts)
                {
                    g.Draw(spriteBatch);
                }
            }
            if (doors != null)
            {
                foreach (Door d in doors)
                {
                    d.Draw(spriteBatch);
                }
            }
            for (int i = 0; i < cornerPositions.Length; i++)
            {
                drawWall(cornerPositions[i], cornerPositions[(i + 1) % cornerPositions.Length]);
            }

            if (availableCoins != null)
            {
                foreach (Coin c in availableCoins)
                {
                    c.Draw(spriteBatch);
                }
            }

            _wumpus.Draw(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }
        private void drawWall(Vector2 p1, Vector2 p2)
        {
            int wallThickness = 20;
            float angle = (float)Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            float dist = Vector2.Distance(p1, p2);
            spriteBatch.Draw(Content.Load<Texture2D>("wood"), new Rectangle((int)p2.X, (int)p2.Y, (int)dist, wallThickness), null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
        private void drawStats()
        {
            for (int i = 0; i < cornerPositions.Length; i++)
            {
                spriteBatch.DrawString(Content.Load<SpriteFont>("WumpusFont"), "Room Number: " + roomNumber.ToString(), Vector2.Zero, Color.White);
                spriteBatch.DrawString(Content.Load<SpriteFont>("WumpusFont"), "Score: " + score.ToString(), new Vector2(0, 50), Color.White);
                spriteBatch.DrawString(Content.Load<SpriteFont>("WumpusFont"), "Coins: " + coins.ToString(), new Vector2(0, 100), Color.White);
                spriteBatch.DrawString(Content.Load<SpriteFont>("WumpusFont"), "Arrows: " + arrows.ToString(), new Vector2(0, 150), Color.White);
                spriteBatch.DrawString(Content.Load<SpriteFont>("WumpusFont"), "Moves: " + turns.ToString(), new Vector2(0, 200), Color.White);
            }
        }

        public bool isWithinHexagon(Vector2 Position, int width, int height)
        {
            bool within = true;
            if (Position.Y < cornerPositions[0].Y)
            {
                within = false;
            }
            else if (Position.Y + height > cornerPositions[3].Y)
            {
                within = false;
            }
            else if (cornerPositions[1].Y - Position.Y > ((cornerPositions[1].Y - cornerPositions[2].Y) / (cornerPositions[1].X - cornerPositions[2].X)) * (cornerPositions[1].X - (Position.X + width)))
            {
                within = false;
            }
            else if (cornerPositions[0].Y - Position.Y > ((cornerPositions[0].Y - cornerPositions[5].Y) / (cornerPositions[0].X - cornerPositions[5].X)) * (cornerPositions[0].X - Position.X))
            {
                within = false;
            }
            else if (cornerPositions[2].Y - (Position.Y + height) < ((cornerPositions[2].Y - cornerPositions[3].Y) / (cornerPositions[2].X - cornerPositions[3].X)) * (cornerPositions[2].X - (Position.X + width)))
            {
                within = false;
            }
            else if (cornerPositions[4].Y - (Position.Y + height) < ((cornerPositions[4].Y - cornerPositions[5].Y) / (cornerPositions[4].X - cornerPositions[5].X)) * (cornerPositions[4].X - Position.X))
            {
                within = false;
            }
            return within;
        }

        private int updateDoorCollision()
        {
            Rectangle diverRect;
            Rectangle doorRect;
            diverRect = new Rectangle((int)diver.Position.X, (int)diver.Position.Y, diver.Size.Width, diver.Size.Height);

            for (int i = 0; i < doors.Length; i++)
            {
                if (!doors[i].Visible)
                {
                    continue;
                }
                doorRect = new Rectangle((int)doors[i].Position.X, (int)doors[i].Position.Y, doors[i].Width, doors[i].Height);

                if (diverRect.Intersects(doorRect))
                {
                    return i;
                }
            }
            return -1;
        }
        private void updateCoinCollision()
        {
            Rectangle diverRect;
            Rectangle coinRect;
            diverRect = new Rectangle((int)diver.Position.X, (int)diver.Position.Y, diver.Size.Width, diver.Size.Height);

            for (int i = 0; i < availableCoins.Length; i++)
            {
                if (!availableCoins[i].Visible)
                {
                    continue;
                }
                coinRect = new Rectangle((int)availableCoins[i].Position.X, (int)availableCoins[i].Position.Y, availableCoins[i].Width, availableCoins[i].Height);

                if (diverRect.Intersects(coinRect))
                {
                    gControl.addACoin();
                    availableCoins[i].setInvisible();
                }
            }
        }

        private void updateHarpoonCollision()
        {
            if (_wumpus.Visible)
            {
                Rectangle harpoonRect;
                Rectangle wumpusRect;
                wumpusRect = new Rectangle((int)_wumpus.Position.X, (int)_wumpus.Position.Y, _wumpus.Size.Width, _wumpus.Size.Height);
                for(int i = 0; i < diver.mHarpoons.Count; i++)
                {
                    if(diver.mHarpoons[i].Visible)
                    {
                        harpoonRect = new Rectangle((int)diver.mHarpoons[i].Position.X, (int)diver.mHarpoons[i].Position.Y, diver.mHarpoons[i].Size.Width, diver.mHarpoons[i].Size.Height);
                        if(wumpusRect.Intersects(harpoonRect))
                        {
                            gControl.endGame();
                        }
                    }
                }
            }
        }

        private void updateWumpusCollision()
        {
            if(_wumpus.Visible)
            {
                Rectangle diverRect;
                Rectangle wumpusRect;
                diverRect = new Rectangle((int)diver.Position.X, (int)diver.Position.Y, diver.Size.Width, diver.Size.Height);
                wumpusRect = new Rectangle((int)_wumpus.Position.X, (int)_wumpus.Position.Y, _wumpus.Size.Width, _wumpus.Size.Height);
                if(wumpusRect.Intersects(diverRect))
                {
                    _wumpus.setInvisible();
                    mCurrentState = State.Minigame;
                    
                }
            }
            
        }

        private Boolean updateGhostCollision()
        {
            Boolean[] collisions = new Boolean[ghosts.Length];
            Rectangle diverRect;
            Rectangle[] ghostRects = new Rectangle[numGhosts];
            diverRect = new Rectangle((int)diver.Position.X, (int)diver.Position.Y, diver.Size.Width, diver.Size.Height);
            for (int i = 0; i < numGhosts; i++)
            {
                ghostRects[i] = new Rectangle((int)ghosts[i].Position.X, (int)ghosts[i].Position.Y, ghosts[i].Width, ghosts[i].Height);
            }
            for (int i = 0; i < collisions.Length; i++)
            {
                collisions[i] = diverRect.Intersects(ghostRects[i]);
            }
            return collisions.Contains(true);
        }

        private int updateButtonCollision()
        {
            Rectangle diverRect;
            Rectangle buttonRect;
            diverRect = new Rectangle((int)diver.Position.X, (int)diver.Position.Y, diver.Size.Width, diver.Size.Height);
            buttonRect = new Rectangle((int)btnStartGame.Position.X, (int)btnStartGame.Position.Y, btnStartGame.Width, btnStartGame.Height);
            if(buttonRect.Intersects(diverRect))
            {
                return 1;
            }
            buttonRect = new Rectangle((int)btnHighScores.Position.X, (int)btnHighScores.Position.Y, btnHighScores.Width, btnHighScores.Height);
            if (buttonRect.Intersects(diverRect))
            {
                return 2;
            }
            return 0;
        }

        private void setCornerPositions()
        {
            int topLeftX = 400;
            int topLeftY = 100;
            int sideLength = 300;
            double angle = 120 * Math.PI / 180; //120 degrees. stored in radians
            cornerPositions[0] = new Vector2(topLeftX, topLeftY);
            cornerPositions[1] = new Vector2(topLeftX + sideLength, topLeftY);
            cornerPositions[2] = new Vector2((float)(topLeftX + sideLength - Math.Cos(angle) * sideLength), (float)(Math.Sin(angle) * sideLength + topLeftY));
            cornerPositions[3] = new Vector2(topLeftX + sideLength, (float)(Math.Sin(angle) * 2 * sideLength + topLeftY));
            cornerPositions[4] = new Vector2(topLeftX, (float)(Math.Sin(angle) * 2 * sideLength + topLeftY));
            cornerPositions[5] = new Vector2((float)(topLeftX + Math.Cos(angle) * sideLength), (float)(Math.Sin(angle) * sideLength + topLeftY));
        }
        public void updateHighScore(List<Score> scores)
        {
            //ScoreField.Text = "" + scores[3].getScore();
        }
        public void updateRoom(RoomInfo info)
        {
            RoomInfo inf = gControl.updateRoomInfo(_wumpus.getLocation(), 0);
            _wumpus.newTurn(gControl.updateRoomInfo(_wumpus.getLocation(), 0));
            wumpusLocation = 1;// _wumpus.getLocation();
            if (info.containsBats())
            {
                mCurrentState = State.Minigame;
            }
            else
            {
                Random gen = new Random();
                availableCoins = new Coin[info.numCoins()];
                for (int i = 0; i < availableCoins.Length; i++)
                {
                    availableCoins[i] = new Coin();
                    availableCoins[i].Initialize(Content.Load<Texture2D>("coin"), new Vector2(0, 0));
                }

                diver.setPosition(doors[info.getSide()].Position);

                bool[] availableDoors = info.getDoors();
                for (int i = 0; i < 6; i++)
                {
                    if (availableDoors[i])
                    {
                        doors[i].setVisible();
                    }
                    else
                    {
                        doors[i].setInvisible();
                    }
                }

                for (int i = 0; i < availableCoins.Length; i++)
                {
                    int x = gen.Next((int)(cornerPositions[2].X - cornerPositions[5].X)) + (int)(cornerPositions[5].X);
                    int y = gen.Next((int)(cornerPositions[3].Y - cornerPositions[0].Y)) + (int)(cornerPositions[0].Y);

                    bool inDoor = coinInDoor(availableCoins[i]);

                    while (!isWithinHexagon(new Vector2(x, y), availableCoins[i].Width, availableCoins[i].Height) && !inDoor)
                    {
                        x = gen.Next((int)(cornerPositions[2].X - cornerPositions[5].X)) + (int)(cornerPositions[5].X);
                        y = gen.Next((int)(cornerPositions[3].Y - cornerPositions[0].Y)) + (int)(cornerPositions[0].Y);
                        availableCoins[i].setPosition(new Vector2(x, y));
                        inDoor = coinInDoor(availableCoins[i]);
                    }
                    availableCoins[i].setPosition(new Vector2(x, y));
                }
                if (info.containsWumpus())
                {
                    _wumpus.setVisible();
                }
                else
                {
                    _wumpus.setInvisible();
                }
            }
        }

        private bool coinInDoor(Coin c)
        {
            Rectangle coinRect;
            Rectangle doorRect;
            bool inDoor = false;
            coinRect = new Rectangle((int)(c.Position.X), (int)(c.Position.Y), c.Width, c.Height);

            for (int i = 0; i < doors.Length; i++)
            {
                if (!doors[i].Visible)
                {
                    continue;
                }
                doorRect = new Rectangle((int)doors[i].Position.X, (int)doors[i].Position.Y, doors[i].Width, doors[i].Height);

                if (coinRect.Intersects(doorRect))
                {
                    inDoor = true;
                }
            }
            return inDoor;
        }
        public void askQuestion(Question q)
        {
            //tboxQuestion.Text = q.getQuestion();
        }
        public void displayCoins(int numCoins)
        {
            this.coins = numCoins;
        }
        public void displayArrows(int arrows)
        {
            this.arrows = arrows;
        }
        public void displayScore(int score)
        {
            this.score = score;
        }
        public void displayTurns(int numTurns)
        {
            this.turns = numTurns;
        }
        public void displayRoomNumber(int roomNumber)
        {
            this.roomNumber = roomNumber;
        }
    }
}
