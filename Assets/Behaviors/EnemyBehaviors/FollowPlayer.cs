using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public Transform player;
	public float walkDistance = 10.0f;
	public float chaseSpeed = 10.0f;
	public bool hasSeperateFacingAnimations;
	public bool iBeLerpin;
	public ParticleSystem chasePS;
	public bool returnToPreviousWhenFar;
	private Vector3 smoothVelocity = Vector3.zero;
	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void OnEnable(){
		player = GameObject.FindGameObjectWithTag("Player").transform; //needs to be in enable because of Dirty Decoy

		if(chasePS != null)
			chasePS.Play();
	}
	// Update is called once per frame
	protected void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < walkDistance) { //TODO: 
                if (iBeLerpin) {
                    transform.position = Vector3.SmoothDamp(transform.position, player.position, ref smoothVelocity, chaseSpeed);
                }
                else {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                }
                if (!hasSeperateFacingAnimations) {
                    if ( anim != null && anim.GetClipByName("chase") != null && !anim.IsPlaying("chase"))
                        anim.Play("chase");

                    if (player.transform.position.x < transform.position.x) {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else {
                        //if(!anim.IsPlaying("chaseL"))
                        //anim.Play("chaseL");
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                }//otherwise for now those specific actors handle it(Questio)
            }else if(returnToPreviousWhenFar){
				//gameObject.GetComponent<FollowPlayerAfterNotice>().enabled = true;
				gameObject.GetComponent<FollowPlayerAfterNotice>().LostSightOfPlayer();
				if(this.gameObject.GetComponent<WanderWithinBounds>() != null){
					//this.gameObject.GetComponent<WanderWithinBounds>().enabled = true;
					this.gameObject.GetComponent<WanderWithinBounds>().ReturnToStart();
					//this.gameObject.GetComponent<WanderWithinBounds>().SetNewBounds(); 
				}
				else if(this.gameObject.GetComponent<RandomDirectionMovement>() != null){
					this.gameObject.GetComponent<RandomDirectionMovement>().GoAgain();
				}else if(this.gameObject.GetComponent<WanderOnPath>() != null){
						this.gameObject.GetComponent<WanderOnPath>().ReturnToStart();
				}


				GameObject confused = ObjectPool.Instance.GetPooledObject("effect_confused");
				confused.transform.parent = this.transform;
				this.enabled = false;
            }
        }
	}

	public void StopSound(){

	}
}
