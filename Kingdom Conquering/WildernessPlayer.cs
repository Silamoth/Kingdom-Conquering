using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
	internal class WildernessPlayer
	{
		private Vector2 position;

		private Rectangle rectangle;

		private AnimatedSprite sprite;

		private KeyboardState oldKeyboardState;

		private bool canDamage;

		private int damageTimer;

		private List<InventoryItem> inventory;

		public bool CanDamage
		{
			get
			{
				return this.canDamage;
			}
			set
			{
				this.canDamage = value;
			}
		}

		public List<InventoryItem> Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		public Vector2 Position
		{
			get
			{
				return this.position;
			}
		}

		public WildernessPlayer(ContentManager content)
		{
			if (this.sprite == null)
			{
				Texture2D texture = content.Load<Texture2D>("playerSpritesheet");
				this.sprite = new AnimatedSprite(texture, 25, 40);
			}
			this.inventory = new List<InventoryItem>();
			this.canDamage = true;
			this.damageTimer = 0;
			this.position = new Vector2(4962f, 2950f);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			this.sprite.Draw(spriteBatch, this.position);
		}

		public void Update(GameTime gameTime, List<Rectangle> destroyableRects)
		{
			this.sprite.Update();
			KeyboardState newKeyboardState = Keyboard.GetState();
			Vector2 oldPosition = this.position;
			if (newKeyboardState.IsKeyDown(Keys.D) && !oldKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.A) && !newKeyboardState.IsKeyDown(Keys.W))
			{
				if (this.position.X + 3f < 9949f)
				{
					this.position.X += 3f;
					this.sprite.SetRight();
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.A) && !newKeyboardState.IsKeyDown(Keys.W))
			{
				if (this.position.X + 3f < 9949f)
				{
					this.position.X += 3f;
					this.sprite.UpdateRight(gameTime);
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.A) && !oldKeyboardState.IsKeyDown(Keys.A) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.W))
			{
				if (this.position.X - 3f > 0f)
				{
					this.position.X -= 3f;
					this.sprite.SetLeft();
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyDown(Keys.A) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.W))
			{
				if (this.position.X - 3f > 0f)
				{
					this.position.X -= 3f;
					this.sprite.UpdateLeft(gameTime);
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.A))
			{
				if (this.position.Y - 3f > 0f)
				{
					this.position.Y -= 3f;
					this.sprite.SetUp();
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyDown(Keys.W) && !newKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.A))
            {
				if (this.position.Y - 3f > 0f)
				{
					this.position.Y -= 3f;
					this.sprite.UpdateUp(gameTime);
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.W) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.A))
			{
				if (this.position.Y + 3f < 7940f)
				{
					this.position.Y += 3f;
					this.sprite.SetDown();
				}
			}
			else if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyDown(Keys.S) && !newKeyboardState.IsKeyDown(Keys.W) && !newKeyboardState.IsKeyDown(Keys.D) && !newKeyboardState.IsKeyDown(Keys.A))
            {
				if (this.position.Y + 3f < 7940f)
				{
					this.position.Y += 3f;
					this.sprite.UpdateDown(gameTime);
				}
			}
			this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, this.sprite.FrameWidth, this.sprite.FrameHeight);
			if (this.position != oldPosition)
			{
				foreach (Rectangle rect in destroyableRects)
				{
					if (this.rectangle.Intersects(rect))
					{
						this.position = oldPosition;
						break;
					}
				}
			}
			if (!this.canDamage)
			{
				this.damageTimer++;
				if (this.damageTimer == 75)
				{
					this.damageTimer = 0;
					this.canDamage = true;
				}
			}
			this.oldKeyboardState = newKeyboardState;
		}
	}
}