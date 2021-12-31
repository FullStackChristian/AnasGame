using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utils;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private float 
        movementInputDirection,
        knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool canJump;
    private bool canFlip = true;
    private bool knockback;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;     //sets the movement speed
    public float jumpForce = 16.0f;         //the force with which the character lifts off when jumping key is pressed
    public float groundCheckRadius;         //radius for the groundchek (see the gizmo at the bottom of the character)
    public float movementForceInAir;        //sets the movement force that controlls the character mid air
    public float airDragMultiplier = 0.95f; //When letting go of directional keys mid air the jump force is controlled by this parameter (If set to 0 the character will stop and fall mid air)
    public float variableJumpHeightMultiplier = 0.5f;        //Controlls the jump height - the longer Jump button is pressed the higher the character jumps 

    [SerializeField]
    private Vector2 knockbackSpeed;

    public Transform groundCheck;
    //public Transform wallCheck;

    public LayerMask whatIsGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    private void Update()
    {
        CheckInput();
        
        UpdateAnimations();
        CheckIfCanJump();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        CheckMovementDirection();
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right); //setup animations first
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
            FlipChatBubble();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
            FlipChatBubble();
        }

        //if(rb.velocity.x != 0)
        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckInput()
    {
        //"Horizontal" is set to A and D (A returns -1 and D returns 1)
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void ApplyMovement()
    {   //only apply movement when not attacking
        if (isGrounded && !knockback && anim.GetBool("isAttacking").Equals(false))
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else if (!isGrounded && movementInputDirection != 0 && !knockback)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        else if (!isGrounded && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }
    private void Flip()
    {
        if (!knockback && canFlip.Equals(true))
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }

    private void FlipChatBubble()
    {
        //flip the chat bubble
        var chatBubbleTrnsfrm = gameObject.transform.Find("ChatBubble");
        Vector3 chatScale = chatBubbleTrnsfrm.transform.localScale;
        chatScale.x *= -1;
        chatBubbleTrnsfrm.transform.localScale = chatScale;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}


