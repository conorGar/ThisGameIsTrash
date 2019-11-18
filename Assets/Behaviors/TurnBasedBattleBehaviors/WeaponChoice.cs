using UnityEngine;
using System.Collections;

public class WeaponChoice : MonoBehaviour
{

	public tk2dSprite weaponSprite;
	public tk2dTextMesh damageNumDisplay;
	public enum WEAPON_TYPE {
		BASIC,
		HEAL,
		BLOCK
	}

	public WEAPON_TYPE myWeaponType = WEAPON_TYPE.BASIC;

	[HideInInspector]
	public int damage;



	public virtual void DefineValues(WeaponDefinition weapontype){
		
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

