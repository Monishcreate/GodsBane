using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private PlayerMovement player;

    private Transform playerPos;

    bool targetisinleft;

    private bool deflected = false;

    

    private Enemy enemy;

    public AudioClip spawnSound;

    public bool canParry;

    private bool hitPlayer = false;

    private bool hitEnemy = false;

    Rigidbody2D RB;
    float movement = 15f;
    // Start is called before the first frame update
    void Start()
    {
        

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        

        canParry = true;

        RB = GetComponent<Rigidbody2D>();

        if (player.transform.position.x < RB.position.x)
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
        if (!deflected)
        {
            
            if (targetisinleft)
            {
                RB.velocity = new Vector2(-movement, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(movement, RB.velocity.y);
            }
            

        }
        else
        {
            Vector2 target = new Vector2(enemy.rb.position.x, RB.position.y);//update player position to target  
            Vector2 newPos = Vector2.MoveTowards(RB.position, target, 20f * Time.fixedDeltaTime);//update new position to reach to newPos
            RB.MovePosition(newPos);
        }
       

        
        
    }

    

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {



            if (!hitPlayer)
            {

                deflected = player.PlayerTakeFreezeDamage(2);
                canParry = false;

                hitPlayer = true;
                

               
                if (!deflected)
                {
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    Destroy(gameObject);
                }
                
            }



        }
        else
        {
            hitPlayer = false;
        }

        if (collision.gameObject.tag == "Ground")
        {
            
            gameObject.GetComponent<Collider2D>().enabled = false;
         
            Destroy(gameObject);


        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (!hitEnemy)
            {
                enemy.TakeDamageLower(30);
                canParry = false;

                hitEnemy = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject);

                
            }
        }
        else
        {
            hitEnemy = false;
        }

        
    }

}
    

