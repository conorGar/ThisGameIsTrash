using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Ev_Ex : MonoBehaviour {

	

	public GameObject myProjectile;
	public GameObject myParticles;
	public GameObject player;
	public AudioClip cast;
	public AudioClip teleport;
	public List<MonoBehaviour> dazeDisables = new List<MonoBehaviour>();

	Vector3 playerPosition;

	Color myColor;//needed for fade in/fadeOut
	tk2dSpriteAnimator myAnim;
    bool isActing = true;
    string action = "Teleport";

	// Use this for initialization
	void Start () {
		myColor = gameObject.GetComponent<tk2dSprite>().color;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        myAnim.AnimationEventTriggered = AnimationEventCallback;
    }

    // Update is called once per frame
    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (!isActing) {
                StartCoroutine(action);
                isActing = true;
            }
        }
	}

	//for debug
	void OnEnable(){
        action = "Teleport";
        isActing = false;
    }

	IEnumerator Teleport(){
		Debug.Log("Teleport Activated ----------- !");
		SoundManager.instance.PlaySingle(teleport);
		this.gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f,.3f);
		this.gameObject.GetComponent<SpecialEffectsBehavior>().FadeOut();
		myParticles.SetActive(true);
		yield return new WaitForSeconds(.3f);
		//TODO: when finalize room art, ex cannot land on one of the acid piles in the room
		gameObject.transform.localPosition = new Vector2(Random.Range(-36f,17f),Random.Range(-14f,10f));
		gameObject.GetComponent<tk2dSprite>().color = new Color(myColor.r,myColor.g,myColor.b,1); //fade back
		myParticles.SetActive(false);
		yield return new WaitForSeconds(Random.Range(1f,3f));
        action = "Fire";
        isActing = false;
    }

    IEnumerator Fire()
    {
        Debug.Log("FIRE ACTIVATED-----------!");
        if (myAnim.CurrentClip.name != "hurt") {
            myAnim.Play("cast"); // Triggers CAST_FINISHED on the last frame to go back to idle.
            StartCoroutine(FireballControl());
        }
        else {
            yield return PrepareNextAction();
        }
    }

    IEnumerator FireballControl()
    {
        // Spawn and home in on the player
        myProjectile.transform.position = gameObject.transform.position;
        myProjectile.GetComponent<KillSelfAfterTime>().CancelInvoke();//prevents projectile from dying shortly after spawn
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        myProjectile.SetActive(true);
        SoundManager.instance.PlaySingle(cast);
        Vector2 moveDirection = (playerPosition - myProjectile.transform.position).normalized * 10;
        myProjectile.GetComponent<FollowPlayer>().enabled = true;
        myProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y);
        yield return new WaitForSeconds(2f);

        // Hovering in place
        myProjectile.GetComponent<FollowPlayer>().enabled = false;
        yield return new WaitForSeconds(3f);

        // Destroy Fireball
        if (myProjectile.activeInHierarchy == true) {
            myProjectile.SetActive(false);
            myProjectile.transform.localPosition = Vector3.zero;
        }

        yield return PrepareNextAction();
    }

    // Helpers
    IEnumerator PrepareNextAction()
    {
        // Figure out the next action
        int randomNextAction = Random.Range(0, 3);
        if (randomNextAction == 0) {
            action = "Teleport";
        }
        else {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            action = "Fire";
        }
        isActing = false;
    }

	void Dazed(){
		//gameObject.GetComponent<EnemyTakeDamage>().StopAllCoroutines();//so follow player isn't enabled again
		for(int i = 0; i < dazeDisables.Count; i++){
			dazeDisables[i].enabled = false;
		}
		gameObject.layer = 11;
		gameObject.GetComponent<ThrowableObject>().enabled = true;
		myAnim.Play("dazed");
		StopAllCoroutines();
		//this.enabled = false;
	}

    // Callbacks
    void AnimationEventCallback(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
    {
        var frame = clip.GetFrame(frameNo);
        Debug.Log("Animation Trigger Check: " + frame.eventInfo);
        switch (frame.eventInfo) {
            case "CAST_FINISHED":
                myAnim.Play("idle");
                break;
            default:
                Debug.Log("Animation Trigger Not Found: " + frame.eventInfo);
                break;
        }
    }
}
