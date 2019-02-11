using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_SpecialItem : MonoBehaviour {

	// Use this for initialization
	public GameObject upgradeUnlockDisplay;
	public Sprite unlockSprite;
	public bool playerAutoMoveToward;
    public GlobalVariableManager.UPGRADES upgrade_type;

	Vector3 smoothVelocity = Vector3.zero;
	bool playerIsMovingToward;
	bool beingTossed;
	bool canPickUp;
    float tossTimePassed;
    public Vector2 TossVector;
    public float TossHeight;
    Vector3 startTossPosition;
	Vector3 endTossPosition;
	Rigidbody2D myBody;
    public ItemDropArea itemDropArea;

	void OnEnable() {
		if(playerAutoMoveToward)
			PlayerMoveToward();

		myBody = gameObject.GetComponent<Rigidbody2D>();
        canPickUp = false;
        beingTossed = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(playerIsMovingToward){
            PlayerManager.Instance.player.transform.position = Vector3.SmoothDamp(PlayerManager.Instance.player.transform.position,gameObject.transform.position, ref smoothVelocity, 4f);

				if(PlayerManager.Instance.player.transform.position.x < transform.position.x){
					transform.localScale = new Vector3(-1,1,1);
				} else{
					//if(!anim.IsPlaying("chaseL"))
						//anim.Play("chaseL");
					transform.localScale = new Vector3(1,1,1);
				}
		}
        else if (beingTossed) {
            tossTimePassed += Time.deltaTime;
            transform.position = MathTGIT.ParabolicArc(startTossPosition, endTossPosition, TossHeight, tossTimePassed);

            // If the item has landed (below the end toss y-value)
            if (transform.position.y < endTossPosition.y) {
                transform.position = endTossPosition;
                canPickUp = true;
                beingTossed = false;
            }
        }
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player" && canPickUp){
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
            PlayerManager.Instance.player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimPickUp");
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(transform.position.x, PlayerManager.Instance.player.transform.position.y,2f);
			PickUp();

		}
	}
	
	void PickUp(){
		//yield return new WaitForSeconds(2f);
		upgradeUnlockDisplay.SetActive(true);
        // Unlock the Special Item.
        GlobalVariableManager.Instance.UPGRADES_UNLOCKED |= upgrade_type;
		gameObject.SetActive(false);
	}

	void PlayerMoveToward(){
        PlayerManager.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,null);
        PlayerManager.Instance.player.GetComponent<tk2dSpriteAnimator>().Play("ani_jimWalk");
			if(PlayerManager.Instance.player.transform.position.x < transform.position.x){
                PlayerManager.Instance.player.transform.localScale = new Vector3(1,1,1);
			} else{
                PlayerManager.Instance.player.transform.localScale = new Vector3(-1,1,1);
			}
		playerIsMovingToward = true;

	}
	public void Toss()
	{
		transform.parent = null;

        // store the start and end points of the toss
        tossTimePassed = 0f;
        startTossPosition = transform.position;

        // Toss away from the player on the x-axis.
        endTossPosition = new Vector3(transform.position.x + TossVector.x * Mathf.Sign(transform.position.x - PlayerManager.Instance.player.transform.position.x), transform.position.y + TossVector.y, transform.position.z);

        if (itemDropArea != null)
            endTossPosition = itemDropArea.GetDropPosition(endTossPosition);

		beingTossed = true;
	}
}
