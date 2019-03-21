using UnityEngine;
using System.Collections;

public class Ev_Enemy_GrubGun : MonoBehaviour
{
	public float projectileSpeedX;
	public float projectileSpeedY;
	public float fireRate;
	public AudioClip throwSFX;
	public GameObject projectile;
	public Transform parentTransform;

	int throwOnceCheck;
	float nextThrowTime;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
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
			if(parentTransform.localScale.x > 0){
			bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeedX;
			}else{
				bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeedX * -1;
			}
			bullet.GetComponent<Ev_ProjectileBasic>().speedY = projectileSpeedY;
		}
		bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
		SoundManager.instance.PlaySingle(throwSFX);

		throwOnceCheck = 0;
		nextThrowTime = Time.time + fireRate;

	}
}

