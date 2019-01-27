using Microsoft.Xna.Framework.Content;
using System;

namespace Kingdom_Conquering
{
	internal class Swordsman : Soldier
	{
		public Swordsman(ContentManager content) : base(content)
		{
			this.attack = 1;
			this.defense = 1;
			this.attackSpeed = 1;
			this.movementSpeed = 1;
			this.initialHealth = 5;
			base.Name = "Swordsman";
		}
	}
}