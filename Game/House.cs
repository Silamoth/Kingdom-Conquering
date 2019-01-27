using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal struct House : IBuilding
	{
		private static Texture2D texture;

		private static int stone;

		private static int iron;

		private static int wood;

		private static int gold;

		static House()
		{
			House.stone = 1000;
			House.iron = 100;
			House.wood = 5000;
			House.gold = 10;
		}

		public House(ContentManager content)
		{
			if (House.texture == null)
			{
				House.texture = content.Load<Texture2D>("house");
			}
		}

		public BuildingActions GetAction()
		{
			return BuildingActions.INFO;
		}

		public string GetName()
		{
			return "House";
		}

		public int[] GetResources()
		{
			int[] resources = new int[] { House.stone, House.iron, House.wood, House.gold };
			return resources;
		}

		public Texture2D GetTexture()
		{
			return House.texture;
		}
	}
}