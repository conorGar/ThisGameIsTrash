using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum Weapon : long {

	NONE =                                 0,
    STICK =                   (long)1 << 0,
    POINTED_POLE =            (long)1 << 1,
    CLAW =                    (long)1 << 2
}

public enum SpecialWeapon{

		NONE,
		JIM,
		ROBOT
	} 

[CreateAssetMenu(fileName = "New Weapon", menuName = "TGIT/Weapon", order = 3)]
public class WeaponDefinition : ScriptableObject
{
	//[NonSerialized]



	[HideInInspector]
    public long weaponValue;

	public Weapon Type
    {
        set { weaponValue = (long)value; }
        get { return (Weapon)weaponValue; }
    }

	public string sprite = "sprite_name";
	public Sprite displaySprite;

    public string displayName = "New Weapon";
    public string description = "New Weapon Description";
    public int damage = 1;
    public int health = 99;
    public int weaponPrice = 0;
    public int weight = 1;
    public SpecialWeapon specialWeapon = SpecialWeapon.NONE; //is this weapon only for a specific character

}

