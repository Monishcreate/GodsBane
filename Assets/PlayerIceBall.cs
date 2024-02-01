using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceBall : MonoBehaviour
{
    private PlayerMovement player;

    private Transform playerPos;

    bool targetisinleft;

    private bool deflected = false;

    private Animator anim;

    private Enemy enemy;

    public AudioClip spawnSound;

    public bool canParry;


    private bool hitEnemy = false;

    Rigidbody2D RB;
    float movement = 15f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        
        
        anim = GetComponent<Animator>();

        anim.SetTrigger("spawn");

        canParry = true;

        RB = GetComponent<Rigidbody2D>();

        if (!player.IsFacingRight)
        {
            targetisinleft = true;
        }
        else
        {
            targetisinleft = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
            
            if (!targetisinleft)
            {
                if (!anim.GetBool("isDestroying"))
                {
                    RB.velocity = new Vector2(movement, RB.velocity.y);
                    this.transform.localScale = new Vector3(6, 6, 6);
                }
                
            }
            else
            {
                 if (!anim.GetBool("isDestroying"))
                 {
                     RB.velocity = new Vector2(-movement, RB.velocity.y);
                     this.transform.localScale = new Vector3(-6, 6, 6);
                 }
            
            }

        if (anim.GetBool("isDestroying"))
        {
            RB.velocity = Vector2.zero;
        }
            

        
        
       

        
        
    }

    

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.tag == "Ground")
        {
            
            gameObject.GetComponent<Collider2D>().enabled = false;


            anim.SetTrigger("destroy");

        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (!hitEnemy)
            {
                enemy.TakeFreezeDamage(175, 4);
                canParry = false;

                hitEnemy = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                anim.SetTrigger("destroy");


            }
        }
        else
        {
            hitEnemy = false;
        }

        
    }

}
    

