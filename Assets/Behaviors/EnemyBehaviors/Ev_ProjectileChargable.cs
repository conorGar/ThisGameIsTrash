using UnityEngine;
using System.Collections;

public class Ev_ProjectileChargable : Ev_ProjectileBasic
{

	public bool charged;

	void OnDisable(){
		charged = false;
	}
}

