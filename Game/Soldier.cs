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
				return attack;
			}
		}

		public int AttackSpeed
		{
			get
			{
				return attackSpeed;
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
				return defense;
			}
		}

		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
		}

		public int MovementSpeed
		{
			get
			{
				return movementSpeed;
			}
		}

		public string Name
		{
			get;
			set;
		}

		public Soldier(ContentManager content)
		{
			canClick = true;
			clickTimer = 0;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch, BattlePosition);
		}

		public void Update(GameTime gameTime)
		{
			if (!canClick)
			{
				clickTimer++;
				if (clickTimer == 75)
				{
					canClick = true;
					clickTimer = 0;
				}
			}
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Position.Y, 5, 5);
			if (mouseRect.Intersects(BattleRectangle))
			{
				if ((mouseState.LeftButton == ButtonState.Pressed &&  canClick))
				{
					isSelected = !isSelected;
					canClick = false;
				}
			}
			if (BattleDestination.X > BattlePosition.X)
			{
				BattlePosition = new Vector2(BattlePosition.X + (float)movementSpeed, BattlePosition.Y);
			}
			else if (BattleDestination.X < BattlePosition.X)
			{
				BattlePosition = new Vector2(BattlePosition.X - (float)movementSpeed, BattlePosition.Y);
			}
			if (BattleDestination.Y > BattlePosition.Y)
			{
				BattlePosition = new Vector2(BattlePosition.X, BattlePosition.Y + (float)movementSpeed);
			}
			else if (BattleDestination.Y < BattlePosition.Y)
			{
				BattlePosition = new Vector2(BattlePosition.X, BattlePosition.Y - (float)movementSpeed);
			}
		}
	}
}