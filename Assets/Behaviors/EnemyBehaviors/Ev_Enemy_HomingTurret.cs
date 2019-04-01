using UnityEngine;
using System.Collections;

public class Ev_Enemy_HomingTurret : Ev_Enemy_Turret
{
	public int bulletBurstNum = 3;
	public float timeUntilStopFollow;
	public float rangeUntilFire = 15f;

	protected override void Update(){
		if(Vector2.Distance( PlayerManager.Instance.player.transform.position, gameObject.transform.position) < rangeUntilFire){
			base.Update();
		}
	}

	public override void Fire(){
		StartCoroutine("FireSequence");
	}

	IEnumerator FireSequence(){
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("throw");
		yield return new WaitForSeconds(.3f);
		for(int i = 0; i < bulletBurstNum; i++){
			GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag,gameObject.transform.position);
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().continuous = true;
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().speed = projectileSpeedX;
			/*if(bullet.GetComponent<Ev_ProjectileTowrdPlayer>() != null){
				bullet.GetComponent<Ev_ProjectileBasic>().speedX = projectileSpeedX;
				bullet.GetComponent<Ev_ProjectileBasic>().speedY = projectileSpeedY;
			}*/
			if(bullet.GetComponent<KillSelfAfterTime>() != null){
				bullet.GetComponent<KillSelfAfterTime>().timeUntilDeath = timeUntilProjDeath;
				bullet.GetComponent<KillSelfAfterTime>().StartCoroutine("Kill");
			}
			bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
			SoundManager.instance.PlaySingle(throwSFX);
			bullet.GetComponent<Ev_ProjectileTowrdPlayer>().StartCoroutine("StopFollowAfterTime",timeUntilStopFollow);
			yield return new WaitForSeconds(.2f);

		}
		gameObject.GetComponent<tk2dSpriteAnimator>().Play("idle");

		throwOnceCheck = 0;
		nextThrowTime = Time.time + fireRate;
	}
}

