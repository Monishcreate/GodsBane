using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script
    public PlayerData Data;


    public Enemy enemy;

    private Animator anim;

    public BoxCollider2D box;

    private int Parry;
    

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask snaptoLayers;
    public LayerMask parryLayers;

    private bool hitMovable;

    [SerializeField] private int maxHealth = 100;
    int currentHealth;

    public Slider healthBar;
    float sliderVelocity = 0f;

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    //Script to handle all player animations, all references can be safely removed if you're importing into your own project.
    #endregion

    #region STATE PARAMETERS
    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    private bool attackMove = false;
    private bool canMove = true;
    private bool hasParried = false;
    private bool isSpamming = false;
    public bool IsFacingRight { get; private set; }
    public float LastOnGroundTime { get; private set; }

    private float LastRMBPressedTime;

    



    bool DoublePress;
    float LastATime;
    float LastDTime;

    float PlayerFacingSide;

    #endregion

    #region INPUT PARAMETERS
    private Vector2 _moveInput;
    #endregion

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Awake()
    {
       
        RB = GetComponent<Rigidbody2D>();
     
      
    }

    private void Start()
    {
        hitMovable = false;
        canMove = true;
        currentHealth = maxHealth;
        

        IsFacingRight = true;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        float healthUpdate = Mathf.SmoothDamp(healthBar.value, currentHealth, ref sliderVelocity, 25 * Time.deltaTime);
        healthBar.value = healthUpdate;
        CanMoveCheck();
        FlipCheck();
        
        LastRMBPressedTime -= Time.deltaTime;
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;


        #endregion

        #region INPUT HANDLER
        
        if (Input.GetMouseButtonDown(1) && enemy.canParry && !isSpamming)
        {
           
            Collider2D[] ParryTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, parryLayers);
            if (ParryTargets.Length > 0f)
            {

                hasParried = true;
                if (Parry < 1)
                {
                    Parry++;
                }
                else
                {
                    Parry = 0;
                }
                

            }
            LastRMBPressedTime = 0.5f;
           
            
        }
       
        else if (enemy.canParry == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                LastRMBPressedTime = 0.5f;
                isSpamming = true;
                
            }
            
            hasParried = false;
        }

        if (LastRMBPressedTime <= 0f)
        {
            isSpamming = false;
        }




        if (canMove)
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
        }
        

        if (_moveInput.x != 0)
        {
            CheckDirectionToFace(_moveInput.x > 0);
            anim.SetBool("isMoving", true);

        }
            
        if (_moveInput.x > 0)
        {
            
            anim.SetBool("isMoving", true);
            if (IsFacingRight)
            {
                anim.SetBool("isMovingBack", false);
            }
            else if (!IsFacingRight)
            {
                anim.SetBool("isMovingBack", true);
            }
            
           
            
        }
            
        if (_moveInput.x < 0)
        {
            anim.SetBool("isMoving", true);
            if (!anim.GetBool("isRunning"))
            {
                if (IsFacingRight)
                {
                    anim.SetBool("isMovingBack", true);
                }
                else if (!IsFacingRight)
                {
                    anim.SetBool("isMovingBack", false);
                }
                
            }
        }

        
        if (_moveInput.x == 0)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isMovingBack", false);
        }
            
        #endregion

        #region COLLISION CHECKS
        //Ground Check
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
            LastOnGroundTime = 0.1f;
        #endregion

        if (IsFacingRight)
        {
            PlayerFacingSide = 1;
        }
        else
        {
            PlayerFacingSide = -1;
        }

        if (hitMovable) // we need to move player based on which side he is attacked from
        {
            Vector2 target = new Vector2(RB.position.x - 2f * PlayerFacingSide, RB.position.y);
            Vector2 newPos = Vector2.MoveTowards(RB.position, target, 5f * Time.fixedDeltaTime);//update new position to reach to newPos
            RB.MovePosition(newPos);

        }
       
        
    }

    private void FlipCheck()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            
            float DoublePressTime = 0.2f;
           
            float TimeSinceLastClick = Time.time - LastDTime;
            if (TimeSinceLastClick < DoublePressTime)
            {
                DoublePress = true;
            }
            else
            {
                DoublePress = false;
            }
            LastDTime = Time.time;

        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            
            float DoublePressTime = 0.2f;
            float TimeSinceLastClick = Time.time - LastATime;
            if (TimeSinceLastClick <= DoublePressTime)
            {
                DoublePress = true;
            }
            else
            {
                DoublePress = false;
            }
            LastATime = Time.time;

        }


       
        


    }

    private void FixedUpdate()
    {
    
         Move();

        
        


    }

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Move()
    {
        if (canMove)
        {
            float sprintSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprintSpeed = 2.5f;
                if (anim.GetBool("isMoving"))
                {
                    anim.SetBool("isRunning", true);
                }
                if (!anim.GetBool("isMoving"))
                {
                    anim.SetBool("isRunning", false);
                }

            }
            else
            {
                sprintSpeed = 1;
                anim.SetBool("isRunning", false);
            }
            //Calculate the direction we want to move in and our desired velocity
            float targetSpeed = _moveInput.x * Data.runMaxSpeed * sprintSpeed;

            #region Calculate AccelRate
            float accelRate;

            //Gets an acceleration value based on if we are accelerating (includes turning) 
            //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
            #endregion

            //Not used since no jump implemented here, but may be useful if you plan to implement your own
            /* 
            #region Add Bonus Jump Apex Acceleration
            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }
            #endregion
            */

            #region Conserve Momentum
            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                //Prevent any deceleration from happening, or in other words conserve are current momentum
                //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }
            #endregion

            //Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - RB.velocity.x;

            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelRate * sprintSpeed;

            //Convert this to a vector and apply to rigidbody

            RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime * movement) / RB.mass, RB.velocity.y);



            /*
             * For those interested here is what AddForce() will do
             * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
             * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
            */
        }
        else if(!canMove && attackMove) //when attackMove is called via attack animation
        {
            RB.velocity = new Vector2 (5 * PlayerFacingSide, RB.velocity.y); //move the player till that bool is true
        }
        else if (!canMove)//set velocity to 0 if canMove is set to false through attack animation
        {
            RB.velocity = new Vector2(0, RB.velocity.y);
        }

    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        if (Input.GetKey(KeyCode.LeftShift) || DoublePress)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
        
     
    }

    private void CanMoveCheck()
    {
        if (anim.GetBool("isAttacking") || anim.GetBool("isHurt") || anim.GetBool("isParrying"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
    public void SetAttackingTrue()
    {
        attackMove = true;
    }
    
    public void SetAttackingFalse()
    {
        attackMove = false;
    }

    public void setHitMovable()
    {
        if (hitMovable)
        {
            hitMovable = false;
        }
        else
        {
            hitMovable = true;
        }
    }

    public void PlayerTakeDamage(int damage)//we gotta make 2 diff ones for taking damage from front and back and use 2 different colliders to do this
    {
        
        
        if (hasParried)
        {
            
            
            
            anim.SetTrigger("Parry" + Parry );
            enemy.GetComponent<Enemy>().TakeParryDamage(20);
            Hitstop.instance.doHitStop(0.24f);
            CameraShake.instance.ShakeCamera();
            return;
            
        }
        else
        {
            currentHealth -= damage;



            anim.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera();
        }
        

    }

    void Die()
    {
        Debug.Log("you died bro");
    }

    public void DealDamage()
    {
        Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in EnemiesToDamage)
        {
            Debug.Log("HIT");
            enemy.GetComponent<Enemy>().TakeDamage(20);
        }
    }
    
    public void SnapToEnemy()
    {
        Collider2D[] SnapToTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, snaptoLayers);
        if (SnapToTargets.Length > 0f)
        {

            GameObject SnapToPoint = GameObject.Find("Teleport Position");
            Vector2 snapPosition = SnapToPoint.transform.position;
            RB.MovePosition(snapPosition);
        }
    }
    
    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    #endregion
}
