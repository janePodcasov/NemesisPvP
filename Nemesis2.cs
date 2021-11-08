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
    public class Nemesis2
    {
        //make the class global
        public static Nemesis2 nemesis2;
        //variables
        public int health, speed, ballDirection;
        public Texture2D reaperFront, reaperBack, reaperLeft, reaperRight, fireBall, reaperTexture;
        public Vector2 reaperPosition;
        public bool isColliding;
        public Rectangle boundingBox;
        public float fireBallDelay;
        public List<Ball> fireBallList;

        //constructor
        public Nemesis2()
        {
            nemesis2 = this;
            health = 200;
            reaperFront = null;
            reaperBack = null;
            reaperLeft = null;
            reaperRight = null;
            fireBall = null;
            reaperTexture = null;
            reaperPosition = new Vector2(771, 224);
            speed = 5;
            fireBallList = new List<Ball>();
            fireBallDelay = 10;
            ballDirection = 1;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {
            reaperFront = Content.Load<Texture2D>("reaper_front");
            reaperBack = Content.Load<Texture2D>("reaper_back");
            reaperRight = Content.Load<Texture2D>("reaper_right");
            reaperLeft = Content.Load<Texture2D>("reaper_left");
            fireBall = Content.Load<Texture2D>("fireBall");
            reaperTexture = reaperFront;
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(reaperTexture, reaperPosition, Color.White);
            foreach (Ball b in fireBallList)
                b.Draw(spriteBatch);
        }

        //update
        public void Update(GameTime gameTime)
        {
            //check for keyboard input
            KeyboardState keyState = Keyboard.GetState();
            //PLAYER 2 MOVEMENT
            if (keyState.IsKeyDown(Keys.Up))
            {
                reaperPosition.Y = reaperPosition.Y - speed;
                reaperTexture = reaperBack;
                ballDirection = 2;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                reaperPosition.X = reaperPosition.X - speed;
                reaperTexture = reaperLeft;
                ballDirection = 3;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                reaperPosition.Y = reaperPosition.Y + speed;
                reaperTexture = reaperFront;
                ballDirection = 1;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                reaperPosition.X = reaperPosition.X + speed;
                reaperTexture = reaperRight;
                ballDirection = 4;
            }
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Left)) { ballDirection = 5; }
            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyDown(Keys.Right)) { ballDirection = 6; }
            if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyDown(Keys.Down)) { ballDirection = 7; }
            if (keyState.IsKeyDown(Keys.Down) && keyState.IsKeyDown(Keys.Right)) { ballDirection = 8; }
            //keep within the battleground bounds
            if (reaperPosition.X <= 0) { reaperPosition.X = 0; }
            if (reaperPosition.Y <= 82) { reaperPosition.Y = 82; }
            if (reaperPosition.X >= 757) { reaperPosition.X = 757; }
            if (reaperPosition.Y >= 549) { reaperPosition.Y = 549; }
            //shooting
            if (keyState.IsKeyDown(Keys.RightControl)) { Shoot(); }
            UpdateBalls();
            boundingBox = new Rectangle((int)reaperPosition.X, (int)reaperPosition.Y, reaperTexture.Width, reaperTexture.Height);
        }

        public void Shoot()
        {
            if (fireBallDelay >= 0)
                fireBallDelay--;
            if (fireBallDelay <= 0)
            {
                Ball newBall = new Ball(fireBall, ballDirection);
                newBall.position = new Vector2(reaperPosition.X + reaperTexture.Width / 2, reaperPosition.Y + reaperTexture.Height / 2);
                newBall.isVisible = true;
                if (fireBallList.Count() < 5)
                    fireBallList.Add(newBall);
            }
            if (fireBallDelay == 0)
                fireBallDelay = 10;
        }

        public void UpdateBalls()
        {
            foreach (Ball b in fireBallList)
            {
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);
                if (b.direction == 1) { b.position.Y = b.position.Y + b.speed; }
                if (b.direction == 2) { b.position.Y = b.position.Y - b.speed; }
                if (b.direction == 3) { b.position.X = b.position.X - b.speed; }
                if (b.direction == 4) { b.position.X = b.position.X + b.speed; }
                if (b.direction == 5) { b.position.X = b.position.X - b.speed; b.position.Y = b.position.Y - b.speed; }
                if (b.direction == 6) { b.position.X = b.position.X + b.speed; b.position.Y = b.position.Y - b.speed; }
                if (b.direction == 7) { b.position.X = b.position.X - b.speed; b.position.Y = b.position.Y + b.speed; }
                if (b.direction == 8) { b.position.X = b.position.X + b.speed; b.position.Y = b.position.Y + b.speed; }
            }
            for (int i = 0; i < fireBallList.Count; i++)
            {
                if (fireBallList[i].position.Y >= 585 || fireBallList[i].position.Y <= 90 || fireBallList[i].position.X <= 0 || fireBallList[i].position.X >= 785)
                    fireBallList[i].isVisible = false;
            }
            for (int i = 0; i < fireBallList.Count; i++)
            {
                if (!fireBallList[i].isVisible)
                {
                    fireBallList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
