using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour {

	public GameObject objectPool;
	public GameObject HPdisplay;
	//public GameObject fadeHelper;//needed for Death()
	public GameObject roomManager; //needed for Death()
	//public GameObject trashCollectedDisplay; //needed for Death()
	public GameObject droppedTrashPile;
	public GUI_DeathDisplay deathDisplay;


	public AudioClip hurt;
	public AudioClip finalHit;
	public AudioClip deathSound;
	public AudioClip healSound;
	[HideInInspector]
	public GameObject currentlyCarriedObject; // set by pickupableObject.cs for use when dropping at death
	int maxHP;
	public int currentHp;
	int damageDealt;

	bool currentlyTakingDamage = false;
	// Use this for initialization
	void Start () {

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.APPLEPLUS)){
			gameObject.GetComponent<PinFunctionsManager>().ApplePlus();
		}
		maxHP = GlobalVariableManager.Instance.Max_HP;	
		currentHp = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // two objects collide
	void OnCollisionEnter2D(Collision2D enemy){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (enemy.gameObject.layer == 9 && !currentlyTakingDamage) { //layer 9 = enemies

                if (enemy.gameObject.tag == "Boss") {
                    damageDealt = enemy.gameObject.GetComponent<Boss>().attkDmg;
                }
                else {
                    damageDealt = enemy.gameObject.GetComponent<Enemy>().attkPower;
                }
                TakeDamage(enemy.gameObject);
            }
        }
	}

    // something entered this collider
	void OnTriggerEnter2D(Collider2D projectile){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (projectile.gameObject.layer == 10 && !currentlyTakingDamage) { //layer 10 = projectiles

                damageDealt = projectile.gameObject.GetComponent<Projectile>().damageToPlayer;

                TakeDamage(projectile.gameObject);
            }
            else if (projectile.gameObject.layer == 16 && !currentlyTakingDamage) {//enemy with non-solid collision(flying enemy)
                if (projectile.gameObject.tag == "Boss") {
                    damageDealt = projectile.gameObject.GetComponent<Boss>().attkDmg;
                }
                else {
                    damageDealt = projectile.gameObject.GetComponent<Enemy>().attkPower;
                }
                TakeDamage(projectile.gameObject);
            }
        }
	}

	void TakeDamage(GameObject enemy){

		SoundManager.instance.PlaySingle(hurt);

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.QUENPINTARANTINO)){
				gameObject.GetComponent<PinFunctionsManager>().QuenpinTarantino();
			}
			if(GlobalVariableManager.Instance.IsPinEquipped(PIN.PASSIVEPILLAGE)){
				gameObject.GetComponent<PinFunctionsManager>().PassivePillage(false);
			}

			currentlyTakingDamage = true;
			GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
			currentHp -= damageDealt;
			gameObject.GetComponent<JimAnimationManager>().PlayAnimation("hurt",true);
			Debug.Log("reached this end of hp hud change" + currentHp);
			HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);
			GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars_player",this.gameObject.transform.position);
			damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
			damageCounter.SetActive(true);
			GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars",this.gameObject.transform.position);
			littleStars.SetActive(true);
            CamManager.Instance.mainCam.ScreenShake(.2f);

			if(enemy.transform.position.x < gameObject.transform.position.x){
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
			}else{
				gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-17f,0f), ForceMode2D.Impulse);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f,10f), ForceMode2D.Impulse);
			}

			if(currentHp <= 0){
				SoundManager.instance.PlaySingle(finalHit);

				StartCoroutine("Death");
			
			}else{
				StartCoroutine("RegainControl");
			}
	}

	IEnumerator RegainControl(){
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		if(GlobalVariableManager.Instance.CARRYING_SOMETHING){
			gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove",true);
		}else{
			gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",true);
		}
		gameObject.GetComponent<EightWayMovement>().clipOverride = false;
		yield return new WaitForSeconds(.5f); //brief period of invincibility
		currentlyTakingDamage = false;
		Debug.Log("Regained Control");

	}

	public void DropTrash(){
		for(int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]; i++){
			Debug.Log("Dropped Trash Here xoxoxoxoxoxoxo");
			GameObject droppedTrash = objectPool.GetComponent<ObjectPool>().GetPooledObject("DroppedTrash",gameObject.transform.position);
			droppedTrash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3f,3f),Random.Range(11f,17f)), ForceMode2D.Impulse);
		}


	}

	public void Heal(int healAmnt){
		if(currentHp + healAmnt < maxHP){
			currentHp += healAmnt;
		}else{
			currentHp = maxHP;
		}
		SoundManager.instance.PlaySingle(healSound);
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);

	}


	IEnumerator Death(){
        // Trigger Respawn State.
        GameStateManager.Instance.PushState(typeof(RespawnState));
        gameObject.GetComponent<EightWayMovement>().StopMovement();
        SoundManager.instance.FadeMusic();
		Time.timeScale = 0.3f;
		yield return new WaitForSeconds(.3f);
		Time.timeScale = 1;

		GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
		if(currentlyCarriedObject != null){
			currentlyCarriedObject.GetComponent<PickupableObject>().Drop();
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer04";//bring player to front
        deathDisplay.DeathFade();
			yield return new WaitForSeconds(.5f); //drop trash and deplete trash collected
		gameObject.GetComponent<JimAnimationManager>().PlayAnimation("death",true);
		SoundManager.instance.PlaySingle(deathSound);
			yield return new WaitForSeconds(.5f);
		deathDisplay.DepleteTrashCollected();
		DropTrash();

			yield return new WaitForSeconds(2f);//truck pickup

		GameObject truck = objectPool.GetComponent<ObjectPool>().GetPooledObject("GarbageTruck",new Vector3(gameObject.transform.position.x - 20, gameObject.transform.position.y,0f));
		truck.GetComponent<Ev_SmallTruck>().ReturnToDumpster();
			yield return new WaitForSeconds(.4f);
		//day meter rise and enter truck
		gameObject.GetComponent<MeshRenderer>().enabled = false;
			yield return new WaitForSeconds(.5f);

        // Trigger Death Clock State
        GameStateManager.Instance.PushState(typeof(DethKlokState));
        deathDisplay.DayMeterRise();
			yield return new WaitForSeconds(1.5f);

        //return to start room
		deathDisplay.FadeHUD();
        deathDisplay.myDayMeter.gameObject.SetActive(false);

        // Pop Death Clock State
        GameStateManager.Instance.PopState();

        yield return new WaitForSeconds(.5f);
		deathDisplay.fader.SetActive(false);
		SoundManager.instance.musicSource.clip = SoundManager.instance.worldMusic;
		SoundManager.instance.musicSource.Play();
		SoundManager.instance.musicSource.volume = GlobalVariableManager.Instance.MASTER_MUSIC_VOL;

        //-----------Resetting of needed values----------------//
        roomManager.GetComponent<RoomManager>().currentRoom.DeactivateRoom();
		gameObject.transform.position = new Vector3(0f,-3f,0f); //Start at Beginning of world
		truck.transform.position = new Vector3(-15f,-3f,0f);
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
        CamManager.Instance.mainCam.transform.position = new Vector3(0f,0f,-10f);
		roomManager.GetComponent<RoomManager>().Restart();
		currentHp = GlobalVariableManager.Instance.Max_HP;
		currentlyTakingDamage = false;
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] = 0;
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.FAITHFULWEAPON)){
				GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] = 0;//reset scrap value
				gameObject.GetComponent<PinFunctionsManager>().FaithfulWeapin();//just used to update weapon HUD in this scenario
		}else{
			gameObject.GetComponent<PinFunctionsManager>().FaithfulWeapin();
		}
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay(currentHp);
        GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		truck.GetComponent<Ev_SmallTruck>().RespawnEnd();
		gameObject.GetComponent<BoxCollider2D>().enabled = true;

		gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",true);
		gameObject.GetComponent<EightWayMovement>().myLegs.SetActive(true);
		gameObject.GetComponent<EightWayMovement>().enabled = true;
		gameObject.GetComponent<EightWayMovement>().clipOverride = false;

        // Pop Respawn State
        GameStateManager.Instance.PopState();
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";//return player to the gameplay layer
    }
   
}
