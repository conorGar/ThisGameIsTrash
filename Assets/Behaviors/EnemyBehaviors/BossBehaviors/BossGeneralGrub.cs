using UnityEngine;
using System.Collections;

public class BossGeneralGrub : MonoBehaviour
{
	tk2dSpriteAnimator myAnim;
	public GameObject myBoulder;
	public float projectileSpeed;
	public float fireRate;
	public AudioClip throwSFX;
	public AudioClip buildupSfx;
	public float nextFireTime = 0f;


	// Use this for initialization
	void OnEnable(){
		nextFireTime = fireRate + Time.time;
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();

	}

	
	// Update is called once per frame
	void Update ()
	{
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
                if (nextFireTime < Time.time) {
                    if (buildupSfx != null) {
                        SoundManager.instance.PlaySingle(buildupSfx);
                    }
					LaunchRocket();

                }
            }
        
	}



	void LaunchRocket(){
			Debug.Log("TossedRock");
	        myAnim.Play("throw");

	        myBoulder = ObjectPool.Instance.GetPooledObject("projectile_boulder", gameObject.transform.position,true);
	        myAnim.Play("idle");
			nextFireTime = fireRate + Time.time;

		
	}
}

