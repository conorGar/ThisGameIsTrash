using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HashStateController))]
public class B_Ev_Hash : MonoBehaviour {

	tk2dSpriteAnimator myAnim;
	public GameObject stuart;
	public GameObject stuartShield;
	public AudioClip teleportSound;
	public AudioClip shieldSound;
	public ParticleSystem smokePuff;

	float landY;
	Rigidbody2D myBody;
    HashStateController controller;
    float direction = 1f;
    public float recoverDazeTime = 10f;
    public float nextRecoverDazeTime = 0f;
    GameObject dazedStars;

    //Protects Stuart Until Hash is hit
    //^Then Hash runs away


    void Awake()
    {
        myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        controller = GetComponent<HashStateController>();
    }

    void OnEnable()
    {
        GetComponent<Renderer>().sortingLayerName = "Layer01";
        gameObject.layer = 9; //switch to enemy layer.
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false; // Animator won't allow translations if it's active.  It's the worst.
    }

    void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
                    controller.SendTrigger(EnemyTrigger.CAST_SHIELD); // merge hash with stuart (hops on top of his head shielding him)
                    break;
                case EnemyState.MERGED:
                    gameObject.transform.localPosition = new Vector2(0f, 3f); //place hash on top of stuart TODO: Why do these drift apart if you don't set the local position every frame?
                    break;
                case EnemyState.HIT:
                    if (gameObject.transform.position.y < landY) {
                        myBody.gravityScale = 0f;
                        myBody.velocity = new Vector2(0, 0f);
                        myBody.AddForce(new Vector2(4f * direction, 0f), ForceMode2D.Impulse);//slide
                        controller.SendTrigger(EnemyTrigger.DEATH); // triggers dazed when hash hits the ground.
                    }
                    break;
                case EnemyState.DAZED:
                    // Undaze and cast shield.
                    if (nextRecoverDazeTime < Time.time) {
                        UnDazed();
                        controller.SendTrigger(EnemyTrigger.IDLE);
                    }
                    break;
                case EnemyState.CARRIED:
                    StopDazedStars();
                    break;
            }
        }
	}

	public void Shield(){
        smokePuff.Play();
        gameObject.transform.parent = stuart.transform;
        gameObject.transform.localPosition = new Vector2(0f, 3f);//place hash on top of stuart
        SoundManager.instance.PlaySingle(shieldSound);
		stuartShield.SetActive(true);
        myBody.gravityScale = 0f;
        myBody.velocity = new Vector2(0, 0f);
        UnDazed();
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
        stuart.GetComponent<StuartStateController>().SendTrigger(EnemyTrigger.INVULNERABLE); // Make stuart invulnerable to damage
	}

	public void KnockOff(){
		stuartShield.SetActive(false);
		gameObject.transform.parent = null;
		landY = gameObject.transform.position.y - 4;
        direction = Mathf.Sign(transform.position.x - PlayerManager.Instance.player.transform.position.x); // face away from the player
		myBody.AddForce(new Vector2(7f * direction, 4f),ForceMode2D.Impulse); // slide
		myBody.gravityScale = 1;
		gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
        gameObject.layer = 11; //switch to item layer.
        controller.SendTrigger(EnemyTrigger.HIT); // hash gets "hit" off stuart and lands on the ground, dazed.
	}

	public void Dazed(){
        gameObject.layer = 11; //switch to item layer.
        nextRecoverDazeTime = recoverDazeTime + Time.time;
        StartDazedStars();
	}

    public void UnDazed()
    {
        gameObject.layer = 1; //switch to transparent layer.
        StopDazedStars();
    }

    void StartDazedStars()
    {
        if (dazedStars != null)
            ObjectPool.Instance.ReturnPooledObject(dazedStars);

        dazedStars = ObjectPool.Instance.GetPooledObject("effect_dazed", new Vector3(transform.position.x, transform.position.y + 2, 0));
        dazedStars.transform.parent = gameObject.transform;
    }

    void StopDazedStars()
    {
        if (dazedStars != null) {
            ObjectPool.Instance.ReturnPooledObject(dazedStars);
            dazedStars = null;
        }
    }
}
