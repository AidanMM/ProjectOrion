using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Imaging;

using System.Linq;
using System.IO;	

namespace ProjectOrion
{
	public class AppMain
	{
		/// <summary>
		/// The base scene for the game to us
		/// </summary>
		public static Scene scene;
		/// <summary>
		/// The graphics Controller.
		/// </summary>
		private static GraphicsContext graphics;
		/// <summary>
		/// The games object Controller.  This manages all of the objects in the game
		/// </summary>
		public static ObjectController GameObjectController;
		public static bool Quit = false;
		public static int[,] LevelArray;
		public static Random rand;
		/// <summary>
		/// The scene list contains all of the current scenes for the game,  allowing the player to go back and forth between levels.
		/// </summary>
		public static List<Scene> SceneList;
		/// <summary>
		/// The current scene.
		/// </summary>
		public static int CurrentScene;
		/// <summary>
		/// The current world the player resides in.  
		/// </summary>
		public static int CurrentWorld;
		public static int screenCenterX;
		public static int screenCenterY;
		
		public static void Main (string[] args)
		{
			SceneList = new List<Scene>();
			GameObjectController = new ObjectController();
			Director.Initialize ();
			scene = new Scene();
			scene.Camera.SetViewFromViewport();
			
			var width = Director.Instance.GL.Context.GetViewport().Width;
			screenCenterX = width / 2;
   			var height = Director.Instance.GL.Context.GetViewport().Height;
			screenCenterY = height / 2;
			SceneList.Add(scene);
			CurrentScene = 0;
			rand = new Random();
			CurrentWorld = 1;
			
			ReadLevel("Application/sprites/WorldSelect.png");
			
			
			
			
			#region AddObjects
			GameObjectController.AddObjectToDrawList(new Player(18, 10));
			GameObjectController.FindObjectWithName("Player").HasGravity = false;
			GameObjectController.FindObjectWithName("Player").Sprite.VertexZ = 1.0f;
			GameObjectController.AddObjectToDrawList(new WorldSelection(1, new Vector2(300, 400)));
			GameObjectController.AddObjectToDrawList(new WorldSelection(2, new Vector2(400, 400)));
			//GameObjectController.AddObjectToDrawList(new BasicObject(new Texture2D("Application/sprites/SampleBackground.png", false), "Background"));
			//GameObjectController.FindObjectWithName("Background").GhostCollision = true;
			//GameObjectController.FindObjectWithName("Background").Sprite.VertexZ = -1.0f;
			
			//GameObjectController.AddObjectToDrawList(new Enemy(new Vector2(400, 400), "Application/sprites/Test.png"));
			CreateGameObjects(LevelArray);
			#endregion
				Director.Instance.RunWithScene(scene, true);
				
				while(Quit == false)
				{
					GameObjectController.UpdateObjectList();
					Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Update();
					
					Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Render();
	   				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
	    			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap();
				
				}
			
			
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			//scene = new Scene();
			//scene.Camera.SetViewFromViewport();
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();

			// Present the screen
			graphics.SwapBuffers ();
		}
		/// <summary>
		/// Reads in a level from the given path.  The level must currently be a png for it to be loaded in
		/// </summary>
		/// <param name='levelPath'>
		/// Path that leads to the png file for the level
		/// </param>
		public static void ReadLevel(string levelPath)
		{
			Image image = new Image(levelPath);
			LevelArray = new int[image.Size.Width, image.Size.Height];
			image.Decode();
			byte[] toHold = image.ToBuffer();
			for(int y = 0; y < image.Size.Height; y++)
			{
				for(int x = 0; x < image.Size.Width; x++)
				{
					LevelArray[x,y] = toHold[y * image.Size.Width * 4 + x * 4 ];	
				}
			}
				
		}
		/// <summary>
		/// Creates the game objects from the read png.
		/// </summary>
		/// <param name='levelArray'>
		/// Takes the level array from the main class
		/// </param>
		public static void CreateGameObjects(int[,] levelArray)
		{
			for(int y = levelArray.GetLength(1) - 1; y >= 0; y--)
			{
				for(int x = 0; x < levelArray.GetLength(0); x++)
				{
					if(levelArray[x,y] == 255)
					{
						int i = 1;
						while(x + i < levelArray.GetLength(0) && i < 6)
						{
							if(levelArray[x + i,y] == 255)
							{
								levelArray[x + i,y] = 0;
								if(i < 5)
								i++;
							}
							else
							{
								break;
							}
						}
						
							BasicObject newGround;
							if(i >= 5)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground5long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + newGround.Sprite.TextureInfo.TextureSizef.X / 2, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 4)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground4long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + newGround.Sprite.TextureInfo.TextureSizef.X / 2, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 3)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground3long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + newGround.Sprite.TextureInfo.TextureSizef.X / 2, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 2)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground2long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + newGround.Sprite.TextureInfo.TextureSizef.X / 2, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 1)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground1long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + newGround.Sprite.TextureInfo.TextureSizef.X / 2, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							

					}
					if(levelArray[x,y] == 100)
					{
						GameObjectController.AddObjectToDrawList(new Enemy(new Vector2(x * 50 + 25, (levelArray.GetLength(1) - y) * 50), "Application/sprites/Test.png"));
						GameObjectController.EnemyCount++;
					}
					if(levelArray[x,y] == 127)
					{
						GameObjectController.AddObjectToDrawList(new BossEnemy(new Vector2(x * 50 + 25, (levelArray.GetLength(1) - y) * 50), 25));
						//GameObjectController.EnemyCount++;
					}
					if(levelArray[x,y] == 76)
					{
						GameObjectController.AddObjectToDrawList(new DoorPortal(false, new Vector2(x * 50 + 25, (levelArray.GetLength(1) - y) * 50)));
					}
					if(levelArray[x,y] == 77)
					{
						GameObjectController.AddObjectToDrawList(new DoorPortal(true, new Vector2(x * 50 + 25, (levelArray.GetLength(1) - y) * 50)));
				 	}
					if(levelArray[x,y] == 200)
					{
						int i = 1;
						while(x + i < levelArray.GetLength(0) && i < 6)
						{
							if(levelArray[x + i,y] == 200)
							{
								levelArray[x + i,y] = 0;
								if(i < 5)
								i++;
							}
							else
							{
								break;
							}
						}
						
							EnemyPlatform newGround;
							if(i >= 5)
							{
								newGround = new EnemyPlatform(new Texture2D("Application/sprites/Ground5long.png", false), new Vector2( x * 50, (levelArray.GetLength(1) - y) * 50));
								newGround.Sprite.Position = new Vector2(newGround.Sprite.Position.X + newGround.Sprite.TextureInfo.Texture.Width / 2, newGround.Sprite.Position.Y);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 4)
							{
								newGround = new EnemyPlatform(new Texture2D("Application/sprites/Ground4long.png", false), new Vector2( x * 50 , (levelArray.GetLength(1) - y) * 50));
								newGround.Sprite.Position = new Vector2(newGround.Sprite.Position.X + newGround.Sprite.TextureInfo.Texture.Width / 2, newGround.Sprite.Position.Y);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 3)
							{
								newGround = new EnemyPlatform(new Texture2D("Application/sprites/Ground3long.png", false), new Vector2( x * 50, (levelArray.GetLength(1) - y) * 50));
								newGround.Sprite.Position = new Vector2(newGround.Sprite.Position.X + newGround.Sprite.TextureInfo.Texture.Width / 2, newGround.Sprite.Position.Y);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 2)
							{
								newGround = new EnemyPlatform(new Texture2D("Application/sprites/Ground2long.png", false), new Vector2( x * 50, (levelArray.GetLength(1) - y) * 50));
								newGround.Sprite.Position = new Vector2(newGround.Sprite.Position.X + newGround.Sprite.TextureInfo.Texture.Width / 2, newGround.Sprite.Position.Y);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 1)
							{
								newGround = new EnemyPlatform(new Texture2D("Application/sprites/Ground1long.png", false), new Vector2( x * 50, (levelArray.GetLength(1) - y) * 50));
								newGround.Sprite.Position = new Vector2(newGround.Sprite.Position.X + newGround.Sprite.TextureInfo.Texture.Width / 2, newGround.Sprite.Position.Y);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							GameObjectController.EnemyCount++;

					}
					if(levelArray[x,y] == 254)
					{
						int i = 1;
						while(y + i < levelArray.GetLength(1) && i < 6 && y - i >= 0)
						{
							if(levelArray[x,y - i] == 254)
							{
								if(i < 5)
								{
								levelArray[x,y - i] = 0;
								i++;
								}
								else
									break;
							}
							else
							{
								break;
							}
						}
						
							BasicObject newGround;
							if(i >= 5)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Wall5long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + 25, (levelArray.GetLength(1) - y) * 50  + 100);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 4)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Wall4long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + 25, (levelArray.GetLength(1) - y) * 50  + 75);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 3)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Wall3long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + 25, (levelArray.GetLength(1) - y) * 50   + newGround.Sprite.TextureInfo.Texture.Height / 3);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if( i == 2)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Wall2long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50  + 25, (levelArray.GetLength(1) - y) * 50 + newGround.Sprite.TextureInfo.TextureSizef.Y / 4);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							else if(i == 1)
							{
								newGround = new BasicObject(new Texture2D("Application/sprites/Ground1long.png", false), "Ground");
								newGround.Sprite.Position = new Vector2( x * 50 + 25, (levelArray.GetLength(1) - y) * 50);
								GameObjectController.AddObjectToDrawList(newGround);
							}
							

					}
					
				}
			}
		}
		/// <summary>
		/// Goes to and creates a new scene using the handed in path to a png file
		/// </summary>
		/// <param name='levelTexture'>
		/// string to the needed png file
		/// </param>
		public static void GoToNewScene(string levelTexture)
		{
			CurrentScene = SceneList.Count;
			Scene sceneTemp = new Scene();
			sceneTemp.Camera.SetViewFromViewport();
			SceneList.Add(sceneTemp);
			Director.Instance.ReplaceScene(new TransitionCrossFade(SceneList[CurrentScene]){Duration = 3.0f});
			
			GameObjectController.ObjectListToHold.Clear();
			GameObjectController.ObjectListToHold.AddRange(GameObjectController.ObjectList);
			GameObjectController.ObjectList.Clear();
			GameObjectController.AddObjectToDrawList(new Player(18,10));
			GameObjectController.ObjectList[0].Sprite.Position = new Vector2(-100, GameObjectController.ObjectList[0].Sprite.Position.Y);
			GameObjectController.CameraDisplacement = new Vector2(0, AppMain.screenCenterY);
			ReadLevel(levelTexture);
			CreateGameObjects(LevelArray);
			
			for(int i = 0; i < GameObjectController.ObjectList.Count; i++)
			{
				GameObjectController.ObjectList[i].Sprite.Position += GameObjectController.CameraDisplacement;
			}
			GameObjectController.CameraDisplacement -= GameObjectController.ObjectList[0].Sprite.Position - GameObjectController.ObjectList[0].OldPosition;
			GameObjectController.ObjectList[0].Sprite.Position = new Vector2(AppMain.screenCenterX, GameObjectController.ObjectList[0].Sprite.Position.Y);
			GameObjectController.ObjectList[0].OldPosition = GameObjectController.ObjectList[0].Sprite.Position;
			//GameObjectController.freezetimer = GameObjectController.timer;
			
		}
		/// <summary>
		/// Takes the player to the previous scene.
		/// </summary>
		public static void GoToPreviousScene()
		{
			if(CurrentScene != 0)
			{
				GameObjectController.CleanUpGame();	
				CurrentScene--;
				Director.Instance.ReplaceScene(new TransitionCrossFade(SceneList[CurrentScene]){Duration = 1.0f});
				//Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL
				
				List<BasicObject> temp = new List<BasicObject>();
				temp.AddRange(GameObjectController.ObjectList);
				GameObjectController.ObjectList.Clear();
				GameObjectController.ObjectList.AddRange(GameObjectController.ObjectListToHold);
				GameObjectController.ObjectListToHold.Clear();
				GameObjectController.ObjectListToHold.AddRange(temp);
				GameObjectController.CameraDisplacement = new Vector2(0,0);
				//GameObjectController.MoveAllObjects(new Vector2(-50, -50));
				//GameObjectController.AddObjectToDrawList(new Player(18,10));
				
				GameObjectController.freezetimer = GameObjectController.timer;
			}
		}
		/// <summary>
		/// Takes the player to the scene that he was previously in.
		/// </summary>
			public static void GoToNextScene()
		{
			if(CurrentScene != SceneList.Count - 1)
			{
				GameObjectController.CleanUpGame();	
				CurrentScene++;
				Director.Instance.ReplaceScene(new TransitionCrossFade(SceneList[CurrentScene]){Duration = 1.0f});
				//Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL
				
				List<BasicObject> temp = new List<BasicObject>();
				temp.AddRange(GameObjectController.ObjectList);
				GameObjectController.ObjectList.Clear();
				GameObjectController.ObjectList.AddRange(GameObjectController.ObjectListToHold);
				GameObjectController.ObjectListToHold.Clear();
				GameObjectController.ObjectListToHold.AddRange(temp);
				GameObjectController.CameraDisplacement = new Vector2(0,0);
				GameObjectController.freezetimer = GameObjectController.timer;
				//GameObjectController.AddObjectToDrawList(new Player(18,10));
			}
		}
	}
}















