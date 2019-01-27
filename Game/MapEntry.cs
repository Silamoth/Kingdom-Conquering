using Microsoft.Xna.Framework;
using System;

namespace Kingdom_Conquering
{
	internal class MapEntry
	{
		private Vector2 position;

		private MapType type;

		private string name;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}

		public MapType Type
		{
			get
			{
				return type;
			}
		}

		public MapEntry(Vector2 position, MapType type, string name)
		{
            this.position = position;
            this.type = type;
			this.name = name;
		}

		public string GetInfo()
		{
			return string.Concat(new string[] { "Name: ", name, "\nType: ", type.ToString().Substring(0, 1), type.ToString().Substring(1).ToLower() });
		}
	}
}