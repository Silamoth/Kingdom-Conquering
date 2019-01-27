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
				return position;
			}
			set
			{
				position = value;
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
				return viewMatrix;
			}
		}

		public Camera()
		{
		}

		public void Update(Vector2 playerPosition)
		{
			position.X = playerPosition.X - (float)(ScreenWidth / 2);
			position.Y = playerPosition.Y - (float)(ScreenHeight / 2);
			viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0f));
		}
	}
}