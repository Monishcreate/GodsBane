using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCombo2 : MeleeBaseState
{
    public PlayerMovement cc;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        cc = GetComponent<PlayerMovement>();
        cc.cooldown = 1f;
        attackIndex = 3;
        duration = 0.63f;
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
                stateMachine.SetNextState(new GroundCombo3());
            }
            else
            {
                if (fixedtime >= realduration)
                {
                    stateMachine.SetNextStateToMain();
                    animator.SetBool("isAttacking", false);
                }
                
                //
            }
        }
    }
}
