using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WumpusXNA
{
    class Ghost
    {
        private static Random r = new Random();
        public Texture2D PlayerTexture;
        public Vector2 Position;
        private int speed = 1;
        private int direction;
        private Texture2D[] texture;
        private int xDir = r.Next(3) - 1;
        private int yDir = r.Next(3) - 1;
        private int rep = 0;
        private int desiredRepititions = 50;

        public Ghost()
        {
            texture = new Texture2D[1];
        }

        public void setSpeed(int newSpeed)
        {
            speed = newSpeed;
        }
        public int getSpeed()
        {
            return speed;
        }
        public void setDirection(int dir)
        {
            direction = dir;
        }
        public int getDirection() 
        { 
            return direction;
        }
        public int Width
        {
            get { return PlayerTexture.Width - 20; }
        }
        public int Height
        {
            get { return PlayerTexture.Height -15; }
        }
        public void Initialize(Texture2D t, Vector2 position)
        {
            PlayerTexture = t;
            Position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void moveTowards(Vector2 position)
        {
            if (position.X > Position.X)
            {
                Position.X += speed;
            }
            if (position.Y > Position.Y)
            {
                Position.Y+= speed;
            }
            if (position.X < Position.X)
            {
                Position.X-= speed;
            }
            if (position.Y < Position.Y)
            {
                Position.Y-=speed;
            }
        }
        /*public int getDirectionTowards(Vector2 v)
        {
            
        }*/
    }
    
}
