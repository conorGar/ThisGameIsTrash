using UnityEngine;
using System.Collections;

public class FireTowardPlayerEnhanced : MonoBehaviour
{
	public float projectileSpeed;
	public float fireRate;
	public bool myProjectileFalls = false;
	public string fireAniName;
	public GameObject projectile;
	public AudioClip throwSFX;
	public AudioClip buildupSfx;
	bool firing;
	tk2dSpriteAnimator myAnim;
	// Use this for initialization
	void Start ()
	{
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}

	void Update(){

		if(firing){
			GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
			Vector2 movementDir = (PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized * 5;
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x,movementDir.y);
			StartCoroutine("Fire");
			firing = false;
		}
	}

	void OnEnable(){
		StartCoroutine("Fire");
	}


	IEnumerator Fire(){
		yield return new WaitForSeconds(fireRate);
		gameObject.GetComponent<RandomDirectionMovement>().StopMoving();
		myAnim.Play(fireAniName);
		if(buildupSfx != null){
			SoundManager.instance.PlaySingle(buildupSfx);
		}
		yield return new WaitForSeconds(.1f);
		yield return new WaitForSeconds(myAnim.ClipTimeSeconds +.1f);
		SoundManager.instance.PlaySingle(throwSFX);
		firing = true;
		yield return new WaitForSeconds(.4f);
		gameObject.GetComponent<RandomDirectionMovement>().StartMoving();
		myAnim.Play("idle");
	}
}

