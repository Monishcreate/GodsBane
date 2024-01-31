using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private PlayerMovement player;

    private Transform playerPos;

 

    private bool deflected = false;

    

    private Enemy enemy;

    public AudioClip spawnSound;

    public bool canParry;

    private bool hitPlayer = false;

    Rigidbody2D RB;
    float movement = -15f;
    // Start is called before the first frame update
    void Start()
    {
       
      
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        

        canParry = true;

        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!deflected)
        {

            Vector2 target = new Vector2(player.RB.position.x, RB.position.y);//update player position to target  
            Vector2 newPos = Vector2.MoveTowards(RB.position, target, 20f * Time.fixedDeltaTime);//update new position to reach to newPos
            RB.MovePosition(newPos);

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
                gameObject.GetComponent<Collider2D>().enabled = false;

               
                if (!deflected)
                {
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

        
    }

}
    

