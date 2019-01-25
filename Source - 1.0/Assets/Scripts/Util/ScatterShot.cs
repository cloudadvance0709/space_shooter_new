using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScatterShot
{
		public int scatterShotCount;
		public float scatterShotArc;
		private float spreadShotIndividualArc;
		private int spreadShotsOnEachSide;

		

		public void Init ()
		{
				spreadShotsOnEachSide = (int)scatterShotCount / 2;
				spreadShotIndividualArc = (scatterShotArc / 2) / spreadShotsOnEachSide;
		}
	
		public void Fire (GameObject shot, Transform shotSpawn)
		{
				Quaternion rotation = shotSpawn.rotation;
		
				//Shots on Left and right side
				for (int i = 1; i <= spreadShotsOnEachSide; i++) {
						Object.Instantiate (shot, shotSpawn.position, Quaternion.Euler (rotation.x, rotation.y - (i * spreadShotIndividualArc), rotation.z));
						Object.Instantiate (shot, shotSpawn.position, Quaternion.Euler (rotation.x, rotation.y + (i * spreadShotIndividualArc), rotation.z));
				}
		
				//Middle shot in case shot count is odd
				if (scatterShotCount % 2 == 1) {
						Object.Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				}
		}


}
