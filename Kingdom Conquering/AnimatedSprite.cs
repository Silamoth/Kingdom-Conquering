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
			this.rectangleX = 0;
			this.rectangleY = 0;
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
					this.rectangleX = 0;
					int frameHeight = base.FrameHeight;
					this.rectangleY = 0;
					this.state = AnimationState.UP;
					break;
				}
				case 1:
				{
					this.rectangleX = base.FrameWidth;
					int num = base.FrameHeight;
					this.rectangleY = 0;
					this.state = AnimationState.UP;
					break;
				}
				case 2:
				{
					this.rectangleX = base.FrameWidth * 2;
					int frameHeight1 = base.FrameHeight;
					this.rectangleY = 0;
					this.state = AnimationState.UP;
					break;
				}
				case 3:
				{
					this.rectangleX = base.FrameWidth * 3;
					int num1 = base.FrameHeight;
					this.rectangleY = 0;
					this.state = AnimationState.UP;
					break;
				}
				case 4:
				{
					int frameWidth1 = base.FrameWidth;
					this.rectangleX = 0;
					this.rectangleY = base.FrameHeight;
					this.state = AnimationState.LEFT;
					break;
				}
				case 5:
				{
					this.rectangleX = base.FrameWidth;
					this.rectangleY = base.FrameHeight;
					this.state = AnimationState.LEFT;
					break;
				}
				case 6:
				{
					this.rectangleX = base.FrameWidth * 2;
					this.rectangleY = base.FrameHeight;
					this.state = AnimationState.LEFT;
					break;
				}
				case 7:
				{
					this.rectangleX = base.FrameWidth * 3;
					this.rectangleY = base.FrameHeight;
					this.state = AnimationState.LEFT;
					break;
				}
				case 8:
				{
					int frameWidth2 = base.FrameWidth;
					this.rectangleX = 0;
					this.rectangleY = base.FrameHeight * 2;
					this.state = AnimationState.DOWN;
					break;
				}
				case 9:
				{
					this.rectangleX = base.FrameWidth;
					this.rectangleY = base.FrameHeight * 2;
					this.state = AnimationState.DOWN;
					break;
				}
				case 10:
				{
					this.rectangleX = base.FrameWidth * 2;
					this.rectangleY = base.FrameHeight * 2;
					this.state = AnimationState.DOWN;
					break;
				}
				case 11:
				{
					this.rectangleX = base.FrameWidth * 3;
					this.rectangleY = base.FrameHeight * 2;
					this.state = AnimationState.DOWN;
					break;
				}
				case 12:
				{
					int num2 = base.FrameWidth;
					this.rectangleX = 0;
					this.rectangleY = base.FrameHeight * 3;
					this.state = AnimationState.RIGHT;
					break;
				}
				case 13:
				{
					this.rectangleX = base.FrameWidth;
					this.rectangleY = base.FrameHeight * 3;
					this.state = AnimationState.RIGHT;
					break;
				}
				case 14:
				{
					this.rectangleX = base.FrameWidth * 2;
					this.rectangleY = base.FrameHeight * 3;
					this.state = AnimationState.RIGHT;
					break;
				}
				case 15:
				{
					this.rectangleX = base.FrameWidth * 3;
					this.rectangleY = base.FrameHeight * 3;
					this.state = AnimationState.RIGHT;
					break;
				}
			}
			this.AnimateAttacks();
		}

		public void UpdateDown(GameTime gameTime)
		{
			float single = this.timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			this.timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 8 ? true : base.CurrentFrame > 11))
				{
					base.CurrentFrame = 8;
				}
				this.timer = 0f;
			}
		}

		public virtual void UpdateDownAttack(GameTime gameTime)
		{
		}

		public void UpdateLeft(GameTime gameTime)
		{
			float single = this.timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			this.timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 4 ? true : base.CurrentFrame > 7))
				{
					base.CurrentFrame = 4;
				}
				this.timer = 0f;
			}
		}

		public virtual void UpdateLeftAttack(GameTime gameTime)
		{
		}

		public void UpdateRight(GameTime gameTime)
		{
			float single = this.timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			this.timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 12 ? true : base.CurrentFrame > 15))
				{
					base.CurrentFrame = 12;
				}
				this.timer = 0f;
			}
		}

		public virtual void UpdateRightAttack(GameTime gameTime)
		{
		}

		public void UpdateUp(GameTime gameTime)
		{
			float single = this.timer;
			TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
			this.timer = single + (float)elapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval)
			{
				base.CurrentFrame = base.CurrentFrame + 1;
				if ((base.CurrentFrame < 0 ? true : base.CurrentFrame > 3))
				{
					base.CurrentFrame = 0;
				}
				this.timer = 0f;
			}
		}

		public virtual void UpdateUpAttack(GameTime gameTime)
		{
		}
	}
}