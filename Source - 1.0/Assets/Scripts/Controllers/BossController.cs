using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{

		public Transform leftShotSpawn;
		public Transform rightShotSpawn;
		public Transform scatterShotSpawn;
		public float shootStartDelay;
		public float delayBetweenLeftAndRightShots;
		public float delayBetweenEachSetOfShots;
		public float delayAfterScatterShots;
		public float delayBeforeScatterShots;
		public GameObject shot;
		public ScatterShot scatterShot;
		public int shotRoundsMin;
		public int shotRoundsMax;
		public int scatterShotRoundsMin;
		public int scatterShotRoundsMax;
		public int bossDifficultyCeiling;
		public float maxDifficultyFactor;

		private int straightShotsFired;
		private int numberOfStraightShotsBeforeScatterShots;
		private int currentBossNumber;
		private float difficultyFactor;
		private GameController gameController;
		private bool shootingPaused;


		void Start ()
		{
				straightShotsFired = 0;
				difficultyFactor = 0.0f;
				numberOfStraightShotsBeforeScatterShots = 0;
				scatterShot.Init ();
				gameController = (GameController)GameObject.FindWithTag (Tags.GAME_CONTROLLER).GetComponent<GameController> ();
				gameController.OnBossCreated (this);
				currentBossNumber = gameController.NumberOfBossesCreated ();
				CalculateDifficultyFactor ();

				StartCoroutine ("StartShootingStuff");
				shootingPaused = false;
		}

		void OnDestroy ()
		{

				gameController.OnBossDestroyed ();
		}

		IEnumerator StartShootingStuff ()
		{
				numberOfStraightShotsBeforeScatterShots = DecideNumStraightShots ();
				yield return new WaitForSeconds (shootStartDelay);
				while (true) {
						
						if (!shootingPaused) {
								if (straightShotsFired == numberOfStraightShotsBeforeScatterShots) {
										straightShotsFired = 0;
										yield return new WaitForSeconds (delayBeforeScatterShots);
										int numScatterShots = DecideNumScatterShots ();
										for (int i = 0; i < numScatterShots; i++) {
												scatterShot.Fire (shot, scatterShotSpawn);
												GetComponent<AudioSource>().Play ();
												yield return new WaitForSeconds (delayAfterScatterShots);
										}
					
										numberOfStraightShotsBeforeScatterShots = DecideNumStraightShots ();
					
					
								} else {
										Fire (leftShotSpawn);
										yield return new WaitForSeconds (delayBetweenLeftAndRightShots);
										Fire (rightShotSpawn);
										yield return new WaitForSeconds (delayBetweenEachSetOfShots);
										straightShotsFired += 2;
								}	
						} else {
								yield return new WaitForSeconds (delayAfterScatterShots);
						}
					
				}
		}

		private int DecideNumStraightShots ()
		{
				int count = Random.Range (shotRoundsMin, shotRoundsMax + 1);
				int newCount = (int)(count - (count * difficultyFactor));
				if (newCount % 2 == 1) {
						newCount--;
				}
				
				
				return newCount;
		}

		private int DecideNumScatterShots ()
		{
				int count = Random.Range (scatterShotRoundsMin, scatterShotRoundsMax + 1);
				int newCount = (int)(count + (count * difficultyFactor));
				return newCount;
		}

		private void CalculateDifficultyFactor ()
		{
				difficultyFactor = (currentBossNumber >= bossDifficultyCeiling) ? maxDifficultyFactor : (currentBossNumber / (float)bossDifficultyCeiling);
		}
	
		void Fire (Transform shotSpawn)
		{
		
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				GetComponent<AudioSource>().Play ();
		}

		public void PauseShooting ()
		{
				shootingPaused = true;

		}

		public void ResumeShooting ()
		{
				shootingPaused = false;
		}

		public void StopShooting ()
		{
				shootingPaused = true;
				StopCoroutine ("StartShootingStuff");
		}
}
