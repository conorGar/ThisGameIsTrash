﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPool : MonoBehaviour
{

	public static BossPool Instance;
	public List<GameObject> bossList = new List<GameObject>();

	void Awake(){
		Instance = this;
		for(int i = 0; i<bossList.Count; i++){
			bossList[i].SetActive(false);
		}
	}

	public void GetPooledBoss(string name, Vector2 spawnPos){
		for(int i = 0; i <bossList.Count; i++){
			if(bossList[i].name.ToLower() == name.ToLower()){
				bossList[i].transform.position = spawnPos;
				bossList[i].SetActive(true);
				break;
			}
		}
	}
}

