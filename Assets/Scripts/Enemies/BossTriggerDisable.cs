using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class BossTriggerDisable : MonoBehaviour
{
    public GameObject boss;

    private void Start()
    {
        try
        {
            boss = GameObject.Find("BossFight").GetComponent<GameObject>();
        }
        catch
        {
            Debug.Log("Boss not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player has entered the trigger");
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            disableBossOnTrigger(false);
            Debug.Log("Player is in Range");
        }
    }

    public void disableBossOnTrigger(bool disabled)
    {
        if (boss)
        {
            boss.gameObject.SetActive(disabled);
        }
        else
        {
            Debug.Log("BossObject not found");
        }
        
    }
}
