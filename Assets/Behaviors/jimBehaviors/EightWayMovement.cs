using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMovement : MonoBehaviour {

	private tk2dSpriteAnimator anim;
    public float speed = 8f;
    public float momentum_max = 2f;
    public float momentum_decay = .2f;
    public float momentum_build = 1f;
    public float dashAngleThreshold = 40f; // flub angle when using the dash button
    private Vector2 movement;
    private Vector2 momentum;
    public AudioSource myFootstepSource;
    public ParticleSystem waterSplash;

    public float footstepWalkInterval = .2f;
    public float footstepCarryInterval = .4f;
    public float nextFootstepTime = 0f;
 
    bool isDiagonal = false;
    bool noDelayStarted = false;
    public float delay = 0.05f;
    private int directionFacing = 1; //1 = right, 2 = left, 3 = up, 4 = down

 
	//public GameObject legs;
	//public GameObject shadow;
	public GameObject walkCloudPS;

    public TrailRenderer speedyTrailRenderer;

	public AudioClip footsteps1;
	public AudioClip footsteps2;

	Vector3 transformScale; // used for facing different directions

	public bool clipOverride; //set by pickUpable object

	float currentBaseSpeed; //set when speed is temporary changed by environment to return to when done.
	bool alreadySlowed;

    // Use this for initialization
    void Start () {

        anim = GetComponent<tk2dSpriteAnimator>();


        transformScale = gameObject.transform.localScale;

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SPEEDY)){
			speed += 1.5f;
            speedyTrailRenderer.enabled = true;
		}

        movement = new Vector2(0f, 0f);
        StopMovement();

        if (walkCloudPS.GetComponent<ParticleSystem>().isPlaying)
            walkCloudPS.GetComponent<ParticleSystem>().Stop();

        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
        currentBaseSpeed = speed;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.UnregisterChangeStateEvent(OnChangeState);
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {

            float inputX = ControllerManager.Instance.GetAxis(INPUTACTION.MOVELEFT);
            float inputY = ControllerManager.Instance.GetAxis(INPUTACTION.MOVEUP);
            var jimStateController = GetComponent<JimStateController>();
            movement = new Vector2(inputX, inputY);

            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.DASH)) {
                if (jimStateController.GetCurrentState() != JimState.CARRYING) {
                    // Get an angle from the input axis.
                    var rad = Mathf.Atan2(inputY, inputX);
                    var degree = rad * Mathf.Rad2Deg;
                    Debug.Log("DashDegree: " + degree);
                    // immediately dash in a direction if the movement axis is close to that cardinal direction.
                    if (degree > 90f - dashAngleThreshold && degree < 90f + dashAngleThreshold) { // up
                        gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVEUP, true);
                        Debug.Log("DASHING UP");
                    } else if (degree > 0 - dashAngleThreshold && degree < 0 + dashAngleThreshold) { // right
                        gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVERIGHT, true);
                        Debug.Log("DASHING RIGHT");
                    } else if (degree > -90f - dashAngleThreshold && degree < -90f + dashAngleThreshold) { // down
                        gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVEDOWN, true);
                        Debug.Log("DASHING DOWN");
                    } else if (Mathf.Abs(degree) > 180f - dashAngleThreshold && Mathf.Abs(degree) <= 180f) { // left
                        gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVELEFT, true);
                        Debug.Log("DASHING LEFT");
                    }
                }
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)) {
                StartMovement();
                jimStateController.SetFlag((int)JimFlag.FACING_LEFT);

               // if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
				if(jimStateController.GetCurrentState() != JimState.CARRYING)
					gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVELEFT);
               // }

                directionFacing = 2;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)) {
                StartMovement();
                jimStateController.RemoveFlag((int)JimFlag.FACING_LEFT);

              // if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
				if(jimStateController.GetCurrentState() != JimState.CARRYING)
					gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVERIGHT);
               // }

                directionFacing = 1;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)) {
                StartMovement();

               // if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
				if(jimStateController.GetCurrentState() != JimState.CARRYING)
					gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVEUP);
               // }
                directionFacing = 3;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)) {
                StartMovement();

                //if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
				if(jimStateController.GetCurrentState() != JimState.CARRYING)
					gameObject.GetComponent<PinFunctionsManager>().StartDumpsterDash(INPUTACTION.MOVEDOWN);
               // }
                directionFacing = 4;
            }

            //Diagonals

            if (inputX != 0 && inputY != 0) {
                isDiagonal = true;
               
            }
            else {
                if (isDiagonal && !noDelayStarted) {
                    StartCoroutine(NoMoreDiagonal());
                    noDelayStarted = true;
                } else if (anim != null) {
                    
                    if (movement == Vector2.zero) {
                        if (walkCloudPS.GetComponent<ParticleSystem>().isPlaying)
                            walkCloudPS.GetComponent<ParticleSystem>().Stop();

                        jimStateController.RemoveFlag((int)JimFlag.MOVING);

                        // if all the momentum is expanded, reactivate Sneaky Scrapper.
                        if (momentum.magnitude == 0) {
							if(GlobalVariableManager.Instance.IS_HIDDEN == false && GlobalVariableManager.Instance.IsPinEquipped(PIN.SNEAKINGSCRAPPER)){
								gameObject.GetComponent<PinFunctionsManager>().SneakyScrapper();
							}
                        }
                    } else {
						if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SNEAKINGSCRAPPER) && GlobalVariableManager.Instance.IS_HIDDEN){ //put here for function with 'Sneaky Scrapper' function, not sure if this will work with later sneaking functions...
            				gameObject.GetComponent<PinFunctionsManager>().SneakyScrapperReturn();
            				Debug.Log("got here- sneaky scrapper");
            			}

                        if (!walkCloudPS.GetComponent<ParticleSystem>().isPlaying)
                            walkCloudPS.GetComponent<ParticleSystem>().Play();
                    }
                }
            }

            // If the player is inputting a move and in a valid movement state.
            if (movement != Vector2.zero &&
               (jimStateController.GetCurrentState() == JimState.IDLE ||
				jimStateController.GetCurrentState() == JimState.CARRYING || jimStateController.GetCurrentState() ==  JimState.CHARGING)) {

				if(jimStateController.GetCurrentState() ==  JimState.CHARGING){
					speed = 1;
				}else{
					speed = currentBaseSpeed;

                // update stored momentum values.
                momentum = Vector2.ClampMagnitude(momentum + movement * momentum_build, momentum_max);

                // correct the facing.
                if (inputX < 0) { // The player is holding left.
                    if (transform.localScale.x > 0) {
                        gameObject.transform.localScale = new Vector3(transformScale.x * -1, transformScale.y, transformScale.z);
                        jimStateController.SetFlag((int)JimFlag.FACING_LEFT);
                    }
                } else if (inputX > 0) { // The player is holding right.
                    if (transform.localScale.x < 0) {
                        gameObject.transform.localScale = new Vector3(transformScale.x, transformScale.y, transformScale.z);
                        jimStateController.RemoveFlag((int)JimFlag.FACING_LEFT);
                    }
                }
                }
                transform.Translate(movement * speed * Time.deltaTime);

                FootstepSounds();
                     
            // If the player is not inputting a move or is in a state where he is not allowed to move.
            } else {
                transform.Translate(momentum * Time.deltaTime);
            }

            //not instant stop.  Decay momentum by shrinking the vectors magnitude.
            float mag = momentum.magnitude;

            if (mag >= momentum_decay)
                momentum = momentum.normalized * (momentum.magnitude - momentum_decay);
            else
                momentum = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.V)) {
                Debug.Log(GlobalVariableManager.Instance.BASIC_ENEMY_LIST["enemy_spawner 2"].dayOfRevival);
            }
        }
    }
 
    IEnumerator NoMoreDiagonal () {
        yield return new WaitForSeconds (delay);
        isDiagonal = false;
        noDelayStarted = false;
    }

    public void StopMovement(){
        GetComponent<JimStateController>().RemoveFlag((int)JimFlag.MOVING);
    }

    void StartMovement()
    {
        GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);
    }

    public void SlowdownSpeed(){
    	if(!alreadySlowed){
    	currentBaseSpeed = 4;//currentBaseSpeed/2;
    	waterSplash.Play();
    	alreadySlowed = true;
    	}
    }
    public void SpeedReturn(){
    	currentBaseSpeed = 8;//currentBaseSpeed*2;
		waterSplash.Stop();
		alreadySlowed = false;

    }
   

    void FootstepSounds(){
        if (Time.time > nextFootstepTime) {
            RandomizeSfx(footsteps2, footsteps1);

            if (GetComponent<JimStateController>().GetCurrentState() == JimState.CARRYING) {
                nextFootstepTime = Time.time + footstepCarryInterval;
            } else {
                nextFootstepTime = Time.time + footstepWalkInterval;
            }
        }
    }

	public void RandomizeSfx(params AudioClip[] clips){
		myFootstepSource.volume = GlobalVariableManager.Instance.MASTER_SFX_VOL;
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(.9f,1.1f);

		myFootstepSource.pitch = randomPitch;

		myFootstepSource.PlayOneShot(clips[randomIndex]);
	}

    public void UpdateSpeed(float updatedSpeed){
    	speed += updatedSpeed;
    	currentBaseSpeed  = speed;
    }

    public void Dash(){ //activated in Pin Functions Manager
    	if(directionFacing == 1){
    		
    	}
    }

    void OnChangeState(System.Type stateType, bool isEntering)
    {
        if (isEntering) {
            // Entering anything other than the gameplaystate
            if (stateType != typeof(GameplayState)) {
                movement = new Vector2(0f, 0f);
                StopMovement();

                if (walkCloudPS.GetComponent<ParticleSystem>().isPlaying)
                    walkCloudPS.GetComponent<ParticleSystem>().Stop();
            }

            if (stateType == typeof(RespawnState)) {
                momentum = Vector2.zero;
            }
        }
    }
}
