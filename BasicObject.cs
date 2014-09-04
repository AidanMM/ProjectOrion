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
	public class BasicObject
	{
		TextureInfo Textureinfo;
		public SpriteUV Sprite;
		public Vector2 OldPosition;
		public Vector2 Velocity;
		public bool HasGravity = false;
		public bool IsCollided = false;
		public bool IsMovingObject = false;
		public bool GhostCollision = false;
		public bool CanJump = false;
		public bool Destroyed = false;
		public string NameOfObject;
		public bool CameraFocus;
		public bool fadeBool;
		public bool wallCollision = false;
		public int fadeCounter;
		public int timer = 0;
		
		public BasicObject (Texture2D myTexture, string Name)
		{
			CameraFocus = false;
			NameOfObject = Name;
			Textureinfo = new TextureInfo(myTexture);
			Sprite = new SpriteUV(){TextureInfo = Textureinfo};
			Sprite.Quad.S = Textureinfo.TextureSizef;
			Sprite.CenterSprite();
			
		}
		public BasicObject (Texture2D myTexture, string Name, int timeToFade)
		{
			CameraFocus = false;
			NameOfObject = Name;
			Textureinfo = new TextureInfo(myTexture);
			Sprite = new SpriteUV(){TextureInfo = Textureinfo};
			Sprite.Quad.S = Textureinfo.TextureSizef;
			Sprite.CenterSprite();
			fadeBool = true;
			fadeCounter = timeToFade;
			
		}
		/// <summary>
		/// Update this instance.
		/// </summary>
		public virtual void Update()
		{
			Sprite.Position += Velocity;	
			if(Velocity.X < 0)
			{
				Sprite.FlipU = true;	
			}
			if(Velocity.X > 0)
			{
				Sprite.FlipU = false;	
			}
			if(fadeBool == true)
			{
				timer++;
				if(timer >= fadeCounter)
				{
					Destroy();
				}
			}
		}
		
		public void Draw()
		{
			
		}
		/// <summary>
		/// Destroy this instance.
		/// </summary>
		public virtual void Destroy()
		{
			Destroyed = true;
			
			HasGravity = false;
			IsMovingObject = false;
			GhostCollision = true;
			Velocity = new Vector2(0,0);
			Sprite.Position = new Vector2(10, 10);
			
			
		}
		/// <summary>
		/// Roam the specified velocity.
		/// </summary>
		/// <param name='velocity'>
		/// Velocity.
		/// </param>
		public virtual void Roam(float velocity)
		{
			/*
			if(ObjectController.CheckCollisionByName("Ground") == true)
			{
				Velocity.X = velocity;
				Velocity.Y = 0;
				Sprite.Position = new Vector2(Sprite.Position.X, Sprite.Position.Y);
			}
			
			if(IsCollided == true)
			{
				if(HasGravity == true)
				{
				HasGravity = false;
				
				}
				Velocity.Y = 0;
				Sprite.Position = new Vector2(Sprite.Position.X, Sprite.Position.Y + Sprite.Position.Y % 50);
			}
			*/
		}
		
		public virtual void OnCollision()
		{
			
		}
		
		public virtual void ClassSpecific()
		{
			
		}
		
	}
}

