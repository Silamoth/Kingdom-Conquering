using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Kingdom_Conquering
{
    enum GameStates { DASHBOARD, KINGDOMSCREEN, WILDERNESS, BUYSOLDIERS, MAP, MANAGEMENT, RESOURCES }
    enum BuildMouseState { LIST, NONE }
    enum MapType { KINGDOM, BANDIT, AIKINGDOM }

	public class Main : Game
	{
		private GraphicsDeviceManager graphics;

		private SpriteBatch spriteBatch;

		private Button mapButton;

		private Button resourceButton;

		private Button menuButton;

		private Button wildernessButton;

		private Button manageButton;

		private Button stoneUpButton;

		private Button stoneDownButton;

		private Button ironUpbutton;

		private Button ironDownButton;

		private Button woodUpButton;

		private Button woodDownButton;

		private GameStates state;

		private SpriteFont font;

		private SpriteFont midLargeFont;

		private SpriteFont mediumFont;

		private SpriteFont largeFont;

		private SpriteFont smallFont;

		private Kingdom playerKingdom;

		private Texture2D buttonTexture;

		private Texture2D upButtonTexture;

		private Texture2D downButtonTexture;

		private TcpListener listener;

		private WildernessPlayer wildernessPlayer;

		private Camera camera;

		private static Random random;

		private List<Destroyable> wildernessDestroyables;

		private Texture2D axeTexture;

		private Texture2D mouseTexture;

		private Texture2D pickaxeTexture;

		private ParticleManager particleManager;

		private Texture2D grassOne;

		private Texture2D grassTwo;

		private Texture2D grassThree;

		private List<List<int>> wildernessGrass;

		private List<Rectangle> destroyableRects;

		private SoundEffect breakEffect;

		private List<MapEntry> mapEntries;

		private Texture2D kingdomIcon;

		private Vector2 mapCenterPosition;

		private Texture2D hoverTexture;

		private string hoverText;

		private List<IBuilding> buildables;

		private Texture2D buildMenuTexture;

		private BuildMouseState buildMouseState;

		private bool canClick;

		private int clickTimer;

		private int hoveredI;

		private int hoveredX;

		private Vector2 buildMenuPosition;

		private Texture2D buySoldiersTexture;

		private Button buySwordsmanButton;

		private Button kingdomButton;

		private Player player;

		private Texture2D menuTexture;

		private Texture2D grassPatch;

		static Main()
		{
			Main.random = new Random();
		}

		public Main()
		{
			this.graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Draw(GameTime gameTime)
		{
			Matrix? nullable;
			if ((this.state == GameStates.WILDERNESS || this.state == GameStates.MAP || this.state == GameStates.MANAGEMENT ? false : this.state != GameStates.BUYSOLDIERS))
			{
				nullable = null;
				this.spriteBatch.Begin(0, null, null, null, null, null, nullable);
			}
			else
			{
				this.spriteBatch.Begin(0, null, null, null, null, null, new Matrix?(this.camera.ViewMatrix));
			}
			if (this.state == GameStates.DASHBOARD)
			{
				this.DrawDashboard();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
			else if (this.state == GameStates.RESOURCES)
			{
				this.DrawResourcesMenu();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
			else if (this.state == GameStates.WILDERNESS)
			{
				this.DrawWilderness();
                GraphicsDevice.Clear(Color.DarkBlue);
			}
			else if (this.state == GameStates.MAP)
			{
				this.spriteBatch.End();
				nullable = null;
				this.spriteBatch.Begin(0, null, null, null, null, null, nullable);
				for (int x = 0; x < 13; x++)
				{
					for (int y = 0; y < 8; y++)
					{
						this.spriteBatch.Draw(this.grassPatch, new Vector2((float)(x * 64), (float)(y * 64)), Color.White);
					}
				}
				this.spriteBatch.End();
				this.spriteBatch.Begin(0, null, null, null, null, null, new Matrix?(this.camera.ViewMatrix));
				this.DrawMap();
				GraphicsDevice.Clear(Color.ForestGreen);
			}
			else if (this.state == GameStates.MANAGEMENT)
			{
				this.DrawManagement();
				GraphicsDevice.Clear(Color.White);
			}
			else if (this.state == GameStates.BUYSOLDIERS)
			{
				this.DrawSoliderBuying();
				GraphicsDevice.Clear(Color.White);
			}
			else if (this.state == GameStates.KINGDOMSCREEN)
			{
				this.DrawKingdomScreen();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
			this.spriteBatch.End();
			base.Draw(gameTime);
		}

		private void DrawDashboard()
		{
			this.spriteBatch.Draw(this.menuTexture, new Vector2(0f, 0f), Color.White);
			this.mapButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Map", new Vector2((float)(this.mapButton.Rectangle.X + 29), (float)(this.mapButton.Rectangle.Y + 5)), Color.Black);
			this.kingdomButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Kingdom", new Vector2((float)(this.kingdomButton.Rectangle.X + 15), (float)(this.kingdomButton.Rectangle.Y + 5)), Color.Black);
		}

		private void DrawKingdomScreen()
		{
			this.spriteBatch.Draw(this.menuTexture, new Vector2(0f, 0f), Color.White);
			this.spriteBatch.DrawString(this.largeFont, this.playerKingdom.Name, new Vector2(275f, 25f), Color.Black);
			this.resourceButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Resources", new Vector2((float)(this.resourceButton.Rectangle.X + 13), (float)(this.resourceButton.Rectangle.Y + 5)), Color.Black);
			this.wildernessButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Wilderness", new Vector2((float)(this.wildernessButton.Rectangle.X + 13), (float)(this.wildernessButton.Rectangle.Y + 5)), Color.Black);
			this.manageButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Buildings", new Vector2((float)(this.manageButton.Rectangle.X + 15), (float)(this.manageButton.Rectangle.Y + 5)), Color.Black);
			this.menuButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Menu", new Vector2((float)(this.menuButton.Rectangle.X + 30), (float)(this.menuButton.Rectangle.Y + 5)), Color.Black);
			this.DrawResourceDisplay();
		}

		private void DrawManagement()
		{
			for (int i = 0; i < (int)this.playerKingdom.Buildings.Length; i++)
			{
				for (int x = 0; x < (int)this.playerKingdom.Buildings.Length; x++)
				{
					this.spriteBatch.Draw(this.playerKingdom.Buildings[i][x].GetTexture(), new Vector2((float)(i * 64), (float)(x * 64)), Color.White);
				}
			}
			this.spriteBatch.DrawString(this.font, "Hit SPACE To Return", new Vector2(325f + this.camera.Position.X, 25f + this.camera.Position.Y), Color.Black);
			if (this.buildMouseState == BuildMouseState.LIST)
			{
				Mouse.GetState();
				int count = 0;
				foreach (IBuilding buildable in this.buildables)
				{
					this.spriteBatch.Draw(this.buildMenuTexture, new Vector2(this.buildMenuPosition.X, this.buildMenuPosition.Y + (float)(count * 32)), Color.White);
					this.spriteBatch.DrawString(this.font, buildable.GetName(), new Vector2(this.buildMenuPosition.X + 10f, this.buildMenuPosition.Y + (float)(count * 32) + 10f), Color.Black);
					count++;
				}
			}
			if (this.hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				this.spriteBatch.Draw(this.hoverTexture, new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Color.Blue);
				this.spriteBatch.DrawString(this.smallFont, this.hoverText, new Vector2((float)mouseState.X + this.camera.Position.X + 10f, (float)mouseState.Y + this.camera.Position.Y + 10f), Color.White);
			}
		}

		private void DrawMap()
		{
			foreach (MapEntry entry in this.mapEntries)
			{
				Vector2 position = entry.Position * 64f;
				if (Vector2.Distance(this.camera.Position, position) < 1000f)
				{
					this.spriteBatch.Draw(this.kingdomIcon, position, Color.White);
				}
			}
			if (this.hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				this.spriteBatch.Draw(this.hoverTexture, new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Color.Blue);
				this.spriteBatch.DrawString(this.font, this.hoverText, new Vector2((float)mouseState.X + this.camera.Position.X + 10f, (float)mouseState.Y + this.camera.Position.Y + 10f), Color.White);
			}
			this.spriteBatch.DrawString(this.font, "Hit SPACE To Return", new Vector2(325f + this.camera.Position.X, 25f + this.camera.Position.Y), Color.Black);
		}

		private void DrawResourceDisplay()
		{
			int gold;
			float iron;
			if (this.playerKingdom.Gold >= 1000000)
			{
				SpriteBatch spriteBatch = this.spriteBatch;
				SpriteFont spriteFont = this.font;
				gold = this.playerKingdom.Gold / 1000;
				spriteBatch.DrawString(spriteFont, string.Concat("Gold: ", gold.ToString(), "K"), new Vector2(150f, 450f), Color.Black);
			}
			else
			{
				SpriteBatch spriteBatch1 = this.spriteBatch;
				SpriteFont spriteFont1 = this.font;
				gold = this.playerKingdom.Gold;
				spriteBatch1.DrawString(spriteFont1, string.Concat("Gold: ", gold.ToString()), new Vector2(150f, 450f), Color.Black);
			}
			if (this.playerKingdom.Iron >= 1000000f)
			{
				SpriteBatch spriteBatch2 = this.spriteBatch;
				SpriteFont spriteFont2 = this.font;
				iron = this.playerKingdom.Iron / 1000f;
				spriteBatch2.DrawString(spriteFont2, string.Concat("Iron: ", iron.ToString(), "K"), new Vector2(425f, 450f), Color.Black);
			}
			else
			{
				SpriteBatch spriteBatch3 = this.spriteBatch;
				SpriteFont spriteFont3 = this.font;
				iron = this.playerKingdom.Iron;
				spriteBatch3.DrawString(spriteFont3, string.Concat("Iron: ", iron.ToString()), new Vector2(425f, 450f), Color.Black);
			}
			if (this.playerKingdom.Stone >= 1000000f)
			{
				SpriteBatch spriteBatch4 = this.spriteBatch;
				SpriteFont spriteFont4 = this.font;
				iron = this.playerKingdom.Stone / 1000f;
				spriteBatch4.DrawString(spriteFont4, string.Concat("Stone: ", iron.ToString(), "K"), new Vector2(275f, 450f), Color.Black);
			}
			else
			{
				SpriteBatch spriteBatch5 = this.spriteBatch;
				SpriteFont spriteFont5 = this.font;
				iron = this.playerKingdom.Stone;
				spriteBatch5.DrawString(spriteFont5, string.Concat("Stone: ", iron.ToString()), new Vector2(275f, 450f), Color.Black);
			}
			if (this.playerKingdom.Wood >= 1000000f)
			{
				SpriteBatch spriteBatch6 = this.spriteBatch;
				SpriteFont spriteFont6 = this.font;
				iron = this.playerKingdom.Wood / 1000f;
				spriteBatch6.DrawString(spriteFont6, string.Concat("Wood: ", iron.ToString(), "K"), new Vector2(550f, 450f), Color.Black);
			}
			else
			{
				SpriteBatch spriteBatch7 = this.spriteBatch;
				SpriteFont spriteFont7 = this.font;
				iron = this.playerKingdom.Wood;
				spriteBatch7.DrawString(spriteFont7, string.Concat("Wood: ", iron.ToString()), new Vector2(550f, 450f), Color.Black);
			}
		}

		private void DrawResourcesMenu()
		{
			this.spriteBatch.DrawString(this.midLargeFont, "Redistribute Workers", new Vector2(250f, 35f), Color.Black);
			this.menuButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Back", new Vector2((float)(this.menuButton.Rectangle.X + 30), (float)(this.menuButton.Rectangle.Y + 5)), Color.Black);
			this.stoneDownButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.mediumFont, "Stone: ", new Vector2((float)(this.stoneDownButton.Rectangle.X + 60), (float)(this.stoneDownButton.Rectangle.Y - 25)), Color.Black);
			SpriteBatch spriteBatch = this.spriteBatch;
			SpriteFont spriteFont = this.largeFont;
			int stoneWorkers = this.playerKingdom.StoneWorkers;
			spriteBatch.DrawString(spriteFont, stoneWorkers.ToString(), new Vector2((float)(this.stoneDownButton.Rectangle.X + 60), (float)(this.stoneDownButton.Rectangle.Y - 5)), Color.Black);
			this.stoneUpButton.Draw(this.spriteBatch);
			this.ironDownButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.mediumFont, "Iron: ", new Vector2((float)(this.ironDownButton.Rectangle.X + 70), (float)(this.ironDownButton.Rectangle.Y - 25)), Color.Black);
			SpriteBatch spriteBatch1 = this.spriteBatch;
			SpriteFont spriteFont1 = this.largeFont;
			stoneWorkers = this.playerKingdom.IronWorkers;
			spriteBatch1.DrawString(spriteFont1, stoneWorkers.ToString(), new Vector2((float)(this.ironDownButton.Rectangle.X + 60), (float)(this.ironDownButton.Rectangle.Y - 5)), Color.Black);
			this.ironUpbutton.Draw(this.spriteBatch);
			this.woodDownButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.mediumFont, "Wood: ", new Vector2((float)(this.woodDownButton.Rectangle.X + 70), (float)(this.woodDownButton.Rectangle.Y - 25)), Color.Black);
			SpriteBatch spriteBatch2 = this.spriteBatch;
			SpriteFont spriteFont2 = this.largeFont;
			stoneWorkers = this.playerKingdom.WoodWorkers;
			spriteBatch2.DrawString(spriteFont2, stoneWorkers.ToString(), new Vector2((float)(this.woodDownButton.Rectangle.X + 60), (float)(this.woodDownButton.Rectangle.Y - 5)), Color.Black);
			this.woodUpButton.Draw(this.spriteBatch);
			this.DrawResourceDisplay();
			SpriteBatch spriteBatch3 = this.spriteBatch;
			SpriteFont spriteFont3 = this.mediumFont;
			stoneWorkers = this.playerKingdom.TotalWorkers;
			spriteBatch3.DrawString(spriteFont3, string.Concat("Total Workers: ", stoneWorkers.ToString()), new Vector2(100f, 325f), Color.Black);
			SpriteBatch spriteBatch4 = this.spriteBatch;
			SpriteFont spriteFont4 = this.mediumFont;
			stoneWorkers = this.playerKingdom.UnusedWorkers;
			spriteBatch4.DrawString(spriteFont4, string.Concat("Unused Workers: ", stoneWorkers.ToString()), new Vector2(475f, 325f), Color.Black);
		}

		private void DrawSoliderBuying()
		{
			this.DrawManagement();
			this.spriteBatch.Draw(this.buySoldiersTexture, new Vector2(85f + this.camera.Position.X, 150f + this.camera.Position.Y), Color.White);
			this.buySwordsmanButton.Draw(this.spriteBatch);
			this.spriteBatch.DrawString(this.font, "Swordsman", new Vector2((float)(this.buySwordsmanButton.Rectangle.X + 5), (float)(this.buySwordsmanButton.Rectangle.Y + 5)), Color.Black);
			if (this.hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				this.spriteBatch.Draw(this.hoverTexture, new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Color.Blue);
				this.spriteBatch.DrawString(this.smallFont, this.hoverText, new Vector2((float)mouseState.X + this.camera.Position.X + 10f, (float)mouseState.Y + this.camera.Position.Y + 10f), Color.White);
			}
		}

		private void DrawWilderness()
		{
			for (int x = 0; x < this.wildernessGrass.Count; x++)
			{
				for (int y = 0; y < this.wildernessGrass[x].Count; y++)
				{
					int index = this.wildernessGrass[x][y];
					Vector2 position = new Vector2((float)(x * 64), (float)(y * 64));
					if (Vector2.Distance(position, this.wildernessPlayer.Position) < 600f)
					{
						switch (index)
						{
							case 0:
							{
								this.spriteBatch.Draw(this.grassOne, position, Color.White);
								break;
							}
							case 1:
							{
								this.spriteBatch.Draw(this.grassTwo, position, Color.White);
								break;
							}
							case 2:
							{
								this.spriteBatch.Draw(this.grassThree, position, Color.White);
								break;
							}
						}
					}
				}
			}
			foreach (Destroyable thing in this.wildernessDestroyables)
			{
				if (Vector2.Distance(new Vector2((float)thing.Rectangle.X, (float)thing.Rectangle.Y), this.wildernessPlayer.Position) < 500f)
				{
					thing.Draw(this.spriteBatch);
				}
			}
			this.wildernessPlayer.Draw(this.spriteBatch);
			if (this.mouseTexture != null)
			{
				SpriteBatch spriteBatch = this.spriteBatch;
				Texture2D texture2D = this.mouseTexture;
				MouseState state = Mouse.GetState();
				float single = (float)state.X + this.camera.Position.X;
				state = Mouse.GetState();
				Rectangle? nullable = null;
				spriteBatch.Draw(texture2D, new Vector2(single, (float)state.Y + this.camera.Position.Y), nullable, Color.White, 0f, Vector2.Zero, 2f, 0, 1f);
			}
			this.spriteBatch.DrawString(this.font, "Hit SPACE To Return", new Vector2(325f + this.camera.Position.X, 25f + this.camera.Position.Y), Color.Black);
			this.particleManager.Draw(this.spriteBatch);
		}

		protected override void Initialize()
		{
			int stoneWorkers;
			int ironWorkers;
			int woodWorkers;
			int totalWorkers;
			int unusedWorkers;
		    IsMouseVisible = true;
			this.state = GameStates.DASHBOARD;
			this.buttonTexture = Content.Load<Texture2D>("birchButton");
			this.upButtonTexture = Content.Load<Texture2D>("upButton");
			this.downButtonTexture = Content.Load<Texture2D>("downButton");
			this.menuTexture = Content.Load<Texture2D>("menuBackground");
			this.mapButton = new Button(new Rectangle(150, 175, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
			this.kingdomButton = new Button(new Rectangle(150, 250, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
			this.playerKingdom = new Kingdom("Playeropia", "Player");
			this.canClick = true;
			this.clickTimer = 0;
			this.listener = new TcpListener(1303);
			TcpClient client = new TcpClient("127.0.0.1", 1303);
			StreamWriter writer = new StreamWriter(client.GetStream());
			string messageString = string.Concat("1 ", this.playerKingdom.Name);
			byte[] message = Encoding.ASCII.GetBytes(messageString);
			writer.BaseStream.Write(message, 0, (int)message.Length);
			byte[] buffer = new byte[client.Client.ReceiveBufferSize];
			client.Client.Receive(buffer);
			string response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });
			List<string> splitResponse = response.Split(new char[] { ' ' }).ToList<string>();
			if (splitResponse[0] == "1")
			{
				int.TryParse(splitResponse[1], out stoneWorkers);
				int.TryParse(splitResponse[2], out ironWorkers);
				int.TryParse(splitResponse[3], out woodWorkers);
				int.TryParse(splitResponse[4], out totalWorkers);
				int.TryParse(splitResponse[5], out unusedWorkers);
				this.playerKingdom.StoneWorkers = stoneWorkers;
				this.playerKingdom.IronWorkers = ironWorkers;
				this.playerKingdom.WoodWorkers = woodWorkers;
				this.playerKingdom.TotalWorkers = totalWorkers;
				this.playerKingdom.UnusedWorkers = unusedWorkers;
			}
			base.Initialize();
		}

		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(GraphicsDevice);
			this.font = Content.Load<SpriteFont>("font");
			this.midLargeFont = Content.Load<SpriteFont>("midLargeFont");
			this.mediumFont = Content.Load<SpriteFont>("mediumFont");
			this.largeFont = Content.Load<SpriteFont>("largeFont");
			this.smallFont = Content.Load<SpriteFont>("smallFont");
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			float stone;
			float iron;
			float wood;
			int gold;
			int stoneWorkers;
			int ironWorkers;
			int woodWorkers;

			if (this.state == GameStates.DASHBOARD)
			{
				this.UpdateDashboard();
			}
			else if (this.state == GameStates.RESOURCES)
			{
				this.UpdateResources();
			}
			else if (this.state == GameStates.WILDERNESS)
			{
				this.UpdateWilderness(gameTime);
			}
			else if (this.state == GameStates.MAP)
			{
				this.UpdateMap();
			}
			else if (this.state == GameStates.MANAGEMENT)
			{
				this.UpdateManagement();
			}
			else if (this.state == GameStates.BUYSOLDIERS)
			{
				this.UpdateSoldierBuying();
			}
			else if (this.state == GameStates.KINGDOMSCREEN)
			{
				this.UpdateKingdomScreen();
			}
			if (!this.canClick)
			{
				this.clickTimer++;
				if (this.clickTimer == 20)
				{
					this.canClick = true;
					this.clickTimer = 0;
				}
			}
			TcpClient client = new TcpClient("127.0.0.1", 1303);
			StreamWriter writer = new StreamWriter(client.GetStream());
			string messageString = string.Concat(new object[] { "0 ", this.playerKingdom.Name, " ", this.playerKingdom.StoneWorkers, " ", this.playerKingdom.IronWorkers, " ", this.playerKingdom.WoodWorkers, " ", this.playerKingdom.TotalWorkers, " ", this.playerKingdom.UnusedWorkers, " ", this.playerKingdom.SoldierMax });
			byte[] message = Encoding.ASCII.GetBytes(messageString);
			writer.BaseStream.Write(message, 0, (int)message.Length);
			byte[] buffer = new byte[client.Client.ReceiveBufferSize];
			client.Client.Receive(buffer);
			string response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });
			List<string> splitResponse = response.Split(new char[] { ' ' }).ToList<string>();
			string item = splitResponse[0];
			if (item == "0")
			{
				float.TryParse(splitResponse[1], out stone);
				float.TryParse(splitResponse[2], out iron);
				float.TryParse(splitResponse[3], out wood);
				int.TryParse(splitResponse[4], out gold);
				this.playerKingdom.Stone = stone;
				this.playerKingdom.Iron = iron;
				this.playerKingdom.Wood = wood;
				this.playerKingdom.Gold = gold;
			}
			else if (item == "1")
			{
				int.TryParse(splitResponse[1], out stoneWorkers);
				int.TryParse(splitResponse[2], out ironWorkers);
				int.TryParse(splitResponse[3], out woodWorkers);
				this.playerKingdom.StoneWorkers = stoneWorkers;
				this.playerKingdom.IronWorkers = ironWorkers;
				this.playerKingdom.WoodWorkers = woodWorkers;
			}
			base.Update(gameTime);
		}

		private void UpdateDashboard()
		{
			int x;
			int y;
			int num = Mouse.GetState().X;
			MouseState state = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle(num, state.Y, 5, 5);
			this.mapButton.Update(mouseRectangle, 1f, 1f);
			this.kingdomButton.Update(mouseRectangle, 1f, 1f);
			if ((!this.mapButton.IsActivated ? false : this.canClick))
			{
				this.canClick = false;
				this.state = GameStates.MAP;
				this.kingdomIcon = Content.Load<Texture2D>("kingdomMapIcon");
				this.hoverTexture = Content.Load<Texture2D>("hoverButton");
				this.hoverText = string.Empty;
				this.grassPatch = Content.Load<Texture2D>("grassPatch");
				this.camera = new Camera();
				this.mapCenterPosition = new Vector2(400f, 240f);
				this.mapEntries = new List<MapEntry>();
				TcpClient client = new TcpClient("127.0.0.1", 1303);
				StreamWriter writer = new StreamWriter(client.GetStream());
				string messageString = string.Concat("5 ", this.playerKingdom.Name);
				byte[] message = Encoding.ASCII.GetBytes(messageString);
				writer.BaseStream.Write(message, 0, (int)message.Length);
				byte[] buffer = new byte[client.Client.ReceiveBufferSize];
				client.Client.Receive(buffer);
				List<string> splitResponse = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) }).Trim().Split(new char[] { ' ' }).ToList<string>();
				if (splitResponse[0] == "5")
				{
					for (int i = 1; i < splitResponse.Count; i++)
					{
						string[] splitEntry = splitResponse[i].Split(new char[] { '/' });
						string name = splitEntry[0];
						MapType type = (MapType)Enum.Parse(typeof(MapType), splitEntry[1]);
						int.TryParse(splitEntry[2], out x);
						int.TryParse(splitEntry[3], out y);
						this.mapEntries.Add(new MapEntry(new Vector2((float)x, (float)y), type, name));
						if (name == this.playerKingdom.Name)
						{
							this.mapCenterPosition = new Vector2((float)(x * 64), (float)(y * 64));
						}
					}
				}
			}
			if ((!this.kingdomButton.IsActivated ? false : this.canClick))
			{
				this.canClick = false;
				this.wildernessButton = new Button(new Rectangle(150, 175, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
				this.manageButton = new Button(new Rectangle(150, 225, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
				this.resourceButton = new Button(new Rectangle(150, 275, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
				this.menuButton = new Button(new Rectangle(150, 325, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
				this.state = GameStates.KINGDOMSCREEN;
			}
		}

		private void UpdateKingdomScreen()
		{
			int num = Mouse.GetState().X;
			MouseState state = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle(num, state.Y, 5, 5);
			this.resourceButton.Update(mouseRectangle, 1f, 1f);
			this.wildernessButton.Update(mouseRectangle, 1f, 1f);
			this.manageButton.Update(mouseRectangle, 1f, 1f);
			this.menuButton.Update(mouseRectangle, 1f, 1f);
			if ((!this.wildernessButton.IsActivated ? false : this.canClick))
			{
				this.canClick = false;
				this.state = GameStates.WILDERNESS;
				this.wildernessPlayer = new WildernessPlayer(Content);
				this.camera = new Camera();
				this.wildernessDestroyables = new List<Destroyable>();
				this.destroyableRects = new List<Rectangle>();
				if (this.particleManager == null)
				{
					this.particleManager = new ParticleManager(Content);
				}
				if (this.grassOne == null)
				{
					this.grassOne = Content.Load<Texture2D>("grassOne");
					this.grassTwo = Content.Load<Texture2D>("grassTwo");
					this.grassThree = Content.Load<Texture2D>("grassThree");
					this.breakEffect = Content.Load<SoundEffect>("break");
				}
				this.wildernessGrass = new List<List<int>>();
				for (int i = 0; i < 156; i++)
				{
					this.wildernessGrass.Add(new List<int>());
				}
				foreach (List<int> list in this.wildernessGrass)
				{
					for (int i = 0; i < 125; i++)
					{
						list.Add(Main.random.Next(0, 3));
					}
				}
				for (int i = 0; i < 100; i++)
				{
					switch (Main.random.Next(0, 3))
					{
						case 0:
						{
							Tree toBeAddedTree = new Tree(Content, this.destroyableRects);
							this.destroyableRects.Add(toBeAddedTree.Rectangle);
							this.wildernessDestroyables.Add(toBeAddedTree);
							break;
						}
						case 1:
						{
							Quarry toBeAddedQuarry = new Quarry(Content, this.destroyableRects);
							this.destroyableRects.Add(toBeAddedQuarry.Rectangle);
							this.wildernessDestroyables.Add(toBeAddedQuarry);
							break;
						}
						case 2:
						{
							IronOre toBeAddedIronOre = new IronOre(Content, this.destroyableRects);
							this.destroyableRects.Add(toBeAddedIronOre.Rectangle);
							this.wildernessDestroyables.Add(toBeAddedIronOre);
							break;
						}
					}
				}
			}
			if ((!this.manageButton.IsActivated ? false : this.canClick))
			{
				this.canClick = false;
				this.state = GameStates.MANAGEMENT;
				this.camera = new Camera();
				this.mapCenterPosition = new Vector2(300f, 240f);
				this.hoverTexture = Content.Load<Texture2D>("hoverButton");
				this.hoverText = string.Empty;
				this.buildables = new List<IBuilding>()
				{
					new House(Content),
					new Barracks(Content)
				};
				this.buildMenuTexture = Content.Load<Texture2D>("buildingMenuModular");
				this.buildMouseState = BuildMouseState.NONE;
				this.canClick = false;
				this.clickTimer = 0;
				this.hoveredI = -1;
				this.hoveredX = -1;
				TcpClient client = new TcpClient("127.0.0.1", 1303);
				StreamWriter writer = new StreamWriter(client.GetStream());
				string messageString = string.Concat("6 ", this.playerKingdom.Name);
				byte[] message = Encoding.ASCII.GetBytes(messageString);
				writer.BaseStream.Write(message, 0, (int)message.Length);
				byte[] buffer = new byte[client.Client.ReceiveBufferSize];
				client.Client.Receive(buffer);
				List<string> splitResponse = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) }).Trim().Split(new char[] { ' ' }).ToList<string>();
				if (splitResponse[0] == "6")
				{
					IBuilding[][] buildings = new IBuilding[8][];
					for (int i = 0; i < 8; i++)
					{
						buildings[i] = new IBuilding[8];
					}
					for (int i = 1; i < splitResponse.Count; i++)
					{
						char[] buildingsChar = splitResponse[i].Trim(new char[] { '\r' }).ToCharArray();
						for (int x = 0; x < (int)buildingsChar.Length; x++)
						{
							char chr = buildingsChar[x];
							if (chr == 'B')
							{
								buildings[i - 1][x] = new Barracks(Content);
							}
							else if (chr == 'G')
							{
								buildings[i - 1][x] = new GrassPatch(Content);
							}
							else if (chr == 'H')
							{
								buildings[i - 1][x] = new House(Content);
							}
						}
					}
					this.playerKingdom.Buildings = buildings;
				}
			}
			if ((!this.resourceButton.IsActivated ? false : this.canClick))
			{
				this.canClick = false;
				this.state = GameStates.RESOURCES;
				this.stoneDownButton = new Button(new Rectangle(65, 150, this.downButtonTexture.Width, this.downButtonTexture.Height), this.downButtonTexture, Content);
				this.stoneUpButton = new Button(new Rectangle(190, 150, this.upButtonTexture.Width, this.upButtonTexture.Height), this.upButtonTexture, Content);
				this.ironDownButton = new Button(new Rectangle(290, 150, this.downButtonTexture.Width, this.downButtonTexture.Height), this.downButtonTexture, Content);
				this.ironUpbutton = new Button(new Rectangle(415, 150, this.upButtonTexture.Width, this.upButtonTexture.Height), this.upButtonTexture, Content);
				this.woodDownButton = new Button(new Rectangle(515, 150, this.downButtonTexture.Width, this.downButtonTexture.Height), this.downButtonTexture, Content);
				this.woodUpButton = new Button(new Rectangle(640, 150, this.upButtonTexture.Width, this.upButtonTexture.Height), this.upButtonTexture, Content);
				this.menuButton = new Button(new Rectangle(25, 25, this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
			}
			if ((!this.menuButton.IsActivated ? false : this.canClick))
			{
				this.state = GameStates.DASHBOARD;
				this.canClick = false;
			}
		}

		private void UpdateManagement()
		{
			bool flag;
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				this.state = GameStates.KINGDOMSCREEN;
			}
			this.hoverText = string.Empty;
			this.camera.Update(this.mapCenterPosition);
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + this.camera.Position.X), (int)((float)mouseState.Y + this.camera.Position.Y), 5, 5);
			if ((mouseState.X <= this.graphics.GraphicsDevice.Viewport.Width - 50 ? false : this.mapCenterPosition.X < 320f))
			{
				this.mapCenterPosition.X += 5f;
			}
			else if ((mouseState.X >= 50 ? false : this.mapCenterPosition.X > 290f))
			{
				this.mapCenterPosition.X -= 5f;
			}
			if ((mouseState.Y <= this.graphics.GraphicsDevice.Viewport.Height - 50 ? false : this.mapCenterPosition.Y < 340f))
			{
				this.mapCenterPosition.Y += 5f;
			}
			else if ((mouseState.Y >= 50 ? false : this.mapCenterPosition.Y > 200f))
			{
				this.mapCenterPosition.Y -= 5f;
			}
			for (int i = 0; i < (int)this.playerKingdom.Buildings.Length; i++)
			{
				for (int x = 0; x < (int)this.playerKingdom.Buildings.Length; x++)
				{
					if (mouseRectangle.Intersects(new Rectangle(i * 64, x * 64, 64, 64)))
					{
						if (this.playerKingdom.Buildings[i][x].GetAction() == BuildingActions.BUILDNEW)
						{
							if (this.buildMouseState != BuildMouseState.LIST)
							{
								this.hoverText = "An empty patch of grass.\nPrime real estate.\nClick to build.";
								if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
								{
									if (this.buildMouseState == BuildMouseState.NONE)
									{
										this.buildMouseState = BuildMouseState.LIST;
									}
									this.canClick = false;
									this.hoveredI = i;
									this.hoveredX = x;
									this.buildMenuPosition = new Vector2((float)mouseState.X + this.camera.Position.X + 10f, (float)mouseState.Y + this.camera.Position.Y);
									this.hoverText = string.Empty;
								}
							}
						}
						else if (this.playerKingdom.Buildings[i][x].GetAction() == BuildingActions.INFO)
						{
							this.hoverText = this.playerKingdom.Buildings[i][x].GetName();
						}
						else if (this.playerKingdom.Buildings[i][x].GetAction() == BuildingActions.BUYSOLDIERS)
						{
							this.hoverText = "Barracks\nClick to buy soldiers";
							if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
							{
								this.canClick = false;
								this.state = GameStates.BUYSOLDIERS;
								this.buySoldiersTexture = Content.Load<Texture2D>("buySoldiersMenu");
								this.buySwordsmanButton = new Button(new Rectangle((int)(85f + this.camera.Position.X + 15f), (int)(150f + this.camera.Position.Y + 15f), this.buttonTexture.Width, this.buttonTexture.Height), this.buttonTexture, Content);
							}
						}
						if (this.hoveredI == -1 || this.hoveredX == -1)
						{
							flag = false;
						}
						else
						{
							flag = (i != this.hoveredI ? true : x != this.hoveredX);
						}
						if (flag)
						{
							this.buildMouseState = BuildMouseState.NONE;
							this.hoveredI = -1;
							this.hoveredX = -1;
						}
					}
				}
			}
			if (this.buildMouseState == BuildMouseState.LIST)
			{
				int count = 0;
				foreach (IBuilding buildable in this.buildables)
				{
					Rectangle menuItemPosition = new Rectangle((int)this.buildMenuPosition.X, (int)this.buildMenuPosition.Y + count * 32, 64, 32);
					if (mouseRectangle.Intersects(menuItemPosition))
					{
						int[] resources = buildable.GetResources();
						int stone = resources[0];
						int iron = resources[1];
						int wood = resources[2];
						int gold = resources[3];
						this.hoverText = string.Concat(new string[] { "Stone: ", stone.ToString(), "\nIron: ", iron.ToString(), "\nWood: ", wood.ToString(), "\nGold: ", gold.ToString() });
						if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
						{
							this.canClick = false;
							if ((this.playerKingdom.Stone < (float)stone || this.playerKingdom.Iron < (float)iron || this.playerKingdom.Wood < (float)wood ? false : this.playerKingdom.Gold >= gold))
							{
								this.playerKingdom.Buildings[this.hoveredI][this.hoveredX] = buildable;
								Kingdom newKingdom = new Kingdom(this.playerKingdom.Name, this.playerKingdom.Owner)
								{
									Stone = (float)(stone * -1),
									Iron = (float)(iron * -1),
									Wood = (float)(wood * -1),
									Gold = gold * -1
								};
								TcpClient client = new TcpClient("127.0.0.1", 1303);
								StreamWriter writer = new StreamWriter(client.GetStream());
								string messageString = string.Concat(new object[] { "2 ", newKingdom.Name, " ", newKingdom.Stone, " ", newKingdom.Iron, " ", newKingdom.Wood, " ", newKingdom.Gold });
								byte[] message = Encoding.ASCII.GetBytes(messageString);
								writer.BaseStream.Write(message, 0, (int)message.Length);
								writer.Flush();
								byte[] buffer = new byte[client.Client.ReceiveBufferSize];
								client.Client.Receive(buffer);
								List<string> splitResponse = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) }).Trim().Split(new char[] { ' ' }).ToList<string>();
								if ((splitResponse[0] != "2" ? false : splitResponse[1] == "Success"))
								{
									this.hoverText = string.Empty;
									this.buildMouseState = BuildMouseState.NONE;
									if (buildable is House)
									{
										Kingdom totalWorkers = this.playerKingdom;
										totalWorkers.TotalWorkers = totalWorkers.TotalWorkers + 15;
										Kingdom unusedWorkers = this.playerKingdom;
										unusedWorkers.UnusedWorkers = unusedWorkers.UnusedWorkers + 15;
									}
									else if (buildable is Barracks)
									{
										Kingdom soldierMax = this.playerKingdom;
										soldierMax.SoldierMax = soldierMax.SoldierMax + 15;
									}
									client = new TcpClient("127.0.0.1", 1303);
									writer = new StreamWriter(client.GetStream());
									string secondMessageString = string.Concat(new string[] { "7 ", this.playerKingdom.Name, " ", this.hoveredI.ToString(), " ", this.hoveredX.ToString(), " ", buildable.GetName() });
									byte[] secondMessage = Encoding.ASCII.GetBytes(secondMessageString);
									writer.BaseStream.Write(secondMessage, 0, (int)secondMessage.Length);
									writer.Flush();
								}
							}
						}
					}
					count++;
				}
			}
		}

		private void UpdateMap()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				this.state = GameStates.DASHBOARD;
			}
			this.hoverText = string.Empty;
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + this.camera.Position.X), (int)((float)mouseState.Y + this.camera.Position.Y), 5, 5);
			this.camera.Update(this.mapCenterPosition);
			if (mouseState.X > this.graphics.GraphicsDevice.Viewport.Width - 50)
			{
				this.mapCenterPosition.X += 5f;
			}
			else if ((mouseState.X >= 50 ? false : this.mapCenterPosition.X > 0f))
			{
				this.mapCenterPosition.X -= 5f;
			}
			if (mouseState.Y > this.graphics.GraphicsDevice.Viewport.Height - 50)
			{
				this.mapCenterPosition.Y += 5f;
			}
			else if ((mouseState.Y >= 50 ? false : this.mapCenterPosition.Y > 0f))
			{
				this.mapCenterPosition.Y -= 5f;
			}
			foreach (MapEntry entry in this.mapEntries)
			{
				if (mouseRectangle.Intersects(new Rectangle((int)entry.Position.X * 64, (int)entry.Position.Y * 64, 64, 64)))
				{
					this.hoverText = entry.GetInfo();
				}
			}
		}

		private void UpdateResources()
		{
			int x = Mouse.GetState().X;
			MouseState state = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle(x, state.Y, 5, 5);
			this.menuButton.Update(mouseRectangle, 1f, 1f);
			this.stoneDownButton.Update(mouseRectangle, 1f, 1f);
			this.stoneUpButton.Update(mouseRectangle, 1f, 1f);
			this.ironDownButton.Update(mouseRectangle, 1f, 1f);
			this.ironUpbutton.Update(mouseRectangle, 1f, 1f);
			this.woodDownButton.Update(mouseRectangle, 1f, 1f);
			this.woodUpButton.Update(mouseRectangle, 1f, 1f);
			if (this.menuButton.IsActivated)
			{
				this.state = GameStates.KINGDOMSCREEN;
				this.canClick = false;
				this.menuButton = new Button(new Rectangle(150, 325, buttonTexture.Width, buttonTexture.Height), this.buttonTexture, Content);
			}
			if (this.stoneDownButton.IsActivated)
			{
				if (this.playerKingdom.StoneWorkers > 0)
				{
					Kingdom stoneWorkers = this.playerKingdom;
					stoneWorkers.StoneWorkers = stoneWorkers.StoneWorkers - 1;
					Kingdom unusedWorkers = this.playerKingdom;
					unusedWorkers.UnusedWorkers = unusedWorkers.UnusedWorkers + 1;
				}
			}
			if (this.stoneUpButton.IsActivated)
			{
				if (this.playerKingdom.UnusedWorkers > 0)
				{
					Kingdom kingdom = this.playerKingdom;
					kingdom.StoneWorkers = kingdom.StoneWorkers + 1;
					Kingdom unusedWorkers1 = this.playerKingdom;
					unusedWorkers1.UnusedWorkers = unusedWorkers1.UnusedWorkers - 1;
				}
			}
			if (this.ironDownButton.IsActivated)
			{
				if (this.playerKingdom.IronWorkers > 0)
				{
					Kingdom ironWorkers = this.playerKingdom;
					ironWorkers.IronWorkers = ironWorkers.IronWorkers - 1;
					Kingdom kingdom1 = this.playerKingdom;
					kingdom1.UnusedWorkers = kingdom1.UnusedWorkers + 1;
				}
			}
			if (this.ironUpbutton.IsActivated)
			{
				if (this.playerKingdom.UnusedWorkers > 0)
				{
					Kingdom ironWorkers1 = this.playerKingdom;
					ironWorkers1.IronWorkers = ironWorkers1.IronWorkers + 1;
					Kingdom unusedWorkers2 = this.playerKingdom;
					unusedWorkers2.UnusedWorkers = unusedWorkers2.UnusedWorkers - 1;
				}
			}
			if (this.woodDownButton.IsActivated)
			{
				if (this.playerKingdom.WoodWorkers > 0)
				{
					Kingdom woodWorkers = this.playerKingdom;
					woodWorkers.WoodWorkers = woodWorkers.WoodWorkers - 1;
					Kingdom kingdom2 = this.playerKingdom;
					kingdom2.UnusedWorkers = kingdom2.UnusedWorkers + 1;
				}
			}
			if (this.woodUpButton.IsActivated)
			{
				if (this.playerKingdom.UnusedWorkers > 0)
				{
					Kingdom woodWorkers1 = this.playerKingdom;
					woodWorkers1.WoodWorkers = woodWorkers1.WoodWorkers + 1;
					Kingdom unusedWorkers3 = this.playerKingdom;
					unusedWorkers3.UnusedWorkers = unusedWorkers3.UnusedWorkers - 1;
				}
			}
		}

		private void UpdateSoldierBuying()
		{
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + this.camera.Position.X), (int)((float)mouseState.Y + this.camera.Position.Y), 5, 5);
			this.buySwordsmanButton.Update(mouseRectangle, 1f, 1f);
			this.hoverText = string.Empty;
			if (this.buySwordsmanButton.IsHovered)
			{
				this.hoverText = "Swordsman\nCost: 5";
				if ((!this.buySwordsmanButton.IsActivated ? false : this.playerKingdom.Gold >= 5))
				{
					Kingdom gold = this.playerKingdom;
					gold.Gold = gold.Gold - 5;
					this.playerKingdom.Soldiers.Add(new Swordsman(Content));
					StreamWriter writer = new StreamWriter((new TcpClient("127.0.0.1", 1303)).GetStream());
					string messageString = string.Concat("8 ", this.playerKingdom.Name, " Swordsman");
					byte[] message = Encoding.ASCII.GetBytes(messageString);
					writer.BaseStream.Write(message, 0, (int)message.Length);
					writer.Flush();
				}
			}
			if ((mouseRectangle.Intersects(new Rectangle((int)(85f + this.camera.Position.X), (int)(150f + this.camera.Position.Y), this.buySoldiersTexture.Width, this.buySoldiersTexture.Height)) && mouseState.LeftButton == ButtonState.Pressed && canClick))
			{
				this.state = GameStates.MANAGEMENT;
				this.canClick = false;
			}
		}

		private void UpdateWilderness(GameTime gameTime)
		{
			this.wildernessPlayer.Update(gameTime, this.destroyableRects);
			this.camera.Update(this.wildernessPlayer.Position);
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + this.camera.Position.X), (int)((float)mouseState.Y + this.camera.Position.Y), 5, 5);
			if (Main.random.Next(0, 15) == 0)
			{
				Tree toBeAdded = new Tree(Content, this.destroyableRects);
				this.destroyableRects.Add(toBeAdded.Rectangle);
				this.wildernessDestroyables.Add(toBeAdded);
			}
			if (Main.random.Next(0, 25) == 0)
			{
				Quarry toBeAdded = new Quarry(Content, this.destroyableRects);
				this.destroyableRects.Add(toBeAdded.Rectangle);
				this.wildernessDestroyables.Add(toBeAdded);
			}
			if (Main.random.Next(0, 35) == 0)
			{
				IronOre toBeAdded = new IronOre(Content, this.destroyableRects);
				this.destroyableRects.Add(toBeAdded.Rectangle);
				this.wildernessDestroyables.Add(toBeAdded);
			}
			this.mouseTexture = null;
			for (int i = 0; i < this.wildernessDestroyables.Count; i++)
			{
				if (this.wildernessDestroyables[i].Rectangle.Intersects(mouseRectangle))
				{
					if (this.wildernessDestroyables[i] is Tree)
					{
						if (this.axeTexture == null)
						{
							this.axeTexture = Content.Load<Texture2D>("axe");
						}
						this.mouseTexture = this.axeTexture;
						if ((mouseState.LeftButton == ButtonState.Pressed &&  this.wildernessPlayer.CanDamage))
						{
							this.wildernessPlayer.CanDamage = false;
							Destroyable health = this.wildernessDestroyables[i];
							health.Health = health.Health - 1;
							for (int x = 0; x < 5; x++)
							{
								this.particleManager.AddParticle(new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.BurlyWood, 1.2f, 300f);
							}
							this.breakEffect.Play();
							if (this.wildernessDestroyables[i].Health <= 0)
							{
								this.wildernessPlayer.Inventory.Add(this.wildernessDestroyables[i].Item);
								this.wildernessDestroyables.RemoveAt(i);
								i--;
							}
						}
					}
					else if (this.wildernessDestroyables[i] is Quarry)
					{
						if (this.pickaxeTexture == null)
						{
							this.pickaxeTexture = Content.Load<Texture2D>("pickaxe");
						}
						this.mouseTexture = this.pickaxeTexture;
						if ((mouseState.LeftButton == ButtonState.Pressed && this.wildernessPlayer.CanDamage))
						{
							this.wildernessPlayer.CanDamage = false;
							Destroyable destroyable = this.wildernessDestroyables[i];
							destroyable.Health = destroyable.Health - 1;
							for (int x = 0; x < 5; x++)
							{
								this.particleManager.AddParticle(new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.DarkSlateGray, 1.2f, 300f);
							}
							this.breakEffect.Play();
							if (this.wildernessDestroyables[i].Health <= 0)
							{
								this.wildernessPlayer.Inventory.Add(this.wildernessDestroyables[i].Item);
								this.wildernessDestroyables.RemoveAt(i);
								i--;
							}
						}
					}
					else if (this.wildernessDestroyables[i] is IronOre)
					{
						if (this.pickaxeTexture == null)
						{
							this.pickaxeTexture = Content.Load<Texture2D>("pickaxe");
						}
						this.mouseTexture = this.pickaxeTexture;
						if ((mouseState.LeftButton == ButtonState.Pressed && this.wildernessPlayer.CanDamage))
						{
							this.wildernessPlayer.CanDamage = false;
							Destroyable item1 = this.wildernessDestroyables[i];
							item1.Health = item1.Health - 1;
							for (int x = 0; x < 5; x++)
							{
								this.particleManager.AddParticle(new Vector2((float)mouseState.X + this.camera.Position.X, (float)mouseState.Y + this.camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.DarkSlateGray, 1.2f, 300f);
							}
							this.breakEffect.Play();
							if (this.wildernessDestroyables[i].Health <= 0)
							{
								this.wildernessPlayer.Inventory.Add(this.wildernessDestroyables[i].Item);
								this.wildernessDestroyables.RemoveAt(i);
								i--;
							}
						}
					}
				}
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				this.state = GameStates.KINGDOMSCREEN;
				Kingdom newKingdom = new Kingdom(this.playerKingdom.Name, this.playerKingdom.Owner);
				foreach (InventoryItem item in this.wildernessPlayer.Inventory)
				{
					if (item is Wood)
					{
						Kingdom wood = newKingdom;
						wood.Wood = wood.Wood + 1000f;
					}
					else if (item is Stone)
					{
						Kingdom stone = newKingdom;
						stone.Stone = stone.Stone + 1000f;
					}
					else if (item is Iron)
					{
						Kingdom iron = newKingdom;
						iron.Iron = iron.Iron + 1000f;
					}
				}
				StreamWriter writer = new StreamWriter((new TcpClient("127.0.0.1", 1303)).GetStream());
				string messageString = string.Concat(new object[] { "2 ", newKingdom.Name, " ", newKingdom.Stone, " ", newKingdom.Iron, " ", newKingdom.Wood, " ", newKingdom.Gold });
				byte[] message = Encoding.ASCII.GetBytes(messageString);
				writer.BaseStream.Write(message, 0, (int)message.Length);
			}
			this.particleManager.Update(gameTime);
		}
	}
}