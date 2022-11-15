using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    public GameMaster _gameMaster;
    [SerializeField]private Animator m_Animator;

    private float moveX;
    [SerializeField]private float speed ;
    [SerializeField]private float jumpHeight  ;
    private bool isFacingRight = true;

    //death script
    private PlayerDeath _playerDeath;
    // jump particles
    [SerializeField]private ParticleSystem jumpParticles;

    [SerializeField]private int extraJump;

    private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float walljumpCounter;
    private float wallJumpDuration = 0.25f;
    private Vector2 wallJumpHeight = new Vector2(12f, 17f);

    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    //gun
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform firePoint;




    // Start is called before the first frame update
    void Start()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        _playerDeath = gameObject.GetComponent<PlayerDeath>();
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playAnimation();
        moveX = Input.GetAxisRaw("Horizontal");
        ProcessExtraJump();
        GroundedJump();
        WallSlide();
        WallJump();
        Shoot();
        if (!isWallJumping)
        {
            Flip();
        }
    }
    private void LateUpdate()
    {
    }
    private bool isGrounded()
    {
        return  Physics2D.OverlapBox(groundCheck.position, new Vector3(0.989f, 0.1f, 0),0, groundLayer);

    }
    void OnDrawGizmosSelected()
    {
        // GIZMO TO DRAW GROUND AND WALL CHECK RANGE
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCheck.position, new Vector3(0.989f, 0.1f,0));
        Gizmos.DrawCube(WallCheck.position, new Vector3(0.1f, 0.989f, 0));
    }


    private void FixedUpdate()
    {
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        }
    }
    private void Flip()
    {
        if(isFacingRight && moveX < 0f || !isFacingRight && moveX > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
     private bool isWalled()
    {
        return Physics2D.OverlapBox(WallCheck.position, new Vector3(0.1f, 0.989f, 0),0, wallLayer);
    }
    private void WallSlide()
    {
        if(isWalled() && !isGrounded() && moveX != 0)
        {
            m_Animator.SetTrigger("WallSlide");
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            walljumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            walljumpCounter -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Jump")&& walljumpCounter >0f)
        {
            isWallJumping = true;
            PlayJumpParticles();
            rb.velocity = new Vector2(wallJumpDirection * wallJumpHeight.x, wallJumpHeight.y);
            walljumpCounter = 0f;

            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }

    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private  IEnumerator  OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Spikes")
        {
            //trigger death
            _gameMaster.die();
            Destroy(rb.gameObject);
            Debug.Log("dead");
            other.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
        }

        if (other.gameObject.tag == "JumpReset")
        {
            print("extra jump +1");
            extraJump += 1;
            if (extraJump >= 1)
            {
                extraJump = 1;
            }
            other.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            other.gameObject.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spikes")
        {
            Debug.Log("dead");
            other.gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
        }

    }

    private void playAnimation()
    {
        if (isGrounded())
        {
            m_Animator.SetTrigger("idle");
        }
        if (!isGrounded() && !isWalled())
        {
            m_Animator.SetTrigger("Falling");
        }
    }

    private void ProcessExtraJump()
    {
        if (!isGrounded() || !isWalled())
        {
            if (extraJump > 0)
                if (Input.GetButtonDown("Jump"))
                {
                    PlayJumpParticles();
                    extraJump -= 1;
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                }
        }

        if (isGrounded() || isWalled())
        {

            extraJump = 1;
        }
    }

    private void GroundedJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            PlayJumpParticles();
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           GameObject bullet = Instantiate  (ProjectilePrefab, firePoint.position, transform.rotation); ;

            if (isFacingRight)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(45, 0);
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-45, 0);
            }
        }
    }
    private void PlayJumpParticles()
    {
        jumpParticles.Play();
    }
}

