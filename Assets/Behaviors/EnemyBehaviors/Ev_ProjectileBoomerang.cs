using UnityEngine;
using System.Collections;

public class Ev_ProjectileBoomerang : MonoBehaviour
{
	public float speedx;

	[HideInInspector]
	public Ev_Enemy_BoomerangSlime myEnemy;
	[HideInInspector]
	public Vector2 startingPlayerPos;

	public Vector2 direction = new Vector2();
//	void onEnable(){
//		direction = (PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized;
//	}
	
	// Update is called once per frame
	void Update ()
	{
		
	
	
	}

	public void Slowdown(){
	Debug.Log("Slowdown call" + speedx);
	 speedx -= 1f;
	 if(speedx < 0){
			direction = (startingPlayerPos - new Vector2(myEnemy.gameObject.transform.position.x,myEnemy.gameObject.transform.position.y)).normalized; //make sure the projectile moves back toward the enemy's CURRENT position, in case anything pushed it
	 }
	 gameObject.GetComponent<Rigidbody2D>().velocity  = direction * speedx;

	}
}

