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
	public class DoorPortal : BasicObject
	{
		int portalLevel;
		bool NextLevelPortal = false;
		public DoorPortal (bool nextOrNot, Vector2 startPos) : base(new Texture2D("Application/sprites/PortalTile.png", false), "Portal") 
		{
			NextLevelPortal = nextOrNot;
			Sprite.Position = startPos;
			IsMovingObject = true;
		}
		
		
		public override void ClassSpecific ()
		{
			base.ClassSpecific ();
			
			
			if(NextLevelPortal == true)
			{
				if(AppMain.CurrentScene == AppMain.SceneList.Count -1)
				{
					AppMain.GameObjectController.MoveAllObjects(new Vector2(10, 0));
					string newSceneString = String.Format("Application/sprites/Level{0}-{1}.png", AppMain.CurrentWorld, AppMain.CurrentScene + 1);
					AppMain.GoToNewScene(newSceneString);
				}
				else if(AppMain.CurrentScene != AppMain.SceneList.Count - 1)
				{
					AppMain.GameObjectController.MoveAllObjects(new Vector2(10, 0));
					AppMain.GoToNextScene();	
				}
			}
			if(NextLevelPortal == false)
			{
				if(AppMain.CurrentScene > AppMain.SceneList.Count - 2)
				{
					AppMain.GameObjectController.MoveAllObjects(new Vector2(-10, 0));
					AppMain.GoToPreviousScene();
				}
			}
		}
	}
}

