using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
		public List<EnemyDescriptor> enemies;
		public int enemyDifficultyFactor;
		public float hazardMultiplier;
		public int maxHazardCount;
		public Vector3 spawnValues;
		public Vector2 spawnBoundary;
		public Transform playerSpawn;
		public GameObject playerObjectType;
		public GameObject boss;
		public Transform bossSpawnLocation;
		public float spawnBias;
		public int startHazardCount;
		public float spawnWait;
		public float minSpawnWait;
		public float spawnWaitDiff;
		public int totalEnemyWeight;
		public float wavesToDecreaseSpawnWait;
		public int waitToSpawnPlayerAfterBossHit;
		public int wavesToSpawnBoss;
		public float startWait;
		public int waveWait;
		public int scoreIntervalForLife;
		public GUIText scoreText;
		public GUIText restartText;
		public GUIText gameOverText;
		public GUIText exitText;
		public GUIText livesText;
		public GUIText wavesText;
		public GUIText versionText;
		public GUIText spreadShotText;
		public int spreadShotAddEnemyCount;
		public int numberOfLives;
		public int maxLives;
		private int score;
		private bool gameOver;
		private bool restart;
		private bool hasKilledPlayer;
		private bool bossCreated;
		private PlayerController playerController;
		private BossController bossController;
		private GameObject currentPlayerObject;
		private int killedEnemyCount;
		private int lastQuotientOfScoreFactor;
		private int[] enemyWeights;
		private int waveCount;
		private int spreadShotCount;
		private int noOfBossesCreated;
		private EnemyDescriptorComparator enemyDescriptorComparator;

		void Start ()
		{
				
				SpawnPlayer ();
				score = 0;
				killedEnemyCount = 0;
				lastQuotientOfScoreFactor = 0;
				waveCount = 0;
				spreadShotCount = 0;
				noOfBossesCreated = 0;
				gameOver = false;
				restart = false;
				hasKilledPlayer = false;
				bossCreated = false;
				
				if (enemyWeights == null) {
						
						enemyWeights = new int[enemies.Count];

				}

				if (enemyDescriptorComparator == null) {
						enemyDescriptorComparator = new EnemyDescriptorComparator ();
				}
				
				restartText.text = "";
				gameOverText.text = "";
				exitText.text = "";
				wavesText.text = "";
				versionText.text = "Version: " + Config.VERSION_NAME;
				RandomizeNextWave ();
				
				UpdateScore ();
				UpdateLives ();
				UpdateWaves ();
				
				StartCoroutine (SpawnWaves ());

		}

		public int GetCurrentWaveCount ()
		{
				return waveCount;
		}

		public void OnBossCreated (BossController bossController)
		{
				this.bossController = bossController;
				bossCreated = true;
				noOfBossesCreated++;
		}

		public int NumberOfBossesCreated ()
		{
				return noOfBossesCreated;
		}

		public void OnBossDestroyed ()
		{
				bossController = null;
				bossCreated = false;
				//waveCount++;
		}

		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {
						Application.Quit ();
				}
				if (restart) {
						if (Input.GetKeyDown (KeyCode.R)) {
								Application.LoadLevel (Application.loadedLevel);
						} else if (Input.GetKeyDown (KeyCode.E)) {
								Application.Quit ();
						}
				}

				if (currentPlayerObject != null) {
						spawnValues.x = currentPlayerObject.transform.position.x;
				}
		}

		void CheckAndUpdateSpawnWait ()
		{
				if (spawnWait > minSpawnWait && waveCount % wavesToDecreaseSpawnWait == 0) {
						spawnWait -= spawnWaitDiff;
						
				}
		}

		IEnumerator SpawnWaves ()
		{
				yield return new WaitForSeconds (startWait);
				
				while (true) {
						
						if (startHazardCount == 0) {
								yield return new WaitForSeconds (startWait);
						}

						if (bossCreated) {
								yield return new WaitForSeconds (startWait);
								continue;
						}

						waveCount++;
						UpdateWaves ();
						if (waveCount > 0 && waveCount % wavesToSpawnBoss == 0) {
								

								SpawnBoss ();
								
								yield return new WaitForSeconds (waveWait);
						} else {
								
								CheckAndUpdateSpawnWait ();
								
								for (int i = 0; i < startHazardCount; i++) {
										if (gameOver || hasKilledPlayer) {
												if (hasKilledPlayer)
														yield return new WaitForSeconds (waveWait); //Give a little time for current enemies to get away from the screen before spawning the player
												break;
										}
										float spawnX = Mathf.Clamp (Random.Range (spawnValues.x - spawnBias, spawnValues.x + spawnBias), spawnBoundary.x, spawnBoundary.y);
										Vector3 spawnPosition = new Vector3 (spawnX, spawnValues.y, spawnValues.z);
										Quaternion spawnRotation = Quaternion.Euler (0.0f, 180.0f, 0.0f);
										Instantiate (GetNextObstacle (), spawnPosition, spawnRotation);
					
										yield return new WaitForSeconds (spawnWait);
								}
				
								if (gameOver) {
					
										restartText.text = "Press 'R' for Restart";
										exitText.text = "Press 'E' for Exit";
										restart = true;
										break;
								} else if (!hasKilledPlayer) {
										if (startHazardCount < maxHazardCount) {
												startHazardCount = Mathf.Clamp ((int)(hazardMultiplier * startHazardCount), 0, maxHazardCount);
										}
					
										RandomizeNextWave ();
					
										yield return new WaitForSeconds (waveWait);
								}
						}
						
				}
				
		}

		void SpawnPlayer ()
		{
				currentPlayerObject = (GameObject)Instantiate (playerObjectType, playerSpawn.position, playerSpawn.rotation);
				playerController = currentPlayerObject.GetComponent<PlayerController> ();
				if (playerController == null) {
						Debug.Log ("Can't find player controller script");
				}
				playerController.ScatterShotText = spreadShotText;

				if (bossCreated) {
						bossController.ResumeShooting ();
				}
		}

		private GameObject GetNextObstacle ()
		{
				int curEnemyWeight = Random.Range (1, 101);

				for (int i = 0; i < enemyWeights.Length; i++) {

						if (curEnemyWeight <= enemyWeights [i]) {
								return enemies [i].enemy;
						}

				}
				// Should never come here
				return enemies [0].enemy;
				
		}
		private void RandomizeNextWave ()
		{
				int totalGeneratedWeight = 0;
				int curEnemyWeight = 0;
				for (int i = 0; i < enemies.Count; i++) {

						EnemyDescriptor enemyDescriptor = enemies [i];

						if (totalGeneratedWeight == totalEnemyWeight) {
								//Debug.Log ("Max Weight reached, setting remaining weights to 0");
								for (int j = i; j < enemies.Count; j++) {
										enemyDescriptor = enemies [i];
										enemyDescriptor.SetCurWeight (0);

								}
								break;
						} else if (i == enemies.Count - 1) {
								curEnemyWeight = totalEnemyWeight - totalGeneratedWeight;
								enemyDescriptor.SetCurWeight (curEnemyWeight);
								break;
						}

						curEnemyWeight = Mathf.Clamp (Random.Range (enemyDescriptor.minWeight, enemyDescriptor.maxWeight + 1), enemyDescriptor.minWeight, totalEnemyWeight - totalGeneratedWeight);
						totalGeneratedWeight += curEnemyWeight;
						//Debug.Log ("Current generated weight:" + curEnemyWeight + " Total generated weight:" + totalGeneratedWeight);
						enemyDescriptor.SetCurWeight (curEnemyWeight);
				}

				CalculateEnemyWeights ();

				
		}

		private void CalculateEnemyWeights ()
		{
				enemies.Sort (enemyDescriptorComparator);
				
				int eachEnemyWeight = 0;
				for (int i = 0; i < enemies.Count; i++) {

						eachEnemyWeight += enemies [i].GetCurWeight ();
						//Debug.Log ("Enemy " + i + " is of type " + enemies [i].enemy + " and has weight " + eachEnemyWeight);
						enemyWeights [i] = eachEnemyWeight;

				}
		}

		void UpdateScore ()
		{
				scoreText.text = "Score: " + score;
		}

		void UpdateLives ()
		{
				livesText.text = "Lives: " + numberOfLives;
		}

		void UpdateWaves ()
		{
				wavesText.text = "Waves: " + waveCount;
		}

		public void KilledEnemy ()
		{

				if (++killedEnemyCount == spreadShotAddEnemyCount) {
						killedEnemyCount = 0;
						playerController.AddScatterShot ();
				}
		}

		public void OnBossKilled ()
		{
				Debug.Log ("Boss Killed!");
		}

		public void AddScore (int newScoreValue)
		{
				score += newScoreValue;
				UpdateScore ();
				
				if (numberOfLives == maxLives) {
						return;
				}
				if (score / scoreIntervalForLife > lastQuotientOfScoreFactor) {
						lastQuotientOfScoreFactor = score / scoreIntervalForLife;
						numberOfLives++;
						//Debug.Log ("Increase Lives To: " + numberOfLives);
						UpdateLives ();
				}
				
				
		}

		private void SpawnBoss ()
		{
				Instantiate (boss, bossSpawnLocation.position, boss.GetComponent<Rigidbody>().rotation);
		}

		public void PlayerHit (GameObject playerExplosion, Transform otherPosition)
		{
				if (Config.DEBUG_MODE && Config.INF_LIVES) {
						return;
				}		
				numberOfLives--;

				if (numberOfLives == 0) {
						
						if (numberOfLives < 0) {
								numberOfLives = 0;
						}
						GameOver ();
						if (bossCreated) {
								bossController.StopShooting ();
						} 
						
				}
				Instantiate (playerExplosion, otherPosition.position, otherPosition.rotation);
				spreadShotCount = playerController.numberOfSpreadShots;
				//Debug.Log ("Number of scatter shots before destroying:" + spreadShotCount);
				Destroy (currentPlayerObject);
				hasKilledPlayer = true;
				UpdateLives ();

				if (!gameOver && bossCreated) {
						bossController.PauseShooting ();
						//SpawnPlayer ();
						Invoke ("SpawnPlayer", waitToSpawnPlayerAfterBossHit);
						
				}
		}

		private void GameOver ()
		{

				gameOverText.text = "Game Over";
				gameOver = true;
		}

		public IEnumerator OnAllEnemiesleftScreen ()
		{
				if (hasKilledPlayer && !gameOver) {
						SpawnPlayer ();
						playerController.numberOfSpreadShots = spreadShotCount;
						playerController.UpdateScatterShotText ();
						hasKilledPlayer = false;
						yield return new WaitForSeconds (spawnWait);
				}
		}
}
