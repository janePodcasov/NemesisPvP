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

namespace NemesisPvP
{
   //---------------------------------------------------------------------------------------------------MAIN
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //---------------------------------------------------------------------------------------GAME STATES
        public enum State
        {
            Start,
            Instructions,
            Playing,
            Nemesis1Victory,
            Nemesis2Victory
        }
        //---------------------------------------------------------------------------------OBJECTS/VARIABLES
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        State gameState = State.Start;                                  //set the starting state of the game
        public Texture2D startImage, instructionsImage, playingBackground, victory1Image, victory2Image;
        HUD hud = new HUD();                                                              //heads-up display
        Nemesis1 n1 = new Nemesis1();                                                              //player1
        Nemesis2 n2 = new Nemesis2();                                                              //player2
        List<Explosion> explosionList = new List<Explosion>();
        List<Potion> potionList = new List<Potion>();
        public Texture2D healthPotion, swiftPotion, poisonPotion, potionTexture;
        public Vector2 potionPosition;
        Random random = new Random();
        public float timer, interval;
        SoundManager sm = new SoundManager();

        //---------------------------------------------------------------------------------------CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;                                             //disable full-screen
            graphics.PreferredBackBufferWidth = 800;                                   //width of the screen
            graphics.PreferredBackBufferHeight = 600;                                 //height of the screen
            this.Window.Title = "NEMESIS: PvP [alpha]";                                       //window title
            startImage = null;
            instructionsImage = null;
            victory1Image = null;
            victory2Image = null;
            healthPotion = null;
            swiftPotion = null;
            poisonPotion = null;
            potionTexture = null;
            timer = 0f;
            interval = 4000f;

