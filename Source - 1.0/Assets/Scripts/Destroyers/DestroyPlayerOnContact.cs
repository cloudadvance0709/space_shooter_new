using UnityEngine;
using System.Collections;

public class DestroyPlayerOnContact : MonoBehaviour
{

		public GameObject myExplosion;
		private GameController gameController;
	
		void Start ()
		{
				GameObject gameControllerObject = GameObject.FindWithTag (Tags.GAME_CONTROLLER);
				if (gameControllerObject != null) {
			
						gameController = gameControllerObject.GetComponent <GameController> ();
				}
		
				if (gameController == null) {
						Debug.Log ("Cannot find 'GameController' script");
				}

	
		}
	
		void OnTriggerEnter (Collider other)
		{
				string theirTag = other.tag;

				if (theirTag == Tags.BOUNDARY || theirTag == Tags.FORMATION) {
						return;
				}

				GameObject theirExplosion = null;
				int scoreValue = 0;
				if (theirTag == Tags.ENEMY_SHIP || theirTag == Tags.ASTEROID) {
						EnemyDeath enemyDeath = other.gameObject.GetComponent<EnemyDeath> ();
						if (enemyDeath == null) {
								throw new SpaceShooterException ("No enemy death script added for enemy:" + other.gameObject);
						}
						theirExplosion = enemyDeath.myExplosion;
						scoreValue = enemyDeath.myScore;

						if (theirTag == Tags.ENEMY_SHIP) {
								gameController.KilledEnemy ();
						}
				} else 

				if (theirExplosion != null) {
						Instantiate (theirExplosion, other.transform.position, other.transform.rotation);
				}

				if (scoreValue > 0) {
						gameController.AddScore (scoreValue);
				}

				Destroy (other.gameObject);

				gameController.PlayerHit (myExplosion, transform);

				
		}
}
