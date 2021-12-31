using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public AudioClip soundEffect;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        
        if (stats)
        {
            bool pickedUp = stats.PickUpItem(gameObject);
            if (pickedUp)
            {
                RemoveItem();
            }
        }
    }
    private void RemoveItem()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        //Instantiate(pickupEffect, transform.position, Quaternion.identity);
        
    }
}
