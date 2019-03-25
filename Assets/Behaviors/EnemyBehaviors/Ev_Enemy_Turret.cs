using UnityEngine;
using System.Collections;

public class Ev_Enemy_Turret : MonoBehaviour
{
	public float projectileSpeedX;
	public float projectileSpeedY;
	public float fireRate;
	public AudioClip throwSFX;
	public GameObject projectile;
	public float timeUntilProjDeath = 7f;

	int throwOnceCheck;
	float nextThrowTime;



	void OnEnable ()
	{
		Fire();
	}
	
	protected virtual void Update () {
		if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
			//Debug.Log(controller.name);

			if(Time.time > nextThrowTime && throwOnceCheck == 0){
						//Debug.Log("Fire rate reached, throw time is now");
						throwOnceCheck = 1;
						Fire();

			}else{
				//Debug.Log(Time.time + "  " + nextThrowTime);
				//Debug.Log(throwOnceCheck);
			}
			
		}
	}

	public void Fire(){
		//yield return new WaitForSeconds(fireRate);
		//Debug.Log("Fire Enum Activated");

		GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);

		if(bullet.GetComponent<Ev_ProjectileBasic>() != null){
			bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeedX;
			bullet.GetComponent<Ev_ProjectileBasic>().speedY = projectileSpeedY;
		}
		if(bullet.GetComponent<KillSelfAfterTime>() != null){
			bullet.GetComponent<KillSelfAfterTime>().timeUntilDeath = timeUntilProjDeath;
			bullet.GetComponent<KillSelfAfterTime>().StartCoroutine("Kill");
		}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(throwSFX);

		throwOnceCheck = 0;
		nextThrowTime = Time.time + fireRate;


				
			
		

	}
}

