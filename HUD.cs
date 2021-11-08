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
    public class HUD
    {
        //variables
        public SpriteFont topPanelFont;
        public int hudWidth, hudHeight;
        public Texture2D healthbarTexture, healthFillerTexture, hudBackground;
        public Rectangle nemesis1HealthRectangle, nemesis2HealthRectangle, hudRectangle;
        public Vector2 nemesis1HealthbarPosition, nemesis2HealthbarPosition;

        //constructor
        public HUD()
        {
            topPanelFont = null;
            hudWidth = 800;
            hudHeight = 75;
            nemesis1HealthbarPosition = new Vector2(12, 12);
            nemesis2HealthbarPosition = new Vector2(588, 12);
            healthbarTexture = null;
            healthFillerTexture = null;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {
            topPanelFont = Content.Load<SpriteFont>("GameFont");
            healthbarTexture = Content.Load<Texture2D>("healthbar");
            healthFillerTexture = Content.Load<Texture2D>("healthbarFiller");
            hudBackground = Content.Load<Texture2D>("hudBackground");
        }

        //update
        public void Update(GameTime gameTime)
        {
            nemesis1HealthRectangle = new Rectangle((int)nemesis1HealthbarPosition.X, (int)nemesis1HealthbarPosition.Y, Nemesis1.nemesis1.health, 51);
            nemesis2HealthRectangle = new Rectangle((int)nemesis2HealthbarPosition.X, (int)nemesis2HealthbarPosition.Y, Nemesis2.nemesis2.health, 51);
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //hud panel background (to stay on top of the game itself)
            spriteBatch.Draw(hudBackground, new Vector2(0, 0), Color.White);
            //nemesis 1 health bar
            spriteBatch.Draw(healthFillerTexture, nemesis1HealthRectangle, Color.White);
            spriteBatch.Draw(healthbarTexture, nemesis1HealthbarPosition , Color.White);
            //nemesis 2 health bar
            spriteBatch.Draw(healthFillerTexture, nemesis2HealthRectangle, Color.White);
            spriteBatch.Draw(healthbarTexture, nemesis2HealthbarPosition, Color.White);
        }
    }
}
