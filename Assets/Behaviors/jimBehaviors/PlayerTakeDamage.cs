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
	public GameObject DeathGhost;

	public AudioClip hurt;
	public AudioClip finalHit;
	public AudioClip deathSound;
	public AudioClip healSound;
	public AudioClip deathLandSfx;
	[HideInInspector]
	public GameObject currentlyCarriedObject; // set by pickupableObject.cs for use when dropping at death
	int damageDealt;

	// Use this for initialization
	void Start () {

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.APPLEPLUS)){
			gameObject.GetComponent<PinFunctionsManager>().ApplePlus();
		}

        GlobalVariableManager.Instance.HP_STAT.ResetCurrent();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // two objects collide
	void OnCollisionEnter2D(Collision2D enemy){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (GetComponent<JimStateController>().IsHittable()) {
                if (enemy.gameObject.layer == 9) { //layer 9 = enemies
                    var enemyController = enemy.gameObject.GetComponent<EnemyStateController>();
                    if (enemyController != null && !enemyController.IsHitting()) // If this is a state where the enemy isn't actively hitting, ignore it.
                        return;

                    if (enemy.gameObject.tag == "Boss") {
                        damageDealt = enemy.gameObject.GetComponent<Boss>().attkDmg;
                    } else {
                        var enemyComp = enemy.gameObject.GetComponent<Enemy>();

                        // Fixing a bug where colliding with toxic slime would cause an error.
                        if (enemyComp != null)
                            damageDealt = enemyComp.attkPower;
                    }

                    if (enemy.gameObject.GetComponent<FollowPlayerAfterNotice>() != null) {
                        if (GlobalVariableManager.Instance.IsPinEquipped(PIN.SNEAKINGSCRAPPER)) {
                            if (enemy.gameObject.GetComponent<FollowPlayerAfterNotice>().IsChasing() || !GlobalVariableManager.Instance.IS_HIDDEN) {
                                TakeDamage(enemy.gameObject);
                            }
                        } else {
                            TakeDamage(enemy.gameObject);
                        }

                    } else {
                        if (!GlobalVariableManager.Instance.IS_HIDDEN) {
                            TakeDamage(enemy.gameObject);
                        }
                    }
                } else if (enemy.gameObject.layer == 16) {//enemy with non-solid collision(flying enemy)
                    if (enemy.gameObject.tag == "Boss") {
                        damageDealt = enemy.gameObject.GetComponent<Boss>().attkDmg;
                    } else {
                        damageDealt = enemy.gameObject.GetComponent<Enemy>().attkPower;
                    }
                    TakeDamage(enemy.gameObject);
                } else if (enemy.gameObject.layer == 19) { //layer 19 = hazards
                    var hazardComp = enemy.gameObject.GetComponent<Hazard>();

                    // Fixing a null reference error where the player keeps colliding with his own weapon.
                    if (hazardComp != null) {
                        damageDealt = hazardComp.damageToPlayer;
                        if (damageDealt > 0)
                            TakeDamage(enemy.gameObject);
                    }
                }
            }
        }
	}

    // something entered this collider
	void OnTriggerEnter2D(Collider2D projectile){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (GetComponent<JimStateController>().IsHittable()) {
                if (projectile.gameObject.layer == 10) { //layer 10 = projectiles
                    var projectileComp = projectile.gameObject.GetComponent<Projectile>();

                    // Fixing a null reference error where the player keeps colliding with his own weapon.
                    if (projectileComp != null) {
                        damageDealt = projectileComp.damageToPlayer;
                        TakeDamage(projectile.gameObject);
                    }
                }
                if (projectile.gameObject.layer == 19) { //layer 19 = hazards
                    var hazardComp = projectile.gameObject.GetComponent<Hazard>();

                    // Fixing a null reference error where the player keeps colliding with his own weapon.
                    if (hazardComp != null) {
                        damageDealt = hazardComp.damageToPlayer;
                        if (damageDealt > 0)
                            TakeDamage(projectile.gameObject);
                    }
                }
                /*else if (projectile.gameObject.layer == 16) {//enemy with non-solid collision(flying enemy)
                    if (projectile.gameObject.tag == "Boss") {
                        damageDealt = projectile.gameObject.GetComponent<Boss>().attkDmg;
                    }
                    else {
                        damageDealt = projectile.gameObject.GetComponent<Enemy>().attkPower;
                    }
                    TakeDamage(projectile.gameObject);
                }*/
            }
        }
	}

	void TakeDamage(GameObject enemy){
        // Stop defeated enemies from damaging the player.
        var etd = enemy.GetComponent<EnemyTakeDamage>();
        if (etd != null)
            if (etd.currentHp < 1)
                return;

		SoundManager.instance.PlaySingle(hurt);

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.QUENPINTARANTINO)){
			gameObject.GetComponent<PinFunctionsManager>().QuenpinTarantino();
		}
		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.PASSIVEPILLAGE)){
			gameObject.GetComponent<PinFunctionsManager>().PassivePillage(false);
		}

		GlobalVariableManager.Instance.HP_STAT.UpdateCurrent(-damageDealt);

        PlayerManager.Instance.controller.SendTrigger(JimTrigger.HIT);

		Debug.Log("reached this end of hp hud change" + GlobalVariableManager.Instance.HP_STAT.GetCurrent());
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay();
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

        if (GlobalVariableManager.Instance.HP_STAT.GetCurrent() <= 0) {
            PlayerManager.Instance.controller.SendTrigger(JimTrigger.DEATH);
            SoundManager.instance.PlaySingle(finalHit);

            StartCoroutine("Death");

        }else{
        	StartCoroutine("InvulTimer");
        }
	}

	public IEnumerator DropTrash(){
		for(int i = 0; i < GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]; i++){
			Debug.Log("Dropped Trash Here xoxoxoxoxoxoxo");
			GameObject droppedTrash = objectPool.GetComponent<ObjectPool>().GetPooledObject("DroppedTrash",gameObject.transform.position);
			droppedTrash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3f,3f),Random.Range(11f,17f)), ForceMode2D.Impulse);
			droppedTrash.GetComponent<Ev_DroppedTrash>().PlaySound();
			yield return new WaitForSeconds(.1f);
		}

		yield return null;


	}

	public void Heal(int healAmnt){
		if(GlobalVariableManager.Instance.HP_STAT.GetCurrent() + healAmnt < GlobalVariableManager.Instance.HP_STAT.GetMax()) {
            GlobalVariableManager.Instance.HP_STAT.UpdateCurrent(+healAmnt);
		}else{
            GlobalVariableManager.Instance.HP_STAT.ResetCurrent();
		}

		SoundManager.instance.PlaySingle(healSound);
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay();

	}
	IEnumerator InvulTimer(){
		yield return new WaitForSeconds(1f);
		GetComponent<JimStateController>().RemoveFlag((int)JimFlag.INVULNERABLE);
	}

	IEnumerator Death(){
        // Trigger Respawn State.
        GameStateManager.Instance.PushState(typeof(RespawnState));
        gameObject.GetComponent<EightWayMovement>().StopMovement();
        SoundManager.instance.FadeMusic();
        CamManager.Instance.mainCamEffects.CameraPan(this.gameObject, true);
		Time.timeScale = 0.3f;
		yield return new WaitForSeconds(.3f);
		Time.timeScale = 1;
		CamManager.Instance.mainCamEffects.ZoomInOut(1.5f,3f);
		if(currentlyCarriedObject != null){
			currentlyCarriedObject.GetComponent<PickupableObject>().Drop();
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer04";//bring player to front
        deathDisplay.DeathFade();
			yield return new WaitForSeconds(.5f); //drop trash and deplete trash collected

		SoundManager.instance.PlaySingle(deathSound);
		yield return new WaitForSeconds(.4f);
		//DeathGhost.SetActive(true);
		//DeathGhost.GetComponent<Animator>().Play("jimDeathGhostAni",-1,0f);
		GameObject landSmoke = ObjectPool.Instance.GetPooledObject("effect_enemyLand",transform.position);
		landSmoke.GetComponent<Renderer>().sortingLayerName = "Layer04"; //needed to be able to see smoke
		SoundManager.instance.PlaySingle(deathLandSfx);
			yield return new WaitForSeconds(.6f);
		deathDisplay.DepleteTrashCollected();
		StartCoroutine("DropTrash");

			yield return new WaitForSeconds(2f);//truck pickup
		//DeathGhost.SetActive(false);
		GameObject truck = objectPool.GetComponent<ObjectPool>().GetPooledObject("GarbageTruck",new Vector3(gameObject.transform.position.x - 20, gameObject.transform.position.y,0f));
		truck.GetComponent<Ev_SmallTruck>().ReturnToDumpster();
			yield return new WaitForSeconds(.4f);
		//day meter rise and enter truck
		gameObject.GetComponent<MeshRenderer>().enabled = false;
			yield return new WaitForSeconds(.5f);

        // Trigger Death Clock State
        GameStateManager.Instance.PushState(typeof(DethKlokState));
        deathDisplay.DayMeterRise();
			yield return new WaitForSeconds(2f);

        //return to start room
		//deathDisplay.FadeHUD();
        deathDisplay.myDayMeter.gameObject.SetActive(false);
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
        // Pop Death Clock State
        GameStateManager.Instance.PopState();

        yield return new WaitForSeconds(.5f);

        //-----------Resetting of needed values----------------//
        roomManager.GetComponent<RoomManager>().currentRoom.DeactivateRoom();

        SoundManager.instance.TransitionMusic(SoundManager.instance.worldMusic, fadeOut:false);


        if(CheckpointManager.Instance.lastCheckpoint == null){
        gameObject.transform.position = new Vector3(0f,-3f,0f); //Start at Beginning of world
		truck.transform.position = new Vector3(-15f,-3f,0f);
			roomManager.GetComponent<RoomManager>().Restart(RoomManager.Instance.startRoom);
			CamManager.Instance.mainCam.transform.position = new Vector3(0f,0f,-10f);
		}else{

			gameObject.transform.position = CheckpointManager.Instance.lastCheckpoint.transform.position; //Start at Beginning of world
			truck.transform.position = new Vector3(transform.position.x -15f,transform.position.y,0f);
			roomManager.GetComponent<RoomManager>().Restart(CheckpointManager.Instance.lastCheckpoint.myRoom);
			CamManager.Instance.mainCam.transform.position = new Vector3(CheckpointManager.Instance.lastCheckpoint.transform.position.x,CheckpointManager.Instance.lastCheckpoint.transform.position.y,-10f);
		}
		deathDisplay.PlayTruckSfx();
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
       
	
		yield return new WaitForSeconds(.2f);
		GlobalVariableManager.Instance.HP_STAT.ResetCurrent();
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] = 0;
		yield return new WaitForSeconds(.1f);
		deathDisplay.FadeHUD();
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		yield return new WaitForSeconds(.2f);
		deathDisplay.PlayTruckSfx();
		truck.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
		gameObject.GetComponent<MeshRenderer>().enabled = true;
        PlayerManager.Instance.controller.SendTrigger(JimTrigger.IDLE);
        yield return new WaitForSeconds(.3f);
		deathDisplay.fader.SetActive(false);
		/*if(!GlobalVariableManager.Instance.IsPinEquipped(PIN.FAITHFULWEAPON)){
				GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] = 0;//reset scrap value
				gameObject.GetComponent<PinFunctionsManager>().FaithfulWeapin();//just used to update weapon HUD in this scenario
		}else{
			gameObject.GetComponent<PinFunctionsManager>().FaithfulWeapin();
		}*/
		HPdisplay.GetComponent<GUI_HPdisplay>().UpdateDisplay();
        GUIManager.Instance.TrashCollectedDisplayGameplay.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
		truck.GetComponent<Ev_SmallTruck>().RespawnEnd();
		gameObject.GetComponent<BoxCollider2D>().enabled = true;

		gameObject.GetComponent<EightWayMovement>().clipOverride = false;

        // Pop Respawn State
        GameStateManager.Instance.PopState();
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";//return player to the gameplay layer
    }
   
}
