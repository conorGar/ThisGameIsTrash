using UnityEngine;
using System.Collections;

public class WeaponChoice : MonoBehaviour
{

	public int Damage;


	public void Highlight(){ //move to 'center' and increase opacity
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);

	}

	public void Unhighlight(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,.5f);

	}
}

