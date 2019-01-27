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
				return isActivated;
			}
		}

		public bool IsHovered
		{
			get
			{
				return isHovered;
			}
		}

		public Microsoft.Xna.Framework.Rectangle Rectangle
		{
			get
			{
				return originalRectangle;
			}
		}

		public Button(Microsoft.Xna.Framework.Rectangle rectangle, Texture2D texture, ContentManager content)
		{
			originalRectangle = rectangle;
            this.texture = texture;
			isHovered = false;
			isActivated = false;
			click = content.Load<SoundEffect>("buttonClick");
		}

		public void Draw(SpriteBatch spriteBatch, float scaleX, float scaleY)
		{
			if (isHovered)
			{
				spriteBatch.Draw(texture, new Rectangle((int)(originalRectangle.X * scaleX), (int)(originalRectangle.Y * scaleY), (int)(originalRectangle.Width * scaleX), (int)(originalRectangle.Height * scaleY)), new Color(175, 175, 175, 255));
			}
			else
			{
                spriteBatch.Draw(texture, new Rectangle((int)(originalRectangle.X * scaleX), (int)(originalRectangle.Y * scaleY), (int)(originalRectangle.Width * scaleX), (int)(originalRectangle.Height * scaleY)), Color.White);
            }
		}

		public void Update(Rectangle mouseRectangle, float scaleX, float scaleY)
		{
			rectangle = new Rectangle((int)((float)originalRectangle.X * scaleX), (int)((float)originalRectangle.Y * scaleY), (int)((float)originalRectangle.Width * scaleX), (int)((float)originalRectangle.Height * scaleY));
			isHovered = false;
			isActivated = false;
			MouseState state = Mouse.GetState();
			if (mouseRectangle.Intersects(rectangle))
			{
				isHovered = true;
			}
			if (isHovered)
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					isActivated = true;
					if (canSound)
					{
						click.Play();
						canSound = false;
						incrementSoundTimer = true;
					}
				}
			}
			if (incrementSoundTimer)
			{
				soundTimer++;
				if (soundTimer == 50)
				{
					incrementSoundTimer = false;
					soundTimer = 0;
					canSound = true;
				}
			}
		}
	}
}