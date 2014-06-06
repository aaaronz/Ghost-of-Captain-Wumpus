using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace WumpusXNA
{
    class Diver : Sprite
    {
        private string[] DIVER_ASSETNAME = {"right","right", "left", "left"};
        const int START_POSITION_X = 500;
        const int START_POSITION_Y = 500;
        const int DIVER_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        Vector2 mStartingPosition = Vector2.Zero;

        public List<Harpoon> mHarpoons = new List<Harpoon>();

        ContentManager mContentManager;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Harpoon aHarpoon in mHarpoons)
            {
                aHarpoon.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, DIVER_ASSETNAME[UP], DIVER_ASSETNAME[RIGHT], DIVER_ASSETNAME[DOWN], DIVER_ASSETNAME[LEFT]);
            Scale = 0.03f;
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    mSpeed.X = DIVER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    mSpeed.X = DIVER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.W) == true)
                {
                    mSpeed.Y = DIVER_SPEED;
                    mDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.S) == true)
                {
                    mSpeed.Y = DIVER_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }

        public int UpdateHarpoon(GameTime theGameTime, KeyboardState aCurrentKeyboardState, int arrows)
        {
            foreach (Harpoon aHarpoon in mHarpoons)
            {
                aHarpoon.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.RightShift) == true && mPreviousKeyboardState.IsKeyDown(Keys.RightShift) == false)
            {
                if (mDirection != Vector2.Zero)
                {
                    arrows = ShootHarpoon(arrows) - 1;
                }
            }

            mPreviousKeyboardState = aCurrentKeyboardState;
            return arrows;
        }

        private int ShootHarpoon(int arrows)
        {
            if (mCurrentState == State.Walking)
            {
                bool aCreateNew = true;
                double multiplier = 1;
                if (mDirection.X != 0 && mDirection.Y != 0)
                {
                    multiplier = Math.Sqrt(2);
                }
                foreach (Harpoon aHarpoon in mHarpoons)
                {
                    if (aHarpoon.Visible == false)
                    {
                        aCreateNew = false;
                        if (arrows > 0)
                        {
                            aHarpoon.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                                new Vector2((int)(800 * Math.Abs(mDirection.X) / multiplier), (int)(800 * Math.Abs(mDirection.Y) / multiplier)), mDirection);
                        }
                        break;
                    }
                }

                if (aCreateNew == true)
                {
                    Harpoon aHarpoon = new Harpoon();
                    aHarpoon.LoadContent(mContentManager);
                    if (arrows > 0)
                    {
                        aHarpoon.Fire(Position + new Vector2(Size.Width / 2, Size.Height / 2),
                            new Vector2((int)(800 * Math.Abs(mDirection.X) / multiplier), (int)(800 * Math.Abs(mDirection.Y) / multiplier)), mDirection);
                    }
                    
                    mHarpoons.Add(aHarpoon);
                }
            }
            return arrows;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Harpoon aHarpoon in mHarpoons)
            {
                aHarpoon.Draw(theSpriteBatch);
            }

            setDirection(mDirection);
            
            base.Draw(theSpriteBatch);
        }
    }
}

