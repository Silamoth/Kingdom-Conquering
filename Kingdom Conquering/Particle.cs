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
				return this.life;
			}
		}

		public Particle(ContentManager content)
		{
			this.texture = content.Load<Texture2D>("particle");
		}

		public void Activate(Vector2 position, int angle, float xSpeed, float ySpeed, Color color, float size, float life)
		{
			this.position = position;
			this.angle = angle;
			this.xSpeed = xSpeed;
			this.ySpeed = ySpeed;
			this.color = color;
			this.originalSize = size;
			this.size = this.originalSize;
			this.originalLife = life;
			this.life = this.originalLife;
			this.angleRadians = (float)((double)angle * 3.14159265358979 / 180);
			this.velocity = new Vector2((float)((double)xSpeed * Math.Cos((double)this.angleRadians)), (float)((double)ySpeed * Math.Sin((double)this.angleRadians)));
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle? nullable = null;
			spriteBatch.Draw(this.texture, this.position, nullable, this.color, 0f, Vector2.Zero, this.size, 0, 1f);
		}

		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			this.life -= time;
			ref float x = ref this.position.X;
			x = x + this.velocity.X * time;
			ref float y = ref this.position.Y;
			y = y + this.velocity.Y * time;
			this.size = this.originalSize * (this.life / this.originalLife);
		}
	}
}