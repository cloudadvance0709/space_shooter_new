using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyDescriptor
{
		public GameObject enemy;
		
		public int minWeight;
		public int maxWeight;

		private int curWeight;

		EnemyDescriptor ()
		{
				curWeight = 0;
		}

		public void SetCurWeight (int weight)
		{
				curWeight = weight;
		}

		public int GetCurWeight ()
		{
				return curWeight;
		}
	
}
