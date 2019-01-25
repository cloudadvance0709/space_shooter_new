using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{

		public Vector2 size;
	
		private Texture2D fullTex;
		private int minHealth, maxHealth;
		private float healthPercent;
	
		void Start ()
		{
				fullTex = InitFullTex ();
		}
		
		void OnGUI ()
		{

				Vector3 screenPosition = Camera.main.WorldToScreenPoint (gameObject.transform.position);
				float widthOfHealthBar = size.x * healthPercent;
				GUI.BeginGroup (new Rect (screenPosition.x, Screen.height - screenPosition.y, widthOfHealthBar, size.y));
				GUI.DrawTexture (new Rect (0, 0, widthOfHealthBar, size.y), fullTex, ScaleMode.StretchToFill);
				GUI.EndGroup ();
		}

		public void SetMinMaxHealth (int minHealth, int maxHealth)
		{
				this.minHealth = minHealth;
				this.maxHealth = maxHealth;
				SetCurrentHealth (maxHealth);
		}

		public void SetCurrentHealth (int health)
		{
				healthPercent = health / (float)(maxHealth - minHealth);
		}
	
		private Texture2D InitFullTex ()
		{
		
				Texture2D tex = new Texture2D ((int)size.x, (int)size.y);
		
				Color[] colors = tex.GetPixels ();
		
				for (int i = 0; i < colors.Length; i++) {
			
						//Fully red
						colors [i] = Color.red;
				}
		
				tex.SetPixels (colors);
				tex.Apply ();
				return tex;
		}
}
