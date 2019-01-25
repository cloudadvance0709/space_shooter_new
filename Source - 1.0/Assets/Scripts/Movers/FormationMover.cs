using UnityEngine;
using System.Collections;

public class FormationMover : MonoBehaviour
{

		public float speed;
		
		void Start ()
		{
				GetComponent<Rigidbody>().velocity = transform.forward * speed;
				
		}

		public void SetMovementSpeedForMoveable (GameObject moveable)
		{
				moveable.GetComponent<Rigidbody>().velocity = transform.forward * speed;
		}

}
