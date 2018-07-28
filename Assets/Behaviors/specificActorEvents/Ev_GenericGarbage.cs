﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_GenericGarbage : MonoBehaviour {

	public GameObject smallShadow;
	public GameObject smallTextDisplay;
	public GameObject newDiscoveryDisplay;
	public GameObject displayTrash;
	public GameObject tempEffectsActor;
    public Vector2 position;

	bool isFalling = false;
	bool justDisplay = false;
	bool beingKilled = false;
	bool magnetic;
	int delay;
	int thisRoom;
	int grabbedPhase = 0;
	int bagSizeBonus;
	public StandardGarbage garbage = new StandardGarbage();
	int whichTrash;
	float Speed = 20;
	float xSpeedWhenCollected;
	string whatTextStartsAs;
	tk2dSprite trashAni;
	Transform playerPos;
	private Vector3 smoothVelocity = Vector3.zero;

    // Use this for initialization
    void OnEnable() {
        smallShadow.SetActive(false);
        smallTextDisplay.SetActive(false);

        trashAni = gameObject.GetComponent<tk2dSprite>();
        thisRoom = GlobalVariableManager.Instance.ROOM_NUM;
        if (GlobalVariableManager.Instance.ROOM_NUM == 101) {

            if (!isFalling && !justDisplay) {
                smallShadow.SetActive(true);
                smallShadow.transform.position = transform.position;
            }

            if (GlobalVariableManager.Instance.IsPinEquipped(PIN.MAGNETICPIN)) {
                magnetic = true;
            }

            Setup();
        } else {
            // if the garbage has been discovered or not viewed yet.
            if ((GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED & garbage.type) == garbage.type || (GlobalVariableManager.Instance.STANDARD_GARBAGE_VIEWED & garbage.type) != garbage.type)
            {
				gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
			}
            // display the new text if it hasn't been viewed yet.
			if((GlobalVariableManager.Instance.STANDARD_GARBAGE_VIEWED & garbage.type) != garbage.type)
            {
                smallTextDisplay.SetActive(true);
			}

		}//end of else

		if(GlobalVariableManager.Instance.ROOM_NUM != 101){
		//makes all enemy dropped trash crumpled paper
			if(whichTrash == 0){
				whichTrash = 1;
			}
		}
	}

    // Update is called once per frame
    void Update () {

		//myCollecion behaior --------------------//
		if(thisRoom == 101){
			if(GlobalVariableManager.Instance.MENU_SELECT_STAGE != 4){
				ObjectPool.Instance.ReturnPooledObject(gameObject);
			}
		}

		//------------------------------------//


		//-----------after grabbed-----------------//
		if(grabbedPhase > 0){
			if(grabbedPhase ==1){
				Speed = Speed + 20 *Time.deltaTime*2;

				position.y = transform.position.y + Speed * Time.deltaTime;
				transform.position = position;
			}else if(grabbedPhase ==2){

				if(Mathf.Abs(transform.position.x - playerPos.position.x) < 10 && Mathf.Abs(transform.position.y - playerPos.position.y) < 10){
					if(!beingKilled){
						StartCoroutine("Kill");
						beingKilled = true;
					}
				}else{
					float distance = Vector3.Distance(transform.position, playerPos.position);
					transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref smoothVelocity, 10);
				}
			}
		}
	}
	void Fall(){
		isFalling = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,Random.Range(14,20));
		garbage.type = STANDARDGARBAGE.NONE;

        smallShadow.SetActive(true);
        smallShadow.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + Random.Range(64, 370));
	}//end of Fall()

	void Squish(){

	}//end of squish

	void FallEnd(){

	}//end of fallend()

	IEnumerator Kill(){

		yield return new WaitForSeconds(.2f);
        ObjectPool.Instance.ReturnPooledObject(gameObject);

    }//end of Kill()

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Collided");
		if(collider.gameObject.CompareTag("Player")){
			Debug.Log("Collided With Player");
				playerPos = collider.GetComponent<Transform>();
				GameObject player = collider.gameObject as GameObject;
				if(!GlobalVariableManager.Instance.SCENE_IS_TRANSITIONING && player.GetComponent<tk2dSpriteAnimator>().CurrentClip.name.CompareTo("hit") != 0){
					if(!justDisplay && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] < (10 + bagSizeBonus) && !isFalling){
						if(grabbedPhase <= 0){
							if((GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED & garbage.type) == garbage.type && (GlobalVariableManager.Instance.STANDARD_GARBAGE_VIEWED & garbage.type) != garbage.type)
                        {
								GameObject discoveryDisplay = GameObject.Find("new discovery display");
								if(discoveryDisplay != null)//if there is already a new discovery display on screen
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().KillMyTrash();
								else{
									discoveryDisplay = Instantiate(newDiscoveryDisplay, GetComponent<tk2dCamera>().ScreenCamera.ViewportToWorldPoint(new Vector3(-14f,4f,0f)), Quaternion.identity);
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().SetWhichTrash(whichTrash,trashAni.CurrentSprite.name);

                                    GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED |= garbage.type;
									GameObject displayTrashInstance = Instantiate(displayTrash,GetComponent<tk2dCamera>().ScreenCamera.ViewportToWorldPoint(new Vector3(-15f,4f,0f)),Quaternion.identity);
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().SetMyTrash(displayTrashInstance);
								}
							}//end of new discover code
						player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimPickUp");

						if(GlobalVariableManager.Instance.IsPinEquipped(PIN.MOGARBAGEMOPROBLEMS)){
                            //Mo Garbage Mo' Problems - changes max HP bac if collect more than 5 trash
                            // TODO: Review and figure this out.
                            /*if (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= 4){
								if(GlobalVariableManager.Instance.CURRENT_HP > int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 2){
									GlobalVariableManager.Instance.CURRENT_HP = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 2);
								}
								GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3].Substring(0,1)) + 2).ToString();
							}*/
						}//mo garbage mo problems check end
						if(GlobalVariableManager.Instance.IsPinEquipped(PIN.PASSIVEPILLAGE)){
						//Passive Pillage
							/*GameObject passivePillageEffect = Instantiate(tempEffectsActor, new Vector2(0f,0f),Quaternion.identity);
							passivePillageEffect.GetComponent<Ev_upgradeActorTempEffects>().Follow();
							GlobalVariableManager.Instance.pinsEquipped[6]++;
							player.GetComponent<EightWayMovement>().UpdateSpeed();*/
						}

                        smallShadow.SetActive(false);

						if(GlobalVariableManager.Instance.characterUpgradeArray[1][32].CompareTo('o') == 0){
							//3rd bag perk - 10% chance to heal
							int randomHealChance = Random.Range(1,11);
							if(randomHealChance == 3 && GlobalVariableManager.Instance.CURRENT_HP < int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3].Substring(0,1))){
								GlobalVariableManager.Instance.CURRENT_HP++;
							}
						}
						if(GlobalVariableManager.Instance.IsPinEquipped(PIN.TRASHPOWER)){
							//Trash Power pin
							GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] += 2;
							GameObject currentWeaponHUD = GameObject.Find("current weapon");
							currentWeaponHUD.GetComponent<Ev_CurrentWeapon>().UpdateMelee();
						}

						if(GlobalVariableManager.Instance.WORLD_ROOM_DISCOVER.Count <= 5){
							// (if not in race mode, trash is collected normally)
							GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]++;
						}else{
							//otherwise increase race collected
							GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[2]++;
						}
						StartCoroutine("Kill");
						grabbedPhase = 1;

						}//end of grabbedPhase > 0 check
					}
				}
		}
	}//end of trigger enter 2d

    public void SetSprite(string sprite)
    {
        trashAni.SetSprite(sprite);
    }

	void Setup(){
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.BULKYBAG)){
		//Bulky Bag pin
			bagSizeBonus = bagSizeBonus + 3;
		}
		if(GlobalVariableManager.Instance.characterUpgradeArray[13].CompareTo('o') == 0){
		//amount of trash available to carry is increased if you upgrade bag size stat
		    bagSizeBonus += 2;
		}

		bagSizeBonus += int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[4].Substring(0,1));

		xSpeedWhenCollected = -30f;
	}//End of Setup()
}
