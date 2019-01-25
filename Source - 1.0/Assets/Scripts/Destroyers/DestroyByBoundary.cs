using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
		private int onScreenEnemyCount;
		private GameController gameController;

		void Start ()
		{
				onScreenEnemyCount = 0;
				gameController = (GameController)GameObject.FindWithTag (Tags.GAME_CONTROLLER).GetComponent<GameController> ();
		}
		void OnTriggerExit (Collider other)
		{
				if (other.tag != Tags.PLAYER && other.tag != Tags.BOSS_ENEMY) {
						Destroy (other.gameObject);
				}
		}
	
		public void OnEnemyCreated ()
		{
				onScreenEnemyCount++;
				
		}

		public void OnEnemyDestroyed ()
		{
				onScreenEnemyCount--;
				
				if (onScreenEnemyCount == 0) {
						
						StartCoroutine (gameController.OnAllEnemiesleftScreen ());
						
				}
		}

}
