using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLunge : IActorState<EnemyState, EnemyTrigger>
{
    public EnemyState GetState()
    {
        return EnemyState.LUNGE;
    }

    public IActorState<EnemyState, EnemyTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<EnemyState, EnemyTrigger> SendTrigger(EnemyTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
        switch (trigger) {
            /*case EnemyTrigger.HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
                return new EnemyHit();*/
			case EnemyTrigger.POWER_HIT:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.HIT));
				Debug.Log("-x-x-x-x-x-x-x- Enemy Idle Hit trigger activate -x-x-x-x-x-");

                return new EnemyPowerHit();
            case EnemyTrigger.RECOVER:
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.WAKE)); // TODO: I just picked this one because I thought it would look nice.  Please change!
                return new EnemyRecover();
                break;
			case EnemyTrigger.VULNERABLE:
				Debug.Log("set from lunge to vulnerable");
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.VULNERABLE)); // TODO: I just picked this one because I thought it would look nice.  Please change!
                return new EnemyVulnerable();
                break;
			case EnemyTrigger.POPUP:
				Debug.Log("set from lunge to vulnerable");
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.POP_UP)); 
                return new EnemyPopout();
                break;
			case EnemyTrigger.THROW: //added because of Rhino Beetle's ability to throw while leaping
				Debug.Log("set from lunge to vulnerable");
                animator.Play(EnemyAnim.GetName(ENEMY_ANIM.THROW)); 
                return new EnemyThrow();
                break;
			
        }

        return null;
    }
}
