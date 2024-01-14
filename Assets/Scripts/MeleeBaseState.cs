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

    protected Collider2D hitCollider;

    private List<Collider2D> collidersDamaged;

    private GameObject HitEffectPrefab;

    private float AttackPressedTimer = 0;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
        collidersDamaged = new List<Collider2D>();
        hitCollider = GetComponent<ComboCharacter>().hitbox;
        HitEffectPrefab = GetComponent<ComboCharacter>().Hiteffect;
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        AttackPressedTimer -= Time.deltaTime;
        

        if (animator.GetFloat("Weapon.Active") > 0f )
        {
            
            Attack();
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            AttackPressedTimer = 2;
            
        }

        if(animator.GetFloat("AttackWindow.Open") > 0f && AttackPressedTimer > 0)
        {
            shouldCombo = true;
        }
    }
    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);
        for ( int i = 0; i < colliderCount; i++ )
        {
            if(!collidersDamaged.Contains(collidersToDamage[i] ) )
            {
                TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();
                
                if( hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                {
                    GameObject.Instantiate(HitEffectPrefab, collidersToDamage[i].transform);
                    Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");
                    collidersDamaged.Add(collidersToDamage[i]);
                }
            }
        }
    }

    protected void MyAttack()
    {
        
    }

    public override void OnExit()
    {
        base.OnExit();  
    }
}
