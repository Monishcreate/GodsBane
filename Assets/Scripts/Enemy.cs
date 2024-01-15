using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Animator anim;
    public bool attackMovable;

    private float coolDowntime = 2f;
    private float coolDownCounter;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask snaptoLayers;


    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        attackMovable = false;
        coolDownCounter = coolDowntime;



    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
       
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }

        CameraShake.instance.ShakeCamera();
        

    }

    void Update()
    {
       if (attackMovable)
        {
            Vector2 target = new Vector2(rb.position.x + 0.13f, rb.position.y);
            rb.MovePosition(target);
            
        }

       coolDownCounter -= Time.deltaTime;

        if (coolDownCounter >= 0)
        {
            SnapToPlayer();
            
        }

    }

    public void EnemyDealDamage()
    {
        Collider2D[] PlayersToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D player in PlayersToDamage)
        {
            Debug.Log("Player HIT");
            player.GetComponent<PlayerMovement>().PlayerTakeDamage(20);
            coolDownCounter = coolDowntime;


        }
    }

    public void SnapToPlayer()
    {
        Collider2D[] SnapToTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, snaptoLayers);
        if (SnapToTargets.Length > 0f)
        {
            anim.SetTrigger("Attack");
            
        }
    }

    public void AttackMove()
    {
        //Vector2 target = new Vector2(rb.position.x + 1f, rb.position.y);
        //rb.MovePosition(target);
        if (attackMovable)
        {
            attackMovable = false;
        }
        else
        {
            attackMovable = true;
        }
    }

    void Die()
    {
        Debug.Log("Bro died");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Update is called once per frame

}
