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
	public class BossEnemy : Enemy
	{
		public BossEnemy (Vector2 startPos, int startingHealth) : base(startPos, "Application/sprites/BossTest.png")
		{
		 	health = startingHealth;
		}
		
		
		public override void Update ()
		{
			base.Update ();
			timer++;
			if(timer % 50 == 0)
			{
				Velocity.X *= -1;
			}
		}
	}
}

