using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Questio : MonoBehaviour {

	public GameObject mySlashR;
	public GameObject mySlashL;
	public GameObject player;
	public GameObject grabbyGloves;
	public List<MonoBehaviour> dazeDisables = new List<MonoBehaviour>();

	public GameObject baseShadow;
	public GameObject dazedShadow;
	public GameObject pickupableGlow;

	public BoxCollider2D hitBox; // disabled during leap


	public AudioClip land;
	public AudioClip leap;
	public AudioClip swing;

	EnemyTakeDamage myETD;
	tk2dSpriteAnimator myAnim;
	FollowPlayer fp;
    bool IsFacingLeft = true;
	bool isSwinging;
	bool leaping;
	Vector2 targetPosition;
	int dropItemOnce;
	GameObject dazedStars;

	void Awake () {
		myETD = gameObject.GetComponent<EnemyTakeDamage>();
		fp = gameObject.GetComponent<FollowPlayer>();
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        myAnim.AnimationEventTriggered = AnimationEventCallback;
	}

	void OnEnable(){
        StopAllCoroutines();
        myETD.moveWhenHit = true;
        hitBox.enabled = true;
        // Reset gloves
        dropItemOnce = 0;
        grabbyGloves.SetActive(false);
        grabbyGloves.transform.parent = transform;
        grabbyGloves.transform.position = transform.position;

        // Reset Questio
        UnDazed();
        baseShadow.transform.parent = gameObject.transform;
        baseShadow.transform.localPosition = new Vector2(-.38f,-1.27f);
        fp.enabled = true; //when returning to room without this Q will just stand there
        pickupableGlow.SetActive(false);
        myETD.currentHp = 16; // TODO: why are we hardcoding this in a range from 16 to 12?  Doing this for now but this is weird and confusing.

    }

	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (!isSwinging)
                UpdateFacing();

            if (fp.enabled == true) {
                if (IsFacingLeft && myAnim.CurrentClip.name != "walkL") {
                    myAnim.Play("walkL");
                }
                else if (!IsFacingLeft && myAnim.CurrentClip.name != "walkR") {
                    myAnim.Play("walkR");
                }
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < 12 && !isSwinging) {
                    Debug.Log("QUESTIO SWING ACTIVATE");
                    StartCoroutine("Swing");
                }
            }

            if (myETD.currentHp <= 12 && dropItemOnce == 0) {
                StopAllCoroutines();
                if (!GlobalVariableManager.Instance.IsUpgradeUnlocked(GlobalVariableManager.UPGRADES.GLOVES))
                    StartCoroutine(DropGloves());
                Dazed();
                dropItemOnce = 1;
            }

            if (gameObject.layer == 11 && grabbyGloves.activeInHierarchy == false && pickupableGlow.activeInHierarchy == false) {
                pickupableGlow.SetActive(true);
            }

            if(leaping){
            	baseShadow.transform.position = Vector2.MoveTowards(baseShadow.transform.position, targetPosition,14*Time.deltaTime);
            	if(Vector2.Distance(baseShadow.transform.position, targetPosition) < 3){
            		if(gameObject.GetComponent<Rigidbody2D>().gravityScale != 3){
            			gameObject.GetComponent<Rigidbody2D>().gravityScale = 3; // questio falls back down
						SoundManager.instance.PlaySingle(swing);
						myAnim.SetFrame(43); //set to actual swing frame
            		}
					gameObject.transform.position = new Vector2(baseShadow.transform.position.x, gameObject.transform.position.y);

					if(gameObject.transform.position.y < baseShadow.transform.position.y+1f){
						Debug.Log("Landed from fall" + gameObject.transform.position.y + targetPosition.y);
						hitBox.enabled = true;
						gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
						SoundManager.instance.PlaySingle(swing);
						leaping = false;
						SoundManager.instance.PlaySingle(land);
						ObjectPool.Instance.GetPooledObject("effect_enemyLand", new Vector2(gameObject.transform.position.x, gameObject.transform.position.y-2f));
						gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
						gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
						baseShadow.transform.parent = gameObject.transform;
						//baseShadow.transform.localPosition = new Vector2(-.38f,-1.27f);
						StartCoroutine("LandDelay");
					}

            	}else{
					gameObject.transform.position = new Vector2(baseShadow.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, baseShadow.transform.position.y+3f, 9*Time.deltaTime));

            	}
            }
        }
	}

    // Helpers
    IEnumerator Swing()
    {
    	myETD.moveWhenHit = false;
    	baseShadow.transform.parent = null;
        fp.enabled = false;
        if (IsFacingLeft) {
            myAnim.Play("swingL");
        }
        else {
            myAnim.Play("swingR");
        }
        yield return new WaitForSeconds(.3f);
        SoundManager.instance.PlaySingle(leap);
        targetPosition = player.transform.position;
        hitBox.enabled = false;
		leaping = true;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
    }


    IEnumerator LandDelay(){
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<FollowPlayer>().enabled = true;
		gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = true;

		yield return new WaitForSeconds(1f);
		isSwinging = false;
	
    }


    void UpdateFacing()
    {
        IsFacingLeft = player.transform.position.x < gameObject.transform.position.x;
    }

    IEnumerator DropGloves()
    {
        GameStateManager.Instance.PushState(typeof(MovieState));

        grabbyGloves.SetActive(true);
        grabbyGloves.GetComponent<Ev_SpecialItem>().Toss();
        CamManager.Instance.mainCamEffects.CameraPan(grabbyGloves, true);
        yield return new WaitForSeconds(2f);
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();

        GameStateManager.Instance.PopState();
    }

	void Dazed(){
		//gameObject.GetComponent<EnemyTakeDamage>().StopAllCoroutines();//so follow player isn't enabled again
		for(int i = 0; i < dazeDisables.Count; i++){
			dazeDisables[i].enabled = false;
		}
		dazedShadow.SetActive(true);
		baseShadow.SetActive(false);
		gameObject.layer = 11;
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		gameObject.GetComponent<FollowPlayer>().chasePS.Stop();
		myAnim.Play("dazed");
		dazedStars = ObjectPool.Instance.GetPooledObject("effect_stars",new Vector3(transform.position.x,transform.position.y+2,0));
		dazedStars.transform.parent = gameObject.transform;
		//this.enabled = false;
	}

    void UnDazed()
    {
        for (int i = 0; i < dazeDisables.Count; i++) {
            dazeDisables[i].enabled = true;
        }

        dazedShadow.SetActive(false);
        baseShadow.SetActive(true);
        gameObject.layer = 9;
        gameObject.GetComponent<ThrowableObject>().enabled = false;
        myAnim.Play("idleL");
        ObjectPool.Instance.ReturnPooledObject(dazedStars);
    }


    // Callbacks
    void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
        var frame = clip.GetFrame(frameNo);
        Debug.Log("Animation Trigger Check: " + frame.eventInfo);
        switch (frame.eventInfo) {
            case "SWING_SET":
                isSwinging = true;
                break;
            case "SWING_UNSET":
                //isSwinging = false;
                mySlashL.SetActive(false);
                mySlashR.SetActive(false);
                break;
            case "SWING_CHARGE":
                // At the start of the charge, Allow Questio to switch animations (mid frame) if the player got to his other side.
                UpdateFacing();
                if (IsFacingLeft) {
                    if (clip.name != "swingL") {
                        myAnim.PlayFromFrame("swingL", frameNo);
                    }
                } else {
                    if (clip.name != "swingR") {
                        myAnim.PlayFromFrame("swingR", frameNo);
                    }
                }
               

                //gameObject.GetComponent<Rigidbody2D>().velocity = (player.transform.position -gameObject.transform.position).normalized *15;
                break;
            case "SWING_MOMENTUM_FINISHED":
                if (clip.name == "swingL"){
                    mySlashL.SetActive(true);
                    mySlashL.GetComponent<Animator>().Play("slashAnimation",-1,0f);
                }else{
                    mySlashR.SetActive(true);
					mySlashR.GetComponent<Animator>().Play("slashAnimation",-1,0f);

                }

                //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                break;
            case "SWING_FINISHED":
                mySlashL.SetActive(false);
                mySlashR.SetActive(false);
                myAnim.Play("idle");
                fp.enabled = true;
                break;
            default:
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
                break;
        }
    }
}