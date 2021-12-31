using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy the enemy - this could also be changed to have Ana with a collider underneath her 
            Destroy(collision.otherCollider.gameObject.transform.parent.gameObject);

            //Death animation?

        }
    }
}
