using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStateController))]
public class InvincibleEnemy : MonoBehaviour
{

	public AudioClip nullHitSfx;


    public bool IsInvulnerable()
    {
        return GetComponent<EnemyStateController>().IsFlag((int)EnemyFlag.INVULNERABLE);
    }

    void OnTriggerEnter2D(Collider2D melee){
		if(IsInvulnerable() && melee.tag == "Weapon"){
			Deflect();
		}
	}

	void Deflect(){
		SoundManager.instance.PlaySingle(nullHitSfx);
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars");
		littleStars.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		littleStars.SetActive(true);

		if(gameObject.transform.position.x < PlayerManager.Instance.player.transform.position.x){
			PlayerManager.Instance.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(2,0),ForceMode2D.Impulse);
		}else{
            PlayerManager.Instance.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2,0),ForceMode2D.Impulse);	
		}
	}
}

