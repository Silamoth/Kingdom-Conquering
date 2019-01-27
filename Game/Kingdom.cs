using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Kingdom_Conquering
{
    internal class Kingdom
	{
		private int totalWorkers;

		private int unusedWorkers;

		private int stoneWorkers;

		private int ironWorkers;

		private int woodWorkers;

		private string name;

		private string owner;

		private List<Soldier> soldiers;

		private IBuilding[][] buildings;

		public IBuilding[][] Buildings
		{
			get
			{
				return buildings;
			}
			set
			{
				buildings = value;
			}
		}

		public int IronWorkers
		{
			get
			{
				return ironWorkers;
			}
			set
			{
				ironWorkers = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string Owner
		{
			get
			{
				return owner;
			}
		}

		public int SoldierMax
		{
			get;
			set;
		}

		public List<Soldier> Soldiers
		{
			get
			{
				return soldiers;
			}
			set
			{
				soldiers = value;
			}
		}

		public int StoneWorkers
		{
			get
			{
				return stoneWorkers;
			}
			set
			{
				stoneWorkers = value;
			}
		}

		public int TotalWorkers
		{
			get
			{
				return totalWorkers;
			}
			set
			{
				totalWorkers = value;
			}
		}

		public int UnusedWorkers
		{
			get
			{
				return unusedWorkers;
			}
			set
			{
				unusedWorkers = value;
			}
		}

		public int WoodWorkers
		{
			get
			{
				return woodWorkers;
			}
			set
			{
				woodWorkers = value;
			}
		}

		public Kingdom(string name, string owner)
		{
			this.name = name;
			this.owner = owner;
			soldiers = new List<Soldier>();
			buildings = new IBuilding[8][];
		}
	}
}