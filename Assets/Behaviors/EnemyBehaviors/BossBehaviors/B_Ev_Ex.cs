using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Ex : MonoBehaviour {

	

	public GameObject myProjectile;
	public GameObject myParticles;
	public GameObject player;

	Vector3 playerPosition;

	Color myColor;//needed for fade in/fadeOut
	tk2dSpriteAnimator myAnim;
	// Use this for initialization
	void Start () {
		myColor = gameObject.GetComponent<tk2dSprite>().color;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//for debug
	void OnEnable(){
		StartBattle();
	}


	public void StartBattle(){
		StartCoroutine("Teleport");
	}


	IEnumerator Teleport(){
		Debug.Log("Teleport Activated ----------- !");

		this.gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f,.3f);
		this.gameObject.GetComponent<SpecialEffectsBehavior>().FadeOut();
		myParticles.SetActive(true);
		yield return new WaitForSeconds(.3f);
		//TODO: when finalize room art, ex cannot land on one of the acid piles in the room
		gameObject.transform.localPosition = new Vector2(Random.Range(-36f,17f),Random.Range(-14f,10f));
		gameObject.GetComponent<tk2dSprite>().color = new Color(myColor.r,myColor.g,myColor.b,1); //fade back
		myParticles.SetActive(false);
		yield return new WaitForSeconds(Random.Range(1f,3f));
		StartCoroutine("Fire");
	}

	IEnumerator Fire(){
		Debug.Log("FIRE ACTIVATED-----------!");
		if(myAnim.CurrentClip.name != "hurt"){
			myAnim.Play("cast");
			yield return new WaitForSeconds(.3f);
			playerPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);

			myProjectile.SetActive(true);
			Vector2 moveDirection = (playerPosition - myProjectile.transform.position).normalized *20;
			myProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x,moveDirection.y);
			myAnim.Play("idle");
			yield return new WaitForSeconds(5f);
			if(myProjectile.activeInHierarchy == true){
			myProjectile.SetActive(false);
			myProjectile.transform.localPosition = Vector3.zero;
			}
			int randomNextAction = Random.Range(0,3);
			if(randomNextAction == 0){
				StartCoroutine("Teleport");
			}else{
				yield return new WaitForSeconds(Random.Range(1f,3f));
				StartCoroutine("Fire");
			}
		}else{
			int randomNextAction = Random.Range(0,3);
			if(randomNextAction == 0){
				StartCoroutine("Teleport");
			}else{
				yield return new WaitForSeconds(Random.Range(1f,3f));
				StartCoroutine("Fire");
			}
		}

	}

}
