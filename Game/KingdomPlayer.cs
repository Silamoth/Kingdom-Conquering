using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
    internal class KingdomPlayer
    {
        private Vector2 position;
        private Rectangle rectangle;
        private AnimatedSprite sprite;
        private KeyboardState oldKeyboardState;
        Keys currentKey;                      

        public KingdomPlayer(ContentManager content)
        {
            if (sprite == null)
            {
                Texture2D texture = content.Load<Texture2D>("playerSpritesheet");
                sprite = new AnimatedSprite(texture, 25, 40);
            }
            currentKey = Keys.None;

            position = new Vector2();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Update(GameTime gameTime/**, List<Rectangle> buildingRects**/)
        {
            sprite.Update();
            KeyboardState newKeyboardState = Keyboard.GetState();
            Vector2 oldPosition = position;

            switch (currentKey)
            {
                case Keys.W:
                    if (position.Y - 3f > 0f)
                    {
                        position.Y -= 3f;
                        sprite.UpdateUp(gameTime);
                    }
                    break;
                case Keys.S:
                    if (position.Y + 3f < 7940f)
                    {
                        position.Y += 3f;
                        sprite.UpdateDown(gameTime);
                    }
                    break;
                case Keys.D:
                    if (position.X + 3f < 9949f)
                    {
                        position.X += 3f;
                        sprite.UpdateRight(gameTime);
                    }
                    break;
                case Keys.A:
                    if (position.X - 3f > 0f)
                    {
                        position.X -= 3f;
                        sprite.UpdateLeft(gameTime);
                    }
                    break;
            }

            if (newKeyboardState.IsKeyDown(Keys.D) && !oldKeyboardState.IsKeyDown(Keys.D))
            {
                sprite.SetRight();
                currentKey = Keys.D;
            }
            else if (newKeyboardState.IsKeyDown(Keys.A) && !oldKeyboardState.IsKeyDown(Keys.A))
            {
                sprite.SetLeft();
                currentKey = Keys.A;
            }
            else if (newKeyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
            {
                sprite.SetUp();
                currentKey = Keys.W;
            }
            else if (newKeyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyDown(Keys.S))
            {
                sprite.SetDown();
                currentKey = Keys.S;
            }
            else if (!newKeyboardState.IsKeyDown(Keys.D) && !oldKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.A) && !oldKeyboardState.IsKeyDown(Keys.A)
                && !newKeyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W) && !newKeyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyDown(Keys.S))
                currentKey = Keys.None;

            rectangle = new Rectangle((int)position.X, (int)position.Y, sprite.FrameWidth, sprite.FrameHeight);

            /**if (position != oldPosition)
            {
                foreach (Rectangle rect in buildingRects)
                {
                    if (rectangle.Intersects(rect))
                    {
                        position = oldPosition;
                        break;
                    }
                }
            }**/

            oldKeyboardState = newKeyboardState;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 Position
        {
            get { return position; }
        }
    }
}