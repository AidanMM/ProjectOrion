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
	public class Bullet : BasicObject
	{
		public Bullet (Vector2 velocity,Vector2 startPosition, int lifeTime) : base(new Texture2D("Application/sprites/BulletTest.png", false), "Bullet", lifeTime )
		{
			Sprite.Position = startPosition;
			Velocity = velocity;
			IsMovingObject = true;
			GhostCollision = true;
			
		}
		public override void Update ()
		{
			base.Update ();
			if(AppMain.GameObjectController.CheckCollisionByName(this, "Ground") != null)
			{
				Destroy();	
			}
			
			if(AppMain.GameObjectController.CheckCollisionByName(this, "Enemy") != null)
			{
				AppMain.GameObjectController.CheckCollisionByName(this, "Enemy").OnCollision();	
				Destroy();
			}	 	
			
		}
	}
}

