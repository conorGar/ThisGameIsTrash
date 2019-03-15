using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestioStateController))]
[RequireComponent(typeof(ThrowableEnemy))]
[RequireComponent(typeof(EnemyPath))]
public class B_Ev_Questio : MonoBehaviour
{
    public GameObject mySlashR;
    public GameObject mySlashL;
    public GameObject grabbyGloves;

    public GameObject baseShadow;
    public GameObject dazedShadow;
    public GameObject pickupableGlow;
    bool IsPickedUpOnce = false; // Throwable body will glow before the first pick up.

    public AudioClip land;
    public AudioClip leap;
    public AudioClip swing;

    EnemyTakeDamage myETD;
    Vector2 targetPosition;
    int dropItemOnce;
    GameObject dazedStars;
    protected QuestioStateController controller;
    public float recoverDazeTime = 10f;
    public float nextRecoverDazeTime = 0f;

    private int transparentLayer;
    private int enemyLayer;

    void Awake()
    {
        myETD = gameObject.GetComponent<EnemyTakeDamage>();
        controller = GetComponent<QuestioStateController>();
        transparentLayer = LayerMask.NameToLayer("TransparentFX");
        enemyLayer = LayerMask.NameToLayer("enemies");

        
    }

    void OnEnable()
    {
        StopAllCoroutines();
        myETD.moveWhenHit = true;
        // Reset gloves
        dropItemOnce = 0;
        grabbyGloves.SetActive(false);
        grabbyGloves.transform.parent = transform;
        grabbyGloves.transform.position = transform.position;

        // Reset Questio
        UnDazed();
        baseShadow.transform.parent = gameObject.transform;
        baseShadow.transform.localPosition = new Vector2(-.38f, -1.27f);
        pickupableGlow.SetActive(false);
        myETD.currentHp = 4;
    }

    private void OnDisable()
    {
        GetComponent<Animator>().enabled = false; // Animator won't allow translations if it's active.  It's the worst.
    }

    void Update()
    {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            switch (controller.GetCurrentState()) {
                case EnemyState.IDLE:
                    controller.SendTrigger(EnemyTrigger.NOTICE); // Questio is always chasing the player.
                    break;
                case EnemyState.HIT:
                    if (mySlashL.activeInHierarchy) mySlashL.SetActive(false);
                    if (mySlashR.activeInHierarchy) mySlashR.SetActive(false);
                    break;
                case EnemyState.CHASE: // When Questio is near the player, start the leap and swing.
                    float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
                    if (distance < 12) {
                        PrepareLeap();
                    }
                    break;
                case EnemyState.DAZED:
                    if (mySlashL.activeInHierarchy) mySlashL.SetActive(false);
                    if (mySlashR.activeInHierarchy) mySlashR.SetActive(false);
                    if (dropItemOnce == 0) { // On the first dazed, drop the gloves.
                        if (!GlobalVariableManager.Instance.IsUpgradeUnlocked(GlobalVariableManager.UPGRADES.GLOVES))
                            StartCoroutine(DropGloves());
                        dropItemOnce = 1;
                    }

                    if (nextRecoverDazeTime < Time.time) {
                        UnDazed();
                        controller.SendTrigger(EnemyTrigger.IDLE);
                    }

                    break;
                case EnemyState.LUNGE: // leaping through the air
                    baseShadow.transform.position = Vector2.MoveTowards(baseShadow.transform.position, targetPosition, 14 * Time.deltaTime);
                    if (Vector2.Distance(baseShadow.transform.position, targetPosition) < 3) {
                        if (gameObject.GetComponent<Rigidbody2D>().gravityScale != 3) {
                            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3; // questio falls back down
                            SoundManager.instance.PlaySingle(swing);
                        }
                        gameObject.transform.position = new Vector2(baseShadow.transform.position.x, gameObject.transform.position.y);

                        // Landing recover
                        if (gameObject.transform.position.y < baseShadow.transform.position.y + 1f) {
                            Recover();
                        }
                    } else {
                        gameObject.transform.position = new Vector2(baseShadow.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, baseShadow.transform.position.y + 3f, 9 * Time.deltaTime));
                    }

                    break;
                case EnemyState.CARRIED:
                    // When Questio is picked up for the first time, disable the glow and never allow it to glow again in this game instance.
                    if (pickupableGlow.activeInHierarchy) {
                        pickupableGlow.SetActive(false);
                        IsPickedUpOnce = true;
                    }
                    StopDazedStars();
                    break;
            }

