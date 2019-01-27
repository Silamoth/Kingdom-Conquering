using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Kingdom_Conquering
{
    enum ShopType { BUYING, SELLING, BOTH }

    class ShopMenu
    {
        static Texture2D texture;
        ShopType type;
        List<String> items;
        List<Texture2D> itemTextures;

        public ShopMenu(ContentManager content, ShopType type, List<String> items)
        {
            if (texture == null)
                texture = content.Load<Texture2D>("shopMenu");

            this.type = type;
            IsActive = false;
            this.items = items;

            itemTextures = new List<Texture2D>();

            for (int i = 0; i < items.Count; i++)
            {
                itemTextures.Add(content.Load<Texture2D>(items[i] + "ShopIcon"));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);

            for (int i = 0; i < itemTextures.Count; i++)
            {
                spriteBatch.Draw(itemTextures[i], new Vector2(20 + 100 * i, 50 + (int)(200) * (int)i / 3), Color.White);
            }
        }

        public bool IsActive { get; set; }
    }
}