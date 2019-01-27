using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal class AnimatedSprite : AnimatedThing
	{
		public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight)
		{
            this.texture = texture;
			base.FrameWidth = frameWidth;
			base.FrameHeight = frameHeight;
			rectangleX = 0;
			rectangleY = 0;
			base.CurrentFrame = 0;
			base.TotalFrames = texture.Width / frameWidth;
		}

		protected AnimatedSprite()
		{
		}

		protected virtual void AnimateAttacks()
		{
		}

		public virtual void SetAttackingDown()
		{
		}

		public virtual void SetAttackingLeft()
		{
		}

		public virtual void SetAttackingRight()
		{
		}

		public virtual void SetAttackingUp()
		{
		}

		public override void SetDown()
		{
			base.CurrentFrame = 8;
		}

		public override void SetLeft()
		{
			base.CurrentFrame = 4;
		}

		public override void SetRight()
		{
			base.CurrentFrame = 12;
		}

		public override void SetUp()
		{
			base.CurrentFrame = 0;
		}

		public override void Update()
		{
			switch (base.CurrentFrame)
			{
				case 0:
				{
					int frameWidth = base.FrameWidth;
					rectangleX = 0;
					int frameHeight = base.FrameHeight;
					rectangleY = 0;
					state = AnimationState.UP;
					break;
				}
				case 1:
				{
					rectangleX = base.FrameWidth;
					int num = base.FrameHeight;
					rectangleY = 0;
					state = AnimationState.UP;
					break;
				}
				case 2:
				{
					rectangleX = base.FrameWidth * 2;
					int frameHeight1 = base.FrameHeight;
					rectangleY = 0;
					state = AnimationState.UP;
					break;
				}
				case 3:
				{
					rectangleX = base.FrameWidth * 3;
					int num1 = base.FrameHeight;
					rectangleY = 0;
					state = AnimationState.UP;
					break;
				}
				case 4:
				{
					int frameWidth1 = base.FrameWidth;
					rectangleX = 0;
					rectangleY = base.FrameHeight;
					state = AnimationState.LEFT;
					break;
				}
				case 5:
				{
					rectangleX = base.FrameWidth;
					rectangleY = base.FrameHeight;
					state = AnimationState.LEFT;
					break;
				}
				case 6:
				{
					rectangleX = base.FrameWidth * 2;
					rectangleY = base.FrameHeight;
					state = AnimationState.LEFT;
					break;
				}
				case 7:
				{
					rectangleX = base.FrameWidth * 3;
					rectangleY = base.FrameHeight;
					state = AnimationState.LEFT;
					break;
				}
				case 8:
				{
					int frameWidth2 = base.FrameWidth;
					rectangleX = 0;
					rectangleY = base.FrameHeight * 2;
					state = AnimationState.DOWN;
					break;
				}
				case 9:
				{
					rectangleX = base.FrameWidth;
					rectangleY = base.FrameHeight * 2;
					state = AnimationState.DOWN;
					break;
				}
				case 10:
				{
					rectangleX = base.FrameWidth * 2;
					rectangleY = base.FrameHeight * 2;
					state = AnimationState.DOWN;
					break;
				}
				case 11:
				{
					rectangleX = base.FrameWidth * 3;
					rectangleY = base.FrameHeight * 2;
					state = AnimationState.DOWN;
					break;
				}
				case 12:
				{
					int num2 = base.FrameWidth;
					rectangleX = 0;
					rectangleY = base.FrameHeight * 3;
					state = AnimationState.RIGHT;
					break;
				}
				case 13:
				{
					rectangleX = base.FrameWidth;
					rectangleY = base.FrameHeight * 3;
					state = AnimationState.RIGHT;
					break;
				}
				case 14:
				{
					rectangleX = base.FrameWidth * 2;
					rectangleY = base.FrameHeight * 3;
					state = AnimationState.RIGHT;
					break;
				}
				case 15:
				{
					rectangleX = base.FrameWidth * 3;
					rectangleY = base.FrameHeight * 3;
					state = AnimationState.RIGHT;
					break;
				}
			}
			AnimateAttacks();
		}

		public void UpdateDown(GameTime gameTime)
		{
			float single = timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (timer > interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 8 ? true : base.CurrentFrame > 11))
				{
					base.CurrentFrame = 8;
				}
				timer = 0f;
			}
		}

		public virtual void UpdateDownAttack(GameTime gameTime)
		{
		}

		public void UpdateLeft(GameTime gameTime)
		{
			float single = timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (timer > interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 4 ? true : base.CurrentFrame > 7))
				{
					base.CurrentFrame = 4;
				}
				timer = 0f;
			}
		}

		public virtual void UpdateLeftAttack(GameTime gameTime)
		{
		}

		public void UpdateRight(GameTime gameTime)
		{
			float single = timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (timer > interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 12 ? true : base.CurrentFrame > 15))
				{
					base.CurrentFrame = 12;
				}
				timer = 0f;
			}
		}

		public virtual void UpdateRightAttack(GameTime gameTime)
		{
		}

		public void UpdateUp(GameTime gameTime)
		{
			float single = timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (timer > interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 0 ? true : base.CurrentFrame > 3))
				{
					base.CurrentFrame = 0;
				}
				timer = 0f;
			}
		}

		public virtual void UpdateUpAttack(GameTime gameTime)
		{
		}
	}
}