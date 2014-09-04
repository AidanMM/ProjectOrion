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
	public class ParticleSystem
	{
		Vector2 particleSpawnPoint;
		string particleTexture;
		public ParticleSystem (Vector2 spawnPosition, string texture)
		{
			particleSpawnPoint = spawnPosition;
			particleTexture = texture;
		}
		
		
	}
}

