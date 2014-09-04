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
	public class ObjectController
	{
		/// <summary>
		/// The object list.
		/// </summary>
		public List<BasicObject> ObjectList = new List<BasicObject>();
		/// <summary>
		/// The force of gravity.
		/// </summary>
		float ForceOfGravity = 1.5f;
		/// <summary>
		/// The camera displacement.
		/// </summary>
		public Vector2 CameraDisplacement;
		/// <summary>
		/// The camera object.
		/// </summary>
		public static int CameraObject;
		public List<BasicObject> ObjectListToHold = new List<BasicObject>();
		public int freezetimer = 0;
		
		public int EnemyCount;
		public int timer = 60;
		
		public List<BasicObject[]> CollidedObjects = new List<BasicObject[]>();
			
			
	 	/// <summary>
		/// Initializes a new instance of the <see cref="ProjectOrion.ObjectController"/> class.
		/// </summary>
		public ObjectController ()
		{
			CameraDisplacement = new Vector2(100,0);
			EnemyCount = 0;
			
		}
		/// <summary>
		/// Updates the object list.
		/// </summary>
		public  void UpdateObjectList()
		{
			timer++;
			if(timer > freezetimer + 45)
			{
				for(int i =0; i < ObjectList.Count; i++)
				{
	
					ObjectList[i].Update();	
					ObjectList[i].Sprite.Position += CameraDisplacement;
				}
				DetectCollisions();
				CameraDisplacement -= ObjectList[CameraObject].Sprite.Position - ObjectList[CameraObject].OldPosition;
				ObjectList[CameraObject].Sprite.Position = new Vector2(AppMain.screenCenterX, ObjectList[CameraObject].Sprite.Position.Y);
				CleanUpGame();	
			}


		}
		/// <summary>
		/// Draws the object list.
		/// </summary>
		public void DrawObjectList()
		{
			for(int i =0; i < ObjectList.Count; i++)
			{
				AppMain.SceneList[AppMain.CurrentScene].AddChild(ObjectList[i].Sprite);
			}
			FocusCamera("Player");
		}
		/// <summary>
		/// Adds the object to draw list.
		/// </summary>
		/// <param name='ObjectToAdd'>
		/// Object to add.
		/// </param>
		public void AddObjectToDrawList(BasicObject ObjectToAdd)
		{
			ObjectList.Add(ObjectToAdd);
			AppMain.SceneList[AppMain.CurrentScene].AddChild(ObjectToAdd.Sprite);
			
		}
		
		
		/// <summary>
		/// Detects collisions.
		/// </summary>
		public void DetectCollisions()
		{
			//Clear the collided Objects list
			CollidedObjects.Clear();
			//Loop through all of the basic objects
			for(int i = 0; i < ObjectList.Count; i++)
			{
				if(ObjectList[i].IsMovingObject == true)
				{
					//Vector2 DifferenceInPosition;
					bool CollisionHappened = false;
					for(int j = 0; j < ObjectList.Count; j++)
					{
						if(j != i )
						{
							//Begin the actual collision checks. Start by creating a vector, that is the distance between two game objects
							Vector2 DistanceBetweenObjects = new Vector2(FMath.Abs(ObjectList[i].Sprite.Position.X - ObjectList[j].Sprite.Position.X), FMath.Abs(ObjectList[i].Sprite.Position.Y - ObjectList[j].Sprite.Position.Y));
							if( DistanceBetweenObjects.X <= 50 + ObjectList[j].Sprite.TextureInfo.TextureSizef.X && DistanceBetweenObjects.Y <= 50 + ObjectList[j].Sprite.TextureInfo.TextureSizef.Y)
							{
								
								//Check for collision using squares deterimined by the size of that objects textures.
								
								if( DistanceBetweenObjects.X < (ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2  ) + (ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2) && DistanceBetweenObjects.Y < (ObjectList[i].Sprite.TextureInfo.TextureSizef.Y / 2  )+ (ObjectList[j].Sprite.TextureInfo.TextureSizef.Y / 2))
								{
									if(ObjectList[i].GhostCollision == false && ObjectList[j].GhostCollision == false)
									{
										float Y = ObjectList[i].Sprite.Position.Y;
										float X = ObjectList[i].Sprite.Position.X;
										if(  
										     (ObjectList[i].OldPosition.X >= ObjectList[j].Sprite.Position.X + ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2 ||
										   ObjectList[i].OldPosition.X <= ObjectList[j].Sprite.Position.X - ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2  )&& (
											FMath.Abs(DistanceBetweenObjects.Y) <= ObjectList[j].Sprite.TextureInfo.TextureSizef.Y / 2 ) )
										{
											if(ObjectList[i].Sprite.Position.X < ObjectList[j].Sprite.Position.X)
											{
												X = ObjectList[j].Sprite.Position.X - ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2 - ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2;
												CameraDisplacement.X = 0;
											}
											else
											{
												X = ObjectList[j].Sprite.Position.X + ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2 + ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2;
												CameraDisplacement.X = 0;
											}
											ObjectList[i].Velocity.X = 0;
											ObjectList[i].wallCollision = true;
											//ObjectList[i].Velocity.Y -= 5;
											
											
											
										}
										else if(ObjectList[i].OldPosition.Y > ObjectList[j].Sprite.Position.Y)
										{
											if(ObjectList[i].OldPosition.X + ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2 > ObjectList[j].Sprite.Position.X - ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2 && ObjectList[i].OldPosition.X - ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2 < ObjectList[j].Sprite.Position.X + ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2 && ObjectList[i].Velocity.Y < 0)
											{
												Y =  ObjectList[j].Sprite.Position.Y + ObjectList[j].Sprite.TextureInfo.TextureSizef.Y / 2 + ObjectList[i].Sprite.TextureInfo.TextureSizef.Y / 2;
												ObjectList[i].Velocity.Y = 0;	
												ObjectList[i].CanJump = true;
												ObjectList[i].IsCollided = true;
											}
											
											
										}
										else if(ObjectList[i].Velocity.Y > 0)
										{
											Y =  ObjectList[j].Sprite.Position.Y - ObjectList[j].Sprite.TextureInfo.TextureSizef.Y / 2 - ObjectList[i].Sprite.TextureInfo.TextureSizef.Y / 2;
											ObjectList[i].Velocity.Y = -0.000000000000000001f;
											ObjectList[i].IsCollided = true;
										}
										ObjectList[i].Sprite.Position = new Vector2(X, Y);
									}
									
									
									BasicObject[] CollidedObjectsArray = new BasicObject[2];
									CollidedObjectsArray[0] = ObjectList[i];
									CollidedObjectsArray[1] = ObjectList[j];
									CollidedObjects.Add(CollidedObjectsArray);
									
									
									CollisionHappened = true;
									if(ObjectList[i].GhostCollision == true)
									{
										ObjectList[i].IsCollided = true;	
									}
									
								}
								if(CollisionHappened == false)
								{
									if(ObjectList[i].Sprite.Position.Y == ObjectList[j].Sprite.Position.Y + ObjectList[j].Sprite.TextureInfo.TextureSizef.Y )
									{
										if(ObjectList[i].Sprite.Position.X - ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2 < ObjectList[j].Sprite.Position.X + ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2  && ObjectList[i].Sprite.Position.X + ObjectList[i].Sprite.TextureInfo.TextureSizef.X / 2 > ObjectList[j].Sprite.Position.X - ObjectList[j].Sprite.TextureInfo.TextureSizef.X / 2)
										CollisionHappened = true;	
									}
								}
							}
						}
					}
					if(CollisionHappened == false)
					{
						ObjectList[i].IsCollided = false;	
					}
					if(ObjectList[i].HasGravity == true && ObjectList[i].IsCollided == false)
					{
						ObjectList[i].Velocity.Y -= ForceOfGravity;
					}	
				}
			}
		}
		/// <summary>
		/// Finds an object given its name
		/// </summary>
		/// <returns>
		/// The object with name.
		/// </returns>
		/// <param name='NameToFind'>
		/// Name to find.
		/// </param>
		public BasicObject FindObjectWithName(string NameToFind)
		{
			for(int i = 0; i < ObjectList.Count; i++)
			{
				if(NameToFind == ObjectList[i].NameOfObject)
				{
					return ObjectList[i];	
				}
			}
			return null;
		}
		/// <summary>
		/// Finds the name of the objects with same.
		/// </summary>
		/// <returns>
		/// The objects with same name.
		/// </returns>
		/// <param name='NameToFind'>
		/// Name to find.
		/// </param>
		public List<BasicObject> FindObjectsWithSameName(string NameToFind)
		{
			List<BasicObject> ObjectsWithSameName = new List<BasicObject>();
			for(int i = 0; i < ObjectList.Count; i++)
			{
				if(NameToFind == ObjectList[i].NameOfObject)
				{
				ObjectsWithSameName.Add(ObjectList[i]);	
				}
			}
			return ObjectsWithSameName;
		}
		/// <summary>
		/// Focuses the camera.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public void FocusCamera(string name)
		{
			BasicObject newCameraFocus = FindObjectWithName(name);
			newCameraFocus.CameraFocus = true;
			for(int i =0; i < ObjectList.Count; i++)
			{
				if(ObjectList[i].CameraFocus == true)
				{
					CameraObject = i;	
				}
			}
		}
		/// <summary>
		/// Cleans up game.
		/// </summary>
		public void CleanUpGame()
		{
		int Maximum = ObjectList.Count;
			for(int i = 0; i < Maximum; i++)
			{
 	 			if(ObjectList[i].Destroyed == true)
				{
					AppMain.SceneList[AppMain.CurrentScene].RemoveChild(ObjectList[i].Sprite, true);
					ObjectList.RemoveAt(i);
					i--;
					Maximum--;
				}
			}
				    
		}
		/// <summary>
		/// Checks the collisions based on name.
		/// </summary>
		/// <returns>
		/// The collision by name.
		/// </returns>
		/// <param name='Object1'>
		/// If set to <c>true</c> object1.
		/// </param>
		/// <param name='Object2'>
		/// If set to <c>true</c> object2.
		/// </param>
		public BasicObject CheckCollisionByName(BasicObject Object1, string Object2)
		{
			
			
			//List<BasicObject> ObjectListToCheck;
			/*
			for(int i = 0; i < ObjectList.Count; i++)
			{
					if(ObjectList[i].NameOfObject == Object1)
				{
				ObjectOneIndex = i;
					break;
				}
			}*/
			
			
			/*
			ObjectListToCheck = FindObjectsWithSameName(Object2);
				for(int i = 0; i < ObjectListToCheck.Count; i++)
				{
					Vector2 DistanceBetweenObjects = new Vector2(FMath.Abs(Object1.Sprite.Position.X - ObjectListToCheck[i].Sprite.Position.X), FMath.Abs(Object1.Sprite.Position.Y - ObjectListToCheck[i].Sprite.Position.Y));
					
					if( DistanceBetweenObjects.X <= (Object1.Sprite.TextureInfo.TextureSizef.X / 2 ) + (ObjectListToCheck[i].Sprite.TextureInfo.TextureSizef.X / 2) && DistanceBetweenObjects.Y <= (Object1.Sprite.TextureInfo.TextureSizef.Y / 2 - 5)+ (ObjectListToCheck[i].Sprite.TextureInfo.TextureSizef.Y / 2))
					{
						return ObjectListToCheck[i];
					}
				}*/
			//ObjectListToCheck = FindObjectsWithSameName(Object2);
			for(int i = 0; i < CollidedObjects.Count; i++)
			{
				if(CollidedObjects[i][0] == Object1 && CollidedObjects[i][1].NameOfObject == Object2 )
				{
					return CollidedObjects[i][1];
				}
			}
			
			
			return null;
		}
		/// <summary>
		/// Moves all objects.
		/// </summary>
		public void MoveAllObjects(Vector2 displacementValue)
		{
			for(int i = 0; i < ObjectList.Count; i++)
			{

				
				ObjectList[i].Sprite.Position += displacementValue;
			}
			ObjectList[CameraObject].Sprite.Position = new Vector2(AppMain.screenCenterX, ObjectList[CameraObject].Sprite.Position.Y);
		}
	}
}

