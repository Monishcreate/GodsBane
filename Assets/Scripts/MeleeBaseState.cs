using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
  
    public float duration;

    public float realduration;

    protected Animator animator;

    protected bool shouldCombo;

    protected int attackIndex;

  

    

    private float AttackPressedTimer = 0;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
        
        
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        AttackPressedTimer -= Time.deltaTime;
        

        
        if(Input.GetMouseButtonDown(0) && animator.GetBool("isPurple"))
        {
            AttackPressedTimer = 1;
            
        }

        if(animator.GetFloat("AttackWindow.Open") > 0f && AttackPressedTimer > 0)
        {
            shouldCombo = true;
        }
    }






    public override void OnExit()
    {
        base.OnExit();  
    }
}
