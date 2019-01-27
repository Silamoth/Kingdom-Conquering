using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
	internal interface IBuilding
	{
		BuildingActions GetAction();

		string GetName();

		int[] GetResources();

		Texture2D GetTexture();
	}
}