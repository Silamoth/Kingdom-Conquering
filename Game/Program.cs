using Microsoft.Xna.Framework;
using System;

namespace Kingdom_Conquering
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			using (Main game = new Main())
			{
				game.Run();
			}
		}
	}
}