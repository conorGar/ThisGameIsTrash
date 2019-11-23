using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttacker : MonoBehaviour
{

	public int currentHP;
	public int damageStr; 
	public int speed;
	public int xpGiven;
	protected EnemyStateController controller;
	public List<WeaponDefinition> myPossibleDrops = new List<WeaponDefinition>();
	public int[] dropChanceTable; //Corresponds to each position in 'myPossibleDrops'. Should add up to 100
	public string myDeadBodyName;
	Vector2 attackTargetPosition;
	Vector2 attackStartingPosition;
	float attackDisFromEnemy; //distance between hero and enemy needed until hero switches to 'hitting' ani varies based on enemy size
	bool returnToStartPositon;

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

    void Update(){
		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){
	    	if(controller.GetCurrentState() == EnemyState.CHASE){
				if(Vector2.Distance(gameObject.transform.position,attackTargetPosition) > attackDisFromEnemy){
					gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, attackTargetPosition, 10*Time.deltaTime);
					Debug.Log(Vector2.Distance(gameObject.transform.position,attackTargetPosition));
				}else{
					StartCoroutine("Attack");
				}
	    	}
	    }
    }

	public void TakeDamage(int damage){
		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){
		Debug.Log(this.gameObject.name + "Takes " + damage +" damage!");
			currentHP -= damage;
			SpawnDamageStars(damage);
			controller.SendTrigger(EnemyTrigger.HIT);

			//Check for death
			if(currentHP <= 0){
				Death();
			}else{
				BattleManager.Instance.ReturnFromAttack();
			}
		}
	}

	public void MoveToAttack(GameObject heroTarget){ //Handles Enemy Attack Animations/Movement to selected enemy


		attackDisFromEnemy = this.gameObject.GetComponent<tk2dSprite>().GetBounds().max.x +.5f; //+.5f to account for player size
		controller.SendTrigger(EnemyTrigger.CHASE);
		Debug.Log("*****Attack Distance From HERO:" + attackDisFromEnemy);

		attackTargetPosition = heroTarget.transform.position;
		attackStartingPosition = gameObject.transform.position;

	}

	public IEnumerator Attack(){


		controller.SendTrigger(EnemyTrigger.LUNGE);
		Debug.Log("Enemy Move To Attack Activate" + BattleManager.Instance.currentState);
		while (controller.GetCurrentState() == EnemyState.LUNGE) //wait until end of lunge animation
           			yield return null;
		Debug.Log("Enemy Move To Attack Finish" +  BattleManager.Instance.currentState);

		BattleManager.Instance.ReturnFromAttack();
		gameObject.GetComponent<EnemyTurnDelayBar>().StartCount();
	}

	void SpawnDamageStars(int damageDealt){
		GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars");
		damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
		damageCounter.SetActive(true);
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
		damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.SetActive(true);
		damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
	}

	void Death(){
		GiveXP();
		BattleManager.Instance.RemoveEnemy(this);
		GetComponent<EnemyStateController>().SendTrigger(EnemyTrigger.DEATH);
		Drop();
		GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff"); 
		deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost",new Vector3((transform.position.x), transform.position.y, transform.position.z));
		deathGhost.GetComponent<Ev_DeathGhost>().OnSpawn();

		GameObject body = ObjectPool.Instance.GetPooledObject("enemyBody",gameObject.transform.position);
		body.GetComponent<tk2dSprite>().SetSprite(myDeadBodyName);



		this.gameObject.SetActive(false);

	}


	void GiveXP(){
		/* 
			For Jim and then each Partner, gain xp by calling either the data directly(Jim) or by calling the GainXP() method in 
			HeroAttacker.cs(Partners) , amount depending on current partner count
		*/

		//TODO: only if hero is alive!
		int xp = Random.Range(xpGiven-5,xpGiven+5);




		foreach(HeroAttacker hero in BattleManager.Instance.heroList){
			hero.GainXP(xp);
		}

	}

	void Drop(){

		//total of all chances, should just be 100 but just in case
		int total = 0;
		foreach(int chance in dropChanceTable){
			total += chance;
		}

		int dropChance = Random.Range(0,total);

		for(int i = 0; i < myPossibleDrops.Count; i++){
			//compare is my random number < the current chance
			if(dropChanceTable[i] >= dropChance){
				//Drop the current weapon
				GameObject droppedWeapon = ObjectPool.Instance.GetPooledObject("WeaponDrop",gameObject.transform.position);
				droppedWeapon.GetComponent<Ev_DroppedWeapon>().setWeaponData(myPossibleDrops[i]);
				break;
			}else{
				dropChance -= dropChanceTable[i];
			}
		}
		
	}

}

