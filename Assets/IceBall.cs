using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private PlayerMovement player;

    public AudioClip spawnSound;

    public bool canParry;

    private bool hitPlayer = false;

    Rigidbody2D RB;
    float movement = -15f;
    // Start is called before the first frame update
    void Start()
    {
       
      
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        canParry = true;

        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RB.velocity = new Vector2( movement, RB.velocity.y); 
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {



            if (!hitPlayer)
            {

                player.PlayerTakeDamage(20);
                canParry = false;

                hitPlayer = true;
                gameObject.GetComponent<Collider2D>().enabled = false;

                Destroy(gameObject);
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
    

