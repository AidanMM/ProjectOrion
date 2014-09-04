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
	public class Player : BasicObject
	{
		float Jump;
		public float LeftRightMovementSpeed;
		int directionFacing = 1;
		int shootTimer = 0;
		int fireRate = 10;
		//Vector2 startingPosition;
		public Player(float jump, float moveSpeed) : base(new Texture2D("Application/sprites/Player.png", false), "Player")
		{
			Jump = jump;
			LeftRightMovementSpeed = moveSpeed;
			CameraFocus = false;
			Sprite.Position = new Vector2(AppMain.screenCenterX, AppMain.screenCenterY);
			HasGravity = true;
			IsMovingObject = true;
			
		}
		
		public override void Update()
		{
			OldPosition = Sprite.Position;
			if( (Input2.GamePad.GetData(0).R.Press || Input2.GamePad.GetData(0).L.Press || Input2.GamePad.GetData(0).Cross.Press) && CanJump == true)
			{
				CanJump = false;
				Velocity.Y = Jump;
			}
			if(Input2.GamePad.GetData(0).Select.Press)
			{
				AppMain.Quit = true;	
			}
			
			if(Input2.GamePad.GetData(0).Left.Down && wallCollision == false)
			{
				Velocity.X = -LeftRightMovementSpeed;
				directionFacing = -1;
			}
			else if(Input2.GamePad.GetData(0).Right.Down && wallCollision == false)
			{
				Velocity.X = LeftRightMovementSpeed;
				directionFacing = 1;
				
			}
			else
			{
				Velocity.X = 0;	
			}
			if(Input2.GamePad.GetData(0).Down.Down)
			{
				directionFacing = 2;	
			}
			else if(Input2.GamePad.GetData(0).Up.Down)
			{
				directionFacing = 0;	
			}
			if(Input2.GamePad.GetData(0).Triangle.Down || Input2.GamePad.GetData(0).AnalogRight.X != 0 || Input2.GamePad.GetData(0).AnalogRight.Y != 0)
			{
				
				if(shootTimer % fireRate == 0)
				{
					if(Input2.GamePad.GetData(0).AnalogRight.X != 0 || Input2.GamePad.GetData(0).AnalogRight.Y != 0)
					{
						Vector2 temp = new Vector2(Input2.GamePad.GetData(0).AnalogRight.X, -(Input2.GamePad.GetData(0).AnalogRight.Y));
						temp.Normalize();
						Shoot(temp);
					}
					else if(directionFacing != 0 && directionFacing != 2)
					{
						Shoot(new Vector2(directionFacing,0));	
					}
					else if(directionFacing == 0)
					{
						Shoot(new Vector2(0, 1));
					}
					else if(directionFacing == 2)
					{
						Shoot(new Vector2(0 , -1));	
					}
				}
				shootTimer++;
			}
			else if(!Input2.GamePad.GetData(0).Triangle.Down)
			{
				shootTimer  = fireRate;	
			}
			if(Velocity.Y != 0)
			{
			IsCollided = false;	
			}
			if(Velocity.X == 0 && wallCollision == false)
			{
				Velocity.X = Input2.GamePad.GetData(0).AnalogLeft.X * LeftRightMovementSpeed;
			}
			if(Velocity.X < 0)
			{
				Sprite.FlipU = true;	
			}
			if(Velocity.X > 0)
			{
				Sprite.FlipU = false;	
			}
			Sprite.Position += Velocity;
			
			
			if(AppMain.GameObjectController.CheckCollisionByName(this, "Portal") != null)
			{
				AppMain.GameObjectController.CheckCollisionByName(this, "Portal").ClassSpecific();
			}
			
			wallCollision = false;
			
		}
		/// <summary>
		/// Shoot the specified direction.
		/// </summary>
		/// <param name='direction'>
		/// Direction.
		/// </param>
		public void Shoot(Vector2 direction)
		{
			AppMain.GameObjectController.AddObjectToDrawList(new Bullet((direction) * 40,Sprite.Position , 55));
		}
	}
}

