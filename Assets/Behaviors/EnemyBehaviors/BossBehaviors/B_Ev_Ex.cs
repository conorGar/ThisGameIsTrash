using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExStateController))]
public class B_Ev_Ex : Boss {

	

	public GameObject myProjectile;
	public GameObject myParticles;
	public AudioClip cast;
	public AudioClip teleport;
	public AudioClip teleportTrail;
	public AudioClip blobSpawnSFX;

	Vector3 playerPosition;

	Color myColor;//needed for fade in/fadeOut
	tk2dSpriteAnimator myAnim;
    Vector2 teleportDestination;
    public GameObject teleportTrailParticle;
    public ParticleSystem slimeSpawnPS;
	[HideInInspector]
    public List<GameObject> currentBlobs = new List<GameObject>();
	[HideInInspector]
    public bool initialTeleport = true;
    float randomCastTime = 0f;
    CAST_TYPE nextCastType = CAST_TYPE.TELEPORT;
    public float nextCastTime = 0f;
    public float recoverDazeTime = 10f;
    public float nextRecoverDazeTime = 0f;
    GameObject dazedStars;
    public PathGrid pathGrid;

	// Use this for initialization
	void Awake () {
		myColor = gameObject.GetComponent<tk2dSprite>().color;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        controller = GetComponent<ExStateController>();
    }

    //for debug
    void OnEnable()
    {  
        StopAllCoroutines();

        Reappear();

        // Reset Questio
        UnDazed();

        GenerateCast(CAST_TYPE.TELEPORT);
        nextCastTime = Time.time + 2f;
        GetComponent<EnemyTakeDamage>().currentHp = 4;
    }