            // Glow when questio is throwable, the gloves have been picked up.
            if (!IsPickedUpOnce && gameObject.layer == 11 && grabbyGloves.activeInHierarchy == false && pickupableGlow.activeInHierarchy == false) {
                pickupableGlow.SetActive(true);
            }
        }
    }

    // Helpers
    void PrepareLeap()
    {
        myETD.moveWhenHit = false;
        baseShadow.transform.parent = null;

        GetComponent<EnemyPath>().ClearPath();
        controller.SendTrigger(EnemyTrigger.PREPARE_LEAP);
    }

    public void Leap()
    {
        SoundManager.instance.PlaySingle(leap);
        targetPosition = new Vector2(PlayerManager.Instance.player.transform.position.x, PlayerManager.Instance.player.transform.position.y);
        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer02";
        gameObject.layer = transparentLayer;
    }

    // Questio begins recovering from the leap, swinging his hook
    public void Recover()
    {
        Debug.Log("Landed from fall" + gameObject.transform.position.y + targetPosition.y);
        SoundManager.instance.PlaySingle(swing);
        var state = controller.GetCurrentState();
        if (state == EnemyState.LUNGE) {
            if (((EnemyLunge)controller.currentState).facingLeft) {
                mySlashL.SetActive(true);
                mySlashL.GetComponent<Animator>().Play("slashAnimation", -1, 0f);
            } else {
                mySlashR.SetActive(true);
                mySlashR.GetComponent<Animator>().Play("slashAnimation", -1, 0f);
            }
        }

        gameObject.GetComponent<Renderer>().sortingLayerName = "Layer01";
        gameObject.layer = enemyLayer;

        SoundManager.instance.PlaySingle(land);
        ObjectPool.Instance.GetPooledObject("effect_enemyLand", new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 2f));
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        baseShadow.transform.parent = gameObject.transform;

        controller.SendTrigger(EnemyTrigger.RECOVER);
    }

    // Questio recovers completely, puts the hook away.
    public void Recovered()
    {
        gameObject.GetComponent<EnemyTakeDamage>().moveWhenHit = true;
    }

    IEnumerator DropGloves()
    {
        GameStateManager.Instance.PushState(typeof(MovieState));

        grabbyGloves.SetActive(true);
        grabbyGloves.GetComponent<Ev_SpecialItem>().Toss();
        CamManager.Instance.mainCamEffects.CameraPan(grabbyGloves, true);
        yield return new WaitForSeconds(2f);
        CamManager.Instance.mainCamEffects.ReturnFromCamEffect();

        GameStateManager.Instance.PopState();
    }

    public void Dazed()
    {
        gameObject.layer = 11;
        nextRecoverDazeTime = recoverDazeTime + Time.time;

        dazedShadow.SetActive(true);
        baseShadow.SetActive(false);

        StartDazedStars();
        gameObject.GetComponent<ThrowableEnemy>().StopSweat();
    }

    public void UnDazed()
    {
        myETD.currentHp = 3;
        dazedShadow.SetActive(false);
        baseShadow.SetActive(true);
        gameObject.layer = 9;
        pickupableGlow.SetActive(false);

        StopDazedStars();
        gameObject.GetComponent<ThrowableEnemy>().StopSweat();
    }

    void StartDazedStars()
    {
        if (dazedStars != null)
            ObjectPool.Instance.ReturnPooledObject(dazedStars);

        dazedStars = ObjectPool.Instance.GetPooledObject("effect_dazed", new Vector3(transform.position.x, transform.position.y + 2, 0));
        dazedStars.transform.parent = gameObject.transform;
    }

    void StopDazedStars()
    {
        if (dazedStars != null) {
            ObjectPool.Instance.ReturnPooledObject(dazedStars);
            dazedStars = null;
        }
    }
}