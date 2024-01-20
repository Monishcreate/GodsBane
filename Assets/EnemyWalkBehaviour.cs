using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rb = animator.GetComponent<Rigidbody2D>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        float ydiff = playerPos.position.y - rb.position.y;
        if (ydiff < 0)
        {
            ydiff = ydiff * -1;
        }

        Vector2 target = new Vector2(playerPos.position.x, rb.position.y);//update player position to target  
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, 3f * Time.fixedDeltaTime);//update new position to reach to newPos
        rb.MovePosition(newPos);

        

        if (ydiff <= 2f)
        {
            animator.SetTrigger("RunFromWalk");
        }





        //move the enemy to newPos which keeps updating
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
