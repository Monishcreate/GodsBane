using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCombo1 : MeleeBaseState
{
    public ComboCharacter cc;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        cc = GetComponent<ComboCharacter>();
        cc.cooldown = 1f;
        attackIndex = 2;
        
        duration = 0.47f;
        realduration = 0.93f;
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
                if(fixedtime >= realduration)
                {
                    stateMachine.SetNextStateToMain();
                    animator.SetBool("isAttacking", false);
                }


            }
        }
    }
}
