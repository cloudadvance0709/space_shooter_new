using UnityEngine;
using System.Collections;

public class DestroyBossOnContact : MonoBehaviour
{

		public GameObject myDeathExplosion;
		public GameObject myHitExplosion;
		public int myHealth;
		public int healthDeductionOnHit;
		public int scoreValue;

		private GameController gameController;
		private HealthBar healthBar;

		void Start ()
		{
				gameController = (GameController)GameObject.FindWithTag (Tags.GAME_CONTROLLER).GetComponent<GameController> ();
				healthBar = (HealthBar)GameObject.FindWithTag (Tags.HEALTH_BAR).GetComponent<HealthBar> ();
				healthBar.SetMinMaxHealth (0, myHealth);
		}
	
		void OnTriggerEnter (Collider other)
		{

				if (other.tag == Tags.PLAYER_BOLT) {

						myHealth = Mathf.Clamp (myHealth - healthDeductionOnHit, 0, myHealth);
						healthBar.SetCurrentHealth (myHealth);

						Instantiate (myHitExplosion, other.transform.position, other.transform.rotation);
						Destroy (other.gameObject);
						
						if (myHealth == 0) {
								Instantiate (myDeathExplosion, gameObject.transform.position, gameObject.transform.rotation);
								gameController.AddScore (scoreValue);
								gameController.OnBossKilled ();
								Destroy (gameObject);
						}
					
				}

		}
}
