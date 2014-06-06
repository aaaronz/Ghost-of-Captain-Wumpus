using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace WumpusXNA
{
    class Wumpus : Sprite
    {
        public bool Visible;
        private int numTurns;
        private int mapLocation;
        private Cave _wumpusCave;
        private int numStationaryTurns;
        private Vector2 mDirection;
        const int START_POSITION_X = 500;
        const int START_POSITION_Y = 500;
        private int CAPTAIN_SPEED = 70;

        public Wumpus()
        {
            Visible = false;
            numTurns = 0;
            Random gen = new Random();
            mapLocation = gen.Next(30) + 1;
            numStationaryTurns = 0;
            mDirection = new Vector2(0,0);
        }

        ContentManager mContentManager;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, "Captain", "Captain", "Captain", "Captain");
            Scale = 0.1f;
        }

        public void newTurn(RoomInfo info)
        {
            Random gen = new Random();
            bool[] availableDoors = info.getDoors();

            int choice = gen.Next(6);
            while (!availableDoors[choice])
            {
                choice = (choice+1)%6;
            }

            

            mapLocation = info.getCave().getAdjacentRooms(getLocation())[choice];
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, new Vector2(CAPTAIN_SPEED * Math.Abs(mDirection.X), CAPTAIN_SPEED * Math.Abs(mDirection.Y)), mDirection);
        }

        public void moveTowards(Vector2 position)
        {
            if (position.X > Position.X)
            {
                mDirection.X = 1;
            }
            if (position.Y > Position.Y)
            {
                mDirection.Y = 1;
            }
            if (position.X < Position.X)
            {
                mDirection.X = -1;
            }
            if (position.Y < Position.Y)
            {
                mDirection.Y = -1;
            }
        }

        public void setVisible()
        {
            Visible = true;
        }

        public void setInvisible()
        {
            Visible = false;
        }

        public int getLocation()
        {
            return mapLocation;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                base.Draw(theSpriteBatch);
            }
        }
    }
}
