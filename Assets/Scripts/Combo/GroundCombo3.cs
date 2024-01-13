using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCombo3 : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        attackIndex = 4;
        duration = 0.6f;
        realduration = 1f;
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
                stateMachine.SetNextState(new GroundFinisher());
            }
            else
            {
                if (fixedtime >= realduration) 
                {
                    stateMachine.SetNextStateToMain();
                }
                
                //animator.SetBool("isAttacking", false);
            }
        }
    }
}