            Content.RootDirectory = "Content";
        }

        //----------------------------------------------------------------------------------------------INIT
        protected override void Initialize()
        {
            base.Initialize();
        }

        //--------------------------------------------------------------------------------------LOAD CONTENT
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startImage = Content.Load<Texture2D>("title");                         //load title screen image
            playingBackground = Content.Load<Texture2D>("background");         //load the dungeon background
            instructionsImage = Content.Load<Texture2D>("instructions");
            hud.LoadContent(Content);
            n1.LoadContent(Content);
            n2.LoadContent(Content);
            victory1Image = Content.Load<Texture2D>("victory1");
            victory2Image = Content.Load<Texture2D>("victory2");
            healthPotion = Content.Load<Texture2D>("health_potion");
            swiftPotion = Content.Load<Texture2D>("swift_potion");
            poisonPotion = Content.Load<Texture2D>("poison_potion");
            sm.LoadContent(Content);
            MediaPlayer.Play(sm.introSong);
            MediaPlayer.IsRepeating = true;
        }
        
        //------------------------------------------------------------------------------------UNLOAD CONTENT
        protected override void UnloadContent()
        {
        }
        
        //--------------------------------------------------------------------------------------------UPDATE
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //-----------------------------------------------------------------------------------GAME STATES
            switch (gameState)
            {
                case State.Start:
                    {
                        //listen for keyboard input
                        KeyboardState keyState = Keyboard.GetState();
                        //start the game if Enter is pressed
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            sm.pressButtonSound.Play();
                            gameState = State.Instructions;
                        }
                        break;
                    }
                case State.Instructions:
                    {
                        //listen for keyboard input
                        KeyboardState keyState = Keyboard.GetState();
                        //start the game if Enter is pressed
                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            sm.pressButtonSound.Play();
                            gameState = State.Playing;
                            MediaPlayer.Stop();
                        }
                        break;
                    }
                case State.Playing:
                    {
                        if (MediaPlayer.State == MediaState.Stopped) { MediaPlayer.Play(sm.battleSong); }
                        //if nemesis1 dies, load victory screen for nemesis2
                            if (n1.health <= 0)
                        {
                            sm.deathSound.Play();
                            gameState = State.Nemesis2Victory;
                            MediaPlayer.Stop();
                        }
                        //if nemesis2 dies, load victory screen for nemesis1
                        if (n2.health <= 0)
                        {
                            sm.deathSound.Play();
                            gameState = State.Nemesis1Victory;
                            MediaPlayer.Stop();
                        }
                        n1.Update(gameTime);
                        n2.Update(gameTime);
                        //collision of arcane balls with nemesis 2
                        for (int i = 0; i < n1.arcaneBallList.Count; i++)
                        {
                            if (n1.arcaneBallList[i].boundingBox.Intersects(n2.boundingBox))
                            {
                                sm.hitSound.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(n2.reaperPosition.X + n2.reaperTexture.Width/2, n2.reaperPosition.Y + n2.reaperTexture.Height/2)));
                                n2.health -= 5;
                                n1.arcaneBallList[i].isVisible = false;
                            }
                        }
                        //collision of fire balls with nemesis 1
                        for (int i = 0; i < n2.fireBallList.Count; i++)
                        {
                            if (n2.fireBallList[i].boundingBox.Intersects(n1.boundingBox))
                            {
                                sm.hitSound.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(n1.ghostPosition.X + n1.ghostTexture.Width/2, n1.ghostPosition.Y + n1.ghostTexture.Height/2 )));
                                n1.health -= 5;
                                n2.fireBallList[i].isVisible = false;
                            }
                        }
                        ManageExplosions();
                        foreach(Explosion ex in explosionList) { ex.Update(gameTime); }
                        //increase timer every millisecond, and spawn a potion every 2 seconds
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timer > interval) { SpawnPotions(); timer = 0f; n1.speed = 5; n2.speed = 5; }
                        //MANAGE COLLISION WITH POTIONS
                        for (int i = 0; i < potionList.Count; i++)
                        {
                            //manage collision of players with health potions
                            if (potionList[i].texture == healthPotion)
                            {
                                if (potionList[i].boundingBox.Intersects(n1.boundingBox))
                                {
                                    sm.pickupPotionSound.Play();
                                    //the health must stay within 200 points
                                    if (n1.health <= 190) { n1.health += 10; }
                                    if (n1.health > 190 && n1.health < 200) { n1.health += (200 - n1.health); }
                                    //even if no health is picked up, the potion is still destroyed
                                    potionList[i].isVisible = false;
                                }
                                if (potionList[i].boundingBox.Intersects(n2.boundingBox))
                                {
                                    sm.pickupPotionSound.Play();
                                    //the health must stay within 200 points
                                    if (n2.health <= 190) { n2.health += 10; }
                                    if (n2.health > 190 && n2.health < 200) { n2.health += (200 - n2.health); }
                                    //even if no health is picked up, the potion is still destroyed
                                    potionList[i].isVisible = false;
                                }
                            }
                            //manage collision of players with swiftness potions
                            if (potionList[i].texture == swiftPotion)
                            {
                                if (potionList[i].boundingBox.Intersects(n1.boundingBox))
                                {
                                    sm.pickupPotionSound.Play();
                                    n1.speed += 5;
                                    potionList[i].isVisible = false;
                                }
                                if (potionList[i].boundingBox.Intersects(n2.boundingBox))
                                {
                                    sm.pickupPotionSound.Play();
                                    n2.speed += 5;
                                    potionList[i].isVisible = false;
                                }
                            }
                            //manage collision of players with poison potions
                            if (potionList[i].texture == poisonPotion)
                            {
                                if (potionList[i].boundingBox.Intersects(n1.boundingBox))
                                {
                                    sm.poisonSound.Play();
                                    n1.health -= 20;
                                    potionList[i].isVisible = false;
                                }
                                if (potionList[i].boundingBox.Intersects(n2.boundingBox))
                                {
                                    sm.poisonSound.Play();
                                    n2.health -= 20;
                                    potionList[i].isVisible = false;
                                }
                            }
                        }
                        //manage the update of potions
                        ManagePotions();
                        //update HUD
                        hud.Update(gameTime);
                        break;
                    }
                case State.Nemesis1Victory:
                    {
                        if (MediaPlayer.State == MediaState.Stopped) { MediaPlayer.Play(sm.introSong); }
                        //listen for keyboard input
                        KeyboardState keyState = Keyboard.GetState();
                        //start the game if Enter is pressed
                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            sm.pressButtonSound.Play();
                            //go back to start screen
                            gameState = State.Start;
                            //reset everything
                            n1.health = 200;
                            n2.health = 200;
                            n1.arcaneBallList.Clear();
                            n2.fireBallList.Clear();
                            n1.ghostPosition = new Vector2(12, 324);
                            n2.reaperPosition = new Vector2(771, 224);
                            n1.ghostTexture = n1.ghostFront;
                            n2.reaperTexture = n2.reaperFront;
                            n1.ballDirection = 1;
                            n2.ballDirection = 1;
                            explosionList.Clear();
                            timer = 0f;
                            potionList.Clear();
                            n1.speed = 5;
                            n2.speed = 5;
                        }
                        break;
                    }
                case State.Nemesis2Victory:
                    {
                        if (MediaPlayer.State == MediaState.Stopped) { MediaPlayer.Play(sm.introSong); }
                        //listen for keyboard input
                        KeyboardState keyState = Keyboard.GetState();
                        //start the game if Enter is pressed
                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            sm.pressButtonSound.Play();
                            //go back to start screen
                            gameState = State.Start;
                            //reset everything
                            n1.health = 200;
                            n2.health = 200;
                            n1.arcaneBallList.Clear();
                            n2.fireBallList.Clear();
                            n1.ghostPosition = new Vector2(12, 324);
                            n2.reaperPosition = new Vector2(771, 224);
                            n1.ghostTexture = n1.ghostFront;
                            n2.reaperTexture = n2.reaperFront;
                            n1.ballDirection = 1;
                            n2.ballDirection = 1;
                            explosionList.Clear();
                            timer = 0f;
                            potionList.Clear();
                            n1.speed = 5;
                            n2.speed = 5;
                        }
                        break;
                    }
            }
            //-------------------------------------------------------------------------------GAME STATES END
            base.Update(gameTime);
        }
        
        //----------------------------------------------------------------------------------------------DRAW
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            //-----------------------------------------------------------------------------------GAME STATES
            switch (gameState)
            {
                case State.Start:
                    {
                        spriteBatch.Draw(startImage, new Vector2(200, 87), Color.White);
                        spriteBatch.DrawString(hud.topPanelFont, "press Enter...", new Vector2(500, 520), Color.White);
                        break;
                    }
                case State.Instructions:
                    {
                        spriteBatch.Draw(instructionsImage, new Vector2(100, 0), Color.White);
                        spriteBatch.DrawString(hud.topPanelFont, "press Space...", new Vector2(450, 500), Color.Black);
                        break;
                    }
                case State.Playing:
                    {
                        //load game
                        spriteBatch.Draw(playingBackground, new Vector2(0, 75), Color.White);
                        n1.Draw(spriteBatch);
                        n2.Draw(spriteBatch);
                        foreach (Potion p in potionList) { p.Draw(spriteBatch); }
                        foreach (Explosion ex in explosionList) { ex.Draw(spriteBatch); }
                        hud.Draw(spriteBatch);
                        break;
                    }
                case State.Nemesis1Victory:
                    {
                        spriteBatch.Draw(victory1Image, new Vector2(200, 87), Color.White);
                        spriteBatch.DrawString(hud.topPanelFont, "press Space...", new Vector2(500, 520), Color.White);
                        break;
                    }
                case State.Nemesis2Victory:
                    {
                        spriteBatch.Draw(victory2Image, new Vector2(200, 87), Color.White);
                        spriteBatch.DrawString(hud.topPanelFont, "press Space...", new Vector2(500, 520), Color.White);
                        break;
                    }
            }
            //-------------------------------------------------------------------------------GAME STATES END
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        //MANAGE EXPLOSIONS
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
        //SPAWN AND MANAGE POTIONS
        public void SpawnPotions()
        {
            //randomise the next potion;
            int randTexture = random.Next(0, 3);
            if (randTexture == 0 || randTexture == 3) { potionTexture = healthPotion; }
            if (randTexture == 1) { potionTexture = swiftPotion; }
            if (randTexture == 2) { potionTexture = poisonPotion; }
            //randomise the next potion's coordinates;
            int randX = random.Next(0, 770);
            int randY = random.Next(85, 560);
            potionPosition = new Vector2(randX, randY);
            //spawn the potion
            potionList.Add(new Potion(potionTexture, potionPosition));
        }
        public void ManagePotions()
        {
            for (int i = 0; i < potionList.Count; i++)
            {
                if (!potionList[i].isVisible)
                {
                    potionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
