using UnityEngine;
using System.Collections;

public class GeneralGrubTankStateController : EnemyStateController
{

	//moving flag - bounce around screen
	//PREPARE - charge up shot
	//THROW - fire projectiles in four directions

	protected new void Awake()
    {
        defaultState = new EnemyIdle();
        base.Awake();
    }

	protected override void AnyStateTrigger(EnemyTrigger trigger)
    {
        switch (trigger) {
            case EnemyTrigger.DEATH:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.DEAD));
                currentState = new EnemyDead();
                break;
        }
    }

	protected override void AnimationEventCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
    {
        base.AnimationEventCompleted(animator, clip);

        switch (currentState.GetState()) {
          
        }
    }
}