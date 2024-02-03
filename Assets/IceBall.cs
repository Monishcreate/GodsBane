using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private PlayerMovement player;

    private Transform playerPos;

    public SpriteRenderer sprite;

    bool targetisinleft;

    private bool deflected = false;

    private Animator anim;

    private Enemy enemy;

    public AudioClip spawnSound;

    public AudioClip breakSound;

    public bool canParry;

    private bool hitPlayer = false;

    private bool hitEnemy = false;

    Rigidbody2D RB;
    float movement = 15f;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySound(spawnSound);
        
        sprite = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        anim.SetTrigger("spawn");

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
        if (!deflected && !anim.GetBool("isDestroying"))
        {
            
            if (targetisinleft)
            {
                RB.velocity = new Vector2(-movement, RB.velocity.y);
                gameObject.transform.localScale = new Vector3(-6,6,6);
                sprite.flipX = false;
            }
            else
            {
                RB.velocity = new Vector2(movement, RB.velocity.y);
                gameObject.transform.localScale = new Vector3(6,6,6);
                
                sprite.flipX = true;
            }
            

        }
        else
        {
            if (!anim.GetBool("isDestroying"))
            {
                Vector2 target = new Vector2(enemy.rb.position.x, RB.position.y);//update player position to target  
                Vector2 newPos = Vector2.MoveTowards(RB.position, target, 20f * Time.fixedDeltaTime);//update new position to reach to newPos
                RB.MovePosition(newPos);
            }
            
        }

        if (anim.GetBool("isDestroying"))
        {
            RB.velocity = Vector2.zero;
        }




    }

    

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {



            if (!hitPlayer)
            {

                deflected = player.PlayerTakeFreezeDamage(4);
                canParry = false;

                hitPlayer = true;
                

               
                if (!deflected)
                {
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    anim.SetTrigger("destroy");
                    SoundManager.instance.PlaySound(breakSound);
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

            anim.SetTrigger("destroy");


        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (!hitEnemy)
            {
                enemy.TakeFrostDamage(100);
                canParry = false;

                hitEnemy = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                anim.SetTrigger("destroy");

                SoundManager.instance.PlaySound(breakSound);


            }
        }
        else
        {
            hitEnemy = false;
        }

        
    }

}
    

