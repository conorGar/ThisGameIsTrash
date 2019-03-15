using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FireTowardPlayerEnhanced))]
public class SlimeStateController : EnemyStateController
{
    // Use this for initialization
    protected new void Awake()
    {
        defaultState = new EnemyIdle();
        base.Awake();
    }

    protected override void AnyStateTrigger(EnemyTrigger trigger)
    {
        switch (trigger) {
            case EnemyTrigger.DEATH:
                currentState = new EnemyDead();
                break;
        }
    }


    protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
            case EnemyState.HIT:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                currentState = new EnemyIdle();
                break;
            case EnemyState.PREPARE:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                PlayerManager.Instance.Face(gameObject);
                currentState = new EnemyThrow();
                break;
            case EnemyState.THROW:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                PlayerManager.Instance.Face(gameObject);
                GetComponent<FireTowardPlayerEnhanced>().Fire();
                currentState = new EnemyIdle();
                break;
        }
    }
}