using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericEnemyStateController = EnemyStateController<EnemyState, EnemyTrigger>;

[RequireComponent(typeof(GenericEnemyStateController))]
public class LungeAtPlayer : MonoBehaviour {

	public float speed = 15;
	public float distanceTillLunge = 8;
	public AudioClip  buildUpSound;
	public AudioClip lungeSfx;
	Vector2 movementDir;

    protected GenericEnemyStateController controller;

    void Awake()
    {
        controller = GetComponent<GenericEnemyStateController>();
    }

    void Update () {
        // able to lunge at a player if they are chasing them.
		if(controller.GetCurrentState() == EnemyState.CHASE &&
           Vector2.Distance(PlayerManager.Instance.player.transform.position,gameObject.transform.position) < distanceTillLunge){
            controller.RemoveFlag((int)EnemyFlag.CHASING);
            controller.SendTrigger(EnemyTrigger.PREPARE);
			StartCoroutine("Lunge");
		}
	}

	IEnumerator Lunge(){
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		SoundManager.instance.PlaySingle(buildUpSound);

        // wait for prepare animation to end or something happens (like getting hit while preparing).
        while (controller.GetCurrentState() == EnemyState.PREPARE)
            yield return null;

        // the prepare animation wasn't interrupted.  We are able to lunge!
        if (controller.GetCurrentState() == EnemyState.LUNGE) {

            // LUNGING!
            SoundManager.instance.PlaySingle(lungeSfx);
            ObjectPool.Instance.GetPooledObject("effect_enemyLand", gameObject.transform.position); //dash clouds
            movementDir = (PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized * speed;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);
            yield return new WaitForSeconds(1f);

            // RECOVERING!
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            controller.SendTrigger(EnemyTrigger.RECOVER);
            yield return new WaitForSeconds(.5f);

            // BACK TO IDLE!
            controller.SendTrigger(EnemyTrigger.NOTICE);
        }

	}
}
