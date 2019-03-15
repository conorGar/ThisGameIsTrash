using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStateController))]
public class FireTowardPlayerEnhanced : MonoBehaviour
{
	public float projectileSpeed;
	public float fireRate;
	public bool myProjectileFalls = false;
	public GameObject projectile;
	public AudioClip throwSFX;
	public AudioClip buildupSfx;
	public float nextFireTime = 0f;
	tk2dSpriteAnimator myAnim;
	protected EnemyStateController controller;

	// Use this for initialization

	void Awake()
    {
        controller = GetComponent<EnemyStateController>();
    }

	void Start ()
	{
		myAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
	}

    void Update()
    {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.GetCurrentState() == EnemyState.IDLE) {
                if (nextFireTime < Time.time) {
                    controller.SendTrigger(EnemyTrigger.PREPARE);
                    if (buildupSfx != null) {
                        SoundManager.instance.PlaySingle(buildupSfx);
                    }
                }
            }
        }
	}

	void OnEnable(){
        nextFireTime = fireRate + Time.time;
	}

    public void Fire() {
        SoundManager.instance.PlaySingle(throwSFX);
        GameObject bullet = ObjectPool.Instance.GetPooledObject(projectile.tag, gameObject.transform.position);
        Vector2 movementDir = (PlayerManager.Instance.player.transform.position - gameObject.transform.position).normalized * 5;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(movementDir.x, movementDir.y);
        nextFireTime = fireRate + Time.time;
    }
}

