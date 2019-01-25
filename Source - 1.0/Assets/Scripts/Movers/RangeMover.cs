using UnityEngine;
using System.Collections;

public class RangeMover : MonoBehaviour
{

		public float minSpeed;
		public float maxSpeed;

		void Start ()
		{
				GetComponent<Rigidbody>().velocity = transform.forward * Random.Range (minSpeed, maxSpeed);

		}
}
