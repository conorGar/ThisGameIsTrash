using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_TrashCan : MonoBehaviour {

	int hp = 3;
	tk2dSpriteAnimator myAnim;
	tk2dSprite startSprite;


	public AudioClip hit;
	public AudioClip finalHit;
	public GameObject pinUnlockHud;
	public GameObject objectPool;
	public AudioClip hitSound;
	public PinDefinition myPin;
	public Sprite pinSprite;//given to dropped pin, which gives it to pin unlock display
	public ParticleSystem smokePuff;
	GameObject spawnedPin;
	int spawnOnce = 0;

	void Start () {
        myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
        if (GlobalVariableManager.Instance.IsPinDiscovered(myPin.Type)){
			myAnim.Play("fall");
			this.enabled = false;
		}
		//startSprite = gameObject.GetComponent<tk2dSprite>().CurrentSprite;
	}
	
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision){
		Debug.Log("Trash can hit collision detected");
		if(collision.gameObject.tag == "Weapon"){
			Debug.Log("Trash can WEAPON collision detected");
			hp--;
			SoundManager.instance.RandomizeSfx(hitSound);
			if(hp>0){
				myAnim.Play("hit");
				SoundManager.instance.PlaySingle(hitSound);
				GameObject damageCounter = objectPool.GetComponent<ObjectPool>().GetPooledObject("HitStars");
				damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(1);
				damageCounter.SetActive(true);
				damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);

				GameObject littleStars = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_LittleStars");
				damageCounter.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
				littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
				littleStars.SetActive(true);
			}else if(spawnOnce == 0){
				smokePuff.gameObject.SetActive(true);
				smokePuff.Play();
				SoundManager.instance.PlaySingle(finalHit);
				myAnim.Play("fall");
				SoundManager.instance.PlaySingle(hitSound);
			}
			StartCoroutine("ContinueHit");


		}
	}

	IEnumerator ContinueHit(){
		if(hp > 0){
			yield return new WaitForSeconds(.2f);
			myAnim.Stop();
		}else if(spawnOnce == 0){
			spawnedPin = objectPool.GetComponent<ObjectPool>().GetPooledObject("DroppedPin",gameObject.transform.position);
			spawnedPin.name = myPin.name;
			spawnedPin.GetComponent<tk2dSprite>().SetSprite(myPin.sprite);
			spawnedPin.GetComponent<Ev_DroppedPin>().SetPinData(myPin,pinUnlockHud, pinSprite);
			spawnOnce = 1;
		}
	}
}
