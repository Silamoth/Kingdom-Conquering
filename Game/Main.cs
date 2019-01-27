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

        KingdomPlayer kingdomPlayer;
        ResourceBuyer resourceBuyer;

        Vector2 baseScreenSize = new Vector2(800, 480);
        float scaleX, scaleY;

        int oldWidth, oldHeight;

        static Main()
		{
			Main.random = new Random();
		}

		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Draw(GameTime gameTime)
		{
			Matrix? nullable;
			if ((state == GameStates.WILDERNESS || state == GameStates.MAP || state == GameStates.MANAGEMENT ? false : state != GameStates.BUYSOLDIERS))
			{
				nullable = null;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, nullable);
			}
			else
			{
				spriteBatch.Begin(0, null, null, null, null, null, new Matrix?(camera.ViewMatrix));
			}
			if (state == GameStates.DASHBOARD)
			{
				DrawDashboard();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
			else if (state == GameStates.RESOURCES)
			{
				DrawResourcesMenu();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
			else if (state == GameStates.WILDERNESS)
			{				
                GraphicsDevice.Clear(Color.DarkBlue);
                DrawWilderness();
            }
			else if (state == GameStates.MAP)
			{
				spriteBatch.End();
				nullable = null;
				spriteBatch.Begin(0, null, null, null, null, null, nullable);
				for (int x = 0; x < 13; x++)
				{
					for (int y = 0; y < 8; y++)
					{
						spriteBatch.Draw(grassPatch, new Vector2((float)(x * 64), (float)(y * 64)), Color.White);
					}
				}
				spriteBatch.End();
				spriteBatch.Begin(0, null, null, null, null, null, new Matrix?(camera.ViewMatrix));
				DrawMap();
				GraphicsDevice.Clear(Color.ForestGreen);
			}
			else if (state == GameStates.MANAGEMENT)
			{
				DrawManagement();
				GraphicsDevice.Clear(Color.White);
			}
			else if (state == GameStates.BUYSOLDIERS)
			{
				DrawSoliderBuying();
				GraphicsDevice.Clear(Color.White);
			}
			else if (state == GameStates.KINGDOMSCREEN)
			{
				DrawKingdomScreen();
				GraphicsDevice.Clear(Color.BlanchedAlmond);
			}
            else if (state == GameStates.AIKINGDOM)
            {
                DrawAIKingdom();
                GraphicsDevice.Clear(Color.BlanchedAlmond);
            }

            spriteBatch.End();
			base.Draw(gameTime);
		}

		private void DrawDashboard()
		{
			spriteBatch.Draw(menuTexture, new Rectangle(0, 0, (int)(baseScreenSize.X * scaleX), (int)(baseScreenSize.Y * scaleY)), Color.White);

			mapButton.Draw(spriteBatch, scaleX, scaleY);
			//spriteBatch.DrawString(font, "Map", new Vector2((float)(mapButton.Rectangle.X + 29) * scaleX, (float)(mapButton.Rectangle.Y + 5) * scaleY), Color.Black);
            spriteBatch.DrawString(font, "Map", new Vector2((mapButton.Rectangle.X + 29) * scaleX, (mapButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            wildernessButton.Draw(spriteBatch, scaleX, scaleY);
            //spriteBatch.DrawString(font, "Wilderness", new Vector2((float)(wildernessButton.Rectangle.X + 13) * scaleX, (float)(wildernessButton.Rectangle.Y + 5) * scaleY), Color.Black);
            spriteBatch.DrawString(font, "Wilderness", new Vector2((wildernessButton.Rectangle.X + 13) * scaleX, (wildernessButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            if (player.HasKingdom)
            {
                kingdomButton.Draw(spriteBatch, scaleX, scaleY);
                //spriteBatch.DrawString(font, "Kingdom", new Vector2((float)(kingdomButton.Rectangle.X + 15) * scaleX, (float)(kingdomButton.Rectangle.Y + 5) * scaleY), Color.Black);
                spriteBatch.DrawString(font, "Kingdom", new Vector2((kingdomButton.Rectangle.X + 15) * scaleX, (kingdomButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            else
            {
                kingdomButton.Draw(spriteBatch, scaleX, scaleY);
                //spriteBatch.DrawString(font, "Unavailable", new Vector2((float)(kingdomButton.Rectangle.X + 5) * scaleX, (float)(kingdomButton.Rectangle.Y + 5) * scaleY), Color.Black);
                spriteBatch.DrawString(font, "Unavailable", new Vector2((kingdomButton.Rectangle.X + 5) * scaleX, (kingdomButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }

            DrawResourceDisplay();
        }

		private void DrawKingdomScreen()
		{
            spriteBatch.Draw(menuTexture, new Rectangle(0, 0, (int)(baseScreenSize.X * scaleX), (int)(baseScreenSize.Y * scaleY)), Color.White);

            //spriteBatch.DrawString(largeFont, playerKingdom.Name, new Vector2(275f, 25f), Color.Black);
			resourceButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(font, "Resources", new Vector2((resourceButton.Rectangle.X + 13) * scaleX, (resourceButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            manageButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(font, "Buildings", new Vector2((manageButton.Rectangle.X + 15) * scaleX, (manageButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            menuButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(font, "Menu", new Vector2((menuButton.Rectangle.X + 30) * scaleX, (menuButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            DrawResourceDisplay();
		}

		private void DrawManagement()
		{
			for (int i = 0; i < (int)playerKingdom.Buildings.Length; i++)
			{
				for (int x = 0; x < (int)playerKingdom.Buildings.Length; x++)
				{
					spriteBatch.Draw(playerKingdom.Buildings[i][x].GetTexture(), new Vector2((float)(i * 64), (float)(x * 64)), Color.White);
				}
			}
			spriteBatch.DrawString(font, "Hit SPACE To Return", new Vector2(325f + camera.Position.X, 25f + camera.Position.Y), Color.Black);
			if (buildMouseState == BuildMouseState.LIST)
			{
				Mouse.GetState();
				int count = 0;
				foreach (IBuilding buildable in buildables)
				{
					spriteBatch.Draw(buildMenuTexture, new Vector2(buildMenuPosition.X, buildMenuPosition.Y + (float)(count * 32)), Color.White);
					spriteBatch.DrawString(smallFont, buildable.GetName(), new Vector2(buildMenuPosition.X + 5, buildMenuPosition.Y + (float)(count * 32) + 10f), Color.Black);
					count++;
				}
			}
			if (hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				spriteBatch.Draw(hoverTexture, new Vector2((float)mouseState.X + camera.Position.X, (float)mouseState.Y + camera.Position.Y), Color.Blue);
				spriteBatch.DrawString(smallFont, hoverText, new Vector2((float)mouseState.X + camera.Position.X + 10f, (float)mouseState.Y + camera.Position.Y + 10f), Color.White);
			}
		}

		private void DrawMap()
		{
            for (int x = (int)((mapCenterPosition.X / (64 * scaleX) - 8) * 1); x < (int)((mapCenterPosition.X / (64 * scaleX) + 12) * 1); x++)
            {
                for (int y = (int)((mapCenterPosition.Y / (64 * scaleY) - 8) * 1); y < (int)((mapCenterPosition.Y / (64 * scaleY) + 8) * 1); y++)
                {
                    spriteBatch.Draw(grassPatch, new Rectangle((int)(x * 64 * scaleX), (int)(y * 64 * scaleY), (int)(grassPatch.Width * scaleX), (int)(grassPatch.Height * scaleY)), Color.White);
                }
            }

			foreach (MapEntry entry in mapEntries)
			{
				Vector2 position = entry.Position * 64f;
				if (Vector2.Distance(camera.Position, position) < 1000f)
				{
					spriteBatch.Draw(kingdomIcon, new Rectangle((int)(position.X * scaleX), (int)(position.Y * scaleY), (int)(kingdomIcon.Width * scaleX), (int)(kingdomIcon.Height * scaleY)), Color.White);
				}
			}
			if (hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				spriteBatch.Draw(hoverTexture, new Rectangle((int)((mouseState.X + camera.Position.X) * 1), (int)((mouseState.Y + camera.Position.Y) * 1), (int)(hoverTexture.Width * scaleX), (int)(hoverTexture.Height * scaleY)), Color.Blue);
				spriteBatch.DrawString(font, hoverText, new Vector2((float)mouseState.X + camera.Position.X + 10f, (float)mouseState.Y + camera.Position.Y + 10f), Color.White);
			}
			spriteBatch.DrawString(font, "Hit SPACE To Return", new Vector2(325f + camera.Position.X, 25f + camera.Position.Y), Color.Black);

            player.Draw(spriteBatch, scaleX, scaleY);
		}

		private void DrawResourceDisplay()
		{
			if (player.Gold >= 1000000)
			{
				SpriteFont spriteFont = font;
				int gold = player.Gold / 1000;
                spriteBatch.DrawString(font, "Gold: " + gold.ToString(), new Vector2(150 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            else
			{
				SpriteFont spriteFont1 = font;
				int gold = player.Gold;
                spriteBatch.DrawString(font, "Gold: " + gold.ToString(), new Vector2(150 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
			if (player.Iron >= 1000000f)
			{
				SpriteFont spriteFont2 = font;
				float iron = player.Iron / 1000f;
                spriteBatch.DrawString(font, "Iron: " + iron.ToString(), new Vector2(425 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
			else
			{
				SpriteFont spriteFont3 = font;
				float iron = player.Iron;
                spriteBatch.DrawString(font, "Iron: " + iron.ToString(), new Vector2(425 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            if (player.Stone >= 1000000f)
			{
				SpriteFont spriteFont4 = font;
				float stone = player.Stone / 1000f;
                spriteBatch.DrawString(font, "Stone: " + stone.ToString(), new Vector2(275 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            else
			{
				SpriteFont spriteFont5 = font;
				float stone = player.Stone;
                spriteBatch.DrawString(font, "Stone: " + stone.ToString(), new Vector2(275 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            if (player.Wood >= 1000000f)
			{
				SpriteBatch spriteBatch6 = spriteBatch;
				SpriteFont spriteFont6 = font;
				float wood = player.Wood / 1000f;
                spriteBatch.DrawString(font, "Wood: " + wood.ToString(), new Vector2(550 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
            else
			{
				SpriteBatch spriteBatch7 = spriteBatch;
				SpriteFont spriteFont7 = font;
				float wood = player.Wood;
                spriteBatch.DrawString(font, "Wood: " + wood.ToString(), new Vector2(550 * scaleX, 450 * scaleY), Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            }
        }

		private void DrawResourcesMenu()
		{
			spriteBatch.DrawString(midLargeFont, "Redistribute Workers", new Vector2(250 * scaleX, 35 * scaleY), Color.Black);

			menuButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(font, "Back: ", new Vector2((menuButton.Rectangle.X + 30) * scaleX, (menuButton.Rectangle.Y + 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            stoneDownButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(mediumFont, "Stone: ", new Vector2((stoneDownButton.Rectangle.X + 60) * scaleX, (stoneDownButton.Rectangle.Y - 25) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);                   
            spriteBatch.DrawString(largeFont, playerKingdom.StoneWorkers.ToString(), new Vector2((stoneDownButton.Rectangle.X + 60) * scaleX, (stoneDownButton.Rectangle.Y - 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            stoneUpButton.Draw(spriteBatch, scaleX, scaleY);

			ironDownButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(mediumFont, "Iron: ", new Vector2((ironDownButton.Rectangle.X + 70) * scaleX, (ironDownButton.Rectangle.Y - 25) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            spriteBatch.DrawString(largeFont, playerKingdom.IronWorkers.ToString(), new Vector2((ironDownButton.Rectangle.X + 60) * scaleX, (ironDownButton.Rectangle.Y - 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            ironUpbutton.Draw(spriteBatch, scaleX, scaleY);

			woodDownButton.Draw(spriteBatch, scaleX, scaleY);
            spriteBatch.DrawString(mediumFont, "Wood: ", new Vector2((woodDownButton.Rectangle.X + 70) * scaleX, (woodDownButton.Rectangle.Y - 25) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            spriteBatch.DrawString(largeFont, playerKingdom.WoodWorkers.ToString(), new Vector2((woodDownButton.Rectangle.X + 60) * scaleX, (woodDownButton.Rectangle.Y - 5) * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
            woodUpButton.Draw(spriteBatch, scaleX, scaleY);

			DrawResourceDisplay();

            spriteBatch.DrawString(mediumFont, "Total Workers: " + playerKingdom.TotalWorkers.ToString(), new Vector2(100 * scaleX, 325 * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);

            spriteBatch.DrawString(mediumFont, "Unused Workers: " + playerKingdom.UnusedWorkers.ToString(), new Vector2(475 * scaleX, 325 * scaleY), Color.Black, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
        }

        private void DrawSoliderBuying()
		{
			DrawManagement();
			spriteBatch.Draw(buySoldiersTexture, new Vector2(85f + camera.Position.X, 150f + camera.Position.Y), Color.White);
			buySwordsmanButton.Draw(spriteBatch, scaleX, scaleY);
			spriteBatch.DrawString(font, "Swordsman", new Vector2((float)(buySwordsmanButton.Rectangle.X + 5), (float)(buySwordsmanButton.Rectangle.Y + 5)), Color.Black);
			if (hoverText != string.Empty)
			{
				MouseState mouseState = Mouse.GetState();
				spriteBatch.Draw(hoverTexture, new Vector2((float)mouseState.X + camera.Position.X, (float)mouseState.Y + camera.Position.Y), Color.Blue);
				spriteBatch.DrawString(smallFont, hoverText, new Vector2((float)mouseState.X + camera.Position.X + 10f, (float)mouseState.Y + camera.Position.Y + 10f), Color.White);
			}
		}

		private void DrawWilderness()
		{
			for (int x = 0; x < wildernessGrass.Count; x++)
			{
				for (int y = 0; y < wildernessGrass[x].Count; y++)
				{
					int index = wildernessGrass[x][y];
					Vector2 position = new Vector2((float)(x * 64 * scaleX), (float)(y * 64 * scaleY));
					if (Vector2.Distance(position, wildernessPlayer.Position) < 600 * scaleX * scaleY)
					{
						switch (index)
						{
							case 0:
							{
								spriteBatch.Draw(grassOne, new Rectangle((int)position.X, (int)position.Y, (int)(65 * scaleX), (int)(65 * scaleY)), Color.White);
								break;
							}
							case 1:
							{
                                spriteBatch.Draw(grassTwo, new Rectangle((int)position.X, (int)position.Y, (int)(65 * scaleX), (int)(65 * scaleY)), Color.White);
                                break;
							}
							case 2:
							{
                                spriteBatch.Draw(grassThree, new Rectangle((int)position.X, (int)position.Y, (int)(65 * scaleX), (int)(65 * scaleY)), Color.White);
                                break;
							}
						}
					}
				}
			}

			foreach (Destroyable thing in wildernessDestroyables)
			{
				if (Vector2.Distance(new Vector2((float)thing.Rectangle.X, (float)thing.Rectangle.Y), wildernessPlayer.Position) < 500f)
				{
					thing.Draw(spriteBatch, scaleX, scaleY);
				}
			}

			wildernessPlayer.Draw(spriteBatch);
            resourceBuyer.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
            resourceBuyer.DrawMenu(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.ViewMatrix);

            if (mouseTexture != null)
			{
				Texture2D texture2D = mouseTexture;
				MouseState state = Mouse.GetState();
				float single = (float)state.X + camera.Position.X;
				state = Mouse.GetState();
				Rectangle? nullable = null;
				spriteBatch.Draw(texture2D, new Vector2(single, (float)state.Y + camera.Position.Y), nullable, Color.White, 0f, Vector2.Zero, 2f, 0, 1f);
			}

			spriteBatch.DrawString(font, "Hit SPACE To Return", new Vector2(325f + camera.Position.X, 25f + camera.Position.Y), Color.Black);

			particleManager.Draw(spriteBatch);
		}

		protected override void Initialize()
        {	
		    IsMouseVisible = true;
            Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            //graphics.IsFullScreen = true;

            oldWidth = Window.ClientBounds.Width;
            oldHeight = Window.ClientBounds.Height;

            state = GameStates.DASHBOARD;
			buttonTexture = Content.Load<Texture2D>("birchButton");
			upButtonTexture = Content.Load<Texture2D>("upButton");
			downButtonTexture = Content.Load<Texture2D>("downButton");
			menuTexture = Content.Load<Texture2D>("menuBackground");
			mapButton = new Button(new Rectangle(150, 175, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
			kingdomButton = new Button(new Rectangle(150, 250, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
            wildernessButton = new Button(new Rectangle(150, 250 + (250 - 175), buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);

            String[] args = Environment.GetCommandLineArgs();

            //String playerName = args[0];
            //player = new Player(Content, playerName);

            player = new Player(Content, "Player");

			canClick = true;
			clickTimer = 0;
			listener = new TcpListener(1303);

            TcpClient client = new TcpClient("127.0.0.1", 1303);
            StreamWriter writer = new StreamWriter(client.GetStream());
            string messageString = string.Concat("9 ", player.Name);
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            writer.BaseStream.Write(message, 0, (int)message.Length);

            byte[] buffer = new byte[client.Client.ReceiveBufferSize];
            client.Client.Receive(buffer);
            String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });
            List<String> splitResponse = response.Split(new char[] { ' ' }).ToList<String>();

            bool hasKingdom;
            int mapX, mapY, gold;
            float stone, iron, wood;

            bool.TryParse(splitResponse[1], out hasKingdom);
            int.TryParse(splitResponse[2], out mapX);
            int.TryParse(splitResponse[3], out mapY);
            int.TryParse(splitResponse[4], out gold);
            float.TryParse(splitResponse[5], out stone);
            float.TryParse(splitResponse[6], out iron);
            float.TryParse(splitResponse[7], out wood);

            player.HasKingdom = hasKingdom;
            player.CurrentPosition = new Vector2(mapX, mapY);
            player.Gold = gold;
            player.Stone = stone;
            player.Iron = iron;
            player.Wood = wood;

            if (player.HasKingdom)
            {
                player.KingdomName = splitResponse[8];
                playerKingdom = new Kingdom(player.KingdomName, player.Name);

                client = new TcpClient("127.0.0.1", 1303);
                writer = new StreamWriter(client.GetStream());
                messageString = string.Concat("1 ", playerKingdom.Name);
                message = Encoding.ASCII.GetBytes(messageString);
                writer.BaseStream.Write(message, 0, (int)message.Length);
                buffer = new byte[client.Client.ReceiveBufferSize];
                client.Client.Receive(buffer);
                response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });
                splitResponse = response.Split(new char[] { ' ' }).ToList<string>();                

                if (splitResponse[0] == "1")
                {
                    int stoneWorkers;
                    int ironWorkers;
                    int woodWorkers;
                    int totalWorkers;
                    int unusedWorkers;

                    int.TryParse(splitResponse[1], out stoneWorkers);
                    int.TryParse(splitResponse[2], out ironWorkers);
                    int.TryParse(splitResponse[3], out woodWorkers);
                    int.TryParse(splitResponse[4], out totalWorkers);
                    int.TryParse(splitResponse[5], out unusedWorkers);
                    playerKingdom.StoneWorkers = stoneWorkers;
                    playerKingdom.IronWorkers = ironWorkers;
                    playerKingdom.WoodWorkers = woodWorkers;
                    playerKingdom.TotalWorkers = totalWorkers;
                    playerKingdom.UnusedWorkers = unusedWorkers;
                }
            }
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("font");
			midLargeFont = Content.Load<SpriteFont>("midLargeFont");
			mediumFont = Content.Load<SpriteFont>("mediumFont");
			largeFont = Content.Load<SpriteFont>("largeFont");
			smallFont = Content.Load<SpriteFont>("smallFont");
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
		}

		protected override void UnloadContent()
		{
            Content.Unload();
		}

		protected override void Update(GameTime gameTime)
		{
            scaleX = Window.ClientBounds.Width / baseScreenSize.X;
            scaleY = Window.ClientBounds.Height / baseScreenSize.Y;
            //Console.WriteLine(GraphicsDevice.Viewport.X + ", " + GraphicsDevice.Viewport.Y + ", " + GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height);

            //graphics.GraphicsDevice.Viewport = new Viewport(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, Window.ClientBounds.Width, Window.ClientBounds.Height,
                //GraphicsDevice.Viewport.MinDepth, GraphicsDevice.Viewport.MaxDepth);

            if (Window.ClientBounds.Width != oldWidth || Window.ClientBounds.Height != oldHeight)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }

            oldWidth = Window.ClientBounds.Width;
            oldHeight = Window.ClientBounds.Height;

            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X * 1, mouseState.Y * 1);

            if (state == GameStates.DASHBOARD)
            {
                UpdateDashboard(new Rectangle(mouseState.X, mouseState.Y, 15, 9));
            }
            else if (state == GameStates.RESOURCES)
            {
                UpdateResources(new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1));
            }
            else if (state == GameStates.WILDERNESS)
            {
                UpdateWilderness(gameTime);
            }
            else if (state == GameStates.MAP)
            {
                UpdateMap(new Rectangle((int)((mouseState.X + camera.Position.X) * 1), (int)((mouseState.Y + camera.Position.Y) * 1), (int)scaleX, (int)scaleY), mousePosition, mouseState);
            }
            else if (state == GameStates.MANAGEMENT)
            {
                UpdateManagement(mousePosition, new Rectangle((int)mousePosition.X + (int)camera.Position.X, (int)mousePosition.Y + (int)camera.Position.Y, 1, 1), mouseState);
            }
            else if (state == GameStates.BUYSOLDIERS)
            {
                UpdateSoldierBuying(new Rectangle((int)mousePosition.X + (int)camera.Position.X, (int)mousePosition.Y + (int)camera.Position.Y, 1, 1), mouseState);
            }
            else if (state == GameStates.KINGDOMSCREEN)
            {
                UpdateKingdomScreen(new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1));
            }
            else if (state == GameStates.AIKINGDOM)
                UpdateAIKingdom(gameTime);

			if (!canClick)
			{
				clickTimer++;
				if (clickTimer == 20)
				{
					canClick = true;
					clickTimer = 0;
				}
			}

            if (player.HasKingdom)
            {
                TcpClient client = new TcpClient("127.0.0.1", 1303);
                StreamWriter writer = new StreamWriter(client.GetStream());
                String messageString = string.Concat(new object[] { "0 ", playerKingdom.Name, " ", playerKingdom.StoneWorkers, " ", playerKingdom.IronWorkers, " ", playerKingdom.WoodWorkers, " ", playerKingdom.TotalWorkers, " ", playerKingdom.UnusedWorkers, " ", playerKingdom.SoldierMax });
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                writer.BaseStream.Write(message, 0, (int)message.Length);
                byte[] buffer = new byte[client.Client.ReceiveBufferSize];
                client.Client.Receive(buffer);
                string response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) });
                List<string> splitResponse = response.Split(new char[] { ' ' }).ToList<string>();
                string id = splitResponse[0];
                float stone;
                float iron;
                float wood;
                int gold;

                float.TryParse(splitResponse[1], out stone);
                float.TryParse(splitResponse[2], out iron);
                float.TryParse(splitResponse[3], out wood);
                int.TryParse(splitResponse[4], out gold);
                player.Stone = stone;
                player.Iron = iron;
                player.Wood = wood;
                player.Gold = gold;
            }

            base.Update(gameTime);
		}

		private void UpdateDashboard(Rectangle mouseRectangle)
		{            
			mapButton.Update(mouseRectangle, scaleX, scaleY);
			kingdomButton.Update(mouseRectangle, scaleX, scaleY);
            wildernessButton.Update(mouseRectangle, scaleX, scaleY);

            if (mapButton.IsActivated && canClick)
			{
				canClick = false;
				state = GameStates.MAP;
				kingdomIcon = Content.Load<Texture2D>("kingdomMapIcon");
				hoverTexture = Content.Load<Texture2D>("hoverButton");
				hoverText = string.Empty;
				grassPatch = Content.Load<Texture2D>("grassPatch");
				camera = new Camera();
				mapCenterPosition = new Vector2(400f, 240f);
				mapEntries = new List<MapEntry>();

				TcpClient client = new TcpClient("127.0.0.1", 1303);
				StreamWriter writer = new StreamWriter(client.GetStream());
                string messageString = "5 " + player.Name;
				byte[] message = Encoding.ASCII.GetBytes(messageString);
				writer.BaseStream.Write(message, 0, (int)message.Length);
				byte[] buffer = new byte[client.Client.ReceiveBufferSize];
				client.Client.Receive(buffer);
				List<string> splitResponse = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) }).Trim().Split(new char[] { ' ' }).ToList<string>();
				if (splitResponse[0] == "5")
				{
					for (int i = 1; i < splitResponse.Count; i++)
					{
                        int x, y;

						string[] splitEntry = splitResponse[i].Split(new char[] { '/' });
						string name = splitEntry[0];
						MapType type = (MapType)Enum.Parse(typeof(MapType), splitEntry[1]);
						int.TryParse(splitEntry[2], out x);
						int.TryParse(splitEntry[3], out y);
						mapEntries.Add(new MapEntry(new Vector2((float)x, (float)y), type, name));
					}
				}

                player.TargetPosition = player.CurrentPosition;
                mapCenterPosition = player.CurrentPosition * 64;
            }

			if (kingdomButton.IsActivated && canClick && player.HasKingdom)
			{
				canClick = false;
				manageButton = new Button(new Rectangle(150, 175, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
				resourceButton = new Button(new Rectangle(150, 250, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
				menuButton = new Button(new Rectangle(150, 250 + (250 - 175), buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
				state = GameStates.KINGDOMSCREEN;
			}

            if (wildernessButton.IsActivated && canClick)
            {
                canClick = false;
                state = GameStates.WILDERNESS;

                wildernessPlayer = new WildernessPlayer(Content);
                resourceBuyer = new ResourceBuyer(Content, wildernessPlayer.Position);

                camera = new Camera();
                wildernessDestroyables = new List<Destroyable>();
                destroyableRects = new List<Rectangle>();

                if (particleManager == null)
                {
                    particleManager = new ParticleManager(Content);
                }
                if (grassOne == null)
                {
                    grassOne = Content.Load<Texture2D>("grassOne");
                    grassTwo = Content.Load<Texture2D>("grassTwo");
                    grassThree = Content.Load<Texture2D>("grassThree");
                    breakEffect = Content.Load<SoundEffect>("break");
                }
                wildernessGrass = new List<List<int>>();
                for (int i = 0; i < 156; i++)
                {
                    wildernessGrass.Add(new List<int>());
                }
                foreach (List<int> list in wildernessGrass)
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
                                Tree toBeAddedTree = new Tree(Content, destroyableRects);
                                destroyableRects.Add(toBeAddedTree.Rectangle);
                                wildernessDestroyables.Add(toBeAddedTree);
                                break;
                            }
                        case 1:
                            {
                                Quarry toBeAddedQuarry = new Quarry(Content, destroyableRects);
                                destroyableRects.Add(toBeAddedQuarry.Rectangle);
                                wildernessDestroyables.Add(toBeAddedQuarry);
                                break;
                            }
                        case 2:
                            {
                                IronOre toBeAddedIronOre = new IronOre(Content, destroyableRects);
                                destroyableRects.Add(toBeAddedIronOre.Rectangle);
                                wildernessDestroyables.Add(toBeAddedIronOre);
                                break;
                            }
                    }
                }
            }
        }

		private void UpdateKingdomScreen(Rectangle mouseRectangle)
		{
			resourceButton.Update(mouseRectangle, scaleX, scaleY);
			manageButton.Update(mouseRectangle, scaleX, scaleY);
			menuButton.Update(mouseRectangle, scaleX, scaleY);
			
			if (manageButton.IsActivated && canClick)
			{
				canClick = false;
				state = GameStates.MANAGEMENT;
				camera = new Camera();
				mapCenterPosition = new Vector2(300f, 240f);
				hoverTexture = Content.Load<Texture2D>("hoverButton");
				hoverText = string.Empty;
				buildables = new List<IBuilding>()
				{
					new House(Content),
					new Barracks(Content)
				};
				buildMenuTexture = Content.Load<Texture2D>("buildingMenuModular");
				buildMouseState = BuildMouseState.NONE;
				canClick = false;
				clickTimer = 0;
				hoveredI = -1;
				hoveredX = -1;
				TcpClient client = new TcpClient("127.0.0.1", 1303);
				StreamWriter writer = new StreamWriter(client.GetStream());
				string messageString = string.Concat("6 ", playerKingdom.Name);
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
					playerKingdom.Buildings = buildings;
				}
			}
			if ((!resourceButton.IsActivated ? false : canClick))
			{
				canClick = false;
				state = GameStates.RESOURCES;
				stoneDownButton = new Button(new Rectangle(65, 150, downButtonTexture.Width, downButtonTexture.Height), downButtonTexture, Content);
				stoneUpButton = new Button(new Rectangle(190, 150, upButtonTexture.Width, upButtonTexture.Height), upButtonTexture, Content);
				ironDownButton = new Button(new Rectangle(290, 150, downButtonTexture.Width, downButtonTexture.Height), downButtonTexture, Content);
				ironUpbutton = new Button(new Rectangle(415, 150, upButtonTexture.Width, upButtonTexture.Height), upButtonTexture, Content);
				woodDownButton = new Button(new Rectangle(515, 150, downButtonTexture.Width, downButtonTexture.Height), downButtonTexture, Content);
				woodUpButton = new Button(new Rectangle(640, 150, upButtonTexture.Width, upButtonTexture.Height), upButtonTexture, Content);
				menuButton = new Button(new Rectangle(25, 25, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
			}
			if ((!menuButton.IsActivated ? false : canClick))
			{
				state = GameStates.DASHBOARD;
				canClick = false;
			}
		}

		private void UpdateManagement(Vector2 mousePosition, Rectangle mouseRectangle, MouseState mouseState)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				state = GameStates.KINGDOMSCREEN;
			}

			hoverText = string.Empty;
			camera.Update(mapCenterPosition);

			if ((mousePosition.X <= graphics.GraphicsDevice.Viewport.Width - 50 ? false : mapCenterPosition.X < 320f))
			{
				mapCenterPosition.X += 5f;
			}
			else if ((mousePosition.X >= 50 ? false : mapCenterPosition.X > 290f))
			{
				mapCenterPosition.X -= 5f;
			}
			if ((mousePosition.Y <= graphics.GraphicsDevice.Viewport.Height - 50 ? false : mapCenterPosition.Y < 340f))
			{
				mapCenterPosition.Y += 5f;
			}
			else if ((mousePosition.Y >= 50 ? false : mapCenterPosition.Y > 200f))
			{
				mapCenterPosition.Y -= 5f;
			}
			for (int i = 0; i < (int)playerKingdom.Buildings.Length; i++)
			{
				for (int x = 0; x < (int)playerKingdom.Buildings.Length; x++)
				{
					if (mouseRectangle.Intersects(new Rectangle(i * 64, x * 64, 64, 64)) && buildMouseState == BuildMouseState.NONE)
					{
						if (playerKingdom.Buildings[i][x].GetAction() == BuildingActions.BUILDNEW)
						{
							if (buildMouseState != BuildMouseState.LIST)
							{
								hoverText = "An empty patch of grass.\nPrime real estate.\nClick to build.";
								if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
								{
									if (buildMouseState == BuildMouseState.NONE)
									{
										buildMouseState = BuildMouseState.LIST;

									}
									canClick = false;
									hoveredI = i;
									hoveredX = x;
									buildMenuPosition = new Vector2((float)mouseState.X + camera.Position.X + 10f, (float)mouseState.Y + camera.Position.Y);
									hoverText = string.Empty;
								}
							}
						}
						else if (playerKingdom.Buildings[i][x].GetAction() == BuildingActions.INFO)
						{
							hoverText = playerKingdom.Buildings[i][x].GetName();
						}
						else if (playerKingdom.Buildings[i][x].GetAction() == BuildingActions.BUYSOLDIERS)
						{
							hoverText = "Barracks\nClick to buy soldiers";
							if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
							{
								canClick = false;
								state = GameStates.BUYSOLDIERS;
								buySoldiersTexture = Content.Load<Texture2D>("buySoldiersMenu");
								buySwordsmanButton = new Button(new Rectangle((int)(85f + camera.Position.X + 15f), (int)(150f + camera.Position.Y + 15f), buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
							}
						}
                    }
				}
			}
			if (buildMouseState == BuildMouseState.LIST)
			{
				int count = 0;
				foreach (IBuilding buildable in buildables)
				{
					Rectangle menuItemPosition = new Rectangle((int)buildMenuPosition.X, (int)buildMenuPosition.Y + count * 32, 64, 32);
					if (mouseRectangle.Intersects(menuItemPosition))
					{
						int[] resources = buildable.GetResources();
						int stone = resources[0];
						int iron = resources[1];
						int wood = resources[2];
						int gold = resources[3];
						hoverText = string.Concat(new string[] { "Stone: ", stone.ToString(), "\nIron: ", iron.ToString(), "\nWood: ", wood.ToString(), "\nGold: ", gold.ToString() });
						if ((mouseState.LeftButton == ButtonState.Pressed && canClick))
						{
							canClick = false;
							if (player.Gold >= gold && player.Stone >= stone && player.Iron >= iron && player.Wood >= wood)
							{
								playerKingdom.Buildings[hoveredI][hoveredX] = buildable;
                                Player newPlayer = new Player(Content, player.Name);
                                newPlayer.Gold = gold * -1;
                                newPlayer.Stone = stone * -1;
                                newPlayer.Iron = iron * -1;
                                newPlayer.Wood = wood * -1;

								TcpClient client = new TcpClient("127.0.0.1", 1303);
								StreamWriter writer = new StreamWriter(client.GetStream());
								string messageString = string.Concat(new object[] { "2 ", newPlayer.Name, " ", newPlayer.Stone, " ", newPlayer.Iron, " ", newPlayer.Wood, " ", newPlayer.Gold });
								byte[] message = Encoding.ASCII.GetBytes(messageString);
								writer.BaseStream.Write(message, 0, (int)message.Length);
								writer.Flush();
								byte[] buffer = new byte[client.Client.ReceiveBufferSize];
								client.Client.Receive(buffer);
								List<string> splitResponse = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', default(char) }).Trim().Split(new char[] { ' ' }).ToList<string>();
								if ((splitResponse[0] != "2" ? false : splitResponse[1] == "Success"))
								{
									hoverText = string.Empty;
									buildMouseState = BuildMouseState.NONE;
									if (buildable is House)
									{
										Kingdom totalWorkers = playerKingdom;
										totalWorkers.TotalWorkers = totalWorkers.TotalWorkers + 15;
										Kingdom unusedWorkers = playerKingdom;
										unusedWorkers.UnusedWorkers = unusedWorkers.UnusedWorkers + 15;
									}
									else if (buildable is Barracks)
									{
										Kingdom soldierMax = playerKingdom;
										soldierMax.SoldierMax = soldierMax.SoldierMax + 15;
									}
									client = new TcpClient("127.0.0.1", 1303);
									writer = new StreamWriter(client.GetStream());
									string secondMessageString = string.Concat(new string[] { "7 ", playerKingdom.Name, " ", hoveredI.ToString(), " ", hoveredX.ToString(), " ", buildable.GetName() });
									byte[] secondMessage = Encoding.ASCII.GetBytes(secondMessageString);
									writer.BaseStream.Write(secondMessage, 0, (int)secondMessage.Length);
									writer.Flush();
								}
							}
						}
					}
					count++;
				}

                Rectangle rectangle = new Rectangle((int)buildMenuPosition.X, (int)buildMenuPosition.Y, 64, 32 + count * 32);

                if (hoveredI != -1 && hoveredX != -1 && !mouseRectangle.Intersects(rectangle) && mouseState.LeftButton == ButtonState.Pressed && canClick)
                {
                    buildMouseState = BuildMouseState.NONE;
                    hoveredI = -1;
                    hoveredX = -1;
                    canClick = false;
                }
            }
		}

		private void UpdateMap(Rectangle mouseRectangle, Vector2 mousePosition, MouseState mouseState)
		{            
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				state = GameStates.DASHBOARD;

                TcpClient client = new TcpClient("127.0.0.1", 1303);
                StreamWriter writer = new StreamWriter(client.GetStream());
                String messageString = "10 " + player.Name + " " + player.CurrentPosition.X + " " + player.CurrentPosition.Y;
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                writer.BaseStream.Write(message, 0, (int)message.Length);
            }
			hoverText = string.Empty;

			camera.Update(mapCenterPosition);
			if (mousePosition.X > graphics.GraphicsDevice.Viewport.Width - 50)
			{
				mapCenterPosition.X += 5f;
			}
			else if ((mousePosition.X >= 50 ? false : mapCenterPosition.X > 0f))
			{
				mapCenterPosition.X -= 5f;
			}
			if (mousePosition.Y > graphics.GraphicsDevice.Viewport.Height - 50)
			{
				mapCenterPosition.Y += 5f;
			}
			else if ((mousePosition.Y >= 50 ? false : mapCenterPosition.Y > 0f))
			{
				mapCenterPosition.Y -= 5f;
			}

			foreach (MapEntry entry in mapEntries)
			{
				if (mouseRectangle.Intersects(new Rectangle((int)(entry.Position.X * 64 * scaleX), (int)(entry.Position.Y * 64 * scaleY), (int)(64 * scaleX), (int)(64 * scaleY))))
				{
					hoverText = entry.GetInfo();
				}
			}

            if (mouseState.LeftButton == ButtonState.Pressed && canClick)
            {
                Vector2 clickedPosition = new Vector2((int)((mousePosition.X + camera.Position.X) / (64 * scaleX)), (int)((mousePosition.Y + camera.Position.Y) / (64 * scaleY)));
                canClick = false;

                if (clickedPosition.X == player.CurrentPosition.X && clickedPosition.Y == player.CurrentPosition.Y)
                {
                    MapEntry occupiedEntry;

                    foreach (MapEntry entry in mapEntries)
                    {
                        if (entry.Position.X == player.CurrentPosition.X && entry.Position.Y == player.CurrentPosition.Y)
                        {
                            switch (entry.Type)
                            {
                                case MapType.AIKINGDOM:
                                    state = GameStates.AIKINGDOM;

                                    kingdomPlayer = new KingdomPlayer(Content);
                                    break;
                                case MapType.BANDIT:
                                    //TODO: be able to attack bandits and stuff
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (clickedPosition.X >= 0 && clickedPosition.Y >= 0)
                    {
                        player.TargetPosition = clickedPosition;
                        canClick = false;
                    }
                }
            }

            player.Update();
		}

		private void UpdateResources(Rectangle mouseRectangle)
		{
			menuButton.Update(mouseRectangle, scaleX, scaleY);
			stoneDownButton.Update(mouseRectangle, scaleX, scaleY);
			stoneUpButton.Update(mouseRectangle, scaleX, scaleY);
			ironDownButton.Update(mouseRectangle, scaleX, scaleY);
			ironUpbutton.Update(mouseRectangle, scaleX, scaleY);
			woodDownButton.Update(mouseRectangle, scaleX, scaleY);
			woodUpButton.Update(mouseRectangle, scaleX, scaleY);
			if (menuButton.IsActivated)
			{
				state = GameStates.KINGDOMSCREEN;
				canClick = false;
				menuButton = new Button(new Rectangle(150, 325, buttonTexture.Width, buttonTexture.Height), buttonTexture, Content);
			}
			if (stoneDownButton.IsActivated)
			{
				if (playerKingdom.StoneWorkers > 0)
				{
					Kingdom stoneWorkers = playerKingdom;
					stoneWorkers.StoneWorkers = stoneWorkers.StoneWorkers - 1;
					Kingdom unusedWorkers = playerKingdom;
					unusedWorkers.UnusedWorkers = unusedWorkers.UnusedWorkers + 1;
				}
			}
			if (stoneUpButton.IsActivated)
			{
				if (playerKingdom.UnusedWorkers > 0)
				{
					Kingdom kingdom = playerKingdom;
					kingdom.StoneWorkers = kingdom.StoneWorkers + 1;
					Kingdom unusedWorkers1 = playerKingdom;
					unusedWorkers1.UnusedWorkers = unusedWorkers1.UnusedWorkers - 1;
				}
			}
			if (ironDownButton.IsActivated)
			{
				if (playerKingdom.IronWorkers > 0)
				{
					Kingdom ironWorkers = playerKingdom;
					ironWorkers.IronWorkers = ironWorkers.IronWorkers - 1;
					Kingdom kingdom1 = playerKingdom;
					kingdom1.UnusedWorkers = kingdom1.UnusedWorkers + 1;
				}
			}
			if (ironUpbutton.IsActivated)
			{
				if (playerKingdom.UnusedWorkers > 0)
				{
					Kingdom ironWorkers1 = playerKingdom;
					ironWorkers1.IronWorkers = ironWorkers1.IronWorkers + 1;
					Kingdom unusedWorkers2 = playerKingdom;
					unusedWorkers2.UnusedWorkers = unusedWorkers2.UnusedWorkers - 1;
				}
			}
			if (woodDownButton.IsActivated)
			{
				if (playerKingdom.WoodWorkers > 0)
				{
					Kingdom woodWorkers = playerKingdom;
					woodWorkers.WoodWorkers = woodWorkers.WoodWorkers - 1;
					Kingdom kingdom2 = playerKingdom;
					kingdom2.UnusedWorkers = kingdom2.UnusedWorkers + 1;
				}
			}
			if (woodUpButton.IsActivated)
			{
				if (playerKingdom.UnusedWorkers > 0)
				{
					Kingdom woodWorkers1 = playerKingdom;
					woodWorkers1.WoodWorkers = woodWorkers1.WoodWorkers + 1;
					Kingdom unusedWorkers3 = playerKingdom;
					unusedWorkers3.UnusedWorkers = unusedWorkers3.UnusedWorkers - 1;
				}
			}
		}

		private void UpdateSoldierBuying(Rectangle mouseRectangle, MouseState mouseState)
		{
			buySwordsmanButton.Update(mouseRectangle, scaleX, scaleY);
			hoverText = string.Empty;
			if (buySwordsmanButton.IsHovered)
			{
				hoverText = "Swordsman\nCost: 5";
				if (buySwordsmanButton.IsActivated && player.Gold >= 5)
				{
                    Player newPlayer = new Player(Content, player.Name);
                    newPlayer.Gold -= 5;
					playerKingdom.Soldiers.Add(new Swordsman(Content));
					StreamWriter writer = new StreamWriter((new TcpClient("127.0.0.1", 1303)).GetStream());
					string messageString = string.Concat("8 ", playerKingdom.Name, " Swordsman");
					byte[] message = Encoding.ASCII.GetBytes(messageString);
					writer.BaseStream.Write(message, 0, (int)message.Length);
					writer.Flush();
				}
			}
			if ((!mouseRectangle.Intersects(new Rectangle((int)(85f + camera.Position.X), (int)(150f + camera.Position.Y), buySoldiersTexture.Width, buySoldiersTexture.Height)) && mouseState.LeftButton == ButtonState.Pressed && canClick))
			{
				state = GameStates.MANAGEMENT;
				canClick = false;
			}
		}

		private void UpdateWilderness(GameTime gameTime)
		{
			wildernessPlayer.Update(gameTime, destroyableRects);
            resourceBuyer.Update(gameTime, wildernessPlayer.Position, ref canClick, camera.Position, Content);

			camera.Update(wildernessPlayer.Position);
			MouseState mouseState = Mouse.GetState();
			Rectangle mouseRectangle = new Rectangle((int)((float)mouseState.X + camera.Position.X), (int)((float)mouseState.Y + camera.Position.Y), 5, 5);
			if (Main.random.Next(0, 15) == 0)
			{
				Tree toBeAdded = new Tree(Content, destroyableRects);
                if (!toBeAdded.Rectangle.Intersects(wildernessPlayer.Rectangle))
                {
                    destroyableRects.Add(toBeAdded.Rectangle);
                    wildernessDestroyables.Add(toBeAdded);
                }
            }
			if (Main.random.Next(0, 25) == 0)
			{
				Quarry toBeAdded = new Quarry(Content, destroyableRects);
                if (!toBeAdded.Rectangle.Intersects(wildernessPlayer.Rectangle))
                {
                    destroyableRects.Add(toBeAdded.Rectangle);
                    wildernessDestroyables.Add(toBeAdded);
                }
            }
			if (Main.random.Next(0, 35) == 0)
			{
				IronOre toBeAdded = new IronOre(Content, destroyableRects);
				
                if (!toBeAdded.Rectangle.Intersects(wildernessPlayer.Rectangle))
                {
                    destroyableRects.Add(toBeAdded.Rectangle);
                    wildernessDestroyables.Add(toBeAdded);
                }
			}

			mouseTexture = null;
			for (int i = 0; i < wildernessDestroyables.Count; i++)
			{
				if (wildernessDestroyables[i].Rectangle.Intersects(new Rectangle((int)camera.Position.X, (int)camera.Position.Y, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height)))
                {
                    if (wildernessDestroyables[i].Rectangle.Intersects(mouseRectangle))
                    {
                        if (wildernessDestroyables[i] is Tree)
                        {
                            if (axeTexture == null)
                            {
                                axeTexture = Content.Load<Texture2D>("axe");
                            }
                            mouseTexture = axeTexture;
                            if ((mouseState.LeftButton == ButtonState.Pressed && wildernessPlayer.CanDamage))
                            {
                                wildernessPlayer.CanDamage = false;
                                Destroyable health = wildernessDestroyables[i];
                                health.Health = health.Health - 1;
                                for (int x = 0; x < 5; x++)
                                {
                                    particleManager.AddParticle(new Vector2((float)mouseState.X + camera.Position.X, (float)mouseState.Y + camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.BurlyWood, 1.2f, 300f);
                                }
                                breakEffect.Play();
                                if (wildernessDestroyables[i].Health <= 0)
                                {
                                    destroyableRects.Remove(wildernessDestroyables[i].Rectangle);

                                    wildernessPlayer.Inventory.Add(wildernessDestroyables[i].Item);
                                    wildernessDestroyables.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        else if (wildernessDestroyables[i] is Quarry)
                        {
                            if (pickaxeTexture == null)
                            {
                                pickaxeTexture = Content.Load<Texture2D>("pickaxe");
                            }
                            mouseTexture = pickaxeTexture;
                            if ((mouseState.LeftButton == ButtonState.Pressed && wildernessPlayer.CanDamage))
                            {
                                wildernessPlayer.CanDamage = false;
                                Destroyable destroyable = wildernessDestroyables[i];
                                destroyable.Health = destroyable.Health - 1;
                                for (int x = 0; x < 5; x++)
                                {
                                    particleManager.AddParticle(new Vector2((float)mouseState.X + camera.Position.X, (float)mouseState.Y + camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.DarkSlateGray, 1.2f, 300f);
                                }
                                breakEffect.Play();
                                if (wildernessDestroyables[i].Health <= 0)
                                {
                                    destroyableRects.Remove(wildernessDestroyables[i].Rectangle);

                                    wildernessPlayer.Inventory.Add(wildernessDestroyables[i].Item);
                                    wildernessDestroyables.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        else if (wildernessDestroyables[i] is IronOre)
                        {
                            if (pickaxeTexture == null)
                            {
                                pickaxeTexture = Content.Load<Texture2D>("pickaxe");
                            }
                            mouseTexture = pickaxeTexture;
                            if ((mouseState.LeftButton == ButtonState.Pressed && wildernessPlayer.CanDamage))
                            {
                                wildernessPlayer.CanDamage = false;
                                Destroyable item1 = wildernessDestroyables[i];
                                item1.Health = item1.Health - 1;
                                for (int x = 0; x < 5; x++)
                                {
                                    particleManager.AddParticle(new Vector2((float)mouseState.X + camera.Position.X, (float)mouseState.Y + camera.Position.Y), Main.random.Next(0, 180), 0.05f, 0.05f, Color.DarkSlateGray, 1.2f, 300f);
                                }
                                breakEffect.Play();
                                if (wildernessDestroyables[i].Health <= 0)
                                {
                                    destroyableRects.Remove(wildernessDestroyables[i].Rectangle);

                                    wildernessPlayer.Inventory.Add(wildernessDestroyables[i].Item);
                                    wildernessDestroyables.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				state = GameStates.DASHBOARD;
                Player newPlayer = new Player(Content, player.Name);

                foreach (InventoryItem item in wildernessPlayer.Inventory)
				{
					if (item is Wood)
					{
                        newPlayer.Wood += 1000f;
					}
					else if (item is Stone)
					{
                        newPlayer.Stone += 1000f;
					}
					else if (item is Iron)
					{
                        newPlayer.Iron += 1000f;
					}
				}
				StreamWriter writer = new StreamWriter((new TcpClient("127.0.0.1", 1303)).GetStream());
				string messageString = string.Concat(new object[] { "2 ", newPlayer.Name, " ", newPlayer.Stone, " ", newPlayer.Iron, " ", newPlayer.Wood, " ", newPlayer.Gold });
				byte[] message = Encoding.ASCII.GetBytes(messageString);
				writer.BaseStream.Write(message, 0, (int)message.Length);
			}
			particleManager.Update(gameTime);
		}

        void UpdateAIKingdom(GameTime gameTime)
        {
            kingdomPlayer.Update(gameTime);
        }

        void DrawAIKingdom()
        {
            kingdomPlayer.Draw(spriteBatch);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

            graphics.ApplyChanges();
        }
    }
}