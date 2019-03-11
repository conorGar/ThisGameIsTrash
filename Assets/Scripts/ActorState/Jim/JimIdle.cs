using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimIdle : IActorState<JimState, JimTrigger>
{

	int whichSwingAni;
    public JimState GetState()
    {
        return JimState.IDLE;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        if ((flags & (int)JimFlag.MOVING) == (int)JimFlag.MOVING) {
            animator.Play("ani_jimWalk");
        } else {
            animator.Play("ani_jimIdle");
        }

        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags )
    {
        switch (trigger) {
            case JimTrigger.HIT:
                animator.Play("hurt");
                return new JimHurt();
			case JimTrigger.DASH:
                return new JimDash();
            case JimTrigger.SWING_RIGHT:
            	Debug.Log("which swing ani:" + whichSwingAni);

                flags &= ~(int)JimFlag.FACING_LEFT;
                return new JimAttacking();

            case JimTrigger.SWING_LEFT:
                //animator.Play("ani_jimSwingR");
				if(whichSwingAni == 0){
                	animator.Play("ani_jimSwingR2");
                	whichSwingAni = 1;
                }else{
					animator.Play("ani_jimSwingR3");
                	whichSwingAni = 0;
                }
                flags |= (int)JimFlag.FACING_LEFT;
                return new JimAttacking();

            case JimTrigger.SWING_UP:
                animator.Play("ani_jimSwingUp");
                return new JimAttacking();

            case JimTrigger.SWING_DOWN:
                animator.Play("ani_jimSwingDown");
                return new JimAttacking();

			case JimTrigger.CHARGE_RIGHT:
                animator.Play("ani_jimChargeR");
                flags &= ~(int)JimFlag.FACING_LEFT;
                return new JimCharging();

            case JimTrigger.CHARGE_LEFT:
                animator.Play("ani_jimChargeR");
                flags |= (int)JimFlag.FACING_LEFT;
                return new JimCharging();

            case JimTrigger.CHARGE_UP:
                animator.Play("ani_jimChargeUp");
                return new JimCharging();

            case JimTrigger.CHARGE_DOWN:
                animator.Play("ani_jimChargeDown");
                return new JimCharging();

            case JimTrigger.PICK_UP_SMALL:
                animator.Play("ani_jimPickUp");
                return new JimPickingUp();

            case JimTrigger.PICK_UP_THROWABLE:
                animator.Play("ani_jimPickUp");
                flags = (int)JimFlag.CARRYING_THROWABLE;
                return new JimPickingUp();

            case JimTrigger.PICK_UP_DROPPABLE:
                animator.Play("ani_jimPickUp");
                flags = (int)JimFlag.CARRYING_DROPABLE;
                return new JimPickingUp();
        }

        return null;
    }
}
