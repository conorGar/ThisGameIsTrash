﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Hub_UpgradeStand : MonoBehaviour {



	public GameObject selectParticleEffect;
	public GameObject player;
	public GameObject spaceIcon;
	public GameObject upgradeHUD;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(transform.position.x - player.transform.position.x) < 4f &&Mathf.Abs(transform.position.y - player.transform.position.y) < 7f){
			spaceIcon.SetActive(true);
			if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT) && upgradeHUD.activeInHierarchy != true){
				player.GetComponent<EightWayMovement>().enabled = false;
				upgradeHUD.SetActive(true);
				this.enabled = false;
			}
        }



    }

	public void ReturnFromDisplay(){
		player.GetComponent<EightWayMovement>().enabled = true;
	}


}
