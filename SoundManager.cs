using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace NemesisPvP
{
    public class SoundManager
    {
        //variables
        public SoundEffect pickupPotionSound, pressButtonSound, deathSound, poisonSound, hitSound;
        public Song introSong, battleSong;

        //constructor
        public SoundManager()
        {
            pickupPotionSound = null;
            pressButtonSound = null;
            poisonSound = null;
            deathSound = null;
            hitSound = null;
            introSong = null;
            battleSong = null;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {
            pickupPotionSound = Content.Load<SoundEffect>("pickupPotion");
            pressButtonSound = Content.Load<SoundEffect>("pressButton");
            hitSound = Content.Load<SoundEffect>("enemyHit");
            poisonSound = Content.Load<SoundEffect>("poisonSound");
            deathSound = Content.Load<SoundEffect>("death");
            introSong = Content.Load<Song>("introSong");
            battleSong = Content.Load<Song>("battleSong");
        }
    }
}
