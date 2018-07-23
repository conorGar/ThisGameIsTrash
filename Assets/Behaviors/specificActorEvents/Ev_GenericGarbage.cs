using System.Collections;
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
	int whatNumberGarbageAmI;
	int bagSizeBonus;
	public GlobalVariableManager.GARBAGE garbageType;
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

            whatNumberGarbageAmI = GlobalVariableManager.Instance.MY_NUM_IN_ROOM;

            if (GlobalVariableManager.Instance.pinsEquipped[39] == 1) {
                magnetic = true;
            }

            Setup();
        } else {
            // if the garbage has been discovered or not viewed yet.
            if ((GlobalVariableManager.Instance.GARBAGE_DISCOVERY_LIST[0] & garbageType) == garbageType || (GlobalVariableManager.Instance.GARBAGE_VIEWED_LIST[0] & garbageType) != garbageType)
            {
				gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
			}
            // display the new text if it hasn't been viewed yet.
			if((GlobalVariableManager.Instance.GARBAGE_VIEWED_LIST[0] & garbageType) != garbageType)
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
		garbageType = GlobalVariableManager.GARBAGE.NONE;

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
							if((GlobalVariableManager.Instance.GARBAGE_DISCOVERY_LIST[0] & garbageType) == garbageType && (GlobalVariableManager.Instance.GARBAGE_VIEWED_LIST[0] & garbageType) != garbageType)
                        {
								GameObject discoveryDisplay = GameObject.Find("new discovery display");
								if(discoveryDisplay != null)//if there is already a new discovery display on screen
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().KillMyTrash();
								else{
									discoveryDisplay = Instantiate(newDiscoveryDisplay, GetComponent<tk2dCamera>().ScreenCamera.ViewportToWorldPoint(new Vector3(-14f,4f,0f)), Quaternion.identity);
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().SetWhichTrash(whichTrash,trashAni.CurrentSprite.name);

                                    GlobalVariableManager.Instance.GARBAGE_VIEWED_LIST[0] |= garbageType;
									GlobalVariableManager.Instance.MY_NUM_IN_ROOM = whatNumberGarbageAmI;
									GameObject displayTrashInstance = Instantiate(displayTrash,GetComponent<tk2dCamera>().ScreenCamera.ViewportToWorldPoint(new Vector3(-15f,4f,0f)),Quaternion.identity);
									discoveryDisplay.GetComponent<Ev_newDiscoveryDisplay>().SetMyTrash(displayTrashInstance);
								}
							}//end of new discover code
						GlobalVariableManager.Instance.WORLD_LIST[GlobalVariableManager.Instance.ROOM_NUM].Replace((char)GlobalVariableManager.Instance.WORLD_LIST[GlobalVariableManager.Instance.ROOM_NUM][whatNumberGarbageAmI], 'o');
						player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimPickUp");

						if(GlobalVariableManager.Instance.pinsEquipped[10] == 1){
							//Mo Garbage Mo' Problems - changes max HP bac if collect more than 5 trash
							if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= 4){
								//if(GlobalVariableManager.Instance.CURRENT_HP > int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 2){
									//GlobalVariableManager.Instance.CURRENT_HP = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3]) - 2);
								}
								GlobalVariableManager.Instance.characterUpgradeArray[3] = (int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3].Substring(0,1)) + 2).ToString();
								GlobalVariableManager.Instance.pinsEquipped[10] = 2;
							}
						//mo garbage mo problems check end
						if(GlobalVariableManager.Instance.pinsEquipped[6] != 0){
						//Passive Pillage
							/*GameObject passivePillageEffect = Instantiate(tempEffectsActor, new Vector2(0f,0f),Quaternion.identity);
							passivePillageEffect.GetComponent<Ev_upgradeActorTempEffects>().Follow();
							GlobalVariableManager.Instance.pinsEquipped[6]++;
							player.GetComponent<EightWayMovement>().UpdateSpeed();*/
						}

                        smallShadow.SetActive(false);

						//if(GlobalVariableManager.Instance.characterUpgradeArray[1][32].CompareTo('o') == 0){
							//3rd bag perk - 10% chance to heal
							//int randomHealChance = Random.Range(1,11);
							//if(randomHealChance == 3 && GlobalVariableManager.Instance.CURRENT_HP < int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3].Substring(0,1))){
								//GlobalVariableManager.Instance.CURRENT_HP++;
							//}
						//}
						if(GlobalVariableManager.Instance.pinsEquipped[40] != 0){
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

	void Setup(){
		if(GlobalVariableManager.Instance.pinsEquipped[1] == 1){
		//Bulky Bag pin
			bagSizeBonus = bagSizeBonus + 3;
		}
		if(GlobalVariableManager.Instance.characterUpgradeArray[13].CompareTo('o') == 0){
		//amount of trash available to carry is increased if you upgrade bag size stat
		    bagSizeBonus += 2;
		}

		bagSizeBonus += int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[4].Substring(0,1));

		xSpeedWhenCollected = -30f;

		if(whatNumberGarbageAmI == 0){
			whatTextStartsAs = "a";
		} else if(whatNumberGarbageAmI == 1){
			whatTextStartsAs = "b";
		} else if(whatNumberGarbageAmI ==2){
			whatTextStartsAs = "c";
		} else if(whatNumberGarbageAmI == 3){
			//for dropped trash(Only spawns paper and vanishes if leave room)
			trashAni.SetSprite("trash_paper2");
			whatTextStartsAs = "9";
			garbageType = GlobalVariableManager.GARBAGE.PAPER;
		}
		string worldListRoomString = GlobalVariableManager.Instance.WORLD_LIST[thisRoom];
		whichTrash = Random.Range(1,24);

		if(whichTrash == 1){
			if(worldListRoomString[0].CompareTo('d') != 0){
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'d');
                garbageType = GlobalVariableManager.GARBAGE.PAPER;
            }
            else{
				whichTrash ++;
				trashAni.SetSprite("trash_ipod2");
                garbageType = GlobalVariableManager.GARBAGE.IPOD;
            }
		}else if(whichTrash ==2){
			if(worldListRoomString[0].CompareTo('e') != 0){
				trashAni.SetSprite("trash_ipod2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'e');
                garbageType = GlobalVariableManager.GARBAGE.IPOD;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_book2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'f');
                garbageType = GlobalVariableManager.GARBAGE.BOOK;
            }

		}else if(whichTrash ==3){
			if(worldListRoomString[0].CompareTo('f') != 0){
				trashAni.SetSprite("trash_book2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'f');
                garbageType = GlobalVariableManager.GARBAGE.BOOK;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_artSupplies");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'g');
                garbageType = GlobalVariableManager.GARBAGE.ART_SUPPLIES;
            }
		}else if(whichTrash ==4){
			if(worldListRoomString[0].CompareTo('g') != 0){
				trashAni.SetSprite("trash_artSupplies");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'g');
                garbageType = GlobalVariableManager.GARBAGE.ART_SUPPLIES;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_bagSpicy2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'h');
                garbageType = GlobalVariableManager.GARBAGE.BAG_SPICY;
            }
		}else if(whichTrash ==5){
			if(worldListRoomString[0].CompareTo('h') != 0){
				trashAni.SetSprite("trash_bagSpicy2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'h');
                garbageType = GlobalVariableManager.GARBAGE.BAG_SPICY;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_chips");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'i');
                garbageType = GlobalVariableManager.GARBAGE.CHIPS;
            }
		}else if(whichTrash ==6){
			if(worldListRoomString[0].CompareTo('i') != 0){
				trashAni.SetSprite("trash_chips");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'i');
                garbageType = GlobalVariableManager.GARBAGE.CHIPS;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_bbqBag2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'j');
                garbageType = GlobalVariableManager.GARBAGE.BAG_BBQ;
            }
		}else if(whichTrash ==7){
			if(worldListRoomString[0].CompareTo('j') != 0){
				trashAni.SetSprite("trash_bbqBag2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'j');
                garbageType = GlobalVariableManager.GARBAGE.BAG_BBQ;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_casette2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'k');
                garbageType = GlobalVariableManager.GARBAGE.CASETTE;
            }
		}else if(whichTrash ==8){
			if(worldListRoomString[0].CompareTo('k') != 0){
				trashAni.SetSprite("trash_casette2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'k');
                garbageType = GlobalVariableManager.GARBAGE.CASETTE;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_chinese2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'l');
                garbageType = GlobalVariableManager.GARBAGE.CHINESE;
            }
		}else if(whichTrash ==9){
			if(worldListRoomString[0].CompareTo('l') != 0){
				trashAni.SetSprite("trash_chinese2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'l');
                garbageType = GlobalVariableManager.GARBAGE.CHINESE;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_juice2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'m');
                garbageType = GlobalVariableManager.GARBAGE.JUICE;
            }
		}else if(whichTrash ==10){
			if(worldListRoomString[0].CompareTo('m') != 0){
				trashAni.SetSprite("trash_juice2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'m');
                garbageType = GlobalVariableManager.GARBAGE.JUICE;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_lightbulb2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'n');
                garbageType = GlobalVariableManager.GARBAGE.LIGHTBULB;
            }
		}else if(whichTrash ==11){
			if(worldListRoomString[0].CompareTo('n') != 0){
				trashAni.SetSprite("trash_lightbulb2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'n');
                garbageType = GlobalVariableManager.GARBAGE.LIGHTBULB;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_mug2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'p');
                garbageType = GlobalVariableManager.GARBAGE.MUG;
            }
		}else if(whichTrash ==12){
			if(worldListRoomString[0].CompareTo('p') != 0){
				trashAni.SetSprite("trash_mug2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'p');
                garbageType = GlobalVariableManager.GARBAGE.MUG;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_party");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'q');
                garbageType = GlobalVariableManager.GARBAGE.PARTY;
            }
		}else if(whichTrash ==13){
			if(worldListRoomString[0].CompareTo('q') != 0){
				trashAni.SetSprite("trash_party");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'q');
                garbageType = GlobalVariableManager.GARBAGE.PARTY;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_sock2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'r');
                garbageType = GlobalVariableManager.GARBAGE.SOCK;
            }
		}else if(whichTrash ==14){
			if(worldListRoomString[0].CompareTo('r') != 0){
				trashAni.SetSprite("trash_sock2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'r');
                garbageType = GlobalVariableManager.GARBAGE.SOCK;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_tissue2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'s');
                garbageType = GlobalVariableManager.GARBAGE.TISSUE;
            }
		}else if(whichTrash ==15){
			if(worldListRoomString[0].CompareTo('s') != 0){
				trashAni.SetSprite("trash_tissue2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'s');
                garbageType = GlobalVariableManager.GARBAGE.TISSUE;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_toilet2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'t');
                garbageType = GlobalVariableManager.GARBAGE.TOILET;
            }
		}else if(whichTrash ==16){
			if(worldListRoomString[0].CompareTo('t') != 0){
				trashAni.SetSprite("trash_toilet2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'t');
                garbageType = GlobalVariableManager.GARBAGE.TOILET;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_hair2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'u');
                garbageType = GlobalVariableManager.GARBAGE.HAIR;
            }
		}else if(whichTrash ==17){
			if(worldListRoomString[0].CompareTo('u') != 0){
				trashAni.SetSprite("trash_hair2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'u');
                garbageType = GlobalVariableManager.GARBAGE.HAIR;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_toilet2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'t');
                garbageType = GlobalVariableManager.GARBAGE.TOILET;
            }
		}else if(whichTrash ==18){
			if(worldListRoomString[0].CompareTo('v') != 0){
				trashAni.SetSprite("trash_fish2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'v');
                garbageType = GlobalVariableManager.GARBAGE.FISH;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_hair2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'u');
                garbageType = GlobalVariableManager.GARBAGE.HAIR;
            }
		}else if(whichTrash ==19){
			if(worldListRoomString[0].CompareTo('u') != 0){
				trashAni.SetSprite("trash_needle2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'w');
                garbageType = GlobalVariableManager.GARBAGE.NEEDLE;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_hair2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'u');
                garbageType = GlobalVariableManager.GARBAGE.HAIR;
            }
		}else if(whichTrash ==20){
			if(worldListRoomString[0].CompareTo('y') != 0){
				trashAni.SetSprite("trash_baby2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'y');
                garbageType = GlobalVariableManager.GARBAGE.BABY;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_arm2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'z');
                garbageType = GlobalVariableManager.GARBAGE.ARM;
            }
		}else if(whichTrash ==21){
			if(worldListRoomString[0].CompareTo('z') != 0){
				trashAni.SetSprite("trash_arm2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'z');
                garbageType = GlobalVariableManager.GARBAGE.ARM;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_childhood2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'1');
                garbageType = GlobalVariableManager.GARBAGE.CHILDHOOD;
            }
		}else if(whichTrash ==22){
			if(worldListRoomString[0].CompareTo('1') != 0){
				trashAni.SetSprite("trash_childhood2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'1');
                garbageType = GlobalVariableManager.GARBAGE.CHILDHOOD;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_momPres2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'2');
                garbageType = GlobalVariableManager.GARBAGE.MOM_PRES;
            }
		}else if(whichTrash ==23){
			if(worldListRoomString[0].CompareTo('2') != 0){
				trashAni.SetSprite("trash_momPres2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'2');
                garbageType = GlobalVariableManager.GARBAGE.MOM_PRES;
            }
            else{
				whichTrash++;
				trashAni.SetSprite("trash_childhood2");
				worldListRoomString.Replace(worldListRoomString[whatNumberGarbageAmI],'1');
                garbageType = GlobalVariableManager.GARBAGE.CHILDHOOD;
            }
		}

		GlobalVariableManager.Instance.WORLD_LIST[thisRoom] = worldListRoomString;

	}//End of Setup()
}
