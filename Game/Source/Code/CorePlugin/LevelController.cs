﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using Duality.Audio;

namespace Game
{
	public class LevelController : Component, ICmpUpdatable
	{
		private float enemySpawnTimer = 2.0f; // Initial hardcoded 2 second delay
		private float enemySpawnDelay = 1.0f;
		private float spawnDist = 500.0f;
		private ContentRef<Prefab> enemyPrefab = null;
		private ContentRef<Sound> backgroundMusic = null;
		private SoundInstance playingMusic = null;

		[DontSerialize]
		private float gameTimer = 0.0f;

		public float GameTimer
		{
			get { return this.gameTimer; }
		}
		public float EnemySpawnDelay
		{
			get { return this.enemySpawnDelay; }
			set { this.enemySpawnDelay = value; }
		}
		public float SpawnDistance
		{
			get { return this.spawnDist; }
			set { this.spawnDist = value; }
		}
		public ContentRef<Prefab> EnemyPrefab
		{
			get { return this.enemyPrefab; }
			set { this.enemyPrefab = value; }
		}
		public ContentRef<Sound> BackgroundMusic
		{
			get { return this.backgroundMusic; }
			set { this.backgroundMusic = value; }
		}

		public void OnUpdate()
		{
			this.enemySpawnTimer -= Time.TimeMult * Time.SPFMult;
			if (this.enemySpawnTimer <= 0.0f)
			{
				this.enemySpawnTimer += this.enemySpawnDelay * MathF.Rnd.NextFloat(0.75f, 1.0f);
				this.SpawnEnemy();
			}

			// Make some eerie space music
			if (this.backgroundMusic != null && this.playingMusic == null || this.playingMusic.Disposed)
			{
				this.playingMusic = DualityApp.Sound.PlaySound(this.backgroundMusic);
				this.playingMusic.Looped = true;
			}

			this.gameTimer += Time.TimeMult * Time.SPFMult;
		}

		private void SpawnEnemy()
		{
			Prefab prefab = this.enemyPrefab.Res;
			if (prefab == null) return;

			GameObject enemyObj = prefab.Instantiate();
			enemyObj.Transform.Pos = new Vector3(MathF.Rnd.NextVector2() * this.spawnDist, 0.0f);
			Scene.Current.AddObject(enemyObj);
		}
	}
}
