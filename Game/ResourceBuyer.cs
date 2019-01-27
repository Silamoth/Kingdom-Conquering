using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kingdom_Conquering
{
    class ResourceBuyer : NPC
    {
        AnimatedSprite sprite;
        ShopMenu shopMenu;

        public ResourceBuyer(ContentManager content, Vector2 playerPosition) : base(content)
        {
            texture = content.Load<Texture2D>("playerSpritesheet");
            action = NPCAction.BUYFROMPLAYER;
            sprite = new AnimatedSprite(texture, 25, 40);
            name = "Resource Buyer";
            shopMenu = new ShopMenu(content, ShopType.BUYING, new String[] { "stone", "wood", "iron" }.ToList<String>());
            position = new Vector2(playerPosition.X - 100, playerPosition.Y);
        }

        public void Update(GameTime gameTime, Vector2 playerPosition, ref bool canClick, Vector2 cameraPosition, ContentManager content)
        {
            sprite.Update();

            if (Vector2.Distance(position, playerPosition) > 200)
            {
                if (playerPosition.X > position.X && Math.Abs(playerPosition.X - position.X) > 10)
                {
                    position.X += 3;

                    if (sprite.State == AnimationState.RIGHT)
                        sprite.UpdateRight(gameTime);
                    else
                        sprite.SetRight();
                }
                else if (playerPosition.X < position.X && Math.Abs(playerPosition.X - position.X) > 10)
                {
                    position.X -= 3;

                    if (sprite.State == AnimationState.LEFT)
                        sprite.UpdateLeft(gameTime);
                    else
                        sprite.SetLeft();
                }
                else if (playerPosition.Y > position.Y && Math.Abs(playerPosition.Y - position.Y) > 10)
                {
                    position.Y += 3;

                    if (sprite.State == AnimationState.DOWN)
                        sprite.UpdateDown(gameTime);
                    else
                        sprite.SetDown();
                }
                else if (playerPosition.Y < position.Y && Math.Abs(playerPosition.Y - position.Y) > 10)
                {
                    position.Y -= 3;

                    if (sprite.State == AnimationState.UP)
                        sprite.UpdateUp(gameTime);
                    else
                        sprite.SetUp();
                }
            }

            rectangle = new Rectangle((int)position.X, (int)position.Y, sprite.FrameWidth, sprite.FrameHeight);

            if (canClick)
            {
                MouseState mouseState = Mouse.GetState();
                Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + cameraPosition.X), (int)((float)mouseState.Y + cameraPosition.Y), 5, 5);

                if (mouseRectangle.Intersects(rectangle) && mouseState.LeftButton == ButtonState.Pressed && canClick)
                {
                    canClick = false;                    
                    shopMenu.IsActive = true;
                }
            }

            if (shopMenu.IsActive)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            if (shopMenu.IsActive)
                shopMenu.Draw(spriteBatch);
        }
    }
}