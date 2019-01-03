using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_GenericGarbage : MonoBehaviour {

	public GameObject smallShadow;
	public GameObject smallTextDisplay;
	public GameObject newDiscoveryDisplay;
	public GameObject displayTrash;
	public GameObject tempEffectsActor;
    public GarbageSpawner garbageSpawner;
    public Vector2 position;
    public AudioClip pickUpTrash;
    public AudioClip newDiscovery;
	bool isFalling = false;
	//bool justDisplay = false;
	bool beingKilled = false;
	bool magnetic;
	int delay;
	int thisRoom;
	int grabbedPhase = 0;
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
       // smallShadow.SetActive(false);
        smallTextDisplay.SetActive(false);

        trashAni = gameObject.GetComponent<tk2dSprite>();
        thisRoom = GlobalVariableManager.Instance.ROOM_NUM;
        if (GlobalVariableManager.Instance.ROOM_NUM == 101) {

           
        smallShadow.SetActive(true);
        smallShadow.transform.position = transform.position;
          

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

    private void OnDisable()
    {
        // lose the spawner reference.
        garbageSpawner = null;
    }

    // Update is called once per frame
    void Update () {

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

		yield return new WaitForSeconds(1f);

        // clear the spawner flag so it's not counted on the map icon.
        garbageSpawner.spawnedGarbage = null;
        ObjectPool.Instance.ReturnPooledObject(gameObject);

    }//end of Kill()

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.CompareTag("Player")){
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] <= GlobalVariableManager.Instance.BAG_SIZE_STAT.GetMax() && !isFalling){
                SoundManager.instance.PlaySingle(pickUpTrash);
                playerPos = collider.gameObject.transform;
                this.gameObject.transform.parent = playerPos;
                ObjectPool.Instance.GetPooledObject("effect_stars",transform.position);
                if (grabbedPhase <= 0){
                    Debug.Log("Garbage Type Collected: " + garbage.type);
					if((GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED & garbage.type) != garbage.type && (GlobalVariableManager.Instance.STANDARD_GARBAGE_VIEWED & garbage.type) != garbage.type)
                    {
						string myName = garbage.GarbageName();
                        GUIManager.Instance.TrashCollectedDisplayGameplay.NewDiscoveryShow(gameObject.GetComponent<tk2dSprite>().CurrentSprite.name, myName);
						GlobalVariableManager.Instance.STANDARD_GARBAGE_DISCOVERED |= garbage.type;
						SoundManager.instance.PlaySingle(newDiscovery);
					}//end of new discover code
				collider.gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimPickUp",true);

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

					collider.gameObject.GetComponent<PinFunctionsManager>().PassivePillage(true);
                        
                }

                smallShadow.SetActive(false);

				//if(GlobalVariableManager.Instance.characterUpgradeArray[1][32].CompareTo('o') == 0){
					//3rd bag perk - 10% chance to heal
					//int randomHealChance = Random.Range(1,11);
					//if(randomHealChance == 3 && GlobalVariableManager.Instance.CURRENT_HP < int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3].Substring(0,1))){
						//GlobalVariableManager.Instance.CURRENT_HP++;
					//}
				//}
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
                    GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);

                    StartCoroutine("Kill");
				    Debug.Log("Got this far - Trash pickup");
				    gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
				    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,17f), ForceMode2D.Impulse);

				    grabbedPhase = 1;

				}//end of grabbedPhase > 0 check
			}
		}
	}//end of trigger enter 2d

    public void SetSprite(string sprite)
    {
        trashAni.SetSprite(sprite);
    }

	void Setup(){
		xSpeedWhenCollected = -30f;
	}//End of Setup()
}
