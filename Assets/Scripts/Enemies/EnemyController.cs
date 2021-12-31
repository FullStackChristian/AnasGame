﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }


    private State currentState;
     
    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHealth,
        knockbackDuration,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;

    [SerializeField]
    private Vector2 knockbackSpeed;

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck,
        touchDamageCheck;

    [SerializeField]
    private GameObject hitParticle, deathChunkParticle, deathBloodParticle;

    [SerializeField]
    private LayerMask 
        whatIsGround,
        whatIsPlayer;

    private float 
        currentHealth,
        lastTouchDamageTime,
        knockbackStartTime;


    private float[] attackDetails = new float[2];

    private int
        facingDirection,
        damageDirection;

    private Vector2 
        movement,
        touchDamageBotLeft,
        touchDamageTopRight;

    private bool
        groundDetected,
        wallDetected;

    private GameObject alive;

    private Rigidbody2D aliveRb;

    private Animator aliveAnim;
    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();
        currentHealth = maxHealth;

        facingDirection = 1;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateHurtState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;

        }
    }

    // WAlking State ------------------------------------------------------------------------------

    private void EnterWalkingState()
    {
        
    }

    private void UpdateWalkingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        CheckTouchDamage();
        if(!groundDetected || wallDetected)
        {
            //Flip
            Flip();
        }
        else
        {
            //movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.AddForce(new Vector2(movementSpeed * -facingDirection * Time.timeScale, aliveRb.velocity.y));
            aliveRb.velocity = movement;
        }
    }

    private void ExitWalkingState()
    {

    }

    // Hurt State ------------------------------------------------------------------------------

    private void EnterHurtState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);

        // fix aliveRB.velocity - after attack it decreases to 0
        Debug.Log("KnockbackState entered");
        
        Debug.Log("AliveRbVelocity" + aliveRb.velocity.x);
    }

    private void UpdateHurtState()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }

    private void ExitHurtState()
    {
        aliveAnim.SetBool("Knockback", false);
        
        Debug.Log("KnockbackState exit");
        Debug.Log("KnockbackSpeed " + knockbackSpeed + " / Movement " + movement + "Movement Speed " + movementSpeed);
        Debug.Log("AliveRbVelocity" + aliveRb.velocity.x);
    }


    // Dead State ------------------------------------------------------------------------------
    private void EnterDeadState()
    {
        Destroy(gameObject);
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }



    //--Other Funcitons ---------------------------------


    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //particles

        if(currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);

        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }
    private void CheckTouchDamage()
    {
        if(Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);

            if(hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }
    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitHurtState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }



        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterHurtState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 botLeft = new Vector2 (touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}