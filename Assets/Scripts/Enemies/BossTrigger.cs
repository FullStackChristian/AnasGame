using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class BossTrigger : MonoBehaviour
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
            enableBossOnTrigger(true);
            Debug.Log("Player is in Range");
        }
    }

    public void enableBossOnTrigger(bool enabled)
    {
        if (boss)
        {
            boss.gameObject.SetActive(enabled);
        }
        else
        {
            Debug.Log("BossObject not found");
        }
    }
}
