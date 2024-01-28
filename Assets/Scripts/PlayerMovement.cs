using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script
    public PlayerData Data;

    private SpriteRenderer rend;

    public AudioClip[] backhits;

    int backhitsindex=0;

    public AudioClip[] footsteps;

    int footstepsindex=0;

    public AudioClip jumpSound;

    private Color color1 = new Color(0.8f,0.8f,0.8f,1);

    private Color color2 = new Color(0.84f,0.42f,0,1);

    private Color color3 = new Color(0,0.69f,0.83f,1);

    public Enemy enemy;

    public GameObject CamTarget;

    private Animator anim;

    public BoxCollider2D box;

    private int Parry;
    

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float jumpForce = 2000f;
    public LayerMask enemyLayers;
    public LayerMask snaptoLayers;
    public LayerMask parryLayers;

    private bool hitMovable;
    private bool backhitMovable;
    private bool KnockMovable;
    

    [SerializeField] private int maxHealth = 100;
    int currentHealth;

    public Slider healthBar;

    public Image wings;

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
    public bool isGrounded = true;
    private bool canMove = true;
    private bool hasParried = false;
    private bool isSpamming = false;
    public bool IsFacingRight { get; private set; }
    public float LastOnGroundTime { get; private set; }

    private float LastRMBPressedTime;

    private float CharacterSwitchCooldown = 1.5f;

    private float CharacterSwitchCounter;

    



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


    private float JumpCooldown = 10;
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

        rend = GetComponent<SpriteRenderer>();

        rend.color = color1;

        if (transform.localScale == new Vector3(1, 1, 1))
        {
            IsFacingRight = true;
        }
        else
        {
            IsFacingRight = false;
        }
        
        anim = GetComponent<Animator>();

        anim.SetBool("isWhite", true);
    }


    private void Update()
    {
        if (backhitsindex == backhits.Length)
        {
            backhitsindex = 0;
        }
        if (footstepsindex == footsteps.Length)
        {
            footstepsindex = 0;
        }
        CharacterSwitchCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1) && CharacterSwitchCounter <= 0 && !anim.GetBool("isWhite") && canMove && isGrounded)
        {
            Hitstop.instance.doSlowDown(1.1f);
            anim.SetTrigger("switchWhite");
            anim.SetBool("isWhite", true);
            anim.SetBool("isOrange", false);
            anim.SetBool("isPurple", false);
            rend.color = color1;

            CharacterSwitchCounter = CharacterSwitchCooldown;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && CharacterSwitchCounter <= 0 && !anim.GetBool("isOrange") && canMove && isGrounded && !OrangeBossScene.instance.isOrangeScene && OrangeBossScene.instance.isBlackScene)
        {
            Hitstop.instance.doSlowDown(1f);
            anim.SetTrigger("switchOrange");
            anim.SetBool("isWhite", false);
            anim.SetBool("isOrange", true);
            anim.SetBool("isPurple", false);
            rend.color = color2;
            CharacterSwitchCounter = CharacterSwitchCooldown;

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && CharacterSwitchCounter <= 0 && !anim.GetBool("isPurple") && canMove && isGrounded && !OrangeBossScene.instance.isOrangeScene && OrangeBossScene.instance.isBlackScene)
        {
            Hitstop.instance.doSlowDown(0.5f);
            anim.SetTrigger("switchPurple");
            anim.SetBool("isWhite", false);
            anim.SetBool("isOrange", false);
            anim.SetBool("isPurple", true);
            rend.color = color3;
            CharacterSwitchCounter = CharacterSwitchCooldown;

        }

        

        float healthUpdate = Mathf.SmoothDamp(healthBar.value, currentHealth, ref sliderVelocity, 25 * Time.deltaTime);
        healthBar.value = healthUpdate;
        CanMoveCheck();
        FlipCheck();
        
        LastRMBPressedTime -= Time.deltaTime;
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        
        JumpCooldown += Time.deltaTime;

        if (JumpCooldown > 10)
        {
            JumpCooldown = 10;
        }
        if (!OrangeBossScene.instance.isOrangeScene)
        {
            wings.fillAmount = JumpCooldown / 10;
        }
        

        
        

        
        #endregion

        #region INPUT HANDLER

        if (Input.GetMouseButtonDown(1) && enemy.canParry && !isSpamming && anim.GetBool("isWhite"))
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
            LastRMBPressedTime = 0.3f;
           
            
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
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))//checks if set box overlaps with ground
        {
            LastOnGroundTime = 0.1f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        #endregion

        if (IsFacingRight)
        {
            PlayerFacingSide = 1;
            //CamTarget.transform.localPosition = new Vector3(7.8f, 0, 0);

        }
        else
        {
            PlayerFacingSide = -1;
            //CamTarget.transform.localPosition = new Vector3(17.8f, 0, 0);
        }


        //if (enemy.enemyFacingDir > 0 && PlayerFacingSide > 0)
        //{
        //    CamTarget.transform.localPosition = new Vector3(-7.8f, 0, 0);
        //}
        //if (enemy.enemyFacingDir > 0 && PlayerFacingSide < 0)
        //{
        //    CamTarget.transform.localPosition = new Vector3(-7.8f, 0, 0);
        //}
        //if (enemy.enemyFacingDir < 0 && PlayerFacingSide > 0)
        //{
        //    CamTarget.transform.localPosition = new Vector3(7.8f, 0, 0);
        //}
        //if (enemy.enemyFacingDir < 0 && PlayerFacingSide < 0)
        //{
        //    CamTarget.transform.localPosition = new Vector3(7.8f, 0, 0);
        //}



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

        if (anim.GetBool("isOrange"))
        {
            Jump();
        }

        if (RB.velocity.y > 0f && !isGrounded)
        {
            anim.SetTrigger("Jump");
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }

        if (RB.velocity.y < -0f && !isGrounded)
        {
            anim.SetTrigger("Fall");
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
            anim.ResetTrigger("Fall");
        }

        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
        }





    }

    private void FixedUpdate()
    {
    
        Move();

        if (hitMovable) // we need to move player based on which side he is attacked from
        {
            Vector2 target = new Vector2(RB.position.x - 3f * PlayerFacingSide, RB.position.y);
            Vector2 newPos = Vector2.MoveTowards(RB.position, target, 3f * Time.fixedDeltaTime);//update new position to reach to newPos
            RB.MovePosition(newPos);

        }

        if (backhitMovable) // we need to move player based on which side he is attacked from
        {
            Vector2 target = new Vector2(RB.position.x + 3f * PlayerFacingSide, RB.position.y);
            Vector2 newPos = Vector2.MoveTowards(RB.position, target, 3f * Time.fixedDeltaTime);//update new position to reach to newPos
            RB.MovePosition(newPos);

        }
        

        if (anim.GetBool("isPurple"))
        {
            initGrab();
            grabDamage();
        }




    }

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Move()
    {
        if (canMove)
        {
            float sprintSpeed;
           

           

            if ((!anim.GetBool("isPurple")) && (Input.GetKey(KeyCode.LeftShift) || anim.GetBool("isOrange")) )
            {
                if (anim.GetBool("isOrange"))
                {
                    sprintSpeed = 2.0f;
                }
                else
                {
                    sprintSpeed = 2.0f;
                }
                
                
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
        if (Input.GetKey(KeyCode.LeftShift) || DoublePress || anim.GetBool("isOrange"))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
        
     
    }

    private void CanMoveCheck()
    {
        if (anim.GetBool("isAttacking") || anim.GetBool("isHurt") || anim.GetBool("isParrying") || anim.GetBool("isGrabbing") || anim.GetBool("isCharging") || anim.GetBool("isSwitching") )
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

    public void setBackHitMovable()
    {
        if (backhitMovable)
        {
            backhitMovable = false;
        }
        else
        {
            backhitMovable = true;
        }
    }

    public void parryToggle()
    {
        anim.SetBool("isParrying", false);
    }

    public void PlayerTakeDamage(int damage)//we gotta make 2 diff ones for taking damage from front and back and use 2 different colliders to do this
    {
        
        
        if (hasParried)
        {
            
            anim.SetTrigger("Parry0");
            anim.SetBool("isParrying", true);
            enemy.GetComponent<Enemy>().TakeParryDamage(damage);
            Hitstop.instance.doHitStop(0.2f);
            CameraShake.instance.ShakeCamera(10f);
            return;
            
        }
        else
        {
            currentHealth -= damage;

            SoundManager.instance.PlaySound(backhits[backhitsindex]);

            backhitsindex++;

            anim.SetTrigger("Hurt1");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera(20f);
        }
        

    }

    public void PlayerTakeJumpDamage(int damage)//we gotta make 2 diff ones for taking damage from front and back and use 2 different colliders to do this
    {


        if (hasParried)
        {

            anim.SetTrigger("Parry1");
            anim.SetBool("isParrying", true);
            enemy.GetComponent<Enemy>().TakeParryDamage(damage);
            Hitstop.instance.doHitStop(0.2f);
            CameraShake.instance.ShakeCamera(20f);
            return;

        }
        else
        {
            currentHealth -= damage;

            SoundManager.instance.PlaySound(backhits[backhitsindex]);

            backhitsindex++;

            anim.SetTrigger("Hurt1");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera(20f);
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AddForce")
        {
            canMove = false;
            Vector2 Knockback = new Vector2(-100f, 5f);
            RB.AddForce(Knockback, ForceMode2D.Impulse);
        }
        else
        {
            canMove = true;
        }

        if (collision.tag == "FallDown")
        {
            PlayerTakeDamage(500);
        }
    }

   

    public void PlayerTakeDamageLower(int damage)//we gotta make 2 diff ones for taking damage from front and back and use 2 different colliders to do this
    {


        if (hasParried)
        {

            anim.SetTrigger("Parry1");
            anim.SetBool("isParrying", true);
            enemy.GetComponent<Enemy>().TakeParryDamage(damage);
            Hitstop.instance.doHitStop(0.2f);
            CameraShake.instance.ShakeCamera(10f);
            return;

        }
        else
        {
            currentHealth -= damage;

            SoundManager.instance.PlaySound(backhits[backhitsindex]);

            backhitsindex++;

            anim.SetTrigger("Hurt2");

            if (currentHealth <= 0)
            {
                Die();
            }

            CameraShake.instance.ShakeCamera(20f);
        }


    }

    public void PlayerBackDamage(int damage)
    {
        currentHealth -= damage;

        SoundManager.instance.PlaySound(backhits[backhitsindex]);

        backhitsindex++;

        anim.SetTrigger("BackHurt");

        if (currentHealth <= 0)
        {
            Die();
        }

        CameraShake.instance.ShakeCamera(20f);
    }

    public void Jump()
    {
        

        if (Input.GetButtonDown("Jump") && isGrounded && JumpCooldown >= 10f && !anim.GetBool("isSwitching"))
        {
            anim.SetBool("isCharging",true);
     

        }
        if (Input.GetButtonUp("Jump") && isGrounded && JumpCooldown >= 10f && !anim.GetBool("isSwitching"))
        {
            SoundManager.instance.PlaySound(jumpSound);
            JumpCooldown = 0f; 
            anim.SetBool("isCharging", false);
            RB.AddForce(new Vector2(RB.velocity.x, jumpForce));
            Hitstop.instance.doHitStop(0.2f);
            CameraShake.instance.ShakeCamera(20f);
        }


    }

    public void initGrab()
    {
        if (Input.GetKeyDown(KeyCode.F) && !anim.GetBool("isParrying") && !anim.GetBool("isAttacking") && !anim.GetBool("isHurt"))
        {
            anim.SetBool("isGrabbing", true);
            anim.SetTrigger("Grab");

        }
        else
        {
            anim.ResetTrigger("Grab");
        }

    }

    public void grabDamage()
    {
        if (anim.GetBool("isGrabbing"))
        {
            Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            if(EnemiesToDamage.Length > 0) 
            {
                anim.SetTrigger("grabAttacking");
                anim.SetBool("isGrabbing", false);
                
            }
            else
            {
                anim.ResetTrigger("grabAttacking");
            }
        }
    }



    public void setGrabBool()
    {
        anim.SetBool("isGrabbing", false);
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DealDamage()
    {
        Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in EnemiesToDamage)
        {
         
            enemy.GetComponent<Enemy>().TakeDamage(20);
        }
    }

    public void DealDamageLower()
    {
        Collider2D[] EnemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in EnemiesToDamage)
        {
           
            enemy.GetComponent<Enemy>().TakeDamageLower(20);
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

    public void PlayFootsteps()
    {
        SoundManager.instance.PlaySound(footsteps[footstepsindex]);
        footstepsindex++;
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
