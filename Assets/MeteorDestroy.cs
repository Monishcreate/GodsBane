using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDestroy : MonoBehaviour
{
    private PlayerMovement player;

    private Animator anim;

    private bool hitPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        anim.Play("Spawn");
      
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            anim.Play("Destroy");
           
            
        }

        if(collision.gameObject.tag == "Player")
        {
            anim.Play("Destroy");

            if (!hitPlayer)
            {
                
                player.PlayerBackDamage(20);
                hitPlayer = true;
            }
           


        }
        else
        {
            hitPlayer = false;
        }
    }

}
