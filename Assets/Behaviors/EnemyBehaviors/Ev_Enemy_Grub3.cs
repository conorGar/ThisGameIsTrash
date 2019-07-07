using UnityEngine;
using System.Collections;

public class Ev_Enemy_Grub3 : EnemyTakeDamage
{
	//shoots 3 times then pauses briefly, can be knocked out of ground by a heavy attack, will follow player if far enough
	public GameObject armor;
	public GameObject burrowingPS;
	public float projectileSpeed;
	public GameObject projectile;
	public bool chaseAfterPlayer;

	Vector2 startingScale = new Vector2();

	Vector2 destinationMark;
	int spitOnceCheck;
	float nextActionTime;

	GameObject target;

	//IDLE- waiting to notice player. Just sitting there idley.
	//RECOVER - Period after firing tripple shot. If player is far enough will start to chase.
	//VULNERABLE - After being knocked out of ground/ hemlet is off.

	void OnEnable(){
		armorRating = 1; //makes sure the armor rating is reset from previous 

	}

	// Use this for initialization
	void Start ()
	{
		base.Start();
		startingScale = gameObject.transform.lossyScale;
		if (PlayerManager.Instance != null && PlayerManager.Instance.player)
            target = PlayerManager.Instance.player;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.IsFlag((int)EnemyFlag.CHASING)) {
				if(Vector2.Distance(gameObject.transform.position,PlayerManager.Instance.player.transform.position) > 10f && controller.GetCurrentState() == EnemyState.RECOVER){
							StartCoroutine("Dive");
					}

				switch (controller.GetCurrentState()) {
				
               
                case EnemyState.IDLE:
					if(Time.time > nextActionTime){
						Debug.Log("TIme is > grub action time: " + spitOnceCheck);
						if(spitOnceCheck < 3){
							Debug.Log("Fire rate reached, throw time is now");
							Spit();
							spitOnceCheck++;
							nextActionTime = Time.time + 1;

						}else{
							spitOnceCheck = 0;
							nextActionTime = Time.time + 3;

						}
					}
					break;
				case EnemyState.LUNGE:
					Debug.Log("grub in lunge state...");
                 	gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,destinationMark,5*Time.deltaTime);
                 break;
            }
            }else{

			switch (controller.GetCurrentState()) {
				
                case EnemyState.IDLE:
					if(Vector2.Distance(gameObject.transform.position,PlayerManager.Instance.player.transform.position) < 10f){
						GameObject notice = ObjectPool.Instance.GetPooledObject("effect_notice", gameObject.transform.position + new Vector3(-1f, gameObject.GetComponent<BoxCollider2D>().size.y/2 + 1f, 0f));
                        notice.transform.parent = this.transform;
						
						controller.SendTrigger(EnemyTrigger.NOTICE); 
					}
                    break;
               
            }
            }

        }
	}


	IEnumerator Dive(){
		Debug.Log("Dive Activate --- grub");
		destinationMark = PlayerManager.Instance.player.transform.position;
		controller.SendTrigger(EnemyTrigger.LUNGE); 
		burrowingPS.SetActive(true);
		yield return new WaitUntil(() => Vector2.Distance(gameObject.transform.position, destinationMark) < 2f );
		burrowingPS.SetActive(false);
		controller.SendTrigger(EnemyTrigger.POPUP);
	}

	void Spit(){
		StartCoroutine("Fire");
	}

	public override void PowerHitEffect(){
		Debug.Log("GRUB POWER HIT EFFECT ACTIVATE!!! WOOOOO!");
		armor.SetActive(false);
		armorRating = 0;
		controller.SendTrigger(EnemyTrigger.VULNERABLE); //use recover for when dive in


	}

	public IEnumerator Fire(){
		//yield return new WaitForSeconds(fireRate);
		if(target == null){
			target = PlayerManager.Instance.player;
		}
		Debug.Log("Fire Enum Activated");
		if(!GlobalVariableManager.Instance.IS_HIDDEN){ //wont fire at player if player is hidden
			if (controller.currentState.GetState() == EnemyState.IDLE) {
				if(gameObject.activeInHierarchy == false){
					StopCoroutine("Fire");
				}
				controller.SendTrigger(EnemyTrigger.PREPARE);

			
				while (controller.GetCurrentState() == EnemyState.PREPARE)
           			yield return null;


				Debug.Log("fired" + controller.GetCurrentState());
				while (controller.GetCurrentState() != EnemyState.THROW)
           			yield return null;
				if (controller.GetCurrentState() == EnemyState.THROW) {
					Debug.Log("fired2");
					if(target.transform.position.x < transform.position.x){
						transform.localScale = startingScale;
					} else{
						transform.localScale = new Vector3(startingScale.x*-1,startingScale.y,1);
					}
					GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
					bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues

					if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
						bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
						bullet.GetComponent<Ev_ProjectileTowrdPlayer>().target = this.target;
					}
					bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
					//SoundManager.instance.PlaySingle(throwSFX);

					//bullet.GetComponent<Ev_FallingProjectile>().enabled = false;

					StopCoroutine("Fire");
				}
			}
		}

	}
}

