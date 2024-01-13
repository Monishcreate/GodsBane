using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntry : MeleeBaseState
{
    public ComboCharacter cc;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        cc = GetComponent<ComboCharacter>();
        attackIndex = 1;
        cc.cooldown = 1f;
        
        duration = 0.27f;
        realduration = 0.42f;
        animator.SetTrigger("Attack" + attackIndex);
        Debug.Log("Player Attack" + attackIndex + "Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
      
        if (fixedtime >= duration)
        {
            
            if (shouldCombo)
            {
                stateMachine.SetNextState(new GroundCombo1());
            }
            else
            {
                if (fixedtime >= realduration)
                {
                    stateMachine.SetNextStateToMain();
                }
               
                    
            }
        }
    }

   
}
