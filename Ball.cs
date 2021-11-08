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
    public class Ball
    {
        //variables
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 origin, position;
        public bool isVisible;
        public float speed;
        public int direction;

        //constructor
        public Ball(Texture2D newTexture, int newDirection)
        {
            speed = 10;
            texture = newTexture;
            isVisible = false;
            direction = newDirection;
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
