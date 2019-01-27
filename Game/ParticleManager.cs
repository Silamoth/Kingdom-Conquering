using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Kingdom_Conquering
{
	public class ParticleManager
	{
		private List<Particle> particles;

		public ParticleManager(ContentManager content)
		{
			particles = new List<Particle>();
			for (int i = 0; i < 30; i++)
			{
				particles.Add(new Particle(content));
			}
		}

		public void AddParticle(Vector2 position, int angle, float xSpeed, float ySpeed, Color color, float size, float life)
		{
			foreach (Particle particle in particles)
			{
				if (particle.Life <= 0f)
				{
					particle.Activate(position, angle, xSpeed, ySpeed, color, size, life);
					break;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Particle particle in particles)
			{
				if (particle.Life > 0f)
				{
					particle.Draw(spriteBatch);
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			foreach (Particle particle in particles)
			{
				if (particle.Life > 0f)
				{
					particle.Update(gameTime);
				}
			}
		}
	}
}