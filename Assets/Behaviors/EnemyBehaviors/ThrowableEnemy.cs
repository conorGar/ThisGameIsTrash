using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Light wrapper class over ThrowableObject so we can control the actor states of a pick up able and throwable enemy. 
[RequireComponent(typeof(EnemyStateController))]
public class ThrowableEnemy : ThrowableObject {
    EnemyStateController controller;
	void Awake () {
        requiresGrabbyGloves = true;
        controller = GetComponent<EnemyStateController>();
	}

    protected override void Update () {
        if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
            if (controller.GetCurrentState() == EnemyState.DAZED ||
                controller.GetCurrentState() == EnemyState.CARRIED ||
                controller.GetCurrentState() == EnemyState.THROWN) {
                if (controller.IsFlag((int)EnemyFlag.THROWABLE)) {
                    base.Update();
                }
            }
        }
	}

    protected override void Throw()
    {
        if (controller.GetCurrentState() == EnemyState.CARRIED) {
            if (controller.IsFlag((int)EnemyFlag.THROWABLE)) {
                controller.SendTrigger(EnemyTrigger.THROWN);
                base.Throw();
            }
        }
    }

    public override void LandingEvent()
    {
        if (controller.GetCurrentState() == EnemyState.THROWN) {
            if (controller.IsFlag((int)EnemyFlag.THROWABLE)) {
                controller.SendTrigger(EnemyTrigger.DEATH); // Dazed, really.
                base.LandingEvent();
            }
        }
    }

    public override void PickUp()
    {
        if (controller.GetCurrentState() == EnemyState.DAZED) {
            if (controller.IsFlag((int)EnemyFlag.THROWABLE)) {
                controller.SendTrigger(EnemyTrigger.CARRIED);
                base.PickUp();
            }
        }
    }
}
