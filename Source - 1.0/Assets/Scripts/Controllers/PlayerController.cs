using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

		public int maxSpreadShots;
		public int numberOfSpreadShots;
		public ScatterShot scatterShot;
		public float speed;
		public float tilt;
		public Boundary boundary;
		public GameObject shot;
		public Transform shotSpawn;
		public float fireRate;

		public GUIText ScatterShotText {
				get { return scatterShotText;}
				set {
						scatterShotText = value;
						UpdateScatterShotText ();
				}
		}

		private float nextFire;
		private GUIText scatterShotText;
		
		void Start ()
		{
				scatterShot.Init ();
		}

		void Update ()
		{

				if (Input.GetButtonDown ("Fire2")) {
						ScatterShot ();
				} else if (Input.GetButton ("Fire1") && Time.time > nextFire) {
						nextFire = Time.time + fireRate;
						GetComponent<AudioSource>().Play ();
						Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				}

		}

		void FixedUpdate ()
		{

				float moveHorizontal = 0.0f;
				float moveVertical = 0.0f;

				moveHorizontal = Input.GetAxis ("Horizontal");
				moveVertical = Input.GetAxis ("Vertical");
				
				/*if (moveHorizontal == 0.0f && moveVertical == 0.0f) {
						moveWithMouse = true;
						moveHorizontal = Input.GetAxis ("Mouse X");
						moveVertical = Input.GetAxis ("Mouse Y");
				} else {
						moveWithMouse = false;
				} */

				Vector3 movementVector = new Vector3 (moveHorizontal, 0.0f, moveVertical);

				
				GetComponent<Rigidbody>().velocity = movementVector * speed;
				

				GetComponent<Rigidbody>().position = new Vector3
				(
					Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
					0.0f,
					Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
				);

				GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
		}

		public void AddScatterShot ()
		{
				if (numberOfSpreadShots < maxSpreadShots) {
						numberOfSpreadShots++;
						UpdateScatterShotText ();
				
				}
		}

		public void UpdateScatterShotText ()
		{
				scatterShotText.text = "Scatter Shots: " + numberOfSpreadShots;
		}

		private void ScatterShot ()
		{
				
				if (numberOfSpreadShots > 0) {

						scatterShot.Fire (shot, shotSpawn);
						GetComponent<AudioSource>().Play ();
						numberOfSpreadShots--;
						UpdateScatterShotText ();
				}
		}
		
}
