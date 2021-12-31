using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    public int attackDamage = 20;

    public Vector3 attackOffset;
    
    private Vector3 pos;

    [SerializeField]
    private Transform attack1HitBoxPos;
    public float attackRange = 1f;
    private float currentHealth;
    public LayerMask whatIsPlayer;
    Rigidbody2D rb;
    BossStats bs;

    private float[] attackDetails = new float[2];

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bs = GetComponent<BossStats>();
    }
    public void Attack()
    {
        pos = attack1HitBoxPos.position;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, whatIsPlayer);
        if (colInfo != null)
        {
            attackDetails[0] = attackDamage;
            attackDetails[1] = rb.transform.position.x;
            colInfo.SendMessage("Damage", attackDetails);
        }
    }

    
    private void Damage(float[] attackDetails)
    {
        Debug.Log("Boss has been damaged");
        bs.DecreaseHealth(attackDetails[0]);

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attackRange);
    }
}
