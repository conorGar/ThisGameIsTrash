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
	private Vector3 smoothVelocity = Vector3.zero;
	private tk2dSpriteAnimator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<tk2dSpriteAnimator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void OnEnable(){
		chasePS.Play();
	}
	// Update is called once per frame
	void Update () {
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
                    if (anim.GetClipByName("chase") != null && !anim.IsPlaying("chase"))
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
            }
        }
	}

	public void StopSound(){

	}
}
