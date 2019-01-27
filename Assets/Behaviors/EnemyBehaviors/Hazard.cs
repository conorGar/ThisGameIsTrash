using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour
{
	public int damageToPlayer;
	public bool toxicProjectile;

	void Start(){
		if(toxicProjectile &&GlobalVariableManager.Instance.IsPinEquipped(PIN.IRRADIATED)){
			damageToPlayer--;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

