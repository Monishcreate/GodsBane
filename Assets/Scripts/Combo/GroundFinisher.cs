using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisher : MeleeBaseState
{
    public ComboCharacter cc;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        cc = GetComponent<ComboCharacter>();
        cc.cooldown = 1f;
        attackIndex = 5;
        duration = 1.18f;
       
        animator.SetTrigger("Attack" + attackIndex);
        Debug.Log("Player Attack" +  attackIndex + "Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            
            stateMachine.SetNextStateToMain();
            animator.SetBool("isAttacking", false);

        }
    }
}
