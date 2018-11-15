using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightWayMovement : MonoBehaviour {

	private tk2dSpriteAnimator anim;
    public float speed = 8f;
    private Vector2 movement;

 
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
	[HideInInspector]
	public bool carryingAbove;
    // Use this for initialization
    void Start () {


        legAnim = myLegs.GetComponent<tk2dSpriteAnimator>();
        anim = GetComponent<tk2dSpriteAnimator>();


        transformScale = gameObject.transform.localScale;

		if(GlobalVariableManager.Instance.IsPinEquipped(PIN.SPEEDY)){
			speed += 1.5f;
			myLegs.GetComponent<TrailRenderer>().enabled = true;
		}
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {

            float inputX = ControllerManager.Instance.GetAxis(INPUTACTION.MOVELEFT);
            float inputY = ControllerManager.Instance.GetAxis(INPUTACTION.MOVEUP);
            movement = new Vector2(inputX, inputY);
            //Debug.Log(inputX);
            if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT)) {
                if (directionFacing != 2) {
                    gameObject.transform.localScale = new Vector2(transformScale.x * -1, transformScale.y);
                    //walkCloudPS.transform.localPosition = new Vector3(0f,-1.75f,0f);
                    //walkCloudPS.transform.localScale = new Vector3(walkCloudPS.transform.localScale.x*-1,walkCloudPS.transform.localScale.y,walkCloudPS.transform.localScale.z);
                }
                if (anim.CurrentClip.name != "ani_jimWalk" && !clipOverride) {
                    if (!GlobalVariableManager.Instance.CARRYING_SOMETHING) {
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk", false);
                    }
                    else {
                        if (carryingAbove)
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove", false);
                        else
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryWalk", false);
                    }
                }
                directionFacing = 2;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT)) {
                if (directionFacing != 1) {
                    gameObject.transform.localScale = new Vector2(transformScale.x, transformScale.y);
                    //walkCloudPS.transform.localPosition = new Vector3(0,-1.75f,0f);
                    //walkCloudPS.transform.localScale = new Vector3(Mathf.Abs(walkCloudPS.transform.localScale.x),walkCloudPS.transform.localScale.y,walkCloudPS.transform.localScale.z);
                }
                if (anim.CurrentClip.name != "ani_jimWalk" && !clipOverride) {

                    if (!GlobalVariableManager.Instance.CARRYING_SOMETHING) {
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk", false);
                    }
                    else {
                        if (carryingAbove)
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove", false);
                        else
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryWalk", false);
                    }

                }
                directionFacing = 1;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEUP)) {
                //legAnim.Play("walk");
                if (anim.CurrentClip.name != "ani_jimWalk" && !clipOverride) {
                    if (!GlobalVariableManager.Instance.CARRYING_SOMETHING) {
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk", false);
                    }
                    else {
                        if (carryingAbove)
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove", false);
                        else
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryWalk", false);
                    }
                }
                //walkCloudPS.transform.localScale = new Vector3(2.42f,2.42f,2.42f); 
                //walkCloudPS.transform.localPosition = new Vector3(2.5f,-3.3f,0f);
                directionFacing = 3;
            }
            else if (ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVEDOWN)) {
                if (anim.CurrentClip.name != "ani_jimWalk" && !clipOverride) {
                    if (!GlobalVariableManager.Instance.CARRYING_SOMETHING) {
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk", false);
                    }
                    else {
                        if (carryingAbove)
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAbove", false);
                        else
                            gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryWalk", false);
                    }
                }
                //walkCloudPS.transform.localScale = new Vector3(2.42f,2.42f,2.42f); 
                //walkCloudPS.transform.localPosition = new Vector3(2.5f,1.2f,0f);
                directionFacing = 4;
            }

            //Diagonals

            if (inputX != 0 && inputY != 0) {
                if (momentum != 4) {
                    momentum = 4f;
                    InvokeRepeating("SpawnClouds", .2f, .2f); //just have this here so it only happens once
                }
                legAnim.Play("walk");

                isDiagonal = true;
                if (movement.y == 1 && movement.x == -1) {
                    if (directionFacing == 1) {
                        /*gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimTurn",false);
                        StartCoroutine ("TurnDelay");*/
                    }
                    else {
                        //if(setAniOnce == 0){
                        //setAniOnce = 1;
                        //gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk",false);
                        //}
                    }
                }

                if (movement.y == 1 && movement.x == 1) {
                    /*if(directionFacing == 2){
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimTurn",false);
                        StartCoroutine ("TurnDelay");
                     }else{
                        /*if(setAniOnce == 0){
                        setAniOnce = 1;
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk",false);
                        }*/
                    //}
                }

                if (movement.y == -1 && movement.x == -1) {
                    /*if(setAniOnce == 0){
                        setAniOnce = 1;
                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalkUp",false);
                    }*/
                }

                if (movement.y == -1 && movement.x == 1) {

                    //gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalkDown",false);
                }
            }
            else {
                if (isDiagonal && !noDelayStarted) {
                    StartCoroutine(NoMoreDiagonal());
                    noDelayStarted = true;
                }
                else if (anim != null) {
                    /*
                    //left/right/up/down
                    if (movement.x == -1) {
                    /*if(directionFacing == 1){
                        gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimTurn",false);
                        StartCoroutine (TurnDelay());

                     }else{
                        //gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk",false);
                     }*/
                    //   }
                    /*
                                   if (movement.x == 1) {
                                   if(directionFacing == 1){
                                       gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimTurn",false);
                                       StartCoroutine ("TurnDelay");

                                    }else{
                                       gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk",false);
                                    }
                                   }


                                   if (movement.y == 1) {
                                       gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimWalk",false);
                                   }


                                   if (movement.y == -1) {
                                       anim.Play("ani_jimWalkDown");
                                   }
                                   */
                    if (movement.x == 0 && movement.y == 0) {
                        walkCloudPS.GetComponent<ParticleSystem>().Stop();
                        //legAnim.Play("stop");

                        if (!clipOverride) {
                            if (!GlobalVariableManager.Instance.CARRYING_SOMETHING) {
                                if (anim.CurrentClip.name == "ani_jimWalk") {
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle", false);
                                }
                                else if (anim.CurrentClip.name == "ani_jimWalkDown") {
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle", false);
                                }
                                else if (anim.CurrentClip.name == "ani_jimWalkUp") {
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle", false);
                                }
                                else if (anim.CurrentClip.name != "ani_jimIdle") {
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle", false);//set to normal idle when return from override clip(hurt, pickup,etc)
                                }
                            }
                            else {
                                Debug.Log("carrying something and carrying above = " + carryingAbove);
                                if (carryingAbove) {
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAboveIdle", false);
                                    //clipOverride = true;

                                }
                                else
                                    gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryIdle", false);
                            }
                        }

                        //not instant stop
                        if (momentum > 0)
                            momentum = momentum - .2f;
                    }
                    else if (momentum != 4) {
                        momentum = 4f;
                        InvokeRepeating("SpawnClouds", .2f, .2f);
                        walkCloudPS.SetActive(true); //just have this here so it only happens once
                        walkCloudPS.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
            if (movement.x == 0 && movement.y == 0) {

                CancelInvoke("SpawnClouds");




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
            if (GlobalVariableManager.Instance.PLAYER_CAN_MOVE) {

                transform.Translate(movement * speed * Time.deltaTime);
                //show legs and change to current animation of Jim
                if (!clipOverride && myLegs.activeInHierarchy) {

                    legAnim.Play(anim.CurrentClip.name);
                    legAnim.PlayFromFrame(anim.CurrentFrame);
                }


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
			if(!GlobalVariableManager.Instance.CARRYING_SOMETHING){
						
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimIdle",false);//set to normal idle when return from override clip(hurt, pickup,etc)
							
			}else{
				if(carryingAbove){
				gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryAboveIdle",false);
				}
				else
					gameObject.GetComponent<JimAnimationManager>().PlayAnimation("ani_jimCarryIdle",false);
			}
		StopAllCoroutines();
		legAnim.Play(anim.CurrentClip.name);
    	this.enabled = true;
    }
    void SpawnClouds(){
		/*GameObject newestCloud;
		newestCloud = Instantiate(walkCloud, new Vector3(transform.position.x,transform.position.y - 1.5f, transform.position.z), Quaternion.identity) as GameObject;
		if(movement.x <0){
			newestCloud.GetComponent<Ev_WalkCloud>().MoveRight();
		}else {
			newestCloud.GetComponent<Ev_WalkCloud>().MoveLeft();
		}*/
		SoundManager.instance.RandomizeSfx(footsteps2,footsteps1);

    }

    public void UpdateSpeed(float updatedSpeed){
    	speed += updatedSpeed;
    }

	IEnumerator TurnDelay() {
        yield return new WaitForSeconds (.1f);
		if(directionFacing == 1 || directionFacing == 2){
			anim.Play("ani_jimWalk");
			gameObject.transform.localScale = new Vector3(transformScale.x*-1,transformScale.y,transformScale.z);
				if(directionFacing == 1){
					directionFacing = 2;
				}else{
					directionFacing = 1;
				}
		}else if(directionFacing == 3){
			anim.Play("ani_jimWalk");
			directionFacing = 4;
        }else{
			anim.Play("ani_jimWalk");
			directionFacing = 3;
        }
	
    }
}
