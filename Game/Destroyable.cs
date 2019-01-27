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
				return health;
			}
			set
			{
				health = value;
			}
		}

		public InventoryItem Item
		{
			get
			{
				return item;
			}
		}

		public Microsoft.Xna.Framework.Rectangle Rectangle
		{
			get
			{
				return rectangle;
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
			while (!IsPositionGood(otherRects))
			{
				position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
				rectangle = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, textureOne.Width, textureOne.Height);
			}
		}

		public void Draw(SpriteBatch spriteBatch, float scaleX, float scaleY)
		{
			float single = (float)health / (float)initialHealth;
			if (1f.Equals(single))
			{
				spriteBatch.Draw(textureOne, new Rectangle((int)position.X, (int)position.Y, (int)(textureOne.Width * scaleX), (int)(textureOne.Height * scaleY)), Color.White);
			}
			else if (0.75f.Equals(single))
			{
				spriteBatch.Draw(textureTwo, position, Color.White);
			}
			else if (0.5f.Equals(single))
			{
				spriteBatch.Draw(textureThree, position, Color.White);
			}
			else if (0.25f.Equals(single))
			{
				spriteBatch.Draw(textureFour, position, Color.White);
			}
		}

		private bool IsPositionGood(List<Microsoft.Xna.Framework.Rectangle> otherRects)
		{
			bool flag;
			foreach (Microsoft.Xna.Framework.Rectangle otherRect in otherRects)
			{
				if (Vector2.Distance(position, new Vector2((float)otherRect.X, (float)otherRect.Y)) < 100f)
				{
					if ((!rectangle.Intersects(otherRect) ? false : rectangle != otherRect))
					{
						position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
						rectangle = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, textureOne.Width, textureOne.Height);
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