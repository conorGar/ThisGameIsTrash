using UnityEngine;
using System.Collections;

public class ClankingMaterial : MonoBehaviour
{
	//Rigidbody2D player;

	public AudioClip clankSound;
	public EnemyTakeDamage parentsETD;
	public bool pushBack = true; // is this enemy pushed when clanked with( usually yes, not with enemies that have armor during a dash or jump or something, like RhinoBeetle)
	//public Rigidbody2D parentEnemyBody;

	// Use this for initialization
	void Start ()
	{
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		if(clankSound == null){
			clankSound = SoundManager.instance.GetSFX(SFXBANK.CLANK);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Weapon"){
			parentsETD.Clank(clankSound,gameObject.transform.position, pushBack);
		}else if (collider.tag == "BigSwoosh"){
			parentsETD.PowerHitEffect();
			parentsETD.StartCoroutine("NonMeleeHit",true);
		}else if (collider.tag == "pObj_bullet"){
			if(collider.GetComponent<Ev_ProjectileChargable>() != null){
				 if(collider.GetComponent<Ev_ProjectileChargable>().charged){
					parentsETD.PowerHitEffect();
					parentsETD.meleeSwingDirection = "plankSwing";
					parentsETD.meleeDmgBonus = 2;
					parentsETD.StartCoroutine("NonMeleeHit",true);
					}
				else{
						ObjectPool.Instance.ReturnPooledObject(collider.gameObject); //TODO: for now, if player projectile isn't charged, it's just destroyed
					}
			}
		}

	}


}

