using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
	internal abstract class Destroyable
	{
		protected Texture2D textureOne;

		protected Texture2D textureTwo;

		protected Texture2D textureThree;

		protected Texture2D textureFour;

		protected Vector2 position;

		protected Microsoft.Xna.Framework.Rectangle rectangle;

		protected int health;

		protected int initialHealth;

		protected InventoryItem item;

		protected static Random random;

		public int Health
		{
			get
			{
				return this.health;
			}
			set
			{
				this.health = value;
			}
		}

		public InventoryItem Item
		{
			get
			{
				return this.item;
			}
		}

		public Microsoft.Xna.Framework.Rectangle Rectangle
		{
			get
			{
				return this.rectangle;
			}
		}

		static Destroyable()
		{
			Destroyable.random = new Random();
		}

		public Destroyable(ContentManager content, List<Microsoft.Xna.Framework.Rectangle> otherRects)
		{
		}

		protected void CheckGoodPosition(List<Microsoft.Xna.Framework.Rectangle> otherRects)
		{
			while (!this.IsPositionGood(otherRects))
			{
				this.position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
				this.rectangle = new Microsoft.Xna.Framework.Rectangle((int)this.position.X, (int)this.position.Y, this.textureOne.Width, this.textureOne.Height);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			float single = (float)this.health / (float)this.initialHealth;
			if (1f.Equals(single))
			{
				spriteBatch.Draw(this.textureOne, this.position, Color.White);
			}
			else if (0.75f.Equals(single))
			{
				spriteBatch.Draw(this.textureTwo, this.position, Color.White);
			}
			else if (0.5f.Equals(single))
			{
				spriteBatch.Draw(this.textureThree, this.position, Color.White);
			}
			else if (0.25f.Equals(single))
			{
				spriteBatch.Draw(this.textureFour, this.position, Color.White);
			}
		}

		private bool IsPositionGood(List<Microsoft.Xna.Framework.Rectangle> otherRects)
		{
			bool flag;
			foreach (Microsoft.Xna.Framework.Rectangle otherRect in otherRects)
			{
				if (Vector2.Distance(this.position, new Vector2((float)otherRect.X, (float)otherRect.Y)) < 100f)
				{
					if ((!this.rectangle.Intersects(otherRect) ? false : this.rectangle != otherRect))
					{
						this.position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
						this.rectangle = new Microsoft.Xna.Framework.Rectangle((int)this.position.X, (int)this.position.Y, this.textureOne.Width, this.textureOne.Height);
						flag = false;
						return flag;
					}
				}
			}
			flag = true;
			return flag;
		}
	}
}