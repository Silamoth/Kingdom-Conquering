using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kingdom_Conquering
{
	internal abstract class Soldier
	{
		protected int attack;

		protected int defense;

		protected int attackSpeed;

		protected int movementSpeed;

		protected int initialHealth;

		protected Texture2D texture;

		protected AnimatedSprite sprite;

		protected bool isSelected;

		protected bool canClick;

		protected int clickTimer;

		private static Dictionary<string, int> costs;

		public int Attack
		{
			get
			{
				return this.attack;
			}
		}

		public int AttackSpeed
		{
			get
			{
				return this.attackSpeed;
			}
		}

		public Vector2 BattleDestination
		{
			get;
			set;
		}

		public Vector2 BattlePosition
		{
			get;
			set;
		}

		public Rectangle BattleRectangle
		{
			get;
			set;
		}

		public static Dictionary<string, int> Costs
		{
			get
			{
				return Soldier.costs;
			}
		}

		public int CurrentHealth
		{
			get;
			set;
		}

		public int Defense
		{
			get
			{
				return this.defense;
			}
		}

		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		public int MovementSpeed
		{
			get
			{
				return this.movementSpeed;
			}
		}

		public string Name
		{
			get;
			set;
		}

		public Soldier(ContentManager content)
		{
			this.canClick = true;
			this.clickTimer = 0;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			this.sprite.Draw(spriteBatch, this.BattlePosition);
		}

		public void Update(GameTime gameTime)
		{
			if (!this.canClick)
			{
				this.clickTimer++;
				if (this.clickTimer == 75)
				{
					this.canClick = true;
					this.clickTimer = 0;
				}
			}
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Position.Y, 5, 5);
			if (mouseRect.Intersects(this.BattleRectangle))
			{
				if ((mouseState.LeftButton == ButtonState.Pressed &&  this.canClick))
				{
					this.isSelected = !this.isSelected;
					this.canClick = false;
				}
			}
			if (this.BattleDestination.X > this.BattlePosition.X)
			{
				this.BattlePosition = new Vector2(this.BattlePosition.X + (float)this.movementSpeed, this.BattlePosition.Y);
			}
			else if (this.BattleDestination.X < this.BattlePosition.X)
			{
				this.BattlePosition = new Vector2(this.BattlePosition.X - (float)this.movementSpeed, this.BattlePosition.Y);
			}
			if (this.BattleDestination.Y > this.BattlePosition.Y)
			{
				this.BattlePosition = new Vector2(this.BattlePosition.X, this.BattlePosition.Y + (float)this.movementSpeed);
			}
			else if (this.BattleDestination.Y < this.BattlePosition.Y)
			{
				this.BattlePosition = new Vector2(this.BattlePosition.X, this.BattlePosition.Y - (float)this.movementSpeed);
			}
		}
	}
}