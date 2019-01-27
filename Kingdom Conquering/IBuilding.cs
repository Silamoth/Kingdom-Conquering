using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kingdom_Conquering
{
    enum BuildingActions { BUILDNEW, BUYSOLDIERS, INFO }

	internal interface IBuilding
	{
		BuildingActions GetAction();

		string GetName();

		int[] GetResources();

		Texture2D GetTexture();
	}
}