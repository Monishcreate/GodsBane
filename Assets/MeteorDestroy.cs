using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDestroy : MonoBehaviour
{
    private PlayerMovement player;

    public AudioClip spawnSound;

    private Enemy enemy;

    private Animator anim;

    private bool hitPlayer = false;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();

        anim.Play("Spawn");

        SoundManager.instance.PlaySound(spawnSound);


    }
    private void Update()
    {
        if (enemy.j == enemy.destroySounds.Length)
        {
            enemy.j = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            anim.Play("Destroy");
            SoundManager.instance.PlaySound(enemy.destroySounds[enemy.j]);
            enemy.j++;
            gameObject.GetComponent<Collider2D>().enabled = false;
           
            
        }

        if(collision.gameObject.tag == "Player")
        {
            anim.Play("Destroy");
            SoundManager.instance.PlaySound(enemy.destroySounds[enemy.j]);
            enemy.j++;

            if (!hitPlayer)
            {
                
                player.PlayerBackDamage(50);
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
