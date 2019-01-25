using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDescriptorComparator : Comparer<EnemyDescriptor>
{
		public override int Compare (EnemyDescriptor x, EnemyDescriptor y)
		{
				if (x.GetCurWeight () == y.GetCurWeight ()) {
						return 0;
				} else if (x.GetCurWeight () < y.GetCurWeight ()) {
						return -1;
				} else {
						return 1;
				}
		}


}
