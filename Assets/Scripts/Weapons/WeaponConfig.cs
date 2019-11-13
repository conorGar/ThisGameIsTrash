using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "TGIT/WeaponConfig", order = 4)]
public class WeaponConfig : ScriptableObject
{
	public List<WeaponDefinition> weaponList;
}

