using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
public class Boss : MonoBehaviour {

	public int bossNumber;
	public int hp;
	public int attkDmg;
	public GameObject hpDisplay;
	public bool vanishAtDeath;
	//public MonoBehaviour myBossScript;
	public GameObject objectToPanTo;
	public AudioClip hpDisplayStartSfx;
    protected EnemyStateController controller;

    [HideInInspector]
	public Room currentRoom; //used to diable other bosses at main bosses' death
	int deathSmokeNumber;

    private void Awake()
    {
        controller = GetComponent<EnemyStateController>();
       
    }

    protected void Start () {


//		if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] > hp){
//			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = hp;
//		}else if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] < hp && GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] != 0 ){
//			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]++; //regain hp each day
//		}

		gameObject.GetComponent<EnemyTakeDamage>().currentHp = gameObject.GetComponent<Enemy>().health; //GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber];

//
		if(hpDisplay == null){
			hpDisplay = GUIManager.Instance.bossHPDisplay.gameObject;
		}
		ActivateBoss();

	}

	public virtual void ActivateBoss(){
        gameObject.SetActive(true);
        currentRoom = RoomManager.Instance.currentRoom;
		gameObject.GetComponent<EnemyTakeDamage>().bossEnemy = true;

	}

	public void ActivateHpDisplay(){
        if (hpDisplay != null) {
            hpDisplay.SetActive(true);
            hpDisplay.GetComponent<Animator>().Play("bossHealthDisplay",-1,0f);
            hpDisplay.GetComponent<GUI_BossHpDisplay>().maxHp = hp;
            hpDisplay.GetComponent<GUI_BossHpDisplay>().UpdateBossHp(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]);
            SoundManager.instance.PlaySingle(hpDisplayStartSfx);
        }
	}

    public void DeactivateHpDisplay()
    {
        if (hpDisplay != null)
            hpDisplay.SetActive(false);
    }

    public virtual void UpdateBossHp(int hp){
        if (hpDisplay != null)
            hpDisplay.GetComponent<GUI_BossHpDisplay>().UpdateBossHp(hp);
    }

	public IEnumerator BossDeath(){
		Debug.Log("BOSS DEATH ACTIVATE ***********");


		if(vanishAtDeath){
            GameStateManager.Instance.PushState(typeof(MovieState));
            CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,"zoom");
//			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = 0;
			//yield return new WaitForSeconds(1.5f);

			GameObject player = GameObject.FindGameObjectWithTag("Player");
			//GameStateManager.Instance.PushState(typeof(DialogState));
			Time.timeScale = .2f;
			yield return new WaitForSeconds(.5f);
			Time.timeScale = 1;
			InvokeRepeating("DeathSmoke",.1f,.2f);
			yield return new WaitForSeconds(1f);
			GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost");
			deathGhost.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
	
            //CamManager.Instance.mainCamEffects.CameraPan(objectToPanTo.transform.position,"BossItem");
            //CamManager.Instance.mainCamEffects.objectToSpawn = objectToPanTo;
            GlobalVariableManager.Instance.BOSSES_KILLED |= GlobalVariableManager.BOSSES.ONE; //use this as way to tell if player has upgrade
            if(currentRoom != null){
				for(int i = 0; i < currentRoom.bosses.Count; i++){//disable all other bosses at death
					currentRoom.bosses[i].SetActive(false);
				}
			}
            GameStateManager.Instance.PopState();
            DeactivateHpDisplay();
            BossDeathEvent();
        } else {
            controller.SendTrigger(EnemyTrigger.DEATH);
		}
	}

	public virtual void BossEvent(){

		//nothing for basic boss

	}

	public virtual void Dazed(){
		//nothing for basic boss
	}


	public virtual void BossDeathEvent(){

		//nothing for basic boss

	}

	public virtual void BossDeactivateEvent(){ //for when player dies
		Debug.Log("Boss Deactivate activated...?");
		//nothing for basic boss

	}


	void DeathSmoke(){
		if(deathSmokeNumber < 15){
			GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff");
			deathSmoke.transform.position = new Vector3((transform.position.x+ Random.Range(-4,4)), transform.position.y+ Random.Range(-4,4), transform.position.z);
			deathSmokeNumber++;
		}else{
			CancelInvoke();
		}
	}

}
