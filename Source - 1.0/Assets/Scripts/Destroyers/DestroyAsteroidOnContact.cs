using UnityEngine;
using System.Collections;

public class DestroyAsteroidOnContact : MonoBehaviour
{

		private GameObject myExplosion;
		private GameController gameController;
		private DestroyByBoundary destroyByBoundary;


		void Start ()
		{
				gameController = GameObject.FindWithTag (Tags.GAME_CONTROLLER).GetComponent<GameController> ();
				destroyByBoundary = (DestroyByBoundary)GameObject.FindWithTag (Tags.BOUNDARY).GetComponent<DestroyByBoundary> ();
				destroyByBoundary.OnEnemyCreated ();
		}

		void OnDestroy ()
		{
				destroyByBoundary.OnEnemyDestroyed ();
		}
	
		void OnTriggerEnter (Collider other)
		{
				string theirTag = other.tag;

				if (theirTag == Tags.BOUNDARY || theirTag == Tags.PLAYER || theirTag == Tags.ENEMY_SHIP || theirTag == Tags.FORMATION) {
						// Since player & enemy have higher priority, they will take responsibility for the collision
						return;
				}

				GameObject theirExplosion = null;
				int myScore = 0;
				
				if (theirTag == Tags.ASTEROID) {
						EnemyDeath enemyDeath = other.gameObject.GetComponent<EnemyDeath> ();
						if (enemyDeath == null) {
								throw new SpaceShooterException ("No Enemy death added to enemy type " + other.gameObject);
						}
						theirExplosion = enemyDeath.myExplosion;
				}

				if (theirTag == Tags.PLAYER_BOLT || theirTag == Tags.ENEMY_BOLT) {
						EnemyDeath myDeath = gameObject.GetComponent<EnemyDeath> ();
						if (myDeath == null) {
								throw new SpaceShooterException ("No Enemy Death added to enemy type " + gameObject);
						}
						myExplosion = myDeath.myExplosion;
						if (theirTag == Tags.PLAYER_BOLT) {
								myScore = myDeath.myScore;
						}
						
				}

				if (theirExplosion != null) {
						Instantiate (theirExplosion, other.transform.position, other.transform.rotation);
				}

				if (myExplosion != null) {
						Instantiate (myExplosion, transform.position, transform.rotation);
				}

				if (myScore > 0) {
						gameController.AddScore (myScore);
						gameController.KilledEnemy ();
				}

				Destroy (other.gameObject);
				Destroy (gameObject);
		}
}
