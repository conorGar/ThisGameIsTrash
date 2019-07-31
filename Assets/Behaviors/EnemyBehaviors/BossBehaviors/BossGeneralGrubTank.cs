using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossGeneralGrubTank : MonoBehaviour
{
	int currentMark;
	Vector3 destinationPos;
	public List<GameObject> pathMarks = new List<GameObject>(); //public for debugging, otherwise should be given by enemy spawner
	Vector3 startingScale;
	protected tk2dSpriteAnimator anim;


	tk2dSpriteAnimator myAnim;
	public GameObject myBoulder;
	public float projectileSpeed;
	public float fireRate;
	public AudioClip throwSFX;
	public AudioClip buildupSfx;
	public float nextFireTime = 0f;

	void OnEnable(){
		nextFireTime = fireRate + Time.time;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}
	// Use this for initialization
	void Start ()
	{
		destinationPos = (pathMarks[0].transform.position);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Vector2.Distance(gameObject.transform.position, destinationPos) > 1) {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, destinationPos, 4 * Time.deltaTime);
        } else {
            NextMark();
        }


		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                if (nextFireTime < Time.time) {
                    if (buildupSfx != null) {
                        SoundManager.instance.PlaySingle(buildupSfx);
                    }
					LaunchRocket();

                }
            }
	}


	void NextMark(){
		
		if(currentMark < (pathMarks.Count-1)){
			currentMark++;
		}else{
			currentMark = 0;
		}


        // use the enemy path grid if they have one.  Tries to find a close position to the pathMark if it can.
      

            destinationPos = (pathMarks[currentMark].transform.position);

    }



	void LaunchRocket(){
			Debug.Log("TossedRock");
	        myAnim.Play("throw");

	        myBoulder = ObjectPool.Instance.GetPooledObject("projectile_boulder", gameObject.transform.position,true);
	        myAnim.Play("idle");
			nextFireTime = fireRate + Time.time;

		
	}
}

