using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

public class FireTowardPlayer : MonoBehaviour {

	public float projectileSpeed;
	public float fireRate;
	public bool myProjectileFalls = false;
	public bool fireAtStart = true;
	public float randomRateChanger;

	public GameObject projectile;
	public AudioClip throwSFX;

	int throwOnceCheck = 1;
	float nextThrowTime;

	[HideInInspector]
	public GameObject target;

	private tk2dSpriteAnimator anim;
	Vector2 startScale;
	protected GenericEnemyStateController controller;


	// Use this for initialization

	void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
    }

	void OnEnable () {
		Debug.Log("Fire toward player on enable activated");
		anim = GetComponent<tk2dSpriteAnimator>();
		CancelInvoke();
		startScale = gameObject.transform.localScale;
        //InvokeRepeating("Fire",2.0f,fireRate);

        if (PlayerManager.Instance != null && PlayerManager.Instance.player)
            target = PlayerManager.Instance.player;
        if(fireAtStart){
       	 //StartCoroutine("Fire");
       	 throwOnceCheck = 0;
       	 }else{
       	 throwOnceCheck = 1;
       	 }
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			//Debug.Log(controller.name);
			switch (controller.GetCurrentState()) {
				case EnemyState.IDLE:
					if(Time.time > nextThrowTime && throwOnceCheck == 0){
						Debug.Log("Fire rate reached, throw time is now");
						StartCoroutine("Fire");
						throwOnceCheck = 1;
					}
				break;
			}
		}
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
						transform.localScale = startScale;
					} else{
						transform.localScale = new Vector3(startScale.x*-1,startScale.y,1);
					}
					GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
					bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues

					if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
						bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
						bullet.GetComponent<Ev_ProjectileTowrdPlayer>().target = this.target;
					}
					bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
					SoundManager.instance.PlaySingle(throwSFX);

					if(!myProjectileFalls && bullet.GetComponent<Ev_FallingProjectile>() !=null)
						bullet.GetComponent<Ev_FallingProjectile>().enabled = false;

					nextThrowTime = Time.time + fireRate + Random.Range(0,randomRateChanger);
					throwOnceCheck = 0;
					StopCoroutine("Fire");
				}
			}
		}

	}

	/*IEnumerator AnimationControl(){

		yield return new WaitForSeconds(0.7f);
		//Vector3 playerPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);

		Debug.Log("FiredObject");
		GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
		bullet.GetComponent<Ev_ProjectileTowrdPlayer>().enabled = true; // starts off disabled only so i didnt have to make another tag for rocks that DONT follow player(like ones that spawn from boulder.) feel free to just do that if tis causes issues
		if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().target = this.target;
		}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(throwSFX);

		if(!myProjectileFalls && bullet.GetComponent<Ev_FallingProjectile>() !=null)
			bullet.GetComponent<Ev_FallingProjectile>().enabled = false;
		//bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeed;
		//bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position).normalized *projectileSpeed;

		anim.Play("idle");
		StartCoroutine("Fire");
		StopCoroutine("AnimationControl");

	}*/
}
