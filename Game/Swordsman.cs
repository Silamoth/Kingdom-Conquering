using Microsoft.Xna.Framework.Content;
using System;

namespace Kingdom_Conquering
{
	internal class Swordsman : Soldier
	{
		public Swordsman(ContentManager content) : base(content)
		{
			attack = 1;
			defense = 1;
			attackSpeed = 1;
			movementSpeed = 1;
			initialHealth = 5;
			base.Name = "Swordsman";
		}
	}
}