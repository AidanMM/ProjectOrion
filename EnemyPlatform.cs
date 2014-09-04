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
	public class EnemyPlatform : BasicObject
	{
		Enemy MyEnemy;
		public EnemyPlatform (Texture2D myTexture, Vector2 startingPosition) : base(myTexture, "Ground")
		{
			Sprite.Position = startingPosition;
			Vector2 temp = new Vector2(startingPosition.X + AppMain.rand.Next(10, Sprite.TextureInfo.Texture.Width / 2), startingPosition.Y + Sprite.TextureInfo.Texture.Height);
			MyEnemy = new Enemy(temp, "Application/sprites/Test.png");
			AppMain.GameObjectController.AddObjectToDrawList(MyEnemy);
		}
		
		public override void Update ()
		{
			base.Update ();
			
			if(MyEnemy.Sprite.Position.X >= Sprite.Position.X + Sprite.TextureInfo.Texture.Width / 2 - MyEnemy.Sprite.TextureInfo.Texture.Width / 2 )
			{
				if(MyEnemy.Velocity.X > 0)
				{
					MyEnemy.Velocity.X *= -1;
				}
			}
			if(MyEnemy.Sprite.Position.X <= Sprite.Position.X - Sprite.TextureInfo.Texture.Width / 2 + MyEnemy.Sprite.TextureInfo.Texture.Width / 2 )
			{
				if(MyEnemy.Velocity.X < 0)
				{
					MyEnemy.Velocity.X *= -1;	
				}
			}
		}
	}
}

