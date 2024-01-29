using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntry : MeleeBaseState
{
    public PlayerMovement cc;
    

   
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

  

        

        //Attack
        cc = GetComponent<PlayerMovement>();
        attackIndex = 1;
        cc.cooldown = 1f;
        
        duration = 0.38f;
        realduration = 0.6f;
        
        animator.SetTrigger("Attack" + attackIndex);
        

        animator.SetBool("isAttacking", true);

        Debug.Log("Player Attack" + attackIndex + "Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
      
        if (fixedtime >= duration)
        {
            
            if (shouldCombo && animator.GetBool("isPurple"))
            {
                stateMachine.SetNextState(new GroundCombo1());
            }
            else
            {
                if (fixedtime >= realduration)
                {
                    stateMachine.SetNextStateToMain();
                    animator.SetBool("isAttacking", false);
                }
               
                    
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
      
    }


}
