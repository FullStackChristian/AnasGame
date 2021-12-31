using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{

    [SerializeField]
    public bool combatEnabled;
    [SerializeField]
    private float inputTimer;

    public Transform firePoint;

    public GameObject firebulletPrefab;

    private Animator anim;

    private Rigidbody2D rb;


    private bool gotInput, isAttacking, isSecondAttack;
    private float lastInputTime = Mathf.NegativeInfinity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //to enable Special attack make variable public and enable once needed (when Tim get's handed three cameras!)
        //combatEnabled = true;
        
        //anim.SetBool("canAttack2", combatEnabled);
    }


    // Update is called once per frame
    void Update()
    {
        CheckCombatInput2();
        CheckAttack2();
        if (combatEnabled.Equals(true))
        {
            anim.SetBool("canAttack2", combatEnabled);
        }
    }

    private void CheckCombatInput2()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (combatEnabled)
            {
                //Attempt Comabt
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttack2()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                rb.velocity = new Vector2(0.0f, 0.0f);
                gotInput = false;
                isAttacking = true;
                isSecondAttack = !isSecondAttack;
                anim.SetBool("attack2", true);
                anim.SetBool("secondAttack", isSecondAttack);
                anim.SetBool("isAttacking", isAttacking);
               //Set canAttack to false so you can't use another attack while already attacking.
                anim.SetBool("canAttack", false);
                anim.SetBool("isWalking", false);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new Input
            gotInput = false;
        }
    }

    private void Shoot()
    {
        Instantiate(firebulletPrefab, firePoint.position, firePoint.rotation);
    }
    private void FinishAttack2()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack2", false);
        anim.SetBool("canAttack", combatEnabled);
    }
}
