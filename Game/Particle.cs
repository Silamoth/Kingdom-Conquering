using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal class Particle
	{
		private Vector2 position;

		private float life;

		private int angle;

		private float xSpeed;

		private float ySpeed;

		private Color color;

		private float angleRadians;

		private Vector2 velocity;

		private float size;

		private float originalSize;

		private float originalLife;

		private Texture2D texture;

		public float Life
		{
			get
			{
				return life;
			}
		}

		public Particle(ContentManager content)
		{
			texture = content.Load<Texture2D>("particle");
		}

		public void Activate(Vector2 position, int angle, float xSpeed, float ySpeed, Color color, float size, float life)
		{
            this.position = position;
            this.angle = angle;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            this.color = color;
			originalSize = size;
			size = originalSize;
			originalLife = life;
			life = originalLife;
			angleRadians = (float)((double)angle * 3.14159265358979 / 180);
			velocity = new Vector2((float)((double)xSpeed * Math.Cos((double)angleRadians)), (float)((double)ySpeed * Math.Sin((double)angleRadians)));
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle? nullable = null;
			spriteBatch.Draw(texture, position, nullable, color, 0f, Vector2.Zero, size, 0, 1f);
		}

		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			life -= time;
			ref float x = ref position.X;
			x = x + velocity.X * time;
			ref float y = ref position.Y;
			y = y + velocity.Y * time;
			size = originalSize * (life / originalLife);
		}
	}
}