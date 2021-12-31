using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public float speed = 40.0f;
    public Rigidbody2D rb;

    private float[] attackDetails = new float[2];

    [SerializeField]
    private float attack2Damage;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        attackDetails[0] = attack2Damage;
        attackDetails[1] = transform.position.x;

        try
        {
            collision.transform.SendMessage("Damage", attackDetails);
        }
        catch
        {
            Debug.Log("You didn't hit anything!");
        }

        Destroy(gameObject);
    }


}
