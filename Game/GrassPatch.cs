using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal struct GrassPatch : IBuilding
	{
		private static Texture2D texture;

		private static int stone;

		private static int iron;

		private static int wood;

		private static int gold;

		static GrassPatch()
		{
			GrassPatch.stone = 0;
			GrassPatch.iron = 0;
			GrassPatch.wood = 0;
			GrassPatch.gold = 0;
		}

		public GrassPatch(ContentManager content)
		{
			if (GrassPatch.texture == null)
			{
				GrassPatch.texture = content.Load<Texture2D>("grassPatch");
			}
		}

		public BuildingActions GetAction()
		{
			return BuildingActions.BUILDNEW;
		}

		public string GetName()
		{
			return "Grass Patch";
		}

		public int[] GetResources()
		{
			int[] resources = new int[] { GrassPatch.stone, GrassPatch.iron, GrassPatch.wood, GrassPatch.gold };
			return resources;
		}

		public Texture2D GetTexture()
		{
			return GrassPatch.texture;
		}
	}
}