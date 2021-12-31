using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class Boss2AttackBehaviour : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float rangedAttackRange = 10f;
    private int[] attackChoice = { 1,2,3};
    System.Random random = new System.Random();

    public float coolDown = 5;
    public float coolDownTimer;

    Transform player;
    Rigidbody2D rb;
    Boss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER).transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);


        //Check for the distance between boss and player and decide when to attack the player.
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            // use random number to decide wether attack 1 or attack 2 or attack 3
            int index = random.Next(1, 3);
            Debug.Log("Random attack number: " + index);
           
            animator.SetTrigger("Attack" + index);

        }
        else if (Vector2.Distance(player.position, rb.position) <= rangedAttackRange || Vector2.Distance(player.position, rb.position) >= attackRange)
        {
            if (animator.GetBool("Attack3").Equals(false) && coolDownTimer.Equals(0))
            {
                animator.SetBool("Attack3", true);
                coolDownTimer = coolDown;
                Debug.Log("CoolDownTimer has been activated. Cooldown= " + coolDownTimer);
            }
            if (coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }
            if (coolDownTimer < 0)
            {
                coolDownTimer = 0;
            }

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack1");
        
        
        if(coolDownTimer == 0)
        {
            animator.SetBool("Attack3", false);
        }
    }

}
