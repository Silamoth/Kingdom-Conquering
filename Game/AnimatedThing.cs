using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
    enum AnimationState { UP, DOWN, LEFT, RIGHT }

	internal class AnimatedThing
	{
		private int currentFrame;

		private int totalFrames;

		protected Texture2D texture;

		protected int rectangleX;

		protected int rectangleY;

		private int frameWidth;

		private int frameHeight;

		protected float timer = 0f;

		protected float interval = 125f;

		protected AnimationState state;

		public int CurrentFrame
		{
			get
			{
				return currentFrame;
			}
			set
			{
				currentFrame = value;
			}
		}

		public int FrameHeight
		{
			get
			{
				return frameHeight;
			}
			set
			{
				frameHeight = value;
			}
		}

		public int FrameWidth
		{
			get
			{
				return frameWidth;
			}
			set
			{
				frameWidth = value;
			}
		}

		public AnimationState State
		{
			get
			{
				return state;
			}
		}

		public float Timer
		{
			get
			{
				return timer;
			}
			set
			{
				timer = value;
			}
		}

		public int TotalFrames
		{
			get
			{
				return totalFrames;
			}
			set
			{
				totalFrames = value;
			}
		}

		public AnimatedThing()
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			Rectangle rectangle = new Rectangle(rectangleX, rectangleY, frameWidth, frameHeight);
			spriteBatch.Draw(texture, position, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, 1.5f, 0, 0f);
		}

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
		{
			Rectangle rectangle = new Rectangle(rectangleX, rectangleY, frameWidth, frameHeight);
			spriteBatch.Draw(texture, position, new Rectangle?(rectangle), color);
		}

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 target)
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
		{
			Rectangle sourceRectangle = new Rectangle(rectangleX, rectangleY, frameWidth, frameHeight);
			spriteBatch.Draw(texture, rectangle, new Rectangle?(sourceRectangle), Color.White);
		}

		public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float directionRadians)
		{
		}

		public virtual void SetDown()
		{
		}

		public virtual void SetLeft()
		{
		}

		public virtual void SetRight()
		{
		}

		public virtual void SetUp()
		{
		}

		public virtual void Update()
		{
		}

		public virtual void Update(GameTime gameTime, Vector2 position)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}
	}
}