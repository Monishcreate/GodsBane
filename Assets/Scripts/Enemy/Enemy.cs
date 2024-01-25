using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //WE GOTTA MAKE SOME CHANGES TO FACILITATE ENEMY DIRECTION TURNING AS WELL
    //AND ALSOOO OUR ENEMY IS USING KINEMATIC RIGIDBODY SO WE GOTTA MAKE CODE TO DETECT WALLS AND SHIT AS WELL :SOB:
    //AND WE GOTTA HANDLE SOME GLITCHES WHEN ENEMY ATTACKS OUR PLAYER
    public Rigidbody2D rb;
    public Animator anim;
    public bool hitMovable;
    public bool attackMove;
    public bool isMeteor;
    public BoxCollider2D box;
    public BoxCollider2D dashcollider;
    public bool canParry;
    public Slider healthBar;
    float sliderVelocity = 0f;
    public int enemyFacingDir;


    [SerializeField] GameObject tp1;
    [SerializeField] GameObject tp2;
    [SerializeField] GameObject tp3;

    float tploc1;
    float tploc2;
    float tploc3;

    private float tiredTimer;

    private float coolDowntime = 2f;
    private float coolDownCounter;

    public Transform attackPoint;
    public Transform knockbackPoint;
    public Transform detectPoint;
    
    
    public Vector2 detectRange;
    public float attackRange = 0.5f;

    public LayerMask detectionLayers;
    public LayerMask snaptofrontlayer;
    public LayerMask snaptobacklayer;
    public LayerMask playerfrontlayer;
    public LayerMask playerbacklayer;
    public LayerMask playerlayer;

    public GameObject Player;

    public PlayerMovement pl;
    public MeteorShower meteor;


    [SerializeField] private int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        hitMovable = false;
        coolDownCounter = coolDowntime;
    }

    public void TakeDamage(int damage)
    {
        if (anim.GetBool("isTired"))
        {
            currentHealth -= damage;



            anim.SetTrigger("Hurt1");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera(4f);
        }


    }

    public void TakeDamageLower(int damage)
    {
        if (anim.GetBool("isTired"))
        {
            currentHealth -= damage;



            anim.SetTrigger("Hurt2");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera(4f);
        }


    }
    public void TakeParryDamage(int damage)
    {
        currentHealth -= damage;



     

        if (currentHealth <= 0)
        {
            Die();
        }

         


    }

    void Update()
    {

        tiredTimer -= Time.deltaTime;

        EnemyFlip();


        float healthUpdate = Mathf.SmoothDamp(healthBar.value, currentHealth, ref sliderVelocity, 25 * Time.deltaTime);
        healthBar.value = healthUpdate;
        

        //rb.bodyType = RigidbodyType2D.Dynamic;

       
       
        //DetectPlayer();
        EnemyDash();

        


        if (anim.GetBool("tiredTimer"))
        {
            tiredTimer = 7f;
        }

        if (anim.GetBool("isTired") && tiredTimer <= 0)
        {
            anim.SetTrigger("ExitTired");
        }
        if (anim.GetBool("teleportReady"))
        {
            tploc1 = tp1.transform.position.x;
            tploc2 = tp2.transform.position.x;
            tploc3 = tp3.transform.position.x;
        }

        if (anim.GetBool("isTeleport"))
        {
            if (Player.GetComponent<Animator>().GetBool("isMovingBack"))
            {
                gameObject.transform.position = new Vector3(tploc1, rb.position.y);
            }
            if (Player.GetComponent<Animator>().GetBool("isMoving") && !Player.GetComponent<Animator>().GetBool("isMovingBack"))
            {
                gameObject.transform.position = new Vector3(tploc2, rb.position.y);
            }
            if (!Player.GetComponent<Animator>().GetBool("isMoving"))
            {
                gameObject.transform.position = new Vector3(tploc3, rb.position.y);
            }

        }
      


    }

    private void FixedUpdate()
    {
        if (hitMovable)
        {
            Vector2 target = new Vector2(rb.position.x - 3f * enemyFacingDir, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 5f * Time.fixedDeltaTime);//update new position to reach to newPos
            rb.MovePosition(newPos);


        }
        if (attackMove)
        {
            Vector2 target = new Vector2(rb.position.x + 7f * enemyFacingDir, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 7f * Time.fixedDeltaTime);//update new position to reach to newPos
            rb.MovePosition(newPos);
        }
    }

    public void EnemyFlip()
    {
        if (Player.transform.position.x < this.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            enemyFacingDir = -1;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            enemyFacingDir = 1;
        }
    }

    public void EnemyDealDamage()
    {
        Collider2D[] PlayersFrontDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerfrontlayer);
        if(PlayersFrontDamage.Length > 0)
        {
            Debug.Log("Player HIT");
            pl.PlayerTakeDamage(20);
            coolDownCounter = coolDowntime;
        }

        Collider2D[] PlayersBackDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerbacklayer);
        if(PlayersBackDamage.Length > 0)
        {
            Debug.Log("Player HIT");
            pl.PlayerBackDamage(20);
            coolDownCounter = coolDowntime;
        }

    }


    public void EnemyDealDamageLower()
    {
        Collider2D[] PlayersFrontDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerfrontlayer);
        if (PlayersFrontDamage.Length > 0)
        {
            Debug.Log("Player HIT");
            pl.PlayerTakeDamageLower(20);
            coolDownCounter = coolDowntime;
        }

        Collider2D[] PlayersBackDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerbacklayer);
        if (PlayersBackDamage.Length > 0)
        {
            Debug.Log("Player HIT");
            pl.PlayerBackDamage(20);
            coolDownCounter = coolDowntime;
        }



    }

    public void EnemyDash()
    {

        if (anim.GetBool("isDashing"))
        {
            dashcollider.enabled = true;
            CameraShake.instance.ShakeCamera(7f);
        }
        else
        {
            dashcollider.enabled = false;
        }
    }

    //public void DetectPlayer()
    //{
    //    Collider2D[] SnapToTargets = Physics2D.OverlapBoxAll(detectPoint.position,detectRange, 1f, detectionLayers);
    //    if (SnapToTargets.Length > 0f)
    //    {
            
    //        anim.SetTrigger("Attack");
  
    //    }
    //    else
    //    {
    //        anim.ResetTrigger("Attack");
    //    }
    //}
    // YO we gotta snap to different positions depending on whether the teleport point is in front of the player or the back :)
    //public void SnapToPlayer()
    //{
    //    Collider2D[] SnapToTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, snaptofrontlayer);
    //    if (SnapToTargets.Length > 0f)
    //    {

    //        GameObject SnapToPoint = GameObject.Find("Player Teleport Position");
    //        Vector2 snapPosition = SnapToPoint.transform.position;
    //        rb.MovePosition(snapPosition);
    //    }

    //    Collider2D[] SnapToBack = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, snaptobacklayer);
    //    if (SnapToBack.Length > 0f)
    //    {

    //        GameObject SnapToPoint = GameObject.Find("Player Teleport Back");
    //        Vector2 snapPosition = SnapToPoint.transform.position;
    //        rb.MovePosition(snapPosition);
    //    }
    //}

    public void AttackMove() //this is for movement of enemy when he gets hit
    {
        //Vector2 target = new Vector2(rb.position.x + 1f, rb.position.y);
        //rb.MovePosition(target);
        if (hitMovable)
        {
            hitMovable = false;
        }
        else
        {
            hitMovable = true;
        }
    }

    public void SetBoolAttackMove() //this is for movement of enemy when he is attacking
    {
        if (attackMove)
        {
            attackMove = false;
        }
        else
        {
            attackMove= true;
        }
    }

    public void ParryableWindowEnable()
    {
        box.enabled = true;
        canParry = true;
    }

    public void ParryableWindowDisable()
    {
        box.enabled = false;
        canParry = false;
    }


    void Die()
    {
        Debug.Log("Bro died");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireCube(detectPoint.position, detectRange);
    }

    // Update is called once per frame

}
