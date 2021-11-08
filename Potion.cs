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
    public class Potion
    {
        //variables
        public Texture2D texture;
        public Vector2 position;
        public Rectangle boundingBox;
        public bool isVisible;

        //constructor
        public Potion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            isVisible = true;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        //update
        public void Update(GameTime gameTime)
        {
            
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
                spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
