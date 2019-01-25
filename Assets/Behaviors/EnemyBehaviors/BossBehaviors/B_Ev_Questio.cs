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
	public bool isSwinging;
    public bool isLeaping;
    public bool isDazed;
    Vector2 targetPosition;
	int dropItemOnce;
	GameObject dazedStars;
	//bool firstKnockDown;



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
        myETD.currentHp = 4;

        isSwinging = false;
        isLeaping = false;
        isDazed = false;
    }

	void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (!isSwinging)
                UpdateFacing();

            if (fp.enabled == true && !isDazed) {
            	Debug.Log("anis should be working...");
                if (IsFacingLeft && myAnim.CurrentClip.name != "walkL") {
                    myAnim.Play("walkL");
                }
                else if (!IsFacingLeft && myAnim.CurrentClip.name != "walkR") {
                    myAnim.Play("walkR");
                }
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < 12 && !isSwinging) {
                    Debug.Log("QUESTIO SWING ACTIVATE");
                    Swing();
                }
            }

            if (myETD.currentHp < 1 && !isDazed) {
                StopAllCoroutines();
				if(dropItemOnce == 0){
                if (!GlobalVariableManager.Instance.IsUpgradeUnlocked(GlobalVariableManager.UPGRADES.GLOVES))
                    StartCoroutine(DropGloves());
					dropItemOnce = 1;
                }
                Dazed();
               
            }

            if (gameObject.layer == 11 && grabbyGloves.activeInHierarchy == false && pickupableGlow.activeInHierarchy == false) {
                pickupableGlow.SetActive(true);
            }

            if(isLeaping){
            	baseShadow.transform.position = Vector2.MoveTowards(baseShadow.transform.position, targetPosition,14*Time.deltaTime);
            	if(Vector2.Distance(baseShadow.transform.position, targetPosition) < 3){
            		if(gameObject.GetComponent<Rigidbody2D>().gravityScale != 3){
            			gameObject.GetComponent<Rigidbody2D>().gravityScale = 3; // questio falls back down
						SoundManager.instance.PlaySingle(swing);
            		}
					gameObject.transform.position = new Vector2(baseShadow.transform.position.x, gameObject.transform.position.y);

                    // LANDING
					if(gameObject.transform.position.y < baseShadow.transform.position.y+1f){
						Debug.Log("Landed from fall" + gameObject.transform.position.y + targetPosition.y);
						hitBox.enabled = true;
						if (myAnim.CurrentClip.name == "swingL"){
                    		mySlashL.SetActive(true);
                            mySlashL.GetComponent<Animator>().Play("slashAnimation", -1, 0f);
                            myAnim.Play("swingFinishL");
              		  	}else{
                  		    mySlashR.SetActive(true);
                            mySlashR.GetComponent<Animator>().Play("slashAnimation", -1, 0f);
                            myAnim.Play("swingFinishR");
               			 }
						gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
						SoundManager.instance.PlaySingle(swing);

                        isLeaping = false;
						SoundManager.instance.PlaySingle(land);
						ObjectPool.Instance.GetPooledObject("effect_enemyLand", new Vector2(gameObject.transform.position.x, gameObject.transform.position.y-2f));
						gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
						gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
						baseShadow.transform.parent = gameObject.transform;
					}

            	}else{
					gameObject.transform.position = new Vector2(baseShadow.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, baseShadow.transform.position.y+3f, 9*Time.deltaTime));

            	}
            }
        }
	}

    // Helpers
    void Swing()
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
    }

    void Leap()
    {
        SoundManager.instance.PlaySingle(leap);
        targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        hitBox.enabled = false;
        isLeaping = true;
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
    }

    void SwingFinished()
    {
        gameObject.GetComponent<FollowPlayer>().enabled = true;
        gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = true;
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
		Debug.Log("Questio Dazed Activated -x-x-x-x-x--x-x-x-x-x-");
		for(int i = 0; i < dazeDisables.Count; i++){
			dazeDisables[i].enabled = false;
		}
		dazedShadow.SetActive(true);
		baseShadow.SetActive(false);
		gameObject.layer = 11;
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		gameObject.GetComponent<FollowPlayer>().chasePS.Stop();
		isDazed = true;
		myAnim.Play("dazed");
		gameObject.GetComponent<FollowPlayer>().enabled = false;
		dazedStars = ObjectPool.Instance.GetPooledObject("effect_stars",new Vector3(transform.position.x,transform.position.y+2,0));
		dazedStars.transform.parent = gameObject.transform;

		//Invoke("UnDazed",5f);
		//this.enabled = false;
	}

    void UnDazed()
    { //activated by BossStuart after Q hits stuart
        for (int i = 0; i < dazeDisables.Count; i++) {
            dazeDisables[i].enabled = true;
        }
        myETD.currentHp = 3;
        dazedShadow.SetActive(false);
        baseShadow.SetActive(true);
        isDazed = false;
        gameObject.layer = 9;
        gameObject.GetComponent<ThrowableObject>().enabled = false;
        myAnim.Play("idle");
        ObjectPool.Instance.ReturnPooledObject(dazedStars);
    }


    // Callbacks
    void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
        var frame = clip.GetFrame(frameNo);
#if DEBUG_ANIMATION
        Debug.Log("Animation Trigger Check: " + frame.eventInfo + " Clip Name: " + clip.name + " Frame No: " + frameNo);
#endif
        switch (frame.eventInfo) {
            case "SWING_SET":
                isSwinging = true;
                break;
            case "SWING_UNSET":
                isSwinging = false;
                mySlashL.SetActive(false);
                mySlashR.SetActive(false);
                break;
            case "LEAP":
                // At the start of the leap, Allow Questio to switch animations (mid frame) if the player got to his other side.
                UpdateFacing();
                if (IsFacingLeft) {
                    if (clip.name != "swingL")
                        myAnim.PlayFromFrame("swingL", frameNo + 1);
                } else {
                    if (clip.name != "swingR")
                        myAnim.PlayFromFrame("swingR", frameNo + 1);
                }

                Leap();
                break;
            case "SWING_FINISHED":
                mySlashL.SetActive(false);
                mySlashR.SetActive(false);
               	if(!isDazed){
	                myAnim.Play("idle");
	                fp.enabled = true;
                }
                SwingFinished();
                UpdateFacing();
                break;
            default:
#if DEBUG_ANIMATION
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
#endif
                break;
        }
    }
}