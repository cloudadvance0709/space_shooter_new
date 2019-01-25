using UnityEngine;
using System.Collections;

public class BossMover : MonoBehaviour
{

		public float speed;
		public float bossStopZ;

		void Start ()
		{
				GetComponent<Rigidbody>().velocity = transform.forward * speed;

		}

		void FixedUpdate ()
		{
				if (GetComponent<Rigidbody>().position.z <= bossStopZ) {
						GetComponent<Rigidbody>().velocity = Vector3.zero;
						Destroy (this);
				}
				
		}
}
