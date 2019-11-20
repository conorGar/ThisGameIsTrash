using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUI_WeaponDisplayBox : MonoBehaviour
{

	public enum Type {
		WEAPON,
		CONSUMABLE
	}

	public Type type;
	public Image weaponIcon;
	// Use this for initialization
	public void Highlight(){
		gameObject.GetComponent<Image>().color = new Color(168,255,165,.5f);
	}

	public void UnHighlight(){
		gameObject.GetComponent<Image>().color = new Color(0,0,0,.5f);
	}
}

