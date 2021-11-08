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
    public class Nemesis1
    {
        //make the class global
        public static Nemesis1 nemesis1;
        //variables
        public int health, speed, ballDirection;
        public Texture2D ghostFront, ghostBack, ghostLeft, ghostRight, arcaneBall, ghostTexture;
        public Vector2 ghostPosition;
        public bool isColliding;
        public Rectangle boundingBox;
        public float arcaneBallDelay;
        public List<Ball> arcaneBallList;

        //constructor
        public Nemesis1()
        {
            nemesis1 = this;
            health = 200;
            ghostFront = null;
            ghostBack = null;
            ghostLeft = null;
            ghostRight = null;
            arcaneBall = null;
            ghostTexture = null;
            ghostPosition = new Vector2(12, 324);
            speed = 5;
            arcaneBallList = new List<Ball>();
            arcaneBallDelay = 10;
            ballDirection = 1;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {
            ghostFront = Content.Load<Texture2D>("ghost_front");
            ghostBack = Content.Load<Texture2D>("ghost_back");
            ghostRight = Content.Load<Texture2D>("ghost_right");
            ghostLeft = Content.Load<Texture2D>("ghost_left");
            arcaneBall = Content.Load<Texture2D>("arcaneBall");
            ghostTexture = ghostFront;
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ghostTexture, ghostPosition, Color.White);
            foreach (Ball b in arcaneBallList)
                b.Draw(spriteBatch);
        }

        //update
        public void Update(GameTime gameTime)
        {
            //check for keyboard input
            KeyboardState keyState = Keyboard.GetState();
            //PLAYER 1 MOVEMENT
            if (keyState.IsKeyDown(Keys.W))
            {
                ghostPosition.Y = ghostPosition.Y - speed;
                ghostTexture = ghostBack;
                ballDirection = 2;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                ghostPosition.X = ghostPosition.X - speed;
                ghostTexture = ghostLeft;
                ballDirection = 3;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                ghostPosition.Y = ghostPosition.Y + speed;
                ghostTexture = ghostFront;
                ballDirection = 1;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                ghostPosition.X = ghostPosition.X + speed;
                ghostTexture = ghostRight;
                ballDirection = 4;
            }
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.A)) { ballDirection = 5; }
            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyDown(Keys.D)) { ballDirection = 6; }
            if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.S)) { ballDirection = 7; }
            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyDown(Keys.D)) { ballDirection = 8; }
            //keep within the battleground bounds
            if (ghostPosition.X <= 0) { ghostPosition.X = 0; }
            if (ghostPosition.Y <= 85) { ghostPosition.Y = 85; }
            if (ghostPosition.X >= 770) { ghostPosition.X = 770; }
            if (ghostPosition.Y >= 554) { ghostPosition.Y = 554; }
            //shooting
            if (keyState.IsKeyDown(Keys.LeftControl)) { Shoot(); }
            UpdateBalls();
            boundingBox = new Rectangle((int)ghostPosition.X, (int)ghostPosition.Y, ghostTexture.Width, ghostTexture.Height);
        }

        public void Shoot()
        {
            if (arcaneBallDelay >= 0)
                arcaneBallDelay--;
            if (arcaneBallDelay <= 0)
            {
                    Ball newBall = new Ball(arcaneBall, ballDirection);
                    newBall.position = new Vector2(ghostPosition.X + ghostTexture.Width/2, ghostPosition.Y + ghostTexture.Height/2);
                    newBall.isVisible = true;
                    if (arcaneBallList.Count() < 5)
                        arcaneBallList.Add(newBall);
            }
            if (arcaneBallDelay == 0)
                arcaneBallDelay = 10;
        }

        public void UpdateBalls()
        {
            foreach (Ball b in arcaneBallList)
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
            for (int i = 0; i < arcaneBallList.Count; i++) { 
                if (arcaneBallList[i].position.Y >= 585 || arcaneBallList[i].position.Y <= 90 || arcaneBallList[i].position.X <= 0 || arcaneBallList[i].position.X >= 785)
                    arcaneBallList[i].isVisible = false;
            }
            for (int i = 0; i < arcaneBallList.Count; i++)
            {
                if (!arcaneBallList[i].isVisible)
                {
                    arcaneBallList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
