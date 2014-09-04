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
	public class WorldSelection : BasicObject
	{
		int worldToGoTo;
		public WorldSelection (int world, Vector2 position) : base(new Texture2D("Application/sprites/Test.png", false), "World Choice")
		{
			worldToGoTo = world;
			Sprite.Position = position;
			IsMovingObject = true;
			GhostCollision = true;
		}
		public override void Update ()
		{
			base.Update ();
			
			if(AppMain.GameObjectController.CheckCollisionByName(this, "Bullet") != null)
			{
				OnCollision();
			}
		}
		public override void OnCollision ()
		{
			base.OnCollision ();
			string newSceneString = String.Format("Application/sprites/Level{0}-1.png", worldToGoTo);
			AppMain.CurrentWorld = worldToGoTo;
			AppMain.GoToNewScene(newSceneString);
		}
	}
}

