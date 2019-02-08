using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMovement : MonoBehaviour {

	private tk2dSpriteAnimator anim;
    public float speed = 8f;
    private Vector2 movement;
    public AudioSource myFootstepSource;
 
    bool isDiagonal = false;
    bool noDelayStarted = false;
    public float delay = 0.05f;
    private int directionFacing = 1; //1 = right, 2 = left, 3 = up, 4 = down
    private float momentum = 0f;
 
	//public GameObject legs;
	//public GameObject shadow;
	public GameObject walkCloudPS;

	public GameObject myLegs;

	public AudioClip footsteps1;
	public AudioClip footsteps2;


	[HideInInspector]
	public tk2dSpriteAnimator legAnim;
	Vector3 transformScale; // used for facing different directions

	public bool clipOverride; //set by pickUpable object

    // Use this for initialization
    void Start () {


        legAnim = myLegs.GetComponent<tk2dSpriteAnimator>();
        anim = GetComponent<tk2dSpriteAnimator>();


        transformScale = gameObject.transform.localScale;

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SPEEDY)){
			speed += 1.5f;
			myLegs.GetComponent<TrailRenderer>().enabled = true;
		}

        movement = new Vector2(0f, 0f);
        StopMovement();
        walkCloudPS.GetComponent<ParticleSystem>().Stop();

        GameStateManager.Instance.RegisterChangeStateEvent(OnChangeState);
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
            movement = new Vector2(inputX, inputY);

            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)) {
                GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);
                GetComponent<JimStateController>().SetFlag((int)JimFlag.FACING_LEFT);

                if(GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("DumpsterDash",INPUTACTION.MOVELEFT);
                }

                directionFacing = 2;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)) {
                GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);
                GetComponent<JimStateController>().RemoveFlag((int)JimFlag.FACING_LEFT);

                if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("DumpsterDash",INPUTACTION.MOVERIGHT);
                }

                directionFacing = 1;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)) {

                GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);

                if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("DumpsterDash",INPUTACTION.MOVEUP);
                }
                directionFacing = 3;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)) {

                GetComponent<JimStateController>().SetFlag((int)JimFlag.MOVING);

                if (GlobalVariableManager.Instance.IsPinEquipped(PIN.DUMPSTERDASH)){
					gameObject.GetComponent<PinFunctionsManager>().StartCoroutine("DumpsterDash",INPUTACTION.MOVEDOWN);
                }
                directionFacing = 4;
            }

            //Diagonals

            if (inputX != 0 && inputY != 0) {
            	
                if (momentum != 4) {
                    momentum = 4f;
              
                    InvokeRepeating("FootstepSounds", .2f, .2f); //just have this here so it only happens once
                }
                if(legAnim.GetClipByName("walk") != null)
                	legAnim.Play("walk");

                isDiagonal = true;
               
            }
            else {
                if (isDiagonal && !noDelayStarted) {
                    StartCoroutine(NoMoreDiagonal());
                    noDelayStarted = true;
                }
                else if (anim != null) {
                    
                    if (movement.x == 0 && movement.y == 0) {
                        walkCloudPS.GetComponent<ParticleSystem>().Stop();

                        GetComponent<JimStateController>().RemoveFlag((int)JimFlag.MOVING);

                        //not instant stop
                        if (momentum > 0){
                            momentum = momentum - .2f;
                        }else{
							if(GlobalVariableManager.Instance.IS_HIDDEN == false && GlobalVariableManager.Instance.IsPinEquipped(PIN.SNEAKINGSCRAPPER)){
								gameObject.GetComponent<PinFunctionsManager>().SneakyScrapper();
							}

                        }
                    }
                    else if (momentum != 4) {
                        momentum = 4f;
                        if(GetComponent<JimStateController>().GetCurrentState() == JimState.CARRYING) {
								InvokeRepeating("FootstepSounds", .2f, .4f); //slower footsteps when carrying something
                        }else{
                       			InvokeRepeating("FootstepSounds", .2f, .2f);
                        }
						if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SNEAKINGSCRAPPER) && GlobalVariableManager.Instance.IS_HIDDEN){ //put here for function with 'Sneaky Scrapper' function, not sure if this will work with later sneaking functions...
            				gameObject.GetComponent<PinFunctionsManager>().SneakyScrapperReturn();
            				Debug.Log("got here- sneaky scrapper");
            			}
                        walkCloudPS.SetActive(true); //just have this here so it only happens once
                        walkCloudPS.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
            if (movement.x == 0 && movement.y == 0) {

                CancelInvoke("FootstepSounds");




                //not instant stop
                if (directionFacing == 3) {//anim.CurrentClip.name == "ani_jimIdleUp"){
                    transform.Translate(new Vector2(0, momentum) * Time.deltaTime);
                }
                else if (directionFacing == 4) {
                    transform.Translate(new Vector2(0, momentum * -1) * Time.deltaTime);
                }
                else if (gameObject.transform.localScale.x > 0) {
                    transform.Translate(new Vector2(momentum, 0) * Time.deltaTime);
                }
                else if (gameObject.transform.localScale.x < 0) {
                    transform.Translate(new Vector2(momentum * -1, 0) * Time.deltaTime);
                }
            }
            if (GetComponent<JimStateController>().GetCurrentState() == JimState.IDLE ||
                GetComponent<JimStateController>().GetCurrentState() == JimState.CARRYING) {
                // correct the facing.
                if (inputX < 0) { // The player is holding left.
                    if (transform.localScale.x > 0) {
                        gameObject.transform.localScale = new Vector2(transformScale.x * -1, transformScale.y);
                        GetComponent<JimStateController>().SetFlag((int)JimFlag.FACING_LEFT);
                    }
                } else if (inputX > 0) { // The player is holding right.
                    if (transform.localScale.x < 0) {
                        gameObject.transform.localScale = new Vector2(transformScale.x, transformScale.y);
                        GetComponent<JimStateController>().RemoveFlag((int)JimFlag.FACING_LEFT);
                    }
                }


                transform.Translate(movement * speed * Time.deltaTime);
                /*//show legs and change to current animation of Jim
                if (!clipOverride && myLegs.activeInHierarchy) {

                    legAnim.Play(anim.CurrentClip.name);
                    legAnim.PlayFromFrame(anim.CurrentFrame);
                }*/
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Debug.Log("JimStateController.GetCurrentState() == " + GetComponent<JimStateController>().GetCurrentState());
            }

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

        CancelInvoke();
		//StopAllCoroutines();
		legAnim.Play(anim.CurrentClip.name);
    	this.enabled = true;
    }
    void FootstepSounds(){
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            RandomizeSfx(footsteps2, footsteps1);
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
                walkCloudPS.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
