using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
	internal class Quarry : Destroyable
	{
		public Quarry(ContentManager content, List<Microsoft.Xna.Framework.Rectangle> otherRects) : base(content, otherRects)
		{
			if (textureOne == null)
			{
				textureOne = content.Load<Texture2D>("stoneOne");
				textureTwo = content.Load<Texture2D>("stoneTwo");
				textureThree = content.Load<Texture2D>("stoneThree");
				textureFour = content.Load<Texture2D>("stoneFour");
			}
			health = 4;
			initialHealth = health;
			item = new Stone(content);
			position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
			rectangle = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, textureOne.Width, textureOne.Height);
			base.CheckGoodPosition(otherRects);
		}
	}
}