using Microsoft.Xna.Framework;
using System;

namespace Kingdom_Conquering
{
	public class Camera
	{
		private Matrix viewMatrix;

		private Vector2 position;

		public Vector2 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public int ScreenHeight
		{
			get
			{
				return GraphicsDeviceManager.DefaultBackBufferHeight;
			}
		}

		public int ScreenWidth
		{
			get
			{
				return GraphicsDeviceManager.DefaultBackBufferWidth;
			}
		}

		public Matrix ViewMatrix
		{
			get
			{
				return this.viewMatrix;
			}
		}

		public Camera()
		{
		}

		public void Update(Vector2 playerPosition)
		{
			this.position.X = playerPosition.X - (float)(this.ScreenWidth / 2);
			this.position.Y = playerPosition.Y - (float)(this.ScreenHeight / 2);
			this.viewMatrix = Matrix.CreateTranslation(new Vector3(-this.position, 0f));
		}
	}
}