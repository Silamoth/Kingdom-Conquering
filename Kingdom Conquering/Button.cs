using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Kingdom_Conquering
{
	internal class Button
	{
		private Microsoft.Xna.Framework.Rectangle originalRectangle;

		private Microsoft.Xna.Framework.Rectangle rectangle;

		private Texture2D texture;

		private bool isHovered;

		private bool isActivated;

		private SoundEffect click;

		private bool canSound = true;

		private bool incrementSoundTimer = false;

		private int soundTimer = 0;

		public bool IsActivated
		{
			get
			{
				return this.isActivated;
			}
		}

		public bool IsHovered
		{
			get
			{
				return this.isHovered;
			}
		}

		public Microsoft.Xna.Framework.Rectangle Rectangle
		{
			get
			{
				return this.originalRectangle;
			}
		}

		public Button(Microsoft.Xna.Framework.Rectangle rectangle, Texture2D texture, ContentManager content)
		{
			this.originalRectangle = rectangle;
			this.texture = texture;
			this.isHovered = false;
			this.isActivated = false;
			this.click = content.Load<SoundEffect>("buttonClick");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.isHovered)
			{
				spriteBatch.Draw(this.texture, this.originalRectangle, new Color(175, 175, 175, 255));
			}
			else
			{
				spriteBatch.Draw(this.texture, this.originalRectangle, Color.White);
			}
		}

		public void Update(Microsoft.Xna.Framework.Rectangle mouseRectangle, float scaleX, float scaleY)
		{
			this.rectangle = new Microsoft.Xna.Framework.Rectangle((int)((float)this.originalRectangle.X * scaleX), (int)((float)this.originalRectangle.Y * scaleY), (int)((float)this.originalRectangle.Width * scaleX), (int)((float)this.originalRectangle.Height * scaleY));
			this.isHovered = false;
			this.isActivated = false;
			MouseState state = Mouse.GetState();
			if (mouseRectangle.Intersects(this.rectangle))
			{
				this.isHovered = true;
			}
			if (this.isHovered)
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					this.isActivated = true;
					if (this.canSound)
					{
						this.click.Play();
						this.canSound = false;
						this.incrementSoundTimer = true;
					}
				}
			}
			if (this.incrementSoundTimer)
			{
				this.soundTimer++;
				if (this.soundTimer == 50)
				{
					this.incrementSoundTimer = false;
					this.soundTimer = 0;
					this.canSound = true;
				}
			}
		}
	}
}