using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Animator anim;
    public bool attackMovable;
    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        attackMovable = false;
        
        
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;


       
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }

    void Update()
    {
       if (attackMovable)
        {
            Vector2 target = new Vector2(rb.position.x + 0.13f, rb.position.y);
            rb.MovePosition(target);
            
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

    // Update is called once per frame
   
}
