using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Kingdom_Conquering
{
    enum NPCAction { SELLTOPLAYER, SPEECH, BUYFROMPLAYER }
    
    abstract class NPC
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected NPCAction action;
        protected String name;
        protected Rectangle rectangle;

        public NPC(ContentManager content) { }
        
        public String Name
        {
            get { return name; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }
    }
}