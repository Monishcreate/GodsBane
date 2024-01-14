using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private float speed = 5f;
    private float acceleration;
    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        
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
       
                
    }

    public void AttackMove()
    {
        
    }

    void Die()
    {
        Debug.Log("Bro died");
    }

    // Update is called once per frame
   
}
