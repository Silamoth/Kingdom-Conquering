using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace Kingdom_Conquering
{
	internal class Player
	{
		private Vector2 currentPosition;

		private Vector2 targetPosition;

		private Texture2D texture;

		public bool HasKingdom
		{
			get;
			set;
		}

		public Player()
		{
		}
	}
}