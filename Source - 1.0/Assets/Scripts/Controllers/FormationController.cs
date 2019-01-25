using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour
{

		public GameObject[] enemies = new GameObject[3];
		public Transform[] spawnPositions = new Transform[3];
		private FormationMover formationMover;

		void Start ()
		{
				FormationMover formationMover = (FormationMover)gameObject.GetComponent<FormationMover> ();
				for (int i = 0; i < enemies.Length; i++) {
						Transform transform = spawnPositions [i];
						GameObject enemy = (GameObject)Instantiate (enemies [i], transform.position, transform.rotation);

						if (formationMover != null) {
								formationMover.SetMovementSpeedForMoveable (enemy);
						}
				}
		}

		void OnTriggerEnter (Collider other)
		{
				return;
		}
}
