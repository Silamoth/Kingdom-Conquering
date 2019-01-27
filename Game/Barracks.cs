using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal struct Barracks : IBuilding
	{
		private static Texture2D texture;

		private static int stone;

		private static int iron;

		private static int wood;

		private static int gold;

		static Barracks()
		{
			Barracks.stone = 2500;
			Barracks.iron = 1000;
			Barracks.wood = 7000;
			Barracks.gold = 15;
		}

		public Barracks(ContentManager content)
		{
			if (Barracks.texture == null)
			{
				Barracks.texture = content.Load<Texture2D>("barracks");
			}
		}

		public BuildingActions GetAction()
		{
			return BuildingActions.BUYSOLDIERS;
		}

		public string GetName()
		{
			return "Barracks";
		}

		public int[] GetResources()
		{
			int[] resources = new int[] { Barracks.stone, Barracks.iron, Barracks.wood, Barracks.gold };
			return resources;
		}

		public Texture2D GetTexture()
		{
			return Barracks.texture;
		}
	}
}