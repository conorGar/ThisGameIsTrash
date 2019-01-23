using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Ex : Boss {

	

	public GameObject myProjectile;
	public GameObject myParticles;
	public GameObject player;
	public AudioClip cast;
	public AudioClip teleport;
	public AudioClip teleportTrail;
	public List<MonoBehaviour> dazeDisables = new List<MonoBehaviour>();

	Vector3 playerPosition;

	Color myColor;//needed for fade in/fadeOut
	tk2dSpriteAnimator myAnim;
    bool isActing = true;
    string action = "Teleport";
    Vector2 teleportDestination;
    bool isTeleporting;
    public GameObject teleportTrailParticle;
    public ParticleSystem slimeSpawnPS;
	public Room myRoom;
	[HideInInspector]
    public List<GameObject> currentBlobs = new List<GameObject>();
	// Use this for initialization
	void Awake () {
		myColor = gameObject.GetComponent<tk2dSprite>().color;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        myAnim.AnimationEventTriggered = AnimationEventCallback;
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (!isActing) {
                StartCoroutine(action);
                isActing = true;
            }
            if(isTeleporting){
            	gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,teleportDestination,5*Time.deltaTime);
            }
        }

		if(RoomManager.Instance.currentRoom != myRoom){//TODO: probably not the best way to keep this from going when player isnt in room but whatever
			CancelInvoke();
			for(int i = 0; i < currentBlobs.Count;i++){
				currentBlobs[i].SetActive(false);
			}
			currentBlobs.Clear();
		}
	}

	//for debug
	void OnEnable(){
        StopAllCoroutines();

        // Reset Questio
        UnDazed();

        action = "Teleport";
        isActing = false;
    }

	IEnumerator Teleport(){
		Debug.Log("Teleport Activated ----------- !");
		SoundManager.instance.PlaySingle(teleport);
		this.gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f,.3f);
		this.gameObject.GetComponent<SpecialEffectsBehavior>().FadeOut();
		myParticles.SetActive(true);
		myParticles.GetComponent<ParticleSystem>().Play();
		yield return new WaitForSeconds(.3f);
		//TODO: when finalize room art, ex cannot land on one of the acid piles in the room
		SoundManager.instance.PlaySingle(teleportTrail);
		isTeleporting = true;

		//turn invisible
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.layer = 0; //default layer, wont collide with anything


		teleportTrailParticle.SetActive(true);
		teleportTrailParticle.GetComponent<ParticleSystem>().Play();
		teleportDestination = new Vector2(Random.Range(-55f,-4f),Random.Range(134f,149f)); 
		//gameObject.transform.localPosition = new Vector2(Random.Range(-36f,17f),Random.Range(-14f,10f));
		yield return new WaitUntil(() => (Vector2.Distance(gameObject.transform.position, teleportDestination) < 1));
		isTeleporting = false;
		SoundManager.instance.PlaySingle(teleport);

		//reappear
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		gameObject.layer = 9; //enemy layer again

		gameObject.GetComponent<tk2dSprite>().color = new Color(myColor.r,myColor.g,myColor.b,1); //fade back
		myParticles.GetComponent<ParticleSystem>().Play();
		teleportTrailParticle.SetActive(false);
		yield return new WaitForSeconds(Random.Range(1f,3f));
		int randomNextAction = Random.Range(0, 3);
		if (randomNextAction == 0) {
			yield return new WaitForSeconds(Random.Range(1f, 3f));
            action = "SpawnBlob";//"Fire";

        }
        else {
			yield return new WaitForSeconds(Random.Range(1f, 3f));
			action = "Teleport";
        }
       
		//yield return PrepareNextAction();
        isActing = false;
    }

    IEnumerator Fire()
    {
        Debug.Log("FIRE ACTIVATED-----------!");
        if (myAnim.CurrentClip.name != "hurt") {
            myAnim.Play("cast"); // Triggers CAST_FINISHED on the last frame to go back to idle.
            StartCoroutine(FireballControl());
        }
        else {
            yield return PrepareNextAction();
        }
    }

    IEnumerator FireballControl()
    {
        // Spawn and home in on the player
        myProjectile.transform.position = gameObject.transform.position;
        myProjectile.GetComponent<KillSelfAfterTime>().CancelInvoke();//prevents projectile from dying shortly after spawn
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        myProjectile.SetActive(true);
        SoundManager.instance.PlaySingle(cast);
        Vector2 moveDirection = (playerPosition - myProjectile.transform.position).normalized * 10;
        myProjectile.GetComponent<FollowPlayer>().enabled = true;
        myProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y);
        yield return new WaitForSeconds(2f);

        // Hovering in place
        myProjectile.GetComponent<FollowPlayer>().enabled = false;
        yield return new WaitForSeconds(3f);

        // Destroy Fireball
        if (myProjectile.activeInHierarchy == true) {
            myProjectile.SetActive(false);
            myProjectile.transform.localPosition = Vector3.zero;
        }

        yield return PrepareNextAction();
    }

    // Helpers
    IEnumerator PrepareNextAction()
    {
        // Figure out the next action
        int randomNextAction = Random.Range(0, 4);
        if (randomNextAction == 0) {
			yield return new WaitForSeconds(Random.Range(1f, 3f));
            action = "SpawnBlob";//"Fire";

        }
        else {
			action = "Teleport";
        }
        isActing = false;
    }

	protected override void Dazed(){
		//gameObject.GetComponent<EnemyTakeDamage>().StopAllCoroutines();//so follow player isn't enabled again
		Debug.Log("Dazed activated - Ex");
		gameObject.layer = 11;
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		myAnim.Play("dazed");
		StopAllCoroutines();
		for(int i = 0; i < dazeDisables.Count; i++){
			dazeDisables[i].enabled = false;
		}

		//this.enabled = false;
	}

    void UnDazed(){
        for (int i = 0; i < dazeDisables.Count; i++) {
            dazeDisables[i].enabled = true;
        }

        gameObject.layer = 9;
        gameObject.GetComponent<ThrowableObject>().enabled = false;

        myAnim.Play("idle");
    }

    // Callbacks
    void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
        var frame = clip.GetFrame(frameNo);
        Debug.Log("Animation Trigger Check: " + frame.eventInfo);
        switch (frame.eventInfo) {
            case "CAST_FINISHED":
                myAnim.Play("idle");
                break;
            default:
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
                break;
        }
    }


    IEnumerator SpawnBlob(){
		if(currentBlobs.Count < 3){
			GameObject spawnedEnemy = ObjectPool.Instance.GetPooledObject("enemy_slime",gameObject.transform.position);
			spawnedEnemy.GetComponent<EnemyTakeDamage>().otherRespawner = this;
			slimeSpawnPS.Play();
			currentBlobs.Add(spawnedEnemy);
			Debug.Log("ENEMY SHOULDVE BEEN SPAWNED");
			yield return new WaitForSeconds(1f); 
			StartCoroutine("Teleport");
		}else{
			yield return new WaitForSeconds(1f); 
    		StartCoroutine("Teleport");
    	}
    }


}
