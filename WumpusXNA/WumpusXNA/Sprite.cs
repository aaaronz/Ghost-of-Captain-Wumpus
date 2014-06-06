using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusXNA
{
    class Sprite
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);
        //The current direction of the Sprite
        public Vector2 Direction = new Vector2(1, 0);
        //The texture object used when drawing the sprite
        private Texture2D[] mSpriteTexture = new Texture2D[4];

        private Texture2D usedTexture;
        //The asset name for the Sprite's Texture
        public string AssetName;

        public int UP = 0;
        public int RIGHT = 1;
        public int DOWN = 2;
        public int LEFT = 3;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The Rectangular area from the original image that 
        //defines the Sprite. 
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }


        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        private float mScale = 1.0f;
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        public void setPosition(Vector2 position)
        {
            Position = position;
        }

        public void setDirection(Vector2 direction)
        {
            if (direction.X != 0 || direction.Y != 0)
            {
                int dir = 1;
                if (direction.X == 0 && direction.Y == 1)
                {
                    dir = 2;
                }
                else if (direction.X == -1)
                {
                    dir = 3;
                }
                else if (direction.X == 0 && direction.Y == -1)
                {
                    dir = 0;
                }
                usedTexture = mSpriteTexture[dir];
                Source = new Rectangle(0, 0, (int)(usedTexture.Width), (int)(usedTexture.Height));
                //Size = Source;
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetNameUp, string theAssetNameRight, string theAssetNameDown, string theAssetNameLeft)
        {
            mSpriteTexture[UP] = theContentManager.Load<Texture2D>(theAssetNameUp);
            mSpriteTexture[RIGHT] = theContentManager.Load<Texture2D>(theAssetNameRight);
            mSpriteTexture[DOWN] = theContentManager.Load<Texture2D>(theAssetNameDown);
            mSpriteTexture[LEFT] = theContentManager.Load<Texture2D>(theAssetNameLeft);
            AssetName = theAssetNameUp;
            usedTexture = mSpriteTexture[UP];

            Source = new Rectangle(0, 0, usedTexture.Width, usedTexture.Height);
            Size = new Rectangle(0, 0, (int)(usedTexture.Width * Scale), (int)(usedTexture.Height * Scale));
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(usedTexture, Position, Source,
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

    }
}

