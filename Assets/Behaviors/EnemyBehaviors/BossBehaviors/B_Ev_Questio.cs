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

	EnemyTakeDamage myETD;
	tk2dSpriteAnimator myAnim;
	FollowPlayer fp;
    bool IsFacingLeft = true;
	bool isSwinging;
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

        // Reset gloves
        dropItemOnce = 0;
        grabbyGloves.SetActive(false);
        grabbyGloves.transform.parent = transform;
        grabbyGloves.transform.position = transform.position;

        // Reset Questio
        UnDazed();
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
                if (distance < 5 && !isSwinging) {
                    Debug.Log("QUESTIO SWING ACTIVATE");
                    Swing();
                }
            }

            if (myETD.currentHp <= 12 && dropItemOnce == 0) {
                StopAllCoroutines();
                StartCoroutine(DropGloves());
                Dazed();
                dropItemOnce = 1;
            }

            if (gameObject.layer == 11 && grabbyGloves.activeInHierarchy == false && pickupableGlow.activeInHierarchy == false) {
                pickupableGlow.SetActive(true);
            }
        }
	}

    // Helpers
    void Swing()
    {
        fp.enabled = false;
        if (IsFacingLeft) {
            myAnim.Play("swingL");
        }
        else {
            myAnim.Play("swingR");
        }
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
                isSwinging = false;
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

                gameObject.GetComponent<Rigidbody2D>().velocity = (player.transform.position -gameObject.transform.position).normalized *15;
                break;
            case "SWING_MOMENTUM_FINISHED":
                if (clip.name == "swingL")
                    mySlashL.SetActive(true);
                else
                    mySlashR.SetActive(true);

                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                break;
            case "SWING_FINISHED":
                mySlashL.SetActive(false);
                mySlashR.SetActive(false);
                myAnim.Play("idleL");
                fp.enabled = true;
                break;
            default:
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
                break;
        }
    }
}