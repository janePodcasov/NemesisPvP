using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NemesisPvP
{
    public class Explosion
    {
        //variables
        public Texture2D texture;
        public Vector2 position, origin;
        public float timer, interval;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;

        //constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 160f;
            currentFrame = 1;
            spriteWidth = 40;
            spriteHeight = 40;
            isVisible = true;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {

        }

        //update
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0f;
            }
            if (currentFrame == 4)
            {
                isVisible = false;
                currentFrame = 0;
            }
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
