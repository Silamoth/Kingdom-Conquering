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
				return this.name;
			}
		}

		public Vector2 Position
		{
			get
			{
				return this.position;
			}
		}

		public MapType Type
		{
			get
			{
				return this.type;
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
			return string.Concat(new string[] { "Name: ", this.name, "\nType: ", this.type.ToString().Substring(0, 1), this.type.ToString().Substring(1).ToLower() });
		}
	}
}