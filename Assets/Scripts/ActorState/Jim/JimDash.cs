using UnityEngine;
using System.Collections;

public class JimDash : IActorState<JimState, JimTrigger>
{
    public JimState GetState()
    {
        return JimState.DASHING;
    }

    public IActorState<JimState, JimTrigger> OnUpdate(tk2dSpriteAnimator animator, ref int flags)
    {
        return null;
    }

    public IActorState<JimState, JimTrigger> SendTrigger(JimTrigger trigger, GameObject actor, tk2dSpriteAnimator animator, ref int flags)
    {
		switch (trigger) {

		case JimTrigger.IDLE: //used when key is let go before charge completion
				animator.Play("ani_jimIdle");
                return new JimIdle();
		case JimTrigger.SWING_RIGHT:
            	Debug.Log("which swing ani:" + whichSwingAni);

                flags &= ~(int)JimFlag.FACING_LEFT;
                return new JimAttacking();

        case JimTrigger.SWING_LEFT:
                //animator.Play("ani_jimSwingR");
				
                flags |= (int)JimFlag.FACING_LEFT;
                return new JimAttacking();

        case JimTrigger.SWING_UP:
                animator.Play("ani_jimSwingUp");
                return new JimAttacking();

        case JimTrigger.SWING_DOWN:
              animator.Play("ani_jimSwingDown");
              return new JimAttacking();

        }
        return null;
    }

}

