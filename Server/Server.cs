using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Server
{
    struct Kingdom
    {
        String name;
        int stoneWorkers, ironWorkers, woodWorkers, totalWorkers, unusedWorkers;

        public int StoneWorkers
        {
            get { return stoneWorkers; }
            set { stoneWorkers = value; }
        }

        public int IronWorkers
        {
            get { return ironWorkers; }
            set { ironWorkers = value; }
        }

        public int WoodWorkers
        {
            get { return woodWorkers; }
            set { woodWorkers = value; }
        }

        public int TotalWorkers
        {
            get { return totalWorkers; }
            set { totalWorkers = value; }
        }

        public int UnusedWorkers
        {
            get { return unusedWorkers; }
            set { unusedWorkers = value; }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public char[][] Buildings { get; set; }

        public List<String> Soldiers { get; set; }
        
        public String Owner { get; set; }

        public float GeneratedStone { get; set; }
        public float GeneratedIron { get; set; }
        public float GeneratedWood { get; set; }
    }

    struct MapEntry
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        //X and Y are in terms of map coordinates, not game coordinates
    }

    struct Player
    {
        public String Password { get; set; }
        public int MapX { get; set; }
        public int MapY { get; set; }
        public String KingdomName { get; set; }
        public bool HasKingdom { get; set; }
        public int Gold { get; set; }
        public float Stone { get; set; }
        public float Iron { get; set; }
        public float Wood { get; set; }
    }

    struct AIKingdom
    {
        public int MapX { get; set; }
        public int MapY { get; set; }
        public String Name { get; set; }
    }

    class Server
    {
        static Dictionary<String, Kingdom> kingdoms;
        static Stopwatch stopwatch;
        static Dictionary<String, Kingdom> workersToUpdate, kingdomsToAdd, kingdomBuildingsToAdd, soldiersToAdd;
        static float totalTicks;
        static List<MapEntry> mapEntries;
        static Random random;
        static Dictionary<String, Player> players, playersToAdd, resourcesToAdd;
        static Dictionary<String, AIKingdom> aiKingdoms;
        
        static void Main(string[] args)
        {
            kingdoms = new Dictionary<String, Kingdom>();
            stopwatch = new Stopwatch();
            workersToUpdate = new Dictionary<String, Kingdom>();
            resourcesToAdd = new Dictionary<String, Player>();
            kingdomsToAdd = new Dictionary<String, Kingdom>();
            mapEntries = new List<MapEntry>();
            kingdomBuildingsToAdd = new Dictionary<String, Kingdom>();
            soldiersToAdd = new Dictionary<String, Kingdom>();
            playersToAdd = new Dictionary<String, Player>();

            random = new Random();

            DirectoryInfo directoryInfo = new DirectoryInfo("Kingdoms");

            List<FileInfo> files = directoryInfo.GetFiles().ToList<FileInfo>();

            foreach (FileInfo file in files)
            {
                StreamReader reader = new StreamReader(file.FullName);
                String[] splitText = reader.ReadToEnd().Split('\n');
                Kingdom kingdom = new Kingdom();
                int stoneWorkers, ironWorkers, woodWorkers, totalWorkers, unusedWorkers, x, y;

                int.TryParse(splitText[0], out totalWorkers);
                int.TryParse(splitText[1], out unusedWorkers);
                int.TryParse(splitText[2], out stoneWorkers);
                int.TryParse(splitText[3], out ironWorkers);
                int.TryParse(splitText[4], out woodWorkers);
                int.TryParse(splitText[5], out x);
                int.TryParse(splitText[6], out y);

                char[][] buildings = new char[8][];

                for (int i = 0; i < 8; i++)
                {
                    buildings[i] = splitText[7 + i].ToCharArray();
                }
                
                String[] splitSoldier = splitText[15].Trim().Split('/');

                kingdom.TotalWorkers = totalWorkers;
                kingdom.UnusedWorkers = unusedWorkers;
                kingdom.StoneWorkers = stoneWorkers;
                kingdom.IronWorkers = ironWorkers;
                kingdom.WoodWorkers = woodWorkers;
                kingdom.X = x;
                kingdom.Y = y;
                kingdom.Buildings = buildings;
                kingdom.Soldiers = splitSoldier.ToList<String>();
                kingdom.Owner = splitText[16];

                kingdoms.Add(file.Name.Remove(file.Name.IndexOf(file.Extension)), kingdom);
                workersToUpdate.Add(file.Name.Remove(file.Name.IndexOf(file.Extension)), kingdom);

                MapEntry entry = new MapEntry();
                entry.X = x;
                entry.Y = y;
                entry.Type = "KINGDOM";
                entry.Name = file.Name.Remove(file.Name.IndexOf(file.Extension));
                mapEntries.Add(entry);
            }

            players = new Dictionary<String, Player>();

            directoryInfo = new DirectoryInfo("Players");
            files = directoryInfo.GetFiles().ToList<FileInfo>();

            foreach (FileInfo file in files)
            {
                StreamReader reader = new StreamReader(file.FullName);
                String[] splitText = reader.ReadToEnd().Split('\n');

                int mapX, mapY, gold;
                float stone, iron, wood;

                int.TryParse(splitText[0], out mapX);
                int.TryParse(splitText[1], out mapY);

                String password = splitText[2];

                String[] kingdomStuff = splitText[3].Split(' ');
                bool hasKingdom;
                bool.TryParse(kingdomStuff[0], out hasKingdom);

                Player newPlayer = new Player();
                newPlayer.MapX = mapX;
                newPlayer.MapY = mapY;
                newPlayer.Password = password;
                newPlayer.HasKingdom = hasKingdom;

                if (hasKingdom)
                    newPlayer.KingdomName = kingdomStuff[1];
                else
                    newPlayer.KingdomName = String.Empty;

                int.TryParse(splitText[4], out gold);
                float.TryParse(splitText[4], out stone);
                float.TryParse(splitText[4], out iron);
                float.TryParse(splitText[4], out wood);

                newPlayer.Gold = gold;
                newPlayer.Stone = stone;
                newPlayer.Iron = iron;
                newPlayer.Wood = wood;

                players.Add(file.Name.Remove(file.Name.IndexOf(file.Extension)), newPlayer);
            }

            aiKingdoms = new Dictionary<String, AIKingdom>();

            directoryInfo = new DirectoryInfo("AIKingdoms");
            files = directoryInfo.GetFiles().ToList<FileInfo>();

            foreach (FileInfo file in files)
            {
                StreamReader reader = new StreamReader(file.FullName);
                String[] splitText = reader.ReadToEnd().Split('\n');

                int mapX, mapY;

                int.TryParse(splitText[0], out mapX);
                int.TryParse(splitText[1], out mapY);

                AIKingdom newAIKingdom = new AIKingdom();
                newAIKingdom.MapX = mapX;
                newAIKingdom.MapY = mapY;

                aiKingdoms.Add(file.Name.Remove(file.Name.IndexOf(file.Extension)), newAIKingdom);

                MapEntry entry = new MapEntry();
                entry.X = mapX;
                entry.Y = mapY;
                entry.Type = "AIKINGDOM";
                entry.Name = file.Name.Remove(file.Name.IndexOf(file.Extension));
                mapEntries.Add(entry);
            }

            ThreadStart updateStart = new ThreadStart(UpdateAllKingdoms);
            ThreadStart clientStart = new ThreadStart(AcceptClients);

            Thread updateThread = new Thread(updateStart);
            Thread clientThread = new Thread(clientStart);

            updateThread.Start();
            clientThread.Start();
        }

        static void UpdateAllKingdoms()
        {
           while (true)
           {
                //Update all passive resource generation

                Dictionary<String, Kingdom> newKingdoms = new Dictionary<String, Kingdom>();
                stopwatch.Start();

                foreach (String key in kingdoms.Keys)
                {
                    Kingdom newKingdom = kingdoms[key];

                    newKingdom.GeneratedStone = (float)newKingdom.StoneWorkers * (float)(stopwatch.ElapsedTicks / 20000);
                    newKingdom.GeneratedIron = (float)newKingdom.IronWorkers * (float)(stopwatch.ElapsedTicks / 20000);
                    newKingdom.GeneratedWood = (float)newKingdom.WoodWorkers * (float)(stopwatch.ElapsedTicks / 20000);

                    //Console.WriteLine("Stone generated: " + newKingdom.GeneratedStone);

                    if (workersToUpdate.ContainsKey(key))
                    {
                        //Update workers
                        newKingdom.StoneWorkers = workersToUpdate[key].StoneWorkers;
                        newKingdom.IronWorkers = workersToUpdate[key].IronWorkers;
                        newKingdom.WoodWorkers = workersToUpdate[key].WoodWorkers;
                        newKingdom.TotalWorkers = workersToUpdate[key].TotalWorkers;
                        newKingdom.UnusedWorkers = workersToUpdate[key].UnusedWorkers;
                    }                    

                    if (kingdomBuildingsToAdd.ContainsKey(key))
                    {
                        newKingdom.Buildings = kingdomBuildingsToAdd[key].Buildings;
                        kingdomBuildingsToAdd.Remove(key);
                    }

                    if (soldiersToAdd.ContainsKey(key))
                    {
                        newKingdom.Soldiers = soldiersToAdd[key].Soldiers;
                        soldiersToAdd.Remove(key);
                    }

                    newKingdom.Owner = kingdoms[key].Owner;
                    newKingdoms.Add(key, newKingdom);
                }

                foreach (String key in kingdomsToAdd.Keys)
                {
                    newKingdoms.Add(key, kingdomsToAdd[key]);
                }

                kingdomsToAdd = new Dictionary<String, Kingdom>();

                kingdoms = newKingdoms;

                Dictionary<String, Player> newPlayers = new Dictionary<String, Player>();

                foreach (String key in players.Keys)
                {
                    Player newPlayer = players[key];

                    if (newPlayer.HasKingdom)
                    {
                        newPlayer.Stone += kingdoms[newPlayer.KingdomName.TrimEnd(new char[] { '\n', '\r', '\0' })].GeneratedStone;
                        newPlayer.Iron += kingdoms[newPlayer.KingdomName.TrimEnd(new char[] { '\n', '\r', '\0' })].GeneratedIron;
                        newPlayer.Wood += kingdoms[newPlayer.KingdomName.TrimEnd(new char[] { '\n', '\r', '\0' })].GeneratedWood;

                        //Console.WriteLine(key + " has " + newPlayer.Stone.ToString() + " stone");
                    }

                    if (playersToAdd.ContainsKey(key))
                    {
                        newPlayer.MapX = playersToAdd[key].MapX;
                        newPlayer.MapY = playersToAdd[key].MapY;
                        playersToAdd.Remove(key);
                    }

                    //Add resources from nonpassive sources
                    if (resourcesToAdd.ContainsKey(key))
                    {
                        newPlayer.Stone += resourcesToAdd[key].Stone;
                        newPlayer.Iron += resourcesToAdd[key].Iron;
                        newPlayer.Wood += resourcesToAdd[key].Wood;
                        newPlayer.Gold += resourcesToAdd[key].Gold;

                        resourcesToAdd.Remove(key);
                    }

                    newPlayers.Add(key, newPlayer);
                }

                players = newPlayers;

                totalTicks += stopwatch.ElapsedTicks;

                //Saves about every minute and a half
                if (totalTicks > 9999999)
                {
                    totalTicks = 0;

                    foreach (String key in kingdoms.Keys)
                    {
                        String fileName = "Kingdoms/" + key + ".txt";
                        StreamWriter writer = new StreamWriter(fileName);

                        writer.WriteLine(kingdoms[key].TotalWorkers.ToString());
                        writer.WriteLine(kingdoms[key].UnusedWorkers.ToString());
                        writer.WriteLine(kingdoms[key].StoneWorkers.ToString());
                        writer.WriteLine(kingdoms[key].IronWorkers.ToString());
                        writer.WriteLine(kingdoms[key].WoodWorkers.ToString());
                        writer.WriteLine(kingdoms[key].X);
                        writer.WriteLine(kingdoms[key].Y);

                        for (int i = 0; i < kingdoms[key].Buildings.Length; i++)
                        {
                            writer.WriteLine(kingdoms[key].Buildings[i]);
                        }

                        String soldierString;

                        if (kingdoms[key].Soldiers.Count == 0)
                            soldierString = String.Empty;
                        else
                        {
                           soldierString = kingdoms[key].Soldiers[0];

                            for (int i = 1; i < kingdoms[key].Soldiers.Count; i++)
                            {
                                soldierString += "/" + kingdoms[key].Soldiers[i];
                            }
                        }

                        writer.WriteLine(soldierString);
                        writer.WriteLine(kingdoms[key].Owner);

                        writer.Close();
                    }

                    foreach (String key in players.Keys)
                    {
                        String fileName = "Players/" + key + ".txt";
                        StreamWriter writer = new StreamWriter(fileName);

                        writer.WriteLine(players[key].MapX.ToString());
                        writer.WriteLine(players[key].MapY.ToString());
                        writer.WriteLine(players[key].Password);                        

                        if (players[key].HasKingdom)
                            writer.WriteLine(players[key].HasKingdom + " " + players[key].KingdomName);
                        else
                            writer.WriteLine(players[key].HasKingdom);

                        writer.WriteLine(players[key].Gold);
                        writer.WriteLine(players[key].Stone);
                        writer.WriteLine(players[key].Iron);
                        writer.WriteLine(players[key].Wood);

                        writer.Close();
                    }
                }

                stopwatch.Restart();
            }
        }

        static void AcceptClients()
        {
            //Accept clients and send and receive messages

            TcpListener listener = new TcpListener(1303);
            listener.Start();

            while (true)
            {
                //try
                //{
                    TcpClient client = listener.AcceptTcpClient();

                    StreamWriter writer = new StreamWriter(client.GetStream());

                    byte[] buffer = new byte[client.Client.ReceiveBufferSize];
                    client.Client.Receive(buffer);
                    String request = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', '\0' });
                    List<String> splitRequest = request.Split(' ').ToList<String>();

                    String id = splitRequest[0];
                    String name = splitRequest[1];
                    
                    if (id != "0" && id != "10")
                        Console.WriteLine("Kingdom " + name + " has sent a request with ID " + id);

                    switch (id)
                    {
                        case "0":
                            //Updating info about workers and requesting info about resources

                            int stoneWorkers, ironWorkers, woodWorkers, totalWorkers, unusedWorkers;

                            int.TryParse(splitRequest[2], out stoneWorkers);
                            int.TryParse(splitRequest[3], out ironWorkers);
                            int.TryParse(splitRequest[4], out woodWorkers);
                            int.TryParse(splitRequest[5], out totalWorkers);
                            int.TryParse(splitRequest[6], out unusedWorkers);

                        //if (!kingdoms.ContainsKey(name))
                        //    Console.WriteLine("Key " + name + " is not present...");

                        Kingdom kingdom = kingdoms[name];

                            kingdom.StoneWorkers = stoneWorkers;
                        kingdom.IronWorkers = ironWorkers;
                            kingdom.WoodWorkers = woodWorkers;
                            kingdom.TotalWorkers = totalWorkers;
                            kingdom.UnusedWorkers = unusedWorkers;

                            workersToUpdate[name] = kingdom;

                            Player player = new Player();

                            foreach (String key in players.Keys)
                            {
                                if (key == kingdom.Owner.TrimEnd(new char[] { '\n', '\r', '\0' }))
                                {
                                    player = players[key];
                                    break;
                                }
                            }

                            String responseString = "0 " + player.Stone + " " + player.Iron + " " + player.Wood + " " + player.Gold;
                            byte[] response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                            writer.Flush();
                            break;
                        case "1":
                            //Giving initial information about workers

                            responseString = "1 " + kingdoms[name].StoneWorkers + " " + kingdoms[name].IronWorkers + " " + kingdoms[name].WoodWorkers + " " + kingdoms[name].TotalWorkers + " " + kingdoms[name].UnusedWorkers;
                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                            writer.Flush();
                            break;
                        case "2":
                            //Updating kingdom resources from wilderness

                            int stone, iron, wood, gold;

                            int.TryParse(splitRequest[2], out stone);
                            int.TryParse(splitRequest[3], out iron);
                            int.TryParse(splitRequest[4], out wood);
                            int.TryParse(splitRequest[5], out gold);

                            player = players[name];

                            player.Stone = stone;
                            player.Iron = iron;
                            player.Wood = wood;
                            player.Gold = gold;

                            resourcesToAdd[name] = player;

                            responseString = "2 Success";
                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                            writer.Flush();
                        break;
                        case "4":
                            //New account
                            String proposedPassword = splitRequest[2];
                            
                            foreach (String existingName in players.Keys)
                            {
                                if (name == existingName)
                                {
                                    responseString = "Taken";
                                    response = Encoding.ASCII.GetBytes(responseString);
                                    writer.BaseStream.Write(response, 0, response.Length);
                                    writer.Flush();
                                    return;
                                }
                            }

                            String fileName = "Players/" + name + ".txt";
                            StreamWriter fileWriter = new StreamWriter(fileName);

                            //TODO: make new player...

                            responseString = "Good";
                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                            writer.Flush();

                            break;

                        case "5":
                        //Map entries
                            responseString = "5 ";
                            foreach (MapEntry entry in mapEntries)
                            {
                                responseString += entry.Name + "/" + entry.Type + "/" + entry.X + "/" + entry.Y + " ";
                            }

                            response = Encoding.ASCII.GetBytes(responseString);
                            writer.BaseStream.Write(response, 0, response.Length);
                            writer.Flush();
                            break;
                    case "6":
                        //Buildings

                        kingdom = kingdoms[name];
                        responseString = "6 ";
                        for (int i = 0; i < kingdom.Buildings.Length; i++)
                        {
                            List<Char> line = new List<Char>();

                            for (int j = 0; j < kingdom.Buildings[i].Length; j++)
                            {
                                line.Add(kingdom.Buildings[i][j]);
                            }

                            responseString += String.Concat(line) + " ";
                        }

                        response = Encoding.ASCII.GetBytes(responseString);
                        writer.BaseStream.Write(response, 0, response.Length);
                        writer.Flush();
                        break;
                    case "7":
                        //Add new building

                        kingdom = kingdoms[name];

                        int iAdd, xAdd;

                        int.TryParse(splitRequest[2], out iAdd);
                        int.TryParse(splitRequest[3], out xAdd);
                        char type = splitRequest[4].ToCharArray()[0];

                        kingdom.Buildings[iAdd][xAdd] = type;
                        kingdomBuildingsToAdd[name] = kingdom;
                        break;
                    case "8":
                        kingdom = kingdoms[name];

                        String soldierType = splitRequest[2];

                        kingdom.Soldiers.Add(soldierType);
                        break;
                    case "9":
                        player = players[name];

                        responseString = "9 " + player.HasKingdom + " " + player.MapX + " " + player.MapY + " " + player.Gold + " " + player.Stone + " " +
                            player.Iron + " " + player.Wood + " " + player.KingdomName;
                        response = Encoding.ASCII.GetBytes(responseString);
                        writer.BaseStream.Write(response, 0, response.Length);
                        writer.Flush();
                        break;
                    case "10":
                        player = players[name];

                        int mapX, mapY;

                        int.TryParse(splitRequest[2], out mapX);
                        int.TryParse(splitRequest[3], out mapY);

                        player.MapX = mapX;
                        player.MapY = mapY;

                        playersToAdd.Add(name, player);
                        break;
                    }
                //}
                //catch (Exception ex)
                //{
                    //Console.WriteLine(ex.Message);
                //}
            }
        }
    }
}