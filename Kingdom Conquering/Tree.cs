using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
	internal class Tree : Destroyable
	{
		public Tree(ContentManager content, List<Microsoft.Xna.Framework.Rectangle> otherRects) : base(content, otherRects)
		{
			if (this.textureOne == null)
			{
				this.textureOne = content.Load<Texture2D>("treeOne");
				this.textureTwo = content.Load<Texture2D>("treeTwo");
				this.textureThree = content.Load<Texture2D>("treeThree");
				this.textureFour = content.Load<Texture2D>("treeFour");
			}
			this.health = 4;
			this.initialHealth = this.health;
			this.item = new Wood(content);
			this.position = new Vector2((float)Destroyable.random.Next(100, 9800), (float)Destroyable.random.Next(100, 7900));
			this.rectangle = new Microsoft.Xna.Framework.Rectangle((int)this.position.X, (int)this.position.Y, this.textureOne.Width, this.textureOne.Height);
			base.CheckGoodPosition(otherRects);
		}
	}
}