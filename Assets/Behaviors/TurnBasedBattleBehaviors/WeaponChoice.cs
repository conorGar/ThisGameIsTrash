using UnityEngine;
using System.Collections;

public class WeaponChoice : MonoBehaviour
{

	public tk2dSprite weaponSprite;
	public tk2dTextMesh damageNumDisplay;

	[HideInInspector]
	public int damage;

	void Awake(){
		
	}

	public void DefineValues(WeaponDefinition weapontype){
		
		weaponSprite.SetSprite(weapontype.sprite);
		damage = weapontype.damage;
		damageNumDisplay.text = damage.ToString();
	}

	public void Highlight(){ //move to 'center' and increase opacity
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);

	}

	public void Unhighlight(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,.5f);

	}
}

