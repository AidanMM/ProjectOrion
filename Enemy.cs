using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Imaging;


namespace ProjectOrion
{
	public class Enemy : BasicObject
	{
		public int health;
		
		public Enemy (Vector2 startingPosition, string myTexture) : base(new Texture2D(myTexture, false), "Enemy")
		{
			Sprite.Position = startingPosition;
			//HasGravity = true;
			IsMovingObject = true;
			GhostCollision = true;
			Random rand = new Random();
			float temp  = rand.Next(-2,2);
			while(temp == 0)
				temp = rand.Next(-2,2);
			Velocity.X = temp;
			health =  rand.Next(3, 6);
			
		}
		public override void Update ()
		{
			base.Update ();
			//Roam(1);
			
			if(health <= 0)
			{
				if(Destroyed == false)
				{
				AppMain.GameObjectController.EnemyCount--;
				}
				Destroy();	
			}
		}
		
		public override void OnCollision ()
		{
			base.OnCollision ();
			health -= 1;
		}
	}
}

