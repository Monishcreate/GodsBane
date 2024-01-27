using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallAttackBehaviour : StateMachineBehaviour
{
    private Enemy enemy;
    private Transform playerPos;


    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        rb = animator.GetComponent<Rigidbody2D>();

        enemy.ParryableWindowEnable();



    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        float xdiff = playerPos.position.x - rb.position.x;
        float ydiff = playerPos.position.y - rb.position.y;
        if (xdiff < 0)
        {
            xdiff *= -1;
        }
        if (ydiff < 0)
        {
            ydiff *= -1;
        }

        Vector2 target = new Vector2(playerPos.position.x - 1.5f * enemy.enemyFacingDir, playerPos.position.y);//update player position to target  
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, 40f * Time.fixedDeltaTime);//update new position to reach to newPos
        rb.MovePosition(newPos);

        if (xdiff <= 2f && ydiff <= 0.5f)
        {
            enemy.EnemyDealJumpDamage();
            enemy.ParryableWindowDisable();
            animator.SetTrigger("Attack");
            
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
