using System.Collections;
using System.Collections.Generic;
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
    public BoxCollider2D box;
    public bool canParry;
    public Slider healthBar;
    float sliderVelocity = 0f;

    private float coolDowntime = 2f;
    private float coolDownCounter;

    public Transform attackPoint;
    public Transform detectPoint;
    
    public Vector2 detectRange;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask detectionLayers;
    public LayerMask snaptoLayers;


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
        currentHealth -= damage;

        
       
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }

        CameraShake.instance.ShakeCamera();
        

    }

    void Update()
    {
        float healthUpdate = Mathf.SmoothDamp(healthBar.value, currentHealth, ref sliderVelocity, 25 * Time.deltaTime);
        healthBar.value = healthUpdate;
        if (hitMovable)
        {
            Vector2 target = new Vector2(rb.position.x + 3f, rb.position.y); 
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 5f * Time.fixedDeltaTime);//update new position to reach to newPos
            rb.MovePosition(newPos);
           
            
        }
       if (attackMove)
        {
            Vector2 target = new Vector2(rb.position.x - 7f, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 7f * Time.fixedDeltaTime);//update new position to reach to newPos
            rb.MovePosition(newPos);
        }

       
       
         DetectPlayer();
            
       

    }

    public void EnemyDealDamage()
    {
        Collider2D[] PlayersToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D player in PlayersToDamage)
        {
            Debug.Log("Player HIT");
            player.GetComponent<PlayerMovement>().PlayerTakeDamage(20);
            coolDownCounter = coolDowntime;


        }
    }

    public void DetectPlayer()
    {
        Collider2D[] SnapToTargets = Physics2D.OverlapBoxAll(detectPoint.position,detectRange, 1f, detectionLayers);
        if (SnapToTargets.Length > 0f)
        {
            anim.SetTrigger("Attack");

            
        }
    }
    // YO we gotta snap to different positions depending on whether the teleport point is in front of the player or the back :)
    public void SnapToPlayer()
    {
        Collider2D[] SnapToTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, snaptoLayers);
        if (SnapToTargets.Length > 0f)
        {

            GameObject SnapToPoint = GameObject.Find("Player Teleport Position");
            Vector2 snapPosition = SnapToPoint.transform.position;
            rb.MovePosition(snapPosition);
        }
    }

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
