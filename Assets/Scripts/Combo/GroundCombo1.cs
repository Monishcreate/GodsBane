using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCombo1 : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        attackIndex = 2;
        duration = 0.5f;
        animator.SetTrigger("Attack" + attackIndex);
        //animator.SetBool("isAttacking", true);
        Debug.Log("Player Attack" + attackIndex + "Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new GroundCombo2());
            }
            else
            {
                stateMachine.SetNextStateToMain();
                //animator.SetBool("isAttacking", false);
            }
        }
    }
}
