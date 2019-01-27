using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Kingdom_Conquering
{
    struct House : IBuilding
    {
        Texture2D texture;

        public House(ContentManager content)
        {
            texture = content.Load<Texture2D>("house");
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public int[] GetResources()
        {
            int[] resources = new int[4];
            resources[0] = 1000;
            resources[1] = 500;
            resources[2] = 1000;
            resources[3] = 10;

            return resources;
        }

        public String GetName()
        {
            return "House";
        }

        public BuildingActions GetAction()
        {
            return BuildingActions.INFO;
        }
    }

    struct GrassPatch : IBuilding
    {
        Texture2D texture;

        public GrassPatch(ContentManager content)
        {
            texture = content.Load<Texture2D>("grassPatch");
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public int[] GetResources()
        {
            int[] resources = new int[4];
            resources[0] = 0;
            resources[1] = 0;
            resources[2] = 0;
            resources[3] = 0;

            return resources;
        }

        public String GetName()
        {
            return "Grass Patch";
        }

        public BuildingActions GetAction()
        {
            return BuildingActions.BUILDNEW;
        }
    }

    struct Barracks : IBuilding
    {
        Texture2D texture;

        public Barracks(ContentManager content)
        {
            texture = content.Load<Texture2D>("barracks");
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public int[] GetResources()
        {
            int[] resources = new int[4];
            resources[0] = 2500;
            resources[1] = 1000;
            resources[2] = 7000;
            resources[3] = 15;

            return resources;
        }

        public String GetName()
        {
            return "Barracks";
        }

        public BuildingActions GetAction()
        {
            return BuildingActions.BUYSOLDIERS;
        }
    }

    internal class Kingdom
	{
		private float stone;

		private float iron;

		private float wood;

		private int totalWorkers;

		private int unusedWorkers;

		private int stoneWorkers;

		private int ironWorkers;

		private int woodWorkers;

		private int gold;

		private string name;

		private string owner;

		private List<Soldier> soldiers;

		private IBuilding[][] buildings;

		public IBuilding[][] Buildings
		{
			get
			{
				return this.buildings;
			}
			set
			{
				this.buildings = value;
			}
		}

		public int Gold
		{
			get
			{
				return this.gold;
			}
			set
			{
				this.gold = value;
			}
		}

		public float Iron
		{
			get
			{
				return this.iron;
			}
			set
			{
				this.iron = value;
			}
		}

		public int IronWorkers
		{
			get
			{
				return this.ironWorkers;
			}
			set
			{
				this.ironWorkers = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Owner
		{
			get
			{
				return this.owner;
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
				return this.soldiers;
			}
			set
			{
				this.soldiers = value;
			}
		}

		public float Stone
		{
			get
			{
				return this.stone;
			}
			set
			{
				this.stone = value;
			}
		}

		public int StoneWorkers
		{
			get
			{
				return this.stoneWorkers;
			}
			set
			{
				this.stoneWorkers = value;
			}
		}

		public int TotalWorkers
		{
			get
			{
				return this.totalWorkers;
			}
			set
			{
				this.totalWorkers = value;
			}
		}

		public int UnusedWorkers
		{
			get
			{
				return this.unusedWorkers;
			}
			set
			{
				this.unusedWorkers = value;
			}
		}

		public float Wood
		{
			get
			{
				return this.wood;
			}
			set
			{
				this.wood = value;
			}
		}

		public int WoodWorkers
		{
			get
			{
				return this.woodWorkers;
			}
			set
			{
				this.woodWorkers = value;
			}
		}

		public Kingdom(string name, string owner)
		{
			this.name = name;
			this.owner = owner;
			this.soldiers = new List<Soldier>();
			this.buildings = new IBuilding[8][];
		}
	}
}