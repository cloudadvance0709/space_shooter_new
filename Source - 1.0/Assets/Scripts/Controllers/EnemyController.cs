using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
	
		public float tilt;
		public GameObject shot;
		public Transform shotSpawn;
		public float fireDelay;
		public float fireStart;
		public float scatterShotChance;
		public ScatterShot scatterShot;
		public float scatterShotDelay;
		public float maxScatterShotChance;
		public float scatterShotChanceIncreaserPerWave;
		public int waveCountForMultiScatterShot;
		public int numberOfScatterShots;
		public float intervalBetweenMultipleScatterShots;
		
		
		private GameController gameController;

		void Start ()
		{
				GameObject controllerObject = GameObject.FindWithTag ("GameController");
				if (controllerObject != null) {
						gameController = (GameController)controllerObject.GetComponent<GameController> ();
				}

				

				int curWaveCount = gameController.GetCurrentWaveCount ();
				scatterShotChance = Mathf.Clamp (scatterShotChance + (scatterShotChanceIncreaserPerWave * curWaveCount), scatterShotChance, maxScatterShotChance);
				
				
				if (Random.Range (0.0f, 1.0f) <= scatterShotChance) {
						scatterShot.Init ();
						if (curWaveCount >= waveCountForMultiScatterShot) {
								for (int i = 0; i < numberOfScatterShots; i++) {
										Invoke ("ScatterShot", scatterShotDelay + (i * intervalBetweenMultipleScatterShots));
								}
						} else {
								Invoke ("ScatterShot", scatterShotDelay);
						}
						
				} else {
						InvokeRepeating ("Fire", fireStart, fireDelay);
				}
				
		}
		

		void ScatterShot ()
		{

				scatterShot.Fire (shot, shotSpawn);
				GetComponent<AudioSource>().Play ();
		}

		void Fire ()
		{

				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				GetComponent<AudioSource>().Play ();
				
		}
}
