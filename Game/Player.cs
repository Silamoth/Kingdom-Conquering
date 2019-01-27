using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

namespace Kingdom_Conquering
{
	internal class Player
	{
	    Vector2 currentPosition;
		Vector2 targetPosition;
		Texture2D texture;
        bool canMove;
        int moveTimer;
        String name;

		public bool HasKingdom { get; set; }

		public Player(ContentManager content, String name)
		{
            if (texture == null)
                texture = content.Load<Texture2D>("playerPrototype");

            canMove = true;
            moveTimer = 0;

            this.name = name;
		}

        public void Update()
        {
            if (!canMove)
            {
                moveTimer++;

                if (moveTimer == 50)
                {
                    canMove = true;
                    moveTimer = 0;
                }
            }

            if (targetPosition.X > currentPosition.X &&  canMove)
            {
                currentPosition.X++;
                canMove = false;
            }
            else if (targetPosition.X < currentPosition.X && canMove)
            {
                currentPosition.X--;
                canMove = false;
            }

            if (targetPosition.Y > currentPosition.Y && canMove)
            {
                currentPosition.Y++;
                canMove = false;
            }
            else if (targetPosition.Y < currentPosition.Y && canMove)
            {
                currentPosition.Y--;
                canMove = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float scaleX, float scaleY)
        {
            //spriteBatch.Draw(texture, new Vector2(currentPosition.X * 64 + texture.Width / 2, currentPosition.Y * 64 + texture.Height / 2), Color.White);
            spriteBatch.Draw(texture, new Rectangle((int)(currentPosition.X * (64 * scaleX) + texture.Width / (2 / scaleX)), (int)(currentPosition.Y * (64 * scaleY) + texture.Height / (2 / scaleY)), (int)(texture.Width * scaleX), (int)(texture.Height * scaleY)), Color.White);

        }

        public Vector2 CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }

        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        public String Name
        {
            get { return name; }
        }

        public String KingdomName { get; set; }

        public int Gold { get; set; }

        public float Stone { get; set; }

        public float Iron { get; set; }

        public float Wood { get; set; }
    }
}