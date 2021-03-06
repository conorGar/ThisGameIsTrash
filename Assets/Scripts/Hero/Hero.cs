﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Hero", menuName = "TGIT/Hero", order = 4)]
public class Hero : ScriptableObject
{
	public string heroName;
	public int maxStrength;
	public int strength;
	public int maxHP;
	public int currentHP;
	public int defense;
	public int level;
	public int xp;
	public int speed = 10;
	public List<WeaponDefinition> myEquippedWeapons = new List<WeaponDefinition>();

}