    private void OnDisable()
    {
        CancelInvoke();
        KillSlimes();
        GetComponent<Animator>().enabled = false; // Animator won't allow translations if it's active.  It's the worst.
    }



    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState) || initialTeleport) {
            switch (controller.currentState.GetState()) {
                case EnemyState.IDLE:
                    // ready to cast
                    if (nextCastTime < Time.time) {
                        Cast();
                    }
                    break;
                case EnemyState.TELEPORT:
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, teleportDestination, 5 * Time.deltaTime);
                    break;
                case EnemyState.DAZED:
                    // Undaze and teleport.
                    if (nextRecoverDazeTime < Time.time) {
                        UnDazed();
                        GenerateCast(CAST_TYPE.TELEPORT);
                        nextCastTime = Time.time + 2f;
                        controller.SendTrigger(EnemyTrigger.IDLE);
                    }
                    break;
                case EnemyState.CARRIED:
                    StopDazedStars();
                    break;
            }
        }
	}

    public void Teleport()
    {
        StartCoroutine(TeleportEnumerator());
    }

	IEnumerator TeleportEnumerator(){
		SoundManager.instance.PlaySingle(teleport);

        Vanish();

		yield return new WaitForSeconds(.3f);
		SoundManager.instance.PlaySingle(teleportTrail);

		teleportTrailParticle.SetActive(true);
		teleportTrailParticle.GetComponent<ParticleSystem>().Play();
		if(!initialTeleport){
		    teleportDestination = new Vector2(Random.Range(-55f,-4f),Random.Range(134f,149f)); 
		}else{
			if(PlayerManager.Instance.player.transform.position.x < -25f){//player on left side
				teleportDestination = new Vector2(-9f,144f); 
			}else{
				teleportDestination = new Vector2(-40f,144f); 
			}
		}
		//gameObject.transform.localPosition = new Vector2(Random.Range(-36f,17f),Random.Range(-14f,10f));
		yield return new WaitUntil(() => (Vector2.Distance(gameObject.transform.position, teleportDestination) < 1));
		SoundManager.instance.PlaySingle(teleport);

        Reappear();

        if (initialTeleport) { // specifically create a blob after 1 second after the initial teleport.
            initialTeleport = false;
            GenerateCast(CAST_TYPE.SPAWN_ADD);
            nextCastTime = Time.time + 1f;
        } else {
            GenerateCast();
            nextCastTime = randomCastTime;
        }

        controller.SendTrigger(EnemyTrigger.IDLE);
    }

    void Vanish()
    {
        //turn invisible
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = 1; //transparent layer, wont collide with anything
        this.gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f, .3f);
        this.gameObject.GetComponent<SpecialEffectsBehavior>().FadeOut();
        myParticles.SetActive(true);
        myParticles.GetComponent<ParticleSystem>().Play();
    }

    void Reappear()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.layer = 9; //enemy layer again

        gameObject.GetComponent<tk2dSprite>().color = new Color(myColor.r, myColor.g, myColor.b, 1); //fade back
        myParticles.GetComponent<ParticleSystem>().Play();
        teleportTrailParticle.SetActive(false);
    }

    public void SpawnBlob()
    {
        if (currentBlobs.Count < 3) {
            SoundManager.instance.PlaySingle(blobSpawnSFX);
            GameObject spawnedEnemy = ObjectPool.Instance.GetPooledObject("enemy_slime", gameObject.transform.position);
            if (spawnedEnemy != null) {
                spawnedEnemy.GetComponent<EnemyTakeDamage>().otherRespawner = this;
                spawnedEnemy.GetComponent<EnemyTakeDamage>().bossSpawnedEnemy = true;//null spawner ID, fixes glitch where killing Ex's slimes would kill slimes in hall
                spawnedEnemy.GetComponent<EnemyPath>().pathGrid = pathGrid;
                slimeSpawnPS.Play();
                currentBlobs.Add(spawnedEnemy);
            }
        }

        // immediately teleport after spawning a blob or not.
        GenerateCast(CAST_TYPE.TELEPORT);
        nextCastTime = Time.time;
        controller.SendTrigger(EnemyTrigger.IDLE);
    }

    // Helpers
    void Cast()
    {
        // Cast based on type.
        switch (nextCastType) {
            case CAST_TYPE.SPAWN_ADD:
                controller.SendTrigger(EnemyTrigger.CAST_SPAWN_BLOB);
                break;
            case CAST_TYPE.TELEPORT:
                controller.SendTrigger(EnemyTrigger.CAST_TELEPORT);
                break;
        }
    }

    // Generates the next cast time and what will get cast.
    void GenerateCast(CAST_TYPE type = CAST_TYPE.NONE)
    {
        // Randomize cast type if none is defined
        if (type == CAST_TYPE.NONE) {
            int randomNextAction = Random.Range(1, 2);
            Debug.Log("RANDOM NEXT ACTION " + randomNextAction);
            if (randomNextAction == 1) { // Spawn a Blob
                Debug.Log("CAST SPAWN ADD");
                type = CAST_TYPE.SPAWN_ADD;
            } else { // Teleport
                Debug.Log("CAST TELEPORT");
                type = CAST_TYPE.TELEPORT;
            }
        }

        nextCastType = type;

        // Teleport if there are already 3 blobs.
        if (currentBlobs.Count > 3) {
            nextCastType = CAST_TYPE.TELEPORT;
        }

        // Generate cast time based on type.
        switch (type) {
            case CAST_TYPE.SPAWN_ADD:
                GenerateCastTime(CAST_TYPE.SPAWN_ADD);
                break;
            case CAST_TYPE.TELEPORT:
                GenerateCastTime(CAST_TYPE.TELEPORT);
                break;
        }
    }

    // generates a random cast time.  It's won't take effect unto nextCastTime is set though.
    void GenerateCastTime(CAST_TYPE type)
    {
        switch (type) {
            case CAST_TYPE.SPAWN_ADD:
                randomCastTime = Time.time + Random.Range(2f, 6f);
                break;
            case CAST_TYPE.TELEPORT:
                randomCastTime = Time.time + Random.Range(3f, 6f);
                break;
        }
    }

    public override void Dazed(){
		gameObject.layer = 11;
        nextRecoverDazeTime = recoverDazeTime + Time.time;
		StopAllCoroutines();
        StartDazedStars();
	}

    void UnDazed(){
        gameObject.layer = 9;
        StopDazedStars();
    }

    public void KillSlimes(){
    	for(int i = 0; i <currentBlobs.Count;i++){
    		currentBlobs[i].SetActive(false);
    	}

        currentBlobs.Clear();
    }

    void StartDazedStars()
    {
        if (dazedStars != null)
            ObjectPool.Instance.ReturnPooledObject(dazedStars);

        dazedStars = ObjectPool.Instance.GetPooledObject("effect_dazed", new Vector3(transform.position.x, transform.position.y + 2, 0));
        dazedStars.transform.parent = gameObject.transform;
    }

    void StopDazedStars()
    {
        if (dazedStars != null) {
            ObjectPool.Instance.ReturnPooledObject(dazedStars);
            dazedStars = null;
        }
    }
}
