using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDestroy : MonoBehaviour
{
    private PlayerMovement player;

    public AudioClip spawnSound;

    public AudioClip[] destroySounds;

    int i;

    private Animator anim;

    private bool hitPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        anim.Play("Spawn");

        SoundManager.instance.PlaySound(spawnSound);


    }
    private void Update()
    {
        if (i == destroySounds.Length)
        {
            i = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            anim.Play("Destroy");
            SoundManager.instance.PlaySound(destroySounds[i]);
            i++;
            gameObject.GetComponent<Collider2D>().enabled = false;
           
            
        }

        if(collision.gameObject.tag == "Player")
        {
            anim.Play("Destroy");
            SoundManager.instance.PlaySound(destroySounds[i]);
            i++;

            if (!hitPlayer)
            {
                
                player.PlayerBackDamage(20);
                hitPlayer = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
           


        }
        else
        {
            hitPlayer = false;
        }
    }

}
